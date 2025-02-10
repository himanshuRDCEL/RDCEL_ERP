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
    public class ProdCatBrandMappingRepository : AbstractRepository<TblProdCatBrandMapping>, IProdCatBrandMappingRepository
    {
        private readonly Digi2l_DevContext _context;
        public ProdCatBrandMappingRepository(Digi2l_DevContext dbContext)
      : base(dbContext)
        {
            _context = dbContext;
        }

        #region Get Product Brand List
        /// <summary>
        /// Get Product Brand List
        /// </summary>
        /// <param name="prodCatId"></param>
        /// <returns></returns>
        public List<TblProdCatBrandMapping>? GetProdBrandListByCatId(int? prodCatId)
        {
            List<TblProdCatBrandMapping>? tblProdCatBrandMappList = null;
            if (prodCatId > 0)
            {
                tblProdCatBrandMappList = _context.TblProdCatBrandMappings
                     .Include(x => x.Brand)
                    .Where(x => x.IsActive == true && x.ProductCatId != null && x.ProductCatId == prodCatId).GroupBy(x => x.BrandId).Select(x => x.FirstOrDefault()).ToList();
            }
            return tblProdCatBrandMappList;
        }
        #endregion

        #region Get Product Brand Group by brandId and catId
        /// <summary>
        /// Get Product Brand Group by brandId and catId
        /// </summary>
        /// <param name="brandId"></param>
        /// <param name="prodCatId"></param>
        /// <returns></returns>
        public TblProdCatBrandMapping? GetProdCatBrandByBrandAndCat(int? brandId,int? prodCatId)
        {
            TblProdCatBrandMapping? tblProdCatBrandMappList = null;
            if (brandId > 0 && prodCatId > 0)
            {
                tblProdCatBrandMappList = _context.TblProdCatBrandMappings
                     .Include(x => x.Brand).Include(x => x.BrandGroup).Include(x => x.ProductCat)
                    .Where(x => x.IsActive == true && x.BrandId == brandId && x.ProductCatId == prodCatId).FirstOrDefault();
            }
            return tblProdCatBrandMappList;
        }
        #endregion
    }
}
