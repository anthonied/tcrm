<%@ Page Language="C#" AutoEventWireup="true" CodeFile="dashboard.aspx.cs" Inherits="pages_new_clients_dash" %>

<%@ Register Assembly="DevExpress.Web.v10.2, Version=10.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="Sol" TagName="EventStackView" Src="~/EventStackView.ascx" %>
<%@ Register TagPrefix="Sol" TagName="MessageBox" Src="~/MessageBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Last 5 clients</title>
    <%--Hides the select column--%>
    <style type="text/css">
        .HideButton
        {
            display: none;
        }
    </style>
    <%--Load Style Sheets--%>
    <link href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.12/themes/base/jquery-ui.css"
        rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.6.0/jquery.min.js"></script>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.12/jquery-ui.min.js"></script>
    <link href="../css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../css/ThemeRed.css" rel="stylesheet" type="text/css" />
    <link href="../css/EventStack.css" rel="stylesheet" type="text/css" />
    <link href="../css/MessageBoxes.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        fieldset
        {
            padding: 0;
            margin: 0;
            border: 0;
        }
        fieldset div label
        {
            float: left;
            vertical-align: text-bottom;
            width: 6em;
            margin: 0.1em 0.42em 0.4em 0;
        }
        fieldset div span
        {
            float: left;
            vertical-align: middle;
            width: 9em;
            height: 2em;
        }
        
        .FilterDateSpan
        {
            float: none;
            vertical-align: middle;
            width: auto;
            height: auto;
            padding: 0;
        }
        .FilterDateSpan input
        {
            float: none;
            vertical-align: middle;
            height: auto;
            padding: 0;
        }
        fieldset div
        {
            clear: left; /*  width: 100%;*/
        }
        
        .DateDiv
        {
            clear: none;
        }
        
        .DateDiv div
        {
            clear: none;
        }
        fieldset div input
        {
            /*margin: 3px 2px 3px 2px;**/ /*padding: 1em;*/
            vertical-align: middle;
        }
        
        fieldset div select
        {
            margin: 2px 2px 3px 0px;
            vertical-align: middle;
        }
        
        .progress
        {
            text-align: center;
            float: left;
            margin-bottom: 5px;
        }
        
        #progressBackgroundFilter
        {
            position: fixed;
            top: 0px;
            bottom: 0px;
            left: 0px;
            right: 0px;
            overflow: hidden;
            padding: 0;
            margin: 0;
            background-color: #000;
            filter: alpha(opacity=50);
            opacity: 0.5;
            z-index: 13000;
        }
        #processMessage
        {
            position: fixed;
            top: 30%;
            left: 43%;
            padding: 10px;
            width: 14%;
            z-index: 13001;
            background-color: #fff;
            border: solid 1px #000;
        }
        
        a.white:visited {color:white;}
        a.white:hover {color:white;}
        a.white:active {color:white;}
        a.white:link {color:white;}
    </style>
</head>
<body>
    <script type="text/javascript">
        function ShowEventPopup(eventID) {
            document.getElementById('<%= hdnEventID.ClientID %>').value = eventID;
            document.getElementById('<%= btnShowEvent.ClientID %>').click();
        }
        function FilterAccordingTo(filterby, filterwith) {
            document.getElementById('<%= hdnFilterBy.ClientID %>').value = filterby;
            document.getElementById('<%= hdnFilterWith.ClientID %>').value = filterwith;
            document.getElementById('<%= btnPreFilter.ClientID %>').click();
            return false;
        }
    </script>
    <form id="form1" runat="server">
    <asp:HiddenField ID="hdnFilterBy" runat="server" />
    <asp:HiddenField ID="hdnFilterWith" runat="server" />
    <asp:Button ID="btnPreFilter" runat="server" Text="Button" Style="display: none;"
        OnClick="btnPreFilter_Click" />
    <div style="width: auto; height: auto; overflow: auto;">
        <asp:ScriptManager ID="ScriptManager1" runat="server" LoadScriptsBeforeUI="true">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always" ChildrenAsTriggers="true"
            ViewStateMode="Enabled" EnableViewState="true">
            <Triggers>
            </Triggers>
            <ContentTemplate>
                <div style="float: left; width: 400px; margin: 10px 10px 10px 10px;">
                
                         <Sol:EventStackView runat="server" ID="dueEventStack" ShowAddNotes="true" ShowDueEvents="false"
                        ShowEventStack="false" />
                
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
      
        <ProgressTemplate>
        <div id="progressBackgroundFilter"></div>

            <div id="processMessage">
                Loading...<br />
                <br />
                <img alt="Loading" src="../images/ajax-loader.gif" />
            </div>
        </ProgressTemplate>
        </asp:UpdateProgress>
        
                    
                    <div id="divOldClients" style="float: left; width: 400px; margin: 0px 10px 10px 10px; clear: both;" runat="server">
                        <table cellpadding="0" cellspacing="0" border="0" width="400">
                            <tr class="ContainerHeader" style="margin-top: 0;">
                                <td height="26px" style="font-size: 14px;">
                                    &nbsp;<b style="font-family: Calibri"> Existing Clients</b>
                                </td>
                                <td height="26px" style="font-size: 14px; text-align: right">
                                    <a class="white" href="assignmentlist.aspx" style="font-family: Calibri; font-style: italic">Print My List</a> &nbsp;
                                </td>
                            </tr>
                        </table>
                        <asp:GridView ID="gvExistingClients" AllowPaging="False" AllowSorting="False" DataKeyNames="PKiClientID"
                            AutoGenerateColumns="false" CellPadding="5" ForeColor="#333333" GridLines="Both"
                            CellSpacing="5" DataSourceID="dsExistingClientFinder" OnSelectedIndexChanged="gvExistingClients_SelectedIndexChanged"
                            runat="server" ShowHeaderWhenEmpty="true" Width="400" EnablePersistedSelection="false"
                            HorizontalAlign="Center" OnRowDataBound="gvExistingClients_RowDataBound" ShowHeader="false">
                            <%--//Formatting of Gridview----%>
                            <AlternatingRowStyle CssClass="GridAlternate" />
                            <RowStyle CssClass="GridNormalRow" />
                            <EmptyDataRowStyle CssClass="GridEmptyRow" />
                            <EmptyDataTemplate>
                                <asp:Label ID="Label1" Style="display: block; width: 100%;" runat="server">Sorry there are no assigned clients</asp:Label>
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:CommandField SelectText="Select" ShowSelectButton="true" ItemStyle-CssClass="HideButton"
                                    HeaderStyle-CssClass="HideButton" />
                                <asp:BoundField DataField="PKiClientID" Visible="false" />
                                <asp:BoundField DataField="sClientNumber" ShowHeader="true" HeaderText="Assigned Clients" />
                                <asp:BoundField DataField="sName"/>
                                <asp:BoundField DataField="sContactPerson" />
                            </Columns>
                        </asp:GridView>
                        <asp:SqlDataSource ID="dsExistingClientFinder" EnableCaching="true" DataSourceMode="DataSet"
                            runat="server" CacheExpirationPolicy="Absolute" SelectCommand="SELECT TOP 1000 [PKiClientID], [sClientNumber], [sName], [sContactPerson], [sEmail], [FKiUserID], [bExcluded] FROM [existing_clients] WHERE [FKiUserID] = 12 AND [bExcluded] = 0 ORDER BY [PKiClientID] DESC"
                            CacheDuration="30">
                            <SelectParameters>
                            <asp:SessionParameter Name="loggedinuser" SessionField="LoggedInUserID" Type="String"/>

                            </SelectParameters>
                            </asp:SqlDataSource>
                    </div>
                    <div id="divNewClients" style="float: left; width: 400px; margin: 0px 10px 10px 10px; clear: both;" runat="server">
                        <table cellpadding="0" cellspacing="0" border="0" width="400">
                            <tr class="ContainerHeader" style="margin-top: 0;">
                                <td height="26px" style="font-size: 14px;">
                                    &nbsp;<b style="font-family: Calibri"> New Clients</b>
                                </td>
                            </tr>
                        </table>
                        <asp:GridView ID="gvLastFiveClients" AllowPaging="True" AllowSorting="False" DataKeyNames="PKiNewClientID"
                            AutoGenerateColumns="false" CellPadding="5" ForeColor="#333333" GridLines="Both"
                            CellSpacing="5" DataSourceID="dsClientFinder" OnSelectedIndexChanged="gvLastFiveClients_SelectedIndexChanged"
                            runat="server" ShowHeaderWhenEmpty="true" Width="400" EnablePersistedSelection="false"
                            HorizontalAlign="Center" OnRowDataBound="gvLastFiveClients_RowDataBound" ShowHeader="false">
                            <%--//Formatting of Gridview----%>
                            <AlternatingRowStyle CssClass="GridAlternate" />
                            <RowStyle CssClass="GridNormalRow" />
                            <EmptyDataRowStyle CssClass="GridEmptyRow" />
                            <EmptyDataTemplate>
                                <asp:Label ID="Label1" Style="display: block; width: 100%;" runat="server">Sorry there are no new clients</asp:Label>
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:CommandField SelectText="Select" ShowSelectButton="true" ItemStyle-CssClass="HideButton"
                                    HeaderStyle-CssClass="HideButton" />
                                <asp:BoundField DataField="PKiNewClientID" Visible="false" />
                                <asp:BoundField DataField="sNewClientName" ShowHeader="true" HeaderText="New Clients" />
                                <asp:BoundField DataField="sNewClientContactPerson" />
                                <asp:BoundField DataField="dtNewClientCreated" />
                            </Columns>
                        </asp:GridView>
                        <asp:SqlDataSource ID="dsClientFinder" EnableCaching="true" DataSourceMode="DataSet"
                            runat="server" CacheExpirationPolicy="Absolute" SelectCommand="SELECT TOP 5 [PKiNewClientID], [dtNewClientCreated], [sNewClientName], [sNewClientContactPerson] FROM [new_clients] ORDER BY [dtNewClientCreated] DESC"
                            CacheDuration="30"></asp:SqlDataSource>
                    </div>
                </div>
                <div style="float: left; ">
                    <Sol:EventStackView runat="server" ID="eventStack" ShowAddNotes="false" ShowDueEvents="true"
                        ShowEventStack="false" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server" ChildrenAsTriggers="true" UpdateMode="Always">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnShowEvent" />
        </Triggers>
        <ContentTemplate>
            <asp:HiddenField ID="hdnEventID" runat="server" />
            <asp:Button ID="btnShowEvent" runat="server" Text="Button" OnClick="btnShowEvent_Click"
                Style="display: none;" />
            <dx:ASPxPopupControl ID="popupEventDetails" runat="server" ClientIDMode="AutoID"
                LoadingPanelImagePosition="Top" Modal="false" CloseAction="OuterMouseClick" ShowHeader="false"
                PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ContentStyle-Paddings-Padding="0"
                Border-BorderWidth="0">
                <ContentStyle VerticalAlign="Top">
                </ContentStyle>
                <ContentCollection>
                    <dx:PopupControlContentControl runat="server">
                        <div runat="server" id="divEventStack" class="EventStack" style="margin: 0;">
                            <div runat="server" id="divEventDetails" class="UserEvent" style="margin: 0;">
                            </div>
                        </div>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
             <asp:UpdateProgress ID="progress1" runat="server" AssociatedUpdatePanelID="UpdatePanel2">
      
        <ProgressTemplate>
        <div id="progressBackgroundFilter"></div>

            <div id="processMessage">
                Loading...<br />
                <br />
                <img alt="Loading" src="../images/ajax-loader.gif" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
        </ContentTemplate>
    </asp:UpdatePanel>

        
    </form>
   
</body>
</html>
