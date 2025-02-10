using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.EVC_Allocated;
using RDCELERP.Model.Master;
using RDCELERP.Model.OrderTrans;

namespace RDCELERP.BAL.Interface
{
    public interface IOrderTransactionManager
    {
        int ManageOrderTransaction(OrderTransactionViewModel OrderTransactionVM, int userId = 3);
        public TblOrderTran GetOrderDetailsByRegdNo(string regdno);
        public Allocate_EVCFromViewModel GetOrderDetailsByOrderTransId(int? orderTransId);

        public string AssignOrderForQC(int adminUserId, int selectUser, string orderIds);

    }
}
