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
    public interface IEVCPriceMasterRepository : IAbstractRepository<TblEvcPriceMaster>
    {
        #region Get EVC Price Master Details by ProdTypeId, ProdCatId and Buid
        /// <summary>
        /// Get EVC Price Master Details by ProdTypeId, ProdCatId and Buid
        /// </summary>
        /// <param name="prodTypeId"></param>
        /// <param name="prodCatId"></param>
        /// <param name="buid"></param>
        /// <returns></returns>
        public TblEvcPriceMaster? GetEvcPriceMaster(int? prodTypeId, int? prodCatId, int? buid);
        #endregion
    }
}
