<%@ Page Language="C#" AutoEventWireup="true" CodeFile="list_existing_clients.aspx.cs" Inherits="pages_list_existing_clients" %>


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../css/ThemeRed.css" rel="stylesheet" type="text/css" />
    <title>Existing Client List</title>

    <script>
        function SelectClient(sID) {
            window.location = 'existing_client_info.aspx?sClientID=' + sID;
        }

        function ClientFilter() {
            document.getElementById("btnFilterClients").click();
        }

        function FilterResults(e) {
            e = e || window.event;
            var code = e.keyCode || e.which;

            if (code == 13)
                ClientFilter();
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <asp:Button ID="btnFilterClients" runat="server" style="display:none;width:50px" onclick="btnFilterClients_Click"/>
                
    <div align=center>
        <%--HEADER LINE TABLES--%>
        <table cellpadding=0 cellspacing=0 border=0 width=100%>
            <tr class="ContainerHeader">
                <td height=26px style="font-size:14px;" colspan=2>&nbsp;<b> Existing Clients</b></td>
                <td height=26px align=right>
                            &nbsp;<input type=text id=txtFilter onkeypress="return FilterResults(event);" value="" runat=server class="textboxBorder"/>
                            &nbsp;<img src="../images/icons/search.png" style="cursor:pointer" title="Click here to filter client list" onclick="ClientFilter()"/>&nbsp;&nbsp;                            
                </td>                                                                                                          
            </tr>
        </table>

        <%--EXISTING TABLE OF TASKS--%>
        <table id=tblExistingUsers cellpadding="0" cellspacing="0" border="1" width=75% runat=server style="margin-top:20px;margin-left:30px;margin-bottom:20px">                    
            <tr class="ContainerHeader">
                <td align=center height=25px width=75px>Client Number</td>                
                <td align=center height=25px width=75px>Name</td>                                               
                <td align=center height=25px width=125>Email</td>                
                <td align=center height=25px width=100>Contact Person</td>                
            </tr>
        </table>
    
    </div>
    </form>
</body>
</html>
