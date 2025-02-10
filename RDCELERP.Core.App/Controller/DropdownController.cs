using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using RDCELERP.BAL.Enum;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Constant;
using RDCELERP.Common.Enums;
using RDCELERP.Common.Helper;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.Model.ABBPlanMaster;
using RDCELERP.Model.ABBPriceMaster;
using RDCELERP.Model.AbbRegistration;
using RDCELERP.Model.Base;
using RDCELERP.Model.BusinessPartner;
using RDCELERP.Model.BusinessUnit;
using RDCELERP.Model.City;
using RDCELERP.Model.Company;
using RDCELERP.Model.DealerDashBoard;
using RDCELERP.Model.EVC;
using RDCELERP.Model.ExchangeOrder;
using RDCELERP.Model.ImagLabel;
using RDCELERP.Model.LGC;
using RDCELERP.Model.Master;
using RDCELERP.Model.ModelNumber;
using RDCELERP.Model.PinCode;
using RDCELERP.Model.PriceMaster;
using RDCELERP.Model.Product;
using RDCELERP.Model.ProductQuality;
using RDCELERP.Model.Program;
using RDCELERP.Model.Role;
using RDCELERP.Model.SearchFilters;
using RDCELERP.Model.ServicePartner;
using RDCELERP.Model.State;
using RDCELERP.Model.StoreCode;
using RDCELERP.Model.TimeLine;
using RDCELERP.Model.Users;

namespace RDCELERP.Core.App.Controller
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DropdownController : ControllerBase
    {
        private Digi2l_DevContext _context;
        public ILogging _logging;
        IDropdownManager _dropdownManager;
        public DropdownController(Digi2l_DevContext context, ILogging logging, IDropdownManager dropdownManager)
        {
            _context = context;
            _logging = logging;
            _dropdownManager = dropdownManager;
        }
        
        #region Product category type
        [HttpGet]
        public JsonResult OnGetProductCategoryTypeAsync(int? productCatId)
        {
            JsonResult productTypeListJson = null;
            try
            {
                if (productCatId != null)
                {
                    var productTypeList = _dropdownManager.GetProductTypeByCategoryId(productCatId);
                    productTypeListJson = new JsonResult(productTypeList);
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("DropdownController", "OnGetProductCategoryTypeAsync", ex);
            }
            return productTypeListJson;
        }
        #endregion
    }
}







