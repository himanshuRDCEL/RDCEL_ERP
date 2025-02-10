using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.ExchangeOrder
{
    public class ExchangeVMExcel
    {
        public int Id { get; set; }
        public string? exchangeId { get; set; }
        [Required(ErrorMessage = "Company name is required.")]
        public string? CompanyName { get; set; }
        [Required(ErrorMessage = "Customer first name is required.")]
        public string? CustomerFirstName { get; set; }
        [Required(ErrorMessage = "Customer last name is required.")]
        public string? CustomerLastName { get; set; }
        [Required(ErrorMessage = "Customer email is required.")]
        public string? CustomerEmail { get; set; }
        [Required(ErrorMessage = "Customer city is required.")]
        public string? CustomerCity { get; set; }
        [Required(ErrorMessage = "Customer pincode is required.")]
        public string? CustomerPinCode { get; set; }
        [Required(ErrorMessage = "Customer state is required.")]
        public string? CustomerState{ get; set; }
        [Required(ErrorMessage = "Customer address1 is required.")]
        public string? CustomerAddress1 { get; set; }
        public string? CustomerAddress2 { get; set; }
        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Please Enter Valid 10 Digit Mobile Number.")]
        public string? CustomerPhoneNumber { get; set; }
        [Required(ErrorMessage = "Product type is required.")]
        public string? ProductType { get; set; }

        [Required(ErrorMessage = "Brand is required.")]
        public string? Brand { get; set; }

       
        public string? SponsorOrderNumber { get; set; }

        [Required(ErrorMessage = "Product condition is required.")]
        public string? ProductCondition { get; set; }

        public bool IsDtoC { get; set; }
        public decimal ExchangePrice { get; set; }
        public bool IsDefferedSettlement { get; set; }
       
       
        [Required(ErrorMessage = "Purchased product category is required.")]
        public string? PurchasedProductCategory { get; set; }
        [Required(ErrorMessage = "Store code is required.")]
        public string? StoreCode { get; set; }
        public string? ProductNumber { get; set; }
        public string? NewBrand { get; set; }
        public string? InvoiceNumber { get; set; }
        public string? ModelNumber { get; set; }
        public decimal Sweetener { get; set; }
        public string? SerialNumber { get; set; }
        public decimal FinalExchangePrice { get; set; }
       
        public string? NewProductcategory { get; set; }
        public string? NewProductType { get; set; }
        public string? VoucherCode { get; set; }
        public bool IsVoucherused { get; set; }
        public int ProductTechnologyId { get; set; }
        public string? Remarks { get; set; }
    }
}
