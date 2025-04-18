using Microsoft.AspNetCore.Mvc.Rendering;
using RDCELERP.Model.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.EcomVoucher
{
    public class EcomVoucherViewModel :BaseViewModel
    {
        public int EcomVoucherId { get; set; }
        [Required(ErrorMessage = "Required")]

        public int? EcomVoucherType { get; set; }
        public string? VoucherCode { get; set; }
        [StringLength(10)]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Please Enter Valid 10 Digit Mobile Number.")]
        public string? Phoneno { get; set; }
        public int? BrandId { get; set; }
        public string? CategoryIds { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string EndDateString { get; set; }
        public string? VoucherStatus { get; set; }
        public int? CompanyId { get; set; }
        public string EcomVoucherName { get; set; }

        public List<SelectListItem> BrandList { get; set; }
        public List<SelectListItem> CategoryList { get; set; }
        public List<int> SelectedCategoryIds { get; set; }

        public List<SelectListItem> EcomVoucherTypeList { get; set; }
        public List<SelectListItem> EcomVoucherValueTypeList { get; set; }
        public int? VoucherCount { get; set; }
        public int? ValueType { get; set; }
        public int? FixedValue { get; set; }
        public int? Percentage { get; set; }
        public int? PercLimit { get; set; }
        public bool? IsUsed { get; set; }

        public List<EcomVoucher> PhoneNumbers { get; set; }
        public List<EcomPhoneSpecificsViewModel> EcomPhoneSpecificsListVM { get; set; }




    }
    public class EcomVoucher
    {
        public string PhoneNumber { get; set; }
        public string VoucherCode { get; set; }
    }
}
