using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.QuestionerLOV
{
    
     public class  QuestionerLOVVMExcel : BaseViewModel
    {

       
        public int QuestionerLovid { get; set; }
        public string? QuestionerLovname { get; set; }
        public int? QuestionerLovparentId { get; set; }
        public decimal? QuestionerLovratingWeightage { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? Date { get; set; }
        public virtual TblUser? CreatedByNavigation { get; set; }
        public virtual TblUser? ModifiedByNavigation { get; set; }
        public virtual ICollection<TblOrderQcrating> TblOrderQcratings { get; set; }
        public virtual ICollection<TblQcratingMaster> TblQcratingMasters { get; set; }
    }
}
