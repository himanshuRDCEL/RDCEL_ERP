using RDCELERP.Model.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.DAL.Entities;

namespace RDCELERP.Model.Users
{
    public class UserViewModel : BaseViewModel
    {

        public int UserId { get; set; }
        public string? ZohoUserId { get; set; }
        public string? ImageName { get; set; }
        // [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$")]
        [Required]
        [RegularExpression(@"^[A-Za-z][a-zA-Z\s]*$", ErrorMessage = "Use letters only please")]
        [Display(Name = "First Name")]
        [StringLength(50)]
        public string? FirstName { get; set; }
        // [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$")]
        [Required]
        [RegularExpression(@"^[A-Za-z][a-zA-Z\s]*$", ErrorMessage = "Use letters only please")]
        [Display(Name = "Last Name")]
        [StringLength(50)]
        public string? LastName { get; set; }
        public string? UserStatus { get; set; }
        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Phone Number Required!")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$",
                   ErrorMessage = "Entered phone format is not valid.")]

        public string? Phone { get; set; }
        public string? Gender { get; set; }
        public string?[] GenderList = new[] { "Male", "Female" };

        public string? ImageURL { get; set; }

        [Display(Name = "Password")]
        //[DataType(DataType.Password)]
        public string? Password { get; set; }
        [DataType(DataType.Date)]
        public DateTime? LastLogin { get; set; }
        public string? UnEncPassword { get; set; }
        public int? CompanyId { get; set; }
        public string? RoleName { get; set; }
        public string? FullName
        {
            get { return FirstName + " " + LastName; }
        }
        public void SplitFullName(string fullName)
        {
            var names = fullName.Split(' ');
            this.FirstName = names[0];
            if (names.Length > 1)
            {
                this.LastName = names[1];
            }
        }
        public string? FullNameAndRole
        {
            get { return FirstName + " " + LastName + " (" + RoleName + ")"; }
        }
        public bool IsSelected { get; set; }
        public List<UserViewModel>? UserViewModelList { get; set; }
        public int? BusinessUnitId { get; set; }
        public string? Name { get; set; }
        public string? MailTemplate { get; set; }
        public string? MailSubject { get; set; }
        public int[]? UserIdList { get; set; }
    }

    public class UserDetailsDataModel
    {
        public int UserId { get; set; }
        public string? ImageName { get; set; }
        public string? ImageURL { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserStatus { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }

        public string? Gender { get; set; }
        // public string? Password { get; set; }
        public DateTime? LastLogin { get; set; }
        public int? CompanyId { get; set; }
        public string? RoleName { get; set; }
        public string? FullName
        {
            get { return FirstName + " " + LastName; }
        }
        public bool IsSelected { get; set; }
        //public bool IsActive { get; set; }

        public List<UserViewModel>? UserViewModelList { get; set; }
        public IList<UserViewModel>? UserViewModelLists { get; set; }

    }

    public class UserListViewModel
    {
        public List<UserViewModel>? UserViewModelList { get; set; }
        public int Count { get; set; }

    }

    public class UserLoginModel
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
        public bool Selected { get; set; }
    }

    public class UserPasswordViewModel
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public string? Email { get; set; }
        public string? OldGetPassword { get; set; }
        public bool IsOldPasswordCorrect { get; set; }
        [Display(Name = "Old Password")]
        [DataType(DataType.Password)]
        [Required]
        public string? OldPassword { get; set; }
        [Display(Name = "New Password")]
        [DataType(DataType.Password)]
        [Required]
        public string? Password { get; set; }
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Required]
        [Compare("Password")]
        public string? ConfirmPassword { get; set; }
        public string? ResponseMessage { get; set; }
    }

    public class userDataViewModal
    {
        public int UserId { get; set; }
        public bool IsActive { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Phone { get; set; }

    }


}
