using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Enums;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.Base;
using RDCELERP.Model.Master;
using RDCELERP.Model.ModelNumber;
using RDCELERP.Model.ProductConditionLabel;

namespace RDCELERP.Core.App.Pages.NewProductConditionLabel
{
    public class ManageModel : CrudBasePageModel
    {

        #region Variable Declaration
        private readonly IProductConditionLabelManager _ProductConditionLabelManager;
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        #endregion

        public ManageModel(IProductConditionLabelManager ProductConditionLabelManager, Digi2l_DevContext context, IOptions<ApplicationSettings> config, CustomDataProtection protector) : base(config, protector)
        {
            _ProductConditionLabelManager = ProductConditionLabelManager;
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public ProductConditionLabelViewModel ProductConditionLabelViewModel { get; set; }

        public IActionResult OnGet(string id)
        {
            var enumData = from EvcPartnerPreferenceEnum e in Enum.GetValues(typeof(EvcPartnerPreferenceEnum))
                           select new
                           {
                               Id = (int)e,
                               Name = e.ToString()
                           };
            ViewData["OrderSequenceList"] = new SelectList(enumData, "Id", "Name");


            if (id != null)
            {
                id = _protector.Decode(id);
                ProductConditionLabelViewModel = _ProductConditionLabelManager.GetProductConditionLabelById(Convert.ToInt32(id));
                TblBusinessUnit businessUnit = _context.TblBusinessUnits.Where(x => x.BusinessUnitId == ProductConditionLabelViewModel.BusinessUnitId).FirstOrDefault();
                ProductConditionLabelViewModel.BusinessUnitName = businessUnit.Name;
                TblBusinessPartner BusinessPartner = _context.TblBusinessPartners.Where(x => x.BusinessPartnerId == ProductConditionLabelViewModel.BusinessPartnerId).FirstOrDefault();
                if (BusinessPartner != null)
                {
                    ProductConditionLabelViewModel.BusinessPartnerName = BusinessPartner.Name;
                }
            }



            if (ProductConditionLabelViewModel == null)
                ProductConditionLabelViewModel = new ProductConditionLabelViewModel();

            if (_loginSession == null)
            {
                return RedirectToPage("/index");
            }
            else
            {
                return Page();
            }

        }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            int result = 0;
            if (ModelState.IsValid)
            {
                result = _ProductConditionLabelManager.ManageProductConditionLabel(ProductConditionLabelViewModel, _loginSession.UserViewModel.UserId);

            }

            if (result > 0)
                return RedirectToPage("./Index", new { id = _protector.Encode(result) });
            else

                return Page();
        }

        public IActionResult OnGetSearchBUName(string term)
        {
            if (term == null)
            {
                return BadRequest();
            }
            var list = _context.TblBusinessUnits
                       .Where(e =>!string.IsNullOrEmpty(e.Name) && e.Name.Contains(term) && e.IsActive == true)
                        .Select(s => new SelectListItem
                        {
                            Value = s!.Name!.ToString(),
                            Text = s.BusinessUnitId.ToString()
                        })
                       .ToArray();
            return new JsonResult(list);
        }


        public IActionResult OnGetSearchBPName(string term)
        {
            if (term == null)
            {
                return BadRequest();
            }
            var list = _context.TblBusinessPartners
                       .Where(e =>!string.IsNullOrEmpty(e.Name) && e.Name.Contains(term) && e.IsActive == true)
                        .Select(s => new SelectListItem
                        {
                            Value = s!.Name!.ToString(),
                            Text = s.BusinessPartnerId.ToString()
                        })
                       .ToArray();
            return new JsonResult(list);
        }
    }

}

