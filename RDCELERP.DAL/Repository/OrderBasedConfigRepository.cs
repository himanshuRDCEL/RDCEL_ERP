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
    public class OrderBasedConfigRepository : AbstractRepository<TblOrderBasedConfig>, IOrderBasedConfigRepository
    {
        Digi2l_DevContext _context;
        public OrderBasedConfigRepository(Digi2l_DevContext dbContext)
       : base(dbContext)
        {
            _context = dbContext;
        }

        public TblOrderBasedConfig GetIsSweetenerModelbase(int? BUId, int? BPId)
        {
            TblOrderBasedConfig TblOrderBasedConfig = null;
            if(BUId > 0 && BPId > 0)
            {
                TblOrderBasedConfig = _context.TblOrderBasedConfigs.FirstOrDefault(x => x.IsActive == true && x.BusinessUnitId == BUId && x.BusinessPartnerId == BPId);
            }

            return TblOrderBasedConfig;
        }

        public TblOrderBasedConfig GetOrderBasedConfigRecordByBusinessPartner(int? bpId)
        {
            TblOrderBasedConfig tblOrderBasedConfig = new TblOrderBasedConfig();

            if (bpId > 0)
            {
                tblOrderBasedConfig = _context.TblOrderBasedConfigs.Where(x=> x.BusinessPartnerId == bpId && x.IsActive == true).FirstOrDefault();
            }
            return tblOrderBasedConfig;
        }

        public bool InsertOrderBasedConfigRecordForBusinessPartner(TblOrderBasedConfig tblOrderBasedConfig,TblBusinessUnit tblBusinessUnit)
        {
            bool IsSuccess = false;
            if (tblOrderBasedConfig != null)
            {
                try
                {
                    _context.TblOrderBasedConfigs.Add(tblOrderBasedConfig);
                    _context.SaveChanges();
                    IsSuccess = true;
                }
                catch 
                { 
                    IsSuccess = false; 
                }
            }
            return IsSuccess;
        }
    }
}
    
    
    

