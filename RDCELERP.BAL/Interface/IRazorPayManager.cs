using RDCELERP.Model.Paymant;
using RDCELERP.Model.RazorPay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.BAL.Interface
{
    public interface IRazorPayManager
    {
        public RazorpayOrderModel CreateOrder(int amountInRupees, string name, string email, string contact,string OrderNo);

        public Task<ResponseRazorpayPaymentViewModel> GetPaymentDetails(string paymentId);

    }
}
