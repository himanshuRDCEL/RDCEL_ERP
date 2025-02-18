using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.Base
{
    public class ApplicationSettings
    {
        public string? BaseURL { get; set; }
        public string? Digi2l_DevContext { get; set; }
        public string? ExcelConString { get; set; }
        public string? SecurityKey { get; set; }
        public string? FromEmail { get; set; }
        public string? Password { get; set; }
        public string? HostName { get; set; }
        public string? PortNumber { get; set; }
        public string? IsSSL { get; set; }
        public string? UseDefaultCredentials { get; set; }
        public string? BccEmailAddress { get; set; }
        public string? FromDisplayName { get; set; }
        public string? MailjetAPIKey { get; set; }
        public string? MailjetAPISecret { get; set; }
        public string? URLPrefixforProd { get; set; }
        public string? InvoiceImageURL { get; set; }
        public string? ExchangeImagesURL { get; set; }
        public string? SMSKey { get; set; }
        public string? OTPActivatedMin { get; set; }
        public string? SenderName { get; set; }
        public string? MVCBaseURL { get; set; }
        public bool SendEmailFlag { get; set; }
        public string? MVCBaseURLForExchangeInvoice { get; set; }
        public string? MVCBaseURLForABBInvoice { get; set; }
        public string? PODPdfUrl { get; set; }
        public string? YellowAiUrl { get; set; }
        public string? YelloaiSenderNumber { get; set; }
        public string? YellowaiMesssaheType { get; set; }
        public string? YellowAi_ApiKey { get; set; }
        public string? SelfQCImagePath { get; set; }
        public string? VideoQCImagePath { get; set; }
        public string? BillCloudKey { get; set; }
        public string? VoucherProcess { get; set; }
        public string? MVCEntryPoint { get; set; }
        public string? SupportEmail { get; set; }
        public string? ServicePartnerAadhar { get; set; }
        public string? ServicePartnerCancelledCheque { get; set; }
        public string? ServicePartnerGST { get; set; }
        public string? ServicePartnerProfilePic { get; set; }
        public string? CashFreeSecretKey { get; set; }
        public string? cashfreeClientId { get; set; }
        public string? CashfreeAuthUrl { get; set; }
        public string? cashfreeVerifyUPI { get; set; }
        public string? cashfreeAddBeneficiary { get; set; }
        public string? cashfreeVerifyToken { get; set; }
        public string? cashfreeGetBeneficiary { get; set; }
        public string? cashfreeTransaction { get; set; }
        public string? cashfreePaymentStatusAPI { get; set; }
        public string? CashfreeGetWalletBalanceAPI { get; set; }
        public string? cashfreeRemoveBeneficiaryAPI { get; set; }
        public string? merchantId { get; set; }
        public string? secretKey { get; set; }
        public string? ZaakPayUrl { get; set; }
        public string? upiverification { get; set; }

        public string? AddLog { get; set; }

        public string? RetailerId { get; set; }
        public string? ApiToken { get; set; }
        public string? CreateTicketUrl { get; set; }
        public string? CancelTicketUrl { get; set; }
        public string? UpdatedCreateTicketUrl { get; set; }
        public string? TokenForBizLog { get; set; }

        public string? apiURl { get; set; }
        public string? apiKey { get; set; }

        public string? CreateOrderUrl { get; set; }
        public string? GetOrderStatus { get; set; }
        public string? xverify { get; set; }
        public string? MerchantIdPlural { get; set; }
        public string? AccessCodePlural { get; set; }
        public string? SecretKeyPlural { get; set; }
        public string? EncryptionKey { get; set; }
        public string? EncryptionIV { get; set; }

        public bool PluralActive { get; set; }
        public bool ZaakPayActive { get; set; }
        public int MultipleOrderCount { get; set; }
        public bool AllowFutureDateSelectionForRedemption { get; set; }
        public bool AllowPastDateSelectionForRedemption { get; set; }

        public string? TechGuardAPIKey { get; set; }
        public string? CompanyIdTechGuard { get; set; }
        public string? TechGuardUrl { get; set; }
        public string? TechGuardBeneficiary { get; set; }
        public string? TechGuardSupportEmail { get; set; }
        public string? TechGuardsupportPhone { get; set; }
        public string? DriverlicenseImage { get; set; }
        public string? ProfilePicture { get; set; }
        public string? VehicleInsuranceCertificate { get; set; }
        public string? VehiclefitnessCertificate { get; set; }
        public string? VehicleRcCertificate { get; set; }
        public bool IsDiagnostic { get; set; }

        public string? TechGuardsettelemntType { get; set; }
        public string? WebCoreBaseUrl { get; set; }
        public string? WebCoreBaseUrlCD { get; set; }
        public string? PostERPImagePath { get; set; }
        public string? MVCPhysicalURL { get; set; }
        public string? MVCCityPhysicalURL { get; set; }
        public bool EditPageforAbbRegistration { get; set; }
        public string? SenderId { get; set; }
        public string? ServerKey { get; set; }
        public string? ASPPercentage { get; set; }
        public string? cashfreeApiTransaction { get; set; }
        public bool SendPushNotification { get; set; }
        public bool EvcAutoAllocation { get; set; }

        public bool IsUPIQrRequired { get; set; }
        public string LattLongKey { get; set; }

        public string? DriverlicenseImagePath { get; set; }
        public string? ProfilePicturePath { get; set; }
        public string? VehicleInsuranceCertificatePath { get; set; }
        public string? VehiclefitnessCertificatePath { get; set; }
        public string? VehicleRcCertificatePath { get; set; }
        public int? PicupExpectedInHrs { get; set; }
        public string? ExchangemanagepageEdit { get; set; }

        public int? TechGuardBUId { get; set; }
        public string? ERPBaseURL { get; set; }
        public int? EVCBulkzipdownloddiff { get; set; }
        public string? EVCLoginPossword { get; set; }
        public string? LGCLoginPossword { get; set; }
        public int? DriverOrderCount { get; set; }
        public string? DaikinAuthUrl { get; set; }
        public string? DaikinGrantType { get; set; }
        public string? DaikinScope { get; set; }
        public string? DaikinClientId { get; set; }
        public string? DaikinClientSecret { get; set; }
        public bool? DaikinRefreshToken { get; set; }
        public string? DaikinAuthorities { get; set; }
        public string? DaikinPushOrderStatusUrl { get; set; }
        public int? DaikinBUId { get; set; }
        public bool IsDaikinAPIOrderStatus { get; set; }

        public string AiSensyApiURL { get; set; }
        public string AiSensyApiKey { get; set; }

    }
}
