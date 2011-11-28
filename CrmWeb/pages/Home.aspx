<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Home.aspx.cs" Inherits="pages_Home" %>

<%@ Register Assembly="DevExpress.Web.v10.2, Version=10.2.6.0"
    Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>TB-CRM Home</title>
    <link href="../css/StyleSheet.css" rel="stylesheet" type="text/css" />    
    <link href="../css/ThemeRed.css" rel="stylesheet" type="text/css" />

</head>
<body topmargin=0px bgcolor="AliceBlue" >
    <form id="form1" runat="server">
    
    <div>
        <dx:ASPxMenu ID="ASPxHomeMenu" runat="server" AllowSelectItem="True" EnableTheming=True  
            AutoSeparators="RootOnly" ClientIDMode="AutoID" 
            CssFilePath="~/App_Themes/RedWineCustom/{0}/styles.css" 
            CssPostfix="RedWine" 
            SpriteCssFilePath="~/App_Themes/RedWineCustom/{0}/sprite.css" 
            GutterImageSpacing="7px" GutterWidth="16px">
            <Items>
                <dx:MenuItem Name="home" Text="Home" Target="frmMain" NavigateUrl="dashboard.aspx"></dx:MenuItem> 
                <dx:MenuItem Name="eventstack" Text="Events" Target="frmMain" NavigateUrl="eventstack.aspx"></dx:MenuItem>              
                <dx:MenuItem Name="clients_main" Text="Clients">
                    <Items>
                        <dx:MenuItem Name="possible_clients" Text="Possible Clients" Target="frmMain" NavigateUrl="new_clients.aspx"></dx:MenuItem>
                        <dx:MenuItem Name="existing_clients" Text="Existing Clients" Target="frmMain">
                            <Items>
                                <dx:MenuItem Name="existing_clients_list" Text="View Client List" NavigateUrl="list_existing_clients.aspx" Target="frmMain"></dx:MenuItem>
                            </Items>
                        </dx:MenuItem>                        
                    </Items>
                </dx:MenuItem>                                

                <dx:MenuItem Name="setup" Text="Setup">  
                    <Items>                        
                        <dx:MenuItem Name="setup_users" Text="Users" NavigateUrl="new_user.aspx" Target="frmMain"></dx:MenuItem>
                    </Items>                      
                </dx:MenuItem>
                
                <dx:MenuItem Name="log_off" Text="Log Off" NavigateUrl="../Default.aspx"></dx:MenuItem>                                                                  

                <dx:MenuItem Name="print_lists" Text="Print Tasks" NavigateUrl="assignmentlist.aspx"></dx:MenuItem>
            </Items>

            <LoadingPanelImage Url="~/App_Themes/RedWineCustom/Web/Loading.gif"></LoadingPanelImage>            
            
            <ItemSubMenuOffset FirstItemX="2" LastItemX="2" X="2" FirstItemY="-2" LastItemY="-2" Y="-2" />               
            <RootItemSubMenuOffset FirstItemY="2" LastItemY="2" Y="2" />
            <ItemStyle DropDownButtonSpacing="10px" />
            <SubMenuStyle GutterWidth="0px" />

        </dx:ASPxMenu>
    </div>

    <br />
    <div>
        <iframe id=frmMain name=frmMain height="90%" width="100%" frameborder="0" src="dashboard.aspx" style="background-color:AliceBlue;"></iframe>
    </div>
    </form>
</body>
</html>
