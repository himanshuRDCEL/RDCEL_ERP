using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class Login
    {
        public Login()
        {
            TblBusinessUnits = new HashSet<TblBusinessUnit>();
            TblSocieties = new HashSet<TblSociety>();
        }

        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? ZohoSponsorId { get; set; }
        public int? SponsorId { get; set; }
        public string? PriceCode { get; set; }
        public int? BusinessPartnerId { get; set; }
        public int? PriceMasterNameId { get; set; }

        public virtual TblPriceMasterName? PriceMasterName { get; set; }
        public virtual ICollection<TblBusinessUnit> TblBusinessUnits { get; set; }
        public virtual ICollection<TblSociety> TblSocieties { get; set; }
    }
}
