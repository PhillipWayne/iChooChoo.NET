using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZM.iChooChoo.Client.Web.settings
{
    public partial class logview : System.Web.UI.Page
    {
        protected IccClient _m;

        protected void Page_Load(object sender, EventArgs e)
        {
            (Master as Principal).PageTitle = "View server log";

            _m = Application["icc"] as IccClient;

#warning Implement log client
            //rpt.DataSource = _m.GetServerLog();
            //rpt.DataBind();
        }
    }
}