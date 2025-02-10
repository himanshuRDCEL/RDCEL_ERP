using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Crypto;
using RDCELERP.BAL.Interface;
using RDCELERP.BAL.MasterManager;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.DAL.Repository;
using RDCELERP.Model.Base;
using RDCELERP.Model.EVC;
using RDCELERP.Model.EVC_Allocated;
using RDCELERP.Model.EVCdispute;
using static Org.BouncyCastle.Math.EC.ECCurve;
using RDCELERP.Common.Enums;
using RDCELERP.Model.EVC_Portal;
using RDCELERP.Common.Helper;

namespace RDCELERP.Core.App.Pages.EVC_Allocation
{
    public class PrimeProductEVC_AllocateFromModel : BasePageModel
    {
        #region Variable Declaration
        private readonly IEVCManager _EVCManager;
        private readonly ICommonManager _commonManager;
        private readonly IUserManager _UserManager;
        private readonly IStateManager _stateManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICityManager _cityManager;
        private readonly IEntityManager _entityManager;
        public readonly CustomDataProtection _protector;
        public readonly IEVCDisputeManager _eVCDisputeManager;
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        public readonly IOptions<ApplicationSettings> _config;
        IOrderTransRepository _OrderTransRepository;
        IAreaLocalityRepository _areaLocalityRepository;
        IOrderTransactionManager _orderTransactionManager;
        public readonly IEVCPartnerRepository _evCPartnerRepository;



        #endregion
        public PrimeProductEVC_AllocateFromModel(RDCELERP.DAL.Entities.Digi2l_DevContext context, IEVCManager EVCManager, IStateManager StateManager, ICityManager CityManager, IEntityManager EntityManager, IWebHostEnvironment webHostEnvironment, IOptions<ApplicationSettings> config, IUserManager userManager, CustomDataProtection protector, IEVCDisputeManager EVCDisputeManager, IOptions<ApplicationSettings> Config, ICommonManager CommonManager, IOrderTransRepository orderTransRepository, IAreaLocalityRepository areaLocalityRepository, IOrderTransactionManager orderTransactionManager, IEVCPartnerRepository eVCPartnerRepository)
            : base(config)

        {
            _EVCManager = EVCManager;
            _webHostEnvironment = webHostEnvironment;
            _UserManager = userManager;
            _stateManager = StateManager;
            _cityManager = CityManager;
            _entityManager = EntityManager;
            _eVCDisputeManager = EVCDisputeManager;
            _protector = protector;
            _context = context;
            _commonManager = CommonManager;
            _config = Config;
            _OrderTransRepository = orderTransRepository;
            _areaLocalityRepository = areaLocalityRepository;
            _orderTransactionManager = orderTransactionManager;
            _evCPartnerRepository = eVCPartnerRepository;
        }
        public TblWalletTransaction tblWalletTransaction { get; set; }

        [BindProperty(SupportsGet = true)]
        public Allocate_EVCFromViewModel allocate { get; set; }

        // [BindProperty(SupportsGet = true)]
        public List<EVcList> EVcLists = new List<EVcList>();

        public List<EVC_PartnerViewModel> eVC_PartnerViewModels = new List<EVC_PartnerViewModel>();
        public string text { get; set; }
        public List<TblEvcPartner> tblEvcPartnerList { get; set; }

        public string title { get; set; }

        public string PageRedirectionURL { get; set; }
        public IActionResult OnGet(int OId)
        {
            if (OId > 0)
            {
                TblAreaLocality tblAreaLocality = null;
                TblOrderTran tblOrderTran = _OrderTransRepository.GetSingleOrderWithExchangereference(OId);

                allocate = _orderTransactionManager.GetOrderDetailsByOrderTransId(Convert.ToInt32(OId));

                if (tblOrderTran != null)
                {                    
                    allocate.ExchAreaLocalityId = (tblOrderTran.OrderType == (Convert.ToInt32(OrderTypeEnum.Exchange)) ? Convert.ToInt32(tblOrderTran?.Exchange?.CustomerDetails?.AreaLocalityId) : Convert.ToInt32(tblOrderTran.Abbredemption.CustomerDetails.AreaLocalityId));
                    allocate.ABBAreaLocalityId = (tblOrderTran.OrderType == (Convert.ToInt32(OrderTypeEnum.ABB)) ? Convert.ToInt32(tblOrderTran?.Abbredemption?.Abbregistration?.Customer?.AreaLocalityId) : Convert.ToInt32(tblOrderTran?.Abbredemption?.Abbregistration?.Customer?.AreaLocalityId));
                    if (allocate.ExchAreaLocalityId > 0)
                    {
                        tblAreaLocality = _areaLocalityRepository.GetArealocalityById(tblOrderTran.Exchange.CustomerDetails.AreaLocalityId);
                        if (tblAreaLocality != null)
                        {
                            allocate.AreaLocality = tblAreaLocality.AreaLocality;
                        }
                    }
                    if (allocate.ABBAreaLocalityId > 0)
                    {
                        tblAreaLocality = _areaLocalityRepository.GetArealocalityById(tblOrderTran.Abbredemption.Abbregistration.Customer.AreaLocalityId);
                        if (tblAreaLocality != null)
                        {
                            allocate.AreaLocality = tblAreaLocality.AreaLocality;
                        }
                    }

                    if (allocate.ActualBasePrice != null && allocate.ActualBasePrice > 0)
                    {
                        var result = allocate.ExpectedPrice;
                        if (result > 0)
                        {
                            allocate.ExpectedPrice = result;
                        }
                        else if (result == -1)
                        {
                            title = "Expected Price Not Found";
                            text = "Pleace add LGC Cost in our System";
                        }
                        else if (result == -2)
                        {
                            title = "Expected Price Not Found";
                            text = "Pleace add UTCPortal Cost in our System ";
                        }
                        else
                        {
                            title = "Expected Price Not Found";
                            text = "Expected Price Not Found";
                        }
                      
                    }
                    else
                    {
                        allocate.ExpectedPrice = (int?)allocate.ActualBasePrice;
                    }

                    if (!string.IsNullOrEmpty(allocate.CustPin) && allocate.ProductCatId > 0)
                    {
                        List<TblConfiguration> tblConfiguration = _context.TblConfigurations.Where(x => x.IsActive == true).ToList();

                        if (tblConfiguration != null)
                        {
                            TblConfiguration? EVCAssignbyState = tblConfiguration.Where(x => x.Name == EnumHelper.DescriptionAttr(ConfigurationEnum.EVCAssignbyState) && x.Value == "1").FirstOrDefault();
                            if (EVCAssignbyState != null)
                            {

                                if (!string.IsNullOrEmpty(allocate.Custstate) || !string.IsNullOrEmpty(allocate.CustCity) || !string.IsNullOrEmpty(allocate.CustPin))
                                {
                                    List<EVcList> EVcLists1 = (List<EVcList>)_EVCManager.GetEVCListbycityAndpin(allocate.Custstate, allocate.CustCity, allocate.CustPin);
                                    if (EVcLists1 != null && EVcLists1.Count > 0)
                                    {
                                        List<EVcList> EvcList1 = new List<EVcList>();
                                        foreach (var item in EvcList1)
                                        {
                                            // Create an EVcList object and add it to the EVCList
                                            EVcList evclist = new EVcList
                                            {
                                                EvcregistrationId = item.EvcregistrationId,
                                                EvcregdNo = item.EvcregdNo,
                                                BussinessName = item.BussinessName
                                            };
                                            if (evclist != null)
                                            {
                                                EvcList1.Add(evclist);
                                            }
                                        }
                                        allocate.EVCLists = EVcLists1;
                                    }
                                }

                            }
                            TblConfiguration? EVCAssignbypartner = tblConfiguration.Where(x => x.Name == EnumHelper.DescriptionAttr(ConfigurationEnum.EVCAssignbypartner) && x.Value == "1").FirstOrDefault();
                            if (EVCAssignbypartner != null)
                            {

                                if (!string.IsNullOrEmpty(allocate.CustPin))
                                {
                                    tblEvcPartnerList = _evCPartnerRepository.GetAllEvcPartnerListByPincode(allocate.CustPin, allocate.ProductCatId, allocate.ActualProdQltyAtQc,allocate.Ordertype);
                                    if (tblEvcPartnerList != null && tblEvcPartnerList.Count > 0)
                                    {
                                        List<EVC_PartnerViewModel> eVCPartnerViewModels1 = new List<EVC_PartnerViewModel>();
                                        foreach (var item in tblEvcPartnerList)
                                        {
                                            // Create an EVcList object and add it to the EVCList
                                            EVC_PartnerViewModel evclist = new EVC_PartnerViewModel
                                            {
                                                EvcPartnerId = item.EvcPartnerId,
                                                EvcregistrationId = item.EvcregistrationId,
                                                EvcregdNo = item.Evcregistration.EvcregdNo,
                                                BussinessName = item.Evcregistration.BussinessName
                                            };
                                            if (evclist != null)
                                            {
                                                eVCPartnerViewModels1.Add(evclist);
                                            }
                                        }
                                        allocate.eVCPartnerViewModels = eVCPartnerViewModels1;
                                    }
                                }

                                TblConfiguration? EVCAssignbypartnerandWallet = tblConfiguration.Where(x => x.Name == EnumHelper.DescriptionAttr(ConfigurationEnum.EVCAssignbypartnerandWallet) && x.Value == "1").FirstOrDefault();
                                if (EVCAssignbypartnerandWallet != null)
                                {
                                   

                                    if (tblEvcPartnerList != null && tblEvcPartnerList.Count > 0)
                                    {
                                        tblEvcPartnerList = _evCPartnerRepository.GetEvcPartnerListHavingClearBalance(tblEvcPartnerList, allocate.ExpectedPrice);
                                        if (tblEvcPartnerList != null && tblEvcPartnerList.Count > 0)
                                        {
                                            List<EVC_PartnerViewModel> eVCPartnerViewModels1 = new List<EVC_PartnerViewModel>();
                                            foreach (var item in tblEvcPartnerList)
                                            {
                                                // Create an EVcList object and add it to the EVCList
                                                EVC_PartnerViewModel evclist = new EVC_PartnerViewModel
                                                {
                                                    EvcPartnerId = item.EvcPartnerId,
                                                    EvcregistrationId = item.EvcregistrationId,
                                                    EvcregdNo = item.Evcregistration.EvcregdNo,
                                                    BussinessName = item.Evcregistration.BussinessName
                                                };
                                                if (evclist != null)
                                                {
                                                    eVCPartnerViewModels1.Add(evclist);
                                                }
                                            }
                                            allocate.eVCPartnerViewModels = eVCPartnerViewModels1;

                                        }

                                    }

                                }

                                TblConfiguration? EVCAssignbyPartnerandWalletandlastTran = tblConfiguration.Where(x => x.Name == EnumHelper.DescriptionAttr(ConfigurationEnum.EVCAssignbyPartnerandWalletandlastTran) && x.Value == "1").FirstOrDefault();
                                if (EVCAssignbyPartnerandWalletandlastTran != null)
                                {

                                    if (tblEvcPartnerList != null && tblEvcPartnerList.Count > 0)
                                    {
                                        tblEvcPartnerList = _evCPartnerRepository.GetEvcPartnerListHavingOldRecharge(tblEvcPartnerList);
                                        if (tblEvcPartnerList != null && tblEvcPartnerList.Count > 0)
                                        {
                                            List<EVC_PartnerViewModel> eVCPartnerViewModels1 = new List<EVC_PartnerViewModel>();
                                            foreach (var item in tblEvcPartnerList)
                                            {
                                                // Create an EVcList object and add it to the EVCList
                                                EVC_PartnerViewModel evclist = new EVC_PartnerViewModel
                                                {
                                                    EvcPartnerId = item.EvcPartnerId,
                                                    EvcregistrationId = item.EvcregistrationId,
                                                    EvcregdNo = item.Evcregistration.EvcregdNo,
                                                    BussinessName = item.Evcregistration.BussinessName
                                                };
                                                if (evclist != null)
                                                {
                                                    eVCPartnerViewModels1.Add(evclist);
                                                }
                                            }
                                            allocate.eVCPartnerViewModels = eVCPartnerViewModels1;

                                        }
                                    }
                                }

                            }

                        }
                    }

                    if (allocate.eVCPartnerViewModels != null && allocate.eVCPartnerViewModels.Count > 0)
                    {
                        ViewData["EVcLists"] = new SelectList((from s in allocate.eVCPartnerViewModels.ToList()
                                                              select new
                                                              {
                                                                  EvcPartnerId= s.EvcPartnerId,
                                                                 EvcregdNo = s.EvcregdNo + "-" + s.BussinessName
                                                               }), "EvcPartnerId", "EvcregdNo", null);
                    }
                    else
                    {
                        title = "EVC not found!";
                        text = "EVC not found for this order ";
                    }

                    // var PriceAfterQC = Allocate_EVCFromViewModels.ActualBasePrice + 600;


                    #region this code for EVC Price Master and EVC Range Master
                    //var tblConfiguration = _context.TblConfigurations.Where(x => x.Name == "UseEVCPriceMater" && x.IsActive == true).FirstOrDefault();
                    //if (tblConfiguration != null)
                    //{
                    //    if (tblConfiguration.Value == "1")
                    //    {
                    //        TblOrderQc tblOrderQc = _context.TblOrderQcs.Where(x => x.OrderTransId == OId).FirstOrDefault();
                    //        if (tblOrderQc != null)
                    //        {
                    //            Allocate_EVCFromViewModels.ActualProdQltyAtQc = tblOrderQc.QualityAfterQc;
                    //            if (Allocate_EVCFromViewModels.ActualProdQltyAtQc == "Excellent")
                    //            {
                    //                TblEvcPriceMaster tblEvcPriceMaster = _context.TblEvcPriceMasters.Where(x => x.ProductTypeId == tblProductType.Id && x.ProductCategoryId == tblProductCategory.Id).FirstOrDefault();
                    //                Allocate_EVCFromViewModels.ExpectedPrice = tblEvcPriceMaster.EvcP;
                    //            }
                    //            else if (Allocate_EVCFromViewModels.ActualProdQltyAtQc == "Good")
                    //            {
                    //                TblEvcPriceMaster tblEvcPriceMaster = _context.TblEvcPriceMasters.Where(x => x.ProductTypeId == tblProductType.Id && x.ProductCategoryId == tblProductCategory.Id).FirstOrDefault();
                    //                Allocate_EVCFromViewModels.ExpectedPrice = tblEvcPriceMaster.EvcQ;
                    //            }
                    //            else if (Allocate_EVCFromViewModels.ActualProdQltyAtQc == "Average")
                    //            {
                    //                TblEvcPriceMaster tblEvcPriceMaster = _context.TblEvcPriceMasters.Where(x => x.ProductTypeId == tblProductType.Id && x.ProductCategoryId == tblProductCategory.Id).FirstOrDefault();
                    //                Allocate_EVCFromViewModels.ExpectedPrice = tblEvcPriceMaster.EvcR;
                    //            }
                    //            else if (Allocate_EVCFromViewModels.ActualProdQltyAtQc == "NotWorking")
                    //            {
                    //                TblEvcPriceMaster tblEvcPriceMaster = _context.TblEvcPriceMasters.Where(x => x.ProductTypeId == tblProductType.Id && x.ProductCategoryId == tblProductCategory.Id).FirstOrDefault();
                    //                Allocate_EVCFromViewModels.ExpectedPrice = tblEvcPriceMaster.EvcS;
                    //            }
                    //        }
                    //    }
                    //    else
                    //    {
                    //        var PriceAfterQC = Convert.ToInt32(Allocate_EVCFromViewModels.ActualBasePrice);
                    //        var per = 0;
                    //        TblEvcpriceRangeMaster tblEvcpriceRangeMaster = _context.TblEvcpriceRangeMasters.Where(x => x.IsActive == true && x.PriceStartRange <= PriceAfterQC && x.PriceEndRange >= PriceAfterQC).FirstOrDefault();
                    //        if (tblEvcpriceRangeMaster != null)
                    //        {
                    //            per = (int)tblEvcpriceRangeMaster.EvcApplicablePercentage;
                    //            Allocate_EVCFromViewModels.ExpectedPrice = ((PriceAfterQC + ((PriceAfterQC / 100) * per)));
                    //        }
                    //        else
                    //        {
                    //            Allocate_EVCFromViewModels.ExpectedPrice = (int?)Allocate_EVCFromViewModels.ActualBasePrice;
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    Allocate_EVCFromViewModels.ExpectedPrice = (int?)Allocate_EVCFromViewModels.ActualBasePrice;
                    //}
                    #endregion
                }
                else
                {
                    return RedirectToPage("EVC_Allocation/Not_Allocated");
                }
                var baseUrl = _config.Value.BaseURL;
                PageRedirectionURL = baseUrl + "EVC_Allocation/Not_Allocated";
                return Page();
            }
            else
            {
                return RedirectToPage("EVC_Allocation/Not_Allocated");
            }
        }
        public IActionResult OnPostAsync()
        {
            //var baseUrl = _config.Value.BaseURL;
            //PageRedirectionURL = baseUrl + "EVC_Allocation/Not_Allocated";
            int result = 0;
            if (ModelState.IsValid)
            {
                result = _EVCManager.AllocateEVCByPrimeOrder(allocate, _loginSession.UserViewModel.UserId);
                if (result > 0)
                {
                    return RedirectToPage("Not_Allocated");
                }

                return RedirectToPage("Not_Allocated");
            }
            else
                return RedirectToPage("Not_Allocated");

        }


    }
}
