using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.DAL.AbstractRepository;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;

namespace RDCELERP.DAL.Repository
{
    public class PaymentLeaser :AbstractRepository<TblPaymentLeaser>, IPaymentLeaser
    {

        private Digi2l_DevContext _dbContext;
        public PaymentLeaser(Digi2l_DevContext dbContext)
         : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public TblPaymentLeaser GetPaymentdetails(string regdNo)
        {
            TblPaymentLeaser tblPaymentLeaserObj = null;
            if (!string.IsNullOrEmpty(regdNo))
            {
                tblPaymentLeaserObj = _dbContext.TblPaymentLeasers.FirstOrDefault(x=>x.RegdNo==regdNo && x.IsActive==true && x.PaymentStatus==true);
            }
            return tblPaymentLeaserObj;
        }
    }
}
