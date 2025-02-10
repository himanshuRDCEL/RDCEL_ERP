using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.DAL.AbstractRepository;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;

namespace RDCELERP.DAL.Repository
{
    public class CustomerFilesRepository : AbstractRepository<TblCustomerFile>, ICustomerFilesRepository
    {
        Digi2l_DevContext _context;
        public CustomerFilesRepository(Digi2l_DevContext dbContext)
         : base(dbContext)
        {
            _context = dbContext;
        }

        #region Get ABB Invoice Max Sr. Num
        public int? GetAbbMaxInvSrNum(string? FinancialYear)
        {
            int? maxInvNum = 0;
            if (!string.IsNullOrEmpty(FinancialYear))
            {
                maxInvNum = _context.TblCustomerFiles.Where(x => x.IsActive == true && (x.FinancialYear ?? "").Trim().ToLower() == FinancialYear).Max(x => x.InvSrNum);
            }
            return maxInvNum;
        }
        #endregion

        #region Get Customer Files by RegdNo
        public TblCustomerFile? GetCustomerFilesByRegdNo(string? regdNo)
        {
            TblCustomerFile? tblCustomerFile = null;
            if (!string.IsNullOrEmpty(regdNo))
            {
                regdNo = regdNo.Trim().ToLower();
                tblCustomerFile = _context.TblCustomerFiles.Where(x => x.IsActive == true && (x.RegdNo ?? "").Trim().ToLower() == regdNo).FirstOrDefault();
            }
            return tblCustomerFile;
        }
        #endregion
    }
}
