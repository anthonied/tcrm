using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

/// <summary>
/// EventStack handles the database side of the messages system. 
/// It retrieves and stores new messages. It can also do basic queries.
/// </summary> 
public class EventStack
{
    private const string EventStackTable = "events";
    private const string EventTypeTable = "event_types";
    private const string ExistingClientTable = "existing_clients";
    private const string NewClientTable = "new_clients";
    private const string UsersTable = "users";

    
    public struct Event
    {
        public int iEventID;
        public string sEventType;
        public DateTime dtCreated;
        public DateTime? dtDue;
        public DateTime? dtReminder;
        public string sRecurrence;
        public int iUserID;
        public int iAssignedUserID;
        public int? iExistingClientID;
        public int? iNewClientID;
        public string sMessage;
        public string sStatus;
    }

    [Serializable()]
    public class Filter
    {
        public bool ShowNewClients;
        public bool ShowExistingClients;
        public string FilterClients;
        public string FilterUsers;
        public string FilterString;
        public string FilterStartDate;
        public string FilterEndDate;
        public string FilterStatus;
        public string FilterMessage;
        public string FilterRecurrence;
        public string FilterType;
        public string FilterCreateStart;
        public string FilterCreateEnd;
        public string FilterDueStart;
        public string FilterDueEnd;
        public string FilterOrderBy;
        public string FilterOrderDir;
        public Filter()
        {
            ShowNewClients = true;
            ShowExistingClients = true;
            FilterClients = "";
            FilterUsers = "";
            FilterString = "";
            FilterStatus = "Active";
            FilterMessage = "";
            FilterRecurrence = "";
            FilterType = "";
            FilterCreateStart = "1900-01-01T00:00:00.000";
            FilterCreateEnd = "5000-12-31T23:59:59.000";
            FilterDueStart = "1900-01-01T00:00:00.000"; //DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            FilterDueEnd = "5000-12-31T23:59:59.000";
            FilterOrderBy = "dtDue";
            FilterOrderDir = "ASC"; // DESC
        }
    }

    public static void ChangeEventStatus(string status, int eventID)
    {
        using (SqlConnection oConn = new SqlConnection(Connect.sConnStr))
        {
            oConn.Open();
            string sSql = " UPDATE " + EventStackTable + " SET";
            sSql += " sStatus='" + status.Trim().Replace("'", "''") + "' ";
            sSql += " WHERE PKiEventID='" + eventID.ToString() + "';";

            int iRet = Connect.getDataCommand(sSql, oConn).ExecuteNonQuery();

            oConn.Close();
        }
    }
    public static void CreateNewEvent(Event newevent)
    {
        // Handle nullable clients
        string iExistingClientID = (newevent.iExistingClientID == null) ? "NULL" : "'" + newevent.iExistingClientID.ToString() + "'";
        string iNewClientID = (newevent.iNewClientID == null) ? "NULL" : "'" + newevent.iNewClientID.ToString() + "'";
        string dtDue = (newevent.dtDue == null) ? "NULL" : "'" + newevent.dtDue.Value.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";
        string dtReminder = (newevent.dtReminder == null) ? "NULL" : "'" + newevent.dtReminder.Value.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";
        
        using (SqlConnection oConn = new SqlConnection(Connect.sConnStr))
        {
            oConn.Open();
            string sSql = " INSERT INTO " + EventStackTable + " VALUES";
            sSql += "( '" + newevent.sEventType.Trim().Replace("'", "''") + "',";
            sSql += " '" + newevent.iUserID.ToString().Replace("'", "''") + "',";
            sSql += " '" + newevent.iAssignedUserID.ToString().Replace("'", "''") + "',";
            sSql += " " + iExistingClientID+ ",";
            sSql += " " + iNewClientID + ",";
            sSql += " " + dtDue + ",";
            sSql += " '" + newevent.sRecurrence.Trim().Replace("'", "''") + "',";
            sSql += " '" + newevent.sMessage.Trim().Replace("'", "''") + "',";
            sSql += " 'Active',";
            sSql += " " + dtReminder + ",";
            sSql += " DEFAULT)";

            int iRet = Connect.getDataCommand(sSql, oConn).ExecuteNonQuery();

            oConn.Close();
        }
        int lasteventid = 0;
        using (SqlConnection oConn = new SqlConnection(Connect.sConnStr))
        {
            oConn.Open();
            string sSql = " SELECT PKiEventID FROM events WHERE PKiEventID = IDENT_CURRENT('events')";

            SqlDataReader reader = Connect.getDataCommand(sSql, oConn).ExecuteReader();
            
            if(reader.HasRows)
            {
                reader.Read();
                lasteventid = reader.GetInt32(0);
            }
            oConn.Close();
        }

        if (newevent.dtReminder != null)
        {
            string phonenumber = GetUserPhoneNumber(newevent.iAssignedUserID);
            if (phonenumber.Length > 0)
            {
                using (SqlConnection oConn = new SqlConnection(Connect.sConnReminderStr))
                {
                    oConn.Open();
                    string sSql = " INSERT INTO reminder_stack VALUES";
                    sSql += "( '" + lasteventid.ToString() + "',";
                    sSql += " " + dtReminder + ",";
                    sSql += " '" + phonenumber + "',";
                    sSql += " '" + newevent.sMessage.Trim().Replace("'", "''") + "')";

                    int iRet = Connect.getDataCommand(sSql, oConn).ExecuteNonQuery();

                    oConn.Close();
                }
            }
        }
    }
    public static Event? GetEvent(int EventID)
    {
        Event? outEvent = null;
        using (SqlConnection oConn = new SqlConnection(Connect.sConnStr))
        {
            oConn.Open();

            string sSQL;
            sSQL = "SELECT * FROM " + EventStackTable + " WHERE PKiEventID='" + EventID.ToString() + "'"; 
            SqlDataReader reader = Connect.getDataCommand(sSQL, oConn).ExecuteReader();

            if (reader.HasRows)
            {
                reader.Read();
                outEvent = GetEventFromReader(reader);
                
            }
            reader.Close();
            oConn.Close();
        }

        return outEvent;
    }
    public static Event[] GetEvents()
    {
        return GetEvents(5, null);
    }
    public static Event[] GetEvents(Filter filter)
    {
        return GetEvents(5, filter);
    }
    public static Event[] GetEvents(int numevents, Filter filter)
    {
        List<Event> events = new List<Event>(50);
        using (SqlConnection oConn = new SqlConnection(Connect.sConnStr))
        {
            oConn.Open();

            // Read the list of site clients
            string sSQL;
            if (numevents == 0)
                sSQL = "SELECT * FROM " + EventStackTable;
            else
                sSQL = "SELECT TOP " + numevents.ToString().Replace("'", "''") + " * FROM " + EventStackTable;
            if (filter != null)
            {
                sSQL += " WHERE (";
                if (!filter.ShowNewClients)
                    sSQL += "(FKiNewClientID IS NULL) AND ";
                else
                    sSQL += "((FKiNewClientID IS NULL) OR ((SELECT TOP 1 sNewClientName FROM new_clients WHERE PKiNewClientID = FKiNewClientID) LIKE '%" + filter.FilterClients + "%')) AND ";
                if (!filter.ShowExistingClients)
                    sSQL += "(FKiExistingClientID IS NULL) AND ";
                else
                    sSQL += "((FKiExistingClientID IS NULL) OR ((SELECT TOP 1 sName FROM existing_clients WHERE PKiClientID = FKiExistingClientID) LIKE '%" + filter.FilterClients + "%')) AND ";
                sSQL += "((SELECT TOP 1 sName FROM users WHERE PKiUserID = FKiUserAssignedID) LIKE '%" + filter.FilterUsers.Trim().Replace("'", "''") + "%' ESCAPE '\\') AND ";
                sSQL += "(sMessage LIKE '%" + filter.FilterMessage.Trim().Replace("'", "''") + "%' ESCAPE '\\') AND ";
                sSQL += "(FKsEventType LIKE '%" + filter.FilterType.Trim().Replace("'", "''") + "%' ESCAPE '\\') AND ";
                sSQL += "(sRecurrence LIKE '%" + filter.FilterRecurrence.Trim().Replace("'", "''") + "%' ESCAPE '\\') AND ";
                if (filter.FilterStatus != "")
                    sSQL += "(sStatus = '" + filter.FilterStatus.Trim().Replace("'", "''") + "') AND ";
                sSQL += " (dtCreated >= '" + filter.FilterCreateStart.Trim().Replace("'", "''") + "' AND dtCreated <= '" + filter.FilterCreateEnd.Trim().Replace("'", "''") + "') AND ";
                sSQL += " (dtDue >= '" + filter.FilterDueStart.Trim().Replace("'", "''") + "' AND dtDue <= '" + filter.FilterDueEnd.Trim().Replace("'", "''") + "'))";
                sSQL += " ORDER BY " + filter.FilterOrderBy.Trim().Replace("'", "''") + " " + filter.FilterOrderDir.Trim().Replace("'", "''");
            }
            else
            {
               // sSQL += "(DATEDIFF(day, GETDATE(), dtDue) >= 0 ))";
                sSQL += " ORDER BY dtDue";
            }            

            SqlDataReader reader = Connect.getDataCommand(sSQL, oConn).ExecuteReader();

            // Bind to dropdown
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    events.Add(GetEventFromReader(reader));
                }
            }
            reader.Close();
            oConn.Close();
        }
        events.TrimExcess();
        return events.ToArray();      
    }
    public static Event[] GetDueEvents()
    {
        return GetDueEvents(10, new TimeSpan(), null);
    }
    public static Event[] GetDueEvents(Filter filter)
    {
        return GetDueEvents(10, new TimeSpan(), filter);
    }
    public static Event[] GetDueEvents(int numevents, TimeSpan duespan, Filter filter)
    {
        List<Event> events = new List<Event>(50);
        using (SqlConnection oConn = new SqlConnection(Connect.sConnStr))
        {
            oConn.Open();

            // Read the list of site clients
            string sSQL;
            if (numevents == 0)
                sSQL = "SELECT * FROM " + EventStackTable + " WHERE (";
            else
                sSQL = "SELECT TOP " + numevents.ToString().Trim().Replace("'", "''") + " * FROM " + EventStackTable + " WHERE (";
            if (filter != null)
            {
                if (!filter.ShowNewClients)
                    sSQL += "(FKiNewClientID IS NULL) AND ";
                else
                    sSQL += "((FKiNewClientID IS NULL) OR ((SELECT TOP 1 sNewClientName FROM new_clients WHERE PKiNewClientID = FKiNewClientID) LIKE '%" + filter.FilterClients.Trim().Replace("'", "''") + "%' ESCAPE '\\')) AND ";
                if (!filter.ShowExistingClients)
                    sSQL += "(FKiExistingClientID IS NULL) AND ";
                else
                    sSQL += "((FKiExistingClientID IS NULL) OR ((SELECT TOP 1 sName FROM existing_clients WHERE PKiClientID = FKiExistingClientID) LIKE '%" + filter.FilterClients.Trim().Replace("'", "''") + "%' ESCAPE '\\')) AND ";
                sSQL += "((SELECT TOP 1 sName FROM users WHERE PKiUserID = FKiUserAssignedID) LIKE '%" + filter.FilterUsers.Trim().Replace("'", "''") + "%' ESCAPE '\\') AND ";
                sSQL += "(sMessage LIKE '%" + filter.FilterMessage.Trim().Replace("'", "''") + "%' ESCAPE '\\') AND ";
                sSQL += "(sRecurrence LIKE '%" + filter.FilterRecurrence.Trim().Replace("'", "''") + "%' ESCAPE '\\') AND ";
                sSQL += "(FKsEventType LIKE '%" + filter.FilterType.Trim().Replace("'", "''") + "%' ESCAPE '\\')";
                sSQL += " AND ";
            }
            sSQL += "(DATEDIFF(day, GETDATE(), dtDue) >= " + duespan.Days.ToString() + ")) AND (sStatus = 'Active')";

            sSQL += " ORDER BY dtDue";

            SqlDataReader reader = Connect.getDataCommand(sSQL, oConn).ExecuteReader();

            // Bind to dropdown
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    events.Add(GetEventFromReader(reader));
                }
            }
            reader.Close();
            oConn.Close();
        }
        events.TrimExcess();
       
        return events.ToArray();
    }
    public static int GetOrdinalFromColumnName(SqlDataReader reader, string columnname)
    {
        if (reader == null)
            throw new ArgumentNullException("reader");
        if (columnname == null)
            throw new ArgumentNullException("columnname");
        if (columnname.Length == 0)
            throw new ArgumentException("columnname must not be an empty string");
        int ordinal;
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

        return ordinal;
    }
    public static Event GetEventFromReader(SqlDataReader reader)
    {
        if (reader == null)
            throw new ArgumentNullException("reader");
        Event readEvent = new Event();
        readEvent.dtCreated = reader.GetDateTime(GetOrdinalFromColumnName(reader, "dtCreated"));
        try
        {
            readEvent.dtDue = reader.GetDateTime(GetOrdinalFromColumnName(reader, "dtDue"));
        }
        catch
        {
            readEvent.dtDue = null;
        }
        
        try
        {
            readEvent.dtReminder = reader.GetDateTime(GetOrdinalFromColumnName(reader, "dtReminder"));
        }
        catch
        {
            readEvent.dtReminder = null;
        }
        readEvent.iEventID = reader.GetInt32(GetOrdinalFromColumnName(reader, "PKiEventID"));
        readEvent.iExistingClientID = null;
        readEvent.iNewClientID = null;
        try
        {
            readEvent.iExistingClientID = reader.GetInt32(GetOrdinalFromColumnName(reader, "FKiExistingClientID"));
        }
        catch { }
        try
        {
            readEvent.iNewClientID = reader.GetInt32(GetOrdinalFromColumnName(reader, "FKiNewClientID"));
        }
        catch { }
        readEvent.iAssignedUserID = reader.GetInt32(GetOrdinalFromColumnName(reader, "FKiUserAssignedID"));
        readEvent.iUserID = reader.GetInt32(GetOrdinalFromColumnName(reader, "FKiUserID"));
        readEvent.sEventType = reader.GetString(GetOrdinalFromColumnName(reader, "FKsEventType"));
        readEvent.sMessage = reader.GetString(GetOrdinalFromColumnName(reader, "sMessage"));
        readEvent.sRecurrence = reader.GetString(GetOrdinalFromColumnName(reader, "sRecurrence"));
        readEvent.sStatus = reader.GetString(GetOrdinalFromColumnName(reader, "sStatus"));
        return readEvent;
    }
    
    public static string[] GetEventTypes()
    {
        List<string> types = new List<string>(50);
        using (SqlConnection oConn = new SqlConnection(Connect.sConnStr))
        {
            oConn.Open();

            // Read the list of site clients
            string sSql = "SELECT PKsEventType FROM " + EventTypeTable + " ORDER BY PKsEventType";

            SqlDataReader reader = Connect.getDataCommand(sSql, oConn).ExecuteReader();

            // Bind to dropdown
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                   types.Add(reader.GetString(0));
                }
            }
            reader.Close();
            oConn.Close();
        }
        types.TrimExcess();
        return types.ToArray();
    }

    public enum ClientType{
        NewClient,
        ExistingClient,
    }

    // A client structure
    public struct Client
    {
        public string Name; // Name of client
        public ClientType Type; // Type of client
        public string ID; // ID
    }
    public static Client[] GetEventClients()
    {
        List<Client> clients = new List<Client>(50);
        using (SqlConnection oConn = new SqlConnection(Connect.sConnStr))
        {
            oConn.Open();

            // Read the list of site clients
            string sSql = "SELECT PKiClientID, sName FROM " + ExistingClientTable;
            SqlDataReader reader = Connect.getDataCommand(sSql, oConn).ExecuteReader();

            // Bind to dropdown
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Client client = new Client();
                    client.ID = reader.GetInt32(0).ToString();
                    client.Name = reader.GetString(1);
                    client.Type = ClientType.ExistingClient;
                    clients.Add(client);
                }
            }
            reader.Close();

            sSql = "SELECT PKiNewClientID, sNewClientName FROM " + NewClientTable;
            reader = Connect.getDataCommand(sSql, oConn).ExecuteReader();
            // Bind to dropdown
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Client client = new Client();
                    client.ID = reader.GetInt32(0).ToString();
                    client.Name = reader.GetString(1);
                    client.Type = ClientType.NewClient;
                    clients.Add(client);
                }
            }
            reader.Close();
            oConn.Close();
        }
        clients.TrimExcess();        
        return clients.ToArray();       
    }
    public static Client[] GetEventClientsFor(string UserID)
    {
        List<Client> clients = new List<Client>(50);
        using (SqlConnection oConn = new SqlConnection(Connect.sConnStr))
        {
            oConn.Open();

            // Read the list of site clients
            string sSql = "select marketer_assignment.FKiClientID, existing_clients.sName, existing_clients.sClientNumber  from marketer_assignment join existing_clients on marketer_assignment.FKiClientID = existing_clients.PKiClientID AND marketer_assignment.FKiUserID='" + UserID + "' AND existing_clients.bExcluded = 0 ORDER BY existing_clients.sClientNumber ASC;";
            SqlDataReader reader = Connect.getDataCommand(sSql, oConn).ExecuteReader();

            // Bind to dropdown
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Client client = new Client();
                    client.ID = reader.GetInt32(0).ToString();
                    client.Name = reader.GetString(2) + " " + reader.GetString(1);
                    client.Type = ClientType.ExistingClient;
                    clients.Add(client);
                }
            }
            reader.Close();

            /*sSql = "SELECT PKiNewClientID, sNewClientName FROM " + NewClientTable;
            reader = Connect.getDataCommand(sSql, oConn).ExecuteReader();
            // Bind to dropdown
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Client client = new Client();
                    client.ID = reader.GetInt32(0).ToString();
                    client.Name = reader.GetString(1);
                    client.Type = ClientType.NewClient;
                    clients.Add(client);
                }
            }
            reader.Close();*/
            oConn.Close();
        }
        clients.TrimExcess();
        return clients.ToArray();
    }
    // A user structure
    public struct User
    {
        public string Name; // Name of user
        public string ID; // ID
    }
    public static User[] GetUsers()
    {
        List<User> users = new List<User>(50);
        using (SqlConnection oConn = new SqlConnection(Connect.sConnStr))
        {
            oConn.Open();

            // Read the list of site users
            string sSql = "SELECT PKiUserID, sName, sSurname FROM users  WHERE sType = 'Marketer' ORDER BY sName";
            SqlDataReader reader = Connect.getDataCommand(sSql, oConn).ExecuteReader();

            // Bind to dropdown
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    User user = new User();
                    user.ID = reader.GetInt32(0).ToString();
                    user.Name = reader.GetString(1) + " " + reader.GetString(2);
                    users.Add(user);
                }
            }

            reader.Close();
            oConn.Close();
        }
        users.TrimExcess();
        return users.ToArray();
    }
    public static string GetUserColor(int UserID)
    {
        string colorcode = "White";
        using (SqlConnection oConn = new SqlConnection(Connect.sConnStr))
        {
            string sSql = "SELECT sColorCode FROM " + UsersTable + " WHERE PKiUserID = '" + UserID.ToString() + "'";
            SqlDataReader reader = Connect.getDataCommand(sSql, oConn).ExecuteReader();
            // Bind to dropdown
            if (reader.HasRows)
            {
                reader.Read();
                colorcode = reader.GetString(0);
            }
            reader.Close();
            oConn.Close();
        }

        return colorcode;
    }
    public static string GetUserPhoneNumber(int UserID)
    {
        string number = "";
        using (SqlConnection oConn = new SqlConnection(Connect.sConnStr))
        {
            string sSql = "SELECT sPhoneNumber FROM " + UsersTable + " WHERE PKiUserID = '" + UserID.ToString() + "'";
            SqlDataReader reader = Connect.getDataCommand(sSql, oConn).ExecuteReader();
            // Bind to dropdown
            if (reader.HasRows)
            {
                reader.Read();
                number = reader.GetString(0);
            }
            reader.Close();
            oConn.Close();
        }

        return number;
    }

    public static string GetClientName(int? iExistingClientID, int? iNewClientID)
    {
        string clientname = "Unknown";

        if (iExistingClientID == null && iNewClientID == null)
            return clientname;
        using (SqlConnection oConn = new SqlConnection(Connect.sConnStr))
        {
            string sSql;
            if(iExistingClientID == null)
            {
                sSql = "SELECT sNewClientName FROM " + NewClientTable + " WHERE PKiNewClientID = '" + iNewClientID.ToString() + "'";
            }
            else
            {
                sSql = "SELECT sName FROM " + ExistingClientTable + " WHERE PKiClientID = '" + iExistingClientID.ToString() + "'";
            }

            SqlDataReader reader = Connect.getDataCommand(sSql, oConn).ExecuteReader();
            // Bind to dropdown
            if (reader.HasRows)
            {
                reader.Read();
                clientname = reader.GetString(0);
            }
            reader.Close();
            oConn.Close();
        }

        return clientname;
    }
    public static string GetUserFullName(int iUserID)
    {
        string username = "Unknown";

        using (SqlConnection oConn = new SqlConnection(Connect.sConnStr))
        {
            string sSql = "SELECT sName, sSurname FROM " + UsersTable + " WHERE PKiUserID = '" + iUserID.ToString() + "'";
           
            SqlDataReader reader = Connect.getDataCommand(sSql, oConn).ExecuteReader();
            // Bind to dropdown
            if (reader.HasRows)
            {
                reader.Read();
                username = reader.GetString(0) + " " + reader.GetString(1);
            }
            reader.Close();
            oConn.Close();
        }

        return username;
    }

    public static string GetUserName(int iUserID)
    {
        string username = "Unknown";

        using (SqlConnection oConn = new SqlConnection(Connect.sConnStr))
        {
            string sSql = "SELECT sUserName FROM " + UsersTable + " WHERE PKiUserID = '" + iUserID.ToString() + "'";

            SqlDataReader reader = Connect.getDataCommand(sSql, oConn).ExecuteReader();
            // Bind to dropdown
            if (reader.HasRows)
            {
                reader.Read();
                username = reader.GetString(0);
            }
            reader.Close();
            oConn.Close();
        }

        return username;
    }

    public static string GetClientImageFile(int? iExistingClientID)
    {
        string sImageLocation = "no_photo_default.jpg";
        if (iExistingClientID == null)
            return "~/images/client_images/" + "no_photo_default.jpg";
        using (SqlConnection oConn = new SqlConnection(Connect.sConnStr))
        {
            string sSql = "SELECT sImageFile FROM " + ExistingClientTable + " WHERE sImageFile IS NOT NULL AND PKiClientID = " + iExistingClientID.ToString();
            SqlDataReader reader = Connect.getDataCommand(sSql, oConn).ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                sImageLocation = reader.GetString(0);
            }
            reader.Close();
            oConn.Close();
        }

        return "~/images/client_images/" + sImageLocation;
    }
}