<%@ Page Language="C#" AutoEventWireup="true" CodeFile="new_user.aspx.cs" Inherits="pages_new_user" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../css/ThemeRed.css" rel="stylesheet" type="text/css" />    
    <title>User Management</title>    
        
    <script type="text/javascript">

        function SubmitInfo() 
        {
            if (document.getElementById("txtName").value == "") {
                alert("Please provide the name for the user.");
                document.getElementById("txtName").focus();
            }
            else if (document.getElementById("txtSurname").value == "") {
                alert("Please provide the surname for the user.");
                document.getElementById("txtSurname").focus();
            }
            else if (document.getElementById("txtPhoneNumber").value == "") {
                alert("Please provide the phone number for the user.");
                document.getElementById("txtPhoneNumber").focus();
            }
            else if (document.getElementById("txtEmail").value == "") {
                alert("Please provide the email for the user.");
                document.getElementById("txtEmail").focus();
            }
            else if (document.getElementById("txtUsername").value == "") {
                alert("Please provide the username for the user.");
                document.getElementById("txtUsername").focus();
            }
            else if (document.getElementById("txtPass").value == "") {
                alert("Please provide the password for the user.");
                document.getElementById("txtPass").focus();
            }
            else if (document.getElementById("txtColorCode").value == "") {
                alert("Please provide the color code for the user.");
                document.getElementById("txtColorCode").focus();
            }
            else if (document.getElementById("selUserType").value == "") {
                alert("Please select the user type.");
                document.getElementById("selUserType").focus();
            }
            else 
            {
                document.getElementById("btnSaveUser").click();
            }
        }

        function SelectUser(sId, sName, sSurname, sEmail, sPhoneNumber, sUsername, sPassword, sColorCode, sType) {
            document.getElementById("txtUserId").value = sId;
            document.getElementById("txtUserId").onchange();

            document.getElementById("txtName").value = sName;
            document.getElementById("txtSurname").value = sSurname;
            document.getElementById("txtEmail").value = sEmail;
            document.getElementById("txtPhoneNumber").value = sPhoneNumber; 

            document.getElementById("txtUsername").value = sUsername;
            document.getElementById("txtPass").value = sPassword;

            document.getElementById("txtColorCode").style.color = "#" + sColorCode;
            document.getElementById("txtColorCode").value = sColorCode;

            document.getElementById("selUserType").value = sType;
        }

        function ClearFields() {
            document.getElementById("txtUserId").value = "";
            document.getElementById("txtUserId").onchange();

            document.getElementById("txtName").value = "";
            document.getElementById("txtSurname").value = "";
            document.getElementById("txtEmail").value = "";
            document.getElementById("txtPhoneNumber").value = ""; 

            document.getElementById("txtUsername").value = "";
            document.getElementById("txtPass").value = "";

            document.getElementById("txtColorCode").value = "";
            document.getElementById("txtColorCode").value = "#000000";

            document.getElementById("selUserType").value = "System";
        }

        function UserFilter() {
            document.getElementById("btnFilterUser").click();
        }

        function FilterResults(e) {
            e = e || window.event;
            var code = e.keyCode || e.which;

            if (code == 13)
                UserFilter();
        }

        function ShowHideDelete() 
        {
            var sID = document.getElementById("txtUserId").value;

            if (sID == "")
                document.getElementById("btnRemoveUser").style.visibility = "hidden"
            else
                document.getElementById("btnRemoveUser").style.visibility = "visible"
        }

        function RemoveUser() {
            if (confirm("Are you sure you want to remove this user")) {
                document.getElementById("btnDelUser").click();
                ClearFields();
            }
        }

        function colorChanged(sender) {

            document.getElementById("txtColorCode").style.color = "#" + sender.get_selectedColor();
            document.getElementById("txtColorCode").style.backgroundColor = "#" + sender.get_selectedColor();            
            //sender.get_element().style.color = "#" + sender.get_selectedColor();
            //sender.get_element().style. = "#" + sender.get_selectedColor(); 
        }              
    </script>

</head>
<body class="body">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <input type=text id="txtUserId" runat=server onchange="ShowHideDelete()" style="display:none;width:50px"/>

    <asp:Button ID="btnSaveUser" runat="server" onclick="btnSaveUser_Click" style="display:none;width:50px"/>
    <asp:Button ID="btnFilterUser" runat="server" onclick="btnFilterUser_Click" style="display:none;width:50px"/>
    <asp:Button ID="btnDelUser" runat="server" onclick="btnDelUser_Click" style="display:none;width:50px"/>     
    
    <div>
    
    <table cellpadding="0" cellspacing="0" border="1" class="holder_table">
        <tr>
            <td valign=top width=350px>

                <%--HEADER LINE TABLES--%>
                <table cellpadding=0 cellspacing=0 border=0 width=100%>
                    <tr class="ContainerHeader">
                        <td height=26px style="font-size:14px;">&nbsp;<b> Add/Edit Users</b></td>                                                                                                         
                    </tr>
                </table>                

                <%--EDITING TABLE OF USERS--%>
                <table cellpadding="0" cellspacing="0" border="0" width=330px>                                         
                    <tr>
                        <td height=25px colspan="2">&nbsp * Indicates required fields.</td>
                    </tr>
                    <tr>
                        <td height=25px width=150px>&nbsp First Name: *</td>
                        <td><input type=text id=txtName name=txtName runat=server class="textBoxBorder" /></td>
                    </tr> 
                    <tr>
                        <td height=25px>&nbsp Last Name: *</td>
                        <td><input type=text id=txtSurname name=txtSurname runat=server class="textBoxBorder" /></td>
                    </tr>
                    <tr>
                        <td height=25px>&nbsp Phone Number: *</td>
                        <td><input type=text id=txtPhoneNumber name=txtPhoneNumber runat=server class="textBoxBorder" /></td>                
                    </tr>   
                    <tr>
                        <td height=25px>&nbsp Email: *</td>
                        <td><input type=text id=txtEmail name=txtEmail runat=server class="textBoxBorder" /></td>                
                    </tr>                   

                    <tr>
                        <td height=25px colspan=2>&nbsp</td>                
                    </tr>
                    <tr>
                        <td height=25px>&nbsp Username:* </td>
                        <td><input type=text id=txtUsername name=txtUsername runat=server class="textBoxBorder" /></td>
                    </tr>
                    <tr>
                        <td height=25px>&nbsp Password:* </td>
                        <td><input type=password id=txtPass name=txtPass runat=server class="textBoxBorder" /></td>
                    </tr>
                    <tr>
                        <td height=25px>&nbsp User Type:* </td>
                        <td>
                            <select id="selUserType" runat="server" name="cbUserType">
                                <option value="System" selected="true">System</option>
                                <option value="Admin">Admin</option>
                                <option value="Marketer">Marketer</option>
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td height=25px>&nbsp Color Code:* </td>
                        <td>
                            <asp:TextBox ClientIDMode="Static"  ID="txtColorCode" runat="server" class="textBoxBorder" MaxLength=7></asp:TextBox>                            
                            <input type=image id=btnColorPicker name=btnColorPicker src="../images/icons/cp_button.png" alt="Click to show color picker"/>

                            <asp:ColorPickerExtender ID="ColorPickerExtender1" runat="server"
                            TargetControlID="txtColorCode"    
                            PopupButtonID="btnColorPicker"  
                            OnClientColorSelectionChanged="colorChanged">                            
                            </asp:ColorPickerExtender>
                        </td>
                        
                    </tr>
                    
                    <tr>
                        <td height=25px colspan=2>&nbsp</td>                
                    </tr>
                    <tr>
                        <td height=25px>&nbsp</td>                
                        <td height=25px>
                            <input type=button id=btnRemoveUser name=btnRemoveUser value="Delete"  onclick="RemoveUser()" style="visibility:hidden" class="btn_delete"/>
                            <input type=button id=btnClearFields name=btnClearFields value="Clear" style="width:50px;" onclick="ClearFields()" class="btn_clear"/>
                            <input type=button id=btnSubmitUser name=btnSubmitUser value="Submit" style="width:50px;" onclick="SubmitInfo()" class="btn_submit"/>                               
                        </td>                
                    </tr> 
                    <tr>
                        <td height=25px colspan=2>&nbsp</td>                
                    </tr>           
                </table>

            </td>

            <td valign=top width=560px>

                <%--HEADER LINE TABLES--%>
                <table cellpadding=0 cellspacing=0 border="0" width=100%>                    
                    <tr class="ContainerHeader">
                        <td height=26px style="font-size:14px;">&nbsp;<b> Current Users</b></td>                
                        <td height=26px align=right>
                            &nbsp;<input type=text id=txtFilter onkeypress="return FilterResults(event);" value="" runat=server class="textBoxBorder"/>
                            &nbsp;<img src="../images/icons/search.png" style="cursor:pointer" title="Click here to filter users" onclick="UserFilter()"/>&nbsp;                            
                        </td>                
                    </tr>
                    
                </table>

                <%--EXISTING TABLE OF USERS--%>
                <table id=tblUsers cellpadding="0" cellspacing="0" border="1" width=500px runat=server style="margin-top:20px;margin-left:30px;margin-bottom:20px">                    
                    <tr class="ContainerHeader">
                        <td align=center width=150px>First Name</td>
                        <td align=center width=150px>Last Name</td>  
                        <td align=center width=200px>Phone Number</td>                              
                        <td align=center width=200px>Email</td>                                                     
                        <td align=center width=200px>Type</td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>

    </div>

    </form>
</body>
</html>
