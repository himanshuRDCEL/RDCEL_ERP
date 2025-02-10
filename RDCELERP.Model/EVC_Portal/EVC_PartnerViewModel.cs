using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Base;


namespace RDCELERP.Model.EVC_Portal
{
    public class EVC_PartnerViewModel : BaseViewModel
    {
        public int EvcPartnerId { get; set; }
        public int EvcregistrationId { get; set; }
        public string? EvcregdNo { get; set; }
        public string? EvcStoreCode { get; set; }
        public string? BussinessName { get; set; }
        public int CityId { get; set; }
        public string? CityName { get; set; }
        public string? Address { get; set; }
        public string? PinCode { get; set; }
        public int StateId { get; set; }
        public string? StateName { get; set; }
        public string? EmailId { get; set; }
        public string? ContactNumber { get; set; }
        public string? Date { get; set; }
        
        //Added by VK
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public bool Isapprove { get; set; }
        public decimal? EvcwalletAmount { get; set; }
        public decimal? RuningBalance { get; set; } 
        public string? CombinedEVCPDisplay
        {
            get { return EvcregdNo + " - " + BussinessName; }
        }
        public bool? IsSweetenerAmtInclude { get; set; }
        public int? GSTTypeId { get; set; }
        public int? ExpectedPrice { get; set; }
    }
    public class EVC_PartnerpreferenceViewModel : BaseViewModel
    {
        public int EvcPartnerpreferenceId { get; set; }
        public string? EvcregdNo { get; set; }
        public string? EvcStoreCode { get; set; }
        public int ProductCatId { get; set; }
        public string? productCategory { get; set; }
        public int ProductQualityId { get; set; }
        public string? ProductQuality { get; set; }
        public string? Date { get; set; }

    }
}
