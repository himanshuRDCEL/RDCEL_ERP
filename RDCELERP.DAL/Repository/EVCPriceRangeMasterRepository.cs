using Microsoft.EntityFrameworkCore;
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
    public class EVCPriceRangeMasterRepository : AbstractRepository<TblEvcpriceRangeMaster>, IEVCPriceRangeMasterRepository
    {
        private readonly Digi2l_DevContext _dbContext;
        public EVCPriceRangeMasterRepository(Digi2l_DevContext dbContext)
        : base(dbContext)
        {
            _dbContext = dbContext;
        }

        #region Get EVC Price Range Master Details by priceRange and Buid
        /// <summary>
        /// Get EVC Price Range Master Details by priceRange and Buid
        /// </summary>
        /// <param name="priceRange"></param>
        /// <param name="buid"></param>
        /// <returns></returns>
        public TblEvcpriceRangeMaster? GetEvcPriceRangeMaster(int? priceRange, int? buid)
        {
            TblEvcpriceRangeMaster? tblEvcpriceRangeMaster = null;
            if (priceRange > 0 && buid > 0)
            {
                tblEvcpriceRangeMaster = _dbContext.TblEvcpriceRangeMasters
                                .FirstOrDefault(x => x.IsActive == true && x.PriceStartRange <= priceRange &&
                                                     x.PriceEndRange >= priceRange &&
                                                     x.BusinessUnitId == buid);
            }
            return tblEvcpriceRangeMaster;
        }
        #endregion
    }
}
