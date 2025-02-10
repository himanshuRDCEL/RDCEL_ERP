using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Base;
using RDCELERP.Model.EVC_Portal;

namespace RDCELERP.Model.EVC_Allocated
{
    public class Allocate_EVCFromViewModel: BaseViewModel
    {
        public string? RegdNo { get; set; }
        public string? FirstName { get; set; }
        public string? CustCity { get; set; }
        public string? Custstate { get; set; }
        public string? ExchProdGroup { get; set; }
        public string? OldProdType { get; set; }
        public string? Bonus { get; set; }
        public string? ActualProdQltyAtQc { get; set; }
        public string? CustPin { get; set; }
        public string? SponsorOrderNumber { get; set; }
        public Decimal? ActualBasePrice { get; set; }
        public int EvcregistrationId { get; set; }
        public string? EvcregdNo { get; set; }
        public List<EVcList>? EVCLists { get; set; }
       public int? SelectEVCId { get; set; }
        public int? ExpectedPrice { get; set; }
        public Decimal? NewExpectedPrice { get; set; }//use in primeproduct
        public int orderTransId { get; set; }
        public string? NotAllocatelistids { get; set; }
        public int ExchAreaLocalityId { get; set; }
        public int ABBAreaLocalityId { get; set; }
        public string? AreaLocality { get; set; }
        public int? ProductCatId { get; set; }
        public int Ordertype { get; set; }

        public int? EvcPartnerId { get; set; }
        public List<EVC_PartnerViewModel>? eVCPartnerViewModels { get; set; }

        // Added for EVC Price Algo New Synerio
        public int? ExpectedPriceWithSweetener { get; set; }
        public decimal? SweetenerAmt { get; set; }
    }
    public class EVcList
    {

        public int EvcregistrationId { get; set; }
        public string? EvcregdNo { get; set; }
        public string? RegdAddressLine1 { get; set; }
        public string? State { get; set; }
        public string? City { get; set; }
        public string? PinCode { get; set; }
        public decimal? EvcwalletAmount { get; set; }
        public decimal? RuningBalance { get; set; } 
        public string? BussinessName { get; set; }
        public string? CombinedDisplay
        {
            get { return EvcregdNo + " - " + BussinessName; }
        }

    }
}
