using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using DBITWebControls;

public partial class pages_new_user : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["LoggedInUserID"] == null)
        {
            Response.Write("<script type=\"text/javascript\">window.top.location.href = '" + Page.ResolveClientUrl("~/Default.aspx") + "';</script>");
            Response.Flush();
            return;
        }
        if (!IsPostBack)
        {
            GenerateExistingUsersTable();
            txtColorCode.Attributes.Add("readonly", "readonly");
        }
    }

    protected void GenerateExistingUsersTable()
    {
        using (SqlConnection oConn = new SqlConnection(Connect.sConnStr))
        {
            string sSQL = "SELECT * FROM users";
            sSQL += " WHERE ";

            sSQL += "(";
            sSQL += "(sName LIKE '%" + txtFilter.Value + "%' OR '' = '" + txtFilter.Value + "') OR ";
            sSQL += "(sSurname LIKE '%" + txtFilter.Value + "%' OR '' = '" + txtFilter.Value + "') OR ";
            sSQL += "(sPhoneNumber LIKE '%" + txtFilter.Value + "%' OR '' = '" + txtFilter.Value + "') OR ";
            sSQL += "(sType LIKE '%" + txtFilter.Value + "%' OR '' = '" + txtFilter.Value + "') OR ";
            sSQL += "(sEmail LIKE '%" + txtFilter.Value + "%' OR '' = '" + txtFilter.Value + "')";
            sSQL += ")";

            sSQL += " ORDER BY sName";

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

                    HtmlTableCell tcFName = new HtmlTableCell();
                    tcFName.Height = "25px";
                    tcFName.InnerHtml = "&nbsp;" + rdReader["sName"].ToString();
                    tr.Cells.Add(tcFName);

                    HtmlTableCell tcLName = new HtmlTableCell();
                    tcLName.Height = "25px";
                    tcLName.InnerHtml = "&nbsp;" + rdReader["sSurname"].ToString();
                    tr.Cells.Add(tcLName);

                    HtmlTableCell tcPhone = new HtmlTableCell();
                    tcPhone.Height = "25px";
                    tcPhone.InnerHtml = "&nbsp;" + rdReader["sPhoneNumber"].ToString();
                    tr.Cells.Add(tcPhone);

                    HtmlTableCell tcEmail = new HtmlTableCell();
                    tcEmail.Height = "25px";
                    tcEmail.InnerHtml = "&nbsp;" + rdReader["sEmail"].ToString();
                    tr.Cells.Add(tcEmail);

                    HtmlTableCell tcType = new HtmlTableCell();
                    tcType.Height = "25px";
                    tcType.InnerHtml = "&nbsp;" + rdReader["sType"].ToString();
                    tr.Cells.Add(tcType);

                    //SET ATTRIBUTES IN ROW
                    tr.Attributes.Add("onclick", "SelectUser('" + rdReader["PKiUserID"].ToString().Trim() + "','" + rdReader["sName"].ToString().Trim() + "','" + rdReader["sSurname"].ToString().Trim() + "','" + rdReader["sEmail"].ToString().Trim() + "','" + rdReader["sPhoneNumber"].ToString().Trim() + "','" + rdReader["sUsername"].ToString().Trim() + "','" + rdReader["sPassword"].ToString().Trim() + "','" + rdReader["sColorCode"].ToString().Trim() + "','" + rdReader["sType"].ToString().Trim() + "')");
                    tr.Attributes.Add("title", "Click on a row to select...");
                    tr.Style.Add("cursor", "pointer");

                    tblUsers.Rows.Add(tr);

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
                tcNoData.ColSpan = 3;
                tcNoData.Align = "center";
                tcNoData.InnerHtml = "&nbsp; NO DATA CURRENTLY AVAILABLE";
                trNoData.Cells.Add(tcNoData);

                tblUsers.Rows.Add(trNoData);

                //***********************************************
            }

            rdReader.Close();
        }
    }

    protected void btnSaveUser_Click(object sender, EventArgs e)
    {
        using (SqlConnection oConn = new SqlConnection(Connect.sConnStr))
        {
            if (txtUserId.Value == "")
            {
                AddNewUser(oConn);
            }
            else
            {
                UpdateUser(oConn);
            }
        }
    }

    //ADD NEW USER TO DB
    protected void AddNewUser(SqlConnection oConn)
    {
        oConn.Open();

        string sSQL = "INSERT INTO users (sName,sSurname,sUsername,sPassword,sColorCode,sEmail,sType,sPhoneNumber) VALUES ";
        sSQL += " ('" + txtName.Value + "',";
        sSQL += " '" + txtSurname.Value + "',";
        sSQL += " '" + txtUsername.Value + "',";        

        sSQL += " '" + txtPass.Value + "',";
        sSQL += " '" + txtColorCode.Text + "',";
        sSQL += " '" + txtEmail.Value + "',";
        sSQL += " '" + selUserType.Value + "',";
        sSQL += " '" + txtPhoneNumber.Value + "')";        

        int iRet = Connect.getDataCommand(sSQL, oConn, Page).ExecuteNonQuery();

        if (iRet == 1)
        {
            General.AlertClient("New user sucessfully added");
            GenerateExistingUsersTable();
        }
        else
        {
            General.AlertClient("An error occurred. Please try again.");
            GenerateExistingUsersTable();
        }

        oConn.Close();
    }

    //UPDATE EXISTING USER
    protected void UpdateUser(SqlConnection oConn)
    {
        oConn.Open();

        string sSQL = "UPDATE users SET";
        sSQL += " sName = '" + txtName.Value + "'";
        sSQL += ",sSurname = '" + txtSurname.Value + "'";
        sSQL += ",sEmail = '" + txtEmail.Value + "'";        

        sSQL += ",sUsername = '" + txtUsername.Value + "'";
        sSQL += ",sPassword = '" + txtPass.Value + "'";

        sSQL += ",sColorCode = '" + txtColorCode.Text + "'";
        sSQL += ",sPhoneNumber = '" + txtPhoneNumber.Value + "'";

        sSQL += ",sType = '" + selUserType.Value + "'";   
        sSQL += " WHERE PKiUserID = '" + txtUserId.Value + "'";

        int iRet = Connect.getDataCommand(sSQL, oConn, Page).ExecuteNonQuery();

        if (iRet == 1)
        {
            General.AlertClient("User details sucessfully updated");
            GenerateExistingUsersTable();
        }
        else
        {
            General.AlertClient("An error occurred on update. Please try again.");
            GenerateExistingUsersTable();
        }

        oConn.Close();
    }

    protected void btnFilterUser_Click(object sender, EventArgs e)
    {
        GenerateExistingUsersTable();
    }

    protected void btnDelUser_Click(object sender, EventArgs e)
    {
        using (SqlConnection oConn = new SqlConnection(Connect.sConnStr))
        {
            if (txtUserId.Value != "")
            {
                string sSQL = "DELETE FROM users";
                sSQL += " WHERE PKiUserID = '" + txtUserId.Value + "'";

                int iRet = Connect.getDataCommand(sSQL, oConn, Page).ExecuteNonQuery();

                if (iRet > 0)
                    General.AlertClient("User has been successfully removed");
                else
                    General.AlertClient("An error occurred during removal, please try again!");

                GenerateExistingUsersTable();

            }
            else
            {
                General.AlertClient("No user was selected for removal!");
                GenerateExistingUsersTable();
            }

            oConn.Close();
        }

        ClearFields();
    }

    protected void ClearFields()
    {
        txtUserId.Value = "";
        
        txtName.Value = "";
        txtSurname.Value = "";
        txtEmail.Value = "";
        txtPhoneNumber.Value = "";

        txtUsername.Value = "";
        txtPass.Value = "";
    }
}