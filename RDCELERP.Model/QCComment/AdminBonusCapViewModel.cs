using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.QCComment
{
    public class AdminBonusCapViewModel
    {
        public string? CustomerName { get; set; }
        public string? RegdNo { get; set; }
        public string? ProductCategoryName { get; set; }
        public string? ProductTypeName { get; set; }
        public string? BrandName { get; set; }
        public string? Size { get; set; }
        public string? Technology { get; set; }
        public double AverageSellingPrice { get; set; }
        public double ExcellentPriceByASP { get; set; }
        public double QuotedPrice { get; set; }
        public decimal Sweetner { get; set; }
        public double QuotedPriceWithSweetner { get; set; }
        public int BonusCapQC { get; set; }
        public double FinalPriceAfterQC { get; set; }
        public double BonusPercentageByQC { get; set; }
        public double FinalCalculatedWeightage { get; set; }
        public double FinalPriceAfterAdminCap { get; set; }
        public int BonusCapAdmin { get; set; }
        public int OrderTransId { get; set; }
        public int ProductTechnolgyId { get; set; }
        public string? ProductTechnolgyName { get; set; }
    }
}
