using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Data;
using System.Text;
using Pervasive.Data.SqlClient;
using System.Data.SqlClient;

namespace PastelCrmDataPump.Classes
{
    class Connect
    {
        public static string sPastelConnStr = GetConnectionStringByName("PervasiveConnStrLocal");
        public static string sCRMConnStr = GetConnectionStringByName("MSSqlConnStrLocalS");
      
        public static string GetConnectionStringByName(string name)
        {
            // Assume failure.
            string returnValue = null;

            // Look for the name in the connectionStrings section.
            var settings = ConfigurationManager.ConnectionStrings[name];

            // If found, return the connection string.
            if (settings != null)
                returnValue = settings.ConnectionString;

            return returnValue;
        }

        public static DataSet getDataSet(string sSQL, string sTable, PsqlConnection conn)
        {
            DataSet ds = new DataSet();
            
            // create a data adapter 
            PsqlDataAdapter da = new PsqlDataAdapter(sSQL, conn);

            da.Fill(ds, sTable);
            return (ds);
        }

        public static SqlCommand getDataCommand(string sSql, SqlConnection conn)
        {
            var cmdSQL = new SqlCommand(sSql, conn);
            return cmdSQL;
        }

        public static PsqlCommand getDataCommand(string sSql, PsqlConnection conn )
        {
            var cmdSQL = new PsqlCommand(sSql, conn);
            return cmdSQL;
        }
    }

}
