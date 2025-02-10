using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.Program
{
    public class ProgramMasterViewModel: BaseViewModel
    {
        public int ProgramMasterId { get; set; }
        [Required(ErrorMessage = "Field can't be empty")]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
        [Display(Name = "Login Credentials")]
        public string? LoginCredentials { get; set; }
        
        public string? SponsorNew { get; set; }

        
        public string? SponsorName { get; set; }
        [Required]
        [Display(Name = "Program Code")]
        public string? ProgramCode { get; set; }
        [Required]
        public string? Exchange { get; set; }
        [Required]
        [Display(Name = "Exchange Price Code")]
        public string? ExchangePriceCode { get; set; }
        
        [Display(Name = "ABB")]
        public string? Abb { get; set; }
        
        [Display(Name = "ABB Price Code")]
        public string? AbbpriceCode { get; set; }
        
        [Display(Name = "Pre(SVC)QC")]
        public string? PreSvcQc { get; set; }
        
        [Required]
        [Display(Name = "Program Start Date")]
        public string? ProgramStartDate { get; set; }
        
        [Display(Name = "Program End Date")]
        public string? ProgramEndDate { get; set; }
        [Required]
        public string? PaymentTo { get; set; }
    }
    public enum Abb
    {
        Yes,
        No
    }
}
