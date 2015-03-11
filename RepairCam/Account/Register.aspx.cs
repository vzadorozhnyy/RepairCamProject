using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using DataModel;
using DataModel.DB;
using DataModel.Objects;

public partial class Register : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
            
        }

        protected void btnCreateUser_Click(object sender, EventArgs e) {
            try {
                if (DataManager.Inst.GetUserByLogin(tbLogin.Text) == null)
                {
                    User lUser = new User();
                    lUser.Email = tbEmail.Text;
                    lUser.Login = tbLogin.Text;
                    lUser.Name = tbCompanyName.Text;
                    lUser.Pwd = Cryptograph.Encrypt(tbPassword.Text);
                    lUser.RegistrationDate = DateTime.Now;
                    DataManager.Inst.SaveUser(lUser);
                    Response.Redirect(Request.QueryString["ReturnUrl"] ?? "~/Account/RegisterSuccess.aspx");
                } else {
                    tbLogin.ErrorText = "You can not register";
                    tbLogin.IsValid = false;
                }
                //MembershipUser user = Membership.CreateUser(tbUserName.Text, tbPassword.Text, tbEmail.Text);
            }
            catch (MembershipCreateUserException exc) {
                if (exc.StatusCode == MembershipCreateStatus.DuplicateEmail || exc.StatusCode == MembershipCreateStatus.InvalidEmail) {
                    tbEmail.ErrorText = exc.Message;
                    tbEmail.IsValid = false;
                }
                else if (exc.StatusCode == MembershipCreateStatus.InvalidPassword) {
                    tbPassword.ErrorText = exc.Message;
                    tbPassword.IsValid = false;
                }
                else {
                    tbCompanyName.ErrorText = exc.Message;
                    tbCompanyName.IsValid = false;
                }
            }
        }
    }