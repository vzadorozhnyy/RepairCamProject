<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AddCustomerDialog.ascx.cs" Inherits="Dialogs_AddCustomerDialog" EnableViewState="true"%>


            <dx:ASPxPopupControl ID="_addCustomerDialog" runat="server" CloseAction="CloseButton" Modal="True" ShowOnPageLoad="False"
                     PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="_addCustomerDialog"
                     HeaderText="Add Customer" AllowDragging="True" PopupAnimationType="None" EnableViewState="False">
    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
            <dx:ASPxCallbackPanel EnableViewState="False" ID="_addCustomerDialogPanel" runat="server" ClientInstanceName="_addCustomerDialogPanel" OnCallback="_addCustomerDialogPanel_OnCallback">
    <ClientSideEvents EndCallback="function(s,e){}"></ClientSideEvents>
    <PanelCollection>
        <dx:PanelContent EnableViewState="False">
        </dx:PanelContent>
    </PanelCollection>
</dx:ASPxCallbackPanel>

            <table>
                <tr>
                    <td style="width: 1px;">
                        <dx:ASPxLabel runat="server" Text="Name:" ID="lbl1"/>
                    </td>
                    <td>
                                                <dx:ASPxTextBox runat="server" ID="_nameTxt" ClientInstanceName="_nameTxt" AutoPostBack="False" Width="100%" EnableViewState="True"></dx:ASPxTextBox>

                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Email:"/>
                    </td>
                    <td>
                        <dx:ASPxTextBox runat="server" ID="_emailTxt" ClientInstanceName="_emailTxt" AutoPostBack="False" Width="100%"></dx:ASPxTextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        <dx:ASPxLabel runat="server" ID="ASPxLabel3" Text="Phone:"/>
                    </td>
                    <td>
                        <dx:ASPxTextBox runat="server" ID="_phoneTxt" ClientInstanceName="_phoneTxt" AutoPostBack="False" Width="100%"></dx:ASPxTextBox>
                    </td>
                </tr>

            </table>
            <table style="float: right; margin-bottom: 5px; margin-top: 5px;">
                <tr>
                    <td align="right">
                        <div style="float: right; margin-left: 2px;">
                            <dx:ASPxButton Width="76px" runat="server" ID="ASPxButton1" ClientInstanceName="_addCustomerCancelBtn" Text="Cancel" AutoPostBack="False">
                                <ClientSideEvents Click="function(s,e){
                        _addCustomerDialog.Hide();
                        }">
                                </ClientSideEvents>
                            </dx:ASPxButton>
                        </div>

                        <div style="float: right; margin-left: 2px;">
                            <dx:ASPxButton Width="76px" runat="server" ID="ASPxButton2" ClientInstanceName="_addCustomerOklBtn" Text="Ok" AutoPostBack="False">
                                <ClientSideEvents Click="function(s,e){debugger;
                                    var arr = {};
                                    arr['Name'] = _nameTxt.GetText();
                                    arr['Email'] = _emailTxt.GetText();
                                    arr['Phone'] = _phoneTxt.GetText();
                                                                            var json = { action : 'closeDialog', dialog: 'AddCustomerDialog', data : arr};
    json = JSON.stringify(json);

                                                                        $DMCP2.PerformCallback(json)

                        _addCustomerDialog.Hide();

                        }">
                                </ClientSideEvents>
                            </dx:ASPxButton>
                        </div>
                    </td>

                </tr>
            </table>
        </dx:PopupControlContentControl>
    </ContentCollection>
    <ContentStyle>
        <Paddings PaddingBottom="5px"/>
    </ContentStyle>
</dx:ASPxPopupControl>


