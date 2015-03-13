using System;
using System.Collections.Generic;
using ASP;
using DataModel;
using DataModel.Objects;
using DevExpress.Web.ASPxCallbackPanel;

public partial class _Default : PageEx {
    protected override void OnLoad(EventArgs e) {
        base.OnLoad(e);
        _grid.DataBind();
    }

    protected override void OnInit(EventArgs e) {
        base.OnInit(e);
        WebDialogManager lDialogManager = Page.Master.Master.FindControl("WebDialogManager") as WebDialogManager;
        if (lDialogManager != null)
            lDialogManager.CallbackEx += DialogManagerOnCallbackEx;
    }

    private void DialogManagerOnCallbackEx(object sender, DialogCallbackEventArgs e) {
        switch (e.DialogName) {
            case EDialogName.AddCustomerDialog:
                SaveCustomer(e.Data, sender);
                break;
            case EDialogName.AddOrderDialog:
                break;
        }
    }

    private void SaveCustomer(Dictionary<string, object> data, object sender) {
        bool lNew;
        Customer lCustomer;
        User lLoggedUser = DataManagerLocal.Inst.LoggedUser;
        string lCustomerId = data["CustomerId"].ToString();
        if (string.IsNullOrEmpty(lCustomerId))
            lCustomerId = Guid.Empty.ToString();
        if (!bool.TryParse(data["NewCustomer"].ToString(), out lNew))
            return;
        if (lNew) {
            lCustomer = new Customer(lLoggedUser);
            lCustomer.Email = data["Email"].ToString();
            lCustomer.Name = data["Name"].ToString();
            lCustomer.Phone = data["Phone"].ToString();
            DataManager.Inst.SaveCustomer(lCustomer);
        } else
            lCustomer = lLoggedUser.GetCustomer(new Guid(lCustomerId));

        if (lCustomer == null)
            return;

        Order lOrder = new Order(lCustomer);
        lOrder.Details = data["Details"].ToString();
        lOrder.Name = data["CarInfo"].ToString();
        lOrder.OrderDate = DateTime.Now;
        DataManager.Inst.SaveOrder(lOrder);
        (sender as ASPxCallbackPanel).JSProperties["cpResulrScript"] = "_grid.Refresh();";
    }

    protected void _grid_OnDataBinding(object sender, EventArgs e) {
        _grid.DataSource = DataManager.Inst.GetOrders(DataManagerLocal.Inst.LoggedUser, EOrderStatus.New);
    }
}