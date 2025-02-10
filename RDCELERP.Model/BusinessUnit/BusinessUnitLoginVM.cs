using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.ABBPriceMaster;
using RDCELERP.Model.Base;
using RDCELERP.Model.BusinessPartner;
using RDCELERP.Model.Company;
using RDCELERP.Model.Master;
using RDCELERP.Model.PriceMaster;

namespace RDCELERP.Model.BusinessUnit
{
    public class BusinessUnitLoginVM : BaseViewModel
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? ZohoSponsorId { get; set; }
        public int? SponsorId { get; set; }
        public SelectList? PriceMasterNameList { get; set; }
        public int? PriceMasterNameId { get; set; }
        public int? BusinessPartnerId { get; set; }
        public SelectListItem? BusinessPartnerList { get; set; }
        public SelectList? BusinessPartnerSelectList { get; set; }
        public string? QrCodeNameBU { get; set; }
        public string? QrCodeUrlBU { get; set; }
        public string? QrCodeNameBPBU { get; set; }
        public string? QrCodeUrlBPBU { get; set; }
    }
}
