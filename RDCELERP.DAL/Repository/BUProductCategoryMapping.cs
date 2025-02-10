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
    public class BUProductCategoryMapping : AbstractRepository<TblBuproductCategoryMapping>, IBUProductCategoryMapping
    {
        Digi2l_DevContext _context;
        public BUProductCategoryMapping(Digi2l_DevContext dbContext)
      : base(dbContext)
        {
            _context= dbContext;
        }

        public List<TblBuproductCategoryMapping> GetBUProdCatList(int? buid)
        {
            List<TblBuproductCategoryMapping>? CategoryWithProducts = null;
            CategoryWithProducts = _context.TblBuproductCategoryMappings.Where(x => x.IsActive == true && x.BusinessUnitId == buid).ToList();
            return CategoryWithProducts;
        }
    }
}
