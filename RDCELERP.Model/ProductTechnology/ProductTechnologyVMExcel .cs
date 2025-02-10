using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.ProductTechnology
{
    public class ProductTechnologyVMExcel : BaseViewModel
    {

        public int ProductTechnologyId { get; set; }
        public string? ProductTechnologyName { get; set; }
        public int? ProductCatId { get; set; }

        public virtual TblUser? CreatedByNavigation { get; set; }
        public virtual TblUser? ModifiedByNavigation { get; set; }
        public virtual TblProductCategory? ProductCat { get; set; }
        public virtual ICollection<TblExchangeOrder>? TblExchangeOrders { get; set; }
        public virtual ICollection<TblPriceMasterQuestioner>? TblPriceMasterQuestioner { get; set; }
    }
}
