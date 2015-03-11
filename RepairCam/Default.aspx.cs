using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataModel;

public partial class _Default : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
            string lConnectionString = "Provider=SQLOLEDB; user id=sa;" +
                        "password=1;server=localhost\\SQLEXPRESS;" +
                        "Trusted_Connection=yes;" +
                        "database=RepairCamDB; " +
                        "connection timeout=30";
            DataManager.Inst.Init(lConnectionString);
        }
		
    }