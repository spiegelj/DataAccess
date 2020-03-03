using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace DataAccess
{
	public static class DataAccess
	{
        public const string connectionString = @"Server=localhost;Database=Melodic;Trusted_Connection=yes;";

        //rethinking b/c sql parames are not our friend.
        public static DataTable GetTable(string procName, Dictionary<string, object> parameters)
        {
            if (parameters == null)
                // Make sure there's at least a non-null object to work with
                parameters = new Dictionary<string, object>();

            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = PrepCommand(procName, parameters, connection);
                command.CommandType = CommandType.StoredProcedure;

                try
                {
                    connection.Open();
                    var r = (IDataReader)command.ExecuteReader();
                    dt.Load(r);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
            return dt;
        }

        private static SqlCommand PrepCommand(string procName, Dictionary<string, object> parameters, SqlConnection conn)
        {
            SqlCommand command = new SqlCommand(procName, conn);
            if (parameters != null)
                foreach (KeyValuePair<string, object> kvp in parameters)
                {
                    command.Parameters.AddWithValue(kvp.Key, kvp.Value);
                }
            command.CommandType = CommandType.StoredProcedure;
            return command;
        }

        public static int GetScalar(string procName, Dictionary<string, object> parameters)
        {
            int result = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = PrepCommand(procName, parameters, connection);

                try
                {
                    connection.Open();
                    result = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    // TODO:  Add own logging.  Can't circularly reference Logging.Log.ToDB;
                }
                finally
                {
                    connection.Close();
                }
            }
            return result;
        }
    }

}
