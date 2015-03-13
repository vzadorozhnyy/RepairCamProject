using System;
using System.Diagnostics;
using System.Web.UI;
using DevExpress.Web.ASPxPopupControl;

/// <summary>
///     Summary description for ViewUserDialog
/// </summary>
public abstract class ViewUserDialog : UserControl {
    public enum EDialog {
        Min = -1,
        AddOrderDialog,
        AddCustomerDialog,
        Max //not used,must be always last item
    }

    public enum EDialogMode {
        None,
        Add,
        Edit,
    }

    public enum EDialogResult {
        Ok,
        Cancel,
        Apply
    }

    protected abstract ASPxPopupControl ASPxPopupControl { get; }
    public EDialog DialogName { get; set; }
    public Guid ObjectId { get; set; }
    public EDialogResult DialogResult { get; protected set; }

    public new PageEx Page {
        get {
            Debug.Assert(base.Page is PageEx);
            return base.Page as PageEx;
        }
    }
}