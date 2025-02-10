using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Role;

namespace RDCELERP.Model.Users
{
    public class UserRoleLoginViewModel
    {
         
        public UserViewModel? User { get; set; }
        public UserRoleViewModel? UserRole { get; set; }
        public IList<RoleAccessViewModel>? RoleAccesses { get; set; }
        
    }

    public class User
    {
        public int UserId { get; set; }
        public string? ZohoUserId { get; set; }
        // [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$")]
        
        public string? FirstName { get; set; }
        // [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$")]
        
        public string? LastName { get; set; }
        public string? UserStatus { get; set; }
        
        
        public string? Email { get; set; }
        
        public string? Phone { get; set; }
        public string? Gender { get; set; }
        public string?[] GenderList = new[] { "Male", "Female" };
        public string? ImageName { get; set; }
        public string? ImageURL { get; set; }
        
        //[DataType(DataType.Password)]
        public string? Password { get; set; }
        
        public DateTime? LastLogin { get; set; }

        public int? CompanyId { get; set; }
        public string? RoleName { get; set; }
        public string? FullName
        {
            get { return FirstName + " " + LastName; }
        }
        public string? FullNameAndRole
        {
            get { return FirstName + " " + LastName + " (" + RoleName + ")"; }
        }


    }


    public class UserRole
    {
        public int UserRoleId { get; set; }
        
        public int? RoleId { get; set; }
        public int? UserId { get; set; }
        public string? Role { get; set; }
        public int? CompanyId { get; set; }
    }

    public class RoleAccess
    {
        
    }
   
}
    

