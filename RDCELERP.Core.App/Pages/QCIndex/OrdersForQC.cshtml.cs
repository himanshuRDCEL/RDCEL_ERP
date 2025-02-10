using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RDCELERP.DAL.Entities;
using RDCELERP.Core.App.Pages.Base;
using Microsoft.Extensions.Options;
using RDCELERP.Model.Base;
using RDCELERP.BAL.Interface;
using RDCELERP.Model.Company;
using RDCELERP.Model.SearchFilters;
using Microsoft.AspNetCore.Mvc.Rendering;
using RDCELERP.Common.Enums;
using RDCELERP.Common.Helper;
using RDCELERP.DAL.IRepository;

namespace RDCELERP.Core.App.Pages.QCIndex
{
    public class OrdersForQCModel : BasePageModel
    {
        #region Variable declartion
        private readonly IProductCategoryManager _productCategoryManager;
        private readonly IProductTypeManager _productTypeManager;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IOrderTransactionManager _orderTransactionManager;
        private readonly Digi2l_DevContext _context;
        #endregion

        #region Constructor
        public OrdersForQCModel(IOptions<ApplicationSettings> config, IProductCategoryManager productCategoryManager, IProductTypeManager productTypeManager, Digi2l_DevContext context, IUserRoleRepository userRoleRepository, IOrderTransactionManager orderTransactionManager)
       : base(config)
        {
            _productCategoryManager = productCategoryManager;
            _productTypeManager = productTypeManager;
            _context = context;
            _userRoleRepository = userRoleRepository;
            _orderTransactionManager = orderTransactionManager;
        }
        #endregion

        [BindProperty(SupportsGet = true)]
        public SearchFilterViewModel searchFilterVM { get; set; }
        [BindProperty(SupportsGet = true)]
        public int? TblReloadTimeMs { get; set; }
        [BindProperty(SupportsGet = true)]
        public string MyIds { get; set; }
        [BindProperty(SupportsGet = true)]
        public bool IsCheckBoxShown { get; set; }
        [BindProperty(SupportsGet = true)]
        public int assignUserId { get; set; }

        public List<TblUserRole> tblUserRoles { get; set; }
        
    public void OnGet()
        {
            searchFilterVM.UserId = _loginSession.UserViewModel.UserId;
            // Get Product Category List
            string userRoleName = _loginSession.RoleViewModel.RoleName;
            //string RoleName1 = EnumHelper.DescriptionAttr(RoleEnum.QcAdmin).ToString();
            List<TblUser> tblUserLIST = new List<TblUser>();
            if(userRoleName == EnumHelper.DescriptionAttr(RoleEnum.QcAdmin) || userRoleName == EnumHelper.DescriptionAttr(RoleEnum.SuperAdmin))
            {
                IsCheckBoxShown=true;

                tblUserRoles = _userRoleRepository.GetListofQCUser();
                if (tblUserRoles!=null && tblUserRoles.Count>0)
                {
                    foreach(var item in tblUserRoles)
                    {
                        TblUser user = new TblUser();
                        user.UserId = item.User.UserId;
                        string fullname = item.User.FirstName + " " + item.User.LastName;
                        user.FirstName = fullname;
                        tblUserLIST.Add(user);
                    }

                }

                

                ViewData["QCUsersList"] = new SelectList(tblUserLIST, "UserId", "FirstName");
            }
            else
            {
                IsCheckBoxShown = false;
            }

            var ProductGroup = _productCategoryManager.GetAllProductCategory();
            if (ProductGroup != null)
            {
                ViewData["ProductGroup"] = new SelectList(ProductGroup, "Id", "Description");
            }
            TblConfiguration tblConfiguration = _context.TblConfigurations.FirstOrDefault(x=>x.Name == EnumHelper.DescriptionAttr(ConfigurationEnum.TblReloadTimeMs));

            if (tblConfiguration != null)
            {
                TblReloadTimeMs = Convert.ToInt32(tblConfiguration.Value);
            }
            else
            {
                TblReloadTimeMs = 30000;
            }


        }

        /*#region Product category type
        public JsonResult OnGetProductCategoryTypeAsync()
        {
            var productTypeList = _productTypeManager.GetProductTypeByCategoryId(Convert.ToInt32(searchFilterVM.ProductCatId));
            if (productTypeList != null)
            {
                ViewData["productTypeList"] = new SelectList(productTypeList, "Id", "Description");
            }
            return new JsonResult(productTypeList);
        }
        #endregion*/

        #region Autopopulate Search Filter for search by RegdNo
        public IActionResult OnGetSearchRegdNo(string term)
        {
            if (term == null)
            {
                return BadRequest();
            }
            //string searchTerm = term.ToString();

            var data = _context.TblExchangeOrders
            .Where(x => x.RegdNo.Contains(term)
            && (x.StatusId == Convert.ToInt32(OrderStatusEnum.OrderCreatedbySponsor)
                                || x.StatusId == Convert.ToInt32(OrderStatusEnum.QCInProgress_3Q)
                                || x.StatusId == Convert.ToInt32(OrderStatusEnum.CallAndGoScheduledAppointmentTaken_3P)
                                || x.StatusId == Convert.ToInt32(OrderStatusEnum.QCAppointmentrescheduled)
                                || x.StatusId == Convert.ToInt32(OrderStatusEnum.ReopenOrder)
                                || x.StatusId == Convert.ToInt32(OrderStatusEnum.InstalledbySponsor)))
            .Select(x => x.RegdNo)
            .ToArray();
            return new JsonResult(data);
        }
        #endregion

        public IActionResult OnPostExportAsync()
        {
            try
            {
                string ids = MyIds;
                bool isAdmin = IsCheckBoxShown;
            }
            catch (Exception ex)
            {
                throw;
            }
            return RedirectToPage("Allocate_EVCFrom", new { ids = MyIds });
        }


        public IActionResult OnGetAllocateOrders()
        {
            string message = "SuccessFully Done";
            try
            {
                string ids = MyIds;
                bool isAdmin = IsCheckBoxShown;
                int adminUserId = _loginSession.UserViewModel.UserId;
                int selectUser = assignUserId;
                if (isAdmin)
                {
                    message = _orderTransactionManager.AssignOrderForQC(adminUserId, selectUser, ids);
                }

            }
            catch (Exception ex)
            {
                throw;
            }
            return new JsonResult(message);
            //return RedirectToPage("Allocate_EVCFrom", new { ids = MyIds });
        }


        //public async Task<IActionResult> OnPostAllocateOrdersAsync(string hdnassignUserId, string hdnExchangeorderIds)
        //{
        //    // Handle the data sent via AJAX
        //    // You can process hdnassignUserId and hdnExchangeorderIds here

        //    // Example: Log the received values
        //    System.Diagnostics.Debug.WriteLine("hdnassignUserId: " + hdnassignUserId);
        //    System.Diagnostics.Debug.WriteLine("hdnExchangeorderIds: " + hdnExchangeorderIds);

        //    // You can perform further processing here

        //    // Return an appropriate response
        //    return new JsonResult(new { success = true }); // For example, returning JSON indicating success
        //}
    }
}
