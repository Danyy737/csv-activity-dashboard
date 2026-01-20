using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace PART2OOPPROJECTUI_DANIELMOURAD
{

    /// <summary>
    /// Provides simple ADO.NET helpers to call stored procedures in ActivitiesDb.
    /// </summary>
    public static class DatabaseHelper

    {
        // LocalDB connection
        private const string ConnectionString =
     "Server=(localdb)\\MSSQLLocalDB;Database=ActivitiesDb;Trusted_Connection=True;";


        /// <summary>
        /// Executes a stored procedure and returns the results in a DataTable.
        /// </summary>

        /// <summary>
        /// Executes the specified stored procedure and returns the results in a DataTable.
        /// </summary>
        /// <param name="procName">Stored procedure name (e.g., "GetAllActivities").</param>
        /// <param name="parameters">Optional SQL parameters passed to the procedure.</param>
        /// <returns>A DataTable containing the result set.</returns>
        public static DataTable ExecuteStoredProcedure(string procName, params SqlParameter[] parameters)

        {
            var table = new DataTable();

            using (var conn = new SqlConnection(ConnectionString))
            using (var cmd = new SqlCommand(procName, conn))
            using (var adapter = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                if (parameters != null && parameters.Length > 0)
                    cmd.Parameters.AddRange(parameters);

                conn.Open();
                adapter.Fill(table);
            }

            return table;
        }

        /// <summary>
        /// Convenience wrapper: Get all activities.
        /// </summary>
        public static DataTable GetAllActivities() =>
            ExecuteStoredProcedure("GetAllActivities");


        /// <summary>
        /// Searches activities by date using the provided operator ('before', 'on', or 'after').
        /// </summary>
        /// <param name="date">The date to compare against (date component is used).</param>
        /// <param name="op">Operator: 'before', 'on', or 'after'.</param>
        /// <returns>DataTable with matching rows.</returns>

        public static DataTable SearchActivitiesByDate(DateTime date, string op) =>
            ExecuteStoredProcedure(
                "SearchActivitiesByDate",
                new SqlParameter("@SearchDate", SqlDbType.Date) { Value = date.Date },
                new SqlParameter("@Operator", SqlDbType.NVarChar, 10) { Value = op }
            );
    
    
public static bool CheckActivityExistsByDate(DateTime date)
        {
            var table = ExecuteStoredProcedure(
                "CheckActivityExistsByDate",
                new SqlParameter("@DateToCheck", SqlDbType.Date) { Value = date.Date });

            // Stored proc returns a single row with ActivityExists = 1 or 0
            if (table.Rows.Count == 0) return false;
            var existsObj = table.Rows[0]["ActivityExists"];
            return existsObj != null && Convert.ToInt32(existsObj) == 1;
        }

        /// <summary>
        /// Adds a fitness activity via stored procedure.
        /// </summary>
        public static void AddFitnessActivity(DateTime dateStartTime, string title, decimal cost, string location)
        {
            // Validation aligned to DB constraints (title length, cost >= 0)
            ExecuteStoredProcedure(
                "AddFitnessActivity",
                new SqlParameter("@DateStartTime", SqlDbType.DateTime) { Value = dateStartTime },
                new SqlParameter("@Title", SqlDbType.NVarChar, 50) { Value = title },
                new SqlParameter("@Cost", SqlDbType.Decimal) { Value = cost },
                new SqlParameter("@Location", SqlDbType.NVarChar, 100) { Value = location }
            );
        }

        /// <summary>
        /// Adds an entertainment activity via stored procedure.
        /// </summary>
        public static void AddEntertainmentActivity(DateTime dateStartTime, string title, decimal cost, int minParticipants)
        {
            ExecuteStoredProcedure(
                "AddEntertainmentActivity",
                new SqlParameter("@DateStartTime", SqlDbType.DateTime) { Value = dateStartTime },
                new SqlParameter("@Title", SqlDbType.NVarChar, 50) { Value = title },
                new SqlParameter("@Cost", SqlDbType.Decimal) { Value = cost },
                new SqlParameter("@MinParticipants", SqlDbType.Int) { Value = minParticipants }
            );
        }

        /// <summary>
        /// Replaces (updates) an existing activity by ActivityID (changes type if needed).
        /// </summary>
        public static void UpdateActivityWithTypeCheck(
            int activityId,
            DateTime newDateStartTime,
            string newTitle,
            decimal newCost,
            string newType,           // "Fitness" or "Entertainment"
            string location = null,
            int? minParticipants = null)
        {
            ExecuteStoredProcedure(
                "UpdateActivityWithTypeCheck",
                new SqlParameter("@ActivityID", SqlDbType.Int) { Value = activityId },
                new SqlParameter("@NewDateStartTime", SqlDbType.DateTime) { Value = newDateStartTime },
                new SqlParameter("@NewTitle", SqlDbType.NVarChar, 50) { Value = newTitle },
                new SqlParameter("@NewCost", SqlDbType.Decimal) { Value = newCost },
                new SqlParameter("@NewType", SqlDbType.NVarChar, 20) { Value = newType },
                new SqlParameter("@Location", SqlDbType.NVarChar, 100) { Value = (object)location ?? DBNull.Value },
                new SqlParameter("@MinParticipants", SqlDbType.Int) { Value = (object)minParticipants ?? DBNull.Value }
            );
        }
    }
}



