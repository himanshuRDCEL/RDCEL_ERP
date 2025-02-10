using RDCELERP.DAL.AbstractRepository;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace RDCELERP.DAL.Repository
{
    public class UserRepository : AbstractRepository<TblUser>, IUserRepository
    {
        Digi2l_DevContext _context;
        public UserRepository(Digi2l_DevContext dbContext)
         : base(dbContext)
        {
            _context = dbContext;
        }


        public TblUser GetByUserId(int? id)
        {
            TblUser TblUser = _context.TblUsers.FirstOrDefault(x => x.UserId == id);

            return TblUser;
        }

        ///// <summary>
        ///// Delete CCTV check async
        ///// </summary>
        ///// <param name="cctvdataId"></param>
        //public DataTable GetRiskOwners(int companyId)
        //{
        //    string sql = "EXEC sp_GetRiskOwners @CompanyId";
        //    List<SqlParameter> parms = new List<SqlParameter>
        //    {
        //            new SqlParameter { ParameterName = "@CompanyId", Value = companyId }
        //    };
        //    _db.out.F. Exe(sql, parms.ToArray());

        //}
    }
}
