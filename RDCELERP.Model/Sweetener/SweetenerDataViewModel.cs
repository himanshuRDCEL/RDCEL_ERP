using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.Sweetener
{
    public class SweetenerDataViewModel : BaseViewModel
    {
        public decimal? SweetenerBu { get; set; }
        public decimal? SweetenerBP { get; set; }
        public decimal? SweetenerDigi2L { get; set; }
        public decimal? SweetenerTotal { get; set; }
        public string? ErrorMessage { get; set; }
    }
    public class GetSweetenerDetailsDataContract
    {
        public int? BusinessUnitId { get; set; }
        public int? BusinessPartnerId { get; set; }
        public int? BrandId { get; set; }
        public bool? IsSweetenerModalBased { get; set; }
        public int? ModalId { get; set; }
        public int? NewProdCatId { get; set; }
        public int? NewProdTypeId { get; set; }
    }
}
