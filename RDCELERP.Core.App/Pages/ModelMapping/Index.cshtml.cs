using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.Base;
using RDCELERP.Model.ModelNumber;

namespace RDCELERP.Core.App.Pages.ModelMapping
{
    public class IndexModel : CrudBasePageModel
    {
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        private readonly IModelNumberManager _modelNumberManager;
        private readonly IProductCategoryManager _productCategoryManager;
        private readonly IProductTypeManager _productTypeManager;

        public IndexModel(RDCELERP.DAL.Entities.Digi2l_DevContext context, IModelNumberManager modelNumberManager, IOptions<ApplicationSettings> config, CustomDataProtection protector) : base(config, protector)
        {
            _context = context;
            _modelNumberManager = modelNumberManager;
            
        }

        [BindProperty(SupportsGet = true)]
        public ModelMappingViewModel ModelmappingVM { get; set; }

        [BindProperty(SupportsGet = true)]
        public IList<TblModelMapping> TblModelMapping { get; set; }
        [BindProperty(SupportsGet = true)]
        public TblModelMapping TblModelMappingObj { get; set; }
        public IActionResult OnGet()
        {
            TblModelMappingObj = new TblModelMapping();
            

            if (_loginSession == null)
            {
                return RedirectToPage("/index");
            }
            else
            {
                ModelmappingVM = _modelNumberManager.GetModelMappingById(_loginSession.UserViewModel.UserId);

                return Page();
            }
        }

        public IActionResult OnPostDeleteAsync()
        {
            if (_loginSession == null)
            {
                return RedirectToPage("/index");
            }
            else
            {
                if (TblModelMappingObj != null && TblModelMappingObj.Id > 0)
                {
                    TblModelMappingObj = _context.TblModelMappings.Find(TblModelMappingObj.Id);
                }

                if (TblModelMappingObj != null)
                {
                    if (TblModelMappingObj.IsActive == true)
                    {
                        TblModelMappingObj.IsActive = false;
                    }
                    else
                    {
                        TblModelMappingObj.IsActive = true;
                    }

                    
                    _context.TblModelMappings.Update(TblModelMappingObj);
                    //  _context.TblRoles.Remove(TblRole);
                    _context.SaveChanges();
                }

                return RedirectToPage("./index");
            }


        }
       
    }
}
