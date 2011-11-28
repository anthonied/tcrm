using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PastelCrmDataPump.Classes;
using Pervasive.Data.SqlClient;
using System.Data.SqlClient;

namespace PastelCrmDataPump
{
    public partial class DataPump : Form
    {
        private DataSet dsSourceData;

        public DataPump()
        {
            InitializeComponent();            
        }

        private void cmdSync_Click(object sender, EventArgs e)
        {
            Cursor = System.Windows.Forms.Cursors.WaitCursor;

            if (Connect.sPastelConnStr != null)
                RetrievePervasiveData("");
            else
            { 
                MessageBox.Show("Connection string of source data is not available. Operation aborted", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (dsSourceData.Tables["CustomerSource"].Rows.Count > 0) //Check Data
                TransferPervasiveData("");
            else
                MessageBox.Show("No source data retrieved. Sync aborted", "No data", MessageBoxButtons.OK, MessageBoxIcon.Information);

            Cursor = System.Windows.Forms.Cursors.Default;
        }

        /// <summary>
        /// Fill dataset with Pastel Customers
        /// </summary>
        /// <param name="sOnlyCustomerCode">Only get one customer in dataset - Leave blank if you want all</param>
        private void RetrievePervasiveData(string sOnlyCustomerCode)
        {
            try
            {
                string sSql = "";

                using (PsqlConnection oConn = new PsqlConnection(Connect.sPastelConnStr))
                {
                    oConn.Open();

                    //Fill Dataset from Source (Pastel)

                    sSql = " SELECT DISTINCT CustomerDesc,CustomerMaster.CustomerCode,PostAddress01,PostAddress02,PostAddress03,PostAddress04,Blocked,";
                    sSql += " Email, Telephone,Fax, Contact, DelAddress01, DelAddress02, DelAddress03, DelAddress04";
                    sSql += " FROM CustomerMaster";
                    sSql += " LEFT JOIN DeliveryAddresses on CustomerMaster.CustomerCode = DeliveryAddresses.CustomerCode";

                    if (sOnlyCustomerCode != "")
                    {
                        sSql += " WHERE CustomerMaster.CustomerCode = '" + sOnlyCustomerCode + "'";

                    }
                    //sSql += " WHERE";

                    dsSourceData = Connect.getDataSet(sSql, "CustomerSource", oConn);                    

                    oConn.Dispose();
                }                                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Info: " + ex.Message, "Exception Occurred (Source)", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// Copy data from Pastel to CRM
        /// </summary>
        /// <param name="sCustomerCode">Only copy one customer to CRM - Leave blank if you want all</param>
        private void TransferPervasiveData(string sOnlyCustomerCode)
        {            
            DataRow[] drFoundRows;
            string sExistList = "";            
            string sExcludeList = "";
            int iRet = 0;
            int iTotalAdded = 0;
            string sTotalExcludeQuery = "";

            try
            {                
                sExistList = RetrieveExistingList();
                sExcludeList = RetrieveExcludedList();

                //sTotalExcludeQuery = " CustomerCode NOT IN ('E&J001')";

                if (sOnlyCustomerCode == "")
                {                    
                    sTotalExcludeQuery = " CustomerCode NOT IN " + sExistList + " AND CustomerCode NOT IN " + sExcludeList;
                }
                //sTotalExcludeQuery = " CustomerCode NOT IN " + sExcludeList; //" AND CustomerCode NOT IN " + sExcludeList;

                using (SqlConnection sqlConn = new SqlConnection(Connect.sCRMConnStr))
                {
                    sqlConn.Open();                    

                    // Use the Select method to find all rows matching the filter.
                    drFoundRows = dsSourceData.Tables["CustomerSource"].Select(sTotalExcludeQuery);
                    MessageBox.Show(drFoundRows.Length.ToString());

                    if (drFoundRows.Length > 0)
                    {
                        foreach (DataRow drSourceRow in drFoundRows)
                        {
                            string sCustomerDesc = drSourceRow["CustomerDesc"].ToString().Trim();
                            string sCustomerCode = drSourceRow["CustomerCode"].ToString().Trim();
                            string sPostAddress01 = drSourceRow["PostAddress01"].ToString().Trim();
                            string sPostAddress02 = drSourceRow["PostAddress02"].ToString().Trim();
                            string sPostAddress03 = drSourceRow["PostAddress03"].ToString().Trim();
                            string sPostAddress04 = drSourceRow["PostAddress04"].ToString().Trim();
                            string sBlocked = drSourceRow["Blocked"].ToString().Trim();

                            string sEmail = drSourceRow["Email"].ToString().Trim();
                            string sTelephone = drSourceRow["Telephone"].ToString().Trim();
                            string sFax = drSourceRow["Fax"].ToString().Trim();
                            string sContact = drSourceRow["Contact"].ToString().Trim();
                            string sDelAddress01 = drSourceRow["DelAddress01"].ToString().Trim();
                            string sDelAddress02 = drSourceRow["DelAddress02"].ToString().Trim();
                            string sDelAddress03 = drSourceRow["DelAddress03"].ToString().Trim();
                            string sDelAddress04 = drSourceRow["DelAddress04"].ToString().Trim();

                            string sSalesPerson = "Sync";

                            //Update/Insert Target
                            string sSql = "INSERT INTO existing_clients (sName,sClientNumber,sEmail,sTelephone,sFax,sAddressLine1,sAddressLine2,sAddressLine3,sPostalCode,";
                            sSql += "sContactPerson,sSalesPerson,FKiCategoryID,dCreditLimit,iCreditTerms,dOutstandingBalance,iBlockedAccount,iWarrantySigned,iFormsCompleted,iCODApproved,";
                            sSql += "sC_ConstrType,FKiC_OppPlantTool,FKiC_OppTSU,FKiC_Marketer,sC_AddLine1,sC_AddLine2,sC_AddLine3,sC_Code,";
                            sSql += "iC_DiscountEquipm,iC_DiscountStruct,iC_HasInsurance,iC_IsVerticalUpClient,iC_IsInterestedDemo)";

                            sSql += " VALUES (";
                            sSql += "'" + sCustomerDesc.Replace("'", "''").ToString() + "',"; //sName
                            sSql += "'" + sCustomerCode.Replace("'", "''").ToString() + "',"; //sClientNumber
                            sSql += "'" + sEmail.Replace("'", "''").ToString() + "',"; //sEmail
                            sSql += "'" + sTelephone + "',"; //sTelephone
                            sSql += "'" + sFax + "',"; //sFax
                            sSql += "'" + sPostAddress01.Replace("'", "''").ToString() + "',"; //sAddressLine1
                            sSql += "'" + sPostAddress02.Replace("'", "''").ToString() + "',"; //sAddressLine2
                            sSql += "'" + sPostAddress03.Replace("'", "''").ToString() + "',"; //sAddressLine3
                            sSql += "'" + sPostAddress04.Replace("'", "''").ToString() + "',"; //sPostalCode

                            sSql += "'" + sContact.Replace("'", "''").ToString() + "',"; //sContactPerson
                            sSql += "'" + sSalesPerson + "',"; //sSalesPerson
                            sSql += "0,"; //FKiCategoryID
                            sSql += "0,"; //dCreditLimit
                            sSql += "0,"; //iCreditTerms
                            sSql += "0,"; //dOutstandingBalance
                            sSql += "'" + sBlocked + "',"; //iBlockedAccount
                            sSql += "0,"; //iWarrantySigned
                            sSql += "0,"; //iFormsCompleted
                            sSql += "0,"; //iCODApproved

                            sSql += "'none',"; //sC_ConstrType
                            sSql += "'0',"; //FKiC_OppPlantTool
                            sSql += "'0',"; //FKiC_OppTSU
                            sSql += "'0',"; //FKiC_Marketer
                            sSql += "'" + sDelAddress01.Replace("'", "''").ToString() + "',"; //sC_AddLine1
                            sSql += "'" + sDelAddress02.Replace("'", "''").ToString() + "',"; //sC_AddLine2
                            sSql += "'" + sDelAddress03.Replace("'", "''").ToString() + "',"; //sC_AddLine3
                            sSql += "'" + sDelAddress04.Replace("'", "''").ToString() + "',"; //sC_Code

                            sSql += "0,"; //iC_DiscountEquipm
                            sSql += "0,"; //iC_DiscountStruct
                            sSql += "0,"; //iC_HasInsurance
                            sSql += "0,"; //iC_IsVerticalUpClient
                            sSql += "0"; //iC_IsInterestedDemo

                            sSql += ")";

                            iRet = Connect.getDataCommand(sSql, sqlConn).ExecuteNonQuery();
                            iTotalAdded += iRet;
                        }

                        //Success Message
                        MessageBox.Show("Sync has completed - " + iTotalAdded.ToString() + " records added", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("All records are currently in sync", "No Sync Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    sqlConn.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Info: " + ex.Message, "Exception Occurred (Target)", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        
        private string RetrieveExistingList()
        {
            string sResult = "(";

            using (SqlConnection sqlConn = new SqlConnection(Connect.sCRMConnStr))
            {
                sqlConn.Open();

                string sSqlExisting = " SELECT sClientNumber FROM existing_clients";

                SqlDataReader rdExistReader = Connect.getDataCommand(sSqlExisting, sqlConn).ExecuteReader();
                if (rdExistReader.HasRows)
                {
                    while (rdExistReader.Read())
                    {
                        sResult += "'" + rdExistReader["sClientNumber"].ToString().Replace("'","''").Trim() + "',";
                    }
                }
                else
                {
                    sResult += "''";
                }

                rdExistReader.Dispose();
                sqlConn.Dispose();
            }

            sResult = sResult.Substring(0, sResult.Length - 1);
            sResult += ")";

            return sResult;
        }

        private string RetrieveExcludedList()
        {
            string sExcludedResult = "(";

            using (SqlConnection sqlConn = new SqlConnection(Connect.sCRMConnStr))
            {
                sqlConn.Open();

                string sSqlExcluded = " SELECT CustomerCode FROM excluded_clients";

                SqlDataReader rdExcludeReader = Connect.getDataCommand(sSqlExcluded, sqlConn).ExecuteReader();
                if (rdExcludeReader.HasRows)
                {
                    while (rdExcludeReader.Read())
                    {
                        sExcludedResult += "'" + rdExcludeReader["CustomerCode"].ToString().Replace("'", "''").Trim() + "',";
                    }
                }
                else
                {
                    sExcludedResult += "''";
                }
                rdExcludeReader.Dispose();
                sqlConn.Dispose();
            }

            sExcludedResult = sExcludedResult.Substring(0, sExcludedResult.Length - 1);
            sExcludedResult += ")";

            return sExcludedResult;
        }

        private void cmdClientStatus_Click(object sender, EventArgs e)
        {
            using (var psqlConn = new PsqlConnection(Connect.sPastelConnStr))
            {
                psqlConn.Open();
                string sSql = "  SELECT DISTINCT CustomerDesc ";
                sSql += " FROM CustomerMaster";
                sSql += " WHERE CustomerCode = '" + txtCustCodeStatus.Text + "'";

                var oReturn = Connect.getDataCommand(sSql, psqlConn).ExecuteScalar();
                if (oReturn != null)
                {
                    txtCustomerDescriptionStatus.Text = oReturn.ToString();
                    picPastelExistStatus.Image = global::PastelCrmDataPump.Properties.Resources.icon_yes;
                    //customer does exist in Pastel, look for him in CRM
                    using (var sqlCon = new SqlConnection(Connect.sCRMConnStr))
                    {
                        sqlCon.Open();
                        sSql = "SELECT count(*) from excluded_clients where CustomerCode = '" + txtCustCodeStatus.Text + "'";
                        oReturn = Connect.getDataCommand(sSql, sqlCon).ExecuteScalar();
                        if (int.Parse(oReturn.ToString()) > 0)
                        {
                            picCrmExistPastel.Image = global::PastelCrmDataPump.Properties.Resources.cut;
                        }
                        else
                        {
                            sSql = "SELECT count(*) from existing_clients where sClientNumber = '" + txtCustCodeStatus.Text + "'";
                            oReturn = Connect.getDataCommand(sSql, sqlCon).ExecuteScalar();
                            if (int.Parse(oReturn.ToString()) == 0)
                            {
                                picCrmExistPastel.Image = global::PastelCrmDataPump.Properties.Resources.Delete_Icon;
                            }
                            else
                            {
                                picCrmExistPastel.Image = global::PastelCrmDataPump.Properties.Resources.icon_yes;
                            }
                        }
                    }
                }
                else
                {
                    txtCustomerDescriptionStatus.Text = "No Customer Found";
                    picPastelExistStatus.Image = global::PastelCrmDataPump.Properties.Resources.Delete_Icon;
                }
                psqlConn.Close();
                MessageBox.Show("Completed");
            }
        }

        private void cmdSyncCustomer_Click(object sender, EventArgs e)
        {
            RetrievePervasiveData(txtCustCodeStatus.Text);
            TransferPervasiveData(txtCustCodeStatus.Text);
        }

       
    }
}
