using System;
using System.Web.UI;
using ASP;

public partial class RootMaster : MasterPage {
    protected void Page_Load(object sender, EventArgs e) {
        ASPxLabel2.Text = DateTime.Now.Year + Server.HtmlDecode(" &copy; Copyright by [company name]");
    }

    public WebDialogManager DialogManager
    {
        get { return WebDialogManager; }
    }
}