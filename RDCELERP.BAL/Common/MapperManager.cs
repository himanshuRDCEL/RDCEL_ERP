using AutoMapper;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.Company;
using RDCELERP.Model.Users;
using RDCELERP.Model.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Master;
using RDCELERP.Model.Product;
using RDCELERP.Model.PriceMaster;
using RDCELERP.Model.ProductQuality;
using RDCELERP.Model.PinCode;
using RDCELERP.Model.City;
using RDCELERP.Model.State;
using RDCELERP.Model.StoreCode;
using RDCELERP.Model.Program;
using RDCELERP.Model.BusinessPartner;
using RDCELERP.Model.EVC;
using RDCELERP.Model;
using RDCELERP.Model.ABBRedemption;
using RDCELERP.Model.AbbRegistration;
using RDCELERP.Model.EVC_Allocation;
using RDCELERP.Model.EVC_Allocated;
using RDCELERP.Model.EVCdispute;
using RDCELERP.Model.EVC_Portal;
using RDCELERP.Model.ImagLabel;
using RDCELERP.Model.LGC;
using RDCELERP.Model.ExchangeOrder;
using RDCELERP.Model.QCComment;
using RDCELERP.Model.TimeLine;
using RDCELERP.Model.QC;
using RDCELERP.Model.ServicePartner;
using RDCELERP.Model.BusinessUnit;
using RDCELERP.Model.ModelNumber;
using RDCELERP.Model.ABBPlanMaster;
using RDCELERP.Model.ABBPriceMaster;
using RDCELERP.Model.MobileApplicationModel.LGC;
using RDCELERP.Model.MobileApplicationModel.Questioners;
using RDCELERP.Model.MapSerVicePartner;
using RDCELERP.Model.DriverDetails;
using RDCELERP.Model.MobileApplicationModel.EVC;
using RDCELERP.Model.CommonModel;
using RDCELERP.Model.VehicleIncentive;
using RDCELERP.Model.LGCMobileApp;
using RDCELERP.Model.ProductTechnology;
using RDCELERP.Model.QuestionerLOV;
using RDCELERP.Model.VehicleJourneyViewModel;
using RDCELERP.Model.UniversalPriceMaster;
using RDCELERP.Model.OrderDetails;
using RDCELERP.Model.OrderTrans;
using RDCELERP.Model.ProductConditionLabel;
using RDCELERP.Model.DealerDashBoard;
using RDCELERP.Model.Refurbisher;
using RDCELERP.DAL.Repository;
using RDCELERP.Model.APICalls;

namespace RDCELERP.BAL.Common
{
    public class MapperManager : Profile
    {
        public MapperManager()
        {
            //user, role and company
            CreateMap<TblUser, UserViewModel>().ReverseMap();
            CreateMap<TblUserRole, UserRoleViewModel>().ReverseMap();
            CreateMap<TblCompany, CompanyViewModel>().ReverseMap();
            CreateMap<TblAddress, AddressListViewModel>().ReverseMap();
            CreateMap<TblRole, RoleViewModel>().ReverseMap();
            CreateMap<TblRoleAccess, RoleAccessViewModel>().ReverseMap();
            CreateMap<TblAccessList, AccessListViewModel>().ReverseMap();
            CreateMap<TblAccessList, RoleAccessViewModel>().ReverseMap();
            CreateMap<TblBrand, BrandViewModel>().ReverseMap();
            CreateMap<TblProductCategory, ProductCategoryViewModel>().ReverseMap();
            CreateMap<TblProductType, ProductTypeViewModel>().ReverseMap();
            CreateMap<TblPriceMaster, PriceMasterViewModel>().ReverseMap();
            CreateMap<TblProductQualityIndex, ProductQualityIndexViewModel>().ReverseMap();
            CreateMap<TblPinCode, PinCodeViewModel>().ReverseMap();
            CreateMap<TblCity, CityViewModel>().ReverseMap();
            CreateMap<TblState, StateViewModel>().ReverseMap();
            CreateMap<TblStoreCode, StoreCodeViewModel>().ReverseMap();
            CreateMap<TblProgramMaster, ProgramMasterViewModel>().ReverseMap();
            CreateMap<TblBusinessPartner, BusinessPartnerViewModel>().ReverseMap();
            CreateMap<TblEntityType, EntityViewModel>().ReverseMap();
            CreateMap<TblEvcregistration, EVC_RegistrationModel>().ReverseMap();
            CreateMap<TblEvcregistration, EVC_NotApprovedViewModel>().ReverseMap();
            CreateMap<TblEvcregistration, EVC_ApprovedViewModel>().ReverseMap();
            CreateMap<TblEvcwalletAddition, EVCWalletAdditionViewModel>().ReverseMap();
            CreateMap<TblAbbregistration, AbbRegistrationModel>().ReverseMap();
            CreateMap<TblEvcregistration, AllWalletSummaryViewModel>().ReverseMap();
            CreateMap<TblOrderTran, NotAllocatedOrderViewModel>().ReverseMap();

            CreateMap<TblEvcregistration, EVcList>().ReverseMap();
            CreateMap<TblEvcregistration, EVcListRessign>().ReverseMap();
            CreateMap<TblEvcdispute, EVCDisputeViewModel>().ReverseMap();
            CreateMap<TblAbbredemption, ABBRedemptionViewModel>().ReverseMap();
            CreateMap<TblAbbregistration, ABBRegistrationViewModel>().ReverseMap();
            CreateMap<TblWalletTransaction, EVCWalletTransaction>().ReverseMap();
            CreateMap<TblEvcregistration, EVC_RegistrationPortalViewModel>().ReverseMap();
            CreateMap<TblModelNumber, ModelNumberViewModel>().ReverseMap();
            CreateMap<TblImageLabelMaster, ImageLabelViewModel>().ReverseMap();
            CreateMap<TblImageLabelMaster, ImageLabelNewViewModel>().ReverseMap();
            CreateMap<TblSelfQc, SelfQCViewModel>().ReverseMap();
            CreateMap<TblImageLabelMaster, ImageLabelViewModel>().ReverseMap();
            CreateMap<TblOrderImageUpload, OrderImageUploadViewModel>().ReverseMap();
            CreateMap<TblOrderLgc, OrderLGCViewModel>().ReverseMap();
            CreateMap<TblEvcpoddetail, PODViewModel>().ReverseMap();
            CreateMap<TblEvcdispute, ShowDisputeListViewModel>().ReverseMap();
            CreateMap<TblWalletTransaction, EVCUser_AllOrderRecordViewModel>().ReverseMap();
            CreateMap<TblWalletTransaction, AssignOrderViewModel>().ReverseMap();
            CreateMap<TblWalletTransaction, PickupDeclineViewModel>().ReverseMap();

            CreateMap<TblOrderTran, OrderTransactionViewModel>().ReverseMap();
            CreateMap<TblExchangeAbbstatusHistory, PODViewModel>().ReverseMap();
            CreateMap<TblExchangeAbbstatusHistory, ExchangeAbbstatusHistoryViewModel>().ReverseMap();
            CreateMap<TblTimelineStatusMapping, TimeLineStatusMappingViewModel>().ReverseMap();
            CreateMap<TblExchangeOrder, QCCommentViewModel>().ReverseMap();
            CreateMap<TblExchangeOrder, ExchangeOrderViewModel>().ReverseMap();
            CreateMap<TblOrderTran, QCCommentViewModel>().ReverseMap();
            CreateMap<TblExchangeOrderStatus, ExchangeOrderStatusViewModel>().ReverseMap();
            CreateMap<TblVoucherVerfication, VoucherDetailsViewModel>().ReverseMap();
            CreateMap<TblTimeLine, TimeLineViewModel>().ReverseMap();
            CreateMap<QCCommentViewModel, TblOrderQc>().ReverseMap();

            CreateMap<TblCustomerDetail, CustomerDetailViewModel>().ReverseMap();
            CreateMap<TblCustomerDetail, TimeListViewModel>().ReverseMap();
            CreateMap<TblExchangeOrderStatus, TimeListViewModel>().ReverseMap();
            CreateMap<TblExchangeOrder, TimeListViewModel>().ReverseMap();
            CreateMap<TblExchangeAbbstatusHistory, TimeListViewModel>().ReverseMap();
            CreateMap<TblTimelineStatusMapping, TimeListViewModel>().ReverseMap();
            CreateMap<TblTimeLine, TimeListViewModel>().ReverseMap();
            CreateMap<TblServicePartner, ServicePartnerViewModel>().ReverseMap();
            CreateMap<TblBusinessUnit, BusinessUnitViewModel>().ReverseMap();
            CreateMap<TblAbbplanMaster, ABBPlanMasterViewModel>().ReverseMap();
            CreateMap<TblAbbpriceMaster, ABBPriceMasterViewModel>().ReverseMap();
            CreateMap<TblServicePartner, RegisterServicePartnerDataModel>().ReverseMap();
            CreateMap<MapServicePartnerCityState, MapServicePartnerViewModel>().ReverseMap();

            CreateMap<TblState, StateName>().ReverseMap();
            CreateMap<TblCity, CityList>().ReverseMap();

            CreateMap<TblUser, UserDetailsDataModel>().ReverseMap();

            CreateMap<TblServicePartner, LGC_NotapprovedViewModel>().ReverseMap();
            CreateMap<TblServicePartner, LGC_ApprovedViewModel>().ReverseMap();

            CreateMap<DriverDetailsDataModel, TblDriverDetail>().ReverseMap();
            CreateMap<TblLogistic, orderRegdnolist>().ReverseMap();
            CreateMap<TblPinCode, PinCodesDataModel>().ReverseMap();
            CreateMap<TblServicePartner, LGCUserViewDataModel>().ReverseMap();

            CreateMap<AddPincode, MapServicePartnerCityState>().ReverseMap();
            CreateMap<TblQcratingMaster, QCRatingViewModel>().ReverseMap();
            CreateMap<TblOrderQcrating, QCRatingViewModel>().ReverseMap();

            CreateMap<TblUser, UserRoleLoginViewModel>().ReverseMap();
            CreateMap<TblBusinessPartner, BusinessPartnerVMExcel>().ReverseMap();
            CreateMap<TblPriceMaster, PriceMasterVMExcel>().ReverseMap();
            CreateMap<Login, BusinessUnitLoginVM>().ReverseMap();
            CreateMap<TblProductType, ProductTypeDataResponseModel>().ReverseMap();

            CreateMap<TblProductCategory, BuProductCatDataModel>().ReverseMap();
            CreateMap<TblBrand, BrandViewModels>().ReverseMap();
            CreateMap<TblProductTechnology, ProductTechnologyDataViewModel>().ReverseMap();
            CreateMap<TblQuestionerLov, QuestionerLovidViewModel>().ReverseMap();
            CreateMap<TblQcratingMaster, QCRatingLOVDataViewModel>().ReverseMap();
            CreateMap<ProductDetailsDataViewModel, TblExchangeOrder>().ReverseMap();

            CreateMap<TblDriverDetail, DriverDetailsResponseViewModal>().ReverseMap();

            CreateMap<TblUser, userDataViewModal>().ReverseMap();
            CreateMap<TblPinCode, mappingZipCode>().ReverseMap();
            CreateMap<AbbRegistrationModel, TblAbbregistration>().ReverseMap();
            CreateMap<TblProductType, ProductTypeNameDescription>().ReverseMap();

            CreateMap<TblEvcregistration, EVCResellerRegisterationModel>().ReverseMap();
            CreateMap<TblDriverDetail, DriverDetailsListByCityResponse>().ReverseMap();
            CreateMap<AbbRegistrationModel, ABBRedemptionViewModel>().ReverseMap();


            //Added by  Kranti for LOV Utility Bulk Upload Excel
            CreateMap<TblState, StateVMExcel>().ReverseMap();
            CreateMap<TblCity, CityVMExcel>().ReverseMap();
            CreateMap<TblPinCode, PinCodeVMExcel>().ReverseMap();
            CreateMap<TblProductCategory, ProductCategoryVMExcel>().ReverseMap();
            CreateMap<TblProductType, ProductTypeVMExcel>().ReverseMap();
            CreateMap<TblBrand, BrandVMExcel>().ReverseMap();
            CreateMap<TblDriverDetail, DriverDetailsListByCityResponse>().ReverseMap();
            CreateMap<TblBusinessPartner, BusinessPartnerVMExcelModel>().ReverseMap();
            CreateMap<ExchangeABBStatusHistoryViewModel, TblExchangeAbbstatusHistory>().ReverseMap();

            CreateMap<TblBrandSmartBuy, BrandViewModel>().ReverseMap();

            CreateMap<TblOrderTran, MapOrderTransModel>().ReverseMap();

            CreateMap<TblVehicleJourneyTracking, StartJournyVehicleListbyServicePResponse>().ReverseMap();
            //Added by Kranti for Vehicle Incentive 
            CreateMap<TblVehicleIncentive, VehicleIncentiveViewModel>().ReverseMap();
            // <<<<<<< Dev
            CreateMap<PriceMasterNameViewModel, TblPriceMasterName>().ReverseMap();
            CreateMap<PriceMasterMappingViewModel, TblPriceMasterMapping>().ReverseMap();
            // =======
            CreateMap<TblBubasedSweetnerValidation, BUBasedSweetnerValidation>().ReverseMap();
            // >>>>>>> Sahu_Priyanshi

            // Added By Kranti Silawat
            CreateMap<TblServicePartner, UpdateServicePartnerDataModel>().ReverseMap();
            CreateMap<TblDriverDetail, UpdateDriverDetailsDataModel>().ReverseMap();

            //Added By Abhishek Sharma 
            CreateMap<TblProductTechnology, ProductTechnologyViewModel>().ReverseMap();
            CreateMap<TblQuestionerLov, QuestionerLOVViewModel>().ReverseMap();
            CreateMap<TblUniversalPriceMaster, UniversalPriceMasterViewModel>().ReverseMap();

            //Added by SK
            CreateMap<TblBusinessUnit, BUViewModel>().ReverseMap();

            //Added by Pooja Jatav
            CreateMap<TblLogistic, MobileAppLogisticViewModel>().ReverseMap();
            CreateMap<TblVehicleJourneyTracking, StartvehicleJourneyViewModel>().ReverseMap();

            CreateMap<TblDriverDetail, DriverDetailsViewModel>().ReverseMap();
            CreateMap<TblVehicleJourneyTrackingDetail, VehiclesTrackingDetails>().ReverseMap();
            CreateMap<TblDriverList, DriverListViewModel>().ReverseMap();
            CreateMap<TblVehicleList, VehicleViewModel>().ReverseMap();

            //Added by VK
            CreateMap<DriverDetailsResponseViewModal, DriverDetailsDataModel>().ReverseMap();


            CreateMap<TblOrderTran, OrderTransViewModel>().ReverseMap();
            CreateMap<TblEvcPartner, EVC_PartnerViewModel>().ReverseMap();

            //added by Priyanshi Sahu
            CreateMap<TblEvcPartner, EVC_PartnerViewModel>().ReverseMap();
            CreateMap<TblEvcPartnerPreference, EVC_PartnerpreferenceViewModel>().ReverseMap();

            //CreateMap<DriverDetailsResponseViewModal, DriverDetailsDataModel>().ReverseMap();
            CreateMap<TblVehicleJourneyTrackingDetail, VehicleJourneyTrackDetailsModel>().ReverseMap();

            // Added By Kranti Silawat
            CreateMap<TblProductConditionLabel, ProductConditionLabelViewModel>().ReverseMap();

            // Added By Kranti Silawat
            CreateMap<TblModelMapping, ModelMappingViewModel>().ReverseMap();



            //Pending Orders 
            CreateMap<TblOrderTran, OrderDetailsViewModel>().ReverseMap();
            CreateMap<TblLogistic, OrderDetailsViewModel>().ReverseMap();
            CreateMap<TblOrderTran, PendingForQCVMExcel>().ReverseMap();
            CreateMap<TblOrderTran, PendingForPriceAcceptVMExcel>().ReverseMap();
            CreateMap<TblLogistic, PendingForPickupVMExcel>().ReverseMap();
            
            //voucher data mapping
            CreateMap<TblExchangeOrder, ExchangeOrderDataContract>().ReverseMap();
//<<<<<<< Dev
            CreateMap<TblRefurbisherRegistration, RefurbisherRegViewModel>().ReverseMap();

//=======

            CreateMap<TblProdCatBrandMapping, ProdCatBrandMapViewModel>().ReverseMap();

            //Mapping for Diagnose V2 yashRathores
            CreateMap<TblQuestionerLovmapping, QuestionerLovMappingViewModel>().ReverseMap();
            CreateMap<TblQuestionerLov, QuestionerLovMappingViewModel>().ReverseMap();
            CreateMap<TblQcratingMasterMapping,QcratingMasterMappingVM>().ReverseMap();

            //>>>>>>> DiagnoseV2_API_10-Jan
            CreateMap<TblCreditRequest, PendingCreditApprovalViewModel>().ReverseMap();
            //added by priyanshi
            CreateMap<TblDriverList, UpdateDriverDataModel>().ReverseMap();
            CreateMap<TblDriverList, DriverDataModel>().ReverseMap();
            CreateMap<TblDriverList, DriverResponseViewModal>().ReverseMap();
            CreateMap<TblVehicleList, VehicleDataModel>().ReverseMap();
            CreateMap<TblVehicleList, UpdateVehicleDataModel>().ReverseMap();
            CreateMap<TblVehicleList, VehicleListByCityResponse>().ReverseMap();
            CreateMap<TblDriverDetail, DriverResponseViewModal>().ReverseMap();




            CreateMap<TblQcratingMasterMapping, QuestionsWithLovViewModel>().ReverseMap();
            CreateMap<TblQcratingMaster, QuestionsWithLovViewModel>().ReverseMap();

            //CreateMap<ApicallTable, ApicallViewModel>().ReverseMap();
            CreateMap<ApicallViewModel, TblApicall>().ReverseMap();

        }
    }
}