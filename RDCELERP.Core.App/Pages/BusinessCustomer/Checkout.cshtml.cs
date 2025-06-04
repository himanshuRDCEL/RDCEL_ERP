using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.Model.Base;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.ItemBooking;
using RDCELERP.Model.BusinessCustomer;

namespace RDCELERP.Core.App.Pages.BusinessCustomer
{
    public class CheckoutModel : BasePageModel
    {
        #region Variable Declaration
        private readonly IItemBookingManager _ItemBookingManager;
        private readonly Digi2l_DevContext _context;
        #endregion

        public CheckoutModel(IItemBookingManager ItemBookingManager, Digi2l_DevContext context, IOptions<ApplicationSettings> config)
        : base(config)
        {
            _ItemBookingManager = ItemBookingManager;
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public BookingItemViewModel ItemBookingViewModel { get; set; }


        public IActionResult OnGet(int? id)
        {
            if (id != null)
            {
                ItemBookingViewModel = _ItemBookingManager.GetItemBookingById(Convert.ToInt32(id));
                var BusinessUnit = _context.TblBusinessUnits.Where(x => x.BusinessUnitId == ItemBookingViewModel.BusinessUnitId && x.IsActive == true).FirstOrDefault();
                if (BusinessUnit != null)
                {
                    ItemBookingViewModel.BusinessUnitName = BusinessUnit.Name;
                }
            }



            if (ItemBookingViewModel == null)
                ItemBookingViewModel = new ItemBookingViewModel();

            //ViewData["CountryList"] = new SelectList(_countryManager.GetAllCountries(), "CountryId", "Name");
            if (!string.IsNullOrEmpty(ItemBookingViewModel.ItemBookingLogoUrl))
            {

                ItemBookingViewModel.ItemBookingLogoUrlLink = _baseConfig.Value.BaseURL + "/DBFiles/ItemBooking/" + ItemBookingViewModel.ItemBookingLogoUrl;
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

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public IActionResult OnPostAsync(IFormFile ItemBookingLogo)
        {
            int result = 0;

            if (ItemBookingLogo != null)
            {
                string fileName = Guid.NewGuid().ToString("N") + ItemBookingLogo.FileName;
                //var filePath = string.Concat(_webHostEnvironment.WebRootPath, "\\", @"\DBFiles\ItemBooking");
                //var fileNameWithPath = string.Concat(filePath, "\\", fileName);
                var filePath = Path.Combine("wwwroot\\DBFiles\\ItemBooking");
                string fileNameWithPath = Path.Combine(filePath, fileName);
                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    ItemBookingLogo.CopyTo(stream);
                    ItemBookingViewModel.ItemBookingLogoUrl = fileName;
                }
            }

            result = _ItemBookingManager.ManageItemBooking(ItemBookingViewModel, _loginSession.UserViewModel.UserId);
            if (result > 0)
                return RedirectToPage("Index");
            //return RedirectToPage("Manage", new { id = result });

            else
                return RedirectToPage("Manage");
        }


    }
}
