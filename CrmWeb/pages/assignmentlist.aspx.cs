using System;
using System.Globalization;
using System.IO;
using System.Web.UI;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using System.Web.UI.WebControls;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using MigraDoc.RtfRendering;
using System.Data.SqlClient;

public partial class pages_assignmentlist : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["LoggedInUserID"] == null)
        {
            Response.Write("<script type=\"text/javascript\">window.top.location.href = '" + Page.ResolveClientUrl("~/Default.aspx") + "';</script>");
            Response.Flush();
            return;
        }

        // Create new PDF document
        Document document = CreateDocument();

        // Create a renderer for the MigraDoc document.
        PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer(true, PdfFontEmbedding.Always);

        // Associate the MigraDoc document with a renderer
        pdfRenderer.Document = document;

        // Layout and render document to PDF
        pdfRenderer.RenderDocument();

        // Send PDF to browser
        MemoryStream stream = new MemoryStream();

        pdfRenderer.PdfDocument.Save(stream, false);
        // document.Save(stream, false);
        Response.Clear();
        Response.ContentType = "application/pdf";
        Response.AddHeader("content-length", stream.Length.ToString());
        string filename = DateTime.Now.ToString("yyyy-MM-dd") + ".pdf";

        // Make dialog save as appear
        Response.AppendHeader("content-disposition", "attachment; filename=" + filename);

        Response.BinaryWrite(stream.ToArray());
        Response.Flush();
        stream.Close();
        Response.End();
    }

    public void AddClient(Document document, SqlDataReader reader, int clientnum)
    {
        Table table = document.LastSection.AddTable();
        table.Format.Alignment = ParagraphAlignment.Left;
        Column column = table.AddColumn(Unit.FromCentimeter(1));
        table.AddColumn(Unit.FromCentimeter(2));
        table.AddColumn(Unit.FromCentimeter(5));
        table.AddColumn(Unit.FromCentimeter(5));
        table.AddColumn(Unit.FromCentimeter(3));
        Row row = table.AddRow();
        row.Format.SpaceBefore = Unit.FromCentimeter(1);
        row.Format.SpaceAfter = Unit.FromCentimeter(1);
        Cell cell = row.Cells[0];
        cell.AddParagraph(clientnum.ToString() + ".");
        cell = row.Cells[1];
        cell.AddParagraph((string)reader["sClientNumber"]);
        cell = row.Cells[2];
        cell.AddParagraph((string)reader["sClientName"]);
        cell = row.Cells[3];
        cell.AddParagraph((string)reader["sContactPerson"]);
        cell = row.Cells[4];
        cell.AddParagraph((string)reader["sTelephone"]);
    }
    public void AddDate(Document document, SqlDataReader reader)
    {
        Paragraph paragraph = document.LastSection.AddParagraph();
        paragraph.Format.LeftIndent = Unit.FromCentimeter(1);
        paragraph.AddCharacter(SymbolName.Bullet);
        DateTime createddate = (DateTime)reader["dtCreated"];
        paragraph.AddTab();
        paragraph.AddText("(" + createddate.Day.ToString() + "/" + createddate.Month.ToString() + ")" + createddate.Year.ToString());
    }
    public void AddEvent(Document document, SqlDataReader reader, int eventnum)
    {
        Paragraph paragraph = document.LastSection.AddParagraph();
        DateTime createddate = (DateTime)reader["dtCreated"];
        paragraph.Format.FirstLineIndent = Unit.FromCentimeter(-1.2);
        paragraph.AddText(eventnum.ToString() + ". " + ((string)reader["sAssignedUserName"])[0].ToString() + ((string)reader["sAssignedUserSurname"])[0].ToString() + ": ");
        paragraph.Format.LeftIndent = Unit.FromCentimeter(2.5);
        paragraph.AddText((string)reader["sMessage"]);
    }
    public Document CreateDocument()
    {
        Document document = new Document();

        document.Info.Title = "Existing Client Task List";
        document.Info.Author = "Sol-TBCRM";
        document.Info.Subject = "";

        using (SqlConnection oConn = new SqlConnection(Connect.sConnStr))
        {
            oConn.Open();

            // Read the list of new clients
            string strQuery;
            if ((string)Session["LoggedInUserID"] == "7")
                strQuery = "SELECT joined.PKiAssignedUser, joined.sUserName, joined.sUserSurname, joined.PKiClientID, joined.sClientName, joined.sClientNumber, joined.sContactPerson, joined.sTelephone, events.FKsEventType as sEventType, events.sMessage, events.dtCreated, events.FKiUserAssignedID, users.sName as sAssignedUserName, users.sSurname as sAssignedUserSurname FROM events INNER JOIN ( SELECT users.PKiUserID as PKiAssignedUser, users.sName as sUserName, users.sSurname as sUserSurname, existing_clients.PKiClientID, existing_clients.sName as sClientName, existing_clients.sClientNumber, existing_clients.sContactPerson, existing_clients.sTelephone FROM users INNER JOIN existing_clients ON users.PKiUserID = existing_clients.FKiUserID WHERE existing_clients.FKiUserID IS NOT NULL AND existing_clients.bExcluded=0) as joined ON events.FKiExistingClientID=joined.PKiClientID, users WHERE events.sStatus = 'Active' AND users.PKiUserID = events.FKiUserAssignedID ORDER BY joined.PKiAssignedUser, joined.PKiClientID, events.dtCreated";
            else
                strQuery = "SELECT joined.PKiAssignedUser, joined.sUserName, joined.sUserSurname, joined.PKiClientID, joined.sClientName, joined.sClientNumber, joined.sContactPerson, joined.sTelephone, events.FKsEventType as sEventType, events.sMessage, events.dtCreated, events.FKiUserAssignedID, users.sName as sAssignedUserName, users.sSurname as sAssignedUserSurname FROM events INNER JOIN ( SELECT users.PKiUserID as PKiAssignedUser, users.sName as sUserName, users.sSurname as sUserSurname, existing_clients.PKiClientID, existing_clients.sName as sClientName, existing_clients.sClientNumber, existing_clients.sContactPerson, existing_clients.sTelephone FROM users INNER JOIN existing_clients ON users.PKiUserID = existing_clients.FKiUserID WHERE existing_clients.FKiUserID = " + (string)Session["LoggedInUserID"] + " AND existing_clients.bExcluded=0) as joined ON events.FKiExistingClientID=joined.PKiClientID, users WHERE events.sStatus = 'Active' AND users.PKiUserID = events.FKiUserAssignedID ORDER BY joined.PKiAssignedUser, joined.PKiClientID, events.dtCreated";

            SqlDataReader reader = Connect.getDataCommand(strQuery, oConn).ExecuteReader();

            int lastuserid = -1;
            int lastclientid = -1;
            int numclients = 0;
            int numevents = 0;
            DateTime lastdate = DateTime.MinValue;
            if (!reader.HasRows)
            {
                Section section = document.AddSection();
                section.PageSetup.PageFormat = PageFormat.A4;
                section.AddParagraph("No items for user '" + (string)Session["LoggedInUserID"] + "'");
            }
            while (reader.Read())
            {
                // Start new section
                if ((int)reader["PKiAssignedUser"] != lastuserid)
                {
                   /* if (lastuserid != -1)
                        document.LastSection.AddPageBreak();*/
                    numclients = 0;
                    lastuserid = (int)reader["PKiAssignedUser"];
                    // Create a paragraph with centered page number. See definition of style "Footer".
                    Paragraph paragraph = new Paragraph();
                    paragraph.AddTab();
                    paragraph.AddPageField();
                    // Add a section to the document
                    Section section = document.AddSection();
                    section.PageSetup.PageFormat = PageFormat.A4;
                    section.PageSetup.StartingNumber = 1;

                    // Add paragraph to footer for odd pages.
                    section.Footers.Primary.Add(paragraph);

                    HeaderFooter header = section.Headers.Primary;
                    header.AddParagraph(DateTime.Now.ToString() + "\t\t" + (string)reader["sUserName"] + " " + (string)reader["sUserSurname"] + " Task List");

                    paragraph = section.AddParagraph();
                    paragraph.Format.Font.Name = "Arial";
                    paragraph.Format.Font.Size = Unit.FromPoint(18);
                    paragraph.Format.SpaceBefore = Unit.FromCentimeter(2);
                    paragraph.Format.SpaceAfter = Unit.FromCentimeter(2);
                    paragraph.Format.Alignment = ParagraphAlignment.Center;
                    paragraph.AddText((string)reader["sUserName"] + " " + (string)reader["sUserSurname"] + " Task List\n");
                    Table table = section.AddTable();
                    table.Format.Alignment = ParagraphAlignment.Left;
                    Column column = table.AddColumn(Unit.FromCentimeter(1));
                    table.AddColumn(Unit.FromCentimeter(2));
                    table.AddColumn(Unit.FromCentimeter(5));
                    table.AddColumn(Unit.FromCentimeter(5));
                    table.AddColumn(Unit.FromCentimeter(3));
                    Row row = table.AddRow();
                    row.Format.Font.Italic = true;
                    row.Format.SpaceAfter = Unit.FromMillimeter(5);
                    Cell cell = row.Cells[0];
                    cell.AddParagraph("#");
                    cell = row.Cells[1];
                    cell.AddParagraph("Code");
                    cell = row.Cells[2];
                    cell.AddParagraph("Client");
                    cell = row.Cells[3];
                    cell.AddParagraph("Contact");
                    cell = row.Cells[4];
                    cell.AddParagraph("Telephone");

                    numclients++;
                    numevents = 1;
                    lastclientid = (int)reader["PKiClientID"];
                    AddClient(document, reader, numclients);
                    lastdate = (DateTime)reader["dtCreated"];
                    AddDate(document, reader);
                    AddEvent(document, reader, numevents);
                }
                else // Continue in section
                {
                    // New client
                    if ((int)reader["PKiClientID"] != lastclientid)
                    {
                        lastclientid = (int)reader["PKiClientID"];
                        lastdate = (DateTime)reader["dtCreated"];
                        numclients++;
                        numevents = 1;
                        AddClient(document, reader, numclients);
                        AddDate(document, reader);
                        AddEvent(document, reader, numevents);
                    }
                    else // Continue with same client
                    {
                        DateTime newdate = (DateTime)reader["dtCreated"];
                        if (newdate.Year != lastdate.Year && newdate.Month != lastdate.Month && newdate.Day != lastdate.Day) // New date
                        {
                            AddDate(document, reader);
                            numevents = 1;
                            AddEvent(document, reader, numevents);
                        }
                        else // Continue with same date
                        {
                            numevents++;
                            AddEvent(document, reader, numevents);
                        }
                    }
                }
            }

            reader.Close();
            oConn.Close();
        }
        return document;
    }
}