using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

public partial class pages_eventstack : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["LoggedInUserID"] == null)
        {
            Response.Write("<script type=\"text/javascript\">window.top.location.href = '" + Page.ResolveClientUrl("~/Default.aspx") + "';</script>");
            Response.Flush();
            return;
        }
        if (!Page.IsPostBack)
        {
           
            txtFilterCreateStartDate.Text = DateTime.Now.AddDays(-7).ToString("dd/MM/yyyy");
            txtFilterCreateEndDate.Text = DateTime.Now.ToString("dd/MM/yyyy");

            if (Request.Params["filterby"] != null && Request.Params["filterwith"] != null)
            {
                string filterby = Request.Params["filterby"];
                string filterwith = Request.Params["filterwith"];
                
                if (filterby == "ClientName")
                {
                    cbFilterClientName.Checked = true;
                    txtFilterClientName.Text = filterwith.Trim();
                }                
                else if (filterby == "EventStatus")
                {
                    cbFilterStatus.Checked = true;
                    ddlFilterStatus.SelectedValue = filterwith.Trim();
                }

                FilterEvents();
            }
        }
        txtFilterUserName.Attributes["style"] = ((cbFilterUserName.Checked) ? null : "display: none");
        txtFilterClientName.Attributes["style"] = ((cbFilterClientName.Checked) ? null : "display: none");        
        txtFilterMessage.Attributes["style"] = ((cbFilterMessage.Checked) ? null : "display: none");
       // txtFilterRecurrence.Attributes["style"] = ((cbFilterRecurrence.Checked) ? null : "display: none");
        ddlFilterStatus.Attributes["style"] = ((cbFilterStatus.Checked) ? null : "display: none");
        spanFilterCreateDate.Attributes["style"] = ((cbFilterCreateDate.Checked) ? null : "display: none");     
    }

    private void FilterEvents()
    {        
        EventStack.Filter filter = new EventStack.Filter();
        filter.ShowExistingClients = ddlClientsToShow.SelectedValue == "Existing" || ddlClientsToShow.SelectedValue == "Both";
        filter.ShowNewClients = ddlClientsToShow.SelectedValue == "New" || ddlClientsToShow.SelectedValue == "Both";
        if (cbFilterClientName.Checked)
            filter.FilterClients = txtFilterClientName.Text.Trim();        
        if (cbFilterMessage.Checked)
            filter.FilterMessage = txtFilterMessage.Text.Trim();
        /*if (cbFilterRecurrence.Checked)
            filter.FilterRecurrence = txtFilterRecurrence.Text.Trim();*/
        if (cbFilterStatus.Checked)
            filter.FilterStatus = ddlFilterStatus.SelectedValue.Trim();
        if (cbFilterUserName.Checked)
            filter.FilterUsers = txtFilterUserName.Text.Trim();
        if (cbFilterCreateDate.Checked)
        {
            DateTime outStartDate, outEndDate;
            if (DateTime.TryParse(txtFilterCreateStartDate.Text.Trim(), out outStartDate) && DateTime.TryParse(txtFilterCreateEndDate.Text.Trim(), out outEndDate))
            {
                filter.FilterCreateStart = outStartDate.ToString("yyyy-MM-ddT00:00:00.000");
                filter.FilterCreateEnd = outEndDate.ToString("yyyy-MM-ddT23:59:59.000");
            }
        }       
        filter.FilterOrderDir = "ASC";        
        filter.FilterOrderBy = ddlSortBy.SelectedValue;
        eventStack.NumFilterEvents = Int32.Parse(txtNumResults.Text.Trim());
        eventStack.EventFilter = filter;
        eventStack.FilterEventStack();
    }

    protected void btnFilter_Click(object sender, EventArgs e)
    {
        FilterEvents();
    }
}