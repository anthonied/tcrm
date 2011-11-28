<%@ Page Language="C#" AutoEventWireup="true" CodeFile="New_Clients.aspx.cs" Inherits="pages_New_Clients" Culture="en-GB" %>

<%@ Register Assembly="DevExpress.Web.v10.2, Version=10.2.6.0"
    Namespace="DevExpress.Web.ASPxTabControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.2, Version=10.2.6.0"
    Namespace="DevExpress.Web.ASPxClasses" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.2, Version=10.2.6.0"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>New Clients</title>
    <%--Load Style Sheets--%>
    <link href="../css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../css/ThemeRed.css" rel="stylesheet" type="text/css" />
    <%--Load jQuery--%>
    <script type="text/javascript" src="http://code.jquery.com/jquery-latest.min.js"></script>
    <style type="text/css">
        .HideButton
        {
            display: none;
        }
    </style>
    <%--To hide gridview select buttons--%>
    <script type="text/javascript">
    // <![CDATA[
        var busyStatus = "none";
        var showHidePressed = 0;
        <%--Run jQuery on startup--%>
        $(document).ready(function () {      
        });
        // Handle the busy status in order to allow one type of operation
        function HandleBusyStatus() {
            if (busyStatus == "none")
                return true;
            else if (busyStatus == "client") {
                var res = confirm("You are busy editing a client. Your entered data will be lost. Are you sure you want to continue?");
                if (res == true)
                    return true;
                else
                    return false;
            }
            else if (busyStatus == "site") {
                var res = confirm("You are busy editing a site. Your entered data will be lost. Are you sure you want to continue?");
                if (res == true)
                    return true;
                else
                    return false;
            }
            else if (busyStatus == "foreman") {
                var res = confirm("You are busy editing a foreman. Your entered data will be lost. Are you sure you want to continue?");
                if (res == true)
                    return true;
                else
                    return false;
            }
        }

        // Show the site edit popup
        function ShowEditSitesPopup() {
            popupEditSites.Show();
        }
        // Hide the site edit popup
        function HideEditSitesPopup() {
            popupEditSites.Hide();
        }
        // Show the foreman edit popup
        function ShowEditForemenPopup() {
            popupEditForemen.Show();
        }
        // Hide the foreman edit popup
        function HideEditForemenPopup() {
            popupEditForemen.Hide();
        }

        function ShowClientFinder() {
            popupClientFinder.Show();
        }
        function HideClientFinder() {
            popupClientFinder.Hide();
        }
        // Handle filter request for sites
        function SitesFilter() {
            if (HandleBusyStatus() == false)
                return false;
            return true;
        }

        // Handle an enter key press on the filter box
        function FilterSitesResults(event) {
            event = event || window.event;
            var code = event.keyCode || event.which;

            if (code == 13)
                document.getElementById("btnFilterSites").click();
            return true;
        }

        // Handle filter request for foremen
        function ForemenFilter() {
            if (HandleBusyStatus() == false)
                return false;
            return true;
        }
        // Handle an enter key press on the filter box
        function FilterForemenResults(event) {
            event = event || window.event;
            var code = event.keyCode || event.which;

            if (code == 13)
                document.getElementById("btnFilterForemen").click();
            return true;
        }

        <%--Client specific functions--%>
        // Allow creation of a new client
        function NewClientClick() {        
            if (HandleBusyStatus() == false)
                return false;
            $("#btnUpdateClient").hide();
            $("#btnDeleteClient").hide();
            busyStatus = "client";
            document.getElementById("lblClientEditStatus").innerHTML = "You are creating a new client";
            $("#lblClientEditStatus").css("display", "block");
            // Clear edit boxes
            $('[id*="txtNewClient"]').each(function () {
                this.value = "";
            });
            $('[id*="txtCE"]').each(function () {
                //document.writeln(this.value);
                this.value = "";
            });
            $('[id*="txtPM"]').each(function () {
                this.value = "";
            });
            $('[id*="txtContractor"]').each(function () {
                this.value = "";
            });
            $("#btnSubmitClient").show();
            $("#btnClientCancel").show();
            $("#btnNewSite").hide();
            $("#btnNewForeman").hide();
            
            $("[id*='tblSites']").children().children("tr").remove(":not(.ContainerHeader)");
            $("[id*='tblSites']").children().append("<tr> <td colspan=\"6\" height=\"35\" align=\"center\">&nbsp; NO DATA CURRENTLY AVAILABLE</td></tr>");
            $("[id*='tblForemen']").children().children("tr").remove(":not(.ContainerHeader)");
            $("[id*='tblForemen']").children().append("<tr> <td colspan=\"4\" height=\"35\" align=\"center\">&nbsp; NO DATA CURRENTLY AVAILABLE</td></tr>");
            return false;
        }

        // Delete a client
         function DeleteClientClick() {
            if (HandleBusyStatus() == false)
                return false;
            $("#btnUpdateClient").show();
            $("#btnClientCancel").hide();
            $("#btnSubmitClient").hide();
            busyStatus = "none";
            var result = confirm("Are you sure you want to delete this client? Note that all data associated with this client will be lost.");
            if (result == true)
                return true;
            else
                return false;
        }
        
        // Cancel the client operation
        function ClientCancelClick() {
            if (HandleBusyStatus() == false)
                return false;
            $("#btnUpdateClient").show();
            $("#btnSubmitClient").hide();
            $("#btnClientCancel").hide();
            return true;
        }

        // Submit new client data
        function SubmitClientClick() {
            if ($("#txtNewClientName").val().trim().length == 0) {
                alert("A client name is compulsory!");
                $("#txtNewClientName").focus();
                return false;
            }
            $("#btnUpdateClient").show();
            $("#btnSubmitClient").hide();
            $("#btnClientCancel").hide();
            return true;
        }

        // Update client data
        function UpdateClientClick() {
            if ($("#txtNewClientName").val().trim().length == 0) {
                alert("A client name is compulsory!");
                $("#txtNewClientName").focus();
                return false;
            }
            $("#btnClientCancel").hide();
            return true;
        }
         <%--Site specific functions--%>
        // Allow creation of a new site
        function NewSiteClick() {
            if (HandleBusyStatus() == false)
                return false;
            busyStatus = "site";

            // Handle housekeeping
            $("#btnUpdateClient").hide();
            $("#btnSubmitClient").hide();
            $("#btnClientCancel").hide();
            ShowEditSitesPopup();
            $("#btnSubmitEditSitesPopup").show();
            $("#btnUpdateEditSitesPopup").hide();
            $("#btnCancelEditSitesPopup").show();

            // Clear all text fields
            $('[id*="txtSite"]').each(function () {
                this.value = "";
            });

            // Show the status bar
            document.getElementById("lblClientEditStatus").innerHTML = "You are creating a new site";
            $("#lblClientEditStatus").css("display", "block");
            return false;
        }

        // Delete a site
        function DeleteSiteClick() {
            if (HandleBusyStatus() == false)
                return false;
            $("#btnUpdateClient").hide();
            $("#btnClientCancel").hide();
            $("#btnSubmitClient").hide();
            busyStatus = "none";
            var result = confirm("Are you sure you want to delete this site? Note that all data associated with this site will be lost.");
            if (result == true)
                return true;
            else
                return false;
        }

        // View site information only
        function ViewSiteDetails() {
            if (HandleBusyStatus() == false)
                return false;
            busyStatus = "site";
            $("#btnUpdateClient").hide();
            $("#btnSubmitClient").hide();
            $("#btnClientCancel").hide();
            ShowEditSitesPopup();
            $("#btnSubmitEditSitesPopup").hide();
            $("#btnUpdateEditSitesPopup").show();
            $("#btnCancelEditSitesPopup").show();
            return false;
        }

        // Cancel the site operation
        function EditSitesCancel() {
            if (HandleBusyStatus() == false)
                return false;
            HideEditSitesPopup();
            return true;
        }

        // Submit new site data
        function EditSitesSubmit() {
            if ($("#txtSiteName").val().trim().length == 0) {
                alert("A site name is compulsory!");
                $("#txtSiteName").focus();
                return false;
            }
            else if ($("#ddSiteType").val().trim().length == 0) {
                alert("A site type is compulsory!");
                $("#txtSiteType").focus();
                return false;
            }
            HideEditSitesPopup();
            return true;
        }

        // Update site data
        function EditSitesUpdate() {
            if ($("#txtSiteName").val().trim().length == 0) {
                alert("A site name is compulsory!");
                $("#txtSiteName").focus();
                return false;
            }
            else if ($("#txtSiteType").val().trim().length == 0) {
                alert("A site type is compulsory!");
                $("#txtSiteType").focus();
                return false;
            }
            HideEditSitesPopup();
            return true;
        }

         <%--Foreman specific functions--%>
         // Allow creation of a new foreman
        function NewForemanClick() {
            if (HandleBusyStatus() == false)
                return false;
            busyStatus = "foreman";

            // Handle housekeeping
            $("#btnUpdateClient").hide();
            $("#btnSubmitClient").hide();
            $("#btnClientCancel").hide();
            ShowEditForemenPopup();
            $("#btnSubmitEditForemenPopup").show();
            $("#btnUpdateEditForemenPopup").hide();
            $("#btnCancelEditForemenPopup").show();            
            
            // Clear all text fields
            $('[id*="txtForeman"]').each(function () {
                this.value = "";
            });

            // Show the status bar
            document.getElementById("lblClientEditStatus").innerHTML = "You are creating a new foreman";
            $("#lblClientEditStatus").css("display", "block");
            return false;
        }
        
        // Delete a foreman
       function DeleteForemanClick() {
            if (HandleBusyStatus() == false)
                return false;
            $("#btnUpdateClient").hide();
            $("#btnClientCancel").hide();
            $("#btnSubmitClient").hide();
            busyStatus = "none";
            var result = confirm("Are you sure you want to delete this foreman? Note that all data associated with this foreman will be lost.");
            if (result == true)
                return true;
            else
                return false;
        }
        
        // View foreman information only
        function ViewForemanDetails() {
            if (HandleBusyStatus() == false)
                return false;
            busyStatus = "foreman";
            $("#btnSubmitClient").hide();
            $("#btnClientCancel").hide();
            ShowEditForemenPopup();
            $("#btnSubmitEditForemenPopup").hide();
            $("#btnUpdateEditForemenPopup").show();
            $("#btnCancelEditForemenPopup").show();
            return false;
        }
        
        // Cancel the foreman operation
         function EditForemenCancel() {
            if (HandleBusyStatus() == false)
                return false;
            HideEditForemenPopup();
            return true;
        }

        // Submit new foreman data
        function EditForemenSubmit() {
            if ($("#txtForemanName").val().trim().length == 0) {
                alert("A foreman name is compulsory!");
                $("#txtForemanName").focus();
                return false;
            }
            HideEditForemenPopup();
            return true;
        }
       
       // Update foreman data
        function EditForemanUpdate() {
            if ($("#txtForemanName").val().trim().length == 0) {
                alert("A foreman name is compulsory!");
                $("#txtForemanName").focus();
                return false;
            }
            HideEditForemenPopup();
            return true;
        }

        // Show the foremen table
        function ShowForemanTable(headertext)
        {
            popupForemenTable.Show();
            popupForemenTable.SetHeaderText(headertext + " - Foremen");
            document.getElementById("hdnSelectedSiteName").value = headertext + " - Foremen";
            if(document.getElementById("hdnForemenVisible").value == "0")
                document.getElementById("hdnForemenVisible").value = "1";
            else
                document.getElementById("hdnForemenVisible").value = "0";
            return true;
        }

        // Hide the foremen table
        function HideForemanTable()
        {
            popupForemenTable.Hide();

            if(document.getElementById("hdnForemenVisible").value == "0")
                document.getElementById("hdnForemenVisible").value = "1";
            else
                document.getElementById("hdnForemenVisible").value = "0";
            return true;
        }
        
       // ]]>
    </script>
</head>
<%--Special block outside of head to allow asp tags--%>
<script type="text/javascript"> 
// <![CDATA[
    // Force a submit when a new site was selected
    function SelectSite(siteID) {
            var sSiteID = siteID.toString();
            if(sSiteID != document.getElementById("hdnSelectedSite").value)
                document.getElementById("hdnSelectedSite").value = sSiteID;
            else
                return false;
            //document.getElementById("btnForcePostBack").click();
            <%= ClientScript.GetPostBackEventReference(this, "@@@@@hdnSelectedSite") %>
            return true;
        }

    // Force a submit when a new foreman was selected
        function SelectForeman(foremanID) {
            var sForemanID = foremanID.toString();
            if(sForemanID != document.getElementById("hdnSelectedForeman").value)
                document.getElementById("hdnSelectedForeman").value = foremanID.toString();
            else
                return false;
            //document.getElementById("btnForcePostBack").click();
            <%= ClientScript.GetPostBackEventReference(this, "@@@@@hdnSelectedForeman") %>
            return true;
        }
         // ]]>
</script>
<body class="body">
    <div>
        <form id="frmNewClients" runat="server">
        <input type="submit" id="btnForcePostBack" name="btnForcePostBack" style="display: none;" />
        <asp:ScriptManager ID="ScriptManager1" runat="server" LoadScriptsBeforeUI="true">
        </asp:ScriptManager>
        <!--Holds currently selected site-->
        <asp:HiddenField runat="server" Value="-1" ClientIDMode="Static" ID="hdnSelectedSite"
            OnValueChanged="hdnSelectedSite_ValueChanged" />
        <!--Holds currently selected foreman-->
        <asp:HiddenField runat="server" Value="-1" ClientIDMode="Static" ID="hdnSelectedForeman"
            OnValueChanged="hdnSelectedForeman_ValueChanged" />
        <!--Keeps status of foreman visibility-->
        <asp:HiddenField runat="server" Value="0" ClientIDMode="Static" ID="hdnForemenVisible"
            OnValueChanged="hdnForemenVisible_ValueChanged" />
        <!--Keeps the selected site name-->
        <asp:HiddenField runat="server" Value="0" ClientIDMode="Static" ID="hdnSelectedSiteName" />
        <!--Label to show status of client's operations-->
        <label id="lblClientEditStatus" style="display: none; width: 982px; height: auto;
            padding: 10px 15px 10px 15px; margin: 5px; border-style: solid; border-width: 2px;
            border-color: #00ff00; background-color: #F0E68C; font-family: Calibri; font-size: 14px;">
            <b>Warning:</b> You are in edit mode</label>
        <br />
        <div class="holder_table" style="width: 1024px;">
            <table cellpadding="0" cellspacing="0" border="0" width="1024px">
                <tr class="ContainerHeader">
                    <td height="26px" style="font-size: 14px;" width="100">
                        &nbsp;<b style="font-family: Calibri"> New Clients</b>
                    </td>
                    <td height="26px" align="left" style="width: 390px;">
                        <!--List of clients-->
                        <asp:DropDownList ID="ddNewClients" runat="server" AutoPostBack="True" DataValueField="PKiNewClientID"
                            DataTextField="sNewClientName" Width="350px" OnSelectedIndexChanged="ddNewClients_SelectedIndexChanged"
                            CssClass="textboxBorder">
                        </asp:DropDownList>
                        &nbsp; &nbsp;<asp:ImageButton runat="server" ClientIDMode="Static" ID="btnClientFinder"
                            ImageUrl="../images/icons/search.png" Style="cursor: pointer; height: 14px;"
                            AlternateText="Filter Sites" ToolTip="Find client" OnClientClick="ShowClientFinder(); return false;" />
                        &nbsp;
                    </td>
                    <td height="26px" align="right">
                        <!--Client operation buttons-->
                        &nbsp;<asp:ImageButton runat="server" ClientIDMode="Static" ID="btnNewClient" ImageUrl="../images/icons/new_metro_black.png"
                            Style="cursor: pointer; height: 14px;" AlternateText="Filter Sites" ToolTip="Create a new client"
                            OnClientClick="return NewClientClick();" />
                        &nbsp; &nbsp;<asp:ImageButton runat="server" ClientIDMode="Static" ID="btnDeleteClient"
                            ImageUrl="../images/icons/delete_black.png" Style="cursor: pointer; height: 14px;"
                            AlternateText="Filter Sites" ToolTip="Delete selected client" OnClientClick="return DeleteClientClick();"
                            OnClick="btnDeleteClient_Click" />
                        &nbsp;
                        <!--Client action buttons-->
                        &nbsp;<asp:ImageButton runat="server" ClientIDMode="Static" ID="btnUpdateClient"
                            ImageUrl="../images/icons/save_metro_black.png" Style="cursor: pointer; height: 14px;"
                            AlternateText="Filter Sites" ToolTip="Apply" OnClientClick="return UpdateClientClick();"
                            OnClick="btnUpdateClient_Click" />
                        &nbsp; &nbsp;<asp:ImageButton runat="server" ClientIDMode="Static" ID="btnSubmitClient"
                            ImageUrl="../images/icons/save_metro_black.png" Style="display: none; cursor: pointer;
                            height: 14px;" AlternateText="Apply" ToolTip="Apply" OnClientClick="return SubmitClientClick();"
                            OnClick="btnSubmitClient_Click" />
                        &nbsp; &nbsp;<asp:ImageButton runat="server" ClientIDMode="Static" ID="btnClientCancel"
                            ImageUrl="../images/icons/cancel_black.png" Style="display: none; cursor: pointer;
                            height: 14px;" AlternateText="Filter Sites" ToolTip="Cancel" OnClientClick="return ClientCancelClick();"
                            OnClick="btnClientCancel_Click" />
                        &nbsp;
                    </td>
                </tr>
            </table>
        </div>
        <!--Main page control (tabs)-->
        <dx:ASPxPageControl ID="pageClientData" runat="server" ActiveTabIndex="0" ClientIDMode="AutoID"
            CssFilePath="~/App_Themes/RedWineCustom/{0}/styles.css" CssPostfix="RedWine"
            SpriteCssFilePath="~/App_Themes/RedWineCustom/{0}/sprite.css" TabSpacing="8px"
            tab Width="1024px">
            <TabPages>
                <dx:TabPage Name="tabClient" Text="Client">
                    <ContentCollection>
                        <dx:ContentControl runat="server" SupportsDisabledAttribute="True">
                            <table>
                                <tr>
                                    <td height="25px">
                                        Client Name *:&nbsp;
                                    </td>
                                    <td height="25px">
                                        <asp:TextBox ClientIDMode="Static" ID="txtNewClientName" runat="server" CssClass="textboxBorder"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td height="25px">
                                        Contact Person:&nbsp;
                                    </td>
                                    <td height="25px">
                                        <asp:TextBox ClientIDMode="Static" ID="txtNewClientContactPerson" runat="server"
                                            CssClass="textboxBorder"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td height="25px">
                                        Telephone:&nbsp;
                                    </td>
                                    <td height="25px">
                                        <asp:TextBox ClientIDMode="Static" ID="txtNewClientTelephone" runat="server" CssClass="textboxBorder"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td height="25px">
                                        Cellphone:&nbsp;
                                    </td>
                                    <td height="25px">
                                        <asp:TextBox ClientIDMode="Static" ID="txtNewClientCellphone" runat="server" CssClass="textboxBorder"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td height="25px">
                                        Email:&nbsp;
                                    </td>
                                    <td height="25px">
                                        <asp:TextBox ClientIDMode="Static" ID="txtNewClientEmail" runat="server" CssClass="textboxBorder"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </dx:ContentControl>
                    </ContentCollection>
                </dx:TabPage>
                <dx:TabPage Name="tabConsultingEng" Text="Consulting Engineer">
                    <ContentCollection>
                        <dx:ContentControl runat="server" SupportsDisabledAttribute="True">
                            <table>
                                <tr>
                                    <td height="25px">
                                        Firm Name:&nbsp;
                                    </td>
                                    <td height="25px">
                                        <asp:TextBox ClientIDMode="Static" ID="txtCEFirmName" runat="server" CssClass="textboxBorder"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td height="25px">
                                        Contact Person:&nbsp;
                                    </td>
                                    <td height="25px">
                                        <asp:TextBox ClientIDMode="Static" ID="txtCEContactPerson" runat="server" CssClass="textboxBorder"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td height="25px">
                                        Telephone:&nbsp;
                                    </td>
                                    <td height="25px">
                                        <asp:TextBox ClientIDMode="Static" ID="txtCETelephone" runat="server" CssClass="textboxBorder"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td height="25px">
                                        Cellphone:&nbsp;
                                    </td>
                                    <td height="25px">
                                        <asp:TextBox ClientIDMode="Static" ID="txtCECellphone" runat="server" CssClass="textboxBorder"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td height="25px">
                                        Email:&nbsp;
                                    </td>
                                    <td height="25px">
                                        <asp:TextBox ClientIDMode="Static" ID="txtCEEmail" runat="server" CssClass="textboxBorder"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </dx:ContentControl>
                    </ContentCollection>
                </dx:TabPage>
                <dx:TabPage Name="tabProjectManager" Text="Project Manager">
                    <ContentCollection>
                        <dx:ContentControl runat="server" SupportsDisabledAttribute="True">
                            <table>
                                <tr>
                                    <td height="25px">
                                        Firm Name:&nbsp;
                                    </td>
                                    <td height="25px">
                                        <asp:TextBox ClientIDMode="Static" ID="txtPMFirmName" runat="server" CssClass="textboxBorder"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td height="25px">
                                        Contact Person:&nbsp;
                                    </td>
                                    <td height="25px">
                                        <asp:TextBox ClientIDMode="Static" ID="txtPMContactPerson" runat="server" CssClass="textboxBorder"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td height="25px">
                                        Telephone:&nbsp;
                                    </td>
                                    <td height="25px">
                                        <asp:TextBox ClientIDMode="Static" ID="txtPMTelephone" runat="server" CssClass="textboxBorder"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td height="25px">
                                        Cellphone:&nbsp;
                                    </td>
                                    <td height="25px">
                                        <asp:TextBox ClientIDMode="Static" ID="txtPMCellphone" runat="server" CssClass="textboxBorder"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td height="25px">
                                        Email:&nbsp;
                                    </td>
                                    <td height="25px">
                                        <asp:TextBox ClientIDMode="Static" ID="txtPMEmail" runat="server" CssClass="textboxBorder"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </dx:ContentControl>
                    </ContentCollection>
                </dx:TabPage>
                <dx:TabPage Name="tabContractor" Text="Contractor">
                    <ContentCollection>
                        <dx:ContentControl runat="server" SupportsDisabledAttribute="True">
                            <table>
                                <tr>
                                    <td height="25px">
                                        Firm Name:&nbsp;
                                    </td>
                                    <td height="25px">
                                        <asp:TextBox ClientIDMode="Static" ID="txtContractorFirmName" runat="server" CssClass="textboxBorder"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td height="25px">
                                        Contact Person:&nbsp;
                                    </td>
                                    <td height="25px">
                                        <asp:TextBox ClientIDMode="Static" ID="txtContractorContactPerson" runat="server"
                                            CssClass="textboxBorder"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td height="25px">
                                        Telephone:&nbsp;
                                    </td>
                                    <td height="25px">
                                        <asp:TextBox ClientIDMode="Static" ID="txtContractorTelephone" runat="server" CssClass="textboxBorder"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td height="25px">
                                        Cellphone:&nbsp;
                                    </td>
                                    <td height="25px">
                                        <asp:TextBox ClientIDMode="Static" ID="txtContractorCellphone" runat="server" CssClass="textboxBorder"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td height="25px">
                                        Email:&nbsp;
                                    </td>
                                    <td height="25px">
                                        <asp:TextBox ClientIDMode="Static" ID="txtContractorEmail" runat="server" CssClass="textboxBorder"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </dx:ContentControl>
                    </ContentCollection>
                </dx:TabPage>
                <dx:TabPage Name="tabSites" Text="Sites">
                    <ContentCollection>
                        <dx:ContentControl runat="server" SupportsDisabledAttribute="True">
                            <!--Site table headers-->
                            <div class="holder_table">
                                <div style="display:inline; position: relative; top: -39px; left: 81px;">
                                <table cellpadding="0" cellspacing="0" border="0" width="100%">                                    
                                    <tr>
                                        <td height="27px" align="right">
                                            &nbsp;<asp:TextBox ClientIDMode="Static" ID="txtFilterSites" onkeypress="return FilterSitesResults(event);"
                                                value="" runat="server" CssClass="textBoxBorder" />
                                            &nbsp;<asp:ImageButton runat="server" ClientIDMode="Static" ID="btnFilterSites" ImageUrl="../images/icons/search.png"
                                                Style="cursor: pointer; height: 14px;" AlternateText="Filter Sites" ToolTip="Filter Sites"
                                                OnClick="btnFilterSites_Click" OnClientClick="return SitesFilter();" />
                                        </td>
                                        <td height="27px" align="left" style="width: 150px;">
                                            <!--Site operation buttons-->    
                                            &nbsp; <asp:ImageButton runat="server" ClientIDMode="Static" ID="btnNewSite"
                                                ImageUrl="../images/icons/new_metro_black.png" Style="cursor: pointer; height: 14px;"
                                                AlternateText="Filter Sites" ToolTip="New site" OnClientClick="return NewSiteClick();" />
                                            &nbsp; &nbsp;<asp:ImageButton runat="server" ClientIDMode="Static" ID="btnDeleteSite"
                                                ImageUrl="../images/icons/delete_black.png" Style="display: none; cursor: pointer;
                                                height: 14px;" AlternateText="Filter Sites" ToolTip="Delete site" OnClientClick="return DeleteSiteClick();"
                                                OnClick="btnDeleteSite_Click" />
                                                 &nbsp;<asp:ImageButton runat="server" ClientIDMode="Static" ID="btnViewSiteDetails"
                                                ImageUrl="../images/icons/info_metro_black.png" Style="display: none; cursor: pointer;
                                                height: 14px;" AlternateText="Filter Sites" ToolTip="View site details" OnClientClick="return ViewSiteDetails();" />
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                                </div>
                                <center>
                                    <!--Holds list of sites-->
                                    <table id="tblSites" cellpadding="0" cellspacing="0" border="1" runat="server" style="margin-top: 0px;
                                        margin-bottom: 20px; width: 96%;">
                                        <tr class="ContainerHeader">
                                            <td align="center" style="font-family: Calibri; font-size: 12px">
                                                Site Name
                                            </td>
                                            <td align="center" style="font-family: Calibri; font-size: 12px">
                                                Type
                                            </td>
                                            <td align="center" style="font-family: Calibri; font-size: 12px">
                                                Contractor
                                            </td>
                                            <td align="center" style="font-family: Calibri; font-size: 12px">
                                                Telephone
                                            </td>
                                            <td align="center" style="font-family: Calibri; font-size: 12px">
                                                Fax
                                            </td>
                                            <td align="center" style="font-family: Calibri; font-size: 12px">
                                                Foremen
                                            </td>
                                        </tr>
                                    </table>
                                </center>
                            </div>
                        </dx:ContentControl>
                    </ContentCollection>
                </dx:TabPage>
            </TabPages>
            <LoadingPanelImage Url="~/App_Themes/RedWine/Web/Loading.gif">
            </LoadingPanelImage>
            <Paddings Padding="2px" PaddingLeft="5px" PaddingRight="5px"></Paddings>
            <ContentStyle>
                <Border BorderColor="#A74768" BorderStyle="Solid" BorderWidth="1px" />
                <Border BorderColor="#A74768" BorderStyle="Solid" BorderWidth="1px"></Border>
            </ContentStyle>
        </dx:ASPxPageControl>
        <!--Site edit popup box-->
        <dx:ASPxPopupControl ID="popupEditSites" runat="server" ClientIDMode="AutoID" CssFilePath="~/App_Themes/RedWineCustom/{0}/styles.css"
            CssPostfix="RedWine" LoadingPanelImagePosition="Top" Modal="True" CloseAction="None"
            SpriteCssFilePath="~/App_Themes/RedWineCustom/{0}/sprite.css" PopupHorizontalAlign="WindowCenter"
            PopupVerticalAlign="WindowCenter" Style="margin-top: 0px; margin-right: 0px;"
            HeaderText="Site Editor" Width="633px">
            <ClientSideEvents CloseButtonClick="function(s, e) { document.getElementById('btnCancelEditSitesPopup').click();
	            return	false;
            }" />
            <LoadingPanelImage Url="~/App_Themes/RedWine/Web/Loading.gif">
            </LoadingPanelImage>
            <ContentStyle VerticalAlign="Top">
            </ContentStyle>
            <ContentCollection>
                <dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">
                    <table>
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td height="25px" width="150px">
                                            Site Name *:&nbsp;
                                        </td>
                                        <td height="25px">
                                            <asp:TextBox ClientIDMode="Static" ID="txtSiteName" runat="server" CssClass="textboxBorder"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td height="25px" width="150px">
                                            Type *:&nbsp;
                                        </td>
                                        <td height="25px" width="150px">
                                            <asp:DropDownList ClientIDMode="Static" ID="ddSiteType" runat="server" CssClass="textboxBorder"
                                                DataValueField="PKsSiteType" DataTextField="PKsSiteType">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td height="25px" width="150px">
                                            Contractor:&nbsp;
                                        </td>
                                        <td height="25px" width="150px">
                                            <asp:TextBox ClientIDMode="Static" ID="txtSiteContractor" runat="server" CssClass="textboxBorder"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td height="25px" width="150px">
                                            Telephone:&nbsp;
                                        </td>
                                        <td height="25px">
                                            <asp:TextBox ClientIDMode="Static" ID="txtSiteTelephone" runat="server" CssClass="textboxBorder"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td height="25px" width="150px">
                                            Phys. Address 1:&nbsp;
                                        </td>
                                        <td height="25px">
                                            <asp:TextBox ClientIDMode="Static" ID="txtSitePhysAddress1" runat="server" CssClass="textboxBorder"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td height="25px" width="150px">
                                            Phys. Address 2:&nbsp;
                                        </td>
                                        <td height="25px" width="150px">
                                            <asp:TextBox ClientIDMode="Static" ID="txtSitePhysAddress2" runat="server" CssClass="textboxBorder"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td height="25px" width="150px">
                                            Phys. Address 3:&nbsp;
                                        </td>
                                        <td height="25px" width="150px">
                                            <asp:TextBox ClientIDMode="Static" ID="txtSitePhysAddress3" runat="server" CssClass="textboxBorder"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td height="25px" width="150px">
                                            Phys. Address 4:&nbsp;
                                        </td>
                                        <td height="25px" width="150px">
                                            <asp:TextBox ClientIDMode="Static" ID="txtSitePhysAddress4" runat="server" CssClass="textboxBorder"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                <table>
                                    <tr>
                                        <td height="25px" width="150px">
                                            Fax:&nbsp;
                                        </td>
                                        <td height="25px">
                                            <asp:TextBox ClientIDMode="Static" ID="txtSiteFax" runat="server" CssClass="textboxBorder"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td height="25px" width="150px">
                                            Email:&nbsp;
                                        </td>
                                        <td height="25px" width="150px">
                                            <asp:TextBox ClientIDMode="Static" ID="txtSiteEmail" runat="server" CssClass="textboxBorder"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td height="25px" width="150px">
                                            Area:&nbsp;
                                        </td>
                                        <td height="25px" width="150px">
                                            <asp:TextBox ClientIDMode="Static" ID="txtSiteArea" runat="server" CssClass="textboxBorder"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td height="25px" width="150px">
                                            Contract Worth:&nbsp;
                                        </td>
                                        <td height="25px" width="150px">
                                            <asp:TextBox ClientIDMode="Static" ID="txtSiteContractValue" runat="server" CssClass="textboxBorder"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td height="25px" width="150px">
                                            Start Date:&nbsp;
                                        </td>
                                        <td height="25px" width="200px">
                                            <asp:TextBox ClientIDMode="Static" ID="txtSiteStartDate" runat="server" class="textboxBorder"
                                                Width="130px"></asp:TextBox>
                                            <input type="image" id="btnStartDatePicker" name="btnStartDatePicker" src="../images/icons/dp_button.png"
                                                alt="Click to show date picker" />
                                            <asp:CalendarExtender ID="txtSiteStartDate_CalendarExtender" runat="server" Enabled="True"
                                                PopupButtonID="btnStartDatePicker" TargetControlID="txtSiteStartDate" Format="d/M/yyyy">
                                            </asp:CalendarExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td height="25px" width="150px">
                                            End Date:&nbsp;
                                        </td>
                                        <td height="25px" width="200px">
                                            <asp:TextBox ClientIDMode="Static" ID="txtSiteEndDate" runat="server" class="textboxBorder"
                                                Width="130px"></asp:TextBox>
                                            <input type="image" id="btnEndDatePicker" name="btnEndDatePicker" src="../images/icons/dp_button.png"
                                                alt="Click to show date picker" />
                                            <asp:CalendarExtender ID="txtSiteEndDate_CalendarExtender" runat="server" Enabled="True"
                                                PopupButtonID="btnEndDatePicker" TargetControlID="txtSiteEndDate" Format="d/M/yyyy">
                                            </asp:CalendarExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td height="25px" width="150px">
                                            GPS Coordinates:&nbsp;
                                        </td>
                                        <td height="25px" width="150px">
                                            <asp:TextBox ClientIDMode="Static" ID="txtSiteGPSCoordinates" runat="server" CssClass="textboxBorder"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td height="25px" width="150px">
                                            &nbsp;
                                        </td>
                                        <td height="25px" width="150px">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <asp:Button ClientIDMode="Static" ID="btnSubmitEditSitesPopup" runat="server" OnClientClick="return EditSitesSubmit();"
                        Text="Submit" CssClass="btn_submit" Width="50" OnClick="btnSubmitEditSitesPopup_Click"
                        Style="display: none;" />
                    <asp:Button ClientIDMode="Static" ID="btnUpdateEditSitesPopup" runat="server" Text="Update"
                        OnClientClick="return EditSitesUpdate();" CssClass="btn_submit" Style="display: none;"
                        OnClick="btnUpdateEditSitesPopup_Click" />
                    <asp:Button ClientIDMode="Static" ID="btnCancelEditSitesPopup" runat="server" OnClientClick="return EditSitesCancel();"
                        Text="Cancel" CssClass="btn_submit" Width="50" OnClick="btnCancelEditSitesPopup_Click"
                        Style="display: none;" />
                </dx:PopupControlContentControl>
            </ContentCollection>
        </dx:ASPxPopupControl>
        <!--Foreman table popup box-->
        <dx:ASPxPopupControl ID="popupForemenTable" runat="server" ClientIDMode="AutoID"
            CssFilePath="~/App_Themes/RedWineCustom/{0}/styles.css" CssPostfix="RedWine"
            LoadingPanelImagePosition="Top" Modal="True" CloseAction="None" SpriteCssFilePath="~/App_Themes/RedWineCustom/{0}/sprite.css"
            PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Style="margin-top: 0px;
            margin-right: 0px;" HeaderText="Foremen" Width="600px">
            <ClientSideEvents CloseButtonClick="function(s, e) { HideForemanTable();
            }" />
            <LoadingPanelImage Url="~/App_Themes/RedWine/Web/Loading.gif">
            </LoadingPanelImage>
            <ContentStyle VerticalAlign="Top">
            </ContentStyle>
            <ContentCollection>
                <dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server" SupportsDisabledAttribute="True">
                    <div id="tblForemenTable" runat="server">
                        <!--Foreman table headers-->
                        <table cellpadding="0" cellspacing="0" border="0" width="100%">
                            <tr class="ContainerHeader">
                                <td height="26px" style="font-size: 14px;" width="200">
                                    &nbsp;<b style="font-family: Calibri;"> Foreman List</b>
                                </td>
                                <td height="26px" align="right">
                                    &nbsp;<asp:TextBox ClientIDMode="Static" ID="txtFilterForemen" onkeypress="return FilterForemenResults(event);"
                                        value="" runat="server" CssClass="textBoxBorder" />
                                    &nbsp;<asp:ImageButton runat="server" ClientIDMode="Static" ID="btnFilterForemen"
                                        ImageUrl="../images/icons/search.png" Style="cursor: pointer" AlternateText="Filter Foremen"
                                        ToolTip="Filter Foremen" OnClick="btnFilterForemen_Click" OnClientClick="return ForemenFilter();" />
                                    &nbsp;
                                </td>
                                <td height="26px" align="right" style="width: 150px;">
                                    <!--Foreman operation buttons-->
                                    &nbsp;<asp:ImageButton runat="server" ClientIDMode="Static" ID="btnViewForemanDetails"
                                        ImageUrl="../images/icons/info_metro_black.png" Style="display: none; cursor: pointer;
                                        height: 14px;" AlternateText="Filter Sites" ToolTip="View foreman details" OnClientClick="return ViewForemanDetails();" />
                                    &nbsp; &nbsp;<asp:ImageButton runat="server" ClientIDMode="Static" ID="btnNewForeman"
                                        ImageUrl="../images/icons/new_metro_black.png" Style="cursor: pointer; height: 14px;"
                                        AlternateText="Filter Sites" ToolTip="New foreman" OnClientClick="return NewForemanClick();" />
                                    &nbsp; &nbsp;<asp:ImageButton runat="server" ClientIDMode="Static" ID="btnDeleteForeman"
                                        ImageUrl="../images/icons/delete_black.png" Style="display: none; cursor: pointer;
                                        height: 14px;" AlternateText="Filter Sites" ToolTip="Delete foreman" OnClientClick="return DeleteForemanClick();"
                                        OnClick="btnDeleteForeman_Click" />
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                        <center>
                            <!--Holds list of foremen-->
                            <table id="tblForemen" cellpadding="0" cellspacing="0" border="1" runat="server"
                                style="margin-top: 20px; margin-bottom: 20px; width: 96%;">
                                <tr class="ContainerHeader">
                                    <td align="center" style="font-family: Calibri; font-size: 12px">
                                        Foreman Name
                                    </td>
                                    <td align="center" style="font-family: Calibri; font-size: 12px">
                                        Telephone
                                    </td>
                                    <td align="center" style="font-family: Calibri; font-size: 12px">
                                        Cellphone
                                    </td>
                                    <td align="center" style="font-family: Calibri; font-size: 12px">
                                        Email
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                </dx:PopupControlContentControl>
            </ContentCollection>
        </dx:ASPxPopupControl>
        <!--Foreman edit popup box-->
        <dx:ASPxPopupControl ID="popupEditForemen" runat="server" ClientIDMode="AutoID" CssFilePath="~/App_Themes/RedWineCustom/{0}/styles.css"
            CssPostfix="RedWine" LoadingPanelImagePosition="Top" Modal="True" CloseAction="None"
            SpriteCssFilePath="~/App_Themes/RedWineCustom/{0}/sprite.css" PopupHorizontalAlign="WindowCenter"
            PopupVerticalAlign="WindowCenter" Style="margin-top: 0px; margin-right: 0px;"
            HeaderText="Foreman Editor" Width="532px">
            <ClientSideEvents CloseButtonClick="function(s, e) { document.getElementById('btnCancelEditForemenPopup').click();
	            return	false;
            }" />
            <LoadingPanelImage Url="~/App_Themes/RedWine/Web/Loading.gif">
            </LoadingPanelImage>
            <ContentStyle VerticalAlign="Top">
            </ContentStyle>
            <ContentCollection>
                <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server" SupportsDisabledAttribute="True">
                    <table>
                        <tr>
                            <td height="25px">
                                Foreman Name *:&nbsp;
                            </td>
                            <td height="25px">
                                <asp:TextBox ClientIDMode="Static" ID="txtForemanName" runat="server" CssClass="textboxBorder"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td height="25px">
                                Telephone:&nbsp;
                            </td>
                            <td height="25px">
                                <asp:TextBox ClientIDMode="Static" ID="txtForemanTelephone" runat="server" CssClass="textboxBorder"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td height="25px">
                                Cellphone:&nbsp;
                            </td>
                            <td height="25px">
                                <asp:TextBox ClientIDMode="Static" ID="txtForemanCellphone" runat="server" CssClass="textboxBorder"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td height="25px">
                                Email:&nbsp;
                            </td>
                            <td height="25px">
                                <asp:TextBox ClientIDMode="Static" ID="txtForemanEmail" runat="server" CssClass="textboxBorder"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <p>
                        <asp:Button ClientIDMode="Static" ID="btnSubmitEditForemenPopup" runat="server" OnClientClick="return EditForemenSubmit();"
                            Text="Submit" CssClass="btn_submit" Width="50" OnClick="btnSubmitEditForemenPopup_Click"
                            Style="display: none;" />
                        <asp:Button ClientIDMode="Static" ID="btnUpdateEditForemenPopup" runat="server" Text="Update"
                            OnClientClick="return EditForemanUpdate();" CssClass="btn_submit" Style="display: none;"
                            OnClick="btnUpdateEditForemenPopup_Click" />
                        <asp:Button ClientIDMode="Static" ID="btnCancelEditForemenPopup" runat="server" OnClientClick="return EditForemenCancel();"
                            Text="Cancel" CssClass="btn_submit" Width="50" OnClick="btnCancelEditForemenPopup_Click"
                            Style="display: none;" />
                    </p>
                </dx:PopupControlContentControl>
            </ContentCollection>
        </dx:ASPxPopupControl>
        <!--Foreman edit popup box-->
        <dx:ASPxPopupControl ID="popupClientFinder" runat="server" ClientIDMode="AutoID"
            CssFilePath="~/App_Themes/RedWineCustom/{0}/styles.css" CssPostfix="RedWine"
            LoadingPanelImagePosition="Top" Modal="True" CloseAction="CloseButton" SpriteCssFilePath="~/App_Themes/RedWineCustom/{0}/sprite.css"
            PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Style="margin-top: 0px;
            margin-right: 0px;" HeaderText="Client Finder" Width="640px">
            <LoadingPanelImage Url="~/App_Themes/RedWine/Web/Loading.gif">
            </LoadingPanelImage>
            <ContentStyle VerticalAlign="Top">
            </ContentStyle>
            <ContentCollection>
                <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server" SupportsDisabledAttribute="True">
                    <asp:UpdatePanel ID="panelClientFinder" runat="server" Visible="true" UpdateMode="Always"
                        ChildrenAsTriggers="true">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="txtClientFinderFilter" EventName="TextChanged" />
                            <asp:AsyncPostBackTrigger ControlID="btnClientSearch" EventName="Click" />
                            <asp:PostBackTrigger ControlID="gvClientDetails" />
                        </Triggers>
                        <ContentTemplate>
                            <!--Foreman table headers-->
                            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                <tr class="ContainerHeader">
                                    <td height="26px" style="font-size: 14px;" width="200">
                                        &nbsp;<b style="font-family: Calibri;"> Select a client</b>
                                    </td>
                                    <td height="26px" align="right">
                                        &nbsp;<asp:TextBox ClientIDMode="Static" ID="txtClientFinderFilter" value="" runat="server"
                                            CssClass="textBoxBorder" AutoPostBack="true" OnTextChanged="txtClientFinderFilter_TextChanged" />
                                        &nbsp;<asp:ImageButton runat="server" ClientIDMode="Static" ID="btnClientSearch"
                                            ImageUrl="../images/icons/search.png" Style="cursor: pointer" AlternateText="Filter Clients"
                                            ToolTip="Filter Clients" OnClick="btnClientSearch_Click" />
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <asp:GridView ID="gvClientDetails" AllowPaging="True" AllowSorting="False" DataKeyNames="PKiNewClientID"
                                AutoGenerateColumns="false" CellPadding="5" ForeColor="#333333" GridLines="Both"
                                DataSourceID="dsClientFinder" OnSelectedIndexChanged="gvClientDetails_SelectedIndexChanged"
                                runat="server" CellSpacing="0" ShowHeaderWhenEmpty="true" Width="100%" EnablePersistedSelection="false"
                                HorizontalAlign="Center" OnRowDataBound="gvClientDetails_RowDataBound" PageSize="20">
                                <%--//Formatting of Gridview----%>
                                <HeaderStyle CssClass="GridHeader" />
                                <AlternatingRowStyle CssClass="GridAlternate" />
                                <RowStyle CssClass="GridNormalRow" />
                                <EmptyDataRowStyle CssClass="GridEmptyRow" />
                                <EmptyDataTemplate>
                                    <asp:Label Style="display: block; width: 100%;" runat="server">Sorry no data found. Try a different filter.</asp:Label>
                                </EmptyDataTemplate>
                                <Columns>
                                    <asp:CommandField SelectText="Select" ShowSelectButton="true" ItemStyle-CssClass="HideButton"
                                        HeaderStyle-CssClass="HideButton" />
                                    <asp:BoundField DataField="PKiNewClientID" Visible="false" />
                                    <asp:BoundField DataField="sNewClientName" HeaderText="Client Name" />
                                    <asp:BoundField DataField="sNewClientContactPerson" HeaderText="Contact Person" />
                                    <asp:BoundField DataField="sNewClientTelephone" HeaderText="Telephone" />
                                    <asp:BoundField DataField="sNewClientCellphone" HeaderText="Cellphone" />
                                    <asp:BoundField DataField="sNewClientEmail" HeaderText="Email" />
                                </Columns>
                            </asp:GridView>
                            <asp:SqlDataSource ID="dsClientFinder" EnableCaching="true" DataSourceMode="DataSet"
                                runat="server" CacheExpirationPolicy="Absolute" SelectCommand="SELECT [PKiNewClientID], [sNewClientName], [sNewClientContactPerson], [sNewClientTelephone], [sNewClientCellphone], [sNewClientEmail] FROM [new_clients]"
                                CacheDuration="30" FilterExpression="sNewClientName LIKE '%{0}%' OR sNewClientContactPerson LIKE '%{0}%' OR sNewClientTelephone LIKE '%{0}%' OR sNewClientTelephone LIKE '%{0}%' OR sNewClientCellphone LIKE '%{0}%' OR sNewClientEmail LIKE '%{0}%' OR '' = '{0}'">
                                <FilterParameters>
                                    <asp:ControlParameter ControlID="txtClientFinderFilter" PropertyName="Text" DefaultValue=""
                                        ConvertEmptyStringToNull="false" />
                                </FilterParameters>
                            </asp:SqlDataSource>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </dx:PopupControlContentControl>
            </ContentCollection>
        </dx:ASPxPopupControl>
        </form>
    </div>
</body>
<script type="text/javascript">
    // jQuery Script to disable erroneous enter submit
    $(function () {
        $('input').keypress(function (e) {
            var code = null;
            code = (e.keyCode ? e.keyCode : e.which);
            return (code == 13) ? false : true;
        });
    }); 
</script>
</html>
