<%@ Control Language="C#" AutoEventWireup="true" CodeFile="WebDialogManager.ascx.cs" Inherits="Dialogs_WebDialogManager" ClassName="WebDialogManager" %>
<dx:ASPxCallbackPanel EnableViewState="False" ID="_webDialogsManagerCallbackPanel" runat="server" ClientInstanceName="$DMCP" OnCallback="_webDialogsManagerCallbackPanel_OnCallback" Width="0px" Height="0px">
    <ClientSideEvents EndCallback="function(s,e){webDialogsManagerCallback(s);renderScripts(s);}"></ClientSideEvents>
    <PanelCollection>
        <dx:PanelContent EnableViewState="False">
            <dx:ASPxCallbackPanel EnableViewState="False" ID="_webDialogsManagerCallbackPanelIn" runat="server" ClientInstanceName="$DMCP2" OnCallback="_webDialogsManagerCallbackPanelIn_OnCallback">
                <ClientSideEvents EndCallback="function(s,e){webDialogsManagerCallback(s);renderScripts(s);}"></ClientSideEvents>
                <PanelCollection>
                    <dx:PanelContent EnableViewState="False">
                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxCallbackPanel>

        </dx:PanelContent>
    </PanelCollection>
</dx:ASPxCallbackPanel>