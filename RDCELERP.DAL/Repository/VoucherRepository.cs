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
    public class VoucherRepository : AbstractRepository<TblVoucherVerfication>, IVoucherRepository
    {
        Digi2l_DevContext _context;
        public VoucherRepository(Digi2l_DevContext dbContext)
         : base(dbContext)
        {
            _context = dbContext;
        }

        public List<TblVoucherVerfication> GetOrderDataforsingeldealer(string AssociateCode)
        {
            List<TblVoucherVerfication> voucherlist = null;
            if(AssociateCode!=null)
            {
                voucherlist = _context.TblVoucherVerfications.Include(x => x.ExchangeOrder).ThenInclude(x => x.BusinessPartner)
                    .Include(x => x.VoucherStatus).Where(x => x.BusinessPartner.AssociateCode == AssociateCode && x.IsActive == true && x.BusinessPartner.IsExchangeBp == true).ToList();
            }
            return voucherlist;
        }

        /// <summary>
        /// Method to get the exchange order by id 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public TblVoucherVerfication GetSingleOrder(int Id)
        {
            TblVoucherVerfication TblVoucherVerfication = _context.TblVoucherVerfications    
                        .Include(x => x.NewBrandId)
                        .Include(x => x.BusinessPartner)                        
                        .Include(x => x.NewProductType).ThenInclude(x => x.ProductCat)                        
                        .FirstOrDefault(x => x.IsActive == true && x.VoucherVerficationId == Id);
            return TblVoucherVerfication;
        }

        public TblVoucherVerfication GetVoucherDataByExchangeId(int exchnageId)
        {
            TblVoucherVerfication voucherData = null;
            if(exchnageId > 0)
            {
                voucherData=_context.TblVoucherVerfications.FirstOrDefault(x=>x.ExchangeOrderId== exchnageId);
            }
            return voucherData;
        }

        public TblVoucherVerfication GetVoucherDataByredemptionId(int redemptionId)
        {
            TblVoucherVerfication? voucherData = null;
            if (redemptionId > 0)
            {
                voucherData = _context.TblVoucherVerfications
                    .Include(x=>x.Redemption)
                    .Include(x=>x.BusinessPartner)
                    .Where(x => x.RedemptionId == redemptionId).FirstOrDefault();
            }
            return voucherData;
        }
    }
}
