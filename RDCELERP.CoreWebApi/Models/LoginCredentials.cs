using System.ComponentModel.DataAnnotations;

namespace RDCELERP.CoreWebApi.Models
{
    public class LoginCredentials
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
