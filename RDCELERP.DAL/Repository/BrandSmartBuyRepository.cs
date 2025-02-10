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
    public class BrandSmartBuyRepository : AbstractRepository<TblBrandSmartBuy>, IBrandSmartBuyRepository

    {
        #region Variable Declaration
        Digi2l_DevContext _context;
        #endregion

        #region Cunstructor
        public BrandSmartBuyRepository(Digi2l_DevContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }
        #endregion
        public TblBrandSmartBuy GetBrand(int? BrandId)
        {
            TblBrandSmartBuy BrandObj = new TblBrandSmartBuy();
            if (BrandId > 0)
            {
                BrandObj = _context.TblBrandSmartBuys.FirstOrDefault(x => x.Id == BrandId && x.IsActive == true);
            }
            return BrandObj;
        }
    }
}
