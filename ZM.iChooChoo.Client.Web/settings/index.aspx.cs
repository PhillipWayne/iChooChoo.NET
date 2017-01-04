using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using ZM.iChooChoo.Library;
using ZM.iChooChoo.Library.Modules;

namespace ZM.iChooChoo.Client.Web.settings
{
    public partial class index : System.Web.UI.Page
    {
        protected IccClient _m;
        protected List<IModule> _modules;

        protected void Page_Load(object sender, EventArgs e)
        {
            (Master as Principal).PageTitle = "Settings";

            litErrorAddressType.Visible = false;
            litErrorDescription.Visible = false;
            litErrorGeneric.Visible = false;

            _m = Application["icc"] as IccClient;

            if (!Page.IsPostBack)
            {
                RefreshTable();
            }
        }

        protected void RefreshTable()
        {
            _modules = _m.Modules.Values.ToList();

            rpt.DataSource = _modules.Where(x => x.ID != ICCConstants.ADDR_MAX).ToList();
            rpt.DataBind();

            var modNew = _modules.Where(x => x.Type == ICCConstants.BICCP_GRP_NEW).SingleOrDefault();
            plhNewModule.Visible = (modNew != null);
            if (modNew != null)
            {
                plhNewModule.Visible = true;

                lstNewAddress.Items.Clear();
                for (int i = ICCConstants.ADDR_MIN; i <= ICCConstants.ADDR_MAX; i++)
                {
                    if (!_modules.Select(x => x.ID).Contains(i))
                        lstNewAddress.Items.Add(new ListItem(string.Format("0x{0:X2}", i), i.ToString()));
                }
                lstNewAddress.Items.Add(new ListItem("0x77", "119"));
                lstNewAddress.SelectedIndex = lstNewAddress.Items.Count - 1;

                lstNewType.Items.Clear();
                foreach (var t in ICCConstants.Types.Where(x => x.Key != ICCConstants.BICCP_GRP_UNKNOWN).ToList())
                    lstNewType.Items.Add(new ListItem(string.Format("0x{0:X2} - {1}", t.Key, t.Value), t.Key.ToString()));
                lstNewType.SelectedIndex = 0;

                litNewVersion.Text = modNew.Version;
            }
        }

        protected void btnRescan_Click(object sender, EventArgs e)
        {
            _m.RefreshBus();
            litErrorGeneric.Visible = !_m.LastResult;
            RefreshTable();
        }

        protected void rpt_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var module = e.Item.DataItem as Module;
                if (module.Type == ICCConstants.BICCP_GRP_UNKNOWN)
                {
                    var litVersion = e.Item.FindControl("litVersion") as Literal;
                    litVersion.Text = "";
                    var txtDescription = e.Item.FindControl("txtDescription") as TextBox;
                    txtDescription.Visible = false;
                    var tr = e.Item.FindControl("tr") as HtmlTableRow;
                    tr.Attributes.Add("class", "invalid");
                    var btnSave = e.Item.FindControl("btnSave") as Button;
                    btnSave.Visible = false;
                    var hypTest = e.Item.FindControl("hypTest") as HyperLink;
                    hypTest.Visible = false;
                    var btnHardReset = e.Item.FindControl("btnHardReset") as Button;
                    btnHardReset.Visible = false;
                    var btnSoftReset = e.Item.FindControl("btnSoftReset") as Button;
                    btnSoftReset.Visible = false;
                }
            }
        }

        protected void rpt_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "SV")
            {
                int iAddress = int.Parse(e.CommandArgument.ToString());
                var txtDescription = e.Item.FindControl("txtDescription") as TextBox;
                var sDescription = txtDescription.Text.Trim();

                if (sDescription.Length > 0 && sDescription.Length <= ICCConstants.DESCSIZE && sDescription.IndexOf(' ') == -1)
                {
                    var mod = _m.Modules[iAddress] as Module;
                    if (mod != null)
                    {
                        mod.SetDescription(sDescription);
                        if (_m.LastResult)
                            _m.RefreshBus();
                        else
                            litErrorGeneric.Visible = true;
                    }
                    else
                        litErrorGeneric.Visible = true;
                }
                else
                    litErrorDescription.Visible = true;
            }
            else if (e.CommandName == "SR")
            {
                int i = int.Parse(e.CommandArgument.ToString());
                var mod = _m.Modules[i] as Module;
                if (mod != null)
                {
                    mod.SoftReset();
                    if (_m.LastResult)
                    {
                        Thread.Sleep(4000);
                        _m.RefreshBus();
                    }
                    else
                        litErrorGeneric.Visible = true;
                }
                else
                    litErrorGeneric.Visible = true;
            }
            else if (e.CommandName == "HR")
            {
                int i = int.Parse(e.CommandArgument.ToString());
                var mod = _m.Modules[i] as Module;
                if (mod != null)
                {
                    mod.HardReset();
                    if (_m.LastResult)
                    {
                        Thread.Sleep(4000);
                        _m.RefreshBus();
                    }
                    else
                        litErrorGeneric.Visible = true;
                }
                else
                    litErrorGeneric.Visible = true;
            }

            RefreshTable();
        }

        protected void btnNewSave_Click(object sender, EventArgs e)
        {
            byte bAddress = byte.Parse(lstNewAddress.SelectedValue);
            byte bType = byte.Parse(lstNewType.SelectedValue);

            if (Module.IsAddressValid(bAddress) && bType != ICCConstants.BICCP_GRP_NEW)
            {
                var mod = _m.Modules[0x77] as Module;
                if (mod != null)
                {
                    mod.SetAddress(bAddress);
                    if (_m.LastResult)
                    {
                        mod.SetType(bType);
                        if (_m.LastResult)
                        {
                            mod.SoftReset();
                            if (_m.LastResult)
                            {
                                Thread.Sleep(4000);
                                _m.RefreshBus();
                            }
                            else
                                litErrorGeneric.Visible = true;
                        }
                        else
                            litErrorGeneric.Visible = true;
                    }
                    else
                        litErrorGeneric.Visible = true;
                }
                else
                    litErrorGeneric.Visible = true;
            }
            else
                litErrorAddressType.Visible = true;

            RefreshTable();
        }

        protected void btnNewSoftReset_Click(object sender, EventArgs e)
        {
            var mod = _m.Modules[0x77] as Module;
            mod.SoftReset();
            if (_m.LastResult)
            {
                _m.RefreshBus();
                if (_m.LastResult)
                    RefreshTable();
                else
                    litErrorGeneric.Visible = true;
            }
            else
                litErrorGeneric.Visible = true;
        }

        protected void btnNewHardReset_Click(object sender, EventArgs e)
        {
            var mod = _m.Modules[0x77] as Module;
            mod.HardReset();
            if (_m.LastResult)
            {
                if (_m.LastResult)
                    RefreshTable();
                else
                    litErrorGeneric.Visible = true;
            }
            else
                litErrorGeneric.Visible = true;
        }
    }
}