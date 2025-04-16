using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RDCELERP.DAL.AbstractRepository;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;


public class BussinessCustomerRepository : AbstractRepository<TblBusinessCustomer>, IBussinessCustomerRepository
{
    private readonly Digi2l_DevContext _dbContext;

    public BussinessCustomerRepository(Digi2l_DevContext dbContext)
        : base(dbContext)
    {
        _dbContext = dbContext;
    }
    /// <summary>
    /// method to get dashboard data by customerid
    /// </summary>
    /// <param name="customerId"></param>
    /// <returns></returns>

public DataTable GetCustomerDashboardById(int customerId)
{
    DataTable dt = new DataTable();

    // Get connection string from DbContext
    string connectionString = _dbContext.Database.GetDbConnection().ConnectionString;

    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        using (SqlCommand cmd = new SqlCommand("GetBusinessCustomerDashboard", connection))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@CustomerId", customerId));

            if (connection.State != ConnectionState.Open)
                connection.Open();

            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                da.Fill(dt);
            }
        }
    }

    return dt;
}

}
