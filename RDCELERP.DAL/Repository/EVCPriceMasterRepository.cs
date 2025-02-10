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
    public class EVCPriceMasterRepository : AbstractRepository<TblEvcPriceMaster>, IEVCPriceMasterRepository
    {
        private readonly Digi2l_DevContext _dbContext;
        public EVCPriceMasterRepository(Digi2l_DevContext dbContext)
        : base(dbContext)
        {
            _dbContext = dbContext;
        }

        #region Get EVC Price Master Details by ProdTypeId, ProdCatId and Buid
        /// <summary>
        /// Get EVC Price Master Details by ProdTypeId, ProdCatId and Buid
        /// </summary>
        /// <param name="prodTypeId"></param>
        /// <param name="prodCatId"></param>
        /// <param name="buid"></param>
        /// <returns></returns>
        public TblEvcPriceMaster? GetEvcPriceMaster(int? prodTypeId, int? prodCatId, int? buid)
        {
            TblEvcPriceMaster? tblEvcPriceMaster = null;
            if (prodTypeId > 0 && prodCatId > 0 && buid > 0)
            {
                tblEvcPriceMaster = _dbContext.TblEvcPriceMasters
                                   .FirstOrDefault(x => x.ProductTypeId == prodTypeId &&
                                                        x.ProductCategoryId == prodCatId &&
                                                        x.BusinessUnitId == buid);
            }
            return tblEvcPriceMaster;
        }
        #endregion
    }
}
