using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using ZM.iChooChoo.Library;
using ZM.iChooChoo.Library.Modules;

namespace ZM.iChooChoo.Client.Web.settings
{
    public partial class moduletest20 : System.Web.UI.Page
    {
        protected IccClient _m;
        protected byte _bAddress;
        protected IModule _module;
        protected string[] _status;

        protected void Page_Load(object sender, EventArgs e)
        {
            (Master as Principal).PageTitle = "Module Test : 0x20";

            litErrorGeneric.Visible = false;

            _m = Application["icc"] as IccClient;

            _bAddress = 0;
            if (byte.TryParse(Request.QueryString["addr"], out _bAddress))
            {
                if (Module.IsAddressValid(_bAddress))
                {
                    _module = _m.Modules[_bAddress] as GenPurpModule;
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

            rpt.DataSource = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            rpt.DataBind();
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

        protected void rpt_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "ON")
            {
                byte bOutput = byte.Parse(e.CommandArgument.ToString());
                var mod = _m.Modules[_bAddress] as GenPurpModule;
                mod.setOutput(bOutput, true);
                if (_m.LastResult)
                    RefreshTable();
                else
                    litErrorGeneric.Visible = true;
            }
            else if (e.CommandName == "OFF")
            {
                byte bOutput = byte.Parse(e.CommandArgument.ToString());
                var mod = _m.Modules[_bAddress] as GenPurpModule;
                mod.setOutput(bOutput, false);
                if (_m.LastResult)
                    RefreshTable();
                else
                    litErrorGeneric.Visible = true;
            }
        }
    }
}
