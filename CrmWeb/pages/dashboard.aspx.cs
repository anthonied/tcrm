using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

public partial class pages_new_clients_dash : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["LoggedInUserID"] == null)
        {
            Response.Write("<script type=\"text/javascript\">window.top.location.href = '" + Page.ResolveClientUrl("~/Default.aspx") + "';</script>");
            Response.Flush();
            return;
        }
        if ((string)Session["LoggedInUserID"] == "7")
        {
            dueEventStack.Visible = true;
            divNewClients.Visible = false;
            divOldClients.Visible = false;
            eventStack.Visible = false;
        }
        else
        {
            dueEventStack.Visible = false;
            dsClientFinder.ConnectionString = Connect.sConnStr;
            dsExistingClientFinder.ConnectionString = Connect.sConnStr;
            gvLastFiveClients.DataBind();
            gvExistingClients.DataBind();
        }
        popupEventDetails.ShowOnPageLoad = false;
    }
    protected void gvLastFiveClients_SelectedIndexChanged(object sender, EventArgs e)
    {   
        Response.Redirect("new_clients.aspx" + "?SelectedClient=" + Server.UrlEncode(gvLastFiveClients.SelectedDataKey.Value.ToString()), true);
    }
    protected void gvExistingClients_SelectedIndexChanged(object sender, EventArgs e)
    {
        Response.Redirect("existing_client_info.aspx" + "?sClientID=" + Server.UrlEncode(gvExistingClients.SelectedDataKey.Value.ToString()), true);
    }
    protected void gvLastFiveClients_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "this.style.cursor='pointer';this.style.textDecoration='underline';";
            e.Row.Attributes["onmouseout"] = "this.style.textDecoration='none';";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(gvLastFiveClients, "Select$" + e.Row.RowIndex);
        }
    }
    protected void gvExistingClients_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "this.style.cursor='pointer';this.style.textDecoration='underline';";
            e.Row.Attributes["onmouseout"] = "this.style.textDecoration='none';";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(gvExistingClients, "Select$" + e.Row.RowIndex);
        }
    }
    protected void btnPreFilter_Click(object sender, EventArgs e)
    {
        string filterby = Server.UrlEncode(hdnFilterBy.Value.Trim());
        string filterwith = Server.UrlEncode(hdnFilterWith.Value.Trim());
        string url = ResolveUrl("~/pages/eventstack.aspx") + "?filterby=" + filterby + "&filterwith=" + filterwith;
        Response.Redirect(url, false);
    }
    protected void btnShowEvent_Click(object sender, EventArgs e)
    {
        int eventID =  int.Parse(hdnEventID.Value);
        popupEventDetails.ShowOnPageLoad = true;
        divEventDetails.InnerText = eventID.ToString();
        EventStack.Event? selectedEvent = EventStack.GetEvent(eventID);

        if (selectedEvent.HasValue)
        {
            string htmlCode = dueEventStack.MakeEventBox(selectedEvent.Value);
            divEventDetails.InnerHtml = htmlCode;
        }
    }
}