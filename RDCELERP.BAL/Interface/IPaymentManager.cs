using RDCELERP.Model.BusinessCustomer;
using RDCELERP.Model.RazorPay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.BAL.Interface
{
  public interface IPaymentManager
    {
        public Task<int> ManagePayment(string paymentId,string orderId, int userid);
        public Task<List<PaymentHistoryViewModel>> GetCustomerPaymentsAsync(int customerId);

    }
}
