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
    public class BrandRepository : AbstractRepository<TblBrand>, IBrandRepository
    {
        Digi2l_DevContext _context;
        public BrandRepository(Digi2l_DevContext dbContext)
       : base(dbContext)
        {
            _context = dbContext;
        }

        public TblBrand GetBrand(int? BrandId)
        {
            TblBrand brandObj = new TblBrand();
            if (BrandId > 0)
            {
                brandObj = _context.TblBrands.FirstOrDefault(x=>x.Id==BrandId&&x.IsActive==true);
            }
            return brandObj;
        }

        public TblBrand GetBrandByBusinessUnit(int? BusinessUnitId)
        {
            TblBrand? tblBrand = null;
            tblBrand = _context.TblBrands.Where(x=>x.BusinessUnitId==BusinessUnitId).FirstOrDefault();
            return tblBrand;
        }
    }
}
    
    
    

