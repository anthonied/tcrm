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

public partial class pages_list_existing_clients : System.Web.UI.Page
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
            GenerateExistingClientList();
        }
    }

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

            sSQL += " AND bExcluded = 0";

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
    }
}