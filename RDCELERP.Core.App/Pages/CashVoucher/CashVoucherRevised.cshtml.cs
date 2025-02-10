using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.Model.Base;
using RDCELERP.Model.QCComment;

namespace RDCELERP.Core.App.Pages.CashVoucher
{
    public class CashVoucherRevisedModel : PageModel
    {
        #region Variable Declaration
        IQCCommentManager _QcCommentManager;
        IUserManager _UserManager;
        IWebHostEnvironment _webHostEnvironment;
        RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        IVoucherRepository _voucherRepository;
        IBusinessUnitRepository _businessUnitRepository;
        IExchangeOrderRepository _exchangeOrderRepository;
        IBusinessPartnerRepository _businessPartnerRepository;
        IOptions<ApplicationSettings> _baseConfig;
        ICompanyRepository _companyRepository;
        #endregion

        #region Constructor
        public CashVoucherRevisedModel(IQCCommentManager qcCommentManager, IUserManager userManager, IWebHostEnvironment webHostEnvironment, Digi2l_DevContext context, IVoucherRepository voucherRepository, IBusinessUnitRepository businessUnitRepository, IExchangeOrderRepository exchangeOrderRepository, IBusinessPartnerRepository businessPartnerRepository, IOptions<ApplicationSettings> baseConfig, ICompanyRepository companyRepository)
        {
            _QcCommentManager = qcCommentManager;
            _UserManager = userManager;
            _webHostEnvironment = webHostEnvironment;
            _context = context;
            _voucherRepository = voucherRepository;
            _businessUnitRepository = businessUnitRepository;
            _exchangeOrderRepository = exchangeOrderRepository;
            _businessPartnerRepository = businessPartnerRepository;
            _baseConfig = baseConfig;
            _companyRepository = companyRepository;
        }
        #endregion

        [BindProperty(SupportsGet = true)]
        public VoucherDetailsViewModel voucherDetailsViewModel { get; set; }
        TblVoucherVerfication VoucherVerfication = new TblVoucherVerfication();
        public IActionResult OnGet(int id, string companyname)
        {
            if (id > 0 && companyname != null)
            {
                TblCompany tblCompany = null;
                TblExchangeOrder tblExchangeOrder = _exchangeOrderRepository.GetSingleOrder(id);
                if (tblExchangeOrder != null)
                {
                    tblCompany = _companyRepository.GetCompanyId(1007);                    
                    if (tblCompany != null && tblCompany.CompanyId == 1007)
                    {
                        string utclogo = _baseConfig.Value.BaseURL + "DBFiles/Company/" + tblCompany.CompanyLogoUrl;
                        voucherDetailsViewModel.UTClogo = utclogo;
                    }
                    TblBusinessPartner tblBusinessPartner = _businessPartnerRepository.GetBPId(tblExchangeOrder.BusinessPartnerId);
                    if (tblBusinessPartner != null)
                    {
                        TblBusinessUnit tblBusinessUnit = _businessUnitRepository.Getbyid(tblBusinessPartner.BusinessUnitId);
                        if (tblBusinessUnit != null)
                        {
                            tblCompany = _companyRepository.GetByBUId(tblBusinessUnit.BusinessUnitId);
                            {
                                if (tblCompany != null)
                                {
                                    string baseUrl = _baseConfig.Value.BaseURL + "DBFiles/Company/" + tblCompany.CompanyLogoUrl;
                                    voucherDetailsViewModel.BrandLogo = baseUrl;                             
                                }
                                else
                                {
                                    tblCompany = new TblCompany();
                                }
                            }
                        }
                    }
                }
                VoucherVerfication = _voucherRepository.GetVoucherDataByExchangeId(id);
                if (VoucherVerfication != null)
                {
                    voucherDetailsViewModel.VoucherCode = VoucherVerfication.VoucherCode;
                    voucherDetailsViewModel.ExchangePrice = Convert.ToDecimal(VoucherVerfication.ExchangePrice);
                    voucherDetailsViewModel.Brandname = companyname;
                }
            }
            
            return Page();
        }
    }
}
