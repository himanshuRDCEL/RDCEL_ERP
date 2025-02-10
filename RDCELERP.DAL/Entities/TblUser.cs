using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblUser
    {
        public TblUser()
        {
            InverseCreatedByNavigation = new HashSet<TblUser>();
            InverseModifiedByNavigation = new HashSet<TblUser>();
            MapLoginUserDeviceCreatedByNavigations = new HashSet<MapLoginUserDevice>();
            MapLoginUserDeviceModifiedByNavigations = new HashSet<MapLoginUserDevice>();
            MapLoginUserDeviceUsers = new HashSet<MapLoginUserDevice>();
            MapServicePartnerCityStateCreatedByNavigations = new HashSet<MapServicePartnerCityState>();
            MapServicePartnerCityStateModifiedByNavigations = new HashSet<MapServicePartnerCityState>();
            PriceMasterNameCreatedByNavigations = new HashSet<PriceMasterName>();
            PriceMasterNameModifiedByNavigations = new HashSet<PriceMasterName>();
            TblAbbredemptionCreatedByNavigations = new HashSet<TblAbbredemption>();
            TblAbbredemptionModifiedByNavigations = new HashSet<TblAbbredemption>();
            TblAccessListCreatedByNavigations = new HashSet<TblAccessList>();
            TblAccessListModifiedByNavigations = new HashSet<TblAccessList>();
            TblAddressCreatedByNavigations = new HashSet<TblAddress>();
            TblAddressModifiedByNavigations = new HashSet<TblAddress>();
            TblAddressUsers = new HashSet<TblAddress>();
            TblApicallCreatedByNavigations = new HashSet<TblApicall>();
            TblApicallModifiedByNavigations = new HashSet<TblApicall>();
            TblBppincodeMappingCreatedByNavigations = new HashSet<TblBppincodeMapping>();
            TblBppincodeMappingModifiedByNavigations = new HashSet<TblBppincodeMapping>();
            TblBrandGroupCreatedbyNavigations = new HashSet<TblBrandGroup>();
            TblBrandGroupModifiedByNavigations = new HashSet<TblBrandGroup>();
            TblBubasedSweetnerValidationCreatedByNavigations = new HashSet<TblBubasedSweetnerValidation>();
            TblBubasedSweetnerValidationModifiedByNavigations = new HashSet<TblBubasedSweetnerValidation>();
            TblBuconfigurationCreatedByNavigations = new HashSet<TblBuconfiguration>();
            TblBuconfigurationMappingCreatedByNavigations = new HashSet<TblBuconfigurationMapping>();
            TblBuconfigurationMappingModifiedByNavigations = new HashSet<TblBuconfigurationMapping>();
            TblBuconfigurationModifiedByNavigations = new HashSet<TblBuconfiguration>();
            TblBuproductCategoryMappingCreatedByNavigations = new HashSet<TblBuproductCategoryMapping>();
            TblBuproductCategoryMappingModifiedByNavigations = new HashSet<TblBuproductCategoryMapping>();
            TblCompanyCreatedByNavigations = new HashSet<TblCompany>();
            TblCompanyModifiedByNavigations = new HashSet<TblCompany>();
            TblCreditRequestCreatedByNavigations = new HashSet<TblCreditRequest>();
            TblCreditRequestCreditRequestApproveUsers = new HashSet<TblCreditRequest>();
            TblCreditRequestCreditRequestUsers = new HashSet<TblCreditRequest>();
            TblCreditRequestModifiedByNavigations = new HashSet<TblCreditRequest>();
            TblCustomerFileCreatedByNavigations = new HashSet<TblCustomerFile>();
            TblCustomerFileModifiedByNavigations = new HashSet<TblCustomerFile>();
            TblDriverDetails = new HashSet<TblDriverDetail>();
            TblDriverListCreatedByNavigations = new HashSet<TblDriverList>();
            TblDriverListModifiedByNavigations = new HashSet<TblDriverList>();
            TblDriverListUsers = new HashSet<TblDriverList>();
            TblEntityTypeCreatedByNavigations = new HashSet<TblEntityType>();
            TblEntityTypeModifiedByNavigations = new HashSet<TblEntityType>();
            TblEvcPartnerCreatedbyNavigations = new HashSet<TblEvcPartner>();
            TblEvcPartnerModifiedByNavigations = new HashSet<TblEvcPartner>();
            TblEvcPartnerPreferenceCreatedbyNavigations = new HashSet<TblEvcPartnerPreference>();
            TblEvcPartnerPreferenceModifiedByNavigations = new HashSet<TblEvcPartnerPreference>();
            TblEvcdisputeCreatedByNavigations = new HashSet<TblEvcdispute>();
            TblEvcdisputeModifiedByNavigations = new HashSet<TblEvcdispute>();
            TblEvcpoddetailCreatedByNavigations = new HashSet<TblEvcpoddetail>();
            TblEvcpoddetailModifiedByNavigations = new HashSet<TblEvcpoddetail>();
            TblEvcregistrationCreatedByNavigations = new HashSet<TblEvcregistration>();
            TblEvcregistrationModifiedByNavigations = new HashSet<TblEvcregistration>();
            TblEvcwalletAdditionCreatedByNavigations = new HashSet<TblEvcwalletAddition>();
            TblEvcwalletAdditionModifiedByNavigations = new HashSet<TblEvcwalletAddition>();
            TblEvcwalletHistoryCreatedByNavigations = new HashSet<TblEvcwalletHistory>();
            TblEvcwalletHistoryModifiedByNavigations = new HashSet<TblEvcwalletHistory>();
            TblExchangeAbbstatusHistoryCreatedByNavigations = new HashSet<TblExchangeAbbstatusHistory>();
            TblExchangeAbbstatusHistoryModifiedByNavigations = new HashSet<TblExchangeAbbstatusHistory>();
            TblExchangeOrderCreatedByNavigations = new HashSet<TblExchangeOrder>();
            TblExchangeOrderModifiedByNavigations = new HashSet<TblExchangeOrder>();
            TblImageLabelMasterCreatedByNavigations = new HashSet<TblImageLabelMaster>();
            TblImageLabelMasterModifiedbyNavigations = new HashSet<TblImageLabelMaster>();
            TblLoginMobiles = new HashSet<TblLoginMobile>();
            TblLogisticCreatedByNavigations = new HashSet<TblLogistic>();
            TblLogisticModifiedbyNavigations = new HashSet<TblLogistic>();
            TblNpssqoptionCreatedByNavigations = new HashSet<TblNpssqoption>();
            TblNpssqoptionModifiedByNavigations = new HashSet<TblNpssqoption>();
            TblNpssqresponseCreatedByNavigations = new HashSet<TblNpssqresponse>();
            TblNpssqresponseModifiedByNavigations = new HashSet<TblNpssqresponse>();
            TblNpssquestionCreatedByNavigations = new HashSet<TblNpssquestion>();
            TblNpssquestionModifiedByNavigations = new HashSet<TblNpssquestion>();
            TblOrderImageUploadCreatedByNavigations = new HashSet<TblOrderImageUpload>();
            TblOrderImageUploadModifiedbyNavigations = new HashSet<TblOrderImageUpload>();
            TblOrderLgcCreatedByNavigations = new HashSet<TblOrderLgc>();
            TblOrderLgcModifiedByNavigations = new HashSet<TblOrderLgc>();
            TblOrderQcratingCreatedByNavigations = new HashSet<TblOrderQcrating>();
            TblOrderQcratingModifiedByNavigations = new HashSet<TblOrderQcrating>();
            TblOrderQcs = new HashSet<TblOrderQc>();
            TblOrderTranAssignByNavigations = new HashSet<TblOrderTran>();
            TblOrderTranAssignToNavigations = new HashSet<TblOrderTran>();
            TblOrderTranCreatedByNavigations = new HashSet<TblOrderTran>();
            TblOrderTranModifiedByNavigations = new HashSet<TblOrderTran>();
            TblOrderTranSelfQclinkResendbyNavigations = new HashSet<TblOrderTran>();
            TblPriceMasterMappingCreatedByNavigations = new HashSet<TblPriceMasterMapping>();
            TblPriceMasterMappingModifiedByNavigations = new HashSet<TblPriceMasterMapping>();
            TblPriceMasterNameCreatedByNavigations = new HashSet<TblPriceMasterName>();
            TblPriceMasterNameModifiedByNavigations = new HashSet<TblPriceMasterName>();
            TblPriceMasterQuestionerCreatedByNavigations = new HashSet<TblPriceMasterQuestioner>();
            TblPriceMasterQuestionerModifiedByNavigations = new HashSet<TblPriceMasterQuestioner>();
            TblProdCatBrandMappingCreatedbyNavigations = new HashSet<TblProdCatBrandMapping>();
            TblProdCatBrandMappingModifiedByNavigations = new HashSet<TblProdCatBrandMapping>();
            TblProductTechnologyCreatedByNavigations = new HashSet<TblProductTechnology>();
            TblProductTechnologyModifiedByNavigations = new HashSet<TblProductTechnology>();
            TblPushNotificationMessageDetailCreatedByNavigations = new HashSet<TblPushNotificationMessageDetail>();
            TblPushNotificationMessageDetailModifiedByNavigations = new HashSet<TblPushNotificationMessageDetail>();
            TblPushNotificationSavedDetailCreatedByNavigations = new HashSet<TblPushNotificationSavedDetail>();
            TblPushNotificationSavedDetailModifiedByNavigations = new HashSet<TblPushNotificationSavedDetail>();
            TblQcratingMasterCreatedByNavigations = new HashSet<TblQcratingMaster>();
            TblQcratingMasterModifiedByNavigations = new HashSet<TblQcratingMaster>();
            TblQuestionerLovCreatedByNavigations = new HashSet<TblQuestionerLov>();
            TblQuestionerLovModifiedByNavigations = new HashSet<TblQuestionerLov>();
            TblQuestionerLovmappingCreatedByNavigations = new HashSet<TblQuestionerLovmapping>();
            TblQuestionerLovmappingModifiedByNavigations = new HashSet<TblQuestionerLovmapping>();
            TblQuestionsForSweetnerCreatedByNavigations = new HashSet<TblQuestionsForSweetner>();
            TblQuestionsForSweetnerModifiedByNavigations = new HashSet<TblQuestionsForSweetner>();
            TblRefurbisherRegistrationCreatedByNavigations = new HashSet<TblRefurbisherRegistration>();
            TblRefurbisherRegistrationModifiedByNavigations = new HashSet<TblRefurbisherRegistration>();
            TblRoleAccessCreatedByNavigations = new HashSet<TblRoleAccess>();
            TblRoleAccessModifiedByNavigations = new HashSet<TblRoleAccess>();
            TblRoleCreatedByNavigations = new HashSet<TblRole>();
            TblRoleModifiedByNavigations = new HashSet<TblRole>();
            TblSelfQcs = new HashSet<TblSelfQc>();
            TblServicePartnerCreatedByNavigations = new HashSet<TblServicePartner>();
            TblServicePartnerModifiedbyNavigations = new HashSet<TblServicePartner>();
            TblServicePartnerUsers = new HashSet<TblServicePartner>();
            TblTempDatumCreatedByNavigations = new HashSet<TblTempDatum>();
            TblTempDatumModifiedbyNavigations = new HashSet<TblTempDatum>();
            TblTimeLines = new HashSet<TblTimeLine>();
            TblTimelineStatusMappings = new HashSet<TblTimelineStatusMapping>();
            TblUniversalPriceMasterCreatedByNavigations = new HashSet<TblUniversalPriceMaster>();
            TblUniversalPriceMasterModifiedByNavigations = new HashSet<TblUniversalPriceMaster>();
            TblUserMappingCreatedByNavigations = new HashSet<TblUserMapping>();
            TblUserMappingModifiedByNavigations = new HashSet<TblUserMapping>();
            TblUserMappingUsers = new HashSet<TblUserMapping>();
            TblUserRoleCreatedByNavigations = new HashSet<TblUserRole>();
            TblUserRoleModifiedByNavigations = new HashSet<TblUserRole>();
            TblUserRoleUsers = new HashSet<TblUserRole>();
            TblVehicleIncentiveCreatedByNavigations = new HashSet<TblVehicleIncentive>();
            TblVehicleIncentiveModifiedByNavigations = new HashSet<TblVehicleIncentive>();
            TblVehicleJourneyTrackingCreatedByNavigations = new HashSet<TblVehicleJourneyTracking>();
            TblVehicleJourneyTrackingDetailCreatedByNavigations = new HashSet<TblVehicleJourneyTrackingDetail>();
            TblVehicleJourneyTrackingDetailModifiedByNavigations = new HashSet<TblVehicleJourneyTrackingDetail>();
            TblVehicleJourneyTrackingModifiedByNavigations = new HashSet<TblVehicleJourneyTracking>();
            TblVehicleListCreatedByNavigations = new HashSet<TblVehicleList>();
            TblVehicleListModifiedByNavigations = new HashSet<TblVehicleList>();
            TblWalletTransactionCreatedByNavigations = new HashSet<TblWalletTransaction>();
            TblWalletTransactionModifiedByNavigations = new HashSet<TblWalletTransaction>();
            UniversalPriceMasterCreatedByNavigations = new HashSet<UniversalPriceMaster>();
            UniversalPriceMasterModifiedByNavigations = new HashSet<UniversalPriceMaster>();
        }

        public int UserId { get; set; }
        public string? ZohoUserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Phone { get; set; }
        public string? Gender { get; set; }
        public string? ImageName { get; set; }
        public string? UserStatus { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? LastLogin { get; set; }
        public int? CompanyId { get; set; }

        public virtual TblCompany? Company { get; set; }
        public virtual TblUser? CreatedByNavigation { get; set; }
        public virtual TblUser? ModifiedByNavigation { get; set; }
        public virtual ICollection<TblUser> InverseCreatedByNavigation { get; set; }
        public virtual ICollection<TblUser> InverseModifiedByNavigation { get; set; }
        public virtual ICollection<MapLoginUserDevice> MapLoginUserDeviceCreatedByNavigations { get; set; }
        public virtual ICollection<MapLoginUserDevice> MapLoginUserDeviceModifiedByNavigations { get; set; }
        public virtual ICollection<MapLoginUserDevice> MapLoginUserDeviceUsers { get; set; }
        public virtual ICollection<MapServicePartnerCityState> MapServicePartnerCityStateCreatedByNavigations { get; set; }
        public virtual ICollection<MapServicePartnerCityState> MapServicePartnerCityStateModifiedByNavigations { get; set; }
        public virtual ICollection<PriceMasterName> PriceMasterNameCreatedByNavigations { get; set; }
        public virtual ICollection<PriceMasterName> PriceMasterNameModifiedByNavigations { get; set; }
        public virtual ICollection<TblAbbredemption> TblAbbredemptionCreatedByNavigations { get; set; }
        public virtual ICollection<TblAbbredemption> TblAbbredemptionModifiedByNavigations { get; set; }
        public virtual ICollection<TblAccessList> TblAccessListCreatedByNavigations { get; set; }
        public virtual ICollection<TblAccessList> TblAccessListModifiedByNavigations { get; set; }
        public virtual ICollection<TblAddress> TblAddressCreatedByNavigations { get; set; }
        public virtual ICollection<TblAddress> TblAddressModifiedByNavigations { get; set; }
        public virtual ICollection<TblAddress> TblAddressUsers { get; set; }
        public virtual ICollection<TblApicall> TblApicallCreatedByNavigations { get; set; }
        public virtual ICollection<TblApicall> TblApicallModifiedByNavigations { get; set; }
        public virtual ICollection<TblBppincodeMapping> TblBppincodeMappingCreatedByNavigations { get; set; }
        public virtual ICollection<TblBppincodeMapping> TblBppincodeMappingModifiedByNavigations { get; set; }
        public virtual ICollection<TblBrandGroup> TblBrandGroupCreatedbyNavigations { get; set; }
        public virtual ICollection<TblBrandGroup> TblBrandGroupModifiedByNavigations { get; set; }
        public virtual ICollection<TblBubasedSweetnerValidation> TblBubasedSweetnerValidationCreatedByNavigations { get; set; }
        public virtual ICollection<TblBubasedSweetnerValidation> TblBubasedSweetnerValidationModifiedByNavigations { get; set; }
        public virtual ICollection<TblBuconfiguration> TblBuconfigurationCreatedByNavigations { get; set; }
        public virtual ICollection<TblBuconfigurationMapping> TblBuconfigurationMappingCreatedByNavigations { get; set; }
        public virtual ICollection<TblBuconfigurationMapping> TblBuconfigurationMappingModifiedByNavigations { get; set; }
        public virtual ICollection<TblBuconfiguration> TblBuconfigurationModifiedByNavigations { get; set; }
        public virtual ICollection<TblBuproductCategoryMapping> TblBuproductCategoryMappingCreatedByNavigations { get; set; }
        public virtual ICollection<TblBuproductCategoryMapping> TblBuproductCategoryMappingModifiedByNavigations { get; set; }
        public virtual ICollection<TblCompany> TblCompanyCreatedByNavigations { get; set; }
        public virtual ICollection<TblCompany> TblCompanyModifiedByNavigations { get; set; }
        public virtual ICollection<TblCreditRequest> TblCreditRequestCreatedByNavigations { get; set; }
        public virtual ICollection<TblCreditRequest> TblCreditRequestCreditRequestApproveUsers { get; set; }
        public virtual ICollection<TblCreditRequest> TblCreditRequestCreditRequestUsers { get; set; }
        public virtual ICollection<TblCreditRequest> TblCreditRequestModifiedByNavigations { get; set; }
        public virtual ICollection<TblCustomerFile> TblCustomerFileCreatedByNavigations { get; set; }
        public virtual ICollection<TblCustomerFile> TblCustomerFileModifiedByNavigations { get; set; }
        public virtual ICollection<TblDriverDetail> TblDriverDetails { get; set; }
        public virtual ICollection<TblDriverList> TblDriverListCreatedByNavigations { get; set; }
        public virtual ICollection<TblDriverList> TblDriverListModifiedByNavigations { get; set; }
        public virtual ICollection<TblDriverList> TblDriverListUsers { get; set; }
        public virtual ICollection<TblEntityType> TblEntityTypeCreatedByNavigations { get; set; }
        public virtual ICollection<TblEntityType> TblEntityTypeModifiedByNavigations { get; set; }
        public virtual ICollection<TblEvcPartner> TblEvcPartnerCreatedbyNavigations { get; set; }
        public virtual ICollection<TblEvcPartner> TblEvcPartnerModifiedByNavigations { get; set; }
        public virtual ICollection<TblEvcPartnerPreference> TblEvcPartnerPreferenceCreatedbyNavigations { get; set; }
        public virtual ICollection<TblEvcPartnerPreference> TblEvcPartnerPreferenceModifiedByNavigations { get; set; }
        public virtual ICollection<TblEvcdispute> TblEvcdisputeCreatedByNavigations { get; set; }
        public virtual ICollection<TblEvcdispute> TblEvcdisputeModifiedByNavigations { get; set; }
        public virtual ICollection<TblEvcpoddetail> TblEvcpoddetailCreatedByNavigations { get; set; }
        public virtual ICollection<TblEvcpoddetail> TblEvcpoddetailModifiedByNavigations { get; set; }
        public virtual ICollection<TblEvcregistration> TblEvcregistrationCreatedByNavigations { get; set; }
        public virtual ICollection<TblEvcregistration> TblEvcregistrationModifiedByNavigations { get; set; }
        public virtual ICollection<TblEvcwalletAddition> TblEvcwalletAdditionCreatedByNavigations { get; set; }
        public virtual ICollection<TblEvcwalletAddition> TblEvcwalletAdditionModifiedByNavigations { get; set; }
        public virtual ICollection<TblEvcwalletHistory> TblEvcwalletHistoryCreatedByNavigations { get; set; }
        public virtual ICollection<TblEvcwalletHistory> TblEvcwalletHistoryModifiedByNavigations { get; set; }
        public virtual ICollection<TblExchangeAbbstatusHistory> TblExchangeAbbstatusHistoryCreatedByNavigations { get; set; }
        public virtual ICollection<TblExchangeAbbstatusHistory> TblExchangeAbbstatusHistoryModifiedByNavigations { get; set; }
        public virtual ICollection<TblExchangeOrder> TblExchangeOrderCreatedByNavigations { get; set; }
        public virtual ICollection<TblExchangeOrder> TblExchangeOrderModifiedByNavigations { get; set; }
        public virtual ICollection<TblImageLabelMaster> TblImageLabelMasterCreatedByNavigations { get; set; }
        public virtual ICollection<TblImageLabelMaster> TblImageLabelMasterModifiedbyNavigations { get; set; }
        public virtual ICollection<TblLoginMobile> TblLoginMobiles { get; set; }
        public virtual ICollection<TblLogistic> TblLogisticCreatedByNavigations { get; set; }
        public virtual ICollection<TblLogistic> TblLogisticModifiedbyNavigations { get; set; }
        public virtual ICollection<TblNpssqoption> TblNpssqoptionCreatedByNavigations { get; set; }
        public virtual ICollection<TblNpssqoption> TblNpssqoptionModifiedByNavigations { get; set; }
        public virtual ICollection<TblNpssqresponse> TblNpssqresponseCreatedByNavigations { get; set; }
        public virtual ICollection<TblNpssqresponse> TblNpssqresponseModifiedByNavigations { get; set; }
        public virtual ICollection<TblNpssquestion> TblNpssquestionCreatedByNavigations { get; set; }
        public virtual ICollection<TblNpssquestion> TblNpssquestionModifiedByNavigations { get; set; }
        public virtual ICollection<TblOrderImageUpload> TblOrderImageUploadCreatedByNavigations { get; set; }
        public virtual ICollection<TblOrderImageUpload> TblOrderImageUploadModifiedbyNavigations { get; set; }
        public virtual ICollection<TblOrderLgc> TblOrderLgcCreatedByNavigations { get; set; }
        public virtual ICollection<TblOrderLgc> TblOrderLgcModifiedByNavigations { get; set; }
        public virtual ICollection<TblOrderQcrating> TblOrderQcratingCreatedByNavigations { get; set; }
        public virtual ICollection<TblOrderQcrating> TblOrderQcratingModifiedByNavigations { get; set; }
        public virtual ICollection<TblOrderQc> TblOrderQcs { get; set; }
        public virtual ICollection<TblOrderTran> TblOrderTranAssignByNavigations { get; set; }
        public virtual ICollection<TblOrderTran> TblOrderTranAssignToNavigations { get; set; }
        public virtual ICollection<TblOrderTran> TblOrderTranCreatedByNavigations { get; set; }
        public virtual ICollection<TblOrderTran> TblOrderTranModifiedByNavigations { get; set; }
        public virtual ICollection<TblOrderTran> TblOrderTranSelfQclinkResendbyNavigations { get; set; }
        public virtual ICollection<TblPriceMasterMapping> TblPriceMasterMappingCreatedByNavigations { get; set; }
        public virtual ICollection<TblPriceMasterMapping> TblPriceMasterMappingModifiedByNavigations { get; set; }
        public virtual ICollection<TblPriceMasterName> TblPriceMasterNameCreatedByNavigations { get; set; }
        public virtual ICollection<TblPriceMasterName> TblPriceMasterNameModifiedByNavigations { get; set; }
        public virtual ICollection<TblPriceMasterQuestioner> TblPriceMasterQuestionerCreatedByNavigations { get; set; }
        public virtual ICollection<TblPriceMasterQuestioner> TblPriceMasterQuestionerModifiedByNavigations { get; set; }
        public virtual ICollection<TblProdCatBrandMapping> TblProdCatBrandMappingCreatedbyNavigations { get; set; }
        public virtual ICollection<TblProdCatBrandMapping> TblProdCatBrandMappingModifiedByNavigations { get; set; }
        public virtual ICollection<TblProductTechnology> TblProductTechnologyCreatedByNavigations { get; set; }
        public virtual ICollection<TblProductTechnology> TblProductTechnologyModifiedByNavigations { get; set; }
        public virtual ICollection<TblPushNotificationMessageDetail> TblPushNotificationMessageDetailCreatedByNavigations { get; set; }
        public virtual ICollection<TblPushNotificationMessageDetail> TblPushNotificationMessageDetailModifiedByNavigations { get; set; }
        public virtual ICollection<TblPushNotificationSavedDetail> TblPushNotificationSavedDetailCreatedByNavigations { get; set; }
        public virtual ICollection<TblPushNotificationSavedDetail> TblPushNotificationSavedDetailModifiedByNavigations { get; set; }
        public virtual ICollection<TblQcratingMaster> TblQcratingMasterCreatedByNavigations { get; set; }
        public virtual ICollection<TblQcratingMaster> TblQcratingMasterModifiedByNavigations { get; set; }
        public virtual ICollection<TblQuestionerLov> TblQuestionerLovCreatedByNavigations { get; set; }
        public virtual ICollection<TblQuestionerLov> TblQuestionerLovModifiedByNavigations { get; set; }
        public virtual ICollection<TblQuestionerLovmapping> TblQuestionerLovmappingCreatedByNavigations { get; set; }
        public virtual ICollection<TblQuestionerLovmapping> TblQuestionerLovmappingModifiedByNavigations { get; set; }
        public virtual ICollection<TblQuestionsForSweetner> TblQuestionsForSweetnerCreatedByNavigations { get; set; }
        public virtual ICollection<TblQuestionsForSweetner> TblQuestionsForSweetnerModifiedByNavigations { get; set; }
        public virtual ICollection<TblRefurbisherRegistration> TblRefurbisherRegistrationCreatedByNavigations { get; set; }
        public virtual ICollection<TblRefurbisherRegistration> TblRefurbisherRegistrationModifiedByNavigations { get; set; }
        public virtual ICollection<TblRoleAccess> TblRoleAccessCreatedByNavigations { get; set; }
        public virtual ICollection<TblRoleAccess> TblRoleAccessModifiedByNavigations { get; set; }
        public virtual ICollection<TblRole> TblRoleCreatedByNavigations { get; set; }
        public virtual ICollection<TblRole> TblRoleModifiedByNavigations { get; set; }
        public virtual ICollection<TblSelfQc> TblSelfQcs { get; set; }
        public virtual ICollection<TblServicePartner> TblServicePartnerCreatedByNavigations { get; set; }
        public virtual ICollection<TblServicePartner> TblServicePartnerModifiedbyNavigations { get; set; }
        public virtual ICollection<TblServicePartner> TblServicePartnerUsers { get; set; }
        public virtual ICollection<TblTempDatum> TblTempDatumCreatedByNavigations { get; set; }
        public virtual ICollection<TblTempDatum> TblTempDatumModifiedbyNavigations { get; set; }
        public virtual ICollection<TblTimeLine> TblTimeLines { get; set; }
        public virtual ICollection<TblTimelineStatusMapping> TblTimelineStatusMappings { get; set; }
        public virtual ICollection<TblUniversalPriceMaster> TblUniversalPriceMasterCreatedByNavigations { get; set; }
        public virtual ICollection<TblUniversalPriceMaster> TblUniversalPriceMasterModifiedByNavigations { get; set; }
        public virtual ICollection<TblUserMapping> TblUserMappingCreatedByNavigations { get; set; }
        public virtual ICollection<TblUserMapping> TblUserMappingModifiedByNavigations { get; set; }
        public virtual ICollection<TblUserMapping> TblUserMappingUsers { get; set; }
        public virtual ICollection<TblUserRole> TblUserRoleCreatedByNavigations { get; set; }
        public virtual ICollection<TblUserRole> TblUserRoleModifiedByNavigations { get; set; }
        public virtual ICollection<TblUserRole> TblUserRoleUsers { get; set; }
        public virtual ICollection<TblVehicleIncentive> TblVehicleIncentiveCreatedByNavigations { get; set; }
        public virtual ICollection<TblVehicleIncentive> TblVehicleIncentiveModifiedByNavigations { get; set; }
        public virtual ICollection<TblVehicleJourneyTracking> TblVehicleJourneyTrackingCreatedByNavigations { get; set; }
        public virtual ICollection<TblVehicleJourneyTrackingDetail> TblVehicleJourneyTrackingDetailCreatedByNavigations { get; set; }
        public virtual ICollection<TblVehicleJourneyTrackingDetail> TblVehicleJourneyTrackingDetailModifiedByNavigations { get; set; }
        public virtual ICollection<TblVehicleJourneyTracking> TblVehicleJourneyTrackingModifiedByNavigations { get; set; }
        public virtual ICollection<TblVehicleList> TblVehicleListCreatedByNavigations { get; set; }
        public virtual ICollection<TblVehicleList> TblVehicleListModifiedByNavigations { get; set; }
        public virtual ICollection<TblWalletTransaction> TblWalletTransactionCreatedByNavigations { get; set; }
        public virtual ICollection<TblWalletTransaction> TblWalletTransactionModifiedByNavigations { get; set; }
        public virtual ICollection<UniversalPriceMaster> UniversalPriceMasterCreatedByNavigations { get; set; }
        public virtual ICollection<UniversalPriceMaster> UniversalPriceMasterModifiedByNavigations { get; set; }
    }
}
