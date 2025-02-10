using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.Users
{
    public class UserRolesView : BaseViewModel
    {
        public int UserRoleId { get; set; }
        public string? UserRoleIds { get; set; }
        [Required]
        [Display(Name = "Role")]
        public int? RoleId { get; set; }
        public string? UserIds { get; set; }
        public int? UserId { get; set; }
        [Required]
        [Display(Name = "Company")]
        public int? CompanyId { get; set; }
        public string? CompanyName { get; set; }
        public string? Role { get; set; }
        [Required]
        [Display(Name = "Name")]
        [StringLength(50)]
        public string? FirstName { get; set; }
        [Required]
        [Display(Name = "Name")]
        [StringLength(50)]
        public string? LastName { get; set; }

        public string? Action { get; set; }
        public string? ZohoUserId { get; set; }
        // [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$")]
        public string? UserStatus { get; set; }
        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        [Required]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Please Enter Valid 10 Digit Mobile Number.")]
        [Display(Name = "Phone")]

        public string? Phone { get; set; }
        public string? Gender { get; set; }
        public string?[] GenderList = new[] { "Male", "Female" };
        public string? ImageName { get; set; }
        public string? ImageURL { get; set; }

        [Display(Name = "Password")]
        //[DataType(DataType.Password)]
        public string? Password { get; set; }
        [DataType(DataType.Date)]
        public DateTime? LastLogin { get; set; }
        public string? UnEncPassword { get; set; }
        
        public string? RoleName { get; set; }

        public string? Name { get; set; }
        public string? Date { get; set; }

    }
}

