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
    public interface IBUProductCategoryMapping : IAbstractRepository<TblBuproductCategoryMapping>
    {
        public List<TblBuproductCategoryMapping> GetBUProdCatList(int? buid);

        
    }
}
