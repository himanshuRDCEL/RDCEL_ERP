using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
using RDCELERP.Model.Master;
using RDCELERP.Model.PriceMaster;

namespace RDCELERP.Core.App.Pages.PriceMasterName
{
    public class ManageModel : CrudBasePageModel
    {

        #region  Constructor & Variable Declaration 
        private readonly IPriceMasterNameRepository _priceMasterNameRepository;
        private readonly ILogging _logging;
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        private readonly IMapper _mapper;
        private readonly IPriceMasterNameManager _priceMasterNameManager;
        private readonly ILogger _logger;


        public ManageModel(IProductTypeManager ProductTypeManager, Digi2l_DevContext context, IMapper mapper, IPriceMasterNameRepository priceMasterNameRepository, IProductCategoryManager productCategoryManager, IPriceMasterNameManager priceMasterNameManager, IOptions<ApplicationSettings> config, CustomDataProtection protector) : base(config, protector)
        {
            _context = context;
            _priceMasterNameRepository = priceMasterNameRepository;
            _mapper = mapper;
           
            _priceMasterNameManager = priceMasterNameManager;
        }
        #endregion


        #region Create Data Objects
        [BindProperty(SupportsGet = true)]
        public PriceMasterNameViewModel PriceMasterNameViewModel { get; set; }
        RDCELERP.DAL.Entities.TblPriceMasterName priceMasterName1 = null;
        #endregion
        public IActionResult OnGet(string id)
        {
            if (id != null)
            {
                id = _protector.Decode(id);
                PriceMasterNameViewModel = _priceMasterNameManager.GetPriceMasterNameById(Convert.ToInt32(id));
            }

            if (_loginSession == null)
            {
                return RedirectToPage("/index");
            }
            else
            {
                return Page();
            }

        }

        public async Task<IActionResult> OnPostAsync()
        {
            int result = 0;
            RDCELERP.DAL.Entities.TblPriceMasterName priceMasterName = null;
            try
            {
                if (ModelState.IsValid)
                {

                    if (ModelState.IsValid)
                    {
                        result = _priceMasterNameManager.ManagePriceMasterName(PriceMasterNameViewModel, _loginSession.UserViewModel.UserId);
                    }

                    
                    if (result == 0)
                    {

                        ViewData["Message"] = "This Category is already exist";

                        return Page();

                    }
                }
                else
                {
                    result = 0;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("PriceMasterName-ManageModel", "Post", ex);
            }
            if (result > 0)
                return RedirectToPage("./Index", new { id = _protector.Encode(result) });
            else
                return RedirectToPage("./Manage");
        }

        public IActionResult OnPostCheckName(string mastername)
        {
            bool isValid = false;
            string Message = string.Empty;
            RDCELERP.DAL.Entities.TblPriceMasterName priceMasterNamecheck = null;
            try
            {
                if (!string.IsNullOrEmpty(mastername))
                {
                    string nameTrimmed = mastername?.Trim(); // Trim the mastername parameter

                    priceMasterNamecheck = _priceMasterNameRepository.GetSingle(x => x.IsActive == true && x.Name.ToLower() == nameTrimmed.ToLower());

                    if (priceMasterNamecheck == null)
                        isValid = true;

                    return new JsonResult(isValid);
                }
                else
                {
                    Message = "NullValue";
                    return new JsonResult(Message);
                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("PriceMasterName_ManageModel", "OnPostCheckName", ex);
            }

            return new JsonResult(isValid);
        }

        
    }
}
