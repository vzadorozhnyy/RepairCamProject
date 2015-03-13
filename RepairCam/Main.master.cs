using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ASP;

public partial class MainMaster : System.Web.UI.MasterPage {
        protected void Page_Load(object sender, EventArgs e) {

        }

        public WebDialogManager DialogManager
        {
            get { return (Master as RootMaster).DialogManager; }
        }
    }