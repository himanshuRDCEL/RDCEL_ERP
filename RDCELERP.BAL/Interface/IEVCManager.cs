using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.City;
using RDCELERP.Model.EVC;
using RDCELERP.Model.EVC_Allocated;
using RDCELERP.Model.EVC_Portal;
using RDCELERP.Model.EVCdispute;
using RDCELERP.Model.InfoMessage;
using RDCELERP.Model.LGC;
using RDCELERP.Model.MobileApplicationModel;
using RDCELERP.Model.Paymant;
using RDCELERP.Model.Users;

namespace RDCELERP.BAL.Interface
{
    public interface IEVCManager
    {
        IList<UserViewModel> GetallEmployeeId();
        int SaveEVCDetails(EVC_RegistrationModel evc_RegistrationModel, int UserId);
        int ApprovedEVC(int EvcregistrationId);
        public EVC_RegistrationModel GetEvcByEvcregistrationId(int id);
        int SaveEVCWalletDetails(EVCWalletAdditionViewModel eVCWalletAdditionViewModels, int userId);
        //IList<RDCELERP.Core.App.Pages.EVC.EVC_ApprovedModel> GetAllApprovedEVC();
        public IList<EVC_RegistrationModel> GetAllEVCRegistration();
        public ExecutionResult EvcByEvcregistrationId(int id);
        public ExecutionResult GetAllWalletSummery();
        public ExecutionResult GetWalletSummeryByEVC(int id);
        public List<Allocate_EVCFromViewModel> ListOfEVCBycity(String ids);
        public ReassignFromViewModel GetReassignEVC(int  OId);

        string AllocateEVCByOrder(List<Allocate_EVCFromViewModel> allocate_EVCFromViewModels,int UserId);
        int AllocateEVCByPrimeOrder(Allocate_EVCFromViewModel allocate_EVCFromViewModels,int UserId);
        int SaveReassignEVC(ReassignFromViewModel reassignFromViewModel, int UserId);
        EVC_PartnerViewModel GetEVCByEVCRegNo(int evcPartnerId, int? orderTransId);
        public ExecutionResult GetApprovedByEVC(int id);
        public ExecutionResult GetEVCAllocation(int id);
        IList<EVCWalletTransaction> GetOrderByEvcregistrationId(int EvcregistrationId);
        public List<MyWalletSummaryAdditionViewModel> EvcUserWalletAdditionHistory(int id);
        public EVC_DashboardViewModel EvcByUserId(int id);
        public List<lattestAllocationViewModel> GetListOFLattestAllocation(int id, bool getassignorder);
        public EVC_RegistrationModel GetEvcByUserId(int id);
        IList<EVcList> GetEVCListbycityAndpin(string state,string city,string pin);
        public IList<EVcListRessign> GetEVCListforEVCReassign(string? ActualProdQltyAtQc, int ProductCatId, string? pin, int EVCId, int? statusId, int OrdertransId, int ExpectedPrice,int ordertype);
        public IList<OrderImageUploadViewModel> GetAllImagesByTransId(int orderTransId);
        public ResponseResult RegisterEVC(EVC_RegistrationModel evc_RegistrationModel);
        public string EVCPaymentstatusUpdate(PaymentResponseModel paymentReponse);
        /// <summary>
        /// this method use for EVCDispute ---Admin/Portal--- Product details by OrdertansID
        /// </summary>
        /// <param name="orderTransId"></param>
        /// <returns></returns>
        public EVCDisputeViewModel GetProductDetailsByTransId(int orderTransId);
        public List<Allocate_EVCFromViewModel> GetEvcForAutoAllocation(int? OrderTransId);

        public List<EVC_PartnerViewModel> GetListOfInHouseEvc(Allocate_EVCFromViewModel allocate);
        public bool AssignEVCByPartnerId(Allocate_EVCFromViewModel allocate,List<EVC_PartnerViewModel> eVCPartnerViewModels, int? orderTransId);
        public List<EVC_PartnerViewModel> GetListOfEvcOtherThanInHouse(Allocate_EVCFromViewModel allocate);

        public int SaveEVCPartnerDetails(EVC_StoreRegistrastionViewModel eVC_StoreRegistrastionViewModels, int userId);
        public int SaveStoreSpecificationDetails(EVCStore_SpecificationViewModel eVCStore_SpecificationViewModel, int userId);

        bool SaveVendorDetails(VendorRegistrationModel vendorRegistrationModel, int userId);
    }
}
