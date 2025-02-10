using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblRefurbisherRegistration
    {
        public int RefurbisherId { get; set; }
        public string? RefurbisherName { get; set; }
        public int? EntityTypeId { get; set; }
        public string? TypeofServices { get; set; }
        public string? NatureOfBusiness { get; set; }
        public string? ContactPerson { get; set; }
        public string? Address { get; set; }
        public int? CityId { get; set; }
        public int? StateId { get; set; }
        public string? Country { get; set; }
        public string? PostalCode { get; set; }
        public string? Telephone { get; set; }
        public string? Mobile { get; set; }
        public string? Gstdeclaration { get; set; }
        public string? Gstno { get; set; }
        public string? PanNo { get; set; }
        public string? EmailId { get; set; }
        public string? CompanyRegNo { get; set; }
        public string? AccountHolder { get; set; }
        public string? BankName { get; set; }
        public string? Branch { get; set; }
        public string? BankAccountNo { get; set; }
        public string? Ifsccode { get; set; }
        public string? UtcemployeeName { get; set; }
        public string? UtcemployeeEmail { get; set; }
        public string? UtcemployeeContact { get; set; }
        public string? RefurbisherApprovalName { get; set; }
        public string? UnitDepartment { get; set; }
        public string? ManagerEmail { get; set; }
        public string? ManagerContact { get; set; }
        public bool? IsApproved { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual TblCity? City { get; set; }
        public virtual TblUser? CreatedByNavigation { get; set; }
        public virtual TblEntityType? EntityType { get; set; }
        public virtual TblUser? ModifiedByNavigation { get; set; }
        public virtual TblState? State { get; set; }
    }
}
