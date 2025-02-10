using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.LGC;
using RDCELERP.Model.Users;

namespace RDCELERP.BAL.Interface
{
  public  interface IDropdownManager
    {
        /// <summary>
        /// Get EVC Dropdown from EVCPODDetails by City Id
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public List<SelectListItem> GetEVCDDListByCityId(int cityId);

        /// <summary>
        /// Get EVC City Dropdown from EVCPODDetails
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="StatusId"></param>
        /// <returns></returns>
        public IEnumerable<SelectListItem> GetEVCCityDDList();

        public List<SelectListItem> GetTimeSlot();
        public IList<SelectListItem> GetProductTypeByCategoryId(int? productCategoryId);

        public List<SelectListItem> GetEVCPartnerlistByEVCID(int EVCID);

        public List<SelectListItem> GetProductCondition();

    }
}
