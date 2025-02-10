using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.BAL.MasterManager;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.Model.Base;
using RDCELERP.Model.EVC_Portal;
using RDCELERP.Model.ImagLabel;
using RDCELERP.Model.LGC;

namespace RDCELERP.Core.App.Pages.LGC_Admin
{
    public class OrderViewPageModel : BasePageModel
    {
        #region Variable declartion
        private readonly ILogisticManager _logisticManager;
        private readonly IEVCManager _evcManager;
        private readonly ILogisticsRepository _logisticsRepository;
        private readonly INotificationManager _notificationManager;
        private readonly IWhatsappNotificationManager _whatsappNotificationManager;
        IWhatsAppMessageRepository _WhatsAppMessageRepository;
        private CustomDataProtection _protector;
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        IAreaLocalityRepository _areaLocalityRepository;
        #endregion

        #region Constructor
        public OrderViewPageModel(IOptions<ApplicationSettings> config, ILogisticManager logisticManager, ILogisticsRepository logisticsRepository, INotificationManager notificationManager, IWhatsappNotificationManager whatsappNotificationManager, IWhatsAppMessageRepository whatsAppMessageRepository, CustomDataProtection protector, RDCELERP.DAL.Entities.Digi2l_DevContext context, IEVCManager EVCManager, IAreaLocalityRepository areaLocalityRepository) : base(config)
        {
            _logisticManager = logisticManager;
            _logisticsRepository = logisticsRepository;
            _notificationManager = notificationManager;
            _whatsappNotificationManager = whatsappNotificationManager;
            _WhatsAppMessageRepository = whatsAppMessageRepository;
            _protector = protector;
            _context = context;
            _evcManager = EVCManager;
            _areaLocalityRepository = areaLocalityRepository;
        }
        #endregion

        #region Model Binding
        [BindProperty(SupportsGet = true)]
        public LGCOrderViewModel lGCOrderViewModel { get; set; }
        [BindProperty(SupportsGet = true)]
        public IList<OrderImageUploadViewModel> OrderImageUploadViewModel { get; set; }
        [BindProperty(SupportsGet = true)]
        public IList<ImageLabelViewModel> imageLabelViewModels { get; set; }

        [BindProperty(SupportsGet = true)]
        public OrderLGCViewModel orderLGCViewModel { get; set; }
        #endregion
        public IActionResult OnGet(int OrderTransId)
        {
            #region Variable Declaration
            TblLogistic? tbllogistic = null;
            TblWalletTransaction? tblWalletTransaction = null;
            string url = _baseConfig.Value.BaseURL;
            #endregion

            #region TblOrderTrans
            TblOrderTran? tblOrderTran = _context.TblOrderTrans.Include(x => x.Exchange).ThenInclude(x => x.CustomerDetails)
               .Include(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
               //Changes for ABB Redemption by Vk 
               .Include(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
               .Include(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
               .Include(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.BusinessUnit)
               .Include(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
                //Changes for ABB Redemption by Vk
                .Where(x => x.IsActive == true && x.OrderTransId == OrderTransId).FirstOrDefault();
            #endregion

            #region TblLogistics
            tbllogistic = _context.TblLogistics
               .Where(x => x.IsActive == true && x.OrderTransId == OrderTransId).FirstOrDefault();
            #endregion

            #region tblWalletTransaction
            tblWalletTransaction = _context.TblWalletTransactions
                .Include(x => x.Evcregistration).ThenInclude(x => x.State)
                .Include(x => x.Evcregistration).ThenInclude(x => x.City)
                .Include(x => x.Evcpartner)
                .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
                //Changes for ABB Redemption by Vk 
                .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
                //Changes for ABB Redemption by Vk
                .Where(x => x.IsActive == true && x.OrderTransId == OrderTransId).FirstOrDefault();
            #endregion

            #region Model Mapping Exchange Abb
            if (tblOrderTran != null)
            {
                #region Exchange and ABB common Configuraion Add by VK
                string productTypeDesc = null; string productCatDesc = null; string City = null; string companyName = null;
                string CustomerName = null; string Email = null; string PhoneNumber = null; string Address1 = null;
                string Address2 = null; string State = null; string ZipCode = null;
                TblAreaLocality tblAreaLocality = null;

                if (tblOrderTran.Exchange != null)
                {
                    companyName = tblOrderTran.Exchange.CompanyName;
                    if (tblOrderTran.Exchange.ProductType != null)
                    {
                        productTypeDesc = tblOrderTran.Exchange.ProductType.Description;
                        if (tblOrderTran.Exchange.ProductType.ProductCat != null)
                        {
                            productCatDesc = tblOrderTran.Exchange.ProductType.ProductCat.Description;
                        }
                    }
                    if (tblOrderTran.Exchange.CustomerDetails != null)
                    {
                        City = tblOrderTran.Exchange.CustomerDetails.City;
                        CustomerName = tblOrderTran.Exchange.CustomerDetails.FirstName + " " + tblOrderTran.Exchange.CustomerDetails.LastName;
                        Email = tblOrderTran.Exchange.CustomerDetails.Email;
                        PhoneNumber = tblOrderTran.Exchange.CustomerDetails.PhoneNumber;
                        Address1 = tblOrderTran.Exchange.CustomerDetails.Address1;
                        Address2 = tblOrderTran.Exchange.CustomerDetails.Address2;
                        State = tblOrderTran.Exchange.CustomerDetails.State;
                        ZipCode = tblOrderTran.Exchange.CustomerDetails.ZipCode;

                        if (tblOrderTran.Exchange.CustomerDetails.AreaLocalityId != null)
                        {
                            tblAreaLocality = _areaLocalityRepository.GetArealocalityById(tblOrderTran.Exchange.CustomerDetails.AreaLocalityId);
                            if (tblAreaLocality != null)
                            {
                                if (tbllogistic != null && tbllogistic.OrderTrans != null && tbllogistic.OrderTrans.Exchange != null && tbllogistic.OrderTrans.Exchange.CustomerDetails != null)
                                {
                                    tbllogistic.OrderTrans.Exchange.CustomerDetails.AreaLocality = tblAreaLocality.AreaLocality;
                                }
                            }
                        }
                    }
                }
                else if (tblOrderTran.Abbredemption != null && tblOrderTran.Abbredemption.Abbregistration != null)
                {
                    if (tblOrderTran.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation != null)
                    {
                        productTypeDesc = tblOrderTran.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation.DescriptionForAbb;
                    }
                    if (tblOrderTran.Abbredemption.Abbregistration.NewProductCategory != null)
                    {
                        productCatDesc = tblOrderTran.Abbredemption.Abbregistration.NewProductCategory.DescriptionForAbb;
                    }
                    if (tblOrderTran.Abbredemption.CustomerDetails != null)
                    {
                        City = tblOrderTran.Abbredemption.CustomerDetails.City;
                        CustomerName = tblOrderTran.Abbredemption.CustomerDetails.FirstName + " " + tblOrderTran.Abbredemption.CustomerDetails.LastName;
                        Email = tblOrderTran.Abbredemption.CustomerDetails.Email;
                        PhoneNumber = tblOrderTran.Abbredemption.CustomerDetails.PhoneNumber;
                        Address1 = tblOrderTran.Abbredemption.CustomerDetails.Address1;
                        Address2 = tblOrderTran.Abbredemption.CustomerDetails.Address2;
                        State = tblOrderTran.Abbredemption.CustomerDetails.State;
                        ZipCode = tblOrderTran.Abbredemption.CustomerDetails.ZipCode;
                    }
                    if (tblOrderTran.Abbredemption.Abbregistration.BusinessUnit != null)
                    {
                        companyName = tblOrderTran.Abbredemption.Abbregistration.BusinessUnit.Name;
                    }
                }
                #endregion
                if (tblWalletTransaction != null && tblWalletTransaction.Evcpartner != null)
                {
                    lGCOrderViewModel = _logisticManager.GetLGCDropDetails(tblWalletTransaction.EvcpartnerId);
                }
                if(lGCOrderViewModel == null)
                {
                    lGCOrderViewModel = new LGCOrderViewModel();
                    lGCOrderViewModel.evcPartnerDetailsVM = new EVC_PartnerViewModel();
                }
                else if (lGCOrderViewModel.evcPartnerDetailsVM == null)
                {
                    lGCOrderViewModel.evcPartnerDetailsVM = new EVC_PartnerViewModel();
                }
                lGCOrderViewModel.RegdNo = tblOrderTran?.RegdNo;
                lGCOrderViewModel.TicketNumber = tbllogistic != null && tbllogistic.TicketNumber != null ? tbllogistic.TicketNumber : "Ticket Cancel";
                if (tbllogistic != null)
                {
                    lGCOrderViewModel.PickupScheduleDate = tbllogistic.PickupScheduleDate != null
         ? Convert.ToDateTime(tbllogistic.PickupScheduleDate).ToString("MM/dd/yyyy")
         : Convert.ToDateTime(tbllogistic.CreatedDate).ToString("MM/dd/yyyy");
                }
                else
                {
                    lGCOrderViewModel.PickupScheduleDate = null;
                }

                lGCOrderViewModel.ProductCategory = productCatDesc;
                lGCOrderViewModel.ProductType = productTypeDesc;
                lGCOrderViewModel.SponsorName = companyName;
                lGCOrderViewModel.BrandName = tblOrderTran?.Exchange?.Brand?.Name;

                #region Customer Details
                lGCOrderViewModel.CustomerName = CustomerName;
                lGCOrderViewModel.TblCustomerDetail.Email = Email;
                lGCOrderViewModel.TblCustomerDetail.PhoneNumber = PhoneNumber;
                lGCOrderViewModel.TblCustomerDetail.Address1 = Address1;
                lGCOrderViewModel.TblCustomerDetail.Address2 = Address2;
                lGCOrderViewModel.TblCustomerDetail.City = City;
                lGCOrderViewModel.TblCustomerDetail.State = State;
                lGCOrderViewModel.TblCustomerDetail.ZipCode = ZipCode;
                #endregion
            }
            #endregion

            #region Evc model Mapping
            lGCOrderViewModel.AmountPayableThroughLGC = Convert.ToDecimal(tbllogistic?.AmtPaybleThroughLgc ?? 0);
            #endregion

            #region Image Mapping
            if (OrderTransId > 0)
            {
                /*imageLabelViewModels = _logisticManager.GetImageLabelUploadByProductCat(lGCOrderViewModel.RegdNo);
                */
                OrderImageUploadViewModel = _evcManager.GetAllImagesByTransId(OrderTransId);
                if (OrderImageUploadViewModel != null)
                {
                    foreach (var item in OrderImageUploadViewModel)
                    {

                        if (item.LgcpickDrop == "Pickup")
                        {
                            item.FilePath = "DBFiles/LGC/LGCPickup/";
                            item.ImageWithPath = url + item.FilePath + item.ImageName;
                        }
                        else if (item.LgcpickDrop == "Drop")
                        {
                            item.FilePath = "DBFiles/LGC/LGCDrop/";
                            item.ImageWithPath = url + item.FilePath + item.ImageName;
                        }
                        else if (item.LgcpickDrop == "EVCDispute")
                        {
                            item.FilePath = "DBFiles/EVC/EVCUser_Dispute/";
                            item.ImageWithPath = url + item.FilePath + item.ImageName;
                        }
                        else
                        {
                            item.FilePath = "DBFiles/QC/VideoQC/";
                            item.ImageWithPath = url + item.FilePath + item.ImageName;
                        }

                    }
                }
            }
            #endregion

            return Page();
        }
    }
}
