using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RDCELERP.Common.Helper;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.Base;
using RDCELERP.Model.BusinessUnit;
using RDCELERP.Model.Master;

namespace RDCELERP.Core.App.Pages.BusinessUnit
{
    public class DetailsModel : BasePageModel
    {
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        public readonly IOptions<ApplicationSettings> _config;
        private CustomDataProtection _protector;
        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();


        public DetailsModel(RDCELERP.DAL.Entities.Digi2l_DevContext context, IOptions<ApplicationSettings> config, CustomDataProtection protector)
        : base(config)
        {
            _context = context;
            _config = config;
            _protector = protector;
        }
        [BindProperty(SupportsGet = true)]
        public IList<TblProductCategory> TblProductCategory { get; set; }
        [BindProperty(SupportsGet = true)]
        public ProductCategoryViewModel ProductCategoryViewModel { get; set; }
        [BindProperty(SupportsGet = true)]
        public List<ProductCategoryViewModel> ProductCategorylist { get; set; }
        [BindProperty(SupportsGet = true)]
        public ProductCategoryViewModel productCategoryViewModel { get; set; }
        [BindProperty(SupportsGet = true)]
        public IList<ProductCategoryViewModel> ProductCategoryViewModelList { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (id != null)
            {
                id = _protector.Decode(id);
            }

            TblProductCategory = await _context.TblProductCategories
                .Where(x => x.IsActive == true).ToListAsync();
            if(TblProductCategory != null)
            {
                ProductCategorylist = new List<ProductCategoryViewModel>();
                foreach(var item in TblProductCategory)
                {
                    productCategoryViewModel  = new ProductCategoryViewModel();
                    productCategoryViewModel.Id = item.Id;
                    productCategoryViewModel.Buid = (Convert.ToInt32(id));
                    productCategoryViewModel.Description = item.Description;
                    ProductCategoryViewModelList.Add(productCategoryViewModel);
                }

            }

            else
            {
                return NotFound();
            }
            return Page();
        }

        [BindProperty(SupportsGet = true)]
        public TblBuproductCategoryMapping TblBUProductCategoryMappingObj { get; set; }
        public IActionResult OnPostAddMappingtable(ProductCategoryViewModel ProductCategorylist)
        {
            if (_loginSession == null)
            {
                return RedirectToPage("/index");
            }
            else
            {
                foreach(var items in ProductCategorylist.ProductCategoryViewModelList)
                {
                    if (items.Selected == true)
                    {
                        TblBUProductCategoryMappingObj = new TblBuproductCategoryMapping();
                        TblBUProductCategoryMappingObj.IsActive = true;
                        TblBUProductCategoryMappingObj.CreatedDate = _currentDatetime;
                        TblBUProductCategoryMappingObj.BusinessUnitId = items.Buid;
                        TblBUProductCategoryMappingObj.ProductCatId = items.Id;
                        _context.TblBuproductCategoryMappings.Add(TblBUProductCategoryMappingObj);
                        //  _context.TblRoles.Remove(TblRole);
                        _context.SaveChanges();
                    }

                }

                return RedirectToPage("./index");
            }
        }
    }
}
