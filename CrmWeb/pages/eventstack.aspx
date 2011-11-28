<%@ Page Language="C#" AutoEventWireup="true" CodeFile="eventstack.aspx.cs" Inherits="pages_eventstack" %>

<%@ Register TagPrefix="Sol" TagName="EventStackView" Src="~/EventStackView.ascx" %>
<%@ Register TagPrefix="Sol" TagName="MessageBox" Src="~/MessageBox.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Event stack</title>
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
    </style>
</head>
<body>
    <form id="form1" runat="server" defaultbutton="btnFilter" style="width: 1024px;">
    <asp:ScriptManager ID="ScriptManager1" runat="server" LoadScriptsBeforeUI="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always" ViewStateMode="Enabled"
        EnableViewState="true">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnFilter" />
        </Triggers>
        <ContentTemplate>
            <Sol:EventStackView runat="server" ID="eventStack" ShowAddNotes="false" ShowDueEvents="false"
                ShowEventStack="true" />
            <div class="EventStack" style="clear: none; margin-bottom: 0; padding-bottom: 0;">
                <h3 class="divEventHeader">
                    Filter Events
                </h3>
                  <div class="UserEvent">
                    <fieldset title="Filter Options">
                <table cellpadding="0" cellspacing="0">
                 <tr>
                    <td width="200px" height="25px"><label># Of Events</label></td>
                    <td> <asp:TextBox ID="txtNumResults" runat="server"  Style="display: inherit" Text="10"></asp:TextBox>
                            <asp:NumericUpDownExtender ID="txtNumResults_NumericUpDownExtender" Step="10" Maximum="100"
                                Minimum="0" Width="50" runat="server" TargetControlID="txtNumResults">
                            </asp:NumericUpDownExtender></td>
                 </tr>
                 <tr>
                    <td height="25px"><label>Clients Types</label></td>
                    <td><asp:DropDownList runat="server" ID="ddlClientsToShow" Width=182px>
                                <asp:ListItem Text="New and Existing Clients" Value="Both" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="New Clients" Value="New"></asp:ListItem>
                                <asp:ListItem Text="Existing Clients" Value="Existing"></asp:ListItem>
                            </asp:DropDownList></td>
                 </tr>
                 <tr>
                 <td height="25px"><label>Marketer</label></td>
                    <td>     <asp:CheckBox ID="cbFilterUserName" runat="server" onclick="$('[id*=&quot;txtFilterUserName&quot;]').toggle(); return true;"
                                TextAlign="left" />
                            &nbsp; <asp:TextBox ID="txtFilterUserName" runat="server" Style="display: none;border:1px solid black;" width=157px BorderColor=Black BorderStyle=Solid BorderWidth=1></asp:TextBox></td>
                   
                 </tr>
                 <tr>
                    <td height="25px"><label>Client Name</label></td>
                    <td>  <asp:CheckBox ID="cbFilterClientName" runat="server" onclick="$('[id*=&quot;txtFilterClientName&quot;]').toggle(); return true;"
                                TextAlign="left" Text="" />
                           &nbsp; <asp:TextBox ID="txtFilterClientName" runat="server" Style="display: none" width=157px BorderColor=Black BorderStyle=Solid BorderWidth=1></asp:TextBox></td>
                 </tr>
                 <tr>
                    <td height="25px"><label>Message</label></td>
                    <td> <asp:CheckBox ID="cbFilterMessage" runat="server" onclick="$('[id*=&quot;txtFilterMessage&quot;]').toggle(); return true;"
                                TextAlign="left"/>
                           &nbsp;  <asp:TextBox ID="txtFilterMessage" runat="server" Style="display: none" width=157px BorderColor=Black BorderStyle=Solid BorderWidth=1></asp:TextBox></td>
                 </tr>
                 <tr>
                    <td height="25px"><label>Status</label></td>
                    <td>  <asp:CheckBox ID="cbFilterStatus" runat="server" onclick="$('[id*=&quot;ddlFilterStatus&quot;]').toggle(); return true;"
                                TextAlign="left" Text="" />
                            &nbsp; <asp:DropDownList runat="server" ID="ddlFilterStatus" Style="display: none">
                                <asp:ListItem Text="All" Value=""></asp:ListItem>
                                <asp:ListItem Text="Active" Value="Active" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Done" Value="Done"></asp:ListItem>
                                <asp:ListItem Text="Overdue" Value="Overdue"></asp:ListItem>
                                <asp:ListItem Text="Inactive" Value="Inactive"></asp:ListItem>
                            </asp:DropDownList></td>
                 </tr>
                 <tr>
                    <td height="25px"><label>Created Date</label></td>
                    <td> <asp:CheckBox ID="cbFilterCreateDate" runat="server" onclick="$('[id*=&quot;spanFilterCreateDate&quot;]').toggle(); return true;"
                                TextAlign="left"  />
                            <span class="FilterDateSpan" id="spanFilterCreateDate" style="display: none;" runat="server">
                           &nbsp;     <asp:TextBox ID="txtFilterCreateStartDate" runat="server" MaxLength="1" Width="80px"></asp:TextBox>
                                <asp:MaskedEditExtender ID="txtFilterCreateStartDate_MaskedEditExtender" runat="server"
                                    CultureName="en-GB" MaskType="Date" Mask="99/99/9999" ErrorTooltipEnabled="false"
                                    MessageValidatorTip="false" ClearMaskOnLostFocus="false" TargetControlID="txtFilterCreateStartDate"
                                    AutoComplete="true" />
                                <input type="image" style="margin-top: 3px;" id="btnCreateDatePicker" name="btnCreateDatePicker"
                                    src="../images/icons/dp_button.png" alt="Click to show date picker" />
                                <asp:CalendarExtender ID="txtFilterCreateStartDate_CalendarExtender" runat="server"
                                    Enabled="True" TodaysDateFormat="dd/MM/yyyy" PopupButtonID="btnCreateDatePicker"
                                    TargetControlID="txtFilterCreateStartDate" Format="dd/MM/yyyy" Animated="true">
                                </asp:CalendarExtender>
                                &nbsp; to &nbsp;
                                <asp:TextBox ID="txtFilterCreateEndDate" runat="server" MaxLength="1" Width="80px"></asp:TextBox>
                                <asp:MaskedEditExtender ID="txtFilterCreateEndDate_MaskedEditExtender" runat="server"
                                    CultureName="en-GB" MaskType="Date" Mask="99/99/9999" ErrorTooltipEnabled="false"
                                    MessageValidatorTip="false" ClearMaskOnLostFocus="false" TargetControlID="txtFilterCreateEndDate"
                                    AutoComplete="true" />
                                <input type="image" style="margin-top: 3px;" id="btnCreateDateEndPicker" name="btnCreateDateEndPicker"
                                    src="../images/icons/dp_button.png" alt="Click to show date picker" />
                                <asp:CalendarExtender ID="txtFilterCreateEndDate_CalendarExtender" runat="server"
                                    Enabled="True" TodaysDateFormat="dd/MM/yyyy" PopupButtonID="btnCreateDateEndPicker"
                                    TargetControlID="txtFilterCreateEndDate" Format="dd/MM/yyyy" Animated="true">
                                </asp:CalendarExtender>
                            </span></td>
                 </tr>
                 <tr>
                    <td height="25px"><label>Sort By</label></td>
                    <td>  <asp:DropDownList runat="server" ID="ddlSortBy">
                                <asp:ListItem Text="Due Date" Value="dtDue" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Created Date" Value="dtCreated"></asp:ListItem>
                                <asp:ListItem Text="Type" Value="FKsEventType"></asp:ListItem>
                                <asp:ListItem Text="Creator" Value="FKiUserID"></asp:ListItem>
                                <asp:ListItem Text="Assignee" Value="FKiUserAssignedID"></asp:ListItem>
                                <asp:ListItem Text="Status" Value="sStatus"></asp:ListItem>
                            </asp:DropDownList></td>
                 </tr>
                </table>                      
                    </fieldset>
                    <div style="clear: both">
                        <asp:UpdateProgress ID="progress1" runat="server">
                            <ProgressTemplate>
                                <div id="progressBackgroundFilter">
                                </div>
                                <div id="processMessage">
                                    Loading...<br />
                                    <br />
                                    <img alt="Loading" src="../images/ajax-loader.gif" />
                                </div>
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                        <asp:Button runat="server" ID="btnFilter" Text="Filter" Style="float: right" OnClick="btnFilter_Click" />
                    </div>
                    <br />
                </div>
            </div>
            <div style="float: left; clear: none; margin: 0; padding: 0;">
                <Sol:EventStackView runat="server" ID="EventStackView1" ShowAddNotes="true" ShowDueEvents="false"
                    ShowEventStack="false" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
    <%--    <script type="text/javascript">
        // jQuery Script to disable erroneous enter submit
        $(function () {
            $('input').keypress(function (e) {
                var code = null;
                code = (e.keyCode ? e.keyCode : e.which);
                return (code == 13) ? false : true;
            });
        }); 
    </script>--%>
</body>
</html>

