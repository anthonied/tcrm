<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<%@ Register Assembly="DevExpress.Web.v10.2, Version=10.2.6.0"
    Namespace="DevExpress.Web.ASPxRoundPanel" TagPrefix="dx" %>

<%@ Register assembly="DevExpress.Web.v10.2, Version=10.2.6.0" namespace="DevExpress.Web.ASPxPanel" tagprefix="dx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>TB-CRM</title>    
    <link href="css/StyleSheet.css" rel="stylesheet" type="text/css" />    
    <link href="css/ThemeRed.css" rel="stylesheet" type="text/css" />

</head>
<body>
    <form id="form1" runat="server">                
    <br /><br /><br /><br /><br /><br /><br /><br /><br />
    
    <div align="center">
    <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" Width="200px"
        HeaderText="TB-CRM Login" ClientIDMode="AutoID" 
        CssFilePath="~/App_Themes/RedWineCustom/{0}/styles.css" CssPostfix="RedWine" 
        GroupBoxCaptionOffsetY="-28px" 
        SpriteCssFilePath="~/App_Themes/RedWineCustom/{0}/sprite.css">
        
        <ContentPaddings Padding="14px" />
        
        <PanelCollection>
            <dx:PanelContent>
                <table>
                    <tr>
                        <td height=25px>Username:&nbsp;</td>
                        <td height=25px><input type=text id=txtUsername name=txtUsername runat=server class="textBoxBorder" />                                                                
                        </td>
                    </tr>
                    <tr>
                        <td height=25px>Password:&nbsp;</td>
                        <td height=25px><input type=password id=txtPass name=txtPass runat=server class="textBoxBorder" />                                                                                                                                                                                               
                        </td>
                    </tr>
                    <tr>
                        <td height=25px>&nbsp;</td>
                        <td height=25px align=right><asp:Button ID="btnLogin" runat="server" Text="Login" 
                                OnClick="btnLogin_Click" /></td>                                
                    </tr>                        
                 </table>
            </dx:PanelContent>
        </PanelCollection>
        
        </dx:ASPxRoundPanel>
    </div>
             
    </form>
</body>
</html>