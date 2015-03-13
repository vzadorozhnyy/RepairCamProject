<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AddOrderDialog.ascx.cs" Inherits="Dialogs_AddOrderDialog" %>

<script type="text/javascript">

</script>
<dx:ASPxPopupControl ID="_addOrderDialog" runat="server" CloseAction="CloseButton" Modal="True" ShowOnPageLoad="True"
                     PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="_addOrderDialog"
                     HeaderText="Add Order" AllowDragging="True" PopupAnimationType="None" EnableViewState="False" OnWindowCallback="_addOrderDialog_OnWindowCallback">
    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
            <table style="width: 100%" cellspacing="4" cellpadding="4" class="popupTableClass">
                <tr>
                    <td>
                       <dx:ASPxLabel runat="server" Text="Customer:" ID="ASPxLabel4"/>
                    </td>
                    <td>
                        <table style="border: rgba(23, 157, 65, 0.74) 1px; border-style: solid; width: 100%;">
                            <tr>
                                <td>
                                    <dx:ASPxRadioButton runat="server" ID="_selectCustomerRadio" ClientInstanceName="_selectCustomerRadio" GroupName="radio" Text="Select existing customer:" Checked="True">
                                        <ClientSideEvents Init="function(s,e){refreshState();}" CheckedChanged="function(s,e){refreshState();}"></ClientSideEvents>
                                    </dx:ASPxRadioButton>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table style="margin-left: 50px;">
                                        <tr>
                                            <td style="width: 1px;">
                                                <dx:ASPxLabel runat="server" Text="Customer:" ID="lbl1"/>
                                            </td>
                                            <td>
                                                <dx:ASPxComboBox DropDownStyle="DropDownList" Width="216px" runat="server" ID="_customerCombo" ClientInstanceName="_customerCombo" AutoPostBack="False">
                                                    <%--                            <ClientSideEvents ButtonClick="function(s,e){createDialog('AddCustomerDialog','add', false);}"></ClientSideEvents>--%>
                                                </dx:ASPxComboBox>

                                            </td>

                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <dx:ASPxRadioButton runat="server" ID="_newCustomerRadio" ClientInstanceName="_newCustomerRadio" GroupName="radio" Text="Or create new customer:"></dx:ASPxRadioButton>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table style="margin-left: 65px; width: 265px;">
                                        <tr>
                                            <td style="width: 1px; text-align: right;">
                                                <dx:ASPxLabel runat="server" Text="Name:" ID="ASPxLabel1"/>
                                            </td>
                                            <td>
                                                <dx:ASPxTextBox runat="server" ID="_nameTxt" ClientInstanceName="_nameTxt" AutoPostBack="False" Width="100%" EnableViewState="True">
                                                    	    <ValidationSettings ValidationGroup="DailogValidationGroup">
	        <RequiredField ErrorText="Name is required." IsRequired="true" />
	    </ValidationSettings>
                                                </dx:ASPxTextBox>

                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right;">
                                                <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="E-mail:"/>
                                            </td>
                                            <td>
                                                <dx:ASPxTextBox runat="server" ID="_emailTxt" ClientInstanceName="_emailTxt" AutoPostBack="False" Width="100%">
	        <ValidationSettings ValidationGroup="DailogValidationGroup">
	            <RequiredField ErrorText="E-mail is required." IsRequired="true" />
	            <RegularExpression ErrorText="-Email validation failed" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
	        </ValidationSettings>
                                                </dx:ASPxTextBox>
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
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right; width: 47px;">
                        <dx:ASPxLabel ID="lbl2" runat="server" Text="Car Info:"/>
                    </td>
                    <td>
                        <dx:ASPxTextBox runat="server" ID="_carInfoTxt" ClientInstanceName="_carInfoTxt" AutoPostBack="False" Width="100%"></dx:ASPxTextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        <dx:ASPxLabel runat="server" ID="lbl3" Text="Detail:"/>
                    </td>
                    <td>
                        <dx:ASPxMemo runat="server" ID="_detailsTxt" ClientInstanceName="_detailsTxt" AutoPostBack="False" Height="80px" Width="100%"></dx:ASPxMemo>
                    </td>
                </tr>

            </table>
            <table style="float: right; margin-bottom: 5px; margin-top: 5px;">
                <tr>
                    <td align="right">
                        <div style="float: right; margin-left: 2px;">
                            <dx:ASPxButton Width="76px" runat="server" ID="_addOrderCancelBtn" ClientInstanceName="_addOrderCancelBtn" Text="Cancel" AutoPostBack="False">
                                <ClientSideEvents Click="function(s,e){
_addOrderDialog.Hide();                        }">
                                </ClientSideEvents>
                            </dx:ASPxButton>
                        </div>

                        <div style="float: right; margin-left: 2px;">
                            <dx:ASPxButton Width="76px" runat="server" ID="_addOrderOklBtn" ClientInstanceName="_addOrderOklBtn" Text="Ok" AutoPostBack="False">
                                <ClientSideEvents Click="function(s,e){
var arr = {};
                                    arr['NewCustomer'] = _newCustomerRadio.GetChecked();
                                    arr['Name'] = _nameTxt.GetText();
                                    arr['Email'] = _emailTxt.GetText();
                                    arr['Phone'] = _phoneTxt.GetText();
                                    arr['CarInfo'] = _carInfoTxt.GetText();
                                    arr['Details'] = _detailsTxt.GetText();
                                    var item = _customerCombo.GetSelectedItem();
                                    arr['CustomerId'] = item ? item.value : '';

                                                                            var json = { action : 'closeDialog', dialog: 'AddCustomerDialog', data : arr};
    json = JSON.stringify(json);

                                                                        $DMCP.PerformCallback(json)
                        _addOrderDialog.Hide();
                                    _grid.Refresh();
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

