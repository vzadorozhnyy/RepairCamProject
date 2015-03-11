using System;
using System.Web.UI;
using DataModel;

public partial class _Default : Page {
    protected void Page_Load(object sender, EventArgs e) {
        string lConnectionString = "Provider=SQLOLEDB; user id=sa;" +
                                   "password=1;server=localhost\\SQLEXPRESS;" +
                                   "Trusted_Connection=yes;" +
                                   "database=RepairCamDB; " +
                                   "connection timeout=30";
        DataManager.Inst.Init(lConnectionString);
    }
}