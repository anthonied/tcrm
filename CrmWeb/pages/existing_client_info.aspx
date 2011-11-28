<%@ Page Language="C#" AutoEventWireup="true" CodeFile="existing_client_info.aspx.cs" Inherits="pages_existing_client_info" %>

<%@ Register Assembly="DevExpress.Web.v10.2, Version=10.2.6.0"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.v10.2, Version=10.2.6.0"
    Namespace="DevExpress.Web.ASPxTabControl" TagPrefix="dx" %>

<%@ Register assembly="DevExpress.Web.v10.2, Version=10.2.6.0" 
    Namespace="DevExpress.Web.ASPxClasses" tagprefix="dx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../css/ThemeRed.css" rel="stylesheet" type="text/css" />
    <title>Client Information</title>
    
    <script>
        function confirmCheck(id) {
            document.getElementById("hdnMarketer" + id.toString()).value = document.getElementById("chkMarketer" + id.toString()).checked.toString();
            return true;
        }
        function SelectClient(sID) {
            window.location = 'existing_client_info.aspx?sClientID=' + sID;
            HideFinderPopup();
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

        function ShowFinderPopup() 
        {            
            //Show
            ASPxClientFinderPopup.Show();
        }

        function HideFinderPopup() 
        {
            //Hide
            ASPxClientFinderPopup.Hide();
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <input type=text id="txtClientId" runat=server style="display:none;width:50px"/>
    <asp:Button ID="btnFilterClients" runat="server" style="display:none;width:50px" onclick="btnFilterClients_Click" />
        
    <%--******************** START OF DEBTORS INFO ***********************--%>
    
    <div>
        <dx:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="0" 
            ClientIDMode="AutoID" CssFilePath="~/App_Themes/RedWineCustom/Web/styles.css" 
            CssPostfix="RedWine" SpriteCssFilePath="~/App_Themes/RedWineCustom/Web/sprite.css" 
            TabSpacing="3px">
        <TabPages>
            <dx:TabPage Text="Debtors Info">
            <ContentCollection>
                <dx:ContentControl runat="server" >  

                       <div>
            <table cellpadding="0" cellspacing="0" border="0" width=900px>
                <tr>
                    <td height=25px width=150px><asp:Image ID="imgClientPhoto" runat="server" ImageUrl="~/images/client_images/no_photo_default.jpg" /></td>                    
                    <td colspan=3>&nbsp Client #: &nbsp;&nbsp;
                        <input type=text id=txtClientNumber name=txtClientNumber runat=server class="textboxBorderRO"/>
                        &nbsp;&nbsp;<input type=button value="" title="Client Finder" style="background: url('../images/icons/search_big.png');background-repeat: no-repeat;background-position:center;width:20px;height:20px;border-style:none;" onclick="ShowFinderPopup()"/>                    
                    </td>
                    <td>&nbsp;</td>                                                                                                                    
                </tr>

                <tr>
                    <td height=25px colspan="5">&nbsp;</td>
                </tr>
                <tr>
                    <td height=auto width=150px valign=top>&nbsp Persons Responsible: </td>
                    <td height=auto width=600px colspan=2>
                        <table cellpadding="0" cellspacing="0" border="0" width="100%" runat="server" id="tblMarketers">     
                        </table>            
                    </td>                                     
                </tr>

                <tr>
                    <td height=25px width=150px>&nbsp Active: </td>
                    <td><input type=checkbox id=chkUserActive name=chkUserActive runat=server class="std_chkbox" />                 
                    </td>

                    <td height=25px width=150px>&nbsp Exclude: </td>
                    <td><input type=checkbox id=chkUserExclude name=chkUserExclude runat=server class="std_chkbox"/>   &nbsp <asp:Button ID="btnUpdate" runat="server" Text="Save" OnClick="btnUpdate_Click"/>                 
                    </td>                    
                </tr>

                <tr>
                    <td height=25px colspan="5">&nbsp;</td>
                </tr>

                <tr>
                    <td height=25px width=150px>&nbsp Client Name: </td>
                    <td><input type=text id=txtName name=txtName runat=server class="textboxBorderRO" />                    
                    </td>

                    <td height=25px width=150px>&nbsp Address Line 1: </td>
                    <td><input type=text id=txtAddLine1 name=txtAddLine1 runat=server class="textboxBorderRO" />                    
                    </td>
                    <td height=25px width=350px rowspan=17 valign=top align=center>
                        <asp:GridView ID="dgvPayments" Width="280px" runat="server" AutoGenerateColumns="False" 
                            CellPadding="5" ForeColor="#333333" GridLines="Both">                        
                        
                            <Columns>
                                <asp:BoundField HeaderText="Trans Date" DataField="PMTDate"/>
                                <asp:BoundField HeaderText="Doc Num" DataField="sDocNum" />
                                <asp:BoundField HeaderText="Amount" DataField="dPMTAmount"/>
                            </Columns>
                        
                            <%--//Formatting of Gridview----%>                        
                            <HeaderStyle CssClass="GridHeader" />
                            <AlternatingRowStyle CssClass="GridAlternate" />
                            <RowStyle CssClass="GridNormalRow" />
                            <EmptyDataRowStyle CssClass="GridEmptyRow" />                                                                                                                  

                        </asp:GridView>                    
                    </td>                                               
                </tr> 
                <tr>
                    <td height=25px>&nbsp Telephone: </td>
                    <td><input type=text id=txtTelephone name=txtTelephone runat=server class="textboxBorderRO" />                    
                    </td>
                
                    <td height=25px width=150px>&nbsp Address Line 2: </td>
                    <td><input type=text id=txtAddLine2 name=txtAddLine2 runat=server class="textboxBorderRO" />                    
                    </td>                                
                </tr>
                <tr>
                    <td height=25px>&nbsp Fax: </td>
                    <td><input type=text id=txtFax name=txtFax runat=server class="textboxBorderRO" />                    
                    </td>                
                                
                    <td height=25px width=150px>&nbsp Address Line 3: </td>
                    <td><input type=text id=txtAddLine3 name=txtAddLine3 runat=server class="textboxBorderRO" />                    
                    </td>
                </tr>
                <tr>
                    <td height=25px>&nbsp Email: </td>
                    <td><input type=text id=txtEmail name=txtEmail runat=server class="textboxBorderRO"/>                    
                    </td>                
                
                    <td height=25px width=150px>&nbsp Postal Code: </td>
                    <td><input type=text id=txtPostalCode name=txtPostalCode runat=server class="textboxBorderRO"/>                    
                    </td>
                </tr> 
                <tr>
                    <td height=25px colspan="4"></td>
                </tr>

                <tr>
                    <td height=25px>&nbsp Contact Person: </td>
                    <td><input type=text id=txtContactP name=txtContactP runat=server class="textboxBorderRO"/>                    
                    </td>                
                                
                    <td height=25px width=150px>&nbsp Sales Person: </td>
                    <td><input type=text id=txtSalesP name=txtSalesP runat=server class="textboxBorderRO"/>                    
                    </td>
                </tr>
                <tr>
                    <td height=25px>&nbsp Category: </td>
                    <td>
                        <select id=selCategory name=selCategory runat=server class="std_dropdown">
                        </select>
                    </td>                
                                
                    <td height=25px width=150px>&nbsp Client Office: </td>
                    <td>
                        <select id=selClientOffice name=selClientOffice runat=server class="std_dropdown">
                        </select>
                    </td>
                </tr>
                <tr>
                    <td height=25px colspan="4"></td>
                </tr>

                <tr>
                    <td height=25px>&nbsp Credit Limit: </td>
                    <td><input type=text id=txtCreditLimit name=txtCreditLimit runat=server class="textboxBorderRO"/>                    
                    </td>                
                                
                    <td height=25px width=150px>&nbsp Date of Application: </td>
                    <td><input type=text id=txtDOA name=txtDOA runat=server class="textboxBorderRO"/>                    
                    </td>
                </tr>
                <tr>
                    <td height=25px>&nbsp Outstanding Balance: </td>
                    <td><input type=text id=txtOutstBal name=txtOutstBal runat=server class="textboxBorderRO"/>                    
                    </td>                
                                
                    <td height=25px width=150px>&nbsp Credit Terms: </td>
                    <td><input type=text id=txtCreditTerms name=txtCreditTerms runat=server class="textboxBorderRO" />                   
                    </td>
                </tr>

                <tr>
                    <td height=25px>&nbsp Blocked Account: </td>
                    <td><input type=checkbox id=chkBlockedAccount name=chkBlockedAccount runat=server class="std_chkbox"/>                    
                    </td>                
                                
                    <td height=25px width=150px>&nbsp Warranty Signed: </td>
                    <td><input type=checkbox id=chkWarrantySigned name=chkWarrantySigned runat=server class="std_chkbox"/>                    
                    </td>
                </tr>
           
                <tr>
                    <td height=25px colspan="4"></td>
                </tr>

                <tr>
                    <td height=25px>&nbsp Forms Completed: </td>
                    <td><input type=checkbox id=chkFormsCompleted name=chkFormsCompleted runat=server class="std_chkbox"/>                    
                    </td>                
                                
                    <td height=25px width=150px>&nbsp COD Form Approved: </td>
                    <td><input type=checkbox id=chkCODApproved name=chkCODApproved runat=server class="std_chkbox"/>                                        
                    </td>
                </tr>
                <tr>
                    <td height=25px>&nbsp Official Order: </td>
                    <td colspan=3>
                        <select id=selOfficialOrder name=selOfficialOrder class="std_dropdown" runat=server>
                            <option value="Option 1">Option 1</option>
                            <option value="Option 2">Option 2</option>
                            <option value="Option 3">Option 3</option>
                        </select>
                    </td>                                                                
                </tr>
            
                <tr>
                    <td height=25px colspan="4">&nbsp;</td>
                </tr>

                <tr>
                    <td height=25px>&nbsp Contact Person: </td>
                    <td><input type=text id=txtAccContactPerson name=txtAccContactPerson runat=server class="textboxBorderRO"/>                    
                    </td>                
                                
                    <td height=25px width=150px>&nbsp Contact Number: </td>
                    <td><input type=text id=txtAccContactNum name=txtAccContactNum runat=server class="textboxBorderRO" />                    
                    </td>
                </tr>
                <tr>                                                                
                    <td height=25px width=150px>&nbsp Birthday Date: </td>
                    <td colspan=3><input type=text id=txtDOB name=txtDOB runat=server class="textboxBorderRO" />                    
                    </td>
                </tr>
            </table>
            </div> 

                </dx:ContentControl>
            </ContentCollection>
            </dx:TabPage>

            <dx:TabPage Text="Company Info">
            <ContentCollection>
                <dx:ContentControl ID="ContentControl1" runat="server" >


                    <div>
    <table cellpadding="0" cellspacing="0" border="0" width=900px>
        <tr>
            <td height=25px width=150px><asp:Image ID="imgClientPhotoComp" runat="server" ImageUrl="~/images/client_images/no_photo_default.jpg" /></td>                    
            <td colspan=3>&nbsp Client #: &nbsp;&nbsp;
                <input type=text id=txtClientNumCompany name=txtClientNumCompany runat=server class="textboxBorderRO"/>
                &nbsp;&nbsp;<input type=button value="" title="Client Finder" style="background: url('../images/icons/search_big.png');background-repeat: no-repeat;background-position:center;width:20px;height:20px;border-style:none;" onclick="ShowFinderPopup()"/>                    
            </td>
            <td>&nbsp;</td>                                                                                                                    
        </tr>
        <tr>
            <td height=25px colspan="5">&nbsp;</td>
        </tr>

        <tr>
            <td height=25px width=150px>&nbsp; Construction Type: </td>
            <td width=150px>
                <select id=selConstructType name=selConstructType runat=server class="std_dropdown">
                    <option value="Option 1">Option 1</option>
                    <option value="Option 2">Option 2</option>
                    <option value="Building Construction">Building Construction</option>
                </select>                    
            </td>
                
            <td height=25px width=150px>&nbsp; Physical Address: </td>
            <td><input type=text id=txtPhysAddLine1 name=txtPhysAddLine1 runat=server class="textboxBorderRO" />                    
            </td>                                
        </tr>

        <tr>
            <td height=25px>&nbsp; Opposition Plant & Tool: </td>
            <td><select id=selOppPlantTool name=selOppPlantTool runat=server class="std_dropdown">
                </select>                    
            </td>
                
            <td height=25px width=150px>&nbsp; Address Line 2: </td>
            <td><input type=text id=txtPhysAddLine2 name=txtPhysAddLine2 runat=server class="textboxBorderRO" />                    
            </td>                                
        </tr>

        <tr>
            <td height=25px>&nbsp; Opposition TSU: </td>
            <td><select id=selOppTSU name=selOppTSU runat=server class="std_dropdown">
                </select>                    
            </td>
                
            <td height=25px width=150px>&nbsp; Address Line 3: </td>
            <td><input type=text id=txtPhysAddLine3 name=txtPhysAddLine3 runat=server class="textboxBorderRO" />                    
            </td>                                
        </tr>

         <tr>
            <td height=25px>&nbsp; Marketer: </td>
            <td><select id=selMarketer name=selMarketer runat=server class="std_dropdown">
                </select>                    
            </td>
                
            <td height=25px width=150px>&nbsp; Code: </td>
            <td><input type=text id=txtCCode name=txtCCode runat=server class="textboxBorderRO" />                    
            </td>                                
        </tr>

        <tr>
            <td height=25px colspan="5">&nbsp;</td>
        </tr>
   
	    <tr>
		    <td height=25px>&nbsp; D M S (Latitude):</td>
		    <td width=160px>
			    <input id=txtCDMSLat1 name="txtCDMSLat1" size="4" maxlength="3" runat=server/>&deg;
			    <input id=txtCDMSLat2 name="txtCDMSLat2" size="4" maxlength="2" runat=server/>'
			    <input id=txtCDMSLat3 name="txtCDMSLat3" size="4" maxlength="5" runat=server/>"
		    </td>

            <td height=25px>&nbsp; D M S (Longitude):</td>
		    <td width=160px>
			    <input id=txtCDMSLon1 name="txtCDMSLon1" size="4" maxlength="3" runat=server/>&deg;
			    <input id=txtCDMSLon2 name="txtCDMSLon2" size="4" maxlength="2" runat=server/>'
			    <input id=txtCDMSLon3 name="txtCDMSLon3" size="4" maxlength="5" runat=server/>"
		    </td>
	    </tr>

        <tr>
			<td height=25px width=150px>&nbsp; Decimal (Latitude):</td>
			<td>
				<input id=txtCDMSDecLat name="txtCDMSDecLat" maxlength="50" runat=server/>
				<input type="hidden" name="zm" />
			</td>

            <td height=25px>&nbsp; Decimal (Longitude):</td>
            <td width=150px>
				<input id=txtCDMSDecLon name="txtCDMSDecLon" maxlength="50" runat=server/>
			</td>
		</tr>
        <tr>
            <td height=25px>&nbsp; Discount (Equipment): </td>
            <td><input type=text id=txtDiscountEq name=txtCCode runat=server class="textboxBorderRO" /> %                     
            </td>
                
            <td height=25px width=150px>&nbsp; Discount (Structure Trp): </td>
            <td><input type=text id=txtDiscountStruct name=txtDiscountStruct runat=server class="textboxBorderRO" /> %                   
            </td>                                
        </tr>
        <tr>
            <td height=25px>&nbsp; Insurance: </td>
            <td colspan=3><input type=checkbox id=chkHasInsurance name=chkHasInsurance runat=server class="std_chkbox"/>                    
            </td>                                                         
        </tr>
         <tr>
            <td height=25px colspan="5">&nbsp;</td>
        </tr>

        <tr>
            <td height=25px colspan=2 rowspan=3>&nbsp; SOME IMAGE GOES HERE </td>
                            
            <td height=25px>&nbsp; Vertical Up Client: </td>
            <td colspan=3><input type=checkbox id=chkIsVerticalUpClient name=chkIsVerticalUpClient runat=server class="std_chkbox"/>                    
            </td>                                
        </tr>
        <tr>                                        
            <td height=25px>&nbsp; Interested in Demo: </td>
            <td colspan=3><input type=checkbox id=chkIsInterestedInDemo name=chkIsInterestedInDemo runat=server class="std_chkbox"/>                    
            </td>                                
        </tr>
        <tr>                                        
            <td height=25px>&nbsp; Possible Demo Date: </td>
            <td colspan=3><input type=text id=txtCDemoDate name=txtCDemoDate runat=server class="textboxBorderRO" />
            </td>                                
        </tr>
        <tr>
            <td height=25px colspan="5">&nbsp;</td>
        </tr>

        <tr>
            <td height=25px>&nbsp; Marketing Strategy: </td>
            <td colspan=3>
                <select id=selMarketingStrategy name=selMarketingStrategy runat=server class="std_dropdown">
                    <option value="Visit">Visit</option>
                    <option value="Biltong">Biltong</option>
                    <option value="Whiskey">Whiskey</option>
                    <option value="Entertain">Entertain</option>
                    <option value="Phone Call">Phone Call</option>
                </select>                    
            </td>                                                           
        </tr>
        <tr>
            <td height=25px>&nbsp; Client Obtained Method: </td>
            <td colspan=3>
                <select id=selClientObMethod name=selClientObMethod runat=server class="std_dropdown">
                    <option value="Other">Other</option>
                    <option value="Outside Branch">Outside Branch</option>
                    <option value="DMC">DMC</option>
                    <option value="Call In">Call In</option>
                    <option value="Walk In">Walk In</option>
                    <option value="TAS">TAS</option>
                    <option value="Via Existing Client">Via Existing Client</option>
                    <option value="Via Other Client">Via Other Client</option>
                </select>                    
            </td>                                                           
        </tr>
        <tr>
            <td height=25px colspan="5">&nbsp;</td>
        </tr>

        <tr>
            <td height=25px>&nbsp; Rentals Locations: </td>
            <td colspan=3>
                <select id=selRentalsLocation name=selRentalsLocation runat=server class="std_dropdown">
                    <option value="Buyer">Buyer</option>
                    <option value="Site">Site</option>
                    <option value="Both">Both</option>                    
                </select>                    
            </td>                                                           
        </tr>

        <tr>
            <td height=25px colspan="5">&nbsp;</td>
        </tr>

        <tr>
            <td height=25px>&nbsp; Owner Name: </td>
            <td><input type=text id=txtOwnerName name=txtOwnerName runat=server class="textboxBorderRO" />                                       
            </td>
                
            <td height=25px width=150px>&nbsp; Cellphone Number: </td>
            <td><input type=text id=txtOwnerCell name=txtOwnerCell runat=server class="textboxBorderRO" />                    
            </td>                                
        </tr>
        <tr>
            <td height=25px>&nbsp; Owner Surname: </td>
            <td><input type=text id=txtOwnerSurname name=txtOwnerSurname runat=server class="textboxBorderRO" />                                       
            </td>
                
            <td height=25px width=150px>&nbsp; Birthdate: </td>
            <td><input type=text id=txtOwnerDOB name=txtOwnerDOB runat=server class="textboxBorderRO" />                    
            </td>                                
        </tr>
        <tr>
            <td height=25px>&nbsp; Email: </td>
            <td><input type=text id=txtOwnerEmail name=txtOwnerEmail runat=server class="textboxBorderRO" />                                       
            </td>
                
            <td height=25px width=150px>&nbsp; Hobbies/Interests: </td>
            <td><input type=text id=txtOwnerHobbies name=txtOwnerHobbies runat=server class="textboxBorderRO" />                    
            </td>                                
        </tr>
        <tr>
            <td height=25px colspan="5">&nbsp;</td>
        </tr>

                <tr>
            <td height=25px>&nbsp; Buyer Name: </td>
            <td><input type=text id=txtBuyerName name=txtBuyerName runat=server class="textboxBorderRO" />                                       
            </td>
                
            <td height=25px width=150px>&nbsp; Cellphone Number: </td>
            <td><input type=text id=txtBuyerCell name=txtBuyerCell runat=server class="textboxBorderRO" />                    
            </td>                                
        </tr>
        <tr>
            <td height=25px>&nbsp; Buyer Surname: </td>
            <td><input type=text id=txtBuyerSurname name=txtBuyerSurname runat=server class="textboxBorderRO" />                                       
            </td>
                
            <td height=25px width=150px>&nbsp; Birthdate: </td>
            <td><input type=text id=txtBuyerDOB name=txtBuyerDOB runat=server class="textboxBorderRO" />                    
            </td>                                
        </tr>
        <tr>
            <td height=25px>&nbsp; Email: </td>
            <td><input type=text id=txtBuyerEmail name=txtBuyerEmail runat=server class="textboxBorderRO" />                                       
            </td>
                
            <td height=25px width=150px>&nbsp; Hobbies/Interests: </td>
            <td><input type=text id=txtBuyerHobbies name=txtBuyerHobbies runat=server class="textboxBorderRO" />                    
            </td>                                
        </tr>
        <tr>
            <td height=25px colspan="5">&nbsp;</td>
        </tr>
        
        <tr>
            <td height=25px>&nbsp; Email (MOS): </td>
            <td><input type=text id=txtMOSEmail name=txtMOSEmail runat=server class="textboxBorderRO" />                                       
            </td>
                
            <td height=25px width=150px>&nbsp; Fax (MOS): </td>
            <td><input type=text id=txtMOSFax name=txtMOSFax runat=server class="textboxBorderRO" />                    
            </td>                                
        </tr>
        <tr>
            <td height=25px>&nbsp; Date of MOS Follow Up: </td>
            <td colspan=3><input type=text id=txtDOMosFollowUPDate name=txtDOMosFollowUPDate runat=server class="textboxBorderRO" />                                       
            </td>                                                          
        </tr>

    </table>
    </div>  


                </dx:ContentControl>
            </ContentCollection>
            </dx:TabPage>
        </TabPages>

        <LoadingPanelImage Url="~/App_Themes/RedWineCustom/Web/Loading.gif"></LoadingPanelImage>
        
        <Paddings Padding="2px" PaddingLeft="5px" PaddingRight="5px" />
        
        <ContentStyle>
            <Border BorderColor="#A74768" BorderStyle="Solid" BorderWidth="1px" />
        </ContentStyle>
        </dx:ASPxPageControl> 
    </div>
       
    <dx:ASPxPopupControl ID="ASPxClientFinderPopup" runat="server" Width=650px Modal=True
    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
    HeaderText="" AllowDragging="True" EnableAnimation="False"
    ClientInstanceName="ASPxClientFinderPopup" EncodeHtml="False"
    ClientIDMode="AutoID" 
    CssFilePath="~/App_Themes/RedWineCustom/Web/styles.css" 
    CssPostfix="RedWine" LoadingPanelImagePosition="Top" 
    SpriteCssFilePath="~/App_Themes/RedWineCustom/Web/sprite.css"> 

    <LoadingPanelImage Url="~/App_Themes/RedWineCustom/Web/Loading.gif"></LoadingPanelImage>        
    <ContentStyle VerticalAlign="Top"></ContentStyle>        

    <ContentCollection>
        <dx:PopupControlContentControl runat=server >           
            <div align=center style="width:650px"> 
            <%--HEADER LINE TABLES--%>

            <%--EXISTING TABLE OF TASKS--%>
            <table cellpadding=0 cellspacing=0 border=0 width=100%>
                <tr class="ContainerHeader">
                    <td height=26px style="font-size:14px;" colspan=2>&nbsp;<b> Existing Clients</b></td>
                    <td height=26px align=right>
                        <input type=text id="txtFilter" onkeypress="return FilterResults(event);" value="" runat=server class="textboxBorder"/>                                                                                                                             
                        &nbsp;<img src="../images/icons/search.png" style="cursor:pointer" title="Click here to filter client list" onclick="ClientFilter()"/>&nbsp;&nbsp;                                                                                                                                                                              
                    </td>                                                                                                          
                </tr>
            </table>

            <table id=tblExistingUsers cellpadding="0" cellspacing="0" border="1" runat=server style="margin-top:20px;margin-left:30px;margin-bottom:20px">                    
                <tr class="ContainerHeader">
                    <td align=center height=25px width=75px>Client #</td>                
                    <td align=center height=25px width=125px>Name</td>                                               
                    <td align=center height=25px width=125px>Email</td>                
                    <td align=center height=25px width=100px>Contact Person</td>                
                </tr>
            </table>    
            </div>            
        </dx:PopupControlContentControl>
    </ContentCollection>
            
    </dx:ASPxPopupControl>    
                 
    <%--*************** END OF DEBTORS INFO ***************--%>

    <br /><br />

   

    </form>
</body>
</html>
