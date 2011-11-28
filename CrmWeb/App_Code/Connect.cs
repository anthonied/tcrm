using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;

public class Connect
{
    //public static string sConnStr = @"Server=BL8ND13-DESKTOP;Database=sol_tbcrm;Trusted_Connection=True;";
    /*public static string sConnStr = @"Server=CLIMAX-LAPTOP;Database=sol_tbcrm;Trusted_Connection=True;";
    public static string sConnReminderStr = @"Server=CLIMAX-LAPTOP;Database=sol_tbcrm;Trusted_Connection=True;";*/
 //public static string sConnStr = @"Server=41.203.5.148;Database=sol_tbcrm;User ID=UWHX11424;Password=w)(3rM24;"; //(NEW LIVE SITE)
    public static string sConnReminderStr = @"Server=41.203.5.148;Database=sol_tbcrm;User ID=UWHX11424;Password=w)(3rM24;"; //(NEW LIVE SITE)
    //public static string sConnStr = @"Server=208.101.12.227;Database=sol_tbcrm;User ID=UWHX11424;Password=w)(3rM24;"; //(LIVE SITE)*/
    public static string sConnStr = @"Server=localhost;Database=sol_tbcrm;User ID=sa;Password=aw3s0me;"; //(Local Anthonie)
   /* public static string sConnStr = @"Server=208.101.12.227;Database=sol_tbcrm;User ID=UWHX11424;Password=w)(3rM24;"; //(LIVE SITE)
    public static string sConnReminderStr = @"Server=208.101.12.227;Database=sol_tbcrm;User ID=UWHX11424;Password=w)(3rM24;"; //(LIVE SITE)
    //public static string sConnStr = @"Server=208.101.12.227;Database=sol_tbcrm;User ID=UWHX11424;Password=w)(3rM24;"; //(LIVE SITE)*/

    public static DataSet getDataSet(string sSQL, string sTable, SqlConnection conn)
    {
        DataSet ds = new DataSet();
        // create a data adapter 
        SqlDataAdapter da = new SqlDataAdapter(sSQL, conn);
        da.Fill(ds, sTable);
        return (ds);
    }

    public static SqlCommand getDataCommand(string sSQL, SqlConnection conn, Page pPage)
    {
        try
        {
            if (conn == null)
            {
                try
                {
                    conn = new SqlConnection(sConnStr);
                    conn.Open();
                }
                catch (SqlException ex)
                {
                    pPage.Response.Write("<script> alert('Error connecting to the server: " + ex.Message + "');</script>");
                }
            }
            else if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            SqlCommand cmdSQL = new SqlCommand(sSQL, conn);
            return cmdSQL;
        }
        catch
        {
            return null;
        }
    }

    public static SqlCommand getDataCommand(string sSQL, SqlConnection conn)
    {
        try
        {
            if (conn == null)
            {
                try
                {
                    conn = new SqlConnection(sConnStr);
                    conn.Open();
                }
                catch (SqlException)
                {
                    //pPage.Response.Write("<script> alert('Error connecting to the server: " + ex.Message + "');</script>");
                }
            }
            else if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            SqlCommand cmdSQL = new SqlCommand(sSQL, conn);
            return cmdSQL;
        }
        catch
        {
            return null;
        }
    }
}
