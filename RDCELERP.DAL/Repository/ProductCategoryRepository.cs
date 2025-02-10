
using System.Linq;
using RDCELERP.DAL.AbstractRepository;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;

namespace RDCELERP.DAL.Repository
{
    public class ProductCategoryRepository : AbstractRepository<TblProductCategory>, IProductCategoryRepository
    {
        Digi2l_DevContext _context;
        public ProductCategoryRepository(Digi2l_DevContext dbContext)
         : base(dbContext)
        {
            _context = dbContext;
        }
        public TblProductCategory GeByid(int? id)
        {
            TblProductCategory TblProductCategory = _context.TblProductCategories.FirstOrDefault(x => x.Id == id);
            return TblProductCategory;
        }

        public TblProductCategory GetCategoryById(int? id)
        {
            TblProductCategory? ProductCategoryObj = new TblProductCategory();
            if(id>0)
            {
                ProductCategoryObj = _context.TblProductCategories.Where(x=>x.Id==id && x.IsActive==true).FirstOrDefault();
                if(ProductCategoryObj != null)
                {
                    return ProductCategoryObj;
                }
            }
            return ProductCategoryObj;
        }
    }
}
