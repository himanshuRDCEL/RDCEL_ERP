using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.EVC
{
    public class EVC_ApprovedViewModel : BaseViewModel
    {
        public int EvcregistrationId { get; set; }
        public string? EmployeeName { get; set; }
        public string? EvcregdNo { get; set; }
        public long  EvcwalletAmount { get; set; }
        public string? ContactPerson { get; set; }       
        public string? BussinessName { get; set; }          
        public string? CityName { get; set; }
        public string? Name { get; set; }//EntityType
        public string? Date { get; set; }

        //this felid use for Excel Inport
        public string? App_Not { get; set; }
        public string? StateName { get; set; }
        public string? Address { get; set; }
        public string? Pincode { get; set; }
        public bool? Isevcapprovrd { get; set; }
        public string? BankName { get; set; }
        public string? Ifsccode { get; set; }
        public string? BankAccountNo { get; set; }
        public string? ContactPersonAddress { get; set; }
        public string? Gstno { get; set; }
        public string? RegdAddressLine1 { get; set; }
        public string? RegdAddressLine2 { get; set; }
        public string? Type { get; set; }
        public string? EvcmobileNumber { get; set; }
        public string? EmailId { get; set; }
        public string? EnitityName { get; set; }
        public int? UserId { get; set; }
       
    }
}
