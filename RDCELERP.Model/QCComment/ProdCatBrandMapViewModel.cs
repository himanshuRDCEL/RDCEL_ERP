using System.ComponentModel.DataAnnotations;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.QCComment
{
    public class ProdCatBrandMapViewModel : BaseViewModel
    {
        public int ProdCatBrandMappingId { get; set; }
        public int BrandId { get; set; }
        public int ProductCatId { get; set; }
        public int BrandGroupId { get; set; }
        public decimal BaseASP { get; set; }
        public decimal FinalASP { get; set; }
        public decimal ExcellentPrice { get; set; }
        public decimal NonWorkingPrice { get; set; }
        public decimal Weightage { get; set; }
        public decimal ASPPercentile { get; set; }
     
    }
}
