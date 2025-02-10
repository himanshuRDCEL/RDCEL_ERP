using RDCELERP.DAL.Helper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace RDCELERP.DAL.Helper
{
    public class ADOHelper : IADOHelper
    {
        //public string connectionstring = string.Empty;

        IConfiguration _iconfiguration;

        public ADOHelper(IConfiguration iconfiguration)
        {
            _iconfiguration = iconfiguration;

        }
        #region Declare Variables 

        SqlConnection connection = null;
        SqlCommand command = null;
        public string connectionstring1 = string.Empty; //_iconfiguration.GetValue<string>[""].ToString();// ["ConnectionString"]; //ConfigurationManager.App["GaayakDBEntitiesSQL"].ToString();

        #endregion

        /// <summary>
        /// Execute procedure 
        /// </summary>
        /// <param name="commandName">procedure name</param>
        /// <param name="paramCollection">collatin of paramter</param>
        /// <returns></returns>
        public DataSet ExecuteProcedure(string commandName, string dbConnectionString, List<SqlParameter> paramCollection)
        {
            DataSet ds = new DataSet();
            try
            {
                connection = new SqlConnection(dbConnectionString);
                command = new SqlCommand(commandName, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                foreach (SqlParameter item in paramCollection)
                {
                    command.Parameters.Add(item);
                }
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = command;
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                //Logging.WriteErrorToLog("DBHelper", "ExecuteProcedure", ex);

            }
            finally
            {
                connection.Close();
            }
            return ds;
        }
      
        public DataSet ExecuteDataSet(string oSql, string dbConnectionString, params SqlParameter[] sqlParam)
        {
            DataSet oDS = null;
            try
            {
                connection = new SqlConnection(dbConnectionString);


                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = oSql;
                if ((sqlParam != null))
                {
                    foreach (SqlParameter param in sqlParam)
                    {
                        cmd.Parameters.Add(param);
                    }
                }
                SqlDataAdapter oDA = new SqlDataAdapter(cmd);
                oDS = new DataSet();
                oDA.Fill(oDS);


            }
            catch (Exception ex)
            {
                //Logging.WriteErrorToLog("DBHelper", "ExecuteDataSet", ex);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
            return oDS;
        }

        /// <summary>
        /// Method to get the DataTable from the procedure.
        /// </summary>
        /// <param name="oSql"></param>
        /// <param name="sqlParam"></param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string oSql, string dbConnectionString, params SqlParameter[] sqlParam)
        {
            DataSet oDS = null;
            DataTable oDT = null;
            try
            {
                connection = new SqlConnection(dbConnectionString);


                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = oSql;
                if ((sqlParam != null))
                {
                    foreach (SqlParameter param in sqlParam)
                    {
                        cmd.Parameters.Add(param);
                    }
                }
                SqlDataAdapter oDA = new SqlDataAdapter(cmd);
                oDS = new DataSet();
                oDA.Fill(oDS);


                if (oDS != null && oDS.Tables.Count > 0)
                {
                    oDT = new DataTable();
                    oDT = oDS.Tables[0];
                }

            }
            catch (Exception ex)
            {
                //Logging.WriteErrorToLog("DBHelper", "ExecuteDataTable", ex);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
            return oDT;
        }

        public int ExecuteNonQuery(string commandName, string dbConnectionString, List<SqlParameter> paramCollection)
        {
            int rowAffected = 0;
            try
            {
                connection = new SqlConnection(dbConnectionString);
                connection.Open();
                command = new SqlCommand(commandName, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                foreach (SqlParameter item in paramCollection)
                {
                    command.Parameters.Add(item);
                }
                rowAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                //Logging.WriteErrorToLog("DBHelper", "ExecuteNonQuery", ex);
            }
            finally
            {
                connection.Close();
            }
            return rowAffected;
        }

    }
}
