using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using ZM.iChooChoo.Library;
using ZM.iChooChoo.Library.Modules;

namespace ZM.iChooChoo.Client.Web.settings
{
    public partial class moduletest21 : System.Web.UI.Page
    {
        protected IccClient _m;
        protected byte _bAddress;
        protected IModule _module;
        protected string[] _status;

        protected void Page_Load(object sender, EventArgs e)
        {
            (Master as Principal).PageTitle = "Module Test : 0x21";

            litErrorGeneric.Visible = false;

            _m = Application["icc"] as IccClient;

            _bAddress = 0;
            if (byte.TryParse(Request.QueryString["addr"], out _bAddress))
            {
                if (Module.IsAddressValid(_bAddress))
                {
                    _module = _m.Modules[_bAddress] as LightingModule;
                    if (_module == null)
                        Response.Redirect("~/settings");
                    if (!Page.IsPostBack)
                        RefreshTable();
                }
                else
                    Response.Redirect("~/settings");
            }
            else
                Response.Redirect("~/settings");

        }

        private void RefreshTable()
        {
            litTitle.Text = string.Format("0x{0:X2} ({1}): {2}", _module.ID, ICCConstants.GetTypeDescription((byte)_module.Type), _module.Description);

            _status = _m.GET_STATUS(_bAddress);

            rpt.DataSource = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            rpt.DataBind();
            rptDim.DataSource = new int[] { 10, 11, 12, 13, 14, 15 };
            rptDim.DataBind();
        }

        protected void rpt_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int i = (int)e.Item.DataItem;

                var litOutID = e.Item.FindControl("litOutID") as Literal;
                var lblStatus = e.Item.FindControl("lblStatus") as Label;
                var btnToggle = e.Item.FindControl("btnToggle") as Button;

                litOutID.Text = i.ToString("X");
                lblStatus.CssClass = (_status[i] == "1" ? "status_on" : "status_off");
                lblStatus.Text = (_status[i] == "1" ? "ON" : "OFF");
                btnToggle.Text = string.Format("Set to {0} - Output {1:X}", (_status[i] == "1" ? "OFF" : "ON "), i);
                btnToggle.CommandName = (_status[i] == "1" ? "OFF" : "ON");
                btnToggle.CommandArgument = i.ToString();
            }
        }

        protected void rptDim_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int i = (int)e.Item.DataItem;
                int iValue = int.Parse(_status[i]);

                string sScript = string.Empty;
                sScript += "<script>\n";
                sScript += "$( function() {{\n";
                sScript += "var handle = $(\"#custom-handle{0:X}\");\n";
                sScript += "var hidden = $(\"#value{0:X}\");\n";
                sScript += "$(\"#slider{0:X}\").slider({{\n";
                sScript += "min: 0, max: 255, value: {1}, \n";
                sScript += "create: function() {{ handle.text($(this).slider(\"value\")); hidden.attr(\"value\", $(this).slider(\"value\")); }},\n";
                sScript += "slide: function(event, ui) {{ handle.text(ui.value); hidden.attr(\"value\", ui.value); }}\n";
                sScript += "}});\n";
                sScript += "}});\n";
                sScript += "</script>\n";

                var litScript = e.Item.FindControl("litScript") as Literal;
                var litOutID = e.Item.FindControl("litOutID") as Literal;
                var btnToggle = e.Item.FindControl("btnToggle") as Button;

                litScript.Text = string.Format(sScript, i, iValue);
                litOutID.Text = i.ToString("X");
                //lblStatus.CssClass = (_status[i] == "1" ? "status_on" : "status_off");
                //lblStatus.Text = (_status[i] == "1" ? "ON" : "OFF");
                //btnToggle.Text = string.Format("Set to {0} - Output {1:X}", (_status[i] == "1" ? "OFF" : "ON "), i);
                btnToggle.CommandName = string.Format("{0:X}", i);
                //btnToggle.CommandArgument = i.ToString();
            }
        }

        protected void rpt_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "ON")
            {
                byte bOutput = byte.Parse(e.CommandArgument.ToString());
                var mod = _m.Modules[_bAddress] as LightingModule;
                mod.setOutput(bOutput, true);
                if (_m.LastResult)
                    RefreshTable();
                else
                    litErrorGeneric.Visible = true;
            }
            else if (e.CommandName == "OFF")
            {
                byte bOutput = byte.Parse(e.CommandArgument.ToString());
                var mod = _m.Modules[_bAddress] as LightingModule;
                mod.setOutput(bOutput, false);
                if (_m.LastResult)
                    RefreshTable();
                else
                    litErrorGeneric.Visible = true;
            }
        }

        protected void rptDim_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            byte bOutput = byte.Parse(e.CommandName, NumberStyles.HexNumber);
            byte bValue = byte.Parse(Request.Form["value" + e.CommandName]);
            var mod = _m.Modules[_bAddress] as LightingModule;
            mod.setDimmableOutput(bOutput, bValue);
            if (_m.LastResult)
                RefreshTable();
            else
                litErrorGeneric.Visible = true;
        }
    }
}