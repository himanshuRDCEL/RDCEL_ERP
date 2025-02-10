using RDCELERP.Model.Base;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.Company
{
    public class AddressViewModel : BaseViewModel
    {
        public int UsersAddressId { get; set; }
        public int? UserId { get; set; }
        public int? CompanyId { get; set; }
        [Display(Name = "Address Title")]
        public string? Title { get; set; }
        [Display(Name = "Address 1")]
        public string? Address1 { get; set; }
        [Display(Name = "Address 2")]
        public string? Address2 { get; set; }
        [Display(Name = "Address 3")]
        public string? Address3 { get; set; }
        [Display(Name = "Select City")]
        public int? CityId { get; set; }
        [Display(Name = "Select State")]
        public int? StateId { get; set; }
        [Display(Name = "Select Country")]
        public int? CountryId { get; set; }
        [Display(Name = "Zipcode")]
        public string? ZipCode { get; set; }
    }

    public class AddressListViewModel
    {
        public List<AddressViewModel>? AddressViewModelList { get; set; }
        public int Count { get; set; }
    }
}
