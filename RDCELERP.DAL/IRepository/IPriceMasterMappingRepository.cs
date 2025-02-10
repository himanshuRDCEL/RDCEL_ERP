using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.DAL.AbstractRepository;
using RDCELERP.DAL.Entities;

namespace RDCELERP.DAL.IRepository
{
    public interface IPriceMasterMappingRepository : IAbstractRepository<TblPriceMasterMapping>
    {
        public TblPriceMasterMapping GetPriceMasterMappingById(int? PriceMasterNameId);
        public TblPriceMasterMapping GetProductPriceByBUIdBPIdBrandId(int? BUId, int? BPId, int? NewBrandId);
        public TblPriceMasterMapping GetProductPriceByBUIdBPId(int? BUId, int? BPId);
    }
}
