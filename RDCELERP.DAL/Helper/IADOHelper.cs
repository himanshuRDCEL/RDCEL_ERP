using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.DAL.Helper
{
    public interface IADOHelper
    {

        public DataSet ExecuteProcedure(string commandName, string dbConnectionString, List<SqlParameter> paramCollection);

        /// <summary>
        /// Method to get the Dataset from the procedure.
        /// </summary>
        /// <param name="oSql"></param>
        /// <param name="sqlParam"></param>
        /// <returns>DataSet</returns>
        public DataSet ExecuteDataSet(string oSql, string dbConnectionString, params SqlParameter[] sqlParam);

        /// <summary>
        /// Method to get the DataTable from the procedure.
        /// </summary>
        /// <param name="oSql"></param>
        /// <param name="sqlParam"></param>
        /// <returns>DataTable</returns>
        public DataTable ExecuteDataTable(string oSql, string dbConnectionString, params SqlParameter[] sqlParam);

        /// <summary>
        /// Method to execute query
        /// </summary>
        /// <param name="commandName"></param>
        /// <param name="paramCollection"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string commandName, string dbConnectionString, List<SqlParameter> paramCollection);
    }
}
