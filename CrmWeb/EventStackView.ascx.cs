using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Globalization;
using System.IO;

public partial class EventStackView : System.Web.UI.UserControl
{
    private bool _showaddnotes;
    public bool ShowAddNotes
    {
        get { return _showaddnotes; }
        set
        {
            _showaddnotes = value;

            if (_showaddnotes == true)
            {
                divAddNewEvent.Visible = true;
                headerAddEvent.Visible = true;
            }
            else
            {
                divAddNewEvent.Visible = false;
                headerAddEvent.Visible = false;
            }
        }
    }

    private bool _showdueevents;
    public bool ShowDueEvents
    {
        get { return _showdueevents; }
        set
        {
            _showdueevents = value;

            if (_showdueevents == true)
            {
                divDueEvents.Visible = true;
                headerDueEvent.Visible = true;
            }
            else
            {
                divDueEvents.Visible = false;
                headerDueEvent.Visible = false;
            }
        }
    }

    private bool _showeventstack;
    public bool ShowEventStack
    {
        get { return _showeventstack; }
        set
        {
            _showeventstack = value;
            if (_showeventstack == true)
            {
                divEventStack.Visible = true;
                headerEvents.Visible = true;
            }
            else
            {
                divEventStack.Visible = false;
                headerEvents.Visible = false;
            }
        }
    }


    private EventStack.Filter _eventfilter = null;
    public EventStack.Filter EventFilter
    {
        get
        {
            if (ViewState["EventFilter"] != null)
                _eventfilter = (EventStack.Filter)ViewState["EventFilter"];
            return _eventfilter;
        }
        set
        {
            _eventfilter = value;
            ViewState.Add("EventFilter", _eventfilter);
        }
    }

    private int _numfilterevents = 10; // 0 is infinite
    public int NumFilterEvents
    {
        get
        {
            if (ViewState["NumFilterEvents"] != null)
                _numfilterevents = (int)ViewState["NumFilterEvents"];
            return _numfilterevents;
        }
        set
        {
            _numfilterevents = value;
            ViewState.Add("NumFilterEvents", _numfilterevents);
        }
    }
    public void FilterEventStack()
    {
        FillEventStack();
        FillDueEvents();
        //UpdatePanel1.Update();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["LoggedInUserID"] == null)
        {
            Response.Write("<script type=\"text/javascript\">window.top.location.href = '" + Page.ResolveClientUrl("~/Default.aspx") + "';</script>");
            Response.Flush();
            return;
        }
        /* _eventfilter = (EventStack.Filter)ViewState["EventFilter"];
         if (ViewState["NumFilterEvents"] != null)
             _numfilterevents = (int)ViewState["NumFilterEvents"];*/
        if (!Page.IsPostBack)
        {
            txtEventDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtEventTime.Text = "12:00";
            txtReminderTime.Text = "12:00";
            if (ShowAddNotes)
                FillAddEvent();
            if (ShowDueEvents)
                FillDueEvents();
            if (ShowEventStack)
                FillEventStack();
        }
        else
        {
            if (ShowDueEvents)
                FillDueEvents();
            if (ShowAddNotes)
            {
                FillUsers();
                FillClients();
            }
        }
    }
    private void FillUsers()
    {
        EventStack.User[] users = EventStack.GetUsers();
        int numusers = 0;
        tblMarketers.Rows.Clear();
        HtmlTableRow row = null;
        if (hdnSelectedUser.Value == "")
            hdnSelectedUser.Value = users[0].ID;
        foreach (EventStack.User user in users)
        {
            if (numusers % 2 == 0)
            {
                row = new HtmlTableRow();
                tblMarketers.Rows.Add(row);
            }
            numusers++;
            HtmlTableCell cell = new HtmlTableCell();
            if (hdnSelectedUser.Value == user.ID)
                cell.BgColor = "crimson";
            else
                cell.BgColor = "salmon";
            cell.BorderColor = "grey";
            cell.Width = "150px";
            row.Cells.Add(cell);
            cell.Height = "25px";
            cell.InnerHtml = "&nbsp" + user.Name;
            cell.ID = "cellUser" + user.ID;
            cell.ClientIDMode = System.Web.UI.ClientIDMode.Static;

            cell.Attributes["onmouseover"] = "this.style.cursor='pointer';";//this.style.textDecoration='underline';";
            cell.Attributes["onmouseout"] = "this.style.textDecoration='none';";
            // cell.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(hdnUserSelected, "Select$"+user.ID);
            cell.Attributes["onclick"] = "return SelectUser('" + user.ID + "');";
            // ListItem item = new ListItem(user.Name, user.ID);
            //comboEventAssignedUser.Items.Add(item);
        }
    }
    private void FillClients()
    {
        int selectedindex = comboEventClient.SelectedIndex;
        comboEventClient.Items.Clear();
        EventStack.Client[] clients = EventStack.GetEventClientsFor(hdnSelectedUser.Value);

        foreach (EventStack.Client client in clients)
        {
            ListItem item = new ListItem(client.Name, client.ID + ":" + client.Type.ToString());
            comboEventClient.Items.Add(item);
        }

        if(comboEventClient.Items.Count > selectedindex)
            comboEventClient.SelectedIndex = selectedindex;
    }
    private void FillAddEvent()
    {
        string[] strings = EventStack.GetEventTypes();
        foreach (string str in strings)
            comboEventType.Items.Add(new ListItem(str));
        ListItem noteitem = comboEventType.Items.FindByText("Comment");
        if (noteitem != null)
            comboEventType.SelectedIndex = comboEventType.Items.IndexOf(noteitem);

        FillUsers();
        FillClients();
    }

    private void FillDueEvents()
    {
        EventStack.Event[] events = EventStack.GetDueEvents();
        tblDueEvents.Rows.Clear();
        if (events.Length > 0)
        {
            int iRowCount = 0;
            string sBackColor;
            foreach (EventStack.Event eventElement in events)
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
                HtmlTableRow newRow = new HtmlTableRow();
                newRow.BgColor = sBackColor;
                //newRow.Attributes.Add("onclick", "alert('Hallooo')");
                newRow.Attributes.Add("title", (eventElement.sMessage == "") ? "No Message" : eventElement.sMessage);
                //newRow.ID = "trDueEvent" + iRowCount.ToString(); i++;
                newRow.Align = "left";
                newRow.Style.Add("cursor", "pointer");
                newRow.Style.Add("height", "40px");
                HtmlTableCell colorCell = new HtmlTableCell();
                colorCell.Width = "50px";
                colorCell.BgColor = "#" + EventStack.GetUserColor(eventElement.iAssignedUserID);
                colorCell.Attributes.Add("onclick", "return ShowEventPopup('" + eventElement.iEventID.ToString() + "');");
                newRow.Cells.Add(colorCell);

                HtmlTableCell clientCell = new HtmlTableCell();
                HtmlAnchor clientText = new HtmlAnchor();
                clientText.Style["padding-left"] = "5px";
                clientText.HRef = "#";
                clientText.InnerText = EventStack.GetClientName(eventElement.iExistingClientID, eventElement.iNewClientID);
                //    clientText.Attributes.Add("class", "DueSpan");
                clientCell.Controls.Add(clientText);
                clientCell.Attributes.Add("onclick", "return FilterAccordingTo('ClientName', '" + EventStack.GetClientName(eventElement.iExistingClientID, eventElement.iNewClientID) + "');");
                newRow.Cells.Add(clientCell);

                HtmlTableCell typeCell = new HtmlTableCell();
                HtmlAnchor typeText = new HtmlAnchor();
                typeText.Style["padding-left"] = "5px";
                typeText.HRef = "#";
                typeText.InnerText = eventElement.sEventType;
                //     typeText.Attributes.Add("class", "DueSpan");
                typeCell.Controls.Add(typeText);
                typeCell.Attributes.Add("onclick", "return FilterAccordingTo('EventType','" + eventElement.sEventType + "');");
                newRow.Cells.Add(typeCell);

                HtmlTableCell dueCell = new HtmlTableCell();
                HtmlAnchor dueText = new HtmlAnchor();
                dueText.Style["padding-left"] = "5px";
                dueText.HRef = "#";
                dueText.InnerText = eventElement.dtDue.Value.ToString("dd/MM/yyyy HH:mm");
                //      dueText.Attributes.Add("class", "DueSpan");
                dueCell.Controls.Add(dueText);
                dueCell.Align = "left";
                dueCell.Attributes.Add("onclick", "return FilterAccordingTo('DueDate', '" + eventElement.dtDue.Value.ToString("dd/MM/yyyy") + "');");
                newRow.Cells.Add(dueCell);

                HtmlTableCell statusCell = new HtmlTableCell();
                HtmlAnchor statusText = new HtmlAnchor();
                statusText.Style["padding-left"] = "5px";
                statusText.HRef = "#";
                statusText.InnerText = eventElement.sStatus;
                //      dueText.Attributes.Add("class", "DueSpan");
                statusCell.Controls.Add(statusText);
                statusCell.Align = "left";
                statusCell.Attributes.Add("onclick", "return FilterAccordingTo('EventStatus', '" + eventElement.sStatus + "');");
                newRow.Cells.Add(statusCell);

                tblDueEvents.Rows.Add(newRow);
                iRowCount++;
            }
        }
        else
        {
            HtmlTableRow newRow = new HtmlTableRow();
            newRow.Align = "center";
            HtmlTableCell newCell = new HtmlTableCell();
            newCell.InnerText = "No due events";
            newCell.Attributes.Add("class", "DueSpan");
            newRow.Cells.Add(newCell);
            tblDueEvents.Rows.Add(newRow);
        }
    }
    public string MakeEventBox(EventStack.Event eventElement)
    {
        string str = @"<div id=""divUserEvent{0}"" class=""UserEvent"">
                    <div class=""EventPOI"">
                        <a href=""{1}"" style=""{14}"">{2}</a></div>
                    <img src=""{3}"" alt=""{2}"" width=""64px""
                        style=""clear: both; float: right; margin-right: 20px; border: 1px solid #ff4747;"" />
                    <dl class=""EventInfo"">
                        <dt class=""EventCreatedDate"">Posted: </dt>

                        <dd class=""EventCreatedDate"">
                            <span class=""eventstack-datetime"">Posted: {4}</span>
                        </dd>
                        <dt class=""EventID"">ID: </dt>
                        <dd class=""EventID"">
                            #{0}
                        </dd>
                        <dt class=""EventAuthor"">Assigned To: </dt>
                        <dd class=""EventAuthor"">
                            for <a href=""{5}"" style=""{12}"">{6}</a>
                        </dd>
                        <dt class=""EventType"">Type: </dt>
                        <dd class=""EventType"">
                            <em>{7}</em>
                        </dd>
                        <dt class=""EventDueDate"">Due: </dt>
                        <dd class=""EventDueDate"">
                            <span class=""eventstack-datetime"">Due: {8}</span>
                        </dd>
                        <dt class=""EventDueDate"">Reminder: </dt>
                        <dd class=""EventDueDate"">
                            <span class=""eventstack-datetime"">Reminder: {15}</span>
                        </dd>
                        <dt class=""EventStatus"">Status: </dt>
                        <dd class=""EventStatus"" style=""{9}"">
                            <em>{10}</em>
                            &nbsp; <select onchange=""{13}"">
                                <option selected=""selected"" value="""">Change Status</option>
                                 <option value=""Done"">Done</option>
                                <option value=""Active"">Active</option>
                                 <option value=""Inactive"">Inactive</option>
                                </select>
                        </dd>";
        if( Session["TypeUser"].ToString() == "Admin")
        {
            str += @"<dd><input type=button value=' delete ' alt='This wil remove the entire event.  Please be carefull.'></dd>";
        }

        str += @"</dl>
                    <div class=""EventContent"">
                        <span></span>
                        <p>
                            {11}
                        </p>
                    </div>
                </div>";

        string ClientPostBackUrl = "";
        string ClientName = EventStack.GetClientName(eventElement.iExistingClientID, eventElement.iNewClientID);
        string path = EventStack.GetClientImageFile(eventElement.iExistingClientID);
        if (File.Exists(Server.MapPath(path)))
        {
            path = ResolveUrl(path);
        }
        string ClientPhotoURL = path;
        string PostDate = eventElement.dtCreated.ToString("yyyy/MM/dd HH:mm") + " " + EventStack.GetUserFullName(eventElement.iUserID);
        string UserPostBackUrl = "";
        string UserName = EventStack.GetUserFullName(eventElement.iAssignedUserID);
        string EventType = eventElement.sEventType;
        string DueDate = eventElement.dtDue.Value.ToString("yyyy/MM/dd HH:mm");
        string StatusStyle = "border-color: #FF6666; color: #663333 !important;";
        string EventStatus = "Unknown status.";
        if (eventElement.sStatus == "Active")
        {
            StatusStyle = "border-color: #DAC366; color: #666633 !important;";
            EventStatus = "This event is still active.";
        }
        else if (eventElement.sStatus == "Done")
        {
            StatusStyle = "border-color: #7AC366; color: #336633 !important;";
            EventStatus = "This event is complete.";
        }
        else if (eventElement.sStatus == "Overdue")
        {
            StatusStyle = "border-color: #FF6666; color: #663333 !important;";
            EventStatus = "This event is overdue.";
        }
        else if (eventElement.sStatus == "Inactive")
        {
            StatusStyle = "border-color: #FF6666; color: #333333 !important;";
            EventStatus = "This event is inactive.";
        }
        string Message = (eventElement.sMessage == "") ? "No Message" : eventElement.sMessage;

        string AuthorStyle = "text-decoration:none; border-right: 20px solid #" + EventStack.GetUserColor(eventElement.iAssignedUserID) + "; padding-right: 5px; cursor: default;";
        string EventStatusChangeScript = "return OnEventStatusChange(this.value, '" + eventElement.iEventID.ToString() + "');";
        string POIStyle = "text-decoration:none; cursor: default;";
        string Reminder;
        if (eventElement.dtReminder != null)
        {
            Reminder = " " + eventElement.dtReminder.Value.ToString("yyyy/MM/dd HH:mm");
        }
        else
        {
            Reminder = " None";
        }
        string result = string.Format(str, eventElement.iEventID, ClientPostBackUrl, ClientName, ClientPhotoURL, PostDate, UserPostBackUrl, UserName, EventType, DueDate, StatusStyle, EventStatus, Message, AuthorStyle, EventStatusChangeScript, POIStyle, Reminder);

        return result;
    }
    private void FillEventStack()
    {
        EventStack.Event[] events = EventStack.GetEvents(NumFilterEvents, EventFilter);
        divEventStack.InnerHtml = "";
        if (events.Length > 0)
        {
            foreach (EventStack.Event eventElement in events)
            {
                divEventStack.InnerHtml += MakeEventBox(eventElement);
            }
        }
        else
        {
            HtmlGenericControl message = new HtmlGenericControl("div");
            message.Attributes.Add("class", "UserEvent");
            message.InnerHtml = "<center>No filter results</center>";
            divEventStack.Controls.Add(message);
        }
    }

    protected void btnSubmitNote_Click(object sender, EventArgs e)
    {
        if (ShowAddNotes)
        {
            if (Page.IsValid)
            {
                ListItem selectedClient = comboEventClient.Items[comboEventClient.SelectedIndex];
                ListItem selectedType = comboEventType.Items[comboEventType.SelectedIndex];
                string ClientName = selectedClient.Text.Trim();
                string[] values = selectedClient.Value.Split(new string[] { ":" }, 2, StringSplitOptions.None);
                string ClientID = values[0].Trim();

                EventStack.ClientType type = (EventStack.ClientType)Enum.Parse(typeof(EventStack.ClientType), values[1].Trim());
                string eventType = selectedType.Text.Trim();
                string sMessage = txtMessage.InnerText.Trim();
                // Correctly parse and format the date string for compatibility with SQL
                CultureInfo provider = CultureInfo.InvariantCulture;

                EventStack.Event newEvent = new EventStack.Event();
                newEvent.dtDue = DateTime.ParseExact(txtEventDate.Text.Trim() + " " + txtEventTime.Text.Trim(), "dd/MM/yyyy HH:mm", provider);
                if (cbReminderSMS.Checked == true)
                {
                    newEvent.dtReminder = DateTime.ParseExact(txtEventDate.Text.Trim() + " " + txtReminderTime.Text.Trim(), "dd/MM/yyyy HH:mm", provider);
                }
                else
                {
                    newEvent.dtReminder = null;
                }
                if (type == EventStack.ClientType.NewClient)
                {
                    newEvent.iExistingClientID = null;
                    newEvent.iNewClientID = Int32.Parse(ClientID);
                }
                else
                {
                    newEvent.iExistingClientID = Int32.Parse(ClientID);
                    newEvent.iNewClientID = null;
                }
                if (Session["LoggedInUserID"] != null)
                    newEvent.iUserID = int.Parse((string)Session["LoggedInUserID"]);
                else
                    newEvent.iUserID = 1;

                newEvent.iAssignedUserID = int.Parse(hdnSelectedUser.Value);
                newEvent.sEventType = eventType;
                newEvent.sMessage = sMessage;
                newEvent.sRecurrence = "None";
                if (eventType == "Birthday")
                    newEvent.sRecurrence = "Yearly";
                newEvent.sStatus = "Due";
                EventStack.CreateNewEvent(newEvent);

                msgAddEvent.ShowSuccess("Your event was successfully added.");
                if (ShowEventStack)
                {
                    this.FilterEventStack();
                    foreach (Control ctrl in this.Parent.Controls)
                    {
                        if (ctrl.GetType() == this.GetType() && (ctrl.ID != this.ID))
                        {
                            ((EventStackView)ctrl).FilterEventStack();
                        }
                    }
                }
            }
            else
            {
                msgAddEvent.Visible = false;
            }

            FillUsers();
        }
        if (ShowDueEvents)
            FillDueEvents();
    }

    protected void hdnDoAsyncPostback_Click(object sender, EventArgs e)
    {
        int selectedevent = int.Parse(hdnEventStatusValue.Value);
        string newstatus = hdnEventStatusID.Value;

        EventStack.ChangeEventStatus(newstatus, selectedevent);
        FillEventStack();
        FillDueEvents();
        FillUsers();
        FillClients();
    }

    protected void hdnUserSelected_Click(object sender, EventArgs e)
    {
        if (ShowAddNotes)
        {
            FillUsers();
            FillClients();
        }
    }
}