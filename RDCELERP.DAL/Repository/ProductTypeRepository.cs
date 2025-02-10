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
    public class ProductTypeRepository : AbstractRepository<TblProductType>, IProductTypeRepository
    {
        Digi2l_DevContext _context;
        public ProductTypeRepository(Digi2l_DevContext dbContext)
         : base(dbContext)
        {
            _context = dbContext;
        }

        public TblProductType GetBytypeid(int? typeid)
        {
            TblProductType TblProductType = _context.TblProductTypes.FirstOrDefault(x => x.Id == typeid && x.IsActive == true);
            return TblProductType;
        }
        public TblProductType GetTypebyId(int? typeid)
        {
            TblProductType TblProductType = _context.TblProductTypes.FirstOrDefault(x => x.Id == typeid && x.IsActive == true);
            return TblProductType;
        }
        public TblProductType GetCatTypebytypeid(int? Typeid) 
        {
            TblProductType? TblProductType = null;
            if(Typeid > 0)
            {
                TblProductType = _context.TblProductTypes
                    .Include(x => x.ProductCat)
                    .FirstOrDefault(x => x.IsActive == true && x.ProductCat != null && x.Id == Typeid);
            }
            return TblProductType;
        }

        #region Get Product Type List
        /// <summary>
        /// Get Product Type List
        /// </summary>
        /// <param name="prodCatId"></param>
        /// <returns></returns>
        public List<TblProductType>? GetProdTypeByCatId(int? prodCatId)
        {
            List<TblProductType>? productTypeslist = null;
            if (prodCatId > 0)
            {
                productTypeslist = _context.TblProductTypes.Where(x=>x.IsActive==true && x.ProductCatId == prodCatId ).ToList();
                     
            }
            return productTypeslist;
        }
        #endregion
    }
}
