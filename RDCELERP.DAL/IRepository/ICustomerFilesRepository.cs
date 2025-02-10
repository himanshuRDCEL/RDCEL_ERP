using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.DAL.AbstractRepository;
using RDCELERP.DAL.Entities;

namespace RDCELERP.DAL.IRepository
{
    public interface ICustomerFilesRepository : IAbstractRepository<TblCustomerFile>
    {
        #region Get ABB Invoice Max Sr. Num
        /// <summary>
        /// Get ABB Invoice Max Sr. Num
        /// </summary>
        /// <param name="FinancialYear"></param>
        /// <returns></returns>
        public int? GetAbbMaxInvSrNum(string? FinancialYear);
        #endregion

        #region Get Customer Files by RegdNo
        /// <summary>
        /// Get Customer Files by RegdNo
        /// </summary>
        /// <param name="regdNo"></param>
        /// <returns></returns>
        public TblCustomerFile? GetCustomerFilesByRegdNo(string? regdNo);
        #endregion
    }
}
