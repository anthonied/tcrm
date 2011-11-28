using System;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using DBITWebControls;
using System.Data;
using System.IO;

public partial class pages_existing_client_info : System.Web.UI.Page
{

    public static string sClientID = "";
    public DataSet dsExistingClientPMTs;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["LoggedInUserID"] == null)
        {
            Response.Write("<script type=\"text/javascript\">window.top.location.href = '" + Page.ResolveClientUrl("~/Default.aspx") + "';</script>");
            Response.Flush();
            return;
        }
        sClientID = Request.QueryString["sClientID"];

        if (!IsPostBack)
        {
            GenerateCategoryDropDown();
            GenerateMainOfficeDropDown();            

            //**** COMPANY INFO ****
            GenerateOppPlantToolDropDown();
            GenerateOppTSUDropDown();
            GenerateMarketerDropDown();
            
            LoadClientInfo();
            LoadPaymentInfo();

            GenerateExistingClientList();
            GenerateMarketerTable();
            LoadActiveStatus();
        }        
    }
    #region RESPONSIBLE_USER
    public void LoadActiveStatus()
    {
        string sHTMLRet = "";

        using (SqlConnection oConn = new SqlConnection(Connect.sConnStr))
        {
            string sSQL = "SELECT bActive FROM existing_clients WHERE PKiClientID='" + sClientID + "'";

            SqlDataReader rdReader = Connect.getDataCommand(sSQL, oConn, Page).ExecuteReader();

            if (rdReader.HasRows)
            {
                rdReader.Read();
                //try
                {
                    bool active;
                    if (bool.TryParse(rdReader["bActive"].ToString(), out active))
                        chkUserActive.Checked = active;
                }
               // catch(Exception){}
            }
            rdReader.Close();
        }

        Response.Write(sHTMLRet);
    }
    public void GenerateMarketerTable()
    {
        tblMarketers.Rows.Clear();
        string sHTMLRet = "";

        using (SqlConnection oConn = new SqlConnection(Connect.sConnStr))
        {
            string sSQL = "SELECT PKiUserID, sName, sSurname FROM users WHERE sType='Marketer'";

            SqlDataReader rdReader = Connect.getDataCommand(sSQL, oConn, Page).ExecuteReader();
           
            if (rdReader.HasRows)
            {
                int usernum = 0;
                HtmlTableRow row = null;
                
                while (rdReader.Read())
                {
                    if (usernum % 2 == 0)
                    {                            
                        row = new HtmlTableRow();
                        tblMarketers.Rows.Add(row);
                    }
                    usernum++;
                    HtmlTableCell cell = new HtmlTableCell();
                    HiddenField hdn = new HiddenField();
                    cell.Width = "150px";
                    row.Cells.Add(cell);
                    hdn.ID = "hdnMarketer" + rdReader["PKiUserID"].ToString();
                    hdn.Value = "false";
                    hdn.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                    CheckBox checkbox = new CheckBox();
                    checkbox.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                    checkbox.Checked = false;
                    checkbox.ID = "chkMarketer" + rdReader["PKiUserID"].ToString();
                    checkbox.Text = "&nbsp;&nbsp;" + rdReader["sName"].ToString() + " " + rdReader["sSurname"].ToString();
                    checkbox.Attributes.Add("onclick", "return confirmCheck('" + rdReader["PKiUserID"].ToString()+"')");
                
                    cell.Controls.Add(checkbox);
                    cell.Controls.Add(hdn);
                }
            }
            rdReader.Close();
             
            sSQL = "select marketer_assignment.FKiUserID, marketer_assignment.FKiClientID, users.sName, users.sSurname from marketer_assignment join users on marketer_assignment.FKiUserID = users.PKiUserID where marketer_assignment.FKiClientID='" + sClientID + "'";
            rdReader = Connect.getDataCommand(sSQL, oConn, Page).ExecuteReader();

            if (rdReader.HasRows)
            {
                while (rdReader.Read())
                {
                    CheckBox checkbox = (CheckBox)tblMarketers.FindControl("chkMarketer" + rdReader["FKiUserID"].ToString());
                    if(checkbox != null) // Should not happen if marketers are correctly set up
                        checkbox.Checked = true;
                    HiddenField hdn = (HiddenField)tblMarketers.FindControl("hdnMarketer" + rdReader["FKiUserID"].ToString());
                    if (hdn != null) // Should not happen if marketers are correctly set up
                        hdn.Value = "true";
                }
            }
            
        }

        Response.Write(sHTMLRet);
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        using (SqlConnection oConn = new SqlConnection(Connect.sConnStr))
        {
            oConn.Open();

            if (sClientID != "")
            {
                string sSQL = " UPDATE existing_clients SET ";
              //  sSQL += " FKiUserID='" + selUserList.Value + "',";
                sSQL += " bActive='" + ((chkUserActive.Checked) ? "1" : "0") + "',";
                sSQL += " bExcluded ='" + ((chkUserExclude.Checked) ? "1" : "0") + "'"; 
                sSQL += " WHERE PKiClientID = '" + sClientID + "'";

                int iRet = Connect.getDataCommand(sSQL, oConn, Page).ExecuteNonQuery();

                if (iRet > 0)
                {
                    if (chkUserExclude.Checked)
                    {
                        //Check Against Exclude List
                        sSQL = "SELECT COUNT(CustomerCode) FROM excluded_clients";
                        sSQL += " WHERE CustomerCode = '" + txtClientNumber.Value.Trim() + "'";

                        int iResult = Convert.ToInt32(Connect.getDataCommand(sSQL, oConn).ExecuteScalar().ToString());

                        if (iResult == 0)
                        { 
                            sSQL = " INSERT INTO excluded_clients"; //Add to Exclude List
                            sSQL += " (CustomerCode,CustomerDesc)";
                            sSQL += " VALUES";
                            sSQL += " ('" + txtClientNumber.Value.Trim() + "',";
                            sSQL += " '" + txtName.Value.Trim() + "')";

                            Connect.getDataCommand(sSQL, oConn).ExecuteNonQuery();
                        }
                    }

                    foreach (string key in Request.Form.AllKeys)
                    {
                        int lastindex = key.LastIndexOf("hdnMarketer");
                        if (lastindex >= 0) // Thus found
                        {
                            string userid = key.Substring(lastindex + "chkMarketer".Length); //checkbox.Name.Replace("chkMarketer", "");
                            string[] values = Request.Form.GetValues(key);

                            foreach (string value in values)
                            {
                                
                                if (value == "true")
                                {
                                    sSQL = "INSERT INTO marketer_assignment (FKiUserID,FKiClientID) VALUES ('" + userid.Trim() + "','" + sClientID.Trim() + "')";

                                }
                                else
                                {
                                    sSQL = "DELETE FROM marketer_assignment WHERE FKiUserID='" + userid.Trim() + "' AND FKiClientID='" + sClientID.Trim() + "'";
                                }

                                try // Ignore duplicate entry errors. Delete should never throw
                                {
                                    int iResult = Connect.getDataCommand(sSQL, oConn).ExecuteNonQuery();

                                }
                                catch{}
                            }
                        }
                    }
                    GenerateMarketerTable();
                    General.AlertClient("Update Successful");
                }

            }
        }
    }

    #endregion


    #region DEBTOR INFO

    public void LoadClientInfo()
    {
        using (SqlConnection oConn = new SqlConnection(Connect.sConnStr))
        {
            oConn.Open();

            if (sClientID != "")
            {
                txtClientId.Value = sClientID;

                string sSQL = " SELECT * FROM existing_clients";
                sSQL += " WHERE PKiClientID = '" + sClientID + "'";

                SqlDataReader rdReader = Connect.getDataCommand(sSQL, oConn, Page).ExecuteReader();

                if (rdReader.HasRows)
                {
                    // ***** Variables DEBTOR INFO ******
                    string sImageLocation = "";
                    string sName = ""; string sSurname = ""; string sIDNumber = ""; string sClientNumber = ""; string sEmail = "";
                    string sTelephone = ""; string sFax = "";

                    string sAddressLine1 = ""; string sAddressLine2 = ""; string sAddressLine3 = ""; string sPostalCode = "";
                    string sContactPerson = ""; string sSalesPerson = ""; string sOfficialOrder = "";
                    string sAccContactPerson = ""; string sAccContactNumber = "";
                        
                    string sFKiCategoryID = ""; string sFKiMainOfficeID = "";

                    decimal dCreditLimit = Convert.ToDecimal("0.00"); 
                    decimal dOutstandingBalance = Convert.ToDecimal("0.00");
                    string sDateOfApplication = ""; string sBirthdayDate = "";

                    int iCreditTerms = 0;
                    bool bIsBlockedAccount = false;
                    bool bIsWarrantySigned = false;
                    bool bIsFormsCompleted = false;
                    bool bIsCODApproved = false;

                    // ***** Variables COMPANY INFO ******

                    string sCConstructType = ""; string sCAddressLine1 = ""; string sCAddressLine2 = ""; string sCAddressLine3 = ""; string sCCode = "";
                    string sFKiOppPandT = ""; string sFKiOppTSU = ""; string sFKiMarketer = "";

                    string sCDMSLat1 = ""; string sCDMSLat2 = ""; string sCDMSLat3 = ""; string sCDMSDecLat = "";
                    string sCDMSLon1 = ""; string sCDMSLon2 = ""; string sCDMSLon3 = ""; string sCDMSDecLon = "";

                    int iCDiscountEquipm = 0; int iCDiscountStruct = 0;
                    bool bCHasInsurance = false;

                    bool bCIsVerticalUpClient = false;
                    bool bCIsInterestedInDemo = false;
                    string sCDemoDate = "";

                    string sCMarketingStrategy = ""; string sCClientObtainedMethod = "";

                    string sCRentalLocation = "";

                    string sCOwnerName = ""; string sCOwnerSurnames = ""; string sCEmail = ""; string sCCellphoneNum = ""; string sCDOB = ""; string sCHobbies = "";

                    string sCBuyerName = ""; string sCBuyerSurname = ""; string sCBuyerEmail = ""; string sCBuyerCellphoneNum = ""; string sCBuyerDOB = ""; string sCBuyerHobbies = "";

                    string sCMosEmail = ""; string sCMosFax = ""; string sCMosFollowUpDate = "";

                    while (rdReader.Read())
                    {
                        //Get Values from DB
                        sImageLocation = "~/images/client_images/" + rdReader["sImageFile"].ToString().Trim();                             

                        sName = rdReader["sName"].ToString().Trim();
                        sSurname = rdReader["sSurname"].ToString().Trim();
                        sIDNumber = rdReader["sIDNumber"].ToString().Trim();
                        sClientNumber = rdReader["sClientNumber"].ToString().Trim();
                        sEmail = rdReader["sEmail"].ToString().Trim();
                        sTelephone = rdReader["sTelephone"].ToString().Trim();
                        sFax = rdReader["sFax"].ToString().Trim();

                        sAddressLine1 = rdReader["sAddressLine1"].ToString().Trim();
                        sAddressLine2 = rdReader["sAddressLine2"].ToString().Trim();
                        sAddressLine3 = rdReader["sAddressLine3"].ToString().Trim();
                        sPostalCode = rdReader["sPostalCode"].ToString().Trim();

                        sContactPerson = rdReader["sContactPerson"].ToString().Trim();
                        sSalesPerson = rdReader["sSalesPerson"].ToString().Trim();                        

                        sOfficialOrder = rdReader["sOfficialOrder"].ToString().Trim();
                        sAccContactPerson = rdReader["sAccContactPerson"].ToString().Trim();
                        sAccContactNumber = rdReader["sAccContactNumber"].ToString().Trim();

                        sFKiCategoryID = rdReader["FKiCategoryID"].ToString().Trim();
                        sFKiMainOfficeID = rdReader["FKiMainOfficeID"].ToString().Trim();

                        dCreditLimit = Convert.ToDecimal(rdReader["dCreditLimit"].ToString().Trim());
                        dOutstandingBalance = Convert.ToDecimal(rdReader["dOutstandingBalance"].ToString().Trim());

                        sDateOfApplication = rdReader["dtDateOfApplication"].ToString().Trim();
                        sBirthdayDate = rdReader["dtBirthdayDate"].ToString().Trim();

                        iCreditTerms = Convert.ToInt32(rdReader["iCreditTerms"].ToString().Trim());

                        if (rdReader["iBlockedAccount"].ToString().Trim() == "1")
                            bIsBlockedAccount = true;
                        else
                            bIsBlockedAccount = false;

                        if (rdReader["iWarrantySigned"].ToString().Trim() == "1")
                            bIsWarrantySigned = true;
                        else
                            bIsWarrantySigned = false;

                        if (rdReader["iFormsCompleted"].ToString().Trim() == "1")
                            bIsFormsCompleted = true;
                        else
                            bIsFormsCompleted = false;

                        if (rdReader["iCODApproved"].ToString().Trim() == "1")
                            bIsCODApproved = true;
                        else
                            bIsCODApproved = false;


                        //*** COMPANY INFO ***                        
                        sCConstructType = rdReader["sC_ConstrType"].ToString().Trim();
                        sCAddressLine1 = rdReader["sC_AddLine1"].ToString().Trim();
                        sCAddressLine2 = rdReader["sC_AddLine2"].ToString().Trim();
                        sCAddressLine3 = rdReader["sC_AddLine3"].ToString().Trim();                        
                        sCCode = rdReader["sC_Code"].ToString().Trim();

                        sFKiOppPandT = rdReader["FKiC_OppPlantTool"].ToString().Trim();
                        sFKiOppTSU = rdReader["FKiC_OppTSU"].ToString().Trim();
                        sFKiMarketer = rdReader["FKiC_Marketer"].ToString().Trim();

                        sCDMSLat1 = rdReader["sC_DMSLat1"].ToString().Trim();
                        sCDMSLat2 = rdReader["sC_DMSLat2"].ToString().Trim();
                        sCDMSLat3 = rdReader["sC_DMSLat3"].ToString().Trim();
                        sCDMSDecLat = rdReader["sC_DMSDecLat"].ToString().Trim();

                        sCDMSLon1 = rdReader["sC_DMSLon1"].ToString().Trim();
                        sCDMSLon2 = rdReader["sC_DMSLon2"].ToString().Trim();
                        sCDMSLon3 = rdReader["sC_DMSLon3"].ToString().Trim();
                        sCDMSDecLon = rdReader["sC_DMSDecLon"].ToString().Trim();

                        iCDiscountEquipm = Convert.ToInt32(rdReader["iC_DiscountEquipm"].ToString().Trim());
                        iCDiscountStruct = Convert.ToInt32(rdReader["iC_DiscountStruct"].ToString().Trim());

                        if (rdReader["iC_HasInsurance"].ToString().Trim() == "1")
                            bCHasInsurance = true;
                        else
                            bCHasInsurance = false;


                        if (rdReader["iC_IsVerticalUpClient"].ToString().Trim() == "1")
                            bCIsVerticalUpClient = true;
                        else
                            bCIsVerticalUpClient = false;

                        if (rdReader["iC_IsInterestedDemo"].ToString().Trim() == "1")
                            bCIsInterestedInDemo = true;
                        else
                            bCIsInterestedInDemo = false;

                        sCDemoDate = rdReader["dC_DemoDate"].ToString().Trim();

                        sCMarketingStrategy = rdReader["sC_MarketingStrategy"].ToString().Trim();
                        sCClientObtainedMethod = rdReader["sC_ClientObtainedMethod"].ToString().Trim();

                        sCRentalLocation = rdReader["sC_RentalLocation"].ToString().Trim();

                        sCOwnerName = rdReader["sC_OwnerName"].ToString().Trim();
                        sCOwnerSurnames = rdReader["sC_OwnerSurname"].ToString().Trim();
                        sCEmail = rdReader["sC_Email"].ToString().Trim();
                        sCCellphoneNum = rdReader["sC_Email"].ToString().Trim();
                        sCDOB = rdReader["sC_DOB"].ToString().Trim();
                        sCHobbies = rdReader["sC_Hobbies"].ToString().Trim();

                        sCBuyerName = rdReader["sC_BuyerName"].ToString().Trim();
                        sCBuyerSurname = rdReader["sC_BuyerSurname"].ToString().Trim();
                        sCBuyerEmail = rdReader["sC_BuyerEmail"].ToString().Trim();
                        sCBuyerCellphoneNum = rdReader["sC_BuyerCellphoneNum"].ToString().Trim();
                        sCBuyerDOB = rdReader["dC_BuyerDOB"].ToString().Trim();
                        sCBuyerHobbies = rdReader["sC_BuyerHobbies"].ToString().Trim();

                        sCMosEmail = rdReader["sC_MosEmail"].ToString().Trim();
                        sCMosFax = rdReader["sC_MosFax"].ToString().Trim();
                        sCMosFollowUpDate = rdReader["dC_MosFollowUpDate"].ToString().Trim();
                    }

                    rdReader.Close();

                    //Assign values to HTML control (fill the form)
                    if (sImageLocation != "")
                    {
                        if (File.Exists(Server.MapPath(sImageLocation)))
                        {
                            imgClientPhoto.ImageUrl = sImageLocation;
                            imgClientPhotoComp.ImageUrl = sImageLocation;
                        }
                    }

                    txtName.Value = sName;
                    txtClientNumber.Value = sClientNumber;
                    txtTelephone.Value = sTelephone;
                    txtFax.Value = sFax;
                    txtEmail.Value = sEmail;

                    txtAddLine1.Value = sAddressLine1;
                    txtAddLine2.Value = sAddressLine2;
                    txtAddLine3.Value = sAddressLine3;
                    txtPostalCode.Value = sPostalCode;

                    txtContactP.Value = sContactPerson;
                    txtSalesP.Value = sSalesPerson;
                    selCategory.Value = sFKiCategoryID;
                    selClientOffice.Value = sFKiMainOfficeID;

                    txtCreditLimit.Value = dCreditLimit.ToString();
                    txtDOA.Value = sDateOfApplication;
                    txtCreditTerms.Value = iCreditTerms.ToString();
                    txtOutstBal.Value = dOutstandingBalance.ToString();

                    chkBlockedAccount.Checked = bIsBlockedAccount;
                    chkWarrantySigned.Checked = bIsWarrantySigned;
                    chkFormsCompleted.Checked = bIsFormsCompleted;
                    chkCODApproved.Checked = bIsCODApproved;

                    selOfficialOrder.Value = sOfficialOrder;
                    txtAccContactPerson.Value = sAccContactPerson;
                    txtAccContactNum.Value = sAccContactNumber;

                    txtDOB.Value = sBirthdayDate;


                    //*** COMPANY INFO - ASSIGNMENT ***
                    txtClientNumCompany.Value = sClientNumber;
                    selConstructType.Value = sCConstructType;
                    txtPhysAddLine1.Value = sCAddressLine1;
                    txtPhysAddLine2.Value = sCAddressLine2;
                    txtPhysAddLine3.Value = sCAddressLine3;
                    txtCCode.Value = sCCode;
                    txtCCode.Value = sCCode;

                    selOppPlantTool.Value = sFKiOppPandT;
                    selOppTSU.Value = sFKiOppTSU;
                    selMarketer.Value = sFKiMarketer;

                    txtCDMSLat1.Value = sCDMSLat1;
                    txtCDMSLat2.Value = sCDMSLat2;
                    txtCDMSLat3.Value = sCDMSLat3;
                    txtCDMSDecLat.Value = sCDMSDecLat;

                    txtCDMSLon1.Value = sCDMSLon1;
                    txtCDMSLon2.Value = sCDMSLon2;
                    txtCDMSLon3.Value = sCDMSLon3;
                    txtCDMSDecLon.Value = sCDMSDecLon;

                    txtDiscountEq.Value = iCDiscountEquipm.ToString();
                    txtDiscountStruct.Value = iCDiscountStruct.ToString();
                    
                    chkHasInsurance.Checked = bCHasInsurance;

                    chkIsVerticalUpClient.Checked = bCIsVerticalUpClient;
                    chkIsInterestedInDemo.Checked = bCIsInterestedInDemo;
                    txtCDemoDate.Value = sCDemoDate;

                    selMarketingStrategy.Value = sCMarketingStrategy;
                    selClientObMethod.Value = sCClientObtainedMethod;

                    selRentalsLocation.Value = sCRentalLocation;

                    txtOwnerName.Value = sCOwnerName;
                    txtOwnerSurname.Value = sCOwnerSurnames;
                    txtOwnerEmail.Value = sCEmail;
                    txtOwnerCell.Value = sCCellphoneNum;
                    txtOwnerDOB.Value = sCDOB;
                    txtOwnerHobbies.Value = sCHobbies;

                    txtBuyerName.Value = sCBuyerName;
                    txtBuyerSurname.Value = sCBuyerSurname;
                    txtBuyerEmail.Value = sCBuyerEmail;
                    txtBuyerCell.Value = sCBuyerCellphoneNum;
                    txtBuyerDOB.Value = sCBuyerDOB;
                    txtBuyerHobbies.Value = sCBuyerHobbies;

                    txtMOSEmail.Value = sCMosEmail;
                    txtMOSFax.Value = sCMosFax;
                    txtDOMosFollowUPDate.Value = sCMosFollowUpDate;                    
                }
            }

            oConn.Close();
        }
    }

    public void LoadPaymentInfo()
    {
        using (SqlConnection oConn = new SqlConnection(Connect.sConnStr))
        {
            string sSQL = "SELECT CONVERT(VARCHAR(10), dtPMTDate, 110) AS [PMTDate], dPMTAmount, sDocNum FROM client_payments";
            sSQL += " WHERE FKiClientID = '" +sClientID + "'";

            dsExistingClientPMTs = Connect.getDataSet(sSQL, "Payments", oConn);

            dgvPayments.DataSource = dsExistingClientPMTs.Tables["Payments"];
            dgvPayments.DataBind();
        }
    }

    public void GenerateCategoryDropDown()
    {
        selCategory.Items.Clear();
        string sHTMLRet = "";

        using (SqlConnection oConn = new SqlConnection(Connect.sConnStr))
        {
            string sSQL = "SELECT PKiCategoryID, sCategoryDesc FROM categories";

            SqlDataReader rdReader = Connect.getDataCommand(sSQL, oConn, Page).ExecuteReader();

            if (rdReader.HasRows)
            {
                while (rdReader.Read())
                {
                    ListItem liCategory = new ListItem(rdReader["sCategoryDesc"].ToString(), rdReader["PKiCategoryID"].ToString());
                    selCategory.Items.Add(liCategory);
                }
            }
            rdReader.Close();
        }

        Response.Write(sHTMLRet);
    }

    public void GenerateMainOfficeDropDown()
    {
        selClientOffice.Items.Clear();
        string sHTMLRet = "";

        using (SqlConnection oConn = new SqlConnection(Connect.sConnStr))
        {
            string sSQL = "SELECT PKiMainOfficeID, sMainOfficeDesc FROM main_offices";

            SqlDataReader rdReader = Connect.getDataCommand(sSQL, oConn, Page).ExecuteReader();

            if (rdReader.HasRows)
            {
                while (rdReader.Read())
                {
                    ListItem liMainOffice = new ListItem(rdReader["sMainOfficeDesc"].ToString(), rdReader["PKiMainOfficeID"].ToString());
                    selClientOffice.Items.Add(liMainOffice);
                }
            }

            rdReader.Close();
        }

        Response.Write(sHTMLRet);
    }

    //################## Client Finder Code ################    
    protected void GenerateExistingClientList()
    {        
        using (SqlConnection oConn = new SqlConnection(Connect.sConnStr))
        {
            string sSQL = "SELECT * FROM existing_clients";
            sSQL += " WHERE ";

            sSQL += "(";
            sSQL += "(sClientNumber LIKE '%" + txtFilter.Value + "%' OR '' = '" + txtFilter.Value + "') OR ";
            sSQL += "(sName LIKE '%" + txtFilter.Value + "%' OR '' = '" + txtFilter.Value + "') OR ";
            sSQL += "(sContactPerson LIKE '%" + txtFilter.Value + "%' OR '' = '" + txtFilter.Value + "') OR ";
            sSQL += "(sEmail LIKE '%" + txtFilter.Value + "%' OR '' = '" + txtFilter.Value + "')";
            sSQL += ")";

            sSQL += " ORDER BY sName ASC";

            SqlDataReader rdReader = Connect.getDataCommand(sSQL, oConn, Page).ExecuteReader();

            int iRowCount = 0;
            string sBackColor = "#EFF3FB";

            if (rdReader.HasRows)
            {
                while (rdReader.Read())
                {
                    //Set Colors 
                    if (iRowCount % 2 == 0)
                    {
                        sBackColor = "#EFF3FB";
                    }
                    else
                    {
                        sBackColor = "white";
                    }

                    //******* CREATE HTML TABLE ROWS AND ADD TO TABLE *******

                    HtmlTableRow tr = new HtmlTableRow();
                    tr.BgColor = sBackColor;

                    HtmlTableCell tcClientID = new HtmlTableCell();
                    tcClientID.Height = "25px";
                    tcClientID.InnerHtml = "&nbsp;" + rdReader["sClientNumber"].ToString();
                    tr.Cells.Add(tcClientID);

                    HtmlTableCell tcName = new HtmlTableCell();
                    tcName.Height = "25px";
                    tcName.InnerHtml = "&nbsp;" + rdReader["sName"].ToString();
                    tr.Cells.Add(tcName);

                    HtmlTableCell tcEmail = new HtmlTableCell();
                    tcEmail.Height = "25px";
                    tcEmail.InnerHtml = "&nbsp;" + rdReader["sEmail"].ToString();
                    tr.Cells.Add(tcEmail);

                    HtmlTableCell tcContact = new HtmlTableCell();
                    tcContact.Height = "25px";
                    tcContact.InnerHtml = "&nbsp;" + rdReader["sContactPerson"].ToString();
                    tr.Cells.Add(tcContact);

                    //SET ATTRIBUTES IN ROW
                    tr.Attributes.Add("onclick", "SelectClient('" + rdReader["PKiClientID"].ToString().Trim() + "')");
                    tr.Attributes.Add("title", "Click on a row to select...");
                    tr.Style.Add("cursor", "pointer");

                    tblExistingUsers.Rows.Add(tr);

                    //*******************************************************                    

                    iRowCount++;
                }
            }
            else
            {
                //******* CREATE HTML - NO DATA AVAILABLE *******

                HtmlTableRow trNoData = new HtmlTableRow();

                HtmlTableCell tcNoData = new HtmlTableCell();
                tcNoData.Height = "35px";
                tcNoData.ColSpan = 5;
                tcNoData.Align = "center";
                tcNoData.InnerHtml = "&nbsp; NO DATA CURRENTLY AVAILABLE";
                trNoData.Cells.Add(tcNoData);

                tblExistingUsers.Rows.Add(trNoData);

                //***********************************************
            }

            rdReader.Close();
        }
    }

    protected void btnFilterClients_Click(object sender, EventArgs e)
    {
        GenerateExistingClientList();
        GenerateMarketerTable();
    }

    #endregion 


    #region COMPANY INFO

    public void GenerateOppPlantToolDropDown()
    {
        selOppPlantTool.Items.Clear();
        string sHTMLRet = "";

        using (SqlConnection oConn = new SqlConnection(Connect.sConnStr))
        {
            string sSQL = "SELECT PKiOppPlantToolID, sOPTName FROM opp_planttool";

            SqlDataReader rdReader = Connect.getDataCommand(sSQL, oConn, Page).ExecuteReader();

            if (rdReader.HasRows)
            {
                while (rdReader.Read())
                {
                    ListItem liCategory = new ListItem(rdReader["sOPTName"].ToString(), rdReader["PKiOppPlantToolID"].ToString());
                    selOppPlantTool.Items.Add(liCategory);
                }
            }
            rdReader.Close();
        }

        Response.Write(sHTMLRet);
    }

    public void GenerateOppTSUDropDown()
    {
        selOppTSU.Items.Clear();
        string sHTMLRet = "";

        using (SqlConnection oConn = new SqlConnection(Connect.sConnStr))
        {
            string sSQL = "SELECT PKiOppTSUID, sOppTSUName FROM opp_TSU";

            SqlDataReader rdReader = Connect.getDataCommand(sSQL, oConn, Page).ExecuteReader();

            if (rdReader.HasRows)
            {
                while (rdReader.Read())
                {
                    ListItem liCategory = new ListItem(rdReader["sOppTSUName"].ToString(), rdReader["PKiOppTSUID"].ToString());
                    selOppTSU.Items.Add(liCategory);
                }
            }
            rdReader.Close();
        }

        Response.Write(sHTMLRet);
    }

    public void GenerateMarketerDropDown()
    {
        selMarketer.Items.Clear();
        string sHTMLRet = "";

        using (SqlConnection oConn = new SqlConnection(Connect.sConnStr))
        {
            string sSQL = "SELECT PKiMarketerID, sMarketerName FROM marketer";

            SqlDataReader rdReader = Connect.getDataCommand(sSQL, oConn, Page).ExecuteReader();

            if (rdReader.HasRows)
            {
                while (rdReader.Read())
                {
                    ListItem liCategory = new ListItem(rdReader["sMarketerName"].ToString(), rdReader["PKiMarketerID"].ToString());
                    selMarketer.Items.Add(liCategory);
                }
            }

            rdReader.Close();
        }

        Response.Write(sHTMLRet);
    }


    #endregion
}