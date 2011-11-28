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

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
       
    }

    //Handle User Login
    protected void btnLogin_Click(object sender, EventArgs e)
    {
        string sUsername = txtUsername.Value.Trim();
        string sPassword = txtPass.Value.Trim();

        using (SqlConnection oConn = new SqlConnection(Connect.sConnStr))
        {
            oConn.Open();

            string sSQL = "SELECT COUNT(PKiUserID) FROM users";
            sSQL += " WHERE sUsername = '" + txtUsername.Value.Trim().Replace("'", "''") + "'";
            sSQL += " AND sPassword = '" + txtPass.Value.Trim().Replace("'", "''") + "'";

            int iCount = Convert.ToInt32(Connect.getDataCommand(sSQL, oConn, Page).ExecuteScalar());

            if (iCount > 0)
            {
                sSQL = "SELECT PKiUserID,sName,sSurname,sEmail,sType FROM users";
                sSQL += " WHERE sUsername = '" + txtUsername.Value.Trim().Replace("'", "''") + "'";
                sSQL += " AND sPassword = '" + txtPass.Value.Trim().Replace("'", "''") + "'";

                SqlDataReader rdReader = Connect.getDataCommand(sSQL, oConn, Page).ExecuteReader();

                if (rdReader.HasRows)
                {
                    while (rdReader.Read())
                    {
                        //Set "Session" LOGIN VARS
                        Session["LoggedInUserID"] = rdReader["PKiUserID"].ToString();
                        Session["TypeUser"] = rdReader["sType"].ToString();
                    }

                    rdReader.Close();
                }

                //Redirect
                Response.Redirect("pages/Home.aspx");
            }
            else
            {
                General.AlertClient("Invalid username/password. Access denied.");
            }

            oConn.Close();
        }
    }
}
