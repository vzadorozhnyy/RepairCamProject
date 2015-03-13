using System;
using System.Web.Security;
using System.Web.UI;
using DataModel;
using DataModel.DB;
using DataModel.Objects;

public partial class Login : PageEx {
    protected void Page_Load(object sender, EventArgs e) {}

    protected void btnLogin_Click(object sender, EventArgs e) {
        User lUser = DataManager.Inst.GetUserByLogin(tbLogin.Text);
        string lEncrypt = Cryptograph.Encrypt(tbPassword.Text);
        if (DataManager.Inst.CanUserLogin(tbLogin.Text, lEncrypt)) {
            DataManagerLocal.Inst.LoggedUser = lUser;
            if (string.IsNullOrEmpty(Request.QueryString["ReturnUrl"])) {
                FormsAuthentication.SetAuthCookie(tbLogin.Text, false);
                Response.Redirect("~/");
            } else
                FormsAuthentication.RedirectFromLoginPage(tbLogin.Text, false);
        } else {
            tbLogin.ErrorText = "Invalid user";
            tbLogin.IsValid = false;
        }
    }
}