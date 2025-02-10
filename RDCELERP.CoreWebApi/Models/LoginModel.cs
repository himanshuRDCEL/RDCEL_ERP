using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RDCELERP.CoreWebApi.Models
{
    public class LoginModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string email { get; set; }
        public string ZohoSponsorId { get; set; }
        public int? SponsorId { get; set; }
        public string PriceCode { get; set; }
        public int? BusinessPartnerId { get; set; }

    }
}
