using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataModel;
using DataModel.Objects;

public partial class _Default : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
            User lLoggedUser = DataManagerLocal.Inst.LoggedUser;
        }
		
    }