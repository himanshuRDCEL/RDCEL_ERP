using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.PinCode
{
    public class PinCodeViewModel:BaseViewModel
    {

        public int Id { get; set; }
        public string? ZohoPinCodeId { get; set; }
        [Required(ErrorMessage = "Pin code is required.")]
        [RegularExpression(@"^[1-9][0-9]{5}$", ErrorMessage = "Please enter valid 6 digit pin code.")]
        public int? ZipCode { get; set; }
        [Required(ErrorMessage = "Location is required.")]
        public string? Location { get; set; }
        public string? HubControl { get; set; }
        [Required(ErrorMessage = "State is required.")]
        public string? State { get; set; }
        public int? StateId { get; set; }
        public bool? IsAbb { get; set; }
        public bool? IsExchange { get; set; }
        public string? Date { get; set; }
        public string? AreaLocality { get; set; }
        public PinCodeVMExcel? PinCodeVM { get; set; }
        public IFormFile? UploadPinCode { get; set; }
        public List<PinCodeVMExcel>? PinCodeVMList { get; set; }
        public List<PinCodeVMExcel> ?PinCodeVMErrorList { get; set; }

        //public string? StateName { get; set; }
        //public int StateId { get; set; }
        //public int CityId { get; set; }
    }
    public class PinCodeListViewModel
    {
        public List<PinCodeViewModel>? PinCodeViewModelList { get; set; }
        public int Count { get; set; }
    }

    public class PinCodesDataModel
    {
        public int Id { get; set; }
        public int? ZipCode { get; set; }
        public string? Location { get; set; }
        public int CityId { get; set; }
    }

    public class PinCodesModelList
    {
        public List<PinCodesModel>? AllPinCodelistViewModel { get; set; }
    }
    public class PinCodesModel
    {
      
        public string? PinCode { get; set; }
        public bool Active { get; set; }
        
    }

    public class CityIdlist
    {
        public int Id { get; set; }
    }

    public class CitiesID
    {
        public List<CityIdlist>? cityIdlists { get; set; }
    }

    public class mappingZipCode
    {
        public int? ZipCode { get; set; }

    }
}
