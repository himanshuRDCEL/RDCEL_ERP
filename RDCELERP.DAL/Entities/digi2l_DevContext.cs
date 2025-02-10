using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace RDCELERP.DAL.Entities
{
    public partial class Digi2l_DevContext : DbContext
    {
        public Digi2l_DevContext()
        {
        }

        public Digi2l_DevContext(DbContextOptions<Digi2l_DevContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AllCallAllocation> AllCallAllocations { get; set; } = null!;
        public virtual DbSet<AllSponsorsDetailsFromZoho> AllSponsorsDetailsFromZohos { get; set; } = null!;
        public virtual DbSet<EvcallCallAllocationsDatum> EvcallCallAllocationsData { get; set; } = null!;
        public virtual DbSet<Login> Logins { get; set; } = null!;
        public virtual DbSet<MapLoginUserDevice> MapLoginUserDevices { get; set; } = null!;
        public virtual DbSet<MapServicePartnerCityState> MapServicePartnerCityStates { get; set; } = null!;
        public virtual DbSet<Mapping> Mappings { get; set; } = null!;
        public virtual DbSet<PriceMasterName> PriceMasterNames { get; set; } = null!;
        public virtual DbSet<Tbl247Around> Tbl247Arounds { get; set; } = null!;
        public virtual DbSet<TblAbbplanMaster> TblAbbplanMasters { get; set; } = null!;
        public virtual DbSet<TblAbbpriceMaster> TblAbbpriceMasters { get; set; } = null!;
        public virtual DbSet<TblAbbredemption> TblAbbredemptions { get; set; } = null!;
        public virtual DbSet<TblAbbregistration> TblAbbregistrations { get; set; } = null!;
        public virtual DbSet<TblAccessList> TblAccessLists { get; set; } = null!;
        public virtual DbSet<TblAddress> TblAddresses { get; set; } = null!;
        public virtual DbSet<TblApicall> TblApicalls { get; set; } = null!;
        public virtual DbSet<TblAreaLocality> TblAreaLocalities { get; set; } = null!;
        public virtual DbSet<TblBizlogTicket> TblBizlogTickets { get; set; } = null!;
        public virtual DbSet<TblBizlogTicketStatus> TblBizlogTicketStatuses { get; set; } = null!;
        public virtual DbSet<TblBlowHornTicket> TblBlowHornTickets { get; set; } = null!;
        public virtual DbSet<TblBpbuassociation> TblBpbuassociations { get; set; } = null!;
        public virtual DbSet<TblBpburedemptionMapping> TblBpburedemptionMappings { get; set; } = null!;
        public virtual DbSet<TblBppincodeMapping> TblBppincodeMappings { get; set; } = null!;
        public virtual DbSet<TblBrand> TblBrands { get; set; } = null!;
        public virtual DbSet<TblBrandGroup> TblBrandGroups { get; set; } = null!;
        public virtual DbSet<TblBrandSmartBuy> TblBrandSmartBuys { get; set; } = null!;
        public virtual DbSet<TblBubasedSweetnerValidation> TblBubasedSweetnerValidations { get; set; } = null!;
        public virtual DbSet<TblBuconfiguration> TblBuconfigurations { get; set; } = null!;
        public virtual DbSet<TblBuconfigurationMapping> TblBuconfigurationMappings { get; set; } = null!;
        public virtual DbSet<TblBuproductCategoryMapping> TblBuproductCategoryMappings { get; set; } = null!;
        public virtual DbSet<TblBusinessPartner> TblBusinessPartners { get; set; } = null!;
        public virtual DbSet<TblBusinessUnit> TblBusinessUnits { get; set; } = null!;
        public virtual DbSet<TblCity> TblCities { get; set; } = null!;
        public virtual DbSet<TblCompany> TblCompanies { get; set; } = null!;
        public virtual DbSet<TblConfiguration> TblConfigurations { get; set; } = null!;
        public virtual DbSet<TblCreditRequest> TblCreditRequests { get; set; } = null!;
        public virtual DbSet<TblCurrentAuthtoken> TblCurrentAuthtokens { get; set; } = null!;
        public virtual DbSet<TblCustomerDetail> TblCustomerDetails { get; set; } = null!;
        public virtual DbSet<TblCustomerFile> TblCustomerFiles { get; set; } = null!;
        public virtual DbSet<TblDriverDetail> TblDriverDetails { get; set; } = null!;
        public virtual DbSet<TblDriverList> TblDriverLists { get; set; } = null!;
        public virtual DbSet<TblEntityType> TblEntityTypes { get; set; } = null!;
        public virtual DbSet<TblErrorLog> TblErrorLogs { get; set; } = null!;
        public virtual DbSet<TblEvcPartner> TblEvcPartners { get; set; } = null!;
        public virtual DbSet<TblEvcPartnerPreference> TblEvcPartnerPreferences { get; set; } = null!;
        public virtual DbSet<TblEvcPriceMaster> TblEvcPriceMasters { get; set; } = null!;
        public virtual DbSet<TblEvcapproved> TblEvcapproveds { get; set; } = null!;
        public virtual DbSet<TblEvcdispute> TblEvcdisputes { get; set; } = null!;
        public virtual DbSet<TblEvcpoddetail> TblEvcpoddetails { get; set; } = null!;
        public virtual DbSet<TblEvcpriceRangeMaster> TblEvcpriceRangeMasters { get; set; } = null!;
        public virtual DbSet<TblEvcregistration> TblEvcregistrations { get; set; } = null!;
        public virtual DbSet<TblEvcwalletAddition> TblEvcwalletAdditions { get; set; } = null!;
        public virtual DbSet<TblEvcwalletHistory> TblEvcwalletHistories { get; set; } = null!;
        public virtual DbSet<TblEvcwalletStatus> TblEvcwalletStatuses { get; set; } = null!;
        public virtual DbSet<TblExchangeAbbstatusHistory> TblExchangeAbbstatusHistories { get; set; } = null!;
        public virtual DbSet<TblExchangeOrder> TblExchangeOrders { get; set; } = null!;
        public virtual DbSet<TblExchangeOrderStatus> TblExchangeOrderStatuses { get; set; } = null!;
        public virtual DbSet<TblFeedBack> TblFeedBacks { get; set; } = null!;
        public virtual DbSet<TblFeedBackAnswer> TblFeedBackAnswers { get; set; } = null!;
        public virtual DbSet<TblFeedBackQuestion> TblFeedBackQuestions { get; set; } = null!;
        public virtual DbSet<TblHistory> TblHistories { get; set; } = null!;
        public virtual DbSet<TblImage> TblImages { get; set; } = null!;
        public virtual DbSet<TblImageLabelMaster> TblImageLabelMasters { get; set; } = null!;
        public virtual DbSet<TblLoV> TblLoVs { get; set; } = null!;
        public virtual DbSet<TblLoginMobile> TblLoginMobiles { get; set; } = null!;
        public virtual DbSet<TblLogistic> TblLogistics { get; set; } = null!;
        public virtual DbSet<TblMahindraLogistic> TblMahindraLogistics { get; set; } = null!;
        public virtual DbSet<TblMessageDetail> TblMessageDetails { get; set; } = null!;
        public virtual DbSet<TblModelMapping> TblModelMappings { get; set; } = null!;
        public virtual DbSet<TblModelNumber> TblModelNumbers { get; set; } = null!;
        public virtual DbSet<TblNpssqoption> TblNpssqoptions { get; set; } = null!;
        public virtual DbSet<TblNpssqresponse> TblNpssqresponses { get; set; } = null!;
        public virtual DbSet<TblNpssquestion> TblNpssquestions { get; set; } = null!;
        public virtual DbSet<TblOrderBasedConfig> TblOrderBasedConfigs { get; set; } = null!;
        public virtual DbSet<TblOrderImageUpload> TblOrderImageUploads { get; set; } = null!;
        public virtual DbSet<TblOrderLgc> TblOrderLgcs { get; set; } = null!;
        public virtual DbSet<TblOrderPromoVoucher> TblOrderPromoVouchers { get; set; } = null!;
        public virtual DbSet<TblOrderQc> TblOrderQcs { get; set; } = null!;
        public virtual DbSet<TblOrderQcrating> TblOrderQcratings { get; set; } = null!;
        public virtual DbSet<TblOrderTran> TblOrderTrans { get; set; } = null!;
        public virtual DbSet<TblPaymentLeaser> TblPaymentLeasers { get; set; } = null!;
        public virtual DbSet<TblPinCode> TblPinCodes { get; set; } = null!;
        public virtual DbSet<TblPincodeMasterDtoC> TblPincodeMasterDtoCs { get; set; } = null!;
        public virtual DbSet<TblPriceMaster> TblPriceMasters { get; set; } = null!;
        public virtual DbSet<TblPriceMasterMapping> TblPriceMasterMappings { get; set; } = null!;
        public virtual DbSet<TblPriceMasterName> TblPriceMasterNames { get; set; } = null!;
        public virtual DbSet<TblPriceMasterQuestioner> TblPriceMasterQuestioners { get; set; } = null!;
        public virtual DbSet<TblProdCatBrandMapping> TblProdCatBrandMappings { get; set; } = null!;
        public virtual DbSet<TblProductCategory> TblProductCategories { get; set; } = null!;
        public virtual DbSet<TblProductConditionLabel> TblProductConditionLabels { get; set; } = null!;
        public virtual DbSet<TblProductQualityIndex> TblProductQualityIndices { get; set; } = null!;
        public virtual DbSet<TblProductTechnology> TblProductTechnologies { get; set; } = null!;
        public virtual DbSet<TblProductType> TblProductTypes { get; set; } = null!;
        public virtual DbSet<TblPromotionalVoucherMaster> TblPromotionalVoucherMasters { get; set; } = null!;
        public virtual DbSet<TblPushNotificationMessageDetail> TblPushNotificationMessageDetails { get; set; } = null!;
        public virtual DbSet<TblPushNotificationSavedDetail> TblPushNotificationSavedDetails { get; set; } = null!;
        public virtual DbSet<TblQcratingMaster> TblQcratingMasters { get; set; } = null!;
        public virtual DbSet<TblQcratingMasterMapping> TblQcratingMasterMappings { get; set; } = null!;
        public virtual DbSet<TblQuestionerLov> TblQuestionerLovs { get; set; } = null!;
        public virtual DbSet<TblQuestionerLovmapping> TblQuestionerLovmappings { get; set; } = null!;
        public virtual DbSet<TblQuestionsForSweetner> TblQuestionsForSweetners { get; set; } = null!;
        public virtual DbSet<TblRefurbisherRegistration> TblRefurbisherRegistrations { get; set; } = null!;
        public virtual DbSet<TblRole> TblRoles { get; set; } = null!;
        public virtual DbSet<TblRoleAccess> TblRoleAccesses { get; set; } = null!;
        public virtual DbSet<TblSelfQc> TblSelfQcs { get; set; } = null!;
        public virtual DbSet<TblServicePartner> TblServicePartners { get; set; } = null!;
        public virtual DbSet<TblSociety> TblSocieties { get; set; } = null!;
        public virtual DbSet<TblSponsorCategoryMapping> TblSponsorCategoryMappings { get; set; } = null!;
        public virtual DbSet<TblState> TblStates { get; set; } = null!;
        public virtual DbSet<TblTempDatum> TblTempData { get; set; } = null!;
        public virtual DbSet<TblTimeLine> TblTimeLines { get; set; } = null!;
        public virtual DbSet<TblTimelineStatusMapping> TblTimelineStatusMappings { get; set; } = null!;
        public virtual DbSet<TblTransMasterAbbplanMaster> TblTransMasterAbbplanMasters { get; set; } = null!;
        public virtual DbSet<TblUnInstallationPriceMaster> TblUnInstallationPriceMasters { get; set; } = null!;
        public virtual DbSet<TblUninstallationPrice> TblUninstallationPrices { get; set; } = null!;
        public virtual DbSet<TblUniversalPriceMaster> TblUniversalPriceMasters { get; set; } = null!;
        public virtual DbSet<TblUpiidUpdatelog> TblUpiidUpdatelogs { get; set; } = null!;
        public virtual DbSet<TblUser> TblUsers { get; set; } = null!;
        public virtual DbSet<TblUserMapping> TblUserMappings { get; set; } = null!;
        public virtual DbSet<TblUserRole> TblUserRoles { get; set; } = null!;
        public virtual DbSet<TblVcareService> TblVcareServices { get; set; } = null!;
        public virtual DbSet<TblVehicleIncentive> TblVehicleIncentives { get; set; } = null!;
        public virtual DbSet<TblVehicleJourneyTracking> TblVehicleJourneyTrackings { get; set; } = null!;
        public virtual DbSet<TblVehicleJourneyTrackingDetail> TblVehicleJourneyTrackingDetails { get; set; } = null!;
        public virtual DbSet<TblVehicleList> TblVehicleLists { get; set; } = null!;
        public virtual DbSet<TblVoucherStatus> TblVoucherStatuses { get; set; } = null!;
        public virtual DbSet<TblVoucherTermsAndCondition> TblVoucherTermsAndConditions { get; set; } = null!;
        public virtual DbSet<TblVoucherVerfication> TblVoucherVerfications { get; set; } = null!;
        public virtual DbSet<TblWalletTransaction> TblWalletTransactions { get; set; } = null!;
        public virtual DbSet<TblWhatsAppMessage> TblWhatsAppMessages { get; set; } = null!;
        public virtual DbSet<TimeSlotMaster> TimeSlotMasters { get; set; } = null!;
        public virtual DbSet<UniversalPriceMaster> UniversalPriceMasters { get; set; } = null!;
        public virtual DbSet<ViewAbb> ViewAbbs { get; set; } = null!;
        public virtual DbSet<ViewAbbCount> ViewAbbCounts { get; set; } = null!;
        public virtual DbSet<ViewAllExchangeDataForPinelab> ViewAllExchangeDataForPinelabs { get; set; } = null!;
        public virtual DbSet<ViewAllExchangeDatum> ViewAllExchangeData { get; set; } = null!;
        public virtual DbSet<ZohoExchangeDatum> ZohoExchangeData { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=103.127.146.29, 1433;Initial Catalog=Digi2l_Dev;Persist Security Info=False;User ID=prod_user;Password=D!g!2L@UTC2023;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AllCallAllocation>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("All_Call_Allocations$");

                entity.Property(e => e.ActualEvcAmountAsPerQc).HasColumnName("Actual EVC Amount As Per QC");

                entity.Property(e => e.ActualProdQltyAtTimeOfQc)
                    .HasMaxLength(255)
                    .HasColumnName("Actual Prod Qlty (at time of QC)");

                entity.Property(e => e.AddressLine1)
                    .HasMaxLength(255)
                    .HasColumnName("Address Line 1");

                entity.Property(e => e.AddressLine2)
                    .HasMaxLength(255)
                    .HasColumnName("Address Line 2");

                entity.Property(e => e.AllocationStatus)
                    .HasMaxLength(255)
                    .HasColumnName("Allocation Status");

                entity.Property(e => e.City).HasMaxLength(255);

                entity.Property(e => e.CustCity)
                    .HasMaxLength(255)
                    .HasColumnName("Cust City");

                entity.Property(e => e.CustPinCode).HasColumnName("Cust PIN Code");

                entity.Property(e => e.EvcAddress)
                    .HasMaxLength(255)
                    .HasColumnName("EVC Address");

                entity.Property(e => e.EvcBusinessName)
                    .HasMaxLength(255)
                    .HasColumnName("EVC BusinessName");

                entity.Property(e => e.EvcCode)
                    .HasMaxLength(255)
                    .HasColumnName("EVC Code");

                entity.Property(e => e.InvoiceDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Invoice Date");

                entity.Property(e => e.InvoiceNo)
                    .HasMaxLength(255)
                    .HasColumnName("Invoice No");

                entity.Property(e => e.ProdGroup)
                    .HasMaxLength(255)
                    .HasColumnName("Prod Group");

                entity.Property(e => e.ProofOfDelivery)
                    .HasMaxLength(255)
                    .HasColumnName("Proof Of Delivery");

                entity.Property(e => e.RNo)
                    .HasMaxLength(255)
                    .HasColumnName("R# No#");

                entity.Property(e => e.State).HasMaxLength(255);

                entity.Property(e => e.Type).HasMaxLength(255);
            });

            modelBuilder.Entity<AllSponsorsDetailsFromZoho>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("All_Sponsors_DetailsFromZoho");

                entity.Property(e => e.Abb).HasColumnName("ABB ?");

                entity.Property(e => e.AbbFees).HasColumnName("ABB Fees");

                entity.Property(e => e.AbbPlanName).HasColumnName("ABB Plan Name");

                entity.Property(e => e.AbbPlanPeriodMonths).HasColumnName("ABB Plan Period (Months)");

                entity.Property(e => e.AbbPriceId).HasColumnName("ABB Price ID");

                entity.Property(e => e.ActualAge).HasColumnName("Actual Age");

                entity.Property(e => e.ActualAmountPaid).HasColumnName("Actual Amount Paid");

                entity.Property(e => e.ActualAmtPayableInclBonus).HasColumnName("Actual Amt payable (incl Bonus)");

                entity.Property(e => e.ActualBaseAmountAsPerQc).HasColumnName("Actual Base Amount As per (QC)");

                entity.Property(e => e.ActualBrand).HasColumnName("Actual Brand");

                entity.Property(e => e.ActualEvcAmountAsPerQc).HasColumnName("Actual EVC Amount As Per QC");

                entity.Property(e => e.ActualPickupDate).HasColumnName("Actual Pickup Date");

                entity.Property(e => e.ActualProdQltyAtTimeOfQc).HasColumnName("Actual Prod Qlty (at time of QC)");

                entity.Property(e => e.ActualSize).HasColumnName("Actual Size");

                entity.Property(e => e.ActualTotalAmountAsPerQc).HasColumnName("Actual (Total) Amount as per QC");

                entity.Property(e => e.ActualType).HasColumnName("Actual Type");

                entity.Property(e => e.AddedTime).HasColumnName("Added Time");

                entity.Property(e => e.AmountPayableThroughLgc).HasColumnName("Amount Payable Through  LGC");

                entity.Property(e => e.AssociateCode).HasColumnName("Associate Code");

                entity.Property(e => e.AssociateEmail).HasColumnName("Associate Email");

                entity.Property(e => e.AssociateName).HasColumnName("Associate Name");

                entity.Property(e => e.BankReference).HasColumnName("Bank Reference ");

                entity.Property(e => e.BaseExchValueP).HasColumnName("Base Exch Value (P)");

                entity.Property(e => e.BaseExchValueQ).HasColumnName("Base Exch Value (Q)");

                entity.Property(e => e.BaseExchValueR).HasColumnName("Base Exch Value (R)");

                entity.Property(e => e.BaseExchValueS).HasColumnName("Base Exch Value (S)");

                entity.Property(e => e.CompressorNoOduSrNo).HasColumnName("Compressor No/ODU Sr No");

                entity.Property(e => e.Cust1stName).HasColumnName("Cust 1st Name");

                entity.Property(e => e.CustAdd1).HasColumnName("Cust Add 1");

                entity.Property(e => e.CustAdd2).HasColumnName("Cust Add 2");

                entity.Property(e => e.CustCity).HasColumnName("Cust City");

                entity.Property(e => e.CustDeclaration).HasColumnName("Cust Declaration");

                entity.Property(e => e.CustDeclaredQlty).HasColumnName("Cust Declared Qlty");

                entity.Property(e => e.CustEMail).HasColumnName("Cust E-mail");

                entity.Property(e => e.CustMobile).HasColumnName("Cust Mobile");

                entity.Property(e => e.CustName).HasColumnName("Cust Name");

                entity.Property(e => e.CustOkForPrice).HasColumnName("Cust OK for Price");

                entity.Property(e => e.CustPinCode).HasColumnName("Cust Pin Code");

                entity.Property(e => e.CustState).HasColumnName("Cust State");

                entity.Property(e => e.CustomerDeclaredAge).HasColumnName("Customer Declared Age");

                entity.Property(e => e.DatePaid).HasColumnName("Date paid");

                entity.Property(e => e.EMailSmsDate).HasColumnName("E-Mail/SMS Date");

                entity.Property(e => e.EMailSmsFlag).HasColumnName("E-Mail/ SMS Flag");

                entity.Property(e => e.EstimateDeliveryDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Estimate Delivery Date ");

                entity.Property(e => e.EvcAcknowledge).HasColumnName("EVC Acknowledge");

                entity.Property(e => e.EvcDropDate).HasColumnName("EVC Drop Date");

                entity.Property(e => e.EvcDropFlag).HasColumnName("EVC Drop Flag");

                entity.Property(e => e.EvcP).HasColumnName("EVC P");

                entity.Property(e => e.EvcQ).HasColumnName("EVC Q");

                entity.Property(e => e.EvcR).HasColumnName("EVC R");

                entity.Property(e => e.EvcS).HasColumnName("EVC S");

                entity.Property(e => e.EvcStatus).HasColumnName("EVC Status");

                entity.Property(e => e.ExchPriceId).HasColumnName("Exch# Price ID");

                entity.Property(e => e.ExchProdGroup).HasColumnName("Exch# Prod Group");

                entity.Property(e => e.Exchange).HasColumnName("Exchange ?");

                entity.Property(e => e.ExchangeStatus).HasColumnName("Exchange Status");

                entity.Property(e => e.ExpectedPickupDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Expected Pickup Date");

                entity.Property(e => e.FirstAttemptDate).HasColumnName("First Attempt Date");

                entity.Property(e => e.HsnCodeForAbbFees).HasColumnName("HSN Code (For ABB Fees)");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.InstallationDate).HasColumnName("Installation Date");

                entity.Property(e => e.InstallationFlag).HasColumnName("Installation Flag");

                entity.Property(e => e.InvoiceDate).HasColumnName("Invoice Date");

                entity.Property(e => e.InvoiceImage).HasColumnName("Invoice Image");

                entity.Property(e => e.InvoiceNo).HasColumnName("Invoice No");

                entity.Property(e => e.InvoicePosted).HasColumnName("Invoice Posted");

                entity.Property(e => e.IsDeferred).HasColumnName("Is Deferred");

                entity.Property(e => e.IsDtoC).HasColumnName("Is DtoC");

                entity.Property(e => e.IsUnderWarranty).HasColumnName("Is Under Warranty");

                entity.Property(e => e.IsVoucherRedeemed).HasColumnName("Is Voucher Redeemed");

                entity.Property(e => e.LastName).HasColumnName("Last Name");

                entity.Property(e => e.LatestDateTime).HasColumnName("Latest Date & Time");

                entity.Property(e => e.LatestStatus).HasColumnName("Latest Status");

                entity.Property(e => e.LgcTktCreatedDate).HasColumnName("Lgc Tkt Created Date");

                entity.Property(e => e.LgcTktNo).HasColumnName("LGC Tkt No");

                entity.Property(e => e.LogisticBy).HasColumnName("Logistic By");

                entity.Property(e => e.LogisticPic1).HasColumnName("Logistic Pic 1");

                entity.Property(e => e.LogisticPic2).HasColumnName("Logistic Pic 2");

                entity.Property(e => e.LogisticPic3).HasColumnName("Logistic Pic 3");

                entity.Property(e => e.LogisticPic4).HasColumnName("Logistic Pic 4");

                entity.Property(e => e.LogisticsBonus).HasColumnName("logistics Bonus");

                entity.Property(e => e.LogisticsStatusRemark).HasColumnName("Logistics Status Remark");

                entity.Property(e => e.ModelNo).HasColumnName("Model No");

                entity.Property(e => e.NatureOfComplaint).HasColumnName("Nature of Complaint");

                entity.Property(e => e.NewBrand).HasColumnName("New Brand");

                entity.Property(e => e.NewPrice).HasColumnName("New Price");

                entity.Property(e => e.NewProdGroup).HasColumnName("New Prod# Group");

                entity.Property(e => e.NewProdType).HasColumnName("New Prod Type");

                entity.Property(e => e.NewSize).HasColumnName("New Size");

                entity.Property(e => e.OldBrand).HasColumnName("Old Brand");

                entity.Property(e => e.OldProdType).HasColumnName("Old Prod Type");

                entity.Property(e => e.OrderDate).HasColumnName("Order Date");

                entity.Property(e => e.OrderFlag).HasColumnName("Order Flag");

                entity.Property(e => e.OrderType).HasColumnName("Order Type");

                entity.Property(e => e.PaymentDate).HasColumnName("Payment Date");

                entity.Property(e => e.PaymentFlag).HasColumnName("Payment Flag");

                entity.Property(e => e.PaymentToCustomer).HasColumnName("Payment To Customer");

                entity.Property(e => e.PdfFileForQc).HasColumnName("pdf file for QC");

                entity.Property(e => e.Pic1).HasColumnName("Pic 1");

                entity.Property(e => e.Pic2).HasColumnName("Pic 2");

                entity.Property(e => e.Pic3).HasColumnName("Pic 3");

                entity.Property(e => e.Pic4).HasColumnName("Pic 4");

                entity.Property(e => e.PickupDate).HasColumnName("Pickup Date");

                entity.Property(e => e.PickupFlag).HasColumnName("Pickup Flag");

                entity.Property(e => e.PickupPriority).HasColumnName("Pickup_Priority");

                entity.Property(e => e.PostingDate).HasColumnName("Posting Date");

                entity.Property(e => e.PostingFlag).HasColumnName("Posting Flag");

                entity.Property(e => e.PreferredQcDateAndTime).HasColumnName("Preferred QC Date and Time");

                entity.Property(e => e.PreferredTimeForQc).HasColumnName("Preferred Time For QC");

                entity.Property(e => e.PriceEndDate).HasColumnName("Price End Date");

                entity.Property(e => e.PriceStartDate).HasColumnName("Price Start Date");

                entity.Property(e => e.ProdSrNo).HasColumnName("Prod# Sr# No#");

                entity.Property(e => e.ProdSrNo1).HasColumnName("Prod Sr No");

                entity.Property(e => e.ProductGroup).HasColumnName(" Product Group");

                entity.Property(e => e.ProductType).HasColumnName("Product Type");

                entity.Property(e => e.ProofOfDelivery).HasColumnName("Proof Of Delivery");

                entity.Property(e => e.PurchasedProduct).HasColumnName("Purchased Product");

                entity.Property(e => e.PurchasedProductCategory).HasColumnName("Purchased Product Category");

                entity.Property(e => e.PurchasedProductModel).HasColumnName("Purchased Product Model");

                entity.Property(e => e.QcByLgcReviseEvcCode).HasColumnName("QC by LGC: Revise EVC Code");

                entity.Property(e => e.QcComment).HasColumnName("QC Comment");

                entity.Property(e => e.QcDate).HasColumnName("QC Date");

                entity.Property(e => e.QcFlag).HasColumnName("QC Flag");

                entity.Property(e => e.ReadyForLogisticTicket).HasColumnName("Ready For Logistic Ticket");

                entity.Property(e => e.ReasonForCancellation).HasColumnName("Reason For Cancellation");

                entity.Property(e => e.RecordStatus).HasColumnName("Record Status");

                entity.Property(e => e.RegdNo).HasColumnName("Regd No");

                entity.Property(e => e.SecondaryOrderFlag).HasColumnName("Secondary Order Flag");

                entity.Property(e => e.SponsorName).HasColumnName("Sponsor Name");

                entity.Property(e => e.SponsorOrderNo).HasColumnName("Sponsor Order No");

                entity.Property(e => e.SponsorProgCode).HasColumnName("Sponsor Prog# code");

                entity.Property(e => e.StatusReason).HasColumnName("Status Reason");

                entity.Property(e => e.StoreCode).HasColumnName("Store Code");

                entity.Property(e => e.StoreNameForPurchasedProduct).HasColumnName("Store Name For Purchased Product");

                entity.Property(e => e.StorePhoneNumber).HasColumnName("Store Phone Number");

                entity.Property(e => e.StoreStatus).HasColumnName("Store Status");

                entity.Property(e => e.SvcSCallNo).HasColumnName("SVC's Call No");

                entity.Property(e => e.TotalQuoteP).HasColumnName("Total Quote P");

                entity.Property(e => e.TotalQuoteQ).HasColumnName("Total Quote Q");

                entity.Property(e => e.TotalQuoteR).HasColumnName("Total Quote R");

                entity.Property(e => e.TotalQuoteS).HasColumnName("Total Quote S");

                entity.Property(e => e.UploadDateTime).HasColumnName("Upload Date & Time");

                entity.Property(e => e.VoucherAmount).HasColumnName("Voucher Amount");

                entity.Property(e => e.VoucherCode).HasColumnName("Voucher Code");

                entity.Property(e => e.VoucherRedeemDate).HasColumnName("Voucher Redeem Date");
            });

            modelBuilder.Entity<EvcallCallAllocationsDatum>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("EVCAll_Call_AllocationsData");

                entity.Property(e => e.ActualEvcAmountAsPerQc).HasColumnName("Actual EVC Amount As Per QC");

                entity.Property(e => e.ActualProdQltyAtTimeOfQc)
                    .HasMaxLength(255)
                    .HasColumnName("Actual Prod Qlty (at time of QC)");

                entity.Property(e => e.AddressLine1)
                    .HasMaxLength(255)
                    .HasColumnName("Address Line 1");

                entity.Property(e => e.AddressLine2)
                    .HasMaxLength(255)
                    .HasColumnName("Address Line 2");

                entity.Property(e => e.AllocationStatus)
                    .HasMaxLength(255)
                    .HasColumnName("Allocation Status");

                entity.Property(e => e.AllocationsDataId).ValueGeneratedOnAdd();

                entity.Property(e => e.City).HasMaxLength(255);

                entity.Property(e => e.CustCity)
                    .HasMaxLength(255)
                    .HasColumnName("Cust City");

                entity.Property(e => e.CustPinCode).HasColumnName("Cust PIN Code");

                entity.Property(e => e.EvcAddress)
                    .HasMaxLength(255)
                    .HasColumnName("EVC Address");

                entity.Property(e => e.EvcBusinessName)
                    .HasMaxLength(255)
                    .HasColumnName("EVC BusinessName");

                entity.Property(e => e.EvcCode)
                    .HasMaxLength(255)
                    .HasColumnName("EVC Code");

                entity.Property(e => e.InvoiceDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Invoice Date");

                entity.Property(e => e.InvoiceNo)
                    .HasMaxLength(255)
                    .HasColumnName("Invoice No");

                entity.Property(e => e.ProdGroup)
                    .HasMaxLength(255)
                    .HasColumnName("Prod Group");

                entity.Property(e => e.ProofOfDelivery)
                    .HasMaxLength(255)
                    .HasColumnName("Proof Of Delivery");

                entity.Property(e => e.RNo)
                    .HasMaxLength(255)
                    .HasColumnName("R# No#");

                entity.Property(e => e.State).HasMaxLength(255);

                entity.Property(e => e.Type).HasMaxLength(255);
            });

            modelBuilder.Entity<Login>(entity =>
            {
                entity.ToTable("Login");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.PriceCode).HasMaxLength(200);

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("username");

                entity.Property(e => e.ZohoSponsorId).HasMaxLength(100);

                entity.HasOne(d => d.PriceMasterName)
                    .WithMany(p => p.Logins)
                    .HasForeignKey(d => d.PriceMasterNameId)
                    .HasConstraintName("FK__Login__PriceMast__09B45E9A");
            });

            modelBuilder.Entity<MapLoginUserDevice>(entity =>
            {
                entity.ToTable("MapLoginUserDevice");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DeviceType)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.UserDeviceId).IsUnicode(false);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.MapLoginUserDeviceCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__MapLoginU__Creat__40AF8DC9");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.MapLoginUserDeviceModifiedByNavigations)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK__MapLoginU__Modif__3FBB6990");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.MapLoginUserDeviceUsers)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__MapLoginU__UserI__04CFADEC");
            });

            modelBuilder.Entity<MapServicePartnerCityState>(entity =>
            {
                entity.ToTable("Map_ServicePartnerCityState");

                entity.Property(e => e.MapServicePartnerCityStateId).HasColumnName("Map_ServicePartnerCityStateId");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.MapServicePartnerCityStates)
                    .HasForeignKey(d => d.CityId)
                    .HasConstraintName("FK__Map_Servi__CityI__07E124C1");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.MapServicePartnerCityStateCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__Map_Servi__Creat__09C96D33");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.MapServicePartnerCityStateModifiedByNavigations)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK__Map_Servi__Modif__0ABD916C");

                entity.HasOne(d => d.Pincode)
                    .WithMany(p => p.MapServicePartnerCityStates)
                    .HasForeignKey(d => d.PincodeId)
                    .HasConstraintName("FK__Map_Servi__Pinco__06ED0088");

                entity.HasOne(d => d.ServicePartner)
                    .WithMany(p => p.MapServicePartnerCityStates)
                    .HasForeignKey(d => d.ServicePartnerId)
                    .HasConstraintName("FK__Map_Servi__Servi__05F8DC4F");

                entity.HasOne(d => d.State)
                    .WithMany(p => p.MapServicePartnerCityStates)
                    .HasForeignKey(d => d.StateId)
                    .HasConstraintName("FK__Map_Servi__State__08D548FA");
            });

            modelBuilder.Entity<Mapping>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("_Mapping");

                entity.Property(e => e.Brand).HasMaxLength(255);

                entity.Property(e => e.Brandid).HasColumnName("brandid");

                entity.Property(e => e.Group).HasMaxLength(255);

                entity.Property(e => e.SrNo).HasColumnName("Sr#No");
            });

            modelBuilder.Entity<PriceMasterName>(entity =>
            {
                entity.ToTable("PriceMasterName");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.PriceMasterNameCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__PriceMast__Creat__3414ACBA");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.PriceMasterNameModifiedByNavigations)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK__PriceMast__Modif__3508D0F3");
            });

            modelBuilder.Entity<Tbl247Around>(entity =>
            {
                entity.ToTable("tbl247Around");

                entity.Property(e => e.Address)
                    .HasMaxLength(100)
                    .HasColumnName("address");

                entity.Property(e => e.Brand)
                    .HasMaxLength(100)
                    .HasColumnName("brand");

                entity.Property(e => e.Category)
                    .HasMaxLength(100)
                    .HasColumnName("category");

                entity.Property(e => e.City)
                    .HasMaxLength(100)
                    .HasColumnName("city");

                entity.Property(e => e.DeliveryDate)
                    .HasColumnType("datetime")
                    .HasColumnName("deliveryDate");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .HasColumnName("email");

                entity.Property(e => e.ItemId)
                    .HasMaxLength(100)
                    .HasColumnName("itemID");

                entity.Property(e => e.Mobile)
                    .HasMaxLength(100)
                    .HasColumnName("mobile");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.Property(e => e.OrderId)
                    .HasMaxLength(100)
                    .HasColumnName("orderID");

                entity.Property(e => e.PaidByCustomer)
                    .HasMaxLength(100)
                    .HasColumnName("paidByCustomer");

                entity.Property(e => e.PartnerName)
                    .HasMaxLength(100)
                    .HasColumnName("partnerName");

                entity.Property(e => e.Pincode)
                    .HasMaxLength(100)
                    .HasColumnName("pincode");

                entity.Property(e => e.Product)
                    .HasMaxLength(100)
                    .HasColumnName("product");

                entity.Property(e => e.ProductType)
                    .HasMaxLength(100)
                    .HasColumnName("productType");

                entity.Property(e => e.RequestType)
                    .HasMaxLength(100)
                    .HasColumnName("requestType");

                entity.Property(e => e.SubCategory)
                    .HasMaxLength(100)
                    .HasColumnName("subCategory");

                entity.Property(e => e.TwoFourSevenAroundBooKingId)
                    .HasMaxLength(100)
                    .HasColumnName("TwoFourSevenAroundBooKingID");
            });

            modelBuilder.Entity<TblAbbplanMaster>(entity =>
            {
                entity.HasKey(e => e.PlanMasterId);

                entity.ToTable("tblABBPlanMaster");

                entity.Property(e => e.AbbplanName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("ABBPlanName");

                entity.Property(e => e.AssuredBuyBackPercentage).HasColumnName("Assured_BuyBack_Percentage");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.FromMonth).HasColumnName("From_Month");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.NoClaimPeriod)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Sponsor).HasMaxLength(255);

                entity.Property(e => e.ToMonth).HasColumnName("To_Month");

                entity.HasOne(d => d.BusinessUnit)
                    .WithMany(p => p.TblAbbplanMasters)
                    .HasForeignKey(d => d.BusinessUnitId)
                    .HasConstraintName("FK_tblABBPlanMaster_tblBusinessUnit");

                entity.HasOne(d => d.ProductCat)
                    .WithMany(p => p.TblAbbplanMasters)
                    .HasForeignKey(d => d.ProductCatId)
                    .HasConstraintName("FK_tblABBPlanMaster_ProductCatId");

                entity.HasOne(d => d.ProductType)
                    .WithMany(p => p.TblAbbplanMasters)
                    .HasForeignKey(d => d.ProductTypeId)
                    .HasConstraintName("FK__tblABBPla__Produ__627A95E8");
            });

            modelBuilder.Entity<TblAbbpriceMaster>(entity =>
            {
                entity.HasKey(e => e.PriceMasterId);

                entity.ToTable("tblABBPriceMaster");

                entity.Property(e => e.BusinessPartnerMarginAmount).HasColumnType("decimal(15, 2)");

                entity.Property(e => e.BusinessPartnerMarginPerc).HasColumnType("decimal(15, 2)");

                entity.Property(e => e.BusinessUnitMarginAmount).HasColumnType("decimal(15, 2)");

                entity.Property(e => e.BusinessUnitMarginPerc).HasColumnType("decimal(15, 2)");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.FeeType)
                    .HasMaxLength(255)
                    .HasColumnName("Fee_Type");

                entity.Property(e => e.FeesApplicableAmt)
                    .HasColumnType("decimal(15, 2)")
                    .HasColumnName("Fees_Applicable_Amt");

                entity.Property(e => e.FeesApplicablePercentage)
                    .HasColumnType("decimal(15, 2)")
                    .HasColumnName("Fees_Applicable_Percentage");

                entity.Property(e => e.GstValueForNewProduct).HasColumnType("decimal(15, 2)");

                entity.Property(e => e.Gstexclusive)
                    .HasColumnType("decimal(15, 2)")
                    .HasColumnName("GSTExclusive");

                entity.Property(e => e.Gstinclusive)
                    .HasColumnType("decimal(15, 2)")
                    .HasColumnName("GSTInclusive");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.PlanPeriodInMonths).HasColumnName("Plan_Period_in_Months");

                entity.Property(e => e.PriceEndRange).HasColumnName("Price_End_Range");

                entity.Property(e => e.PriceStartRange).HasColumnName("Price_Start_Range");

                entity.Property(e => e.ProductCategory)
                    .HasMaxLength(255)
                    .HasColumnName("Product_Category");

                entity.Property(e => e.ProductSabcategory)
                    .HasMaxLength(255)
                    .HasColumnName("Product_Sabcategory");

                entity.Property(e => e.Sponsor).HasMaxLength(255);

                entity.HasOne(d => d.BusinessUnit)
                    .WithMany(p => p.TblAbbpriceMasters)
                    .HasForeignKey(d => d.BusinessUnitId)
                    .HasConstraintName("FK_tblABBPriceMaster_tblBusinessUnit");

                entity.HasOne(d => d.ProductCat)
                    .WithMany(p => p.TblAbbpriceMasters)
                    .HasForeignKey(d => d.ProductCatId)
                    .HasConstraintName("FK_tblABBPriceMaster_tblProductCategory");

                entity.HasOne(d => d.ProductType)
                    .WithMany(p => p.TblAbbpriceMasters)
                    .HasForeignKey(d => d.ProductTypeId)
                    .HasConstraintName("FK__tblABBPri__Produ__636EBA21");
            });

            modelBuilder.Entity<TblAbbredemption>(entity =>
            {
                entity.HasKey(e => e.RedemptionId);

                entity.ToTable("tblABBRedemption");

                entity.Property(e => e.AbbredemptionStatus)
                    .HasMaxLength(50)
                    .HasColumnName("ABBRedemptionStatus");

                entity.Property(e => e.AbbregistrationId).HasColumnName("ABBRegistrationId");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.FinalRedemptionValue).HasColumnType("decimal(15, 2)");

                entity.Property(e => e.InvoiceDate).HasColumnType("date");

                entity.Property(e => e.InvoiceImage).HasMaxLength(2000);

                entity.Property(e => e.InvoiceNo).HasMaxLength(50);

                entity.Property(e => e.LogisticsComments).HasMaxLength(4000);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Qccomments)
                    .HasMaxLength(4000)
                    .HasColumnName("QCComments");

                entity.Property(e => e.RedemptionDate).HasColumnType("date");

                entity.Property(e => e.RedemptionValue).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.ReferenceId)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.RegdNo).HasMaxLength(10);

                entity.Property(e => e.Sponsor).HasMaxLength(50);

                entity.Property(e => e.StoreOrderNo).HasMaxLength(50);

                entity.Property(e => e.VoucherCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.VoucherCodeExpDate).HasColumnType("datetime");

                entity.Property(e => e.ZohoAbbredemptionId).HasColumnName("ZohoABBRedemptionId");

                entity.HasOne(d => d.Abbregistration)
                    .WithMany(p => p.TblAbbredemptions)
                    .HasForeignKey(d => d.AbbregistrationId)
                    .HasConstraintName("FK_tblABBRedemption_tblABBRegistration");

                entity.HasOne(d => d.BusinessPartner)
                    .WithMany(p => p.TblAbbredemptions)
                    .HasForeignKey(d => d.BusinessPartnerId)
                    .HasConstraintName("FK_BusinessPartnerId");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.TblAbbredemptionCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_tblABBRedemption_tblUser");

                entity.HasOne(d => d.CustomerDetails)
                    .WithMany(p => p.TblAbbredemptions)
                    .HasForeignKey(d => d.CustomerDetailsId)
                    .HasConstraintName("FK_tblABBRedemption_tblCustomerDetails");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.TblAbbredemptionModifiedByNavigations)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK_tblABBRedemption_tblUser1");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.TblAbbredemptions)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("FK_tblABBRedemption_tblExchangeOrderStatus");

                entity.HasOne(d => d.VoucherStatus)
                    .WithMany(p => p.TblAbbredemptions)
                    .HasForeignKey(d => d.VoucherStatusId)
                    .HasConstraintName("FK_VoucherStatus");
            });

            modelBuilder.Entity<TblAbbregistration>(entity =>
            {
                entity.HasKey(e => e.AbbregistrationId)
                    .HasName("PK__tblABBRegistration__D65247C2BDC3EBF0");

                entity.ToTable("tblABBRegistration");

                entity.Property(e => e.AbbregistrationId).HasColumnName("ABBRegistrationId");

                entity.Property(e => e.Abbfees)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("ABBFees");

                entity.Property(e => e.AbbplanName)
                    .HasMaxLength(150)
                    .HasColumnName("ABBPlanName");

                entity.Property(e => e.AbbplanPeriod)
                    .HasMaxLength(10)
                    .HasColumnName("ABBPlanPeriod");

                entity.Property(e => e.AbbpriceId).HasColumnName("ABBPriceId");

                entity.Property(e => e.BaseValue).HasColumnType("decimal(15, 2)");

                entity.Property(e => e.BusinessUnitMargin).HasColumnType("decimal(15, 2)");

                entity.Property(e => e.Cgst).HasColumnType("decimal(15, 2)");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.CustAddress1)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.CustAddress2)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.CustCity)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.CustEmail).HasMaxLength(250);

                entity.Property(e => e.CustFirstName)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.CustLastName)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.CustMobile).HasMaxLength(50);

                entity.Property(e => e.CustPinCode).HasMaxLength(250);

                entity.Property(e => e.CustState)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.DealerMargin).HasColumnType("decimal(15, 2)");

                entity.Property(e => e.EmployeeId)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Hsncode)
                    .HasMaxLength(50)
                    .HasColumnName("HSNCode");

                entity.Property(e => e.InvoiceDate).HasColumnType("date");

                entity.Property(e => e.InvoiceNo).HasMaxLength(150);

                entity.Property(e => e.Location).HasMaxLength(250);

                entity.Property(e => e.MarCom).HasMaxLength(50);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.NewPrice).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.NewProductCategoryName).HasMaxLength(50);

                entity.Property(e => e.NewProductCategoryType).HasMaxLength(50);

                entity.Property(e => e.NewSize).HasMaxLength(50);

                entity.Property(e => e.NoOfClaimPeriod).HasMaxLength(10);

                entity.Property(e => e.OrderType).HasMaxLength(150);

                entity.Property(e => e.OtherModelNo).HasMaxLength(255);

                entity.Property(e => e.ProductNetPrice).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.ProductSrNo).HasMaxLength(50);

                entity.Property(e => e.RegdNo)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Sgst).HasColumnType("decimal(15, 2)");

                entity.Property(e => e.SponsorOrderNo).HasMaxLength(150);

                entity.Property(e => e.SponsorProdCode).HasMaxLength(50);

                entity.Property(e => e.StoreCode)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.StoreManagerEmail).HasMaxLength(50);

                entity.Property(e => e.StoreName)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.UploadDateTime).HasColumnType("datetime");

                entity.Property(e => e.YourRegistrationNo).HasMaxLength(50);

                entity.Property(e => e.ZohoAbbregistrationId)
                    .HasMaxLength(50)
                    .HasColumnName("ZohoABBRegistrationId");

                entity.HasOne(d => d.BusinessPartner)
                    .WithMany(p => p.TblAbbregistrations)
                    .HasForeignKey(d => d.BusinessPartnerId)
                    .HasConstraintName("FK__tblABBReg__BP__2B0A656D");

                entity.HasOne(d => d.BusinessUnit)
                    .WithMany(p => p.TblAbbregistrations)
                    .HasForeignKey(d => d.BusinessUnitId)
                    .HasConstraintName("FK_tblABBRegistration_BusinessUnitId");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.TblAbbregistrations)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_CustomerId");

                entity.HasOne(d => d.ModelNumber)
                    .WithMany(p => p.TblAbbregistrations)
                    .HasForeignKey(d => d.ModelNumberId)
                    .HasConstraintName("FK_tblABBRegistration_ModelNumberId");

                entity.HasOne(d => d.NewProductCategory)
                    .WithMany(p => p.TblAbbregistrations)
                    .HasForeignKey(d => d.NewProductCategoryId)
                    .HasConstraintName("FK__tblABBReg__PC__2B0A656D");

                entity.HasOne(d => d.NewProductCategoryTypeNavigation)
                    .WithMany(p => p.TblAbbregistrations)
                    .HasForeignKey(d => d.NewProductCategoryTypeId)
                    .HasConstraintName("FK__tblABBReg__NewPr__5BCD9859");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.TblAbbregistrations)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("FK__tblABBReg__Statu__5DEAEAF5");
            });

            modelBuilder.Entity<TblAccessList>(entity =>
            {
                entity.HasKey(e => e.AccessListId)
                    .HasName("PK__tblAcces__158C8AA401FA3C46");

                entity.ToTable("tblAccessList");

                entity.Property(e => e.ActionName).HasMaxLength(150);

                entity.Property(e => e.ActionUrl)
                    .HasMaxLength(600)
                    .HasColumnName("ActionURL");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.SetIcon)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.TblAccessLists)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("Fk_tblAccessList_CompanyId");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.TblAccessListCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("Fk_tblAccessList_CreatedBy");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.TblAccessListModifiedByNavigations)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("Fk_tblAccessList_ModifiedBy");

                entity.HasOne(d => d.ParentAccessList)
                    .WithMany(p => p.InverseParentAccessList)
                    .HasForeignKey(d => d.ParentAccessListId)
                    .HasConstraintName("Fk_tblAccessList_ParentAccessListId");
            });

            modelBuilder.Entity<TblAddress>(entity =>
            {
                entity.HasKey(e => e.UsersAddressId)
                    .HasName("PK__tblAddre__FB72B1D6ACE0B3AB");

                entity.ToTable("tblAddress");

                entity.Property(e => e.Address1).HasMaxLength(150);

                entity.Property(e => e.Address2).HasMaxLength(150);

                entity.Property(e => e.Address3).HasMaxLength(150);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Title).HasMaxLength(150);

                entity.Property(e => e.ZipCode)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.TblAddresses)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("Fk_tblAddress_CompanyId");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.TblAddressCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("Fk_tblAddress_CreatedBy");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.TblAddressModifiedByNavigations)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("Fk_tblAddress_ModifiedBy");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TblAddressUsers)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("Fk_tblAddress_userId");
            });

            modelBuilder.Entity<TblApicall>(entity =>
            {
                entity.HasKey(e => e.ApicallId)
                    .HasName("PK__tblAPICa__73861C0D2DC234F5");

                entity.ToTable("tblAPICalls");

                entity.Property(e => e.ApicallId).HasColumnName("APICallId");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.MethodType).HasMaxLength(100);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Url)
                    .HasMaxLength(100)
                    .HasColumnName("URL");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.TblApicallCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("Fk_tblAPICalls_CreatedBy");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.TblApicallModifiedByNavigations)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("Fk_tblAPICalls_ModifiedBy");
            });

            modelBuilder.Entity<TblAreaLocality>(entity =>
            {
                entity.HasKey(e => e.AreaId);

                entity.ToTable("tblAreaLocality");

                entity.Property(e => e.AreaLocality).HasMaxLength(50);

                entity.Property(e => e.BranchCode).HasMaxLength(50);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.District).HasMaxLength(50);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Taluk).HasMaxLength(50);
            });

            modelBuilder.Entity<TblBizlogTicket>(entity =>
            {
                entity.ToTable("tblBizlogTicket");

                entity.Property(e => e.AddressLine1).HasMaxLength(100);

                entity.Property(e => e.AddressLine2).HasMaxLength(100);

                entity.Property(e => e.AlternateTelephoneNumber).HasMaxLength(100);

                entity.Property(e => e.BizlogTicketNo).HasMaxLength(100);

                entity.Property(e => e.Brand).HasMaxLength(100);

                entity.Property(e => e.City).HasMaxLength(100);

                entity.Property(e => e.ConsumerComplaintNumber).HasMaxLength(100);

                entity.Property(e => e.ConsumerName).HasMaxLength(100);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DateOfComplaint).HasMaxLength(100);

                entity.Property(e => e.DateOfPurchase).HasMaxLength(100);

                entity.Property(e => e.DropLocAddress1).HasMaxLength(100);

                entity.Property(e => e.DropLocAddress2).HasMaxLength(100);

                entity.Property(e => e.DropLocAlternateNo).HasMaxLength(100);

                entity.Property(e => e.DropLocCity).HasMaxLength(100);

                entity.Property(e => e.DropLocContactNo).HasMaxLength(100);

                entity.Property(e => e.DropLocContactPerson).HasMaxLength(100);

                entity.Property(e => e.DropLocPincode).HasMaxLength(100);

                entity.Property(e => e.DropLocState).HasMaxLength(100);

                entity.Property(e => e.DropLocation).HasMaxLength(100);

                entity.Property(e => e.EmailId).HasMaxLength(100);

                entity.Property(e => e.IdentificationNo).HasMaxLength(100);

                entity.Property(e => e.IsUnderWarranty).HasMaxLength(100);

                entity.Property(e => e.Model).HasMaxLength(100);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.NatureOfComplaint).HasMaxLength(100);

                entity.Property(e => e.PhysicalEvaluation).HasMaxLength(100);

                entity.Property(e => e.Pincode).HasMaxLength(100);

                entity.Property(e => e.ProductCategory).HasMaxLength(100);

                entity.Property(e => e.ProductCode).HasMaxLength(100);

                entity.Property(e => e.ProductName).HasMaxLength(100);

                entity.Property(e => e.RetailerPhoneNo).HasMaxLength(100);

                entity.Property(e => e.SponsrorOrderNo).HasMaxLength(100);

                entity.Property(e => e.TechEvalRequired).HasMaxLength(100);

                entity.Property(e => e.TelephoneNumber).HasMaxLength(100);

                entity.Property(e => e.TicketPriority).HasMaxLength(200);

                entity.Property(e => e.Value).HasMaxLength(100);
            });

            modelBuilder.Entity<TblBizlogTicketStatus>(entity =>
            {
                entity.ToTable("tblBizlogTicketStatus");

                entity.Property(e => e.AwbNo)
                    .HasMaxLength(50)
                    .HasColumnName("awbNo");

                entity.Property(e => e.BizlogTicketNo).HasMaxLength(100);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Status).HasMaxLength(200);
            });

            modelBuilder.Entity<TblBlowHornTicket>(entity =>
            {
                entity.ToTable("tblBlowHornTicket");

                entity.Property(e => e.AlternateCustomerMobile)
                    .HasMaxLength(100)
                    .HasColumnName("alternate_customer_mobile");

                entity.Property(e => e.AwbNumber)
                    .HasMaxLength(100)
                    .HasColumnName("awb_number");

                entity.Property(e => e.CashOnDelivery)
                    .HasMaxLength(100)
                    .HasColumnName("cash_on_delivery");

                entity.Property(e => e.CommercialClass)
                    .HasMaxLength(100)
                    .HasColumnName("commercial_class");

                entity.Property(e => e.CustomerEmail)
                    .HasMaxLength(100)
                    .HasColumnName("customer_email");

                entity.Property(e => e.CustomerMobile)
                    .HasMaxLength(100)
                    .HasColumnName("customer_mobile");

                entity.Property(e => e.CustomerName)
                    .HasMaxLength(100)
                    .HasColumnName("customer_name");

                entity.Property(e => e.CustomerReferenceNumber)
                    .HasMaxLength(100)
                    .HasColumnName("customer_reference_number");

                entity.Property(e => e.DeliveryAddress)
                    .HasMaxLength(100)
                    .HasColumnName("delivery_address");

                entity.Property(e => e.DeliveryHub)
                    .HasMaxLength(100)
                    .HasColumnName("delivery_hub");

                entity.Property(e => e.DeliveryLat)
                    .HasMaxLength(100)
                    .HasColumnName("delivery_lat");

                entity.Property(e => e.DeliveryLon)
                    .HasMaxLength(100)
                    .HasColumnName("delivery_lon");

                entity.Property(e => e.DeliveryPostalCode)
                    .HasMaxLength(100)
                    .HasColumnName("delivery_postal_code");

                entity.Property(e => e.Division)
                    .HasMaxLength(100)
                    .HasColumnName("division");

                entity.Property(e => e.ExpectedDeliveryTime)
                    .HasColumnType("datetime")
                    .HasColumnName("expected_delivery_time");

                entity.Property(e => e.IsCod).HasColumnName("is_cod");

                entity.Property(e => e.IsCommercialAddress)
                    .HasMaxLength(100)
                    .HasColumnName("is_commercial_address");

                entity.Property(e => e.IsHyperlocal)
                    .HasMaxLength(100)
                    .HasColumnName("is_hyperlocal");

                entity.Property(e => e.IsReturnOrder)
                    .HasMaxLength(100)
                    .HasColumnName("is_return_order");

                entity.Property(e => e.PickupAddress)
                    .HasMaxLength(100)
                    .HasColumnName("pickup_address");

                entity.Property(e => e.PickupCustomerMobile)
                    .HasMaxLength(100)
                    .HasColumnName("pickup_customer_mobile");

                entity.Property(e => e.PickupCustomerName)
                    .HasMaxLength(100)
                    .HasColumnName("pickup_customer_name");

                entity.Property(e => e.PickupDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("pickup_datetime");

                entity.Property(e => e.PickupHub)
                    .HasMaxLength(100)
                    .HasColumnName("pickup_hub");

                entity.Property(e => e.PickupLat)
                    .HasMaxLength(100)
                    .HasColumnName("pickup_lat");

                entity.Property(e => e.PickupLon)
                    .HasMaxLength(100)
                    .HasColumnName("pickup_lon");

                entity.Property(e => e.PickupPostalCode)
                    .HasMaxLength(100)
                    .HasColumnName("pickup_postal_code");

                entity.Property(e => e.PinNumber)
                    .HasMaxLength(100)
                    .HasColumnName("pin_number");

                entity.Property(e => e.ProductStatus).HasMaxLength(100);

                entity.Property(e => e.ReferenceNumber)
                    .HasMaxLength(100)
                    .HasColumnName("reference_number");

                entity.Property(e => e.SponsrorOrderNo).HasMaxLength(100);

                entity.Property(e => e.What3words)
                    .HasMaxLength(100)
                    .HasColumnName("what3words");
            });

            modelBuilder.Entity<TblBpbuassociation>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tblBPBUAssociation");

                entity.Property(e => e.AssociationCode).HasMaxLength(250);

                entity.Property(e => e.BpbuassociationId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("BPBUAssociationId");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.BusinessPartner)
                    .WithMany()
                    .HasForeignKey(d => d.BusinessPartnerId)
                    .HasConstraintName("FK_tblBPBUAssociation_tblBusinessPartner");

                entity.HasOne(d => d.BusinessUnit)
                    .WithMany()
                    .HasForeignKey(d => d.BusinessUnitId)
                    .HasConstraintName("FK_tblBPBUAssociation_tblBusinessUnit");
            });

            modelBuilder.Entity<TblBpburedemptionMapping>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tblBPBURedemptionMapping");

                entity.Property(e => e.BpburedemptionMappingId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("BPBURedemptionMappingId");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.BusinessPartner)
                    .WithMany()
                    .HasForeignKey(d => d.BusinessPartnerId)
                    .HasConstraintName("FK_tblBPBURedemptionMapping_tblBusinessPartner");

                entity.HasOne(d => d.BusinessUnit)
                    .WithMany()
                    .HasForeignKey(d => d.BusinessUnitId)
                    .HasConstraintName("FK_tblBPBURedemptionMapping_tblBusinessUnit");
            });

            modelBuilder.Entity<TblBppincodeMapping>(entity =>
            {
                entity.HasKey(e => e.BupincodeMappingId);

                entity.ToTable("TblBPPincodeMapping");

                entity.Property(e => e.BupincodeMappingId).HasColumnName("BUPincodeMappingId");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.BusinessPartner)
                    .WithMany(p => p.TblBppincodeMappings)
                    .HasForeignKey(d => d.BusinessPartnerId)
                    .HasConstraintName("Fk_TblBPPincodeMapping_BusinessPartnerId");

                entity.HasOne(d => d.BusinessUnit)
                    .WithMany(p => p.TblBppincodeMappings)
                    .HasForeignKey(d => d.BusinessUnitId)
                    .HasConstraintName("Fk_TblBPPincodeMapping_BusinessUnitId");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.TblBppincodeMappingCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("Fk_TblBPPincodeMapping_CreatedBy");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.TblBppincodeMappingModifiedByNavigations)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("Fk_TblBPPincodeMapping_ModifiedBy");

                entity.HasOne(d => d.Pincode)
                    .WithMany(p => p.TblBppincodeMappings)
                    .HasForeignKey(d => d.PincodeId)
                    .HasConstraintName("Fk_TblBPPincodeMapping_PincodeId");
            });

            modelBuilder.Entity<TblBrand>(entity =>
            {
                entity.ToTable("tblBrand");

                entity.Property(e => e.BrandLogoUrl)
                    .HasMaxLength(600)
                    .HasColumnName("BrandLogoURL");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.HasOne(d => d.BusinessUnit)
                    .WithMany(p => p.TblBrands)
                    .HasForeignKey(d => d.BusinessUnitId)
                    .HasConstraintName("FK__tblBrand__Busine__5D60DB10");
            });

            modelBuilder.Entity<TblBrandGroup>(entity =>
            {
                entity.HasKey(e => e.BrandGroupId)
                    .HasName("PK__TblBrand__F118381667EA33BA");

                entity.ToTable("TblBrandGroup");

                entity.Property(e => e.BrandGroupName).HasMaxLength(255);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Weightage)
                    .HasColumnType("decimal(15, 2)")
                    .HasColumnName("weightage");

                entity.HasOne(d => d.CreatedbyNavigation)
                    .WithMany(p => p.TblBrandGroupCreatedbyNavigations)
                    .HasForeignKey(d => d.Createdby)
                    .HasConstraintName("FK__TblBrandG__Creat__1EAF7B80");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.TblBrandGroupModifiedByNavigations)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK__TblBrandG__Modif__1FA39FB9");

                entity.HasOne(d => d.ProductCat)
                    .WithMany(p => p.TblBrandGroups)
                    .HasForeignKey(d => d.ProductCatId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TblBrandG__Produ__1DBB5747");
            });

            modelBuilder.Entity<TblBrandSmartBuy>(entity =>
            {
                entity.ToTable("tblBrandSmartBuy");

                entity.Property(e => e.BrandLogoUrl)
                    .HasMaxLength(600)
                    .HasColumnName("BrandLogoURL");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.TblBrandSmartBuys)
                    .HasForeignKey(d => d.BrandId)
                    .HasConstraintName("FK__tblBrandS__Brand__1A1FD08D");

                entity.HasOne(d => d.BusinessUnit)
                    .WithMany(p => p.TblBrandSmartBuys)
                    .HasForeignKey(d => d.BusinessUnitId)
                    .HasConstraintName("FK__tblBrandSmartBuy_tblBusinessUnit");

                entity.HasOne(d => d.ProductCategory)
                    .WithMany(p => p.TblBrandSmartBuys)
                    .HasForeignKey(d => d.ProductCategoryId)
                    .HasConstraintName("FK_tblBrandSmartBuy_tblProductCategory");
            });

            modelBuilder.Entity<TblBubasedSweetnerValidation>(entity =>
            {
                entity.ToTable("tblBUBasedSweetnerValidation");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.BusinessUnit)
                    .WithMany(p => p.TblBubasedSweetnerValidations)
                    .HasForeignKey(d => d.BusinessUnitId)
                    .HasConstraintName("Fk_tblBUBasedSweetnerValidation_BusinessUnitId");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.TblBubasedSweetnerValidationCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("Fk_tblBUBasedSweetnerValidation_CreatedBy");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.TblBubasedSweetnerValidationModifiedByNavigations)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("Fk_tblBUBasedSweetnerValidation_ModifiedBy");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.TblBubasedSweetnerValidations)
                    .HasForeignKey(d => d.QuestionId)
                    .HasConstraintName("Fk_tblBUBasedSweetnerValidation_QuestionId");
            });

            modelBuilder.Entity<TblBuconfiguration>(entity =>
            {
                entity.HasKey(e => e.ConfigId);

                entity.ToTable("tblBUConfiguration");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(300);

                entity.Property(e => e.Key).HasMaxLength(150);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.TblBuconfigurationCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("Fk_tblBUConfiguration_CreatedBy");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.TblBuconfigurationModifiedByNavigations)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("Fk_tblBUConfiguration_ModifiedBy");
            });

            modelBuilder.Entity<TblBuconfigurationMapping>(entity =>
            {
                entity.ToTable("tblBUConfigurationMapping");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Value).HasMaxLength(300);

                entity.HasOne(d => d.BusinessUnit)
                    .WithMany(p => p.TblBuconfigurationMappings)
                    .HasForeignKey(d => d.BusinessUnitId)
                    .HasConstraintName("FK_tblBUConfigurationMapping_BusinessUnitId");

                entity.HasOne(d => d.Config)
                    .WithMany(p => p.TblBuconfigurationMappings)
                    .HasForeignKey(d => d.ConfigId)
                    .HasConstraintName("Fk_tblBUConfigurationMapping_ConfigId");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.TblBuconfigurationMappingCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("Fk_tblBUConfigurationMapping_CreatedBy");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.TblBuconfigurationMappingModifiedByNavigations)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("Fk_tblBUConfigurationMapping_ModifiedBy");
            });

            modelBuilder.Entity<TblBuproductCategoryMapping>(entity =>
            {
                entity.ToTable("tblBUProductCategoryMapping");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.BusinessUnit)
                    .WithMany(p => p.TblBuproductCategoryMappings)
                    .HasForeignKey(d => d.BusinessUnitId)
                    .HasConstraintName("FK__tblBUProd__Busin__589C25F3");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.TblBuproductCategoryMappingCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("Fk_tblBUProductCategoryMapping_CreatedBy");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.TblBuproductCategoryMappingModifiedByNavigations)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("Fk_tblBUProductCategoryMapping_ModifiedBy");

                entity.HasOne(d => d.OldProductCat)
                    .WithMany(p => p.TblBuproductCategoryMappingOldProductCats)
                    .HasForeignKey(d => d.OldProductCatId)
                    .HasConstraintName("Fk_tblBUProductCategoryMapping_OldProductCatId");

                entity.HasOne(d => d.ProductCat)
                    .WithMany(p => p.TblBuproductCategoryMappingProductCats)
                    .HasForeignKey(d => d.ProductCatId)
                    .HasConstraintName("FK__tblBUProd__Produ__59904A2C");

                entity.HasOne(d => d.ProductType)
                    .WithMany(p => p.TblBuproductCategoryMappings)
                    .HasForeignKey(d => d.ProductTypeId)
                    .HasConstraintName("Fk_tblBUProductCategoryMapping_ProductTypeId");
            });

            modelBuilder.Entity<TblBusinessPartner>(entity =>
            {
                entity.HasKey(e => e.BusinessPartnerId)
                    .HasName("PK__tblBusinessPartner__D65247C2BDC3EBF0");

                entity.ToTable("tblBusinessPartner");

                entity.Property(e => e.AccountNo).HasMaxLength(100);

                entity.Property(e => e.AddressLine1).HasMaxLength(250);

                entity.Property(e => e.AddressLine2).HasMaxLength(250);

                entity.Property(e => e.AssociateCode).HasMaxLength(20);

                entity.Property(e => e.BankDetails).HasMaxLength(250);

                entity.Property(e => e.Bppassword)
                    .HasMaxLength(250)
                    .HasColumnName("BPPassword");

                entity.Property(e => e.City).HasMaxLength(150);

                entity.Property(e => e.CityId).HasColumnName("cityId");

                entity.Property(e => e.ContactPersonFirstName)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.ContactPersonLastName)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DashBoardImage)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Email).HasMaxLength(150);

                entity.Property(e => e.FormatName).HasMaxLength(200);

                entity.Property(e => e.Gstnumber)
                    .HasMaxLength(100)
                    .HasColumnName("GSTNumber");

                entity.Property(e => e.Ifsccode)
                    .HasMaxLength(200)
                    .HasColumnName("IFSCCode");

                entity.Property(e => e.IsAbbbp).HasColumnName("IsABBBP");

                entity.Property(e => e.IsD2c).HasColumnName("IsD2C");

                entity.Property(e => e.IsExchangeBp).HasColumnName("IsExchangeBP");

                entity.Property(e => e.IsOrc).HasColumnName("IsORC");

                entity.Property(e => e.IsUnInstallationRequired).HasDefaultValueSql("((0))");

                entity.Property(e => e.LogoImage)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(250);

                entity.Property(e => e.PhoneNumber).HasMaxLength(20);

                entity.Property(e => e.Pincode).HasMaxLength(150);

                entity.Property(e => e.QrcodeUrl)
                    .HasColumnType("text")
                    .HasColumnName("QRCodeURL");

                entity.Property(e => e.Qrimage)
                    .HasColumnType("text")
                    .HasColumnName("QRImage");

                entity.Property(e => e.SponsorName).HasMaxLength(100);

                entity.Property(e => e.State).HasMaxLength(150);

                entity.Property(e => e.StoreCode).HasMaxLength(150);

                entity.Property(e => e.StoreType).HasMaxLength(100);

                entity.Property(e => e.SweetenerBp)
                    .HasColumnType("decimal(15, 2)")
                    .HasColumnName("SweetenerBP");

                entity.Property(e => e.SweetenerBu)
                    .HasColumnType("decimal(15, 2)")
                    .HasColumnName("SweetenerBU");

                entity.Property(e => e.SweetenerDigi2l).HasColumnType("decimal(15, 2)");

                entity.Property(e => e.Upiid)
                    .HasMaxLength(150)
                    .HasColumnName("UPIId");

                entity.HasOne(d => d.BusinessUnit)
                    .WithMany(p => p.TblBusinessPartners)
                    .HasForeignKey(d => d.BusinessUnitId)
                    .HasConstraintName("FK_tblBusinessPartner_BusinessUnitId");

                entity.HasOne(d => d.CityNavigation)
                    .WithMany(p => p.TblBusinessPartners)
                    .HasForeignKey(d => d.CityId)
                    .HasConstraintName("FK__tblBusine__cityI__67C95AEA");
            });

            modelBuilder.Entity<TblBusinessUnit>(entity =>
            {
                entity.HasKey(e => e.BusinessUnitId)
                    .HasName("PK__tblBusinessUnit__D65247C2BDC3EBF0");

                entity.ToTable("tblBusinessUnit");

                entity.Property(e => e.AddressLine1).HasMaxLength(250);

                entity.Property(e => e.AddressLine2).HasMaxLength(250);

                entity.Property(e => e.City).HasMaxLength(150);

                entity.Property(e => e.ContactPersonFirstName)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.ContactPersonLastName)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(150);

                entity.Property(e => e.Gsttype).HasColumnName("GSTType");

                entity.Property(e => e.IsAbb).HasColumnName("IsABB");

                entity.Property(e => e.IsBpassociated).HasColumnName("IsBPAssociated");

                entity.Property(e => e.IsBucatIdOn)
                    .HasColumnName("IsBUCatIdOn")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.IsBud2c).HasColumnName("IsBUD2C");

                entity.Property(e => e.IsBumultiBrand).HasColumnName("IsBUMultiBrand");

                entity.Property(e => e.IsProductSerialNumberRequired).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsQcdateTimeRequiredOnD2c).HasColumnName("IsQCDateTimeRequiredOnD2C");

                entity.Property(e => e.IsQualityRequiredOnUi).HasColumnName("IsQualityRequiredOnUI");

                entity.Property(e => e.IsSfidrequired)
                    .HasColumnName("IsSFIDRequired")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.IsSponsorNumberRequiredOnUi).HasColumnName("IsSponsorNumberRequiredOnUI");

                entity.Property(e => e.LogoName)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(250);

                entity.Property(e => e.PhoneNumber).HasMaxLength(20);

                entity.Property(e => e.Pincode).HasMaxLength(150);

                entity.Property(e => e.QrcodeUrl)
                    .HasColumnType("text")
                    .HasColumnName("QRCodeURL");

                entity.Property(e => e.RegistrationNumber).HasMaxLength(150);

                entity.Property(e => e.ReportEmails).HasMaxLength(250);

                entity.Property(e => e.State).HasMaxLength(150);

                entity.Property(e => e.SweetnerForDtc)
                    .HasColumnType("decimal(15, 2)")
                    .HasColumnName("SweetnerForDTC");

                entity.Property(e => e.SweetnerForDtd)
                    .HasColumnType("decimal(15, 2)")
                    .HasColumnName("SweetnerForDTD");

                entity.Property(e => e.ZohoSponsorId).HasMaxLength(50);

                entity.HasOne(d => d.GsttypeNavigation)
                    .WithMany(p => p.TblBusinessUnitGsttypeNavigations)
                    .HasForeignKey(d => d.Gsttype)
                    .HasConstraintName("Fk_tblBusinessUnit_GSTType");

                entity.HasOne(d => d.Login)
                    .WithMany(p => p.TblBusinessUnits)
                    .HasForeignKey(d => d.LoginId)
                    .HasConstraintName("FK_tblBusinessUnit_LoginId");

                entity.HasOne(d => d.MarginTypeNavigation)
                    .WithMany(p => p.TblBusinessUnitMarginTypeNavigations)
                    .HasForeignKey(d => d.MarginType)
                    .HasConstraintName("Fk_tblBusinessUnit_MarginType");
            });

            modelBuilder.Entity<TblCity>(entity =>
            {
                entity.HasKey(e => e.CityId)
                    .HasName("PK__tblCity__F2D21B76A8A4E9FF");

                entity.ToTable("tblCity");

                entity.Property(e => e.CityCode).HasMaxLength(50);

                entity.Property(e => e.CityLogo)
                    .HasMaxLength(255)
                    .HasColumnName("cityLogo");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.IsMetro).HasColumnName("isMetro");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.HasOne(d => d.State)
                    .WithMany(p => p.TblCities)
                    .HasForeignKey(d => d.StateId)
                    .HasConstraintName("Fk_tblCity_StateId");
            });

            modelBuilder.Entity<TblCompany>(entity =>
            {
                entity.HasKey(e => e.CompanyId)
                    .HasName("PK__tblCompa__2D971CACBEE01668");

                entity.ToTable("tblCompany");

                entity.Property(e => e.CompanyLogoUrl)
                    .HasMaxLength(600)
                    .HasColumnName("CompanyLogoURL");

                entity.Property(e => e.CompanyName).HasMaxLength(150);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .HasMaxLength(120)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Phone)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.RegistrtionNumber).HasMaxLength(50);

                entity.Property(e => e.WebSite)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.BusinessUnit)
                    .WithMany(p => p.TblCompanies)
                    .HasForeignKey(d => d.BusinessUnitId)
                    .HasConstraintName("BusinessUnitId");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.TblCompanyCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("Fk_tblCompany_CreatedBy");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.TblCompanyModifiedByNavigations)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("Fk_tblCompany_ModifiedBy");
            });

            modelBuilder.Entity<TblConfiguration>(entity =>
            {
                entity.HasKey(e => e.ConfigId);

                entity.ToTable("tblConfiguration");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(150);

                entity.Property(e => e.Value).HasMaxLength(600);
            });

            modelBuilder.Entity<TblCreditRequest>(entity =>
            {
                entity.HasKey(e => e.CreditRequestId)
                    .HasName("PK__tblCredi__B1247BE38AEF9EF8");

                entity.ToTable("tblCreditRequests");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.TblCreditRequestCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__tblCredit__Creat__522F1F86");

                entity.HasOne(d => d.CreditRequestApproveUser)
                    .WithMany(p => p.TblCreditRequestCreditRequestApproveUsers)
                    .HasForeignKey(d => d.CreditRequestApproveUserId)
                    .HasConstraintName("FK__tblCredit__Credi__513AFB4D");

                entity.HasOne(d => d.CreditRequestUser)
                    .WithMany(p => p.TblCreditRequestCreditRequestUsers)
                    .HasForeignKey(d => d.CreditRequestUserId)
                    .HasConstraintName("FK__tblCredit__Credi__5046D714");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.TblCreditRequestModifiedByNavigations)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK__tblCredit__Modif__532343BF");

                entity.HasOne(d => d.WalletTransaction)
                    .WithMany(p => p.TblCreditRequests)
                    .HasForeignKey(d => d.WalletTransactionId)
                    .HasConstraintName("FK__tblCredit__Walle__4F52B2DB");
            });

            modelBuilder.Entity<TblCurrentAuthtoken>(entity =>
            {
                entity.HasKey(e => e.CurrentAuthtokenId)
                    .HasName("PK__tblCurre__47705605D050E341");

                entity.ToTable("tblCurrentAuthtoken");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.CurrentAuthToken).HasMaxLength(400);

                entity.Property(e => e.CurrentAuthTokenName).HasMaxLength(400);
            });

            modelBuilder.Entity<TblCustomerDetail>(entity =>
            {
                entity.ToTable("tblCustomerDetails");

                entity.Property(e => e.Address1).HasMaxLength(255);

                entity.Property(e => e.Address2).HasMaxLength(255);

                entity.Property(e => e.AreaLocality).HasMaxLength(255);

                entity.Property(e => e.City).HasMaxLength(255);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(255);

                entity.Property(e => e.FirstName).HasMaxLength(255);

                entity.Property(e => e.LastName).HasMaxLength(255);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.PhoneNumber).HasMaxLength(255);

                entity.Property(e => e.SponsorRefId).HasMaxLength(400);

                entity.Property(e => e.State).HasMaxLength(255);

                entity.Property(e => e.ZipCode).HasMaxLength(255);
            });

            modelBuilder.Entity<TblCustomerFile>(entity =>
            {
                entity.ToTable("tblCustomerFiles");

                entity.Property(e => e.AbbregistrationId).HasColumnName("ABBRegistrationId");

                entity.Property(e => e.CertificatePdfName).HasMaxLength(255);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.FinancialYear).HasMaxLength(100);

                entity.Property(e => e.InvoiceAmount).HasColumnType("decimal(15, 2)");

                entity.Property(e => e.InvoiceDate).HasColumnType("datetime");

                entity.Property(e => e.InvoicePdfName).HasMaxLength(255);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.RegdNo).HasMaxLength(20);

                entity.HasOne(d => d.Abbregistration)
                    .WithMany(p => p.TblCustomerFiles)
                    .HasForeignKey(d => d.AbbregistrationId)
                    .HasConstraintName("Fk_tblCustomerFiles_ABBRegistrationId");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.TblCustomerFileCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("Fk_tblCustomerFiles_CreatedBy");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.TblCustomerFileModifiedByNavigations)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("Fk_tblCustomerFiles_ModifiedBy");

                entity.HasOne(d => d.OrderTrans)
                    .WithMany(p => p.TblCustomerFiles)
                    .HasForeignKey(d => d.OrderTransId)
                    .HasConstraintName("Fk_tblCustomerFiles_OrderTransId");
            });

            modelBuilder.Entity<TblDriverDetail>(entity =>
            {
                entity.HasKey(e => e.DriverDetailsId)
                    .HasName("PK__tblDrive__0DD3C9AE24DDABA5");

                entity.ToTable("tblDriverDetails");

                entity.Property(e => e.ApprovedBy).HasColumnName("approvedBy");

                entity.Property(e => e.City)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("city");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DriverName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.DriverPhoneNumber)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.DriverlicenseImage).IsUnicode(false);

                entity.Property(e => e.DriverlicenseNumber)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.IsApproved).HasColumnName("is_approved");

                entity.Property(e => e.JourneyPlanDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.ProfilePicture).IsUnicode(false);

                entity.Property(e => e.VehicleInsuranceCertificate).IsUnicode(false);

                entity.Property(e => e.VehicleNumber)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.VehicleRcCertificate)
                    .IsUnicode(false)
                    .HasColumnName("Vehicle_RC_Certificate");

                entity.Property(e => e.VehicleRcNumber)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("Vehicle_RC_Number");

                entity.Property(e => e.VehiclefitnessCertificate).IsUnicode(false);

                entity.HasOne(d => d.CityNavigation)
                    .WithMany(p => p.TblDriverDetails)
                    .HasForeignKey(d => d.CityId)
                    .HasConstraintName("Fk_tblDriverDetails_CityId");

                entity.HasOne(d => d.Driver)
                    .WithMany(p => p.TblDriverDetails)
                    .HasForeignKey(d => d.DriverId)
                    .HasConstraintName("FK_tblDriverDetails_DriverId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TblDriverDetails)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__tblDriver__UserI__05C3D225");

                entity.HasOne(d => d.Vehicle)
                    .WithMany(p => p.TblDriverDetails)
                    .HasForeignKey(d => d.VehicleId)
                    .HasConstraintName("FK_tblDriverDetails_VehicleId");
            });

            modelBuilder.Entity<TblDriverList>(entity =>
            {
                entity.HasKey(e => e.DriverId)
                    .HasName("PK__tblDrive__F1B1CD04864B5071");

                entity.ToTable("tblDriverList");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DriverLicenseImage).IsUnicode(false);

                entity.Property(e => e.DriverLicenseNumber)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.DriverName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.DriverPhoneNumber)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.ProfilePicture).IsUnicode(false);

                entity.HasOne(d => d.City)
                    .WithMany(p => p.TblDriverLists)
                    .HasForeignKey(d => d.CityId)
                    .HasConstraintName("FK__tblDriver__CityI__57E7F8DC");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.TblDriverListCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__tblDriver__Creat__58DC1D15");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.TblDriverListModifiedByNavigations)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK__tblDriver__Modif__59D0414E");

                entity.HasOne(d => d.ServicePartner)
                    .WithMany(p => p.TblDriverLists)
                    .HasForeignKey(d => d.ServicePartnerId)
                    .HasConstraintName("FK__tblDriver__Servi__5AC46587");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TblDriverListUsers)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__tblDriver__UserI__56F3D4A3");
            });

            modelBuilder.Entity<TblEntityType>(entity =>
            {
                entity.HasKey(e => e.EntityTypeId)
                    .HasName("PK__tblEntit__E45C98F3F9E5FB33");

                entity.ToTable("tblEntityType");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(600);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.TblEntityTypeCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("Fk_tblEntityType_CreatedBy");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.TblEntityTypeModifiedByNavigations)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("Fk_tblEntityType_ModifiedBy");
            });

            modelBuilder.Entity<TblErrorLog>(entity =>
            {
                entity.HasKey(e => e.ErrorLogId)
                    .HasName("PK__tblError__D65247C2BDC3EBF0");

                entity.ToTable("tblErrorLog");

                entity.Property(e => e.ClassName).HasMaxLength(200);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.MethodName).HasMaxLength(200);

                entity.Property(e => e.SponsorOrderNo).HasMaxLength(100);
            });

            modelBuilder.Entity<TblEvcPartner>(entity =>
            {
                entity.HasKey(e => e.EvcPartnerId)
                    .HasName("PK__tblEvcPa__C65974CC5953EC3B");

                entity.ToTable("tblEvcPartner");

                entity.Property(e => e.Address1).HasMaxLength(255);

                entity.Property(e => e.Address2).HasMaxLength(255);

                entity.Property(e => e.ContactNumber).HasMaxLength(255);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("createdDate");

                entity.Property(e => e.EmailId).HasMaxLength(255);

                entity.Property(e => e.EvcStoreCode).HasMaxLength(255);

                entity.Property(e => e.EvcregistrationId).HasColumnName("EVCRegistrationId");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.PinCode).HasMaxLength(255);

                entity.HasOne(d => d.City)
                    .WithMany(p => p.TblEvcPartners)
                    .HasForeignKey(d => d.CityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblEvcPar__CityI__78BEDCC2");

                entity.HasOne(d => d.CreatedbyNavigation)
                    .WithMany(p => p.TblEvcPartnerCreatedbyNavigations)
                    .HasForeignKey(d => d.Createdby)
                    .HasConstraintName("FK__tblEvcPar__Creat__7AA72534");

                entity.HasOne(d => d.Evcregistration)
                    .WithMany(p => p.TblEvcPartners)
                    .HasForeignKey(d => d.EvcregistrationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblEvcPar__EVCRe__77CAB889");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.TblEvcPartnerModifiedByNavigations)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK__tblEvcPar__Modif__7B9B496D");

                entity.HasOne(d => d.State)
                    .WithMany(p => p.TblEvcPartners)
                    .HasForeignKey(d => d.StateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblEvcPar__State__79B300FB");
            });

            modelBuilder.Entity<TblEvcPartnerPreference>(entity =>
            {
                entity.HasKey(e => e.EvcPartnerpreferenceId)
                    .HasName("PK__tblEvcPa__377C5615291770FF");

                entity.ToTable("tblEvcPartnerPreference");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("createdDate");

                entity.Property(e => e.EvcpartnerId).HasColumnName("EVCPartnerId");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedbyNavigation)
                    .WithMany(p => p.TblEvcPartnerPreferenceCreatedbyNavigations)
                    .HasForeignKey(d => d.Createdby)
                    .HasConstraintName("FK__tblEvcPar__Creat__015422C3");

                entity.HasOne(d => d.Evcpartner)
                    .WithMany(p => p.TblEvcPartnerPreferences)
                    .HasForeignKey(d => d.EvcpartnerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblEvcPar__EVCPa__7E77B618");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.TblEvcPartnerPreferenceModifiedByNavigations)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK__tblEvcPar__Modif__024846FC");

                entity.HasOne(d => d.ProductCat)
                    .WithMany(p => p.TblEvcPartnerPreferences)
                    .HasForeignKey(d => d.ProductCatId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblEvcPar__Produ__7F6BDA51");

                entity.HasOne(d => d.ProductQuality)
                    .WithMany(p => p.TblEvcPartnerPreferences)
                    .HasForeignKey(d => d.ProductQualityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblEvcPar__Produ__005FFE8A");
            });

            modelBuilder.Entity<TblEvcPriceMaster>(entity =>
            {
                entity.HasKey(e => e.EvcpriceMasterId);

                entity.ToTable("tblEvcPriceMaster");

                entity.Property(e => e.EvcpriceMasterId)
                    .ValueGeneratedNever()
                    .HasColumnName("EVCPriceMasterId");

                entity.Property(e => e.EndDate)
                    .HasColumnType("datetime")
                    .HasColumnName("End_Date");

                entity.Property(e => e.EvcP).HasColumnName("EVC_P");

                entity.Property(e => e.EvcQ).HasColumnName("EVC_Q");

                entity.Property(e => e.EvcR).HasColumnName("EVC_R");

                entity.Property(e => e.EvcS).HasColumnName("EVC_S");

                entity.Property(e => e.Lgccost).HasColumnName("LGCCost");

                entity.Property(e => e.ProductGroup).HasMaxLength(255);

                entity.Property(e => e.ProductType).HasMaxLength(255);

                entity.Property(e => e.Size).HasMaxLength(255);

                entity.Property(e => e.SponsorName).HasMaxLength(255);

                entity.Property(e => e.StartDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Start_Date");
            });

            modelBuilder.Entity<TblEvcapproved>(entity =>
            {
                entity.ToTable("tblEVCApproved");

                entity.Property(e => e.BussinessName).HasMaxLength(255);

                entity.Property(e => e.City).HasMaxLength(50);

                entity.Property(e => e.ContactPerson).HasMaxLength(255);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EmailId)
                    .HasMaxLength(255)
                    .HasColumnName("EmailID");

                entity.Property(e => e.EvcmobileNumber)
                    .HasMaxLength(50)
                    .HasColumnName("EVCMobileNumber");

                entity.Property(e => e.EvcregdNo)
                    .HasMaxLength(50)
                    .HasColumnName("EVCRegdNo");

                entity.Property(e => e.EvcwalletAmount)
                    .HasMaxLength(50)
                    .HasColumnName("EVCWalletAmount");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.PinCode).HasMaxLength(50);

                entity.Property(e => e.RegdAddressLine1).HasMaxLength(255);

                entity.Property(e => e.RegdAddressLine2).HasMaxLength(255);

                entity.Property(e => e.State).HasMaxLength(50);

                entity.Property(e => e.UploadGstregistration).HasColumnName("UploadGSTRegistration");

                entity.Property(e => e.ZohoEvcapprovedId)
                    .HasMaxLength(255)
                    .HasColumnName("ZohoEVCApprovedId");
            });

            modelBuilder.Entity<TblEvcdispute>(entity =>
            {
                entity.HasKey(e => e.EvcdisputeId);

                entity.ToTable("tblEVCDispute");

                entity.Property(e => e.EvcdisputeId).HasColumnName("EVCDisputeId");

                entity.Property(e => e.Amount).HasColumnType("decimal(15, 2)");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Digi2Lresponse)
                    .IsUnicode(false)
                    .HasColumnName("Digi2LResponse");

                entity.Property(e => e.DisputeRegno).HasMaxLength(100);

                entity.Property(e => e.Evcdisputedescription)
                    .IsUnicode(false)
                    .HasColumnName("EVCDisputedescription");

                entity.Property(e => e.EvcregistrationId).HasColumnName("EVCRegistrationId");

                entity.Property(e => e.Image1)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Image2)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Image3)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Image4)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.LevelStatus).HasMaxLength(50);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Status).HasMaxLength(50);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.TblEvcdisputeCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("Fk_tblEVCDispute_CreatedBy");

                entity.HasOne(d => d.Evcregistration)
                    .WithMany(p => p.TblEvcdisputes)
                    .HasForeignKey(d => d.EvcregistrationId)
                    .HasConstraintName("Fk_tblEVCDispute_EVCRegistrationId");

                entity.HasOne(d => d.ExchangeOrder)
                    .WithMany(p => p.TblEvcdisputes)
                    .HasForeignKey(d => d.ExchangeOrderId)
                    .HasConstraintName("Fk_tblEVCDispute_ExchangeOrderId");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.TblEvcdisputeModifiedByNavigations)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("Fk_tblEVCDispute_ModifiedBy");

                entity.HasOne(d => d.OrderTrans)
                    .WithMany(p => p.TblEvcdisputes)
                    .HasForeignKey(d => d.OrderTransId)
                    .HasConstraintName("FK_OrderTransId");
            });

            modelBuilder.Entity<TblEvcpoddetail>(entity =>
            {
                entity.ToTable("tblEVCPODDetails");

                entity.Property(e => e.AbbredemptionId).HasColumnName("ABBRedemptionId");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DebitNoteAmount).HasColumnType("decimal(15, 2)");

                entity.Property(e => e.DebitNoteDate).HasColumnType("datetime");

                entity.Property(e => e.DebitNotePdfName).HasMaxLength(255);

                entity.Property(e => e.DnsrNum).HasColumnName("DNSrNum");

                entity.Property(e => e.Evcid).HasColumnName("EVCId");

                entity.Property(e => e.EvcpartnerId).HasColumnName("EVCPartnerId");

                entity.Property(e => e.FinancialYear).HasMaxLength(100);

                entity.Property(e => e.InvoiceAmount).HasColumnType("decimal(15, 2)");

                entity.Property(e => e.InvoiceDate).HasColumnType("datetime");

                entity.Property(e => e.InvoicePdfName).HasMaxLength(255);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Podurl)
                    .HasMaxLength(600)
                    .HasColumnName("PODURL");

                entity.Property(e => e.RegdNo).HasMaxLength(20);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.TblEvcpoddetailCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("Fk_tblEVCPODDetails_CreatedBy");

                entity.HasOne(d => d.Evc)
                    .WithMany(p => p.TblEvcpoddetails)
                    .HasForeignKey(d => d.Evcid)
                    .HasConstraintName("Fk_tblEVCPODDetails_EVCId");

                entity.HasOne(d => d.Evcpartner)
                    .WithMany(p => p.TblEvcpoddetails)
                    .HasForeignKey(d => d.EvcpartnerId)
                    .HasConstraintName("Fk_tblEVCPODDetails_EVCPartnerId");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.TblEvcpoddetailModifiedByNavigations)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("Fk_tblEVCPODDetails_ModifiedBy");

                entity.HasOne(d => d.OrderTrans)
                    .WithMany(p => p.TblEvcpoddetails)
                    .HasForeignKey(d => d.OrderTransId)
                    .HasConstraintName("Fk_tblEVCPODDetails_OrderTransId");
            });

            modelBuilder.Entity<TblEvcpriceRangeMaster>(entity =>
            {
                entity.HasKey(e => e.EvcpriceRangeMasterId);

                entity.ToTable("tblEVCPriceRangeMaster");

                entity.Property(e => e.EvcpriceRangeMasterId).HasColumnName("EVCPriceRangeMasterId");

                entity.Property(e => e.BusinessUnitId).HasColumnName("BusinessUnitID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EvcApplicablePercentage)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("EVC_Applicable_Percentage");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.PriceEndRange).HasColumnName("Price_End_Range");

                entity.Property(e => e.PriceStartRange).HasColumnName("Price_Start_Range");
            });

            modelBuilder.Entity<TblEvcregistration>(entity =>
            {
                entity.HasKey(e => e.EvcregistrationId)
                    .HasName("PK_tblEVC_Registration");

                entity.ToTable("tblEVCRegistration");

                entity.Property(e => e.EvcregistrationId).HasColumnName("EVCRegistrationId");

                entity.Property(e => e.AadharBackImage)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.AadharfrontImage)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.AccountHolder)
                    .HasMaxLength(250)
                    .HasColumnName("accountHolder");

                entity.Property(e => e.AlternateMobileNumber).HasMaxLength(50);

                entity.Property(e => e.ApproverName)
                    .HasMaxLength(250)
                    .HasColumnName("approverName");

                entity.Property(e => e.BankAccountNo).HasMaxLength(600);

                entity.Property(e => e.BankName).HasMaxLength(600);

                entity.Property(e => e.Branch)
                    .HasMaxLength(250)
                    .HasColumnName("branch");

                entity.Property(e => e.BussinessName).HasMaxLength(255);

                entity.Property(e => e.CompanyRegNo)
                    .HasMaxLength(250)
                    .HasColumnName("companyRegNo");

                entity.Property(e => e.ContactPerson).HasMaxLength(255);

                entity.Property(e => e.Country).HasMaxLength(250);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.EmailId)
                    .HasMaxLength(255)
                    .HasColumnName("EmailID");

                entity.Property(e => e.EvcapprovalStatusId).HasColumnName("EVCApprovalStatusId");

                entity.Property(e => e.EvcmobileNumber)
                    .HasMaxLength(50)
                    .HasColumnName("EVCMobileNumber");

                entity.Property(e => e.EvcregdNo)
                    .HasMaxLength(50)
                    .HasColumnName("EVCRegdNo");

                entity.Property(e => e.EvcwalletAmount)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("EVCWalletAmount");

                entity.Property(e => e.EvczohoBookName)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("EVCZohoBookName");

                entity.Property(e => e.EwasteCertificate)
                    .HasMaxLength(300)
                    .HasColumnName("EWasteCertificate");

                entity.Property(e => e.EwasteRegistrationNumber)
                    .HasMaxLength(100)
                    .HasColumnName("EWasteRegistrationNumber");

                entity.Property(e => e.Gstno)
                    .HasMaxLength(255)
                    .HasColumnName("GSTNo");

                entity.Property(e => e.GsttypeId).HasColumnName("GSTTypeId");

                entity.Property(e => e.IconfirmTermsCondition).HasColumnName("IConfirmTermsCondition");

                entity.Property(e => e.Ifsccode)
                    .HasMaxLength(600)
                    .HasColumnName("IFSCCODE");

                entity.Property(e => e.InsertOtp).HasColumnName("InsertOTP");

                entity.Property(e => e.Isevcapprovrd).HasColumnName("ISEVCApprovrd");

                entity.Property(e => e.ManagerContact)
                    .HasMaxLength(250)
                    .HasColumnName("managerContact");

                entity.Property(e => e.ManagerEmail)
                    .HasMaxLength(250)
                    .HasColumnName("managerEmail");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.NatureOfBusiness).HasMaxLength(250);

                entity.Property(e => e.PanNo)
                    .HasMaxLength(250)
                    .HasColumnName("panNo");

                entity.Property(e => e.PinCode).HasMaxLength(50);

                entity.Property(e => e.Pocname)
                    .HasMaxLength(100)
                    .HasColumnName("POCName");

                entity.Property(e => e.Pocplace)
                    .HasMaxLength(100)
                    .HasColumnName("POCPlace");

                entity.Property(e => e.ProfilePic)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.RegdAddressLine1).HasMaxLength(255);

                entity.Property(e => e.RegdAddressLine2).HasMaxLength(255);

                entity.Property(e => e.UnitDepartment)
                    .HasMaxLength(250)
                    .HasColumnName("unitDepartment");

                entity.Property(e => e.UploadGstregistration).HasColumnName("UploadGSTRegistration");

                entity.Property(e => e.UtcEmployeeContact)
                    .HasMaxLength(250)
                    .HasColumnName("utcEmployeeContact");

                entity.Property(e => e.UtcEmployeeEmail)
                    .HasMaxLength(250)
                    .HasColumnName("utcEmployeeEmail");

                entity.Property(e => e.UtcEmployeeName)
                    .HasMaxLength(250)
                    .HasColumnName("utcEmployeeName");

                entity.Property(e => e.ZohoEvcapprovedId)
                    .HasMaxLength(255)
                    .HasColumnName("ZohoEVCApprovedId");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.TblEvcregistrations)
                    .HasForeignKey(d => d.CityId)
                    .HasConstraintName("Fk_tblEVCRegistration_CityId");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.TblEvcregistrationCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("Fk_tblEVCRegistration_CreatedBy");

                entity.HasOne(d => d.EntityType)
                    .WithMany(p => p.TblEvcregistrations)
                    .HasForeignKey(d => d.EntityTypeId)
                    .HasConstraintName("Fk_tblEVCRegistration_EntityTypeId");

                entity.HasOne(d => d.Gsttype)
                    .WithMany(p => p.TblEvcregistrations)
                    .HasForeignKey(d => d.GsttypeId)
                    .HasConstraintName("Fk_tblEVCRegistration_GSTTypeId");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.TblEvcregistrationModifiedByNavigations)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("Fk_tblEVCRegistration_ModifiedBy");

                entity.HasOne(d => d.State)
                    .WithMany(p => p.TblEvcregistrations)
                    .HasForeignKey(d => d.StateId)
                    .HasConstraintName("Fk_tblEVCRegistration_StateId");
            });

            modelBuilder.Entity<TblEvcwalletAddition>(entity =>
            {
                entity.HasKey(e => e.EvcwalletAdditionId)
                    .HasName("PK_EVCWalletAddition");

                entity.ToTable("tblEVCWalletAddition");

                entity.Property(e => e.EvcwalletAdditionId).HasColumnName("EVCWalletAdditionId");

                entity.Property(e => e.Amount).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Comments)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EvcregistrationId).HasColumnName("EVCRegistrationId");

                entity.Property(e => e.InvoiceImage)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.TransactionDate).HasColumnType("datetime");

                entity.Property(e => e.TransactionId)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.TblEvcwalletAdditionCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("Fk_tblEVCWalletAddition_CreatedBy");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.TblEvcwalletAdditionModifiedByNavigations)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("Fk_tblEVCWalletAddition_ModifiedBy");
            });

            modelBuilder.Entity<TblEvcwalletHistory>(entity =>
            {
                entity.HasKey(e => e.WalletHistoryId);

                entity.ToTable("TblEVCWalletHistory");

                entity.Property(e => e.AddAmount).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.BalanceWalletAmount).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.CurrentWalletAmount).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.EvcpartnerId).HasColumnName("EVCPartnerId");

                entity.Property(e => e.EvcregistrationId).HasColumnName("EVCRegistrationId");

                entity.Property(e => e.FinalOrderAmount).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.TransactionId)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.TblEvcwalletHistoryCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("Fk_tblEVCWalletHistory_CreatedBy");

                entity.HasOne(d => d.Evcpartner)
                    .WithMany(p => p.TblEvcwalletHistories)
                    .HasForeignKey(d => d.EvcpartnerId)
                    .HasConstraintName("FK__TblEVCWal__EVCPa__04308F6E");

                entity.HasOne(d => d.Evcregistration)
                    .WithMany(p => p.TblEvcwalletHistories)
                    .HasForeignKey(d => d.EvcregistrationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fk_tblEVCWalletHistory_EVCRegistrationId");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.TblEvcwalletHistoryModifiedByNavigations)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("Fk_tblEVCWalletHistory_ModifiedBy");

                entity.HasOne(d => d.OrderTrans)
                    .WithMany(p => p.TblEvcwalletHistories)
                    .HasForeignKey(d => d.OrderTransId)
                    .HasConstraintName("FK__TblEVCWal__Order__1293BD5E");
            });

            modelBuilder.Entity<TblEvcwalletStatus>(entity =>
            {
                entity.HasKey(e => e.WalletStatusId);

                entity.ToTable("tblEVCWalletStatus");

                entity.Property(e => e.EvcregistrationId).HasColumnName("EVCRegistrationId");

                entity.Property(e => e.RegdNo).HasMaxLength(150);
            });

            modelBuilder.Entity<TblExchangeAbbstatusHistory>(entity =>
            {
                entity.HasKey(e => e.StatusHistoryId)
                    .HasName("PK_tblStatusHistory");

                entity.ToTable("tblExchangeABBStatusHistory");

                entity.Property(e => e.Comment).HasMaxLength(500);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Evcid).HasColumnName("EVCId");

                entity.Property(e => e.JsonObjectString).IsUnicode(false);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.RegdNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SponsorOrderNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ZohoSponsorId).HasMaxLength(100);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.TblExchangeAbbstatusHistoryCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__tblExchan__Creat__52AE4273");

                entity.HasOne(d => d.DriverDetail)
                    .WithMany(p => p.TblExchangeAbbstatusHistories)
                    .HasForeignKey(d => d.DriverDetailId)
                    .HasConstraintName("FK__tblExchan__Drive__3D9E16F4");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.TblExchangeAbbstatusHistoryModifiedByNavigations)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("Fk_tblExchangeABBStatusHistory_ModifiedBy");

                entity.HasOne(d => d.OrderTrans)
                    .WithMany(p => p.TblExchangeAbbstatusHistories)
                    .HasForeignKey(d => d.OrderTransId)
                    .HasConstraintName("FK__tblExchan__Order__4EDDB18F");

                entity.HasOne(d => d.Servicepartner)
                    .WithMany(p => p.TblExchangeAbbstatusHistories)
                    .HasForeignKey(d => d.ServicepartnerId)
                    .HasConstraintName("FK__tblExchan__Servi__3CA9F2BB");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.TblExchangeAbbstatusHistories)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("FK__tblExchan__Statu__4FD1D5C8");
            });

            modelBuilder.Entity<TblExchangeOrder>(entity =>
            {
                entity.ToTable("tblExchangeOrder");

                entity.Property(e => e.BaseExchangePrice).HasColumnType("decimal(15, 2)");

                entity.Property(e => e.Bonus).HasMaxLength(50);

                entity.Property(e => e.Comment1)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.Comment2)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.Comment3)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.CompanyName).HasMaxLength(255);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EmployeeId)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.EndTime).HasMaxLength(15);

                entity.Property(e => e.EstimatedDeliveryDate).HasMaxLength(255);

                entity.Property(e => e.ExchPriceCode).HasMaxLength(50);

                entity.Property(e => e.ExchangePrice).HasColumnType("decimal(15, 2)");

                entity.Property(e => e.FinalExchangePrice).HasColumnType("decimal(15, 2)");

                entity.Property(e => e.InvoiceNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IsUnInstallationRequired).HasMaxLength(5);

                entity.Property(e => e.LoginId).HasColumnName("LoginID");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.OrderStatus).HasMaxLength(255);

                entity.Property(e => e.ProductCondition).HasMaxLength(50);

                entity.Property(e => e.ProductNumber).HasMaxLength(200);

                entity.Property(e => e.PurchasedProductCategory).HasMaxLength(300);

                entity.Property(e => e.Qcdate)
                    .HasMaxLength(20)
                    .HasColumnName("QCDate");

                entity.Property(e => e.RegdNo).HasMaxLength(15);

                entity.Property(e => e.SaleAssociateCode).HasMaxLength(100);

                entity.Property(e => e.SaleAssociateName).HasMaxLength(200);

                entity.Property(e => e.SalesAssociateEmail).HasMaxLength(500);

                entity.Property(e => e.SalesAssociatePhone).HasMaxLength(50);

                entity.Property(e => e.SerialNumber)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.SponsorOrderNumber).HasMaxLength(255);

                entity.Property(e => e.SponsorServiceRefId).HasMaxLength(400);

                entity.Property(e => e.StartTime).HasMaxLength(15);

                entity.Property(e => e.StoreCode).HasMaxLength(300);

                entity.Property(e => e.Sweetener).HasColumnType("decimal(15, 2)");

                entity.Property(e => e.SweetenerBp)
                    .HasColumnType("decimal(15, 2)")
                    .HasColumnName("SweetenerBP");

                entity.Property(e => e.SweetenerBu)
                    .HasColumnType("decimal(15, 2)")
                    .HasColumnName("SweetenerBU");

                entity.Property(e => e.SweetenerDigi2l).HasColumnType("decimal(15, 2)");

                entity.Property(e => e.UnInstallationPrice).HasColumnType("decimal(15, 2)");

                entity.Property(e => e.VoucherCode).HasMaxLength(200);

                entity.Property(e => e.VoucherCodeExpDate).HasColumnType("datetime");

                entity.Property(e => e.ZohoSponsorOrderId).HasMaxLength(255);

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.TblExchangeOrders)
                    .HasForeignKey(d => d.BrandId)
                    .HasConstraintName("FK__tblSponso__Brand__47DBAE45");

                entity.HasOne(d => d.BusinessPartner)
                    .WithMany(p => p.TblExchangeOrders)
                    .HasForeignKey(d => d.BusinessPartnerId)
                    .HasConstraintName("FK_tblExchangeOrder_BusinessPartnerId");

                entity.HasOne(d => d.BusinessUnit)
                    .WithMany(p => p.TblExchangeOrders)
                    .HasForeignKey(d => d.BusinessUnitId)
                    .HasConstraintName("FK__tblExchan__Busin__1C3D2329");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.TblExchangeOrderCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("Fk_tblExchangeOrder_CreatedBy");

                entity.HasOne(d => d.CustomerDetails)
                    .WithMany(p => p.TblExchangeOrders)
                    .HasForeignKey(d => d.CustomerDetailsId)
                    .HasConstraintName("FK__tblSponso__Custo__48CFD27E");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.TblExchangeOrderModifiedByNavigations)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("Fk_tblExchangeOrder_ModifiedBy");

                entity.HasOne(d => d.NewProductType)
                    .WithMany(p => p.TblExchangeOrderNewProductTypes)
                    .HasForeignKey(d => d.NewProductTypeId)
                    .HasConstraintName("FK__tblExchan__NewPr__6B0FDBE9");

                entity.HasOne(d => d.PriceMasterName)
                    .WithMany(p => p.TblExchangeOrders)
                    .HasForeignKey(d => d.PriceMasterNameId)
                    .HasConstraintName("FK__tblExchan__Price__2D32A501");

                entity.HasOne(d => d.ProductTechnology)
                    .WithMany(p => p.TblExchangeOrders)
                    .HasForeignKey(d => d.ProductTechnologyId)
                    .HasConstraintName("FK__tblExchan__Produ__58BC2184");

                entity.HasOne(d => d.ProductType)
                    .WithMany(p => p.TblExchangeOrderProductTypes)
                    .HasForeignKey(d => d.ProductTypeId)
                    .HasConstraintName("FK__tblExchan__Produ__664B26CC");

                entity.HasOne(d => d.Society)
                    .WithMany(p => p.TblExchangeOrders)
                    .HasForeignKey(d => d.SocietyId)
                    .HasConstraintName("FK_tblExchangeOrder_SocietyId");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.TblExchangeOrders)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("FK__tblExchan__Statu__797309D9");

                entity.HasOne(d => d.VoucherStatus)
                    .WithMany(p => p.TblExchangeOrders)
                    .HasForeignKey(d => d.VoucherStatusId)
                    .HasConstraintName("FK__tblExchan__Vouch__17F790F9");
            });

            modelBuilder.Entity<TblExchangeOrderStatus>(entity =>
            {
                entity.ToTable("tblExchangeOrderStatus");

                entity.Property(e => e.StatusCode).HasMaxLength(5);

                entity.Property(e => e.StatusDescription).HasMaxLength(255);

                entity.Property(e => e.StatusName).HasMaxLength(30);
            });

            modelBuilder.Entity<TblFeedBack>(entity =>
            {
                entity.ToTable("tblFeedBack");

                entity.Property(e => e.CreatedBy).HasMaxLength(100);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.RatingNo).HasMaxLength(5);

                entity.HasOne(d => d.Answer)
                    .WithMany(p => p.TblFeedBacks)
                    .HasForeignKey(d => d.AnswerId)
                    .HasConstraintName("FK__tblFeedBa__Answe__2645B050");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.TblFeedBacks)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK__tblFeedBa__Custo__2739D489");

                entity.HasOne(d => d.ExchangeOrder)
                    .WithMany(p => p.TblFeedBacks)
                    .HasForeignKey(d => d.ExchangeOrderId)
                    .HasConstraintName("FK__tblFeedBa__Excha__29221CFB");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.TblFeedBacks)
                    .HasForeignKey(d => d.QuestionId)
                    .HasConstraintName("FK__tblFeedBa__Quest__25518C17");
            });

            modelBuilder.Entity<TblFeedBackAnswer>(entity =>
            {
                entity.ToTable("tblFeedBackAnswers");

                entity.Property(e => e.Answers).HasMaxLength(300);

                entity.Property(e => e.CreatedBy).HasMaxLength(100);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.TblFeedBackAnswers)
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("FK__tblFeedBa__Paren__282DF8C2");
            });

            modelBuilder.Entity<TblFeedBackQuestion>(entity =>
            {
                entity.ToTable("tblFeedBackQuestions");

                entity.Property(e => e.CreatedBy).HasMaxLength(100);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Question).HasMaxLength(300);
            });

            modelBuilder.Entity<TblHistory>(entity =>
            {
                entity.ToTable("tblHistory");

                entity.Property(e => e.Createdate)
                    .HasColumnType("datetime")
                    .HasColumnName("createdate");

                entity.Property(e => e.ExchangeAmount).HasColumnType("decimal(15, 2)");

                entity.Property(e => e.Modifieddate)
                    .HasColumnType("datetime")
                    .HasColumnName("modifieddate");

                entity.Property(e => e.RegdNo).HasMaxLength(250);

                entity.Property(e => e.Sweetner).HasColumnType("decimal(15, 2)");

                entity.Property(e => e.VoucherCode).HasMaxLength(255);

                entity.HasOne(d => d.Cust)
                    .WithMany(p => p.TblHistories)
                    .HasForeignKey(d => d.CustId)
                    .HasConstraintName("FK__tblHistor__CustI__55BFB948");

                entity.HasOne(d => d.ExchangeOrder)
                    .WithMany(p => p.TblHistories)
                    .HasForeignKey(d => d.ExchangeOrderId)
                    .HasConstraintName("FK__tblHistor__Excha__54CB950F");
            });

            modelBuilder.Entity<TblImage>(entity =>
            {
                entity.ToTable("tblImages");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ImageUrl)
                    .HasColumnType("text")
                    .HasColumnName("ImageURL");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.BizlogTicket)
                    .WithMany(p => p.TblImages)
                    .HasForeignKey(d => d.BizlogTicketId)
                    .HasConstraintName("FK__tblImages__Bizlo__44FF419A");

                entity.HasOne(d => d.Sponsor)
                    .WithMany(p => p.TblImages)
                    .HasForeignKey(d => d.SponsorId)
                    .HasConstraintName("FK__tblImages__Spons__45F365D3");
            });

            modelBuilder.Entity<TblImageLabelMaster>(entity =>
            {
                entity.HasKey(e => e.ImageLabelid)
                    .HasName("PK__tblImage__69D1D32288A0CE50");

                entity.ToTable("tblImageLabelMaster");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ImagePlaceHolder).HasMaxLength(600);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.ProductImageDescription)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ProductImageLabel)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ProductName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.TblImageLabelMasterCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("Fk_tblImageLabelMaster_CreatedBy");

                entity.HasOne(d => d.ModifiedbyNavigation)
                    .WithMany(p => p.TblImageLabelMasterModifiedbyNavigations)
                    .HasForeignKey(d => d.Modifiedby)
                    .HasConstraintName("Fk_tblImageLabelMaster_ModifiedBy");

                entity.HasOne(d => d.ProductCat)
                    .WithMany(p => p.TblImageLabelMasters)
                    .HasForeignKey(d => d.ProductCatId)
                    .HasConstraintName("FK__tblImageL__Produ__77DFC722");

                entity.HasOne(d => d.ProductType)
                    .WithMany(p => p.TblImageLabelMasters)
                    .HasForeignKey(d => d.ProductTypeId)
                    .HasConstraintName("FK__tblImageL__Produ__5CC1BC92");
            });

            modelBuilder.Entity<TblLoV>(entity =>
            {
                entity.HasKey(e => e.LoVid);

                entity.ToTable("tblLoV");

                entity.Property(e => e.LoVid).HasColumnName("LoVId");

                entity.Property(e => e.CreatedDate).HasColumnType("date");

                entity.Property(e => e.LoVname)
                    .HasMaxLength(50)
                    .HasColumnName("LoVName");

                entity.Property(e => e.ModifiedDate).HasColumnType("date");
            });

            modelBuilder.Entity<TblLoginMobile>(entity =>
            {
                entity.ToTable("tblLogin_Mobile");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DeviceType)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.MobileNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.UserDeviceId).IsUnicode(false);

                entity.Property(e => e.UserRoleName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("username");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TblLoginMobiles)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__tblLogin___UserI__70FDBF69");
            });

            modelBuilder.Entity<TblLogistic>(entity =>
            {
                entity.HasKey(e => e.LogisticId)
                    .HasName("PK__tblLogis__FAAA72862B8351BE");

                entity.ToTable("tblLogistics");

                entity.Property(e => e.AmtPaybleThroughLgc)
                    .HasColumnType("decimal(15, 2)")
                    .HasColumnName("AmtPaybleThroughLGC");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DriverDetailsId).HasColumnName("driverDetailsId");

                entity.Property(e => e.IsOrderAcceptedByDriver).HasColumnName("isOrderAcceptedByDriver");

                entity.Property(e => e.IsOrderRejectedByDriver).HasColumnName("isOrderRejectedByDriver");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.PickupScheduleDate).HasColumnType("datetime");

                entity.Property(e => e.RegdNo)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.RescheduleComment)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Rescheduledate).HasColumnType("datetime");

                entity.Property(e => e.TicketNumber)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.TblLogisticCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("Fk_tblLogistics_CreatedBy");

                entity.HasOne(d => d.DriverDetails)
                    .WithMany(p => p.TblLogistics)
                    .HasForeignKey(d => d.DriverDetailsId)
                    .HasConstraintName("FK__tblLogist__drive__7775B2CE");

                entity.HasOne(d => d.ModifiedbyNavigation)
                    .WithMany(p => p.TblLogisticModifiedbyNavigations)
                    .HasForeignKey(d => d.Modifiedby)
                    .HasConstraintName("Fk_tblLogistics_ModifiedBy");

                entity.HasOne(d => d.OrderTrans)
                    .WithMany(p => p.TblLogistics)
                    .HasForeignKey(d => d.OrderTransId)
                    .HasConstraintName("FK__tblLogist__Order__7869D707");

                entity.HasOne(d => d.ServicePartner)
                    .WithMany(p => p.TblLogistics)
                    .HasForeignKey(d => d.ServicePartnerId)
                    .HasConstraintName("FK__tblLogist__Servi__795DFB40");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.TblLogistics)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("FK__tblLogist__Statu__000AF8CF");
            });

            modelBuilder.Entity<TblMahindraLogistic>(entity =>
            {
                entity.ToTable("tblMahindraLogistics");

                entity.Property(e => e.AwbNumber)
                    .HasMaxLength(100)
                    .HasColumnName("awbNumber");

                entity.Property(e => e.City)
                    .HasMaxLength(250)
                    .HasColumnName("city");

                entity.Property(e => e.CityPickup)
                    .HasMaxLength(100)
                    .HasColumnName("city_pickup");

                entity.Property(e => e.ClientOrderId)
                    .HasMaxLength(150)
                    .HasColumnName("client_order_id");

                entity.Property(e => e.DeliveryAddress).HasColumnName("deliveryAddress");

                entity.Property(e => e.Email)
                    .HasMaxLength(500)
                    .HasColumnName("email");

                entity.Property(e => e.EmailNone)
                    .HasMaxLength(100)
                    .HasColumnName("email_none");

                entity.Property(e => e.EmailPickup)
                    .HasMaxLength(500)
                    .HasColumnName("email_pickup");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(250)
                    .HasColumnName("first_name");

                entity.Property(e => e.FirstNameNone)
                    .HasMaxLength(100)
                    .HasColumnName("first_name_none");

                entity.Property(e => e.FirstNamePickup)
                    .HasMaxLength(100)
                    .HasColumnName("first_name_pickup");

                entity.Property(e => e.LastName)
                    .HasMaxLength(100)
                    .HasColumnName("last_name");

                entity.Property(e => e.LastNameNone)
                    .HasMaxLength(100)
                    .HasColumnName("last_name_none");

                entity.Property(e => e.LastNamePickup)
                    .HasMaxLength(100)
                    .HasColumnName("last_name_pickup");

                entity.Property(e => e.Latitude)
                    .HasMaxLength(50)
                    .HasColumnName("latitude");

                entity.Property(e => e.LatitudePickup)
                    .HasMaxLength(50)
                    .HasColumnName("latitude_pickup");

                entity.Property(e => e.Longitude)
                    .HasMaxLength(50)
                    .HasColumnName("longitude");

                entity.Property(e => e.LongitudePickup)
                    .HasMaxLength(50)
                    .HasColumnName("longitude_pickup");

                entity.Property(e => e.Name)
                    .HasMaxLength(250)
                    .HasColumnName("name");

                entity.Property(e => e.OrderDeliveryDate)
                    .HasMaxLength(40)
                    .HasColumnName("order_delivery_date");

                entity.Property(e => e.OrderId)
                    .HasMaxLength(100)
                    .HasColumnName("orderId");

                entity.Property(e => e.PickUpAddress).HasColumnName("pickUpAddress");

                entity.Property(e => e.PricePerEachItem)
                    .HasMaxLength(100)
                    .HasColumnName("price_per_each_item");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.Property(e => e.Sku)
                    .HasMaxLength(100)
                    .HasColumnName("sku");

                entity.Property(e => e.State)
                    .HasMaxLength(100)
                    .HasColumnName("state");

                entity.Property(e => e.StatePickup)
                    .HasMaxLength(100)
                    .HasColumnName("state_pickup");

                entity.Property(e => e.StreetAddress).HasColumnName("street_address");

                entity.Property(e => e.StreetAddressPickup).HasColumnName("street_address_pickup");

                entity.Property(e => e.Telephone)
                    .HasMaxLength(50)
                    .HasColumnName("telephone");

                entity.Property(e => e.TelephoneNone)
                    .HasMaxLength(100)
                    .HasColumnName("telephone_none");

                entity.Property(e => e.TelephonePickup)
                    .HasMaxLength(50)
                    .HasColumnName("telephone_pickup");

                entity.Property(e => e.TotalPrice)
                    .HasColumnType("decimal(15, 2)")
                    .HasColumnName("total_price");

                entity.Property(e => e.WmsOrderId)
                    .HasMaxLength(100)
                    .HasColumnName("wmsOrderId");

                entity.Property(e => e.Zipcode)
                    .HasMaxLength(100)
                    .HasColumnName("zipcode");

                entity.Property(e => e.ZipcodePickup)
                    .HasMaxLength(10)
                    .HasColumnName("zipcode_pickup");
            });

            modelBuilder.Entity<TblMessageDetail>(entity =>
            {
                entity.HasKey(e => e.MessageDetailId)
                    .HasName("PK__tblMessa__64D41BD77C5E7BB0");

                entity.ToTable("tblMessageDetail");

                entity.Property(e => e.Code).HasMaxLength(10);

                entity.Property(e => e.Email).HasMaxLength(200);

                entity.Property(e => e.Message).HasMaxLength(2000);

                entity.Property(e => e.PhoneNumber).HasMaxLength(15);

                entity.Property(e => e.ResponseJson)
                    .HasMaxLength(600)
                    .HasColumnName("ResponseJSON");

                entity.Property(e => e.SendDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<TblModelMapping>(entity =>
            {
                entity.ToTable("tblModelMapping");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.SweetenerBp)
                    .HasColumnType("decimal(15, 2)")
                    .HasColumnName("SweetenerBP");

                entity.Property(e => e.SweetenerBu).HasColumnType("decimal(15, 2)");

                entity.Property(e => e.SweetenerDigi2l).HasColumnType("decimal(15, 2)");

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.TblModelMappings)
                    .HasForeignKey(d => d.BrandId)
                    .HasConstraintName("FK__tblModelM__Brand__5A3A55A2");

                entity.HasOne(d => d.BusinessPartner)
                    .WithMany(p => p.TblModelMappings)
                    .HasForeignKey(d => d.BusinessPartnerId)
                    .HasConstraintName("FK__tblModelM__Busin__60E75331");

                entity.HasOne(d => d.BusinessUnit)
                    .WithMany(p => p.TblModelMappings)
                    .HasForeignKey(d => d.BusinessUnitId)
                    .HasConstraintName("FK__tblModelM__Busin__5FF32EF8");

                entity.HasOne(d => d.Model)
                    .WithMany(p => p.TblModelMappings)
                    .HasForeignKey(d => d.ModelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblModelM__Model__5D16C24D");
            });

            modelBuilder.Entity<TblModelNumber>(entity =>
            {
                entity.HasKey(e => e.ModelNumberId)
                    .HasName("PK__tblModelNo__3214EC07BF1A99F2");

                entity.ToTable("tblModelNumber");

                entity.Property(e => e.Code).HasMaxLength(255);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.IsAbb).HasColumnName("IsABB");

                entity.Property(e => e.ModelName).HasMaxLength(255);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.SweetenerBp)
                    .HasColumnType("decimal(15, 2)")
                    .HasColumnName("SweetenerBP");

                entity.Property(e => e.SweetenerBu)
                    .HasColumnType("decimal(15, 2)")
                    .HasColumnName("SweetenerBU");

                entity.Property(e => e.SweetenerDigi2l).HasColumnType("decimal(15, 2)");

                entity.Property(e => e.SweetnerForDtc)
                    .HasColumnType("decimal(15, 2)")
                    .HasColumnName("SweetnerForDTC");

                entity.Property(e => e.SweetnerForDtd)
                    .HasColumnType("decimal(15, 2)")
                    .HasColumnName("SweetnerForDTD");

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.TblModelNumbers)
                    .HasForeignKey(d => d.BrandId)
                    .HasConstraintName("FK__tblModelNo__Brand__2B0A656D");

                entity.HasOne(d => d.BusinessPartner)
                    .WithMany(p => p.TblModelNumbers)
                    .HasForeignKey(d => d.BusinessPartnerId)
                    .HasConstraintName("FK__tblModelN__Busin__1D314762");

                entity.HasOne(d => d.BusinessUnit)
                    .WithMany(p => p.TblModelNumbers)
                    .HasForeignKey(d => d.BusinessUnitId)
                    .HasConstraintName("FK__tblModelN__Busin__5C6CB6D7");

                entity.HasOne(d => d.ProductCategory)
                    .WithMany(p => p.TblModelNumbers)
                    .HasForeignKey(d => d.ProductCategoryId)
                    .HasConstraintName("FK__tblModelNo__ProductCat__2B0A656D");

                entity.HasOne(d => d.ProductType)
                    .WithMany(p => p.TblModelNumbers)
                    .HasForeignKey(d => d.ProductTypeId)
                    .HasConstraintName("FK__tblModelN__Produ__5DB5E0CB");
            });

            modelBuilder.Entity<TblNpssqoption>(entity =>
            {
                entity.HasKey(e => e.NpssqoptionId)
                    .HasName("PK__tblNPSSQ__5630CBF72DD1647D");

                entity.ToTable("tblNPSSQOptions");

                entity.Property(e => e.NpssqoptionId).HasColumnName("NPSSQOptionId");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.NpssquestionId).HasColumnName("NPSSQuestionId");

                entity.Property(e => e.Option).HasMaxLength(255);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.TblNpssqoptionCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("Fk_tblNPSSQOptions_CreatedBy");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.TblNpssqoptionModifiedByNavigations)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("Fk_tblNPSSQOptions_ModifiedBy");

                entity.HasOne(d => d.Npssquestion)
                    .WithMany(p => p.TblNpssqoptions)
                    .HasForeignKey(d => d.NpssquestionId)
                    .HasConstraintName("Fk_tblNPSSQOptions_NPSSQuestionId");
            });

            modelBuilder.Entity<TblNpssqresponse>(entity =>
            {
                entity.HasKey(e => e.NpssqresponseId)
                    .HasName("PK__tblNPSSQ__387683219F4EA10E");

                entity.ToTable("tblNPSSQResponse");

                entity.Property(e => e.NpssqresponseId).HasColumnName("NPSSQResponseId");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.NpssqoptionId).HasColumnName("NPSSQOptionId");

                entity.Property(e => e.NpssquestionId).HasColumnName("NPSSQuestionId");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.TblNpssqresponseCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("Fk_tblNPSSQResponse_CreatedBy");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.TblNpssqresponseModifiedByNavigations)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("Fk_tblNPSSQResponse_ModifiedBy");

                entity.HasOne(d => d.Npssqoption)
                    .WithMany(p => p.TblNpssqresponses)
                    .HasForeignKey(d => d.NpssqoptionId)
                    .HasConstraintName("Fk_tblNPSSQResponse_NPSSQOptionId");

                entity.HasOne(d => d.Npssquestion)
                    .WithMany(p => p.TblNpssqresponses)
                    .HasForeignKey(d => d.NpssquestionId)
                    .HasConstraintName("Fk_tblNPSSQResponse_NPSSQuestionId");

                entity.HasOne(d => d.OrderTrans)
                    .WithMany(p => p.TblNpssqresponses)
                    .HasForeignKey(d => d.OrderTransId)
                    .HasConstraintName("Fk_tblNPSSQResponse_OrderTransId");
            });

            modelBuilder.Entity<TblNpssquestion>(entity =>
            {
                entity.HasKey(e => e.NpssquestionId)
                    .HasName("PK__tblNPSSQ__C208914EEC32CFAC");

                entity.ToTable("tblNPSSQuestions");

                entity.Property(e => e.NpssquestionId).HasColumnName("NPSSQuestionId");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Question).HasMaxLength(255);

                entity.Property(e => e.RatingType).HasMaxLength(50);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.TblNpssquestionCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("Fk_tblNPSSQuestions_CreatedBy");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.TblNpssquestionModifiedByNavigations)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("Fk_tblNPSSQuestions_ModifiedBy");
            });

            modelBuilder.Entity<TblOrderBasedConfig>(entity =>
            {
                entity.ToTable("tblOrderBasedConfig");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.IsBpmultiBrand).HasColumnName("IsBPMultiBrand");

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.TblOrderBasedConfigs)
                    .HasForeignKey(d => d.BrandId)
                    .HasConstraintName("FK__tblOrderB__Brand__4DD47EBD");

                entity.HasOne(d => d.BusinessPartner)
                    .WithMany(p => p.TblOrderBasedConfigs)
                    .HasForeignKey(d => d.BusinessPartnerId)
                    .HasConstraintName("FK__tblOrderB__Busin__4FBCC72F");

                entity.HasOne(d => d.BusinessUnit)
                    .WithMany(p => p.TblOrderBasedConfigs)
                    .HasForeignKey(d => d.BusinessUnitId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblOrderB__Busin__4EC8A2F6");
            });

            modelBuilder.Entity<TblOrderImageUpload>(entity =>
            {
                entity.HasKey(e => e.OrderImageUploadId)
                    .HasName("PK__tblOrder__2081518CC35015AE");

                entity.ToTable("tblOrderImageUpload");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ImageName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.LgcpickDrop)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("LGCPickDrop");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.TblOrderImageUploadCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("Fk_tblOrderImageUpload_CreatedBy");

                entity.HasOne(d => d.ModifiedbyNavigation)
                    .WithMany(p => p.TblOrderImageUploadModifiedbyNavigations)
                    .HasForeignKey(d => d.Modifiedby)
                    .HasConstraintName("Fk_tblOrderImageUpload_ModifiedBy");
            });

            modelBuilder.Entity<TblOrderLgc>(entity =>
            {
                entity.HasKey(e => e.OrderLgcid);

                entity.ToTable("tblOrderLGC");

                entity.Property(e => e.OrderLgcid).HasColumnName("OrderLGCId");

                entity.Property(e => e.ActualDropDate).HasColumnType("datetime");

                entity.Property(e => e.ActualPickupDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.CustDeclartionpdfname)
                    .HasMaxLength(255)
                    .HasColumnName("custDeclartionpdfname");

                entity.Property(e => e.EvcpartnerId).HasColumnName("EVCPartnerId");

                entity.Property(e => e.Evcpodid).HasColumnName("EVCPODId");

                entity.Property(e => e.EvcregistrationId).HasColumnName("EVCRegistrationId");

                entity.Property(e => e.Lgccomments)
                    .HasMaxLength(5000)
                    .IsUnicode(false)
                    .HasColumnName("LGCComments");

                entity.Property(e => e.LgcpayableAmt)
                    .HasColumnType("decimal(15, 2)")
                    .HasColumnName("LGCPayableAmt");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.ProposedPickDate).HasColumnType("datetime");

                entity.Property(e => e.SecondaryOrderFlag)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.TblOrderLgcCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("Fk_tblOrderLGC_CreatedBy");

                entity.HasOne(d => d.DriverDetails)
                    .WithMany(p => p.TblOrderLgcs)
                    .HasForeignKey(d => d.DriverDetailsId)
                    .HasConstraintName("FK__tblOrderL__Drive__297722B6");

                entity.HasOne(d => d.Evcpartner)
                    .WithMany(p => p.TblOrderLgcs)
                    .HasForeignKey(d => d.EvcpartnerId)
                    .HasConstraintName("FK__tblOrderL__EVCPa__0524B3A7");

                entity.HasOne(d => d.Evcpod)
                    .WithMany(p => p.TblOrderLgcs)
                    .HasForeignKey(d => d.Evcpodid)
                    .HasConstraintName("Fk_tblOrderLGC_EVCPODId");

                entity.HasOne(d => d.Evcregistration)
                    .WithMany(p => p.TblOrderLgcs)
                    .HasForeignKey(d => d.EvcregistrationId)
                    .HasConstraintName("FK__tblOrderL__EVCRe__23BE4960");

                entity.HasOne(d => d.Logistic)
                    .WithMany(p => p.TblOrderLgcs)
                    .HasForeignKey(d => d.LogisticId)
                    .HasConstraintName("FK_LogisticId");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.TblOrderLgcModifiedByNavigations)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("Fk_tblOrderLGC_ModifiedBy");

                entity.HasOne(d => d.OrderTrans)
                    .WithMany(p => p.TblOrderLgcs)
                    .HasForeignKey(d => d.OrderTransId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblOrderL__Order__2B5F6B28");
            });

            modelBuilder.Entity<TblOrderPromoVoucher>(entity =>
            {
                entity.HasKey(e => e.OrderPromoVoucherId);

                entity.ToTable("tblOrderPromoVoucher");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.VoucherCode)
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblOrderQc>(entity =>
            {
                entity.HasKey(e => e.OrderQcid);

                entity.ToTable("tblOrderQC");

                entity.Property(e => e.OrderQcid).HasColumnName("OrderQCId");

                entity.Property(e => e.AdditionalBonus).HasColumnType("decimal(15, 2)");

                entity.Property(e => e.AverageSellingPrice).HasColumnType("decimal(15, 2)");

                entity.Property(e => e.BonusPercentAdmin).HasColumnType("decimal(15, 2)");

                entity.Property(e => e.BonusPercentQc)
                    .HasColumnType("decimal(15, 2)")
                    .HasColumnName("BonusPercentQC");

                entity.Property(e => e.BonusbyQc)
                    .HasColumnType("decimal(15, 2)")
                    .HasColumnName("BonusbyQC");

                entity.Property(e => e.CollectedAmount).HasColumnType("decimal(15, 2)");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DagnosticPdfName).HasMaxLength(255);

                entity.Property(e => e.ExcellentPriceByAsp)
                    .HasColumnType("decimal(15, 2)")
                    .HasColumnName("ExcellentPriceByASP");

                entity.Property(e => e.FinalCalculatedWeightage).HasColumnType("decimal(15, 2)");

                entity.Property(e => e.FrontViewImage).HasMaxLength(400);

                entity.Property(e => e.InsideViewImage).HasMaxLength(400);

                entity.Property(e => e.IsUpino).HasColumnName("IsUPIno");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.PaymentImage).HasMaxLength(400);

                entity.Property(e => e.PickupEndTime)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PickupStartTime)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PreferredPickupDate).HasColumnType("datetime");

                entity.Property(e => e.PriceAfterQc)
                    .HasColumnType("decimal(15, 2)")
                    .HasColumnName("PriceAfterQC");

                entity.Property(e => e.ProposedQcdate)
                    .HasColumnType("datetime")
                    .HasColumnName("ProposedQCDate");

                entity.Property(e => e.Qccomments)
                    .HasMaxLength(5000)
                    .IsUnicode(false)
                    .HasColumnName("QCComments");

                entity.Property(e => e.Qcdate)
                    .HasColumnType("datetime")
                    .HasColumnName("QCDate");

                entity.Property(e => e.QualityAfterQc)
                    .HasMaxLength(5000)
                    .IsUnicode(false)
                    .HasColumnName("QualityAfterQC");

                entity.Property(e => e.QuotedPrice).HasColumnType("decimal(15, 2)");

                entity.Property(e => e.QuotedWithSweetner).HasColumnType("decimal(15, 2)");

                entity.Property(e => e.RescheduleNote).HasMaxLength(500);

                entity.Property(e => e.ShortVideoName).HasMaxLength(400);

                entity.Property(e => e.SideViewImage).HasMaxLength(400);

                entity.Property(e => e.SrNumberImage).HasMaxLength(400);

                entity.Property(e => e.Sweetener).HasColumnType("decimal(15, 2)");

                entity.Property(e => e.SweetenerBp)
                    .HasColumnType("decimal(15, 2)")
                    .HasColumnName("SweetenerBP");

                entity.Property(e => e.SweetenerBu)
                    .HasColumnType("decimal(15, 2)")
                    .HasColumnName("SweetenerBU");

                entity.Property(e => e.SweetenerDigi2l).HasColumnType("decimal(15, 2)");

                entity.Property(e => e.Sweetner).HasColumnType("decimal(15, 2)");

                entity.Property(e => e.Upiid)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("UPIId");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.TblOrderQcs)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("Fk_tblOrderQC_ModifiedBy");

                entity.HasOne(d => d.OrderTrans)
                    .WithMany(p => p.TblOrderQcs)
                    .HasForeignKey(d => d.OrderTransId)
                    .HasConstraintName("FK__tblOrderQ__Order__2759D01A");
            });

            modelBuilder.Entity<TblOrderQcrating>(entity =>
            {
                entity.HasKey(e => e.OrderQcratingId);

                entity.ToTable("tblOrderQCRating");

                entity.Property(e => e.OrderQcratingId).HasColumnName("OrderQCRatingId");

                entity.Property(e => e.CalculatedWeightage).HasColumnType("decimal(15, 2)");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.QcComments).HasMaxLength(500);

                entity.Property(e => e.QcquestionId).HasColumnName("QCQuestionId");

                entity.Property(e => e.QuestionerLovid).HasColumnName("QuestionerLOVId");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.TblOrderQcratingCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__tblOrderQ__Creat__1AF3F935");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.TblOrderQcratingModifiedByNavigations)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK__tblOrderQ__Modif__1BE81D6E");

                entity.HasOne(d => d.OrderTrans)
                    .WithMany(p => p.TblOrderQcratings)
                    .HasForeignKey(d => d.OrderTransId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblOrderQ__Order__1CDC41A7");

                entity.HasOne(d => d.ProductCat)
                    .WithMany(p => p.TblOrderQcratings)
                    .HasForeignKey(d => d.ProductCatId)
                    .HasConstraintName("FK__tblOrderQ__Produ__35A7EF71");

                entity.HasOne(d => d.Qcquestion)
                    .WithMany(p => p.TblOrderQcratings)
                    .HasForeignKey(d => d.QcquestionId)
                    .HasConstraintName("FK__tblOrderQ__QCQue__369C13AA");

                entity.HasOne(d => d.QuestionerLov)
                    .WithMany(p => p.TblOrderQcratings)
                    .HasForeignKey(d => d.QuestionerLovid)
                    .HasConstraintName("FK__tblOrderQ__Quest__379037E3");
            });

            modelBuilder.Entity<TblOrderTran>(entity =>
            {
                entity.HasKey(e => e.OrderTransId);

                entity.ToTable("tblOrderTrans");

                entity.Property(e => e.AbbredemptionId).HasColumnName("ABBRedemptionId");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Evcprice)
                    .HasColumnType("decimal(15, 2)")
                    .HasColumnName("EVCPrice");

                entity.Property(e => e.EvcpriceMasterId).HasColumnName("EVCPriceMasterId");

                entity.Property(e => e.ExchangePrice).HasColumnType("decimal(15, 2)");

                entity.Property(e => e.FinalPriceAfterQc)
                    .HasColumnType("decimal(15, 2)")
                    .HasColumnName("FinalPriceAfterQC");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.QuotedPrice).HasColumnType("decimal(15, 2)");

                entity.Property(e => e.RegdNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SelfQclinkResendby).HasColumnName("SelfQCLinkResendby");

                entity.Property(e => e.SponsorOrderNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Sweetner).HasColumnType("decimal(15, 2)");

                entity.HasOne(d => d.Abbredemption)
                    .WithMany(p => p.TblOrderTrans)
                    .HasForeignKey(d => d.AbbredemptionId)
                    .HasConstraintName("FK_tblOrderTrans_ABBRedemptionId");

                entity.HasOne(d => d.AssignByNavigation)
                    .WithMany(p => p.TblOrderTranAssignByNavigations)
                    .HasForeignKey(d => d.AssignBy)
                    .HasConstraintName("FK__tblOrderT__Assig__36870511");

                entity.HasOne(d => d.AssignToNavigation)
                    .WithMany(p => p.TblOrderTranAssignToNavigations)
                    .HasForeignKey(d => d.AssignTo)
                    .HasConstraintName("FK__tblOrderT__Assig__377B294A");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.TblOrderTranCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_CreatedBy");

                entity.HasOne(d => d.Exchange)
                    .WithMany(p => p.TblOrderTrans)
                    .HasForeignKey(d => d.ExchangeId)
                    .HasConstraintName("FK_ExchangeId");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.TblOrderTranModifiedByNavigations)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK_ModifiedBy");

                entity.HasOne(d => d.OrderTypeNavigation)
                    .WithMany(p => p.TblOrderTrans)
                    .HasForeignKey(d => d.OrderType)
                    .HasConstraintName("FK_OrderType");

                entity.HasOne(d => d.SelfQclinkResendbyNavigation)
                    .WithMany(p => p.TblOrderTranSelfQclinkResendbyNavigations)
                    .HasForeignKey(d => d.SelfQclinkResendby)
                    .HasConstraintName("FK__tblOrderT__SelfQ__74EE4BDE");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.TblOrderTrans)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("FK__tblOrderT__Statu__6E565CE8");
            });

            modelBuilder.Entity<TblPaymentLeaser>(entity =>
            {
                entity.ToTable("tblPaymentLeaser");

                entity.Property(e => e.Amount)
                    .HasColumnType("decimal(15, 2)")
                    .HasColumnName("amount");

                entity.Property(e => e.Bank)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.BankId)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.CardHashId)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.CardId)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.CardScheme)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.CardToken)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.CheckSum)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.GatewayTransactioId)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.ModuleType)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.OrderId)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.OrderStatus)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.PaymentDate).HasColumnType("datetime");

                entity.Property(e => e.PaymentMode)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.PaymentResponse)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("paymentResponse");

                entity.Property(e => e.RegdNo)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ResponseCode)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ResponseDescription).IsUnicode(false);

                entity.Property(e => e.TransactionId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("transactionId");

                entity.Property(e => e.TransactionType)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UtcreferenceId)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("UTCReferenceID");
            });

            modelBuilder.Entity<TblPinCode>(entity =>
            {
                entity.ToTable("tblPinCode");

                entity.HasIndex(e => e.ZipCode, "UQ__tblPinCo__2CC2CDB8C5013C8F")
                    .IsUnique();

                entity.HasIndex(e => e.ZipCode, "UQ__tblPinCo__2CC2CDB8D3AF4E0A")
                    .IsUnique();

                entity.Property(e => e.AreaLocality).HasMaxLength(255);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.HubControl).HasMaxLength(255);

                entity.Property(e => e.IsAbb).HasColumnName("IsABB");

                entity.Property(e => e.IsExchange).HasColumnName("isExchange");

                entity.Property(e => e.Location).HasMaxLength(255);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.State).HasMaxLength(255);

                entity.Property(e => e.ZohoPinCodeId).HasMaxLength(50);

                entity.HasOne(d => d.City)
                    .WithMany(p => p.TblPinCodes)
                    .HasForeignKey(d => d.CityId)
                    .HasConstraintName("FK__tblPinCod__CityI__031C6FA4");
            });

            modelBuilder.Entity<TblPincodeMasterDtoC>(entity =>
            {
                entity.ToTable("tblPincodeMasterDtoC");

                entity.Property(e => e.AreaName).HasMaxLength(250);

                entity.Property(e => e.City).HasMaxLength(255);

                entity.Property(e => e.CreatedBy).HasMaxLength(100);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Modifiedby).HasMaxLength(100);

                entity.Property(e => e.PinCode).HasMaxLength(10);

                entity.Property(e => e.State).HasMaxLength(255);
            });

            modelBuilder.Entity<TblPriceMaster>(entity =>
            {
                entity.ToTable("tblPriceMaster");

                entity.Property(e => e.BrandName1)
                    .HasMaxLength(255)
                    .HasColumnName("BrandName-1");

                entity.Property(e => e.BrandName2)
                    .HasMaxLength(255)
                    .HasColumnName("BrandName-2");

                entity.Property(e => e.BrandName3)
                    .HasMaxLength(255)
                    .HasColumnName("BrandName-3");

                entity.Property(e => e.BrandName4)
                    .HasMaxLength(255)
                    .HasColumnName("BrandName-4");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ExchPriceCode).HasMaxLength(255);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.OtherBrand).HasMaxLength(10);

                entity.Property(e => e.PriceEndDate).HasMaxLength(255);

                entity.Property(e => e.PriceStartDate).HasMaxLength(255);

                entity.Property(e => e.ProductCat).HasMaxLength(255);

                entity.Property(e => e.ProductType).HasMaxLength(255);

                entity.Property(e => e.ProductTypeCode).HasMaxLength(255);

                entity.Property(e => e.QuoteP)
                    .HasMaxLength(255)
                    .HasColumnName("Quote-P");

                entity.Property(e => e.QuotePHigh)
                    .HasMaxLength(255)
                    .HasColumnName("Quote-P-High");

                entity.Property(e => e.QuoteQ)
                    .HasMaxLength(255)
                    .HasColumnName("Quote-Q");

                entity.Property(e => e.QuoteQHigh)
                    .HasMaxLength(255)
                    .HasColumnName("Quote-Q-High");

                entity.Property(e => e.QuoteR)
                    .HasMaxLength(255)
                    .HasColumnName("Quote-R");

                entity.Property(e => e.QuoteRHigh)
                    .HasMaxLength(255)
                    .HasColumnName("Quote-R-High");

                entity.Property(e => e.QuoteS)
                    .HasMaxLength(255)
                    .HasColumnName("Quote-S");

                entity.Property(e => e.QuoteSHigh)
                    .HasMaxLength(255)
                    .HasColumnName("Quote-S-High");

                entity.Property(e => e.ZohoPriceMasterId).HasMaxLength(255);

                entity.HasOne(d => d.ProductCategory)
                    .WithMany(p => p.TblPriceMasters)
                    .HasForeignKey(d => d.ProductCategoryId)
                    .HasConstraintName("FK__tblProduc__Produ__1B9317B3");

                entity.HasOne(d => d.ProductTypeNavigation)
                    .WithMany(p => p.TblPriceMasters)
                    .HasForeignKey(d => d.ProductTypeId)
                    .HasConstraintName("FK__tblPriceM__Produ__5F9E293D");
            });

            modelBuilder.Entity<TblPriceMasterMapping>(entity =>
            {
                entity.HasKey(e => e.PriceMasterMappingId)
                    .HasName("PK__tblPrice__C90D5EC575201E93");

                entity.ToTable("tblPriceMasterMapping");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Enddate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Startdate).HasColumnType("datetime");

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.TblPriceMasterMappings)
                    .HasForeignKey(d => d.BrandId)
                    .HasConstraintName("FK__tblPriceM__Brand__24D2692A");

                entity.HasOne(d => d.BusinessPartner)
                    .WithMany(p => p.TblPriceMasterMappings)
                    .HasForeignKey(d => d.BusinessPartnerId)
                    .HasConstraintName("FK__tblPriceM__Busin__26BAB19C");

                entity.HasOne(d => d.BusinessUnit)
                    .WithMany(p => p.TblPriceMasterMappings)
                    .HasForeignKey(d => d.BusinessUnitId)
                    .HasConstraintName("FK__tblPriceM__Busin__25C68D63");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.TblPriceMasterMappingCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__tblPriceM__Creat__27AED5D5");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.TblPriceMasterMappingModifiedByNavigations)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK__tblPriceM__Modif__28A2FA0E");

                entity.HasOne(d => d.PriceMasterName)
                    .WithMany(p => p.TblPriceMasterMappings)
                    .HasForeignKey(d => d.PriceMasterNameId)
                    .HasConstraintName("FK__tblPriceM__Price__29971E47");
            });

            modelBuilder.Entity<TblPriceMasterName>(entity =>
            {
                entity.HasKey(e => e.PriceMasterNameId)
                    .HasName("PK__tblPrice__EF45EED1AB14286C");

                entity.ToTable("tblPriceMasterName");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.TblPriceMasterNameCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__tblPriceM__Creat__1960B67E");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.TblPriceMasterNameModifiedByNavigations)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK__tblPriceM__Modif__1A54DAB7");
            });

            modelBuilder.Entity<TblPriceMasterQuestioner>(entity =>
            {
                entity.HasKey(e => e.PriceMasterQuestionerId)
                    .HasName("PK__TblPrice__3DE8F93F8FE09720");

                entity.ToTable("TblPriceMasterQuestioner");

                entity.Property(e => e.AverageSellingPrice).HasColumnType("decimal(15, 2)");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.NonWorkingPrice).HasColumnType("decimal(15, 2)");

                entity.HasOne(d => d.BusinessUnit)
                    .WithMany(p => p.TblPriceMasterQuestioners)
                    .HasForeignKey(d => d.BusinessUnitId)
                    .HasConstraintName("FK__TblPriceM__Busin__20ACD28B");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.TblPriceMasterQuestionerCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__TblPriceM__Creat__22951AFD");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.TblPriceMasterQuestionerModifiedByNavigations)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK__TblPriceM__Modif__23893F36");

                entity.HasOne(d => d.ProductCat)
                    .WithMany(p => p.TblPriceMasterQuestioners)
                    .HasForeignKey(d => d.ProductCatId)
                    .HasConstraintName("FK_ProductCatId");

                entity.HasOne(d => d.ProductTechnology)
                    .WithMany(p => p.TblPriceMasterQuestioners)
                    .HasForeignKey(d => d.ProductTechnologyId)
                    .HasConstraintName("FK__TblPriceM__Produ__21A0F6C4");

                entity.HasOne(d => d.ProductType)
                    .WithMany(p => p.TblPriceMasterQuestioners)
                    .HasForeignKey(d => d.ProductTypeId)
                    .HasConstraintName("FK__TblPriceM__Produ__7F01C5FD");
            });

            modelBuilder.Entity<TblProdCatBrandMapping>(entity =>
            {
                entity.HasKey(e => e.ProdCatBrandMappingId)
                    .HasName("PK__TblProdC__74AA83C8235E67B2");

                entity.ToTable("TblProdCatBrandMapping");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.BrandGroup)
                    .WithMany(p => p.TblProdCatBrandMappings)
                    .HasForeignKey(d => d.BrandGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TblProdCa__Brand__255C790F");

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.TblProdCatBrandMappings)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TblProdCa__Brand__2374309D");

                entity.HasOne(d => d.CreatedbyNavigation)
                    .WithMany(p => p.TblProdCatBrandMappingCreatedbyNavigations)
                    .HasForeignKey(d => d.Createdby)
                    .HasConstraintName("FK__TblProdCa__Creat__26509D48");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.TblProdCatBrandMappingModifiedByNavigations)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK__TblProdCa__Modif__2744C181");

                entity.HasOne(d => d.ProductCat)
                    .WithMany(p => p.TblProdCatBrandMappings)
                    .HasForeignKey(d => d.ProductCatId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TblProdCa__Produ__246854D6");
            });

            modelBuilder.Entity<TblProductCategory>(entity =>
            {
                entity.ToTable("tblProductCategory");

                entity.Property(e => e.Asppercentage)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("ASPPercentage");

                entity.Property(e => e.Code).HasMaxLength(255);

                entity.Property(e => e.CommentForWorking)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.DescriptionForAbb)
                    .HasMaxLength(255)
                    .HasColumnName("Description_For_ABB");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(255);
            });

            modelBuilder.Entity<TblProductConditionLabel>(entity =>
            {
                entity.ToTable("tblProductConditionLabel");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Modifieddate).HasColumnType("datetime");

                entity.Property(e => e.PclabelName)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("PCLabelName");

                entity.HasOne(d => d.BusinessPartner)
                    .WithMany(p => p.TblProductConditionLabels)
                    .HasForeignKey(d => d.BusinessPartnerId)
                    .HasConstraintName("FK__tblProduc__Busin__1B48FEF0");

                entity.HasOne(d => d.BusinessUnit)
                    .WithMany(p => p.TblProductConditionLabels)
                    .HasForeignKey(d => d.BusinessUnitId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblProduc__Busin__57C7FD4B");

                entity.HasOne(d => d.ProductCat)
                    .WithMany(p => p.TblProductConditionLabels)
                    .HasForeignKey(d => d.ProductCatId)
                    .HasConstraintName("FK_tblProductConditionLabel_tblProductCategory");
            });

            modelBuilder.Entity<TblProductQualityIndex>(entity =>
            {
                entity.HasKey(e => e.ProductQualityIndexId)
                    .HasName("PK__tblProdu__D9EF5B3F4563A351");

                entity.ToTable("tblProductQualityIndex");

                entity.Property(e => e.AverageDesc).HasMaxLength(600);

                entity.Property(e => e.CategoryName).HasMaxLength(200);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ExcellentDesc).HasMaxLength(600);

                entity.Property(e => e.GoodDesc).HasMaxLength(600);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.NonWorkingDesc).HasMaxLength(600);

                entity.HasOne(d => d.ProductCategory)
                    .WithMany(p => p.TblProductQualityIndices)
                    .HasForeignKey(d => d.ProductCategoryId)
                    .HasConstraintName("FK_ProductQualityIndex_ProductCategoryId");
            });

            modelBuilder.Entity<TblProductTechnology>(entity =>
            {
                entity.HasKey(e => e.ProductTechnologyId)
                    .HasName("PK__TblProdu__FD17E030E9390401");

                entity.ToTable("TblProductTechnology");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.ProductTechnologyName).HasMaxLength(255);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.TblProductTechnologyCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__TblProduc__Creat__10766AC2");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.TblProductTechnologyModifiedByNavigations)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK__TblProduc__Modif__116A8EFB");

                entity.HasOne(d => d.ProductCat)
                    .WithMany(p => p.TblProductTechnologies)
                    .HasForeignKey(d => d.ProductCatId)
                    .HasConstraintName("FK__TblProduc__Produ__0F824689");
            });

            modelBuilder.Entity<TblProductType>(entity =>
            {
                entity.ToTable("tblProductType");

                entity.Property(e => e.Code).HasMaxLength(255);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.DescriptionForAbb)
                    .HasMaxLength(255)
                    .HasColumnName("Description_For_ABB");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.Size).HasMaxLength(255);

                entity.HasOne(d => d.ProductCat)
                    .WithMany(p => p.TblProductTypes)
                    .HasForeignKey(d => d.ProductCatId)
                    .HasConstraintName("FK__tblProduc__Produ__2B0A656D");
            });

            modelBuilder.Entity<TblPromotionalVoucherMaster>(entity =>
            {
                entity.HasKey(e => e.PromotionalVoucherId);

                entity.ToTable("tblPromotionalVoucherMaster");

                entity.Property(e => e.CompanyName)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.VoucherCode)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.VoucherName)
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblPushNotificationMessageDetail>(entity =>
            {
                entity.HasKey(e => e.NotificationMessageId)
                    .HasName("PK__tblPushN__FFFA59DA2A34E8ED");

                entity.ToTable("tblPushNotificationMessageDetails");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.MessageType).HasMaxLength(200);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Title).HasMaxLength(200);

                entity.Property(e => e.UserType).HasMaxLength(200);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.TblPushNotificationMessageDetailCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_tblPushNotificationMessageDetails_CreatedBy");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.TblPushNotificationMessageDetailModifiedByNavigations)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK__tblPushNo__Modif__41A3B202");
            });

            modelBuilder.Entity<TblPushNotificationSavedDetail>(entity =>
            {
                entity.HasKey(e => e.SavedDetailsId)
                    .HasName("PK__tblPushN__ED6094442935874F");

                entity.ToTable("tblPushNotificationSavedDetails");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Title).HasMaxLength(200);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.TblPushNotificationSavedDetailCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_tblPushNotificationSavedDetails_CreatedBy");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.TblPushNotificationSavedDetailModifiedByNavigations)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK__tblPushNo__Modif__4297D63B");
            });

            modelBuilder.Entity<TblQcratingMaster>(entity =>
            {
                entity.HasKey(e => e.QcratingId);

                entity.ToTable("tblQCRatingMaster");

                entity.Property(e => e.QcratingId).HasColumnName("QCRatingId");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Qcquestion)
                    .HasMaxLength(5000)
                    .IsUnicode(false)
                    .HasColumnName("QCQuestion");

                entity.Property(e => e.QuestionerLovid).HasColumnName("QuestionerLOVId");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.TblQcratingMasterCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__tblQCRati__Creat__162F4418");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.TblQcratingMasterModifiedByNavigations)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK__tblQCRati__Modif__17236851");

                entity.HasOne(d => d.QuestionerLov)
                    .WithMany(p => p.TblQcratingMasters)
                    .HasForeignKey(d => d.QuestionerLovid)
                    .HasConstraintName("FK__tblQCRati__Quest__18178C8A");
            });

            modelBuilder.Entity<TblQcratingMasterMapping>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tblQCRatingMasterMapping");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.QcratingId).HasColumnName("QCRatingId");

                entity.Property(e => e.QcratingMasterMappingId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("QCRatingMasterMappingId");

                entity.HasOne(d => d.ProductCat)
                    .WithMany()
                    .HasForeignKey(d => d.ProductCatId)
                    .HasConstraintName("FK_tblQCRatingMasterMapping_tblProductCategory");

                entity.HasOne(d => d.ProductTechnology)
                    .WithMany()
                    .HasForeignKey(d => d.ProductTechnologyId)
                    .HasConstraintName("FK_tblQCRatingMasterMapping_TblProductTechnology");

                entity.HasOne(d => d.ProductType)
                    .WithMany()
                    .HasForeignKey(d => d.ProductTypeId)
                    .HasConstraintName("FK_tblQCRatingMasterMapping_tblProductType");

                entity.HasOne(d => d.Qcrating)
                    .WithMany()
                    .HasForeignKey(d => d.QcratingId)
                    .HasConstraintName("FK_tblQCRatingMasterMapping_tblQCRatingMaster");
            });

            modelBuilder.Entity<TblQuestionerLov>(entity =>
            {
                entity.HasKey(e => e.QuestionerLovid)
                    .HasName("PK__TblQuest__62281FF8741C4CE9");

                entity.ToTable("TblQuestionerLOV");

                entity.Property(e => e.QuestionerLovid).HasColumnName("QuestionerLOVId");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.QuestionerLovname)
                    .HasMaxLength(255)
                    .HasColumnName("QuestionerLOVName");

                entity.Property(e => e.QuestionerLovparentId).HasColumnName("QuestionerLOVParentId");

                entity.Property(e => e.QuestionerLovratingWeightage)
                    .HasColumnType("decimal(15, 2)")
                    .HasColumnName("QuestionerLOVRatingWeightage");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.TblQuestionerLovCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__TblQuesti__Creat__1446FBA6");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.TblQuestionerLovModifiedByNavigations)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK__TblQuesti__Modif__153B1FDF");
            });

            modelBuilder.Entity<TblQuestionerLovmapping>(entity =>
            {
                entity.HasKey(e => e.QuestionerLovmappingId)
                    .HasName("PK__TblQuest__519459829C02FC07");

                entity.ToTable("TblQuestionerLOVMapping");

                entity.Property(e => e.QuestionerLovmappingId).HasColumnName("QuestionerLOVMappingId");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.QuestionerLovid).HasColumnName("QuestionerLOVId");

                entity.Property(e => e.RatingWeightageLov)
                    .HasColumnType("decimal(15, 2)")
                    .HasColumnName("RatingWeightageLOV");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.TblQuestionerLovmappingCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("Fk_TblQuestionerLOVMapping_CreatedBy");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.TblQuestionerLovmappingModifiedByNavigations)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("Fk_TblQuestionerLOVMapping_ModifiedBy");

                entity.HasOne(d => d.ProductCat)
                    .WithMany(p => p.TblQuestionerLovmappings)
                    .HasForeignKey(d => d.ProductCatId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fk_TblQuestionerLOVMapping_ProductCatId");

                entity.HasOne(d => d.QuestionerLov)
                    .WithMany(p => p.TblQuestionerLovmappings)
                    .HasForeignKey(d => d.QuestionerLovid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fk_TblQuestionerLOVMapping_QuestionerLOVId");
            });

            modelBuilder.Entity<TblQuestionsForSweetner>(entity =>
            {
                entity.HasKey(e => e.QuestionId)
                    .HasName("PK__tblQuest__0DC06FAC83CEFCA4");

                entity.ToTable("tblQuestionsForSweetner");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Question).HasMaxLength(255);

                entity.Property(e => e.QuestionKey)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.TblQuestionsForSweetnerCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("Fk_tblQuestionsForSweetner_CreatedBy");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.TblQuestionsForSweetnerModifiedByNavigations)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("Fk_tblQuestionsForSweetner_ModifiedBy");
            });

            modelBuilder.Entity<TblRefurbisherRegistration>(entity =>
            {
                entity.HasKey(e => e.RefurbisherId);

                entity.ToTable("tblRefurbisherRegistration");

                entity.Property(e => e.AccountHolder).HasMaxLength(250);

                entity.Property(e => e.Address)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.BankAccountNo).HasMaxLength(600);

                entity.Property(e => e.BankName).HasMaxLength(600);

                entity.Property(e => e.Branch).HasMaxLength(250);

                entity.Property(e => e.CompanyRegNo).HasMaxLength(250);

                entity.Property(e => e.ContactPerson).HasMaxLength(255);

                entity.Property(e => e.Country).HasMaxLength(250);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EmailId).HasMaxLength(255);

                entity.Property(e => e.Gstdeclaration)
                    .HasMaxLength(250)
                    .HasColumnName("GSTDeclaration");

                entity.Property(e => e.Gstno)
                    .HasMaxLength(255)
                    .HasColumnName("GSTNo");

                entity.Property(e => e.Ifsccode)
                    .HasMaxLength(600)
                    .HasColumnName("IFSCCode");

                entity.Property(e => e.ManagerContact).HasMaxLength(250);

                entity.Property(e => e.ManagerEmail).HasMaxLength(250);

                entity.Property(e => e.Mobile)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.NatureOfBusiness).HasMaxLength(250);

                entity.Property(e => e.PanNo).HasMaxLength(250);

                entity.Property(e => e.PostalCode)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.RefurbisherApprovalName).HasMaxLength(250);

                entity.Property(e => e.RefurbisherName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Telephone)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.TypeofServices).HasMaxLength(250);

                entity.Property(e => e.UnitDepartment)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UtcemployeeContact)
                    .HasMaxLength(250)
                    .HasColumnName("UTCEmployeeContact");

                entity.Property(e => e.UtcemployeeEmail)
                    .HasMaxLength(250)
                    .HasColumnName("UTCEmployeeEmail");

                entity.Property(e => e.UtcemployeeName)
                    .HasMaxLength(250)
                    .HasColumnName("UTCEmployeeName");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.TblRefurbisherRegistrations)
                    .HasForeignKey(d => d.CityId)
                    .HasConstraintName("Fk_tblRefurbisherRegistration_CityId");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.TblRefurbisherRegistrationCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("Fk_tblRefurbisherRegistration_CreatedBy");

                entity.HasOne(d => d.EntityType)
                    .WithMany(p => p.TblRefurbisherRegistrations)
                    .HasForeignKey(d => d.EntityTypeId)
                    .HasConstraintName("Fk_tblRefurbisherRegistration_EntityTypeId");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.TblRefurbisherRegistrationModifiedByNavigations)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("Fk_tblRefurbisherRegistration_ModifiedBy");

                entity.HasOne(d => d.State)
                    .WithMany(p => p.TblRefurbisherRegistrations)
                    .HasForeignKey(d => d.StateId)
                    .HasConstraintName("Fk_tblRefurbisherRegistration_StateId");
            });

            modelBuilder.Entity<TblRole>(entity =>
            {
                entity.HasKey(e => e.RoleId)
                    .HasName("PK__tblRole__8AFACE1ABE6B7F96");

                entity.ToTable("tblRole");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.IsRoleMultiBrand).HasDefaultValueSql("((0))");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.RoleName).HasMaxLength(150);

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.TblRoles)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("Fk_tblRole_CompanyId");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.TblRoleCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("Fk_tblRole_CreatedBy");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.TblRoleModifiedByNavigations)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("Fk_tblRole_ModifiedBy");
            });

            modelBuilder.Entity<TblRoleAccess>(entity =>
            {
                entity.HasKey(e => e.RoleAccessId)
                    .HasName("PK__tblRoleA__C1244FD4A3A7CCEE");

                entity.ToTable("tblRoleAccess");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.AccessList)
                    .WithMany(p => p.TblRoleAccesses)
                    .HasForeignKey(d => d.AccessListId)
                    .HasConstraintName("Fk_tblRoleAccess_AccessListId");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.TblRoleAccesses)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("Fk_tblRoleAccess_CompanyId");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.TblRoleAccessCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("Fk_tblRoleAccess_CreatedBy");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.TblRoleAccessModifiedByNavigations)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("Fk_tblRoleAccess_ModifiedBy");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.TblRoleAccesses)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("Fk_tblRoleAccess_RoleId");
            });

            modelBuilder.Entity<TblSelfQc>(entity =>
            {
                entity.HasKey(e => e.SelfQcid)
                    .HasName("PK__tblSelfQ__DE91E5E0DFF8B278");

                entity.ToTable("tblSelfQC");

                entity.Property(e => e.SelfQcid).HasColumnName("SelfQCId");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ImageName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.IsAbb).HasColumnName("IsABB");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Modifiedby)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.RegdNo)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.HasOne(d => d.ExchangeOrder)
                    .WithMany(p => p.TblSelfQcs)
                    .HasForeignKey(d => d.ExchangeOrderId)
                    .HasConstraintName("FK__tblSelfQC__Excha__7CA47C3F");

                entity.HasOne(d => d.Redemption)
                    .WithMany(p => p.TblSelfQcs)
                    .HasForeignKey(d => d.RedemptionId)
                    .HasConstraintName("FK_AbbRedemptionId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TblSelfQcs)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__tblSelfQC__UserI__73FA27A5");
            });

            modelBuilder.Entity<TblServicePartner>(entity =>
            {
                entity.HasKey(e => e.ServicePartnerId)
                    .HasName("PK__tblServi__C795E7D58C35000F");

                entity.ToTable("tblServicePartner");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.IconfirmTermsCondition).HasColumnName("IConfirmTermsCondition");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.ServicePartnerAadharBackImage).IsUnicode(false);

                entity.Property(e => e.ServicePartnerAadharfrontImage).IsUnicode(false);

                entity.Property(e => e.ServicePartnerAddressLine1).HasMaxLength(255);

                entity.Property(e => e.ServicePartnerAddressLine2).HasMaxLength(255);

                entity.Property(e => e.ServicePartnerAlternateMobileNumber).HasMaxLength(50);

                entity.Property(e => e.ServicePartnerBankAccountNo).HasMaxLength(600);

                entity.Property(e => e.ServicePartnerBankName).HasMaxLength(600);

                entity.Property(e => e.ServicePartnerBusinessName).HasMaxLength(255);

                entity.Property(e => e.ServicePartnerCancelledCheque).IsUnicode(false);

                entity.Property(e => e.ServicePartnerDescription)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ServicePartnerEmailId)
                    .HasMaxLength(255)
                    .HasColumnName("ServicePartnerEmailID");

                entity.Property(e => e.ServicePartnerFirstName).HasMaxLength(255);

                entity.Property(e => e.ServicePartnerGstno)
                    .HasMaxLength(255)
                    .HasColumnName("ServicePartnerGSTNo");

                entity.Property(e => e.ServicePartnerGstregisteration)
                    .IsUnicode(false)
                    .HasColumnName("ServicePartnerGSTRegisteration");

                entity.Property(e => e.ServicePartnerIfsccode)
                    .HasMaxLength(600)
                    .HasColumnName("ServicePartnerIFSCCODE");

                entity.Property(e => e.ServicePartnerInsertOtp).HasColumnName("ServicePartnerInsertOTP");

                entity.Property(e => e.ServicePartnerLastName).HasMaxLength(255);

                entity.Property(e => e.ServicePartnerMobileNumber).HasMaxLength(50);

                entity.Property(e => e.ServicePartnerName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ServicePartnerPinCode).HasMaxLength(50);

                entity.Property(e => e.ServicePartnerProfilePic).IsUnicode(false);

                entity.Property(e => e.ServicePartnerRegdNo).HasMaxLength(50);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.TblServicePartnerCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("Fk_tblServicePartner_CreatedBy");

                entity.HasOne(d => d.ModifiedbyNavigation)
                    .WithMany(p => p.TblServicePartnerModifiedbyNavigations)
                    .HasForeignKey(d => d.Modifiedby)
                    .HasConstraintName("Fk_tblServicePartner_ModifiedBy");

                entity.HasOne(d => d.ServicePartnerCity)
                    .WithMany(p => p.TblServicePartners)
                    .HasForeignKey(d => d.ServicePartnerCityId)
                    .HasConstraintName("Fk_tblServicePartner_ServicePartnerCityId");

                entity.HasOne(d => d.ServicePartnerState)
                    .WithMany(p => p.TblServicePartners)
                    .HasForeignKey(d => d.ServicePartnerStateId)
                    .HasConstraintName("Fk_tblServicePartner_ServicePartnerStateId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TblServicePartnerUsers)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__tblServic__UserI__75035A77");
            });

            modelBuilder.Entity<TblSociety>(entity =>
            {
                entity.HasKey(e => e.SocietyId);

                entity.ToTable("tblSociety");

                entity.Property(e => e.AddressLine1).HasMaxLength(250);

                entity.Property(e => e.AddressLine2).HasMaxLength(250);

                entity.Property(e => e.City).HasMaxLength(150);

                entity.Property(e => e.ContactPersonFirstName)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.ContactPersonLastName)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(150);

                entity.Property(e => e.LogoName)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(250);

                entity.Property(e => e.PhoneNumber).HasMaxLength(20);

                entity.Property(e => e.Pincode).HasMaxLength(150);

                entity.Property(e => e.QrcodeUrl)
                    .HasColumnType("text")
                    .HasColumnName("QRCodeURL");

                entity.Property(e => e.RegistrationNumber).HasMaxLength(150);

                entity.Property(e => e.State).HasMaxLength(150);

                entity.HasOne(d => d.BusinessUnit)
                    .WithMany(p => p.TblSocieties)
                    .HasForeignKey(d => d.BusinessUnitId)
                    .HasConstraintName("FK_tblSociety_BusinessUnitId");

                entity.HasOne(d => d.Login)
                    .WithMany(p => p.TblSocieties)
                    .HasForeignKey(d => d.LoginId)
                    .HasConstraintName("FK_tblSociety_LoginId");
            });

            modelBuilder.Entity<TblSponsorCategoryMapping>(entity =>
            {
                entity.ToTable("tblSponsorCategoryMapping");

                entity.Property(e => e.BucategoryId).HasColumnName("BUCategoryId");

                entity.Property(e => e.BucategoryName)
                    .HasMaxLength(255)
                    .HasColumnName("BUCategoryName");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.BusinessUnit)
                    .WithMany(p => p.TblSponsorCategoryMappings)
                    .HasForeignKey(d => d.BusinessUnitId)
                    .HasConstraintName("FK_tblSponsorCategoryMapping_tblBusinessUnit");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.TblSponsorCategoryMappings)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_tblSponsorCategoryMapping_tblProductCategory");
            });

            modelBuilder.Entity<TblState>(entity =>
            {
                entity.HasKey(e => e.StateId)
                    .HasName("PK__tblState__C3BA3B3A1000266B");

                entity.ToTable("tblState");

                entity.Property(e => e.Code).HasMaxLength(5);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(150);
            });

            modelBuilder.Entity<TblTempDatum>(entity =>
            {
                entity.ToTable("tblTempData");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.FileName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.RegdNo)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.TblTempDatumCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("Fk_tblTempData_CreatedBy");

                entity.HasOne(d => d.ImageLabel)
                    .WithMany(p => p.TblTempData)
                    .HasForeignKey(d => d.ImageLabelid)
                    .HasConstraintName("Fk_tblTempData_ImageLabelid");

                entity.HasOne(d => d.Lov)
                    .WithMany(p => p.TblTempData)
                    .HasForeignKey(d => d.LovId)
                    .HasConstraintName("Fk_tblTempData_LoVId");

                entity.HasOne(d => d.ModifiedbyNavigation)
                    .WithMany(p => p.TblTempDatumModifiedbyNavigations)
                    .HasForeignKey(d => d.Modifiedby)
                    .HasConstraintName("Fk_tblTempData_ModifiedBy");

                entity.HasOne(d => d.OrderTrans)
                    .WithMany(p => p.TblTempData)
                    .HasForeignKey(d => d.OrderTransId)
                    .HasConstraintName("Fk_tblTempData_OrderTransId");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.TblTempData)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("Fk_tblTempData_StatusId");
            });

            modelBuilder.Entity<TblTimeLine>(entity =>
            {
                entity.HasKey(e => e.TimeLineId)
                    .HasName("PK_tblOrderTimeLine");

                entity.ToTable("tblTimeLine");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.OrderTimeline)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.TblTimeLines)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_tblTimeLine_tblUser");
            });

            modelBuilder.Entity<TblTimelineStatusMapping>(entity =>
            {
                entity.HasKey(e => e.TimelineStatusMappingId);

                entity.ToTable("tblTimelineStatusMapping");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.StatusCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.TblTimelineStatusMappings)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_tblTimelineStatusMapping_tblUser");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.TblTimelineStatusMappings)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblTimelineStatusMapping_tblExchangeOrderStatus");
            });

            modelBuilder.Entity<TblTransMasterAbbplanMaster>(entity =>
            {
                entity.ToTable("tblTransMasterABBPlanMaster");

                entity.Property(e => e.AbbregistrationId).HasColumnName("ABBRegistrationId");

                entity.Property(e => e.AssuredBuyBackPercentage).HasColumnName("Assured_BuyBack_Percentage");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.FromMonth).HasColumnName("From_Month");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Sponsor)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ToMonth).HasColumnName("To_Month");

                entity.HasOne(d => d.Abbregistration)
                    .WithMany(p => p.TblTransMasterAbbplanMasters)
                    .HasForeignKey(d => d.AbbregistrationId)
                    .HasConstraintName("FK__tblTransM__ABBRe__31A25463");

                entity.HasOne(d => d.BusinessUnit)
                    .WithMany(p => p.TblTransMasterAbbplanMasters)
                    .HasForeignKey(d => d.BusinessUnitId)
                    .HasConstraintName("FK__tblTransM__Busin__30AE302A");
            });

            modelBuilder.Entity<TblUnInstallationPriceMaster>(entity =>
            {
                entity.ToTable("tblUnInstallationPriceMaster");

                entity.Property(e => e.Product).HasMaxLength(100);

                entity.Property(e => e.ProductType).HasMaxLength(100);

                entity.Property(e => e.Size).HasMaxLength(50);

                entity.Property(e => e.Type).HasMaxLength(100);

                entity.Property(e => e.UninstallationPrice).HasColumnType("decimal(15, 2)");
            });

            modelBuilder.Entity<TblUninstallationPrice>(entity =>
            {
                entity.ToTable("tblUninstallationPrices");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.UnInstallationPrice).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.BusinessPartner)
                    .WithMany(p => p.TblUninstallationPrices)
                    .HasForeignKey(d => d.BusinessPartnerId)
                    .HasConstraintName("FK_tblUnistallationPrices_tblBusinessPartner");

                entity.HasOne(d => d.BusinessUnit)
                    .WithMany(p => p.TblUninstallationPrices)
                    .HasForeignKey(d => d.BusinessUnitId)
                    .HasConstraintName("FK_tblUnistallationPrices_tblBusinessUnit");

                entity.HasOne(d => d.ProductCat)
                    .WithMany(p => p.TblUninstallationPrices)
                    .HasForeignKey(d => d.ProductCatId)
                    .HasConstraintName("FK_tblUnistallationPrices_tblProductCategory");

                entity.HasOne(d => d.ProductType)
                    .WithMany(p => p.TblUninstallationPrices)
                    .HasForeignKey(d => d.ProductTypeId)
                    .HasConstraintName("FK_tblUnistallationPrices_tblProductType");
            });

            modelBuilder.Entity<TblUniversalPriceMaster>(entity =>
            {
                entity.HasKey(e => e.PriceMasterUniversalId)
                    .HasName("PK__tblUnive__71B600B358A9AA35");

                entity.ToTable("tblUniversalPriceMaster");

                entity.Property(e => e.BrandName1)
                    .HasMaxLength(255)
                    .HasColumnName("BrandName-1");

                entity.Property(e => e.BrandName2)
                    .HasMaxLength(255)
                    .HasColumnName("BrandName-2");

                entity.Property(e => e.BrandName3)
                    .HasMaxLength(255)
                    .HasColumnName("BrandName-3");

                entity.Property(e => e.BrandName4)
                    .HasMaxLength(255)
                    .HasColumnName("BrandName-4");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.OtherBrand).HasMaxLength(255);

                entity.Property(e => e.PriceEndDate).HasMaxLength(255);

                entity.Property(e => e.PriceMasterName).HasMaxLength(255);

                entity.Property(e => e.PriceStartDate).HasMaxLength(255);

                entity.Property(e => e.ProductCategoryName).HasMaxLength(255);

                entity.Property(e => e.ProductTypeCode).HasMaxLength(255);

                entity.Property(e => e.ProductTypeName).HasMaxLength(255);

                entity.Property(e => e.QuoteP)
                    .HasMaxLength(255)
                    .HasColumnName("Quote-P");

                entity.Property(e => e.QuotePHigh)
                    .HasMaxLength(255)
                    .HasColumnName("Quote-P-High");

                entity.Property(e => e.QuoteQ)
                    .HasMaxLength(255)
                    .HasColumnName("Quote-Q");

                entity.Property(e => e.QuoteQHigh)
                    .HasMaxLength(255)
                    .HasColumnName("Quote-Q-High");

                entity.Property(e => e.QuoteR)
                    .HasMaxLength(255)
                    .HasColumnName("Quote-R");

                entity.Property(e => e.QuoteRHigh)
                    .HasMaxLength(255)
                    .HasColumnName("Quote-R-High");

                entity.Property(e => e.QuoteS)
                    .HasMaxLength(255)
                    .HasColumnName("Quote-S");

                entity.Property(e => e.QuoteSHigh)
                    .HasMaxLength(255)
                    .HasColumnName("Quote-S-High");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.TblUniversalPriceMasterCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__tblUniver__Creat__2C738AF2");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.TblUniversalPriceMasterModifiedByNavigations)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK__tblUniver__Modif__2D67AF2B");

                entity.HasOne(d => d.PriceMasterNameNavigation)
                    .WithMany(p => p.TblUniversalPriceMasters)
                    .HasForeignKey(d => d.PriceMasterNameId)
                    .HasConstraintName("FK__tblUniver__Price__2E5BD364");

                entity.HasOne(d => d.ProductCategory)
                    .WithMany(p => p.TblUniversalPriceMasters)
                    .HasForeignKey(d => d.ProductCategoryId)
                    .HasConstraintName("FK__tblUniver__Produ__2F4FF79D");

                entity.HasOne(d => d.ProductType)
                    .WithMany(p => p.TblUniversalPriceMasters)
                    .HasForeignKey(d => d.ProductTypeId)
                    .HasConstraintName("FK__tblUniver__Produ__30441BD6");
            });

            modelBuilder.Entity<TblUpiidUpdatelog>(entity =>
            {
                entity.ToTable("tblUPIIdUpdatelog");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.OldUpiid)
                    .HasMaxLength(50)
                    .HasColumnName("OldUPIId");

                entity.Property(e => e.PayLoad).HasMaxLength(1000);

                entity.Property(e => e.RegdNo).HasMaxLength(50);

                entity.Property(e => e.Upiid)
                    .HasMaxLength(50)
                    .HasColumnName("UPIId");
            });

            modelBuilder.Entity<TblUser>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK__tblUser__1788CC4CA77D3F12");

                entity.ToTable("tblUser");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName).HasMaxLength(150);

                entity.Property(e => e.Gender)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ImageName)
                    .HasMaxLength(600)
                    .IsUnicode(false);

                entity.Property(e => e.LastLogin).HasColumnType("datetime");

                entity.Property(e => e.LastName).HasMaxLength(150);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Password)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.UserStatus).HasMaxLength(100);

                entity.Property(e => e.ZohoUserId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("ZohoUserID");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.TblUsers)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("Fk_tblUser_CompanyId");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.InverseCreatedByNavigation)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("Fk_tblUser_CreatedBy");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.InverseModifiedByNavigation)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("Fk_tblUser_ModifiedBy");
            });

            modelBuilder.Entity<TblUserMapping>(entity =>
            {
                entity.HasKey(e => e.UserMappingId)
                    .HasName("PK__tblUserM__B03C2992E68DB0B9");

                entity.ToTable("tblUserMapping");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.BusinessPartner)
                    .WithMany(p => p.TblUserMappings)
                    .HasForeignKey(d => d.BusinessPartnerId)
                    .HasConstraintName("FK__tblUserMa__Busin__34D3C6C9");

                entity.HasOne(d => d.BusinessUnit)
                    .WithMany(p => p.TblUserMappings)
                    .HasForeignKey(d => d.BusinessUnitId)
                    .HasConstraintName("FK__tblUserMa__Busin__35C7EB02");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.TblUserMappingCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__tblUserMa__Creat__36BC0F3B");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.TblUserMappingModifiedByNavigations)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK__tblUserMa__Modif__37B03374");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TblUserMappingUsers)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__tblUserMa__UserI__33DFA290");
            });

            modelBuilder.Entity<TblUserRole>(entity =>
            {
                entity.HasKey(e => e.UserRoleId)
                    .HasName("PK__tblUserR__3D978A35830700BB");

                entity.ToTable("tblUserRole");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.TblUserRoles)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("Fk_tblUserRole_CompanyId");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.TblUserRoleCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("Fk_tblUserRole_CreatedBy");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.TblUserRoleModifiedByNavigations)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("Fk_tblUserRole_ModifiedBy");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.TblUserRoles)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("Fk_tblUserRole_RoleId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TblUserRoleUsers)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("Fk_tblUserRole_UserId");
            });

            modelBuilder.Entity<TblVcareService>(entity =>
            {
                entity.ToTable("tblVcareService");

                entity.Property(e => e.CallNo).HasMaxLength(100);

                entity.Property(e => e.Cust1stName).HasMaxLength(100);

                entity.Property(e => e.CustCity).HasMaxLength(100);

                entity.Property(e => e.CustMobile).HasMaxLength(20);

                entity.Property(e => e.CustPinCode).HasMaxLength(10);

                entity.Property(e => e.CustState).HasMaxLength(60);

                entity.Property(e => e.Custemail).HasMaxLength(500);

                entity.Property(e => e.ExchAge).HasMaxLength(100);

                entity.Property(e => e.ExchBrand).HasMaxLength(100);

                entity.Property(e => e.ExchCondition).HasMaxLength(100);

                entity.Property(e => e.ExchProdGroup).HasMaxLength(100);

                entity.Property(e => e.ExchProdType).HasMaxLength(100);

                entity.Property(e => e.ExchSize).HasMaxLength(100);

                entity.Property(e => e.Landmark).HasMaxLength(500);

                entity.Property(e => e.LastName).HasMaxLength(100);

                entity.Property(e => e.QuoteP).HasMaxLength(100);

                entity.Property(e => e.QuoteQ).HasMaxLength(100);

                entity.Property(e => e.QuoteR).HasMaxLength(100);

                entity.Property(e => e.QuoteS).HasMaxLength(100);

                entity.Property(e => e.Salutation).HasMaxLength(250);

                entity.Property(e => e.SpOrderNo).HasMaxLength(50);

                entity.Property(e => e.SponsorName).HasMaxLength(150);

                entity.Property(e => e.UtcregNo)
                    .HasMaxLength(100)
                    .HasColumnName("UTCRegNo");
            });

            modelBuilder.Entity<TblVehicleIncentive>(entity =>
            {
                entity.HasKey(e => e.IncentiveId)
                    .HasName("PK__TblVehic__1617835CA772EB5E");

                entity.ToTable("TblVehicleIncentive");

                entity.Property(e => e.BasePrice).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DropImageIncentive).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.DropIncAmount).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.PackagingIncentive).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.PickupIncAmount).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.BusinessUnit)
                    .WithMany(p => p.TblVehicleIncentives)
                    .HasForeignKey(d => d.BusinessUnitId)
                    .HasConstraintName("FK_TblVehicleIncentive_BusinessUnitId");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.TblVehicleIncentiveCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_TblVehicleIncentive_CreatedBy");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.TblVehicleIncentiveModifiedByNavigations)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK_TblVehicleIncentive_ModifiedBy");

                entity.HasOne(d => d.ProductCategory)
                    .WithMany(p => p.TblVehicleIncentives)
                    .HasForeignKey(d => d.ProductCategoryId)
                    .HasConstraintName("FK_TblVehicleIncentive_ProductCategoryId");

                entity.HasOne(d => d.ProductType)
                    .WithMany(p => p.TblVehicleIncentives)
                    .HasForeignKey(d => d.ProductTypeId)
                    .HasConstraintName("FK_TblVehicleIncentive_ProductTypeId");
            });

            modelBuilder.Entity<TblVehicleJourneyTracking>(entity =>
            {
                entity.HasKey(e => e.TrackingId)
                    .HasName("PK__TblVehic__3C19EDF1D61CCDF1");

                entity.ToTable("TblVehicleJourneyTracking");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.CurrentVehicleLatt).HasMaxLength(50);

                entity.Property(e => e.CurrentVehicleLong).HasMaxLength(50);

                entity.Property(e => e.JourneyEndTime).HasColumnType("datetime");

                entity.Property(e => e.JourneyPlanDate).HasColumnType("datetime");

                entity.Property(e => e.JourneyStartDatetime).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.ServicePartnerId).HasColumnName("ServicePartnerID");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.TblVehicleJourneyTrackingCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_TblVehicleJourneyTracking_CreatedBy");

                entity.HasOne(d => d.Driver)
                    .WithMany(p => p.TblVehicleJourneyTrackings)
                    .HasForeignKey(d => d.DriverId)
                    .HasConstraintName("FK_TblVehicleJourneyTracking_DriverId");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.TblVehicleJourneyTrackingModifiedByNavigations)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK_TblVehicleJourneyTracking_ModifiedBy");

                entity.HasOne(d => d.ServicePartner)
                    .WithMany(p => p.TblVehicleJourneyTrackings)
                    .HasForeignKey(d => d.ServicePartnerId)
                    .HasConstraintName("FK_TblVehicleJourneyTracking_ServicePartnerID");
            });

            modelBuilder.Entity<TblVehicleJourneyTrackingDetail>(entity =>
            {
                entity.HasKey(e => e.TrackingDetailsId)
                    .HasName("PK__TblVehic__7BB78FCE7C31E747");

                entity.Property(e => e.BasePrice).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DropImageInc).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.DropInc).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.DropLatt).HasMaxLength(50);

                entity.Property(e => e.DropLong).HasMaxLength(50);

                entity.Property(e => e.EstimateEarning).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Evcid).HasColumnName("EVCId");

                entity.Property(e => e.EvcpartnerId).HasColumnName("EVCPartnerId");

                entity.Property(e => e.JourneyPlanDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.OrderDropTime).HasColumnType("datetime");

                entity.Property(e => e.PackingInc).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.PickupEndDatetime).HasColumnType("datetime");

                entity.Property(e => e.PickupInc).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.PickupLatt).HasMaxLength(50);

                entity.Property(e => e.PickupLong).HasMaxLength(50);

                entity.Property(e => e.PickupStartDatetime).HasColumnType("datetime");

                entity.Property(e => e.ServicePartnerId).HasColumnName("ServicePartnerID");

                entity.Property(e => e.Total).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.TblVehicleJourneyTrackingDetailCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_TblVehicleJourneyTrackingDetails_CreatedBy");

                entity.HasOne(d => d.Driver)
                    .WithMany(p => p.TblVehicleJourneyTrackingDetails)
                    .HasForeignKey(d => d.DriverId)
                    .HasConstraintName("FK_TblVehicleJourneyTrackingDetails_DriverId");

                entity.HasOne(d => d.Evc)
                    .WithMany(p => p.TblVehicleJourneyTrackingDetails)
                    .HasForeignKey(d => d.Evcid)
                    .HasConstraintName("FK_TblVehicleJourneyTrackingDetails_EVCId");

                entity.HasOne(d => d.Evcpartner)
                    .WithMany(p => p.TblVehicleJourneyTrackingDetails)
                    .HasForeignKey(d => d.EvcpartnerId)
                    .HasConstraintName("FK__TblVehicl__EVCPa__0618D7E0");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.TblVehicleJourneyTrackingDetailModifiedByNavigations)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK_TblVehicleJourneyTrackingDetails_ModifiedBy");

                entity.HasOne(d => d.OrderTrans)
                    .WithMany(p => p.TblVehicleJourneyTrackingDetails)
                    .HasForeignKey(d => d.OrderTransId)
                    .HasConstraintName("FK_TblVehicleJourneyTrackingDetails_OrderTransId");

                entity.HasOne(d => d.ServicePartner)
                    .WithMany(p => p.TblVehicleJourneyTrackingDetails)
                    .HasForeignKey(d => d.ServicePartnerId)
                    .HasConstraintName("FK_TblVehicleJourneyTrackingDetails_ServicePartnerID");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.TblVehicleJourneyTrackingDetails)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("FK_TblVehicleJourneyTrackingDetails_StatusId");

                entity.HasOne(d => d.Tracking)
                    .WithMany(p => p.TblVehicleJourneyTrackingDetails)
                    .HasForeignKey(d => d.TrackingId)
                    .HasConstraintName("FK_TblVehicleJourneyTrackingDetails_TrackingId");
            });

            modelBuilder.Entity<TblVehicleList>(entity =>
            {
                entity.HasKey(e => e.VehicleId)
                    .HasName("PK__tblVehic__476B54925C27BF9D");

                entity.ToTable("tblVehicleList");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.VehicleInsuranceCertificate).IsUnicode(false);

                entity.Property(e => e.VehicleNumber)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.VehicleRcCertificate)
                    .IsUnicode(false)
                    .HasColumnName("Vehicle_RC_Certificate");

                entity.Property(e => e.VehicleRcNumber)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("Vehicle_RC_Number");

                entity.Property(e => e.VehiclefitnessCertificate).IsUnicode(false);

                entity.HasOne(d => d.City)
                    .WithMany(p => p.TblVehicleLists)
                    .HasForeignKey(d => d.CityId)
                    .HasConstraintName("FK__tblVehicl__CityI__5DA0D232");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.TblVehicleListCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__tblVehicl__Creat__5E94F66B");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.TblVehicleListModifiedByNavigations)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK__tblVehicl__Modif__5F891AA4");

                entity.HasOne(d => d.ServicePartner)
                    .WithMany(p => p.TblVehicleLists)
                    .HasForeignKey(d => d.ServicePartnerId)
                    .HasConstraintName("FK__tblVehicl__Servi__607D3EDD");
            });

            modelBuilder.Entity<TblVoucherStatus>(entity =>
            {
                entity.HasKey(e => e.VoucherStatusId)
                    .HasName("PK__tblVouch__E033B2A78690CCA9");

                entity.ToTable("tblVoucherStatus");

                entity.Property(e => e.VoucherStatusName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblVoucherTermsAndCondition>(entity =>
            {
                entity.ToTable("tblVoucherTermsAndConditions");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.TermsandCondition).IsUnicode(false);

                entity.HasOne(d => d.BusinessUnit)
                    .WithMany(p => p.TblVoucherTermsAndConditions)
                    .HasForeignKey(d => d.BusinessUnitId)
                    .HasConstraintName("FK__tblVouche__Busin__3D7E1B63");
            });

            modelBuilder.Entity<TblVoucherVerfication>(entity =>
            {
                entity.HasKey(e => e.VoucherVerficationId)
                    .HasName("PK__tblVouch__D48E82186A1EF2D0");

                entity.ToTable("tblVoucherVerfication");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(600);

                entity.Property(e => e.ExchangePrice).HasColumnType("decimal(15, 2)");

                entity.Property(e => e.InvoiceImageName).HasMaxLength(600);

                entity.Property(e => e.InvoiceName).HasMaxLength(200);

                entity.Property(e => e.InvoiceNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Sweetneer).HasColumnType("decimal(15, 2)");

                entity.Property(e => e.VoucherCode).HasMaxLength(50);

                entity.HasOne(d => d.BusinessPartner)
                    .WithMany(p => p.TblVoucherVerfications)
                    .HasForeignKey(d => d.BusinessPartnerId)
                    .HasConstraintName("FK_tblVoucherVerfication_BusinessPartnerId");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.TblVoucherVerfications)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_tblVoucherVerfication_CustomerId");

                entity.HasOne(d => d.ExchangeOrder)
                    .WithMany(p => p.TblVoucherVerfications)
                    .HasForeignKey(d => d.ExchangeOrderId)
                    .HasConstraintName("FK_tblVoucherVerfication_ExchangeOrderId");

                entity.HasOne(d => d.ModelNumber)
                    .WithMany(p => p.TblVoucherVerfications)
                    .HasForeignKey(d => d.ModelNumberId)
                    .HasConstraintName("FK_tblVoucherVerfication_ModelNumberId");

                entity.HasOne(d => d.NewBrand)
                    .WithMany(p => p.TblVoucherVerfications)
                    .HasForeignKey(d => d.NewBrandId)
                    .HasConstraintName("FK_tblVoucherVerficatio_NewBrandId");

                entity.HasOne(d => d.NewProductCategory)
                    .WithMany(p => p.TblVoucherVerfications)
                    .HasForeignKey(d => d.NewProductCategoryId)
                    .HasConstraintName("FK_tblABBReg_NewProductCategoryId");

                entity.HasOne(d => d.NewProductType)
                    .WithMany(p => p.TblVoucherVerfications)
                    .HasForeignKey(d => d.NewProductTypeId)
                    .HasConstraintName("FK__tblVouche__NewPr__6462DE5A");

                entity.HasOne(d => d.Redemption)
                    .WithMany(p => p.TblVoucherVerfications)
                    .HasForeignKey(d => d.RedemptionId)
                    .HasConstraintName("FK_RedemptionId");

                entity.HasOne(d => d.VoucherStatus)
                    .WithMany(p => p.TblVoucherVerfications)
                    .HasForeignKey(d => d.VoucherStatusId)
                    .HasConstraintName("FK__tblVouche__Vouch__18EBB532");
            });

            modelBuilder.Entity<TblWalletTransaction>(entity =>
            {
                entity.HasKey(e => e.WalletTransactionId);

                entity.ToTable("tblWalletTransaction");

                entity.Property(e => e.BaseValue).HasColumnType("decimal(15, 2)");

                entity.Property(e => e.Cgstamt)
                    .HasColumnType("decimal(15, 2)")
                    .HasColumnName("CGSTAmt");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EvcpartnerId).HasColumnName("EVCPartnerId");

                entity.Property(e => e.EvcregistrationId).HasColumnName("EVCRegistrationId");

                entity.Property(e => e.GsttypeId).HasColumnName("GSTTypeId");

                entity.Property(e => e.Igstamt)
                    .HasColumnType("decimal(15, 2)")
                    .HasColumnName("IGSTAmt");

                entity.Property(e => e.Lgccost).HasColumnName("LGCCost");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.NewEvcid).HasColumnName("NewEVCId");

                entity.Property(e => e.OldEvcid).HasColumnName("OldEVCId");

                entity.Property(e => e.OrderAmount).HasColumnType("decimal(15, 2)");

                entity.Property(e => e.OrderOfCompleteDate).HasColumnType("datetime");

                entity.Property(e => e.OrderOfDeliverdDate).HasColumnType("datetime");

                entity.Property(e => e.OrderOfInprogressDate).HasColumnType("datetime");

                entity.Property(e => e.OrderType).HasMaxLength(50);

                entity.Property(e => e.OrderofAssignDate).HasColumnType("datetime");

                entity.Property(e => e.PrimeProductDiffAmt).HasColumnType("decimal(15, 2)");

                entity.Property(e => e.RegdNo).HasMaxLength(150);

                entity.Property(e => e.Sgstamt)
                    .HasColumnType("decimal(15, 2)")
                    .HasColumnName("SGSTAmt");

                entity.Property(e => e.SponserOrderNo).HasMaxLength(150);

                entity.Property(e => e.StatusId).HasMaxLength(50);

                entity.Property(e => e.SweetenerAmt).HasColumnType("decimal(15, 2)");

                entity.Property(e => e.ZohoAllocationId).HasMaxLength(150);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.TblWalletTransactionCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("Fk_tblWalletTransaction_CreatedBy");

                entity.HasOne(d => d.Evcpartner)
                    .WithMany(p => p.TblWalletTransactions)
                    .HasForeignKey(d => d.EvcpartnerId)
                    .HasConstraintName("FK__tblWallet__EVCPa__033C6B35");

                entity.HasOne(d => d.Evcregistration)
                    .WithMany(p => p.TblWalletTransactions)
                    .HasForeignKey(d => d.EvcregistrationId)
                    .HasConstraintName("Fk_tblWalletTransaction_EVCRegistrationId");

                entity.HasOne(d => d.Gsttype)
                    .WithMany(p => p.TblWalletTransactions)
                    .HasForeignKey(d => d.GsttypeId)
                    .HasConstraintName("Fk_tblWalletTransaction_GSTTypeId");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.TblWalletTransactionModifiedByNavigations)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("Fk_tblWalletTransaction_ModifiedBy");

                entity.HasOne(d => d.OrderTrans)
                    .WithMany(p => p.TblWalletTransactions)
                    .HasForeignKey(d => d.OrderTransId)
                    .HasConstraintName("FK__tblWallet__Order__740F363E");
            });

            modelBuilder.Entity<TblWhatsAppMessage>(entity =>
            {
                entity.ToTable("tblWhatsAppMessage");

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("code");

                entity.Property(e => e.MsgId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("msgId");

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SendDate).HasColumnType("datetime");

                entity.Property(e => e.TemplateName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TimeSlotMaster>(entity =>
            {
                entity.ToTable("TimeSlotMaster");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<UniversalPriceMaster>(entity =>
            {
                entity.HasKey(e => e.PriceMasterUniversalId)
                    .HasName("PK__Universa__71B600B31BE1D48A");

                entity.ToTable("UniversalPriceMaster");

                entity.Property(e => e.BrandName1)
                    .HasMaxLength(255)
                    .HasColumnName("BrandName-1");

                entity.Property(e => e.BrandName2)
                    .HasMaxLength(255)
                    .HasColumnName("BrandName-2");

                entity.Property(e => e.BrandName3)
                    .HasMaxLength(255)
                    .HasColumnName("BrandName-3");

                entity.Property(e => e.BrandName4)
                    .HasMaxLength(255)
                    .HasColumnName("BrandName-4");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.OtherBrand).HasMaxLength(255);

                entity.Property(e => e.PriceEndDate).HasMaxLength(255);

                entity.Property(e => e.PriceMasterName).HasMaxLength(255);

                entity.Property(e => e.PriceStartDate).HasMaxLength(255);

                entity.Property(e => e.ProductCategoryName).HasMaxLength(255);

                entity.Property(e => e.ProductTypeCode).HasMaxLength(255);

                entity.Property(e => e.ProductTypeName).HasMaxLength(255);

                entity.Property(e => e.QuoteP)
                    .HasMaxLength(255)
                    .HasColumnName("Quote-P");

                entity.Property(e => e.QuotePHigh)
                    .HasMaxLength(255)
                    .HasColumnName("Quote-P-High");

                entity.Property(e => e.QuoteQ)
                    .HasMaxLength(255)
                    .HasColumnName("Quote-Q");

                entity.Property(e => e.QuoteQHigh)
                    .HasMaxLength(255)
                    .HasColumnName("Quote-Q-High");

                entity.Property(e => e.QuoteR)
                    .HasMaxLength(255)
                    .HasColumnName("Quote-R");

                entity.Property(e => e.QuoteRHigh)
                    .HasMaxLength(255)
                    .HasColumnName("Quote-R-High");

                entity.Property(e => e.QuoteS)
                    .HasMaxLength(255)
                    .HasColumnName("Quote-S");

                entity.Property(e => e.QuoteSHigh)
                    .HasMaxLength(255)
                    .HasColumnName("Quote-S-High");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.UniversalPriceMasterCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__Universal__Creat__37E53D9E");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.UniversalPriceMasterModifiedByNavigations)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK__Universal__Modif__38D961D7");

                entity.HasOne(d => d.PriceMasterNameNavigation)
                    .WithMany(p => p.UniversalPriceMasters)
                    .HasForeignKey(d => d.PriceMasterNameId)
                    .HasConstraintName("FK__Universal__Price__39CD8610");

                entity.HasOne(d => d.ProductCategory)
                    .WithMany(p => p.UniversalPriceMasters)
                    .HasForeignKey(d => d.ProductCategoryId)
                    .HasConstraintName("FK__Universal__Produ__3AC1AA49");

                entity.HasOne(d => d.ProductType)
                    .WithMany(p => p.UniversalPriceMasters)
                    .HasForeignKey(d => d.ProductTypeId)
                    .HasConstraintName("FK__Universal__Produ__3BB5CE82");
            });

            modelBuilder.Entity<ViewAbb>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("View_ABB");

                entity.Property(e => e.Abbfees)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("ABBFees");

                entity.Property(e => e.AbbplanName)
                    .HasMaxLength(150)
                    .HasColumnName("ABBPlanName");

                entity.Property(e => e.AbbplanPeriod)
                    .HasMaxLength(10)
                    .HasColumnName("ABBPlanPeriod");

                entity.Property(e => e.AbbpriceId).HasColumnName("ABBPriceId");

                entity.Property(e => e.AbbregistrationId).HasColumnName("ABBRegistrationId");

                entity.Property(e => e.Brand).HasMaxLength(255);

                entity.Property(e => e.Company).HasMaxLength(250);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.CustAddress1)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.CustAddress2)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.CustCity)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.CustEmail).HasMaxLength(250);

                entity.Property(e => e.CustFirstName)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.CustLastName)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.CustMobile).HasMaxLength(50);

                entity.Property(e => e.CustPinCode).HasMaxLength(250);

                entity.Property(e => e.CustState)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.EmployeeId)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Hsncode)
                    .HasMaxLength(50)
                    .HasColumnName("HSNCode");

                entity.Property(e => e.InvoiceDate).HasColumnType("date");

                entity.Property(e => e.InvoiceNo).HasMaxLength(150);

                entity.Property(e => e.InvoicePath).HasColumnName("Invoice Path");

                entity.Property(e => e.Location).HasMaxLength(250);

                entity.Property(e => e.MarCom).HasMaxLength(50);

                entity.Property(e => e.ModelName).HasMaxLength(255);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.NewPrice).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.NewProductCategoryName).HasMaxLength(50);

                entity.Property(e => e.NewProductCategoryType).HasMaxLength(50);

                entity.Property(e => e.NewSize).HasMaxLength(50);

                entity.Property(e => e.NoOfClaimPeriod).HasMaxLength(10);

                entity.Property(e => e.OrderType).HasMaxLength(150);

                entity.Property(e => e.OtherModelNo).HasMaxLength(255);

                entity.Property(e => e.ProductGroup)
                    .HasMaxLength(255)
                    .HasColumnName("Product Group");

                entity.Property(e => e.ProductNetPrice).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.ProductSrNo).HasMaxLength(50);

                entity.Property(e => e.ProductType)
                    .HasMaxLength(255)
                    .HasColumnName("Product Type");

                entity.Property(e => e.RegdNo).HasMaxLength(10);

                entity.Property(e => e.SponsorOrderNo).HasMaxLength(150);

                entity.Property(e => e.SponsorProdCode).HasMaxLength(50);

                entity.Property(e => e.StoreCode).HasMaxLength(150);

                entity.Property(e => e.StoreLocation).HasMaxLength(150);

                entity.Property(e => e.StoreManagerEmail).HasMaxLength(50);

                entity.Property(e => e.StoreName).HasMaxLength(250);

                entity.Property(e => e.UploadDateTime).HasColumnType("datetime");

                entity.Property(e => e.YourRegistrationNo).HasMaxLength(50);
            });

            modelBuilder.Entity<ViewAbbCount>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("View_ABB_Count");

                entity.Property(e => e.CountBu).HasColumnName("CountBU");

                entity.Property(e => e.Name).HasMaxLength(250);
            });

            modelBuilder.Entity<ViewAllExchangeDataForPinelab>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("View_AllExchangeDataForPinelabs");

                entity.Property(e => e.ActualDropDate).HasColumnType("datetime");

                entity.Property(e => e.ActualPickupDate).HasColumnType("datetime");

                entity.Property(e => e.Amount).HasColumnType("decimal(15, 2)");

                entity.Property(e => e.CompanyName).HasMaxLength(255);

                entity.Property(e => e.CurrentStatus)
                    .HasMaxLength(5)
                    .HasColumnName("Current Status");

                entity.Property(e => e.DateOfDevicePickUp).HasColumnType("datetime");

                entity.Property(e => e.DateOfTrx)
                    .HasColumnType("datetime")
                    .HasColumnName("DateOfTRX");

                entity.Property(e => e.ExchangePrice).HasColumnType("decimal(15, 2)");

                entity.Property(e => e.Lgcamount1)
                    .HasColumnType("decimal(15, 2)")
                    .HasColumnName("LGCAmount1");

                entity.Property(e => e.Lgccomments)
                    .HasMaxLength(5000)
                    .IsUnicode(false)
                    .HasColumnName("LGCComments");

                entity.Property(e => e.OembonusAmount)
                    .HasColumnType("decimal(15, 2)")
                    .HasColumnName("OEMBonusAmount");

                entity.Property(e => e.OrderDate).HasColumnType("datetime");

                entity.Property(e => e.OroembonusAmount)
                    .HasColumnType("decimal(15, 2)")
                    .HasColumnName("OROEMBonusAmount");

                entity.Property(e => e.PickUpStatus).HasMaxLength(5);

                entity.Property(e => e.ProductCondition).HasMaxLength(50);

                entity.Property(e => e.ProposedPickDate).HasColumnType("datetime");

                entity.Property(e => e.RegdNo).HasMaxLength(15);

                entity.Property(e => e.SponsorOrderNumber).HasMaxLength(255);

                entity.Property(e => e.StatusDescription).HasMaxLength(255);

                entity.Property(e => e.StatusName).HasMaxLength(30);

                entity.Property(e => e.Sweetener).HasColumnType("decimal(15, 2)");

                entity.Property(e => e.TransactionId).HasMaxLength(15);

                entity.Property(e => e.Utrnumber)
                    .HasMaxLength(15)
                    .HasColumnName("UTRNumber");

                entity.Property(e => e.VoucherCode).HasMaxLength(200);
            });

            modelBuilder.Entity<ViewAllExchangeDatum>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("View_AllExchangeData");

                entity.Property(e => e.ActualDropDate).HasColumnType("datetime");

                entity.Property(e => e.ActualPickupDate).HasColumnType("datetime");

                entity.Property(e => e.BrandName).HasMaxLength(255);

                entity.Property(e => e.BusinessPartnerName).HasMaxLength(250);

                entity.Property(e => e.BusinessUnit).HasMaxLength(250);

                entity.Property(e => e.CancellationDate).HasColumnType("datetime");

                entity.Property(e => e.City).HasMaxLength(150);

                entity.Property(e => e.CompanyName).HasMaxLength(255);

                entity.Property(e => e.CurrentStatus)
                    .HasMaxLength(5)
                    .HasColumnName("Current Status");

                entity.Property(e => e.CustCity)
                    .HasMaxLength(255)
                    .HasColumnName("Cust City");

                entity.Property(e => e.CustEMail)
                    .HasMaxLength(255)
                    .HasColumnName("Cust E-mail");

                entity.Property(e => e.CustMobile)
                    .HasMaxLength(255)
                    .HasColumnName("Cust Mobile");

                entity.Property(e => e.CustName)
                    .HasMaxLength(511)
                    .HasColumnName("Cust Name");

                entity.Property(e => e.CustState)
                    .HasMaxLength(255)
                    .HasColumnName("Cust State");

                entity.Property(e => e.CustZipCode)
                    .HasMaxLength(255)
                    .HasColumnName("Cust ZipCode");

                entity.Property(e => e.EmployeeId)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.EvcAllocated)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("EVC Allocated");

                entity.Property(e => e.EvcAssignedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("EVC AssignedDate");

                entity.Property(e => e.Evcprice)
                    .HasColumnType("decimal(15, 2)")
                    .HasColumnName("EVCPrice");

                entity.Property(e => e.ExchangePrice).HasColumnType("decimal(15, 2)");

                entity.Property(e => e.Expr1).HasMaxLength(255);

                entity.Property(e => e.InstallationDate).HasColumnType("datetime");

                entity.Property(e => e.IsFromHome)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("Is From Home");

                entity.Property(e => e.IsOrc)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("Is ORC");

                entity.Property(e => e.IssueDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Issue Date");

                entity.Property(e => e.Lgccomments)
                    .HasMaxLength(5000)
                    .IsUnicode(false)
                    .HasColumnName("LGCComments");

                entity.Property(e => e.LgcpayableAmt)
                    .HasColumnType("decimal(15, 2)")
                    .HasColumnName("LGCPayableAmt");

                entity.Property(e => e.LogisticsPartner)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("Logistics Partner");

                entity.Property(e => e.OldProductPrice)
                    .HasColumnType("decimal(16, 2)")
                    .HasColumnName("Old Product Price");

                entity.Property(e => e.OrderDate).HasColumnType("datetime");

                entity.Property(e => e.OrderDate1)
                    .HasColumnType("date")
                    .HasColumnName("Order Date");

                entity.Property(e => e.Pincode).HasMaxLength(150);

                entity.Property(e => e.PodName)
                    .HasMaxLength(600)
                    .HasColumnName("POD Name");

                entity.Property(e => e.PreferredPickupDate)
                    .HasMaxLength(204)
                    .IsUnicode(false);

                entity.Property(e => e.PriceAfterQc)
                    .HasColumnType("decimal(15, 2)")
                    .HasColumnName("PriceAfterQC");

                entity.Property(e => e.ProdCatCode).HasMaxLength(255);

                entity.Property(e => e.ProdTypeCode).HasMaxLength(255);

                entity.Property(e => e.ProductCat)
                    .HasMaxLength(255)
                    .HasColumnName("Product Cat");

                entity.Property(e => e.ProductCondition).HasMaxLength(50);

                entity.Property(e => e.ProductType)
                    .HasMaxLength(255)
                    .HasColumnName("Product Type");

                entity.Property(e => e.ProposedPickDate).HasColumnType("datetime");

                entity.Property(e => e.ProposedQcdateTime)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("ProposedQCDate time");

                entity.Property(e => e.Qccomments)
                    .HasMaxLength(5000)
                    .IsUnicode(false)
                    .HasColumnName("QCComments");

                entity.Property(e => e.Qcdate)
                    .HasColumnType("datetime")
                    .HasColumnName("QCDate");

                entity.Property(e => e.Qcflag)
                    .HasMaxLength(5)
                    .HasColumnName("QCFlag");

                entity.Property(e => e.QualityAfterQc)
                    .HasMaxLength(5000)
                    .IsUnicode(false)
                    .HasColumnName("QualityAfterQC");

                entity.Property(e => e.RegdNo).HasMaxLength(15);

                entity.Property(e => e.Rescheduledate).HasColumnType("datetime");

                entity.Property(e => e.SelfQcdate)
                    .HasColumnType("datetime")
                    .HasColumnName("SelfQCDate");

                entity.Property(e => e.SponsorOrderNumber).HasMaxLength(255);

                entity.Property(e => e.SponsorServiceRefId).HasMaxLength(400);

                entity.Property(e => e.StatusDescription).HasMaxLength(255);

                entity.Property(e => e.StatusName).HasMaxLength(30);

                entity.Property(e => e.StoreCode).HasMaxLength(150);

                entity.Property(e => e.StoreName).HasMaxLength(250);

                entity.Property(e => e.Sweetener).HasColumnType("decimal(15, 2)");

                entity.Property(e => e.SweetenerAmount)
                    .HasColumnType("decimal(15, 2)")
                    .HasColumnName("Sweetener Amount");

                entity.Property(e => e.TicketGenerationDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Ticket Generation Date");

                entity.Property(e => e.TicketNumber)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Upiid)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("UPIId");

                entity.Property(e => e.VoucherAmount)
                    .HasColumnType("decimal(15, 2)")
                    .HasColumnName("Voucher Amount");

                entity.Property(e => e.VoucherNo)
                    .HasMaxLength(200)
                    .HasColumnName("Voucher No");

                entity.Property(e => e.VoucherStatus)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("Voucher Status");

                entity.Property(e => e.VoucherStatusDetail)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Voucher Status Detail");
            });

            modelBuilder.Entity<ZohoExchangeDatum>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Abb)
                    .HasMaxLength(255)
                    .HasColumnName("ABB ?");

                entity.Property(e => e.AbbFees)
                    .HasMaxLength(255)
                    .HasColumnName("ABB Fees");

                entity.Property(e => e.AbbPlanName)
                    .HasMaxLength(255)
                    .HasColumnName("ABB Plan Name");

                entity.Property(e => e.AbbPlanPeriodMonths)
                    .HasMaxLength(255)
                    .HasColumnName("ABB Plan Period (Months)");

                entity.Property(e => e.AbbPriceId)
                    .HasMaxLength(255)
                    .HasColumnName("ABB Price ID");

                entity.Property(e => e.ActualAge)
                    .HasMaxLength(255)
                    .HasColumnName("Actual Age");

                entity.Property(e => e.ActualAmountPaid)
                    .HasMaxLength(255)
                    .HasColumnName("Actual Amount Paid");

                entity.Property(e => e.ActualAmtPayableInclBonus).HasColumnName("Actual Amt payable (incl Bonus)");

                entity.Property(e => e.ActualBaseAmountAsPerQc).HasColumnName("Actual Base Amount As per (QC)");

                entity.Property(e => e.ActualBrand)
                    .HasMaxLength(255)
                    .HasColumnName("Actual Brand");

                entity.Property(e => e.ActualEvcAmountAsPerQc)
                    .HasMaxLength(255)
                    .HasColumnName("Actual EVC Amount As Per QC");

                entity.Property(e => e.ActualPickupDate)
                    .HasMaxLength(255)
                    .HasColumnName("Actual Pickup Date");

                entity.Property(e => e.ActualProdQltyAtTimeOfQc)
                    .HasMaxLength(255)
                    .HasColumnName("Actual Prod Qlty (at time of QC)");

                entity.Property(e => e.ActualSize)
                    .HasMaxLength(255)
                    .HasColumnName("Actual Size");

                entity.Property(e => e.ActualTotalAmountAsPerQc)
                    .HasMaxLength(255)
                    .HasColumnName("Actual (Total) Amount as per QC");

                entity.Property(e => e.ActualType)
                    .HasMaxLength(255)
                    .HasColumnName("Actual Type");

                entity.Property(e => e.AddedTime)
                    .HasMaxLength(255)
                    .HasColumnName("Added Time");

                entity.Property(e => e.AmountPayableThroughLgc)
                    .HasMaxLength(255)
                    .HasColumnName("Amount Payable Through  LGC");

                entity.Property(e => e.AssociateCode)
                    .HasMaxLength(255)
                    .HasColumnName("Associate Code");

                entity.Property(e => e.AssociateEmail)
                    .HasMaxLength(255)
                    .HasColumnName("Associate Email");

                entity.Property(e => e.AssociateName)
                    .HasMaxLength(255)
                    .HasColumnName("Associate Name");

                entity.Property(e => e.BankReference)
                    .HasMaxLength(255)
                    .HasColumnName("Bank Reference ");

                entity.Property(e => e.BaseExchValueP).HasColumnName("Base Exch Value (P)");

                entity.Property(e => e.BaseExchValueQ).HasColumnName("Base Exch Value (Q)");

                entity.Property(e => e.BaseExchValueR).HasColumnName("Base Exch Value (R)");

                entity.Property(e => e.BaseExchValueS).HasColumnName("Base Exch Value (S)");

                entity.Property(e => e.Closed).HasMaxLength(255);

                entity.Property(e => e.CompressorNoOduSrNo)
                    .HasMaxLength(255)
                    .HasColumnName("Compressor No/ODU Sr No");

                entity.Property(e => e.Cust1stName)
                    .HasMaxLength(255)
                    .HasColumnName("Cust 1st Name");

                entity.Property(e => e.CustAdd1)
                    .HasMaxLength(255)
                    .HasColumnName("Cust Add 1");

                entity.Property(e => e.CustAdd2)
                    .HasMaxLength(255)
                    .HasColumnName("Cust Add 2");

                entity.Property(e => e.CustCity)
                    .HasMaxLength(255)
                    .HasColumnName("Cust City");

                entity.Property(e => e.CustDeclaration)
                    .HasMaxLength(255)
                    .HasColumnName("Cust Declaration");

                entity.Property(e => e.CustDeclaredQlty)
                    .HasMaxLength(255)
                    .HasColumnName("Cust Declared Qlty");

                entity.Property(e => e.CustEMail)
                    .HasMaxLength(255)
                    .HasColumnName("Cust E-mail");

                entity.Property(e => e.CustMobile)
                    .HasMaxLength(255)
                    .HasColumnName("Cust Mobile");

                entity.Property(e => e.CustName)
                    .HasMaxLength(255)
                    .HasColumnName("Cust Name");

                entity.Property(e => e.CustOkForPrice)
                    .HasMaxLength(255)
                    .HasColumnName("Cust OK for Price");

                entity.Property(e => e.CustPinCode).HasColumnName("Cust Pin Code");

                entity.Property(e => e.CustState)
                    .HasMaxLength(255)
                    .HasColumnName("Cust State");

                entity.Property(e => e.CustomerDeclaredAge)
                    .HasMaxLength(255)
                    .HasColumnName("Customer Declared Age");

                entity.Property(e => e.DatePaid)
                    .HasMaxLength(255)
                    .HasColumnName("Date paid");

                entity.Property(e => e.EMailSmsDate)
                    .HasMaxLength(255)
                    .HasColumnName("E-Mail/SMS Date");

                entity.Property(e => e.EMailSmsFlag)
                    .HasMaxLength(255)
                    .HasColumnName("E-Mail/ SMS Flag");

                entity.Property(e => e.EstimateDeliveryDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Estimate Delivery Date ");

                entity.Property(e => e.EvcAcknowledge)
                    .HasMaxLength(255)
                    .HasColumnName("EVC Acknowledge");

                entity.Property(e => e.EvcDropDate)
                    .HasMaxLength(255)
                    .HasColumnName("EVC Drop Date");

                entity.Property(e => e.EvcDropFlag)
                    .HasMaxLength(255)
                    .HasColumnName("EVC Drop Flag");

                entity.Property(e => e.EvcP).HasColumnName("EVC P");

                entity.Property(e => e.EvcQ).HasColumnName("EVC Q");

                entity.Property(e => e.EvcR).HasColumnName("EVC R");

                entity.Property(e => e.EvcS).HasColumnName("EVC S");

                entity.Property(e => e.EvcStatus)
                    .HasMaxLength(255)
                    .HasColumnName("EVC Status");

                entity.Property(e => e.ExchPriceId)
                    .HasMaxLength(255)
                    .HasColumnName("Exch# Price ID");

                entity.Property(e => e.ExchProdGroup)
                    .HasMaxLength(255)
                    .HasColumnName("Exch# Prod Group");

                entity.Property(e => e.Exchange)
                    .HasMaxLength(255)
                    .HasColumnName("Exchange ?");

                entity.Property(e => e.ExchangeStatus)
                    .HasMaxLength(255)
                    .HasColumnName("Exchange Status");

                entity.Property(e => e.ExpectedPickupDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Expected Pickup Date");

                entity.Property(e => e.FirstAttemptDate)
                    .HasMaxLength(255)
                    .HasColumnName("First Attempt Date");

                entity.Property(e => e.HsnCodeForAbbFees)
                    .HasMaxLength(255)
                    .HasColumnName("HSN Code (For ABB Fees)");

                entity.Property(e => e.Id)
                    .HasMaxLength(255)
                    .HasColumnName("ID");

                entity.Property(e => e.InstallationDate)
                    .HasMaxLength(255)
                    .HasColumnName("Installation Date");

                entity.Property(e => e.InstallationFlag)
                    .HasMaxLength(255)
                    .HasColumnName("Installation Flag");

                entity.Property(e => e.InvoiceDate)
                    .HasMaxLength(255)
                    .HasColumnName("Invoice Date");

                entity.Property(e => e.InvoiceImage)
                    .HasMaxLength(255)
                    .HasColumnName("Invoice Image");

                entity.Property(e => e.InvoiceNo)
                    .HasMaxLength(255)
                    .HasColumnName("Invoice No");

                entity.Property(e => e.InvoicePosted)
                    .HasMaxLength(255)
                    .HasColumnName("Invoice Posted");

                entity.Property(e => e.IsDeferred)
                    .HasMaxLength(255)
                    .HasColumnName("Is Deferred");

                entity.Property(e => e.IsDtoC)
                    .HasMaxLength(255)
                    .HasColumnName("Is DtoC");

                entity.Property(e => e.IsUnderWarranty)
                    .HasMaxLength(255)
                    .HasColumnName("Is Under Warranty");

                entity.Property(e => e.IsVoucherRedeemed)
                    .HasMaxLength(255)
                    .HasColumnName("Is Voucher Redeemed");

                entity.Property(e => e.Landmark).HasMaxLength(255);

                entity.Property(e => e.LastName)
                    .HasMaxLength(255)
                    .HasColumnName("Last Name");

                entity.Property(e => e.LatestDateTime)
                    .HasMaxLength(255)
                    .HasColumnName("Latest Date & Time");

                entity.Property(e => e.LatestStatus)
                    .HasMaxLength(255)
                    .HasColumnName("Latest Status");

                entity.Property(e => e.LgcTktCreatedDate)
                    .HasMaxLength(255)
                    .HasColumnName("Lgc Tkt Created Date");

                entity.Property(e => e.LgcTktNo)
                    .HasMaxLength(255)
                    .HasColumnName("LGC Tkt No");

                entity.Property(e => e.LogisticBy)
                    .HasMaxLength(255)
                    .HasColumnName("Logistic By");

                entity.Property(e => e.LogisticPic1)
                    .HasMaxLength(255)
                    .HasColumnName("Logistic Pic 1");

                entity.Property(e => e.LogisticPic2)
                    .HasMaxLength(255)
                    .HasColumnName("Logistic Pic 2");

                entity.Property(e => e.LogisticPic3)
                    .HasMaxLength(255)
                    .HasColumnName("Logistic Pic 3");

                entity.Property(e => e.LogisticPic4)
                    .HasMaxLength(255)
                    .HasColumnName("Logistic Pic 4");

                entity.Property(e => e.LogisticsBonus)
                    .HasMaxLength(255)
                    .HasColumnName("logistics Bonus");

                entity.Property(e => e.LogisticsStatusRemark)
                    .HasMaxLength(255)
                    .HasColumnName("Logistics Status Remark");

                entity.Property(e => e.Mode).HasMaxLength(255);

                entity.Property(e => e.ModelNo)
                    .HasMaxLength(255)
                    .HasColumnName("Model No");

                entity.Property(e => e.NatureOfComplaint)
                    .HasMaxLength(255)
                    .HasColumnName("Nature of Complaint");

                entity.Property(e => e.NewBrand)
                    .HasMaxLength(255)
                    .HasColumnName("New Brand");

                entity.Property(e => e.NewPrice)
                    .HasMaxLength(255)
                    .HasColumnName("New Price");

                entity.Property(e => e.NewProdGroup)
                    .HasMaxLength(255)
                    .HasColumnName("New Prod# Group");

                entity.Property(e => e.NewProdType)
                    .HasMaxLength(255)
                    .HasColumnName("New Prod Type");

                entity.Property(e => e.NewSize)
                    .HasMaxLength(255)
                    .HasColumnName("New Size");

                entity.Property(e => e.OldBrand)
                    .HasMaxLength(255)
                    .HasColumnName("Old Brand");

                entity.Property(e => e.OldProdType)
                    .HasMaxLength(255)
                    .HasColumnName("Old Prod Type");

                entity.Property(e => e.OrderDate)
                    .HasMaxLength(255)
                    .HasColumnName("Order Date");

                entity.Property(e => e.OrderFlag)
                    .HasMaxLength(255)
                    .HasColumnName("Order Flag");

                entity.Property(e => e.OrderType)
                    .HasMaxLength(255)
                    .HasColumnName("Order Type");

                entity.Property(e => e.PaymentDate)
                    .HasMaxLength(255)
                    .HasColumnName("Payment Date");

                entity.Property(e => e.PaymentFlag)
                    .HasMaxLength(255)
                    .HasColumnName("Payment Flag");

                entity.Property(e => e.PaymentToCustomer)
                    .HasMaxLength(255)
                    .HasColumnName("Payment To Customer");

                entity.Property(e => e.PdfFileForQc)
                    .HasMaxLength(255)
                    .HasColumnName("pdf file for QC");

                entity.Property(e => e.Pic1)
                    .HasMaxLength(255)
                    .HasColumnName("Pic 1");

                entity.Property(e => e.Pic2)
                    .HasMaxLength(255)
                    .HasColumnName("Pic 2");

                entity.Property(e => e.Pic3)
                    .HasMaxLength(255)
                    .HasColumnName("Pic 3");

                entity.Property(e => e.Pic4)
                    .HasMaxLength(255)
                    .HasColumnName("Pic 4");

                entity.Property(e => e.PickupDate)
                    .HasMaxLength(255)
                    .HasColumnName("Pickup Date");

                entity.Property(e => e.PickupFlag)
                    .HasMaxLength(255)
                    .HasColumnName("Pickup Flag");

                entity.Property(e => e.PickupPriority)
                    .HasMaxLength(255)
                    .HasColumnName("Pickup_Priority");

                entity.Property(e => e.PostingDate)
                    .HasMaxLength(255)
                    .HasColumnName("Posting Date");

                entity.Property(e => e.PostingFlag)
                    .HasMaxLength(255)
                    .HasColumnName("Posting Flag");

                entity.Property(e => e.PreferredQcDateAndTime)
                    .HasMaxLength(255)
                    .HasColumnName("Preferred QC Date and Time");

                entity.Property(e => e.PreferredTimeForQc)
                    .HasMaxLength(255)
                    .HasColumnName("Preferred Time For QC");

                entity.Property(e => e.PriceEndDate)
                    .HasMaxLength(255)
                    .HasColumnName("Price End Date");

                entity.Property(e => e.PriceStartDate)
                    .HasMaxLength(255)
                    .HasColumnName("Price Start Date");

                entity.Property(e => e.ProdSrNo)
                    .HasMaxLength(255)
                    .HasColumnName("Prod# Sr# No#");

                entity.Property(e => e.ProdSrNo1)
                    .HasMaxLength(255)
                    .HasColumnName("Prod Sr No");

                entity.Property(e => e.ProductGroup)
                    .HasMaxLength(255)
                    .HasColumnName(" Product Group");

                entity.Property(e => e.ProductType)
                    .HasMaxLength(255)
                    .HasColumnName("Product Type");

                entity.Property(e => e.ProofOfDelivery)
                    .HasMaxLength(255)
                    .HasColumnName("Proof Of Delivery");

                entity.Property(e => e.PurchasedProduct)
                    .HasMaxLength(255)
                    .HasColumnName("Purchased Product");

                entity.Property(e => e.PurchasedProductCategory)
                    .HasMaxLength(255)
                    .HasColumnName("Purchased Product Category");

                entity.Property(e => e.PurchasedProductModel)
                    .HasMaxLength(255)
                    .HasColumnName("Purchased Product Model");

                entity.Property(e => e.QcByLgcReviseEvcCode)
                    .HasMaxLength(255)
                    .HasColumnName("QC by LGC: Revise EVC Code");

                entity.Property(e => e.QcComment)
                    .HasMaxLength(255)
                    .HasColumnName("QC Comment");

                entity.Property(e => e.QcDate)
                    .HasMaxLength(255)
                    .HasColumnName("QC Date");

                entity.Property(e => e.QcFlag)
                    .HasMaxLength(255)
                    .HasColumnName("QC Flag");

                entity.Property(e => e.ReadyForLogisticTicket)
                    .HasMaxLength(255)
                    .HasColumnName("Ready For Logistic Ticket");

                entity.Property(e => e.ReasonForCancellation)
                    .HasMaxLength(255)
                    .HasColumnName("Reason For Cancellation");

                entity.Property(e => e.RecordStatus)
                    .HasMaxLength(255)
                    .HasColumnName("Record Status");

                entity.Property(e => e.RegdNo)
                    .HasMaxLength(255)
                    .HasColumnName("Regd No");

                entity.Property(e => e.SecondaryOrderFlag)
                    .HasMaxLength(255)
                    .HasColumnName("Secondary Order Flag");

                entity.Property(e => e.Size).HasMaxLength(255);

                entity.Property(e => e.SponsorDataId).ValueGeneratedOnAdd();

                entity.Property(e => e.SponsorName)
                    .HasMaxLength(255)
                    .HasColumnName("Sponsor Name");

                entity.Property(e => e.SponsorOrderNo)
                    .HasMaxLength(255)
                    .HasColumnName("Sponsor Order No");

                entity.Property(e => e.SponsorProgCode)
                    .HasMaxLength(255)
                    .HasColumnName("Sponsor Prog# code");

                entity.Property(e => e.StatusReason)
                    .HasMaxLength(255)
                    .HasColumnName("Status Reason");

                entity.Property(e => e.StoreCode)
                    .HasMaxLength(255)
                    .HasColumnName("Store Code");

                entity.Property(e => e.StoreNameForPurchasedProduct)
                    .HasMaxLength(255)
                    .HasColumnName("Store Name For Purchased Product");

                entity.Property(e => e.StorePhoneNumber)
                    .HasMaxLength(255)
                    .HasColumnName("Store Phone Number");

                entity.Property(e => e.StoreStatus)
                    .HasMaxLength(255)
                    .HasColumnName("Store Status");

                entity.Property(e => e.SvcSCallNo)
                    .HasMaxLength(255)
                    .HasColumnName("SVC's Call No");

                entity.Property(e => e.TotalQuoteP).HasColumnName("Total Quote P");

                entity.Property(e => e.TotalQuoteQ).HasColumnName("Total Quote Q");

                entity.Property(e => e.TotalQuoteR).HasColumnName("Total Quote R");

                entity.Property(e => e.TotalQuoteS).HasColumnName("Total Quote S");

                entity.Property(e => e.UploadDateTime)
                    .HasMaxLength(255)
                    .HasColumnName("Upload Date & Time");

                entity.Property(e => e.VoucherAmount)
                    .HasMaxLength(255)
                    .HasColumnName("Voucher Amount");

                entity.Property(e => e.VoucherCode)
                    .HasMaxLength(255)
                    .HasColumnName("Voucher Code");

                entity.Property(e => e.VoucherRedeemDate)
                    .HasMaxLength(255)
                    .HasColumnName("Voucher Redeem Date");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
