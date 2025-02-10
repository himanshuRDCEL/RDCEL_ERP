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
    public interface IEVCPriceRangeMasterRepository : IAbstractRepository<TblEvcpriceRangeMaster>
    {
        #region Get EVC Price Range Master Details by priceRange and Buid
        /// <summary>
        /// Get EVC Price Range Master Details by priceRange and Buid
        /// </summary>
        /// <param name="priceRange"></param>
        /// <param name="buid"></param>
        /// <returns></returns>
        public TblEvcpriceRangeMaster? GetEvcPriceRangeMaster(int? priceRange, int? buid);
        #endregion
    }
}
