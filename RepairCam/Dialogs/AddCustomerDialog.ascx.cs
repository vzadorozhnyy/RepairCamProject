using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxClasses;
using DevExpress.Web.ASPxPopupControl;

public partial class Dialogs_AddCustomerDialog : ViewUserDialog
{
    protected override void OnLoad(EventArgs e) {
        base.OnLoad(e);
        Page.CloseDialog += PageOnCloseDialog;
    }

    private void PageOnCloseDialog(string dialogName, EventArgs eventArgs) {
    }

    protected override ASPxPopupControl ASPxPopupControl {
        get { return _addCustomerDialog; }
    }

    protected void _addCustomerDialogPanel_OnCallback(object sender, CallbackEventArgsBase e) {
    }
}