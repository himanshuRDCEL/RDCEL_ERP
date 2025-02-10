using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.DAL.AbstractRepository;
using RDCELERP.DAL.Entities;

namespace RDCELERP.DAL.IRepository
{
    public interface IProductTypeRepository : IAbstractRepository<TblProductType>
    {
        public TblProductType GetBytypeid(int? typeid);
        public TblProductType GetTypebyId(int? typeid);
        public TblProductType GetCatTypebytypeid(int? Typeid);
        public List<TblProductType>? GetProdTypeByCatId(int? prodCatId);
    }
  
}
