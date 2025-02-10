using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Base;
using RDCELERP.Model.EVC_Portal;

namespace RDCELERP.Model.EVC_Allocated
{
    public class ReassignFromViewModel : BaseViewModel
    {
        public string? RegdNo { get; set; }
        public int? StatusId { get; set; }
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
        public int? ProductCatId { get; set; }       
        public int ordertype { get; set; }

        public int OldEvcregistrationId { get; set; }
        public int newEvcregistrationId { get; set; }
        public int newEVCPartnerId { get; set; }


        public string? OldEvcregdNo { get; set; }
        public string? OldEvcName { get; set; }
        public string? OldEVCState { get; set; }
        public string? OldEVCCity { get; set; }
        public string? OldEVCPinCode { get; set; }
        public decimal? OldEVCEvcwalletAmount { get; set; }
        public decimal? OldEVCClearBalance { get; set; } = 0;
        public int? OldEvcExpectedPrice { get; set; }
        public string? PrimeProduct { get; set; }

      
        public List<EVcListRessign>? EVcListRessigns { get; set; }
        public int? SelectEVCId { get; set; }
        public int? ExpectedPrice { get; set; }
        public Decimal? NewExpectedPrice { get; set; }//use in primeproduct
        public int orderTransId { get; set; }
        public string? ReallocationComment { get; set; }
        public string? AreaLocality { get; set; }
        public int ExchAreaLocalityId { get; set; }
        public int ABBAreaLocalityId { get; set; }
        public string? OldEVCStoreCode { get; set; }
        public int EVCPartnerId { get; set; }

    }
    public class EVcListRessign
    {
        public int EvcregistrationId { get; set; }
        public string? EvcregdNo { get; set; }
        public string? RegdAddressLine1 { get; set; }
        public string? State { get; set; }
        public string? City { get; set; }
        public string? PinCode { get; set; }
        public decimal? EvcwalletAmount { get; set; }
        public decimal? RuningBalance { get; set; } = 0;
        public string? BussinessName { get; set; }
        public int EvcPartnerId { get; set; }
        public string? CombinedDisplay
        {
            get { return EvcregdNo + " - " + BussinessName; }
        }        
    }

    public class EvcLattLongModel
    {
        public double? EvcPartnerlatitude { get; set; }
        public double? EvcPartnerlongitude { get; set; }
    }
}
