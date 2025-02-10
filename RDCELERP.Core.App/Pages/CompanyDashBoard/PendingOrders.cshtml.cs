using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.BAL.MasterManager;
using RDCELERP.Common.Helper;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.Model.Base;
using RDCELERP.Model.SearchFilters;

namespace RDCELERP.Core.App.Pages.CompanyDashBoard
{
    public class PendingOrdersModel : BasePageModel
    {
        #region Variable Declaration
        IBusinessUnitManager _BusinessUnitManager;
        ILogging _logging;
        IBusinessUnitRepository _businessUnitRepository;
        IProductCategoryManager _productCategoryManager;
        #endregion

        #region Constructor
        public PendingOrdersModel(IOptions<ApplicationSettings> config, IBusinessUnitManager BusinessUnitManager, ILogging logging, IBusinessUnitRepository businessUnitRepository, IProductCategoryManager productCategoryManager)
        : base(config)
        {
            _BusinessUnitManager = BusinessUnitManager;
            _logging = logging;
            _businessUnitRepository = businessUnitRepository;
            _productCategoryManager = productCategoryManager;
        }
        #endregion

        #region Bind Properties
        [BindProperty(SupportsGet = true)]
        public SearchFilterViewModel searchFilterVM { get; set; }
        [BindProperty(SupportsGet = true)]
        public int UserId { get; set; }
        #endregion
        public IActionResult OnGet()
        {
            bool? flag = false;
            UserId = _loginSession.UserViewModel.UserId;
            //OnPostPendingForQC();
            var ProductGroup = _productCategoryManager.GetAllProductCategory();
            if (ProductGroup != null)
            {
                ViewData["ProductGroup"] = new SelectList(ProductGroup, "Id", "Description");
            }
            return Page();
        }

        #region Get Pending for QC Order List
        public IActionResult OnPostMailPendingOrdersAsync()
        {
            List<TblBusinessUnit>? businessUnitList = null;
            bool? flag = false;
            try
            {
                businessUnitList = _businessUnitRepository.GetSponsorListForReporting();
                if (businessUnitList != null && businessUnitList.Count > 0)
                {
                    foreach (TblBusinessUnit tblBusinessUnit in businessUnitList)
                    {
                        flag = _BusinessUnitManager.SendReportingMail(tblBusinessUnit);
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("PendingOrdersModel", "PendingForQC", ex);
            }
            return RedirectToPage("./PendingOrders");
        }
        #endregion
    }
}
