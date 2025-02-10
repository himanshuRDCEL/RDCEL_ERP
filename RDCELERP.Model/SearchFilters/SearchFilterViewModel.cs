using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.SearchFilters
{
    public class SearchFilterViewModel : BaseViewModel
    {
        public int Id { get; set; }
        [DisplayName("Product Type")]
        public SelectListItem? ProductTypeList { get; set; }
        [DisplayName("Product Category")]
        public SelectListItem? ProductCatList { get; set; }
        public string? RegdNo { get; set; }
        public string? TicketNum { get; set; }
        public int? ProductCatId { get; set; }
        public int? ProductTypeId { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }

        public int companyId { get; set; }
        public string? orderStartDate { get; set; }
        public string? orderEndDate { get; set; }
        public string? resheduleStartDate { get; set; }
        public string? resheduleEndDate { get; set; }
        public string? productCatId { get; set; }
        public string? productTypeId { get; set; }
        public string? phoneNo { get; set; }
        public string? custCity { get; set; }
        public int? UserId { get; set; }



    }
}
