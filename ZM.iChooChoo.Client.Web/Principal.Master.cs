using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZM.iChooChoo.Client.Web
{
    public partial class Principal : System.Web.UI.MasterPage
    {
        public virtual string PageTitle { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = string.Format("{0} - {1}", PageTitle, ConfigurationManager.AppSettings["AppTitle"]);
            hypRoot.ToolTip = ConfigurationManager.AppSettings["AppTitle"];
            hypRoot.Text = ConfigurationManager.AppSettings["AppTitle"];
        }
    }
}