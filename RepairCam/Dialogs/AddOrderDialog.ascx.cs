using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataModel;
using DevExpress.Web.ASPxClasses;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxPopupControl;

public partial class Dialogs_AddOrderDialog : ViewUserDialog
{
    protected override void OnInit(EventArgs e) {
        base.OnInit(e);
        Page.CloseDialog += PageOnCloseDialog;
    }

    private void PageOnCloseDialog(string dialogName, EventArgs eventArgs) {

    }

    protected override void OnLoad(EventArgs e) {
        base.OnLoad(e);
        Populate();
    }

    private void Populate() {
        _customerCombo.ValueField = "Id";
        _customerCombo.TextField = "Name";
        _customerCombo.DataSource = DataManagerLocal.Inst.LoggedUser.GetCustomers();
        _customerCombo.DataBind();
    }

    protected override ASPxPopupControl ASPxPopupControl {
        get { return _addOrderDialog; }
    }

    protected void _customerComboPanel_OnCallback(object sender, CallbackEventArgsBase e) {
        Populate();
    }

    protected void _addOrderDialog_OnWindowCallback(object source, PopupWindowCallbackArgs e) {
    }
}