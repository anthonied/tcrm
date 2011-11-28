<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MessageBox.ascx.cs"
    Inherits="MessageBox" %>

<div class="container">
    <asp:Panel ID="panelMessageBox" runat="server">
        <asp:HyperLink runat="server" id="CloseButton" >
            <asp:Image runat="server" ImageUrl="~/images/close.png" AlternateText="Click here to close this message" />
        </asp:HyperLink>
        <p class="message">
            <asp:Literal ID="litMessage" runat="server"></asp:Literal></p>
    </asp:Panel>
</div>