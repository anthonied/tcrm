using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Globalization;
using DBITWebControls;

public partial class pages_New_Clients : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["LoggedInUserID"] == null)
        {
            Response.Write("<script type=\"text/javascript\">window.top.location.href = '" + Page.ResolveClientUrl("~/Default.aspx") + "';</script>");
            Response.Flush();
            return;
        }
        dsClientFinder.ConnectionString = Connect.sConnStr;
        string selectedClient = Page.Request.Params["SelectedClient"];
        // When not postback
        if (!IsPostBack && selectedClient == null)
        {
            // Load all the data
            LoadNewClientNames();
            LoadSiteTypes();
            LoadAllClientFields();
            GenerateSitesTable();
            GenerateForemenTable();
        }
        else if(selectedClient != null)
        {
             // Load all the data
            LoadNewClientNames();
            LoadSiteTypes();
            ddNewClients.SelectedValue = selectedClient;
            LoadAllClientFields();
            GenerateSitesTable();
            GenerateForemenTable();
        }
        gvClientDetails.DataBind();
    }

    // Load the dropdown with client names
    protected void LoadNewClientNames()
    {
        using (SqlConnection oConn = new SqlConnection(Connect.sConnStr))
        {
            oConn.Open();

            // Read the list of new clients
            string sSql = "SELECT PKiNewClientID, sNewClientName FROM new_clients ORDER BY sNewClientName";

            SqlDataReader reader = Connect.getDataCommand(sSql, oConn).ExecuteReader();

            // Bind to dropdown
            ddNewClients.DataSource = reader;
            ddNewClients.DataBind();
            reader.Close();
            oConn.Close();
        }
    }

    // Load the site clients
    protected void LoadSiteTypes()
    {
        using (SqlConnection oConn = new SqlConnection(Connect.sConnStr))
        {
            oConn.Open();

            // Read the list of site clients
            string sSql = "SELECT PKsSiteType FROM site_types ORDER BY PKsSiteType";

            SqlDataReader reader = Connect.getDataCommand(sSql, oConn).ExecuteReader();

            // Bind to dropdown
            ddSiteType.DataSource = reader;
            ddSiteType.DataBind();
            reader.Close();
            oConn.Close();
        }
    }
    // GetStringFromReader safely reads a string column from a reader
    protected string GetStringFromReader(string columnname, SqlDataReader reader)
    {
        // Check if parameters are valid
        if (reader == null)
            throw new ArgumentNullException("reader");
        if (columnname == null)
            throw new ArgumentNullException("columnname");
        if (columnname.Length == 0)
            throw new ArgumentException("columnname must not be an empty string");
        int ordinal = -1;

        // Try to find the ordinal number of the column
        try
        {
            ordinal = reader.GetOrdinal(columnname);
        }
        catch (IndexOutOfRangeException)
        {
            // Column was not found so return an empty string
            // TODO: maby show some sort of error
            return "";
        }
        string str = "";
        try
        {
            str = reader.GetString(ordinal).Trim();
        }
        catch
        {

        }
        // Return the column as a string
        return str; 
    }

    // GetInt32FromReader safely reads a string column from a reader
    protected Int32 GetInt32FromReader(string columnname, SqlDataReader reader)
    {
        // Check if parameters are valid
        if (reader == null)
            throw new ArgumentNullException("reader");
        if (columnname == null)
            throw new ArgumentNullException("columnname");
        if (columnname.Length == 0)
            throw new ArgumentException("columnname must not be an empty string");
        int ordinal = -1;

        // Try to find the ordinal number of the column
        try
        {
            ordinal = reader.GetOrdinal(columnname);
        }
        catch (IndexOutOfRangeException)
        {
            // Column was not found so return an empty string
            // TODO: maby show some sort of error
            return -1;
        }

        // Return the column as a string
        return reader.GetInt32(ordinal);
    }

    // GetMoneyFromReader safely reads a money column from a reader
    protected double GetMoneyFromReader(string columnname, SqlDataReader reader)
    {
        // Check if parameters are valid
        if (reader == null)
            throw new ArgumentNullException("reader");
        if (columnname == null)
            throw new ArgumentNullException("columnname");
        if (columnname.Length == 0)
            throw new ArgumentException("columnname must not be an empty string");
        int ordinal = -1;

        // Try to find the ordinal number of the column
        try
        {
            ordinal = reader.GetOrdinal(columnname);
        }
        catch (IndexOutOfRangeException)
        {
            // Column was not found so return an empty string
            // TODO: maby show some sort of error
            return -1;
        }

        // Return the column as a string
        return reader.GetSqlMoney(ordinal).ToDouble();
    }

    // GetDateFromReader safely reads a date column from a reader
    protected DateTime GetDateFromReader(string columnname, SqlDataReader reader)
    {
        // Check if parameters are valid
        if (reader == null)
            throw new ArgumentNullException("reader");
        if (columnname == null)
            throw new ArgumentNullException("columnname");
        if (columnname.Length == 0)
            throw new ArgumentException("columnname must not be an empty string");
        int ordinal = -1;

        // Try to find the ordinal number of the column
        try
        {
            ordinal = reader.GetOrdinal(columnname);
        }
        catch (IndexOutOfRangeException)
        {
            // Column was not found so return an empty string
            // TODO: maby show some sort of error
            return DateTime.Now;
        }

        // Return the column as a string
        return reader.GetDateTime(ordinal);
    }

    // Empties all client related textboxes
    protected void ClearAllClientFields()
    {
        txtNewClientName.Text = "";
        txtNewClientContactPerson.Text = "";
        txtNewClientTelephone.Text = "";
        txtNewClientCellphone.Text = "";
        txtNewClientEmail.Text = "";

        txtCEFirmName.Text = "";
        txtCEContactPerson.Text = "";
        txtCETelephone.Text = "";
        txtCECellphone.Text = "";
        txtCEEmail.Text = "";

        txtPMFirmName.Text = "";
        txtPMContactPerson.Text = "";
        txtPMTelephone.Text = "";
        txtPMCellphone.Text = "";
        txtPMEmail.Text = "";

        txtContractorFirmName.Text = "";
        txtContractorContactPerson.Text = "";
        txtContractorTelephone.Text = "";
        txtContractorCellphone.Text = "";
        txtContractorEmail.Text = "";

    }

    // Empties all site related textboxes
    protected void ClearAllSiteFields()
    {
        txtSiteName.Text = "";
        txtSiteTelephone.Text = "";
        ddSiteType.SelectedIndex = -1;
        txtSiteEmail.Text = "";
        txtSiteFax.Text = "";
        txtSiteContractor.Text = "";
        txtSitePhysAddress1.Text = "";
        txtSitePhysAddress2.Text = "";
        txtSitePhysAddress3.Text = "";
        txtSitePhysAddress4.Text = "";
        txtSiteArea.Text = "";
        txtSiteContractValue.Text = "";
        txtSiteEndDate.Text = "";
        txtSiteStartDate.Text = "";
        txtSiteGPSCoordinates.Text = "";
    }

    // Empties all forman related textboxes
    protected void ClearAllForemanFields()
    {
        txtForemanName.Text = "";
        txtForemanTelephone.Text = "";
        txtForemanCellphone.Text = "";
        txtForemanEmail.Text = "";
    }

    // Loads all the client fields
    protected void LoadAllClientFields()
    {
        int SelectedClient = -1;
        try
        {
            SelectedClient = Convert.ToInt32(ddNewClients.SelectedValue);
        }
        catch
        {
            SelectedClient = -1;
        }
        if (SelectedClient == -1)
        {
            ClearAllClientFields();
            return;
        }
        pageClientData.Visible = true;

        using (SqlConnection oConn = new SqlConnection(Connect.sConnStr))
        {
            oConn.Open();

            // Query all the client's information
            string sSql = "SELECT * FROM new_clients WHERE PKiNewClientID=" + SelectedClient.ToString();

            SqlDataReader reader = Connect.getDataCommand(sSql, oConn).ExecuteReader();
            reader.Read();
            // Check if there are rows
            if (reader.HasRows)
            {
                // Read the client's data
                txtNewClientName.Text = GetStringFromReader("sNewClientName", reader);
                txtNewClientContactPerson.Text = GetStringFromReader("sNewClientContactPerson", reader);
                txtNewClientTelephone.Text = GetStringFromReader("sNewClientTelephone", reader);
                txtNewClientCellphone.Text = GetStringFromReader("sNewClientCellphone", reader);
                txtNewClientEmail.Text = GetStringFromReader("sNewClientEmail", reader);

                // Read the consulting engineer's data
                txtCEFirmName.Text = GetStringFromReader("sCEFirmName", reader);
                txtCEContactPerson.Text = GetStringFromReader("sCEContactPerson", reader);
                txtCETelephone.Text = GetStringFromReader("sCETelephone", reader);
                txtCECellphone.Text = GetStringFromReader("sCECellphone", reader);
                txtCEEmail.Text = GetStringFromReader("sCEEmail", reader);

                // Read the project manager's data
                txtPMFirmName.Text = GetStringFromReader("sPMFirmName", reader);
                txtPMContactPerson.Text = GetStringFromReader("sPMContactPerson", reader);
                txtPMTelephone.Text = GetStringFromReader("sPMTelephone", reader);
                txtPMCellphone.Text = GetStringFromReader("sPMCellphone", reader);
                txtPMEmail.Text = GetStringFromReader("sPMEmail", reader);

                // Read the contractor's data
                txtContractorFirmName.Text = GetStringFromReader("sContractorFirmName", reader);
                txtContractorContactPerson.Text = GetStringFromReader("sContractorContactPerson", reader);
                txtContractorTelephone.Text = GetStringFromReader("sContractorTelephone", reader);
                txtContractorCellphone.Text = GetStringFromReader("sContractorCellphone", reader);
                txtContractorEmail.Text = GetStringFromReader("sContractorEmail", reader);
            }
            else
            {
                // No rows where found so clear all fields
                ClearAllClientFields();
            }
            reader.Close();

            oConn.Close();
        }
    }

    // Updates the database client entry
    protected void UpdateAllClientFields()
    {
        // Read the client's data
        string sNewClientName = txtNewClientName.Text.Trim();
        string sNewClientContactPerson = txtNewClientContactPerson.Text.Trim();
        string sNewClientTelephone = txtNewClientTelephone.Text.Trim();
        string sNewClientCellphone = txtNewClientCellphone.Text.Trim();
        string sNewClientEmail = txtNewClientEmail.Text.Trim();

        // Read the consulting engineer's data
        string sCEFirmName = txtCEFirmName.Text.Trim();
        string sCEContactPerson = txtCEContactPerson.Text.Trim();
        string sCETelephone = txtCETelephone.Text.Trim();
        string sCECellphone = txtCECellphone.Text.Trim();
        string sCEEmail = txtCEEmail.Text.Trim();

        // Read the project manager's data
        string sPMFirmName = txtPMFirmName.Text.Trim();
        string sPMContactPerson = txtPMContactPerson.Text.Trim();
        string sPMTelephone = txtPMTelephone.Text.Trim();
        string sPMCellphone = txtPMCellphone.Text.Trim();
        string sPMEmail = txtPMEmail.Text.Trim();

        // Read the contractor's data
        string sContractorFirmName = txtContractorFirmName.Text.Trim();
        string sContractorContactPerson = txtContractorContactPerson.Text.Trim();
        string sContractorTelephone = txtContractorTelephone.Text.Trim();
        string sContractorCellphone = txtContractorCellphone.Text.Trim();
        string sContractorEmail = txtContractorEmail.Text.Trim();

        int SelectedClient = -1;
        try
        {
            SelectedClient = Convert.ToInt32(ddNewClients.SelectedValue);
        }
        catch
        {
            SelectedClient = -1;
            return;
        }
        using (SqlConnection oConn = new SqlConnection(Connect.sConnStr))
        {
            oConn.Open();
            string sSql = " UPDATE new_clients SET";
            sSql += " sNewClientName = '" + sNewClientName.Trim().Replace("'", "''") + "',";
            sSql += " sNewClientContactPerson = '" + sNewClientContactPerson.Trim().Replace("'", "''") + "',";
            sSql += " sNewClientTelephone = '" + sNewClientTelephone.Trim().Replace("'", "''") + "',";
            sSql += " sNewClientCellphone = '" + sNewClientCellphone.Trim().Replace("'", "''") + "',";
            sSql += " sNewClientEmail = '" + sNewClientEmail.Trim().Replace("'", "''") + "',";

            sSql += " sCEFirmName = '" + sCEFirmName.Trim().Replace("'", "''") + "',";
            sSql += " sCEContactPerson = '" + sCEContactPerson.Trim().Replace("'", "''") + "',";
            sSql += " sCETelephone = '" + sCETelephone.Trim().Replace("'", "''") + "',";
            sSql += " sCECellphone = '" + sCECellphone.Trim().Replace("'", "''") + "',";
            sSql += " sCEEmail = '" + sCEEmail.Trim().Replace("'", "''") + "',";

            sSql += " sPMFirmName = '" + sPMFirmName.Trim().Replace("'", "''") + "',";
            sSql += " sPMContactPerson = '" + sPMContactPerson.Trim().Replace("'", "''") + "',";
            sSql += " sPMTelephone = '" + sPMTelephone.Trim().Replace("'", "''") + "',";
            sSql += " sPMCellphone = '" + sPMCellphone.Trim().Replace("'", "''") + "',";
            sSql += " sPMEmail = '" + sPMEmail.Trim().Replace("'", "''") + "',";

            sSql += " sContractorFirmName = '" + sContractorFirmName.Trim().Replace("'", "''") + "',";
            sSql += " sContractorContactPerson = '" + sContractorContactPerson.Trim().Replace("'", "''") + "',";
            sSql += " sContractorTelephone = '" + sContractorTelephone.Trim().Replace("'", "''") + "',";
            sSql += " sContractorCellphone = '" + sContractorCellphone.Trim().Replace("'", "''") + "',";
            sSql += " sContractorEmail = '" + sContractorEmail.Trim().Replace("'", "''") + "'";

            sSql += " WHERE PKiNewClientID=" + SelectedClient.ToString();
            int iRet = Connect.getDataCommand(sSql, oConn, Page).ExecuteNonQuery();

            oConn.Close();
        }
    }

    // Updates the database site entry
    protected void UpdateAllSiteFields()
    {
        string sSiteName = txtSiteName.Text.Trim();
        string sSiteTelephone = txtSiteTelephone.Text.Trim();
        string sSiteType = ddSiteType.SelectedValue.Trim();
        string sSiteEmail = txtSiteEmail.Text.Trim();
        string sSiteFax = txtSiteFax.Text.Trim();
        string sSiteContractor = txtSiteContractor.Text.Trim();
        string sSitePhysAddress1 = txtSitePhysAddress1.Text.Trim();
        string sSitePhysAddress2 = txtSitePhysAddress2.Text.Trim();
        string sSitePhysAddress3 = txtSitePhysAddress3.Text.Trim();
        string sSitePhysAddress4 = txtSitePhysAddress4.Text.Trim();
        string sSiteArea = txtSiteArea.Text.Trim();
        string sSiteContractValue = txtSiteContractValue.Text.Trim();
        CultureInfo provider = CultureInfo.InvariantCulture;
        // Correctly parse and format the date string for compatibility with SQL
        string sSiteEndDate = "";
        string sSiteStartDate = "";

        if (txtSiteEndDate.Text.Trim() != "")
        {
            sSiteEndDate = DateTime.ParseExact(txtSiteEndDate.Text.Trim(), "d/M/yyyy", provider).ToString("yyyy-MM-dd");
        }

        if (txtSiteStartDate.Text.Trim() != "")
        {
            sSiteStartDate = DateTime.ParseExact(txtSiteStartDate.Text.Trim(), "d/M/yyyy", provider).ToString("yyyy-MM-dd");
        }
        string sSiteGPSCoordinates = txtSiteGPSCoordinates.Text.Trim();


        int SelectedSite = -1;
        try
        {
            SelectedSite = Convert.ToInt32(hdnSelectedSite.Value);
        }
        catch
        {
            SelectedSite = -1;
            return;
        }

        using (SqlConnection oConn = new SqlConnection(Connect.sConnStr))
        {
            oConn.Open();
            string sSql = " UPDATE new_sites SET";
            sSql += " sSiteName = '" + sSiteName.Trim().Replace("'", "''") + "',";
            sSql += " sSiteTelephone = '" + sSiteTelephone.Trim().Replace("'", "''") + "',";
            sSql += " FKsSiteType = '" + sSiteType.Trim().Replace("'", "''") + "',";
            sSql += " sSiteEmail = '" + sSiteEmail.Trim().Replace("'", "''") + "',";
            sSql += " sSiteFax = '" + sSiteFax.Trim().Replace("'", "''") + "',";

            sSql += " sSiteContractor = '" + sSiteContractor.Trim().Replace("'", "''") + "',";
            sSql += " sSitePhysAddress1 = '" + sSitePhysAddress1.Trim().Replace("'", "''") + "',";
            sSql += " sSitePhysAddress2 = '" + sSitePhysAddress2.Trim().Replace("'", "''") + "',";
            sSql += " sSitePhysAddress3 = '" + sSitePhysAddress3.Trim().Replace("'", "''") + "',";
            sSql += " sSitePhysAddress4 = '" + sSitePhysAddress4.Trim().Replace("'", "''") + "',";

            sSql += " sSiteArea = '" + sSiteArea.Trim().Replace("'", "''") + "',";
            sSql += " iSiteContractValue = '" + sSiteContractValue.Trim().Replace("'", "''") + "',";
            sSql += " dSiteStartDate = '" + sSiteStartDate.Trim().Replace("'", "''") + "',";
            sSql += " dSiteEndDate = '" + sSiteEndDate.Trim().Replace("'", "''") + "',";
            sSql += " sSiteGPSCoordinates = '" + sSiteGPSCoordinates.Trim().Replace("'", "''") + "'";

            sSql += " WHERE PKiNewSiteID=" + SelectedSite.ToString();
            int iRet = Connect.getDataCommand(sSql, oConn, Page).ExecuteNonQuery();

            oConn.Close();
        }
    }

    // Updates the database foreman entry
    protected void UpdateAllForemanFields()
    {
        string sForemanName = txtForemanName.Text.Trim();
        string sForemanTelephone = txtForemanTelephone.Text.Trim();
        string sForemanCellphone = txtForemanCellphone.Text.Trim();
        string sForemanEmail = txtForemanEmail.Text.Trim();

        int SelectedForeman = -1;
        try
        {
            SelectedForeman = Convert.ToInt32(hdnSelectedForeman.Value);
        }
        catch
        {
            SelectedForeman = -1;
            return;
        }
        using (SqlConnection oConn = new SqlConnection(Connect.sConnStr))
        {
            oConn.Open();
            string sSql = " UPDATE new_foremen SET";
            sSql += " sForemanName = '" + sForemanName.Trim().Replace("'", "''") + "',";
            sSql += " sForemanTelephone = '" + sForemanTelephone.Trim().Replace("'", "''") + "',";
            sSql += " sForemanCellphone = '" + sForemanCellphone.Trim().Replace("'", "''") + "',";
            sSql += " sForemanEmail = '" + sForemanEmail.Trim().Replace("'", "''") + "'";

            sSql += " WHERE PKiNewForemanID=" + SelectedForeman.ToString();
            int iRet = Connect.getDataCommand(sSql, oConn, Page).ExecuteNonQuery();

            oConn.Close();
        }
    }

    // Adds a new client to the database
    protected void AddNewClient()
    {
        // Read the client's data
        string sNewClientName = txtNewClientName.Text.Trim();
        string sNewClientContactPerson = txtNewClientContactPerson.Text.Trim();
        string sNewClientTelephone = txtNewClientTelephone.Text.Trim();
        string sNewClientCellphone = txtNewClientCellphone.Text.Trim();
        string sNewClientEmail = txtNewClientEmail.Text.Trim();

        // Read the consulting engineer's data
        string sCEFirmName = txtCEFirmName.Text.Trim();
        string sCEContactPerson = txtCEContactPerson.Text.Trim();
        string sCETelephone = txtCETelephone.Text.Trim();
        string sCECellphone = txtCECellphone.Text.Trim();
        string sCEEmail = txtCEEmail.Text.Trim();

        // Read the project manager's data
        string sPMFirmName = txtPMFirmName.Text.Trim();
        string sPMContactPerson = txtPMContactPerson.Text.Trim();
        string sPMTelephone = txtPMTelephone.Text.Trim();
        string sPMCellphone = txtPMCellphone.Text.Trim();
        string sPMEmail = txtPMEmail.Text.Trim();

        // Read the contractor's data
        string sContractorFirmName = txtContractorFirmName.Text.Trim();
        string sContractorContactPerson = txtContractorContactPerson.Text.Trim();
        string sContractorTelephone = txtContractorTelephone.Text.Trim();
        string sContractorCellphone = txtContractorCellphone.Text.Trim();
        string sContractorEmail = txtContractorEmail.Text.Trim();

        using (SqlConnection oConn = new SqlConnection(Connect.sConnStr))
        {
            oConn.Open();
            string sSql = " INSERT INTO new_clients VALUES";
            sSql += "( '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "',";

            sSql += " '" + sNewClientName.Trim().Replace("'", "''") + "',";
            sSql += " '" + sNewClientContactPerson.Trim().Replace("'", "''") + "',";
            sSql += " '" + sNewClientTelephone.Trim().Replace("'", "''") + "',";
            sSql += " '" + sNewClientCellphone.Trim().Replace("'", "''") + "',";
            sSql += " '" + sNewClientEmail.Trim().Replace("'", "''") + "',";

            sSql += " '" + sCEFirmName.Trim().Replace("'", "''") + "',";
            sSql += " '" + sCEContactPerson.Trim().Replace("'", "''") + "',";
            sSql += " '" + sCETelephone.Trim().Replace("'", "''") + "',";
            sSql += " '" + sCECellphone.Trim().Replace("'", "''") + "',";
            sSql += " '" + sCEEmail.Trim().Replace("'", "''") + "',";

            sSql += " '" + sPMFirmName.Trim().Replace("'", "''") + "',";
            sSql += " '" + sPMContactPerson.Trim().Replace("'", "''") + "',";
            sSql += " '" + sPMTelephone.Trim().Replace("'", "''") + "',";
            sSql += " '" + sPMCellphone.Trim().Replace("'", "''") + "',";
            sSql += " '" + sPMEmail.Trim().Replace("'", "''") + "',";

            sSql += " '" + sContractorFirmName.Trim().Replace("'", "''") + "',";
            sSql += " '" + sContractorContactPerson.Trim().Replace("'", "''") + "',";
            sSql += " '" + sContractorTelephone.Trim().Replace("'", "''") + "',";
            sSql += " '" + sContractorCellphone.Trim().Replace("'", "''") + "',";
            sSql += " '" + sContractorEmail.Trim().Replace("'", "''") + "')";

            // Note: handle invalid chars
            int iRet = Connect.getDataCommand(sSql, oConn, Page).ExecuteNonQuery();

            oConn.Close();
        }
    }

    // Adds a new site to the database
    protected void AddNewSite()
    {
        int SelectedClient = -1;
        try
        {
            SelectedClient = Convert.ToInt32(ddNewClients.SelectedValue);
        }
        catch
        {
            SelectedClient = -1;
            return;
        }
        string sSiteName = txtSiteName.Text.Trim();
        string sSiteTelephone = txtSiteTelephone.Text.Trim();
        string sSiteType = ddSiteType.SelectedValue.Trim();
        string sSiteEmail = txtSiteEmail.Text.Trim();
        string sSiteFax = txtSiteFax.Text.Trim();
        string sSiteContractor = txtSiteContractor.Text.Trim();
        string sSitePhysAddress1 = txtSitePhysAddress1.Text.Trim();
        string sSitePhysAddress2 = txtSitePhysAddress2.Text.Trim();
        string sSitePhysAddress3 = txtSitePhysAddress3.Text.Trim();
        string sSitePhysAddress4 = txtSitePhysAddress4.Text.Trim();
        string sSiteArea = txtSiteArea.Text.Trim();
        string sSiteContractValue = txtSiteContractValue.Text.Trim();
        // Correctly parse and format the date string for compatibility with SQL
        CultureInfo provider = CultureInfo.InvariantCulture;
        
        string sSiteEndDate = "";
        string sSiteStartDate = ""; 

        if(txtSiteEndDate.Text.Trim() != "")
        {
            sSiteEndDate = DateTime.ParseExact(txtSiteEndDate.Text.Trim(), "d/M/yyyy", provider).ToString("yyyy-MM-dd");
        }

        if(txtSiteStartDate.Text.Trim() != "")
        {
           sSiteStartDate = DateTime.ParseExact(txtSiteStartDate.Text.Trim(), "d/M/yyyy", provider).ToString("yyyy-MM-dd");
        }
        string sSiteGPSCoordinates = txtSiteGPSCoordinates.Text.Trim();

        using (SqlConnection oConn = new SqlConnection(Connect.sConnStr))
        {
            oConn.Open();
            string sSql = " INSERT INTO new_sites VALUES";
            sSql += "( '" + SelectedClient.ToString() + "',";
            sSql += " '" + sSiteName.Trim().Replace("'", "''") + "',";
            sSql += " '" + sSiteType.Trim().Replace("'", "''") + "',";
            sSql += " '" + sSiteContractor.Trim().Replace("'", "''") + "',";
            sSql += " '" + sSiteTelephone.Trim().Replace("'", "''") + "',";
            sSql += " '" + sSiteFax.Trim().Replace("'", "''") + "',";
            sSql += " '" + sSitePhysAddress1.Trim().Replace("'", "''") + "',";
            sSql += " '" + sSitePhysAddress2.Trim().Replace("'", "''") + "',";
            sSql += " '" + sSitePhysAddress3.Trim().Replace("'", "''") + "',";
            sSql += " '" + sSitePhysAddress4.Trim().Replace("'", "''") + "',";
            sSql += " '" + sSiteEmail.Trim().Replace("'", "''") + "',";
            sSql += " '" + sSiteArea.Trim().Replace("'", "''") + "',";
            sSql += " '" + sSiteContractValue.Trim().Replace("'", "''") + "',";
            sSql += " '" + sSiteStartDate.Trim().Replace("'", "''") + "',";
            sSql += " '" + sSiteEndDate.Trim().Replace("'", "''") + "',";
            sSql += " '" + sSiteGPSCoordinates.Trim().Replace("'", "''") + "')";

            int iRet = Connect.getDataCommand(sSql, oConn, Page).ExecuteNonQuery();

            oConn.Close();
        }
    }

    // Adds a new foreman to the database
    protected void AddNewForeman()
    {
        int SelectedSite = -1;
        try
        {
            SelectedSite = Convert.ToInt32(hdnSelectedSite.Value);
        }
        catch
        {
            SelectedSite = -1;
            return;
        }
        string sForemanName = txtForemanName.Text.Trim();
        string sForemanTelephone = txtForemanTelephone.Text.Trim();
        string sForemanCellphone = txtForemanCellphone.Text.Trim();
        string sForemanEmail = txtForemanEmail.Text.Trim();

        using (SqlConnection oConn = new SqlConnection(Connect.sConnStr))
        {
            oConn.Open();
            string sSql = " INSERT INTO new_foremen VALUES";
            sSql += "( '" + SelectedSite.ToString() + "',";
            sSql += " '" + sForemanName.Trim().Replace("'", "''") + "',";
            sSql += " '" + sForemanTelephone.Trim().Replace("'", "''") + "',";
            sSql += " '" + sForemanCellphone.Trim().Replace("'", "''") + "',";
            sSql += " '" + sForemanEmail.Trim().Replace("'", "''") + "')";

            int iRet = Connect.getDataCommand(sSql, oConn, Page).ExecuteNonQuery();

            oConn.Close();
        }
    }

    // Deletes the database client entry
    protected void DeleteNewClient()
    {
        int SelectedClient = -1;
        try
        {
            SelectedClient = Convert.ToInt32(ddNewClients.SelectedValue);
        }
        catch
        {
            SelectedClient = -1;
            return;
        }
        using (SqlConnection oConn = new SqlConnection(Connect.sConnStr))
        {
            oConn.Open();
            // Delete foremen who are connected to the sites of the client being deleted
            string sSql = " DELETE FROM new_foremen";
            sSql += " WHERE FKiNewSiteID IN";
            sSql += " (SELECT PKiNewSiteID FROM new_sites";
            sSql += " WHERE FKiNewClientID=" + SelectedClient.ToString();
            sSql += ");";
            int iRet = Connect.getDataCommand(sSql, oConn, Page).ExecuteNonQuery();
            // Delete the sites connected to the client being deleted
            sSql = " DELETE FROM new_sites";
            sSql += " WHERE FKiNewClientID=" + SelectedClient.ToString();
            iRet = Connect.getDataCommand(sSql, oConn, Page).ExecuteNonQuery();
            // Delete the client
            sSql = " DELETE FROM new_clients";
            sSql += " WHERE PKiNewClientID=" + SelectedClient.ToString();
            iRet = Connect.getDataCommand(sSql, oConn, Page).ExecuteNonQuery();

            oConn.Close();
        }
    }

    // Deletes the database site entry
    protected void DeleteNewSite()
    {
        int SelectedSite = -1;
        try
        {
            SelectedSite = Convert.ToInt32(hdnSelectedSite.Value);
        }
        catch
        {
            SelectedSite = -1;
            return;
        }
        using (SqlConnection oConn = new SqlConnection(Connect.sConnStr))
        {
            oConn.Open();
            // Delete the foremen connected to the site being deleted
            string sSql = " DELETE FROM new_foremen";
            sSql += " WHERE FKiNewSiteID =" + SelectedSite.ToString();
            int iRet = Connect.getDataCommand(sSql, oConn, Page).ExecuteNonQuery();
            // Delete the site
            sSql = " DELETE FROM new_sites";
            sSql += " WHERE PKiNewSiteID=" + SelectedSite.ToString();
            iRet = Connect.getDataCommand(sSql, oConn, Page).ExecuteNonQuery();

            oConn.Close();
        }
    }

    // Deletes the database foreman entry
    protected void DeleteNewForeman()
    {
        int SelectedForeman = -1;
        try
        {
            SelectedForeman = Convert.ToInt32(hdnSelectedForeman.Value);
        }
        catch
        {
            SelectedForeman = -1;
            return;
        }
        using (SqlConnection oConn = new SqlConnection(Connect.sConnStr))
        {
            oConn.Open();
            // Delete the foreman
            string sSql = " DELETE FROM new_foremen";
            sSql += " WHERE PKiNewForemanID =" + SelectedForeman.ToString();
            sSql += ";";
            int iRet = Connect.getDataCommand(sSql, oConn, Page).ExecuteNonQuery();

            oConn.Close();
        }
    }

    // Load the client fields when a new client was selected
    protected void ddNewClients_SelectedIndexChanged(object sender, EventArgs e)
    {
        // Deselect site and foreman
        hdnSelectedSite.Value = "-1";
        hdnSelectedForeman.Value = "-1";

        // Hide buttons
        btnViewSiteDetails.Style["display"] = "none";
        btnDeleteSite.Style["display"] = "none";
        btnViewForemanDetails.Style["display"] = "none";
        btnDeleteForeman.Style["display"] = "none";
        btnNewForeman.Style["display"] = "none";

        // Load data
        LoadAllClientFields();
        GenerateSitesTable();
        GenerateForemenTable();
    }

    // Update the database with client data
    protected void btnUpdateClient_Click(object sender, EventArgs e)
    {
        int SelectedClient = -1;
        try
        {
            SelectedClient = Convert.ToInt32(ddNewClients.SelectedValue);
        }
        catch
        {
            SelectedClient = -1;
            return;
        }
        UpdateAllClientFields();
        LoadNewClientNames();
        // Select same client again
        ddNewClients.SelectedValue = SelectedClient.ToString();

        // Load data
        GenerateSitesTable();
        GenerateForemenTable();
    }
    protected void btnClientFinder_Click(object sender, EventArgs e)
    {

    }
    // Add new client to the database
    protected void btnSubmitClient_Click(object sender, EventArgs e)
    {
        string newClient = txtNewClientName.Text.Trim();
        AddNewClient();
        LoadNewClientNames();
        // Select new client
        ddNewClients.SelectedValue = ddNewClients.Items.FindByText(newClient).Value;
        // Hide buttons
        btnSubmitClient.Style["display"] = "none";
        btnClientCancel.Style["display"] = "none";

        // Deselect foremen and sites
        hdnSelectedSite.Value = "-1";
        hdnSelectedForeman.Value = "-1";

        // Load data
        GenerateSitesTable();
        GenerateForemenTable();
    }
    // Delete a client
    protected void btnDeleteClient_Click(object sender, EventArgs e)
    {
        // Deselect all
        hdnSelectedSite.Value = "-1";
        hdnSelectedForeman.Value = "-1";
        DeleteNewClient();
        // Load data
        LoadNewClientNames();
        LoadAllClientFields();
        GenerateSitesTable();
        GenerateForemenTable();
    }
    // Filter the sites table
    protected void btnFilterSites_Click(object sender, ImageClickEventArgs e)
    {
        // Deselect all
        hdnSelectedSite.Value = "-1";
        hdnSelectedForeman.Value = "-1";

        // Load filtered data
        GenerateSitesTable();
        LoadAllSiteFields();
        GenerateForemenTable();
    }
    // Filter the foremen table
    protected void btnFilterForemen_Click(object sender, ImageClickEventArgs e)
    {
        // Deselect foreman
        hdnSelectedForeman.Value = "-1";

        // Load filtered data
        GenerateSitesTable();
        GenerateForemenTable();
        LoadAllForemanFields();
    }

    // Load the filtered site table
    protected void GenerateSitesTable()
    {
        using (SqlConnection oConn = new SqlConnection(Connect.sConnStr))
        {
            int SelectedClient = -1;
            try
            {
                SelectedClient = Convert.ToInt32(ddNewClients.SelectedValue);
            }
            catch
            {
                SelectedClient = -1;
                return;
            }
            string sSQL = "SELECT * FROM new_sites WHERE";
            sSQL += " FKiNewClientID=" + SelectedClient.ToString() + " AND (";
            sSQL += "(sSiteName LIKE '%" + txtFilterSites.Text.Trim().Replace("'", "''") + "%' ESCAPE '\\') OR ";
            sSQL += "(FKsSiteType LIKE '%" + txtFilterSites.Text.Trim().Replace("'", "''") + "%' ESCAPE '\\') OR ";
            sSQL += "(sSiteContractor LIKE '%" + txtFilterSites.Text.Trim().Replace("'", "''") + "%' ESCAPE '\\') OR ";
            sSQL += "(sSiteTelephone LIKE '%" + txtFilterSites.Text.Trim().Replace("'", "''") + "%' ESCAPE '\\') OR ";
            sSQL += "(sSiteFax LIKE '%" + txtFilterSites.Text.Trim().Replace("'", "''") + "%' ESCAPE '\\') OR ";
            sSQL += "(sSitePhysAddress1 LIKE '%" + txtFilterSites.Text.Trim().Replace("'", "''") + "%' ESCAPE '\\') OR ";
            sSQL += "(sSitePhysAddress2 LIKE '%" + txtFilterSites.Text.Trim().Replace("'", "''") + "%' ESCAPE '\\') OR ";
            sSQL += "(sSitePhysAddress3 LIKE '%" + txtFilterSites.Text.Trim().Replace("'", "''") + "%' ESCAPE '\\') OR ";
            sSQL += "(sSitePhysAddress4 LIKE '%" + txtFilterSites.Text.Trim().Replace("'", "''") + "%' ESCAPE '\\') OR ";

            sSQL += "(sSiteEmail LIKE '%" + txtFilterSites.Text.Trim().Replace("'", "''") + "%' ESCAPE '\\')";
            sSQL += ")";

            sSQL += " ORDER BY sSiteName";

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

                    HtmlTableCell tcSiteName = new HtmlTableCell();
                    tcSiteName.Height = "25px";
                    tcSiteName.InnerHtml = "&nbsp;" + GetStringFromReader("sSiteName", rdReader);
                    tr.Cells.Add(tcSiteName);

                    HtmlTableCell tcSiteType = new HtmlTableCell();
                    tcSiteType.Height = "25px";
                    tcSiteType.InnerHtml = "&nbsp;" + GetStringFromReader("FKsSiteType", rdReader);
                    tr.Cells.Add(tcSiteType);

                    HtmlTableCell tcSiteContractor = new HtmlTableCell();
                    tcSiteContractor.Height = "25px";
                    tcSiteContractor.InnerHtml = "&nbsp;" + GetStringFromReader("sSiteContractor", rdReader);
                    tr.Cells.Add(tcSiteContractor);

                    HtmlTableCell tcSiteTelephone = new HtmlTableCell();
                    tcSiteTelephone.Height = "25px";
                    tcSiteTelephone.InnerHtml = "&nbsp;" + GetStringFromReader("sSiteTelephone", rdReader);
                    tr.Cells.Add(tcSiteTelephone);

                    HtmlTableCell tcFax = new HtmlTableCell();
                    tcFax.Height = "25px";
                    tcFax.InnerHtml = "&nbsp;" + GetStringFromReader("sSiteFax", rdReader);
                    tr.Cells.Add(tcFax);

                    HtmlTableCell tcForemenLink= new HtmlTableCell();
                    tcForemenLink.Height = "25px";
                    tcForemenLink.Width = "auto";
                    tcForemenLink.InnerHtml = "&nbsp;" + "<a onclick=\"return ShowForemanTable('" + GetStringFromReader("sSiteName", rdReader) + "');\" style=\"text-decoration:underline;\">Show Table</a>";
                    tr.Cells.Add(tcForemenLink);

                    //SET ATTRIBUTES IN ROW
                    tr.Attributes.Add("onclick", "SelectSite('" + GetInt32FromReader("PKiNewSiteID", rdReader).ToString() + "');");
                    tr.Attributes.Add("title", "Click on a row to select...");
                    tr.Style.Add("cursor", "pointer");
                    tr.Style.Add("width", "100%");
                    tblSites.Rows.Add(tr);

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
                tcNoData.ColSpan = 6;
                tcNoData.Align = "center";
                tcNoData.InnerHtml = "&nbsp; NO DATA CURRENTLY AVAILABLE";
                trNoData.Cells.Add(tcNoData);

                tblSites.Rows.Add(trNoData);

                //***********************************************
            }

            rdReader.Close();
        }
    }

    // Loads all the Site fields
    protected void LoadAllSiteFields()
    {
        int SelectedSite = -1;
        try
        {
            SelectedSite = Convert.ToInt32(hdnSelectedSite.Value);
        }
        catch
        {
            SelectedSite = -1;
        }
        if (SelectedSite == -1)
        {
            //  btnEditClient.Enabled = false;
            ClearAllSiteFields();
            return;
        }
        pageClientData.Visible = true;

        using (SqlConnection oConn = new SqlConnection(Connect.sConnStr))
        {
            oConn.Open();

            // Query all the client's information
            string sSql = "SELECT * FROM new_sites WHERE PKiNewSiteID=" + SelectedSite.ToString();

            SqlDataReader reader = Connect.getDataCommand(sSql, oConn).ExecuteReader();
            reader.Read();
            // Check if there are rows
            if (reader.HasRows)
            {
                txtSiteName.Text = GetStringFromReader("sSiteName", reader);
                txtSiteTelephone.Text = GetStringFromReader("sSiteTelephone", reader);
                //  LoadSiteTypes();
                ddSiteType.SelectedValue = GetStringFromReader("FKsSiteType", reader);
                txtSiteEmail.Text = GetStringFromReader("sSiteEmail", reader);
                txtSiteFax.Text = GetStringFromReader("sSiteFax", reader);
                txtSiteContractor.Text = GetStringFromReader("sSiteContractor", reader);
                txtSitePhysAddress1.Text = GetStringFromReader("sSitePhysAddress1", reader);
                txtSitePhysAddress2.Text = GetStringFromReader("sSitePhysAddress2", reader);
                txtSitePhysAddress3.Text = GetStringFromReader("sSitePhysAddress3", reader);
                txtSitePhysAddress4.Text = GetStringFromReader("sSitePhysAddress4", reader);
                txtSiteArea.Text = GetStringFromReader("sSiteArea", reader);
                txtSiteContractValue.Text = GetMoneyFromReader("iSiteContractValue", reader).ToString();
                txtSiteEndDate.Text = GetDateFromReader("dSiteEndDate", reader).ToString("d/M/yyyy");
                txtSiteStartDate.Text = GetDateFromReader("dSiteStartDate", reader).ToString("d/M/yyyy");
                CultureInfo provider = CultureInfo.InvariantCulture;
                txtSiteStartDate_CalendarExtender.SelectedDate = DateTime.ParseExact(txtSiteStartDate.Text.Trim(), "d/M/yyyy", provider);
                txtSiteEndDate_CalendarExtender.SelectedDate = DateTime.ParseExact(txtSiteEndDate.Text.Trim(), "d/M/yyyy", provider);
                txtSiteGPSCoordinates.Text = GetStringFromReader("sSiteGPSCoordinates", reader);
            }
            else
            {
                // No rows where found so clear all fields
                ClearAllSiteFields();
            }
            reader.Close();

            oConn.Close();
        }
    }

    // Loads all the foreman fields
    protected void LoadAllForemanFields()
    {
        int SelectedForeman = -1;
        try
        {
            SelectedForeman = Convert.ToInt32(hdnSelectedForeman.Value);
        }
        catch
        {
            SelectedForeman = -1;
            return;
        }
        if (SelectedForeman == -1)
        {
            //  btnEditClient.Enabled = false;
            ClearAllForemanFields();
            return;
        }
        pageClientData.Visible = true;

        using (SqlConnection oConn = new SqlConnection(Connect.sConnStr))
        {
            oConn.Open();

            // Query all the client's information
            string sSql = "SELECT * FROM new_foremen WHERE PKiNewForemanID=" + SelectedForeman.ToString();

            SqlDataReader reader = Connect.getDataCommand(sSql, oConn).ExecuteReader();
            reader.Read();
            // Check if there are rows
            if (reader.HasRows)
            {
                txtForemanName.Text = GetStringFromReader("sForemanName", reader);
                txtForemanTelephone.Text = GetStringFromReader("sForemanTelephone", reader);
                txtForemanCellphone.Text = GetStringFromReader("sForemanCellphone", reader);
                txtForemanEmail.Text = GetStringFromReader("sForemanEmail", reader);
            }
            else
            {
                // No rows where found so clear all fields
                ClearAllForemanFields();
            }
            reader.Close();

            oConn.Close();
        }
    }

    // Load the filtered foreman table
    protected void GenerateForemenTable()
    {
        using (SqlConnection oConn = new SqlConnection(Connect.sConnStr))
        {
            int SelectedSite = -1;
            try
            {
                SelectedSite = Convert.ToInt32(hdnSelectedSite.Value);
            }
            catch
            {
                SelectedSite = -1;
                return;
            }
            string sSQL = "SELECT * FROM new_foremen WHERE";
            sSQL += " FKiNewSiteID=" + SelectedSite.ToString() + " AND (";
            sSQL += "(sForemanName LIKE '%" + txtFilterForemen.Text.Trim().Replace("'", "''") + "%' ESCAPE '\\') OR ";
            sSQL += "(sForemanTelephone LIKE '%" + txtFilterForemen.Text.Trim().Replace("'", "''") + "%' ESCAPE '\\') OR ";
            sSQL += "(sForemanCellphone LIKE '%" + txtFilterForemen.Text.Trim().Replace("'", "''") + "%' ESCAPE '\\') OR ";
            sSQL += "(sForemanEmail LIKE '%" + txtFilterForemen.Text.Trim().Replace("'", "''") + "%' ESCAPE '\\')";
            sSQL += ")";

            sSQL += " ORDER BY sForemanName";

            SqlDataReader rdReader = Connect.getDataCommand(sSQL, oConn, Page).ExecuteReader();

            int iRowCount = 0;
            string sBackColor = "#EFF3FB";

            if (rdReader.HasRows)
            {
                while (rdReader.Read())
                {
                    //Set Colors 
                    if ((iRowCount % 2) == 0)
                    {
                        sBackColor = "#EFF3FB";
                    }
                    else
                    {
                        sBackColor = "#FFFFFF";
                    }

                    //******* CREATE HTML TABLE ROWS AND ADD TO TABLE *******

                    HtmlTableRow tr = new HtmlTableRow();
                    tr.BgColor = sBackColor;

                    HtmlTableCell tcForemanName = new HtmlTableCell();
                    tcForemanName.Height = "25px";
                    tcForemanName.InnerHtml = "&nbsp;" + GetStringFromReader("sForemanName", rdReader);
                    tr.Cells.Add(tcForemanName);

                    HtmlTableCell tcForemanTelephone = new HtmlTableCell();
                    tcForemanTelephone.Height = "25px";
                    tcForemanTelephone.InnerHtml = "&nbsp;" + GetStringFromReader("sForemanTelephone", rdReader);
                    tr.Cells.Add(tcForemanTelephone);

                    HtmlTableCell tcForemanCellphone = new HtmlTableCell();
                    tcForemanCellphone.Height = "25px";
                    tcForemanCellphone.InnerHtml = "&nbsp;" + GetStringFromReader("sForemanCellphone", rdReader);
                    tr.Cells.Add(tcForemanCellphone);

                    HtmlTableCell tcForemanEmail = new HtmlTableCell();
                    tcForemanEmail.Height = "25px";
                    tcForemanEmail.InnerHtml = "&nbsp;" + GetStringFromReader("sForemanEmail", rdReader);
                    tr.Cells.Add(tcForemanEmail);

                    //SET ATTRIBUTES IN ROW
                    tr.Attributes.Add("onclick", "SelectForeman('" + GetInt32FromReader("PKiNewForemanID", rdReader) + "');");
                    tr.Attributes.Add("title", "Click on a row to select...");
                    tr.Style.Add("cursor", "pointer");
                    tr.Style.Add("width", "100%");
                    tblForemen.Rows.Add(tr);

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
                tcNoData.ColSpan = 4;
                tcNoData.Align = "center";
                tcNoData.InnerHtml = "&nbsp; NO DATA CURRENTLY AVAILABLE";
                trNoData.Cells.Add(tcNoData);

                tblForemen.Rows.Add(trNoData);

                //***********************************************
            }

            rdReader.Close();
        }
    }

    // Handle selected site change
    protected void hdnSelectedSite_ValueChanged(object sender, EventArgs e)
    {
        // Hide unwanted buttons
        if (Convert.ToInt32(hdnSelectedSite.Value) != -1)
        {
            btnViewSiteDetails.Style["display"] = "inline";
            btnDeleteSite.Style["display"] = "inline";
            btnNewForeman.Style["display"] = "inline";
        }
        else
            btnNewForeman.Style["display"] = "none";

        // Load appropriate data
        GenerateSitesTable();
        LoadAllSiteFields();
        GenerateForemenTable();
    }

    // Handle selected foreman change
    protected void hdnSelectedForeman_ValueChanged(object sender, EventArgs e)
    {
        // Hide unwanted buttons
        if (Convert.ToInt32(hdnSelectedSite.Value) != -1)
        {
            btnViewForemanDetails.Style["display"] = "inline";
            btnDeleteForeman.Style["display"] = "inline";
        }

        // Load appropriate data
        GenerateSitesTable();
        LoadAllSiteFields();
        GenerateForemenTable();
        LoadAllForemanFields();
    }

    // Handle a client edit cancel
    protected void btnClientCancel_Click(object sender, EventArgs e)
    {
        // Reload all data
        LoadAllClientFields();
        GenerateSitesTable();
        LoadAllSiteFields();
        GenerateForemenTable();
        LoadAllForemanFields();
    }
    // Handle new site data
    protected void btnSubmitEditSitesPopup_Click(object sender, EventArgs e)
    {
        AddNewSite();
        GenerateSitesTable();
        LoadAllSiteFields();
        GenerateForemenTable();
    }
    // Handle site edit cancel
    protected void btnCancelEditSitesPopup_Click(object sender, EventArgs e)
    {
        LoadAllClientFields();
        GenerateSitesTable();
        LoadAllSiteFields();
        GenerateForemenTable();
        LoadAllForemanFields();
    }
    // Handle new foreman data
    protected void btnSubmitEditForemenPopup_Click(object sender, EventArgs e)
    {
        AddNewForeman();
        GenerateSitesTable();
        GenerateForemenTable();
        LoadAllForemanFields();
    }
    // Handle site cancel
    protected void btnCancelEditForemenPopup_Click(object sender, EventArgs e)
    {
        LoadAllClientFields();
        GenerateSitesTable();
        LoadAllSiteFields();
        GenerateForemenTable();
        LoadAllForemanFields();
    }
    // Handle forman delete
    protected void btnDeleteForeman_Click(object sender, EventArgs e)
    {
        GenerateSitesTable();
        LoadAllSiteFields();
        DeleteNewForeman();
        btnViewForemanDetails.Style["display"] = "none";
        btnDeleteForeman.Style["display"] = "none";
        hdnSelectedForeman.Value = "-1";
        GenerateForemenTable();
        LoadAllForemanFields();
    }
    // Handle site delete
    protected void btnDeleteSite_Click(object sender, EventArgs e)
    {
        DeleteNewSite();
        hdnSelectedSite.Value = "-1";
        hdnSelectedForeman.Value = "-1";
        btnNewForeman.Style["display"] = "none";
        btnViewForemanDetails.Style["display"] = "none";
        btnDeleteForeman.Style["display"] = "none";
        btnViewSiteDetails.Style["display"] = "none";
        btnDeleteSite.Style["display"] = "none";
        GenerateSitesTable();
        GenerateForemenTable();
    }
    // Handle updated foreman data
    protected void btnUpdateEditForemenPopup_Click(object sender, EventArgs e)
    {
        UpdateAllForemanFields();
        GenerateSitesTable();
        GenerateForemenTable();
        LoadAllForemanFields();
    }
    // Handle updated site data
    protected void btnUpdateEditSitesPopup_Click(object sender, EventArgs e)
    {
        UpdateAllSiteFields();
        GenerateSitesTable();
        LoadAllSiteFields();
        GenerateForemenTable();
    }
    protected void hdnForemenVisible_ValueChanged(object sender, EventArgs e)
    {
        int bVisible = Int32.Parse(hdnForemenVisible.Value);

        popupForemenTable.ShowOnPageLoad = (bVisible == 1);
        popupForemenTable.HeaderText = hdnSelectedSiteName.Value;
    }
    protected void gvClientDetails_SelectedIndexChanged(object sender, EventArgs e)
    {
        popupClientFinder.ShowOnPageLoad = false;
        ddNewClients.SelectedValue = gvClientDetails.SelectedDataKey.Value.ToString();
        hdnSelectedSite.Value = "-1";
        hdnSelectedForeman.Value = "-1";
        // Load all the data
        LoadNewClientNames();
        LoadAllClientFields();
        GenerateSitesTable();
        GenerateForemenTable();
        gvClientDetails.DataBind();
    }
    protected void txtClientFinderFilter_TextChanged(object sender, EventArgs e)
    {
        gvClientDetails.DataBind();
    }
    protected void btnClientSearch_Click(object sender, ImageClickEventArgs e)
    {
        gvClientDetails.DataBind();
    }
    protected void gvClientDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "this.style.cursor='pointer';this.style.textDecoration='underline';";
            e.Row.Attributes["onmouseout"] = "this.style.textDecoration='none';";

            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(gvClientDetails, "Select$" + e.Row.RowIndex);
        }
    }
}
