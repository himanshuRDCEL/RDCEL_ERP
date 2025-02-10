using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.EVC
{   
    public class VendorRegistrationModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Vendor Name is required")]
        public string? VendorName { get; set; }

        [Required(ErrorMessage = "TypeOfServices is required")]
        public string? typeOfServices { get; set; }

        [Required(ErrorMessage = "NatureOfBusiness is required")]
        [Display(Name = "NatureOfBusiness")]
        [StringLength(50)]
        public string? NatureOfBusiness { get; set; }

        [Required(ErrorMessage = "Contact Person is required")]
        public string? contactPerson { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [Display(Name = "Regd Address is required")]
        [StringLength(maximumLength: 100)]
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }

        [Required(ErrorMessage = "Postal code is required.")]
        [RegularExpression(@"^[1-9][0-9]{5}$", ErrorMessage = "Please enter valid 6 digit postal code.")] 
        public string PostalCode { get; set; }

        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Please Enter Valid 10 Digit Mobile Number.")] 
        public string? Telephone { get; set; }

        [Required(ErrorMessage = "Mobile Number is Required")]
        [StringLength(10)]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Please Enter Valid 10 Digit Mobile Number.")] 
        public string? Mobile { get; set; }
        public string? gstDeclaration { get; set; } 
        public string? gstin { get; set;}

        [Required(ErrorMessage ="PAN Number is Required")]
        [RegularExpression(@"[A-Z]{5}[0-9]{4}[A-Z]{1}", ErrorMessage = "Please Provide PAN number in Standard Format.")]
        public string? panNo { get; set;}

        [Required(ErrorMessage = " Email Id is required")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail adress")]
        public string? email { get; set;}
        public string? companyRegNo { get; set; }

        [Required(ErrorMessage = "Account holder's Name is required")]
        public string? accountHolder { get; set; }

        [Required(ErrorMessage = "Bank name is required")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$", ErrorMessage = "First letter should be Capital")]
        [Display(Name = "Bank Name")]
        [StringLength(50)] 
        public string? bankName { get; set; }
        public string? branch { get; set; }

        [RegularExpression(@"^[0-9]{9,18}$", ErrorMessage = "Please Enter Valid Bank Account No")]
        [Required(ErrorMessage = " Bank account no is required")]
        [Display(Name = "Bank Account No ")]
        public string? accountNo { get; set; }

        [Required(ErrorMessage = " IFSC code is required")]
        [RegularExpression(@"^[A-Z]{4}0[A-Z0-9]{6}$", ErrorMessage = "Please Enter Valid 11 Digit IFSC Code ")]
        [Display(Name = "IFSC Code")]
        public string? ifscCode { get; set; }   
        public string? utcEmployeeName { get; set; }

        [Required(ErrorMessage = " Email Id is required")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail adress")]
        public string? utcEmployeeEmail { get; set;}

        [Required(ErrorMessage = "UTC Employee contact is Required")]
        [StringLength(10)]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Please Enter Valid 10 Digit Contact Number.")]
        public string? utcEmployeeContact { get; set; }
        public string? approverName { get; set; }
        public string? unitDepartment { get; set; }

        [Required(ErrorMessage = " Email Id is required")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail adress")]
        public string? managerEmail { get; set; }

        [Required(ErrorMessage = "Manager contact is Required")]
        [StringLength(10)]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Please Enter Valid 10 Digit Contact Number.")]
        public string? managerContact { get; set;}

        [Required(ErrorMessage = "State is required")]
        public int StateId { get; set; }
        [Required(ErrorMessage = "City is required")]
        public int cityId { get; set; }
    }
}
