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
    public interface IProdCatBrandMappingRepository : IAbstractRepository<TblProdCatBrandMapping>
    {
        #region Get Product Brand List
        /// <summary>
        /// Get Product Brand List
        /// </summary>
        /// <param name="prodCatId"></param>
        /// <returns></returns>
        public List<TblProdCatBrandMapping>? GetProdBrandListByCatId(int? prodCatId);
        #endregion

        #region Get Product Brand Group by brandId and catId
        /// <summary>
        /// Get Product Brand Group by brandId and catId
        /// </summary>
        /// <param name="brandId"></param>
        /// <param name="prodCatId"></param>
        /// <returns></returns>
        public TblProdCatBrandMapping? GetProdCatBrandByBrandAndCat(int? brandId, int? prodCatId);
        #endregion
    }
}
