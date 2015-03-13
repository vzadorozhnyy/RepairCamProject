using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web.UI;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxClasses;

public partial class Dialogs_WebDialogManager : UserControl {
    public delegate void CallbackHandler(object sender, DialogCallbackEventArgs e);

    private static readonly Dictionary<string, string> _dialogs = new Dictionary<string, string> {
        {"AddOrderDialog", "~/Dialogs/AddOrderDialog.ascx"},
        {"AddCustomerDialog", "~/Dialogs/AddCustomerDialog.ascx"},
    };

    private static readonly Dictionary<string, string> _clientDialogs = new Dictionary<string, string> {
        {"AddOrderDialog", "_addOrderDialog"},
        {"AddCustomerDialog", "_addCustomerDialog"},
    };

    public new PageEx Page {
        get {
            Debug.Assert(base.Page is PageEx);
            return base.Page as PageEx;
        }
    }

    protected void Page_Load(object sender, EventArgs e) {}

    protected void _webDialogsManagerCallbackPanel_OnCallback(object sender, CallbackEventArgsBase e) {
        IDictionary<string, object> lDeserialize = WebUtils.Deserialize(e.Parameter);
        if (lDeserialize != null && lDeserialize.ContainsKey("action") && lDeserialize["action"].ToString() == "closeDialog")
            CloseDialog(sender, lDeserialize);
        else if (lDeserialize != null && lDeserialize.ContainsKey("dialog"))
            CreateDialog(lDeserialize, false);
    }

    private void CreateDialog(IDictionary<string, object> deserialize, bool isFirstCalbackPanel) {
        ASPxCallbackPanel lPanel = isFirstCalbackPanel ? _webDialogsManagerCallbackPanel : _webDialogsManagerCallbackPanelIn;
        string lDialogName = deserialize["dialog"].ToString();
        string path = _dialogs[lDialogName];
        ViewUserDialog lDialog = Page.LoadControl(path) as ViewUserDialog;
        lDialog.ID = lDialogName;
        lDialog.ObjectId = Guid.NewGuid();
        lPanel.Controls.Add(lDialog);
        Session[lDialogName] = lDialog;
        lPanel.JSProperties["cpDialogs"] = _clientDialogs[lDialogName];
    }

    protected void _webDialogsManagerCallbackPanelIn_OnCallback(object sender, CallbackEventArgsBase e) {
        IDictionary<string, object> lDeserialize = WebUtils.Deserialize(e.Parameter);
        if (lDeserialize != null && lDeserialize.ContainsKey("action") && lDeserialize["action"].ToString() == "closeDialog")
            CloseDialog(sender, lDeserialize);
        else if (lDeserialize != null && lDeserialize.ContainsKey("dialog"))
            CreateDialog(lDeserialize, false);
    }

    private void CloseDialog(object sender, IDictionary<string, object> deserialize) {
        string lDialogName = deserialize["dialog"].ToString();
        EDialogName lName = (EDialogName) Enum.Parse(typeof (EDialogName), lDialogName);
        Dictionary<string, object> arr = deserialize["data"] as Dictionary<string, object>;
        if (CallbackEx != null)
            CallbackEx(sender, new DialogCallbackEventArgs(arr, lName));
    }

    public event CallbackHandler CallbackEx;
}

public enum EDialogName {
    AddOrderDialog,
    AddCustomerDialog
}

public class DialogCallbackEventArgs : EventArgs {
    public DialogCallbackEventArgs(Dictionary<string, object> arr, EDialogName name) {
        Data = arr;
        DialogName = name;
    }

    public Dictionary<string, object> Data { get; set; }
    public EDialogName DialogName { get; set; }
}