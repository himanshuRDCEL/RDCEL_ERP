using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.Model.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.Model.VoucherRedemption;
using Microsoft.AspNetCore.Mvc;

namespace RDCELERP.Core.App.Pages.Voucher
{
    public class VoucherVerfication : BasePageModel
    {
        #region Variable discussion
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        private readonly IBusinessUnitRepository _businessUnitRepository;
        private readonly IVoucherRedemptionManager _voucherRedemptionManager;
        //DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();
        private readonly IPriceMasterMappingRepository _priceMasterMappingRepository;

        #endregion

        #region Constructor
        public VoucherVerfication(IOptions<ApplicationSettings> config, IBusinessUnitRepository businessUnitRepository, IVoucherRedemptionManager voucherRedemptionManager, IPriceMasterMappingRepository priceMasterMappingRepository, Digi2l_DevContext context)
            : base(config)
        {
            _businessUnitRepository = businessUnitRepository;
            _voucherRedemptionManager = voucherRedemptionManager;
            _priceMasterMappingRepository = priceMasterMappingRepository;
            _context = context;
        }
        #endregion

        #region Bind Properties
        [BindProperty(SupportsGet = true)]
        public VoucherDataContract? voucherDataContract { get; set; }
        
        
        [BindProperty(SupportsGet = true)]
        public loginUserDetailsforVoucher? LoginUserDetailsforVoucher { get; set; }


        #endregion

        public IActionResult OnGet()
        {
            if (_loginSession == null)
            {
                TempData["Message"] = "Not allowed to access this page";
                return RedirectToPage("/Voucher/Thankyou");
            }
            else
            {
                var sessionLoginobj = _loginSession.UserViewModel;
                

            }

            return Page();

        }
        public IActionResult OnPostAsync()
        {
            try
            {
                string urlforback = "/Voucher/VoucherVerfication";
                if (_loginSession.RoleViewModel != null && _loginSession.RoleViewModel.CompanyId > 0)
                {
                    int userCompanyId = Convert.ToInt32(_loginSession.RoleViewModel.CompanyId);
                    if (_loginSession.UserViewModel != null && _loginSession.UserViewModel.UserId > 0)
                    {
                        var lgnId = Convert.ToInt32(_loginSession.UserViewModel.UserId);
                        voucherDataContract.loginUserDetailsforVoucher.userId = Convert.ToInt32(lgnId);
                    }
                }

                if (voucherDataContract!=null && voucherDataContract.VoucherCode != string.Empty && voucherDataContract.PhoneNumber != string.Empty )
                {
                    TblUserMapping? tblUserMapping = _context.TblUserMappings.Where(x => x.IsActive == true && x.UserId == voucherDataContract.loginUserDetailsforVoucher.userId).FirstOrDefault();
                    if (tblUserMapping != null)
                    {
                        voucherDataContract.loginUserDetailsforVoucher.businessPartnerId =Convert.ToInt32(tblUserMapping.BusinessPartnerId);
                        voucherDataContract.loginUserDetailsforVoucher.businessUnitId =Convert.ToInt32(tblUserMapping.BusinessUnitId);
                        //VerifyVoucherResult verifyVoucherResult = _voucherRedemptionManager.VerifyVoucherCode(voucherDataContract);
                        voucherDataContract = _voucherRedemptionManager.VerifyVoucherCode(voucherDataContract);
                        
                        if (voucherDataContract.verifyVoucherResult.isVerified)
                        {
                            TempData["VoucherCode"] = voucherDataContract.VoucherCode;
                            TempData["ReturnUrl"] = urlforback;
                            return RedirectToPage("/Voucher/VoucherDetails", voucherDataContract);
                        }
                        else
                        {
                            TempData["Message"] = voucherDataContract.verifyVoucherResult.responseMesage;
                            TempData["ReturnUrl"] = urlforback;
                            return RedirectToPage("/Voucher/Thankyou");
                        }
                    }
                    else
                    {
                        TempData["Message"] = "User not mapped with businesspartner and businessunit";
                        TempData["ReturnUrl"] = urlforback;
                        return RedirectToPage("/Voucher/Thankyou");
                    }

                }
                else
                {
                    TempData["Message"] = "data not found for login user";
                    TempData["ReturnUrl"] = urlforback;
                    return RedirectToPage("/Voucher/Thankyou");
                }

            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message.ToString();

                return RedirectToPage("/Voucher/Thankyou");
            }
        }



      
    }
}

