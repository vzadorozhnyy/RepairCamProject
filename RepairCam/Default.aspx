<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Main.master" CodeFile="Default.aspx.cs" Inherits="_Default" %>
<%@ Reference Control="~/Dialogs/WebDialogManager.ascx" %>

<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">
    <%-- DXCOMMENT: Configure ASPxGridView control --%>
<dx:ASPxGridView ID="_grid" runat="server" AutoGenerateColumns="False" ClientInstanceName="_grid" EnableViewState="False" OnDataBinding="_grid_OnDataBinding" KeyFieldName="Id"
    Width="100%" PreviewFieldName="Details">
    <SettingsPager Visible="True" PageSize="20" />
    <Settings VerticalScrollBarMode="Auto" VerticalScrollableHeight="550" HorizontalScrollBarMode="Hidden" ShowPreview="True"/>
<SettingsPager PageSize="20" Position="Bottom">
                                                    <PageSizeItemSettings Items="5, 10, 20, 50, 100, 200" Visible="True">
                                                    </PageSizeItemSettings>
                                                </SettingsPager>    <Border BorderWidth="0px" />
    <BorderBottom BorderWidth="1px" />
    <Templates>
            <PreviewRow>
                <table>
                    <tr>
                        <td style="vertical-align: top">
                            <dx:ASPxLabel ID="lblNotes" runat="server" Text='<%# Container.Text %>' />
                        </td>
                    </tr>
                </table>
            </PreviewRow>
        </Templates>
	<%-- DXCOMMENT: Configure ASPxGridView's columns in accordance with datasource fields --%>
	<Columns>
        <dx:GridViewDataTextColumn FieldName="Customer.Name" VisibleIndex="1">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="Name" VisibleIndex="2">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="OrderDate" VisibleIndex="3">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="Status" VisibleIndex="4">
        </dx:GridViewDataTextColumn>
    </Columns>
</dx:ASPxGridView>


</asp:Content>