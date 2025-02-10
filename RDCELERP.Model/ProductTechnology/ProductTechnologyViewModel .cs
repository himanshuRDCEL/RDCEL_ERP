using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.ProductTechnology
{

    public class ProductTechnologyViewModel : BaseViewModel
    {
        public int ProductTechnologyId { get; set; }
        public string? ProductTechnologyName { get; set; }
        public int? ProductCatId { get; set; }
     

        public String ? ProductCategoryDiscription { get; set; }

        public string ? Date { get; set; } 
        public virtual TblUser? CreatedByNavigation { get; set; }
        public virtual TblUser? ModifiedByNavigation { get; set; }
        public virtual TblProductCategory? ProductCat { get; set; }
        public virtual ICollection<TblExchangeOrder>? TblExchangeOrders { get; set; }
        public virtual ICollection<TblPriceMasterQuestioner>? TblPriceMasterQuestioner { get; set; }
    }
}
