using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RDCELERP.Model.DealerDashBoard;

using RDCELERP.Model;
using RDCELERP.Model.ExchangeOrder;
using RDCELERP.Model.MobileApplicationModel;
using RDCELERP.Model.MobileApplicationModel.Questioners;
using RDCELERP.Model.QCComment;

namespace RDCELERP.BAL.Interface
{
    public interface IExchangeOrderManager
    {
        public int ManageExchangeOrder(ExchangeOrderViewModel ExchangeOrderVM, int userId);
        //public int ManageExchangeOrder(ExchangeOrderViewModel ExchangeOrderVM, QCCommentViewModel QCCommentVM, QCCommentViewModel commentViewModel, VoucherDetailsViewModel voucherDetailsViewModel, int userId);
        ExchangeOrderViewModel GetExchangeOrderById(int id);      
        bool DeletExchangeOrderById(int id);       
        ExchangeOrderViewModel GetQCOrderByExchangeId(int Id);      
        public List<ExchangeOrderStatusViewModel> GetAllFlag();      
        ExchangeOrderViewModel GetPQRSPrice(ExchangeOrderViewModel exchangeOrderViewModel);
        public bool sendSelfQCUrl(string regdNo, string mobnumber, int? loginid);

        public ResponseResult AddMultipleOrders(MultipleExchangeOrdersDataModel multipleExchangeOrdersDataModel, string username);
        public ExchangeOrderViewModel ManageExchangeOrderBulk(ExchangeOrderViewModel ExchangeVM, int userId);
        public ExchangeBulkLiquidatioModel ManageExchangeBulkUpload(ExchangeBulkLiquidatioModel ExchangeVM, int userId);
        public string AddExchangeOrders(ExchangeOrderDataContract ExchangeOrdersDataModel);

        // Added for Diagnose V2
        public ResponseResult AddMultipleOrdersV2(MultipleExchangeOrdersDataModel multipleExchangeOrdersDataModel, string username);
    }
}
