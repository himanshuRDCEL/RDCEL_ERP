using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Helper;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.Model.Base;
using RDCELERP.Model.PinCode;

namespace RDCELERP.Core.App.Pages.PinCode
{
    public class IndexModel : CrudBasePageModel
    {
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        private readonly IPinCodeManager _PinCodeManager;
        private readonly IPinCodeRepository _PinCodeRepository;
        public IndexModel(RDCELERP.DAL.Entities.Digi2l_DevContext context, IPinCodeManager PinCodeManager, IPinCodeRepository PinCodeRepository, IOptions<ApplicationSettings> config, CustomDataProtection protector) : base(config, protector)
        {
            _context = context;
            _PinCodeManager = PinCodeManager;
            _PinCodeRepository = PinCodeRepository;
        }

        [BindProperty(SupportsGet = true)]
        public PinCodeViewModel PinCodeVM { get; set; }
        [BindProperty(SupportsGet = true)]
        public IList<TblPinCode> TblPinCode { get; set; }
        [BindProperty(SupportsGet = true)]
        public TblPinCode TblPinCodeObj { get; set; }
        public IActionResult OnGet()
        {
            TblPinCodeObj = new TblPinCode();

            if (_loginSession == null)
            {
                return RedirectToPage("/index");
            }
            else
            {
                PinCodeVM = _PinCodeManager.GetPinCodeById(_loginSession.UserViewModel.UserId);

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
                if (TblPinCodeObj != null && TblPinCodeObj.Id > 0)
                {
                    TblPinCodeObj = _context.TblPinCodes.Find(TblPinCodeObj.Id);
                }

                if (TblPinCodeObj != null)
                {
                    if (TblPinCodeObj.IsActive == true)
                    {
                        TblPinCodeObj.IsActive = false;
                    }
                    else
                    {
                        TblPinCodeObj.IsActive = true;
                    }
                    TblPinCodeObj.ModifiedBy = _loginSession.UserViewModel.UserId;
                    _context.TblPinCodes.Update(TblPinCodeObj);
                    //  _context.TblRoles.Remove(TblRole);
                    _context.SaveChanges();
                }

                return RedirectToPage("./index");
            }
        }

           
        }
    }




