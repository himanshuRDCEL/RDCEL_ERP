using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Enums;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.Model.Base;
using RDCELERP.Model.ImagLabel;
using RDCELERP.Model.LGC;

namespace RDCELERP.Core.App.Pages.EVC_Admin_Dispute
{
    public class DisputeViewPageModel : BasePageModel
    {
        #region Variable declartion
        private readonly ILogisticManager _logisticManager;
        private readonly IEVCManager _evcManager;
        private readonly ILogisticsRepository _logisticsRepository;
        private readonly INotificationManager _notificationManager;
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        #endregion

        #region Constructor
        public DisputeViewPageModel(IOptions<ApplicationSettings> config, ILogisticManager logisticManager, ILogisticsRepository logisticsRepository, INotificationManager notificationManager, RDCELERP.DAL.Entities.Digi2l_DevContext context, IEVCManager evcManager) : base(config)
        {
            _logisticManager = logisticManager;
            _logisticsRepository = logisticsRepository;
            _notificationManager = notificationManager;
            _context = context;
            _evcManager = evcManager;
        }
        #endregion

        #region Model Binding

        [BindProperty(SupportsGet = true)]
        public LGCOrderViewModel lGCOrderViewModel { get; set; }

        [BindProperty(SupportsGet = true)]
        public PODViewModel PODVM { get; set; }
        public TblEvcpoddetail tblEvcPodDetails { get; set; }
        [BindProperty(SupportsGet = true)]
        public IList<OrderImageUploadViewModel> OrderImageUploadVMList { get; set; }

        [BindProperty(SupportsGet = true)]
        public IList<ImageLabelViewModel> imageLabelVMList { get; set; }
        [BindProperty(SupportsGet = true)]
        public IList<OrderImageUploadViewModel> OrderImageUploadViewModel { get; set; }
        [BindProperty(SupportsGet = true)]
        public List<ImageLabelViewModel> imageLabelViewModels { get; set; }
        #endregion


        public async Task<IActionResult> OnGetAsync(int OrderTransId)
        {
            #region Variable Declaration
            TblLogistic tbllogistic = null;
            TblOrderLgc tblOrderLgc = null;
            TblWalletTransaction tblWalletTransaction = null;
            string url = _baseConfig.Value.BaseURL;
            lGCOrderViewModel = new LGCOrderViewModel();
            PODVM.evcDetailsVM = new EVCDetailsViewModel();
            lGCOrderViewModel.podVM = new PODViewModel();
            #region Common Implementations for (ABB or Exchange)
            TblExchangeOrder tblExchangeOrder = null;
            TblAbbredemption tblAbbredemption = null;
            TblAbbregistration tblAbbregistration = null;
            string regdNo = null; string? productTypeDesc = null; string? productCatDesc = null;
            TblOrderTran tblOrderTrans = null;
            #endregion
            #endregion

            tbllogistic = _context.TblLogistics
                .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
                .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                .Where(x => x.IsActive == true && x.OrderTransId == OrderTransId).FirstOrDefault();
            if (tbllogistic != null)
            {
                #region Common Implementations for (ABB or Exchange)
                if (tbllogistic.OrderTrans != null)
                {
                    regdNo = tbllogistic.OrderTrans.RegdNo;
                    if (tbllogistic.OrderTrans.OrderType == Convert.ToInt32(LoVEnum.Exchange))
                    {
                        tblExchangeOrder = tbllogistic.OrderTrans.Exchange;
                        if (tblExchangeOrder != null)
                        {
                            productTypeDesc = tblExchangeOrder.ProductType?.Description;
                            productCatDesc = tblExchangeOrder.ProductType?.ProductCat?.Description;
                        }
                    }
                    else if (tbllogistic.OrderTrans.OrderType == Convert.ToInt32(LoVEnum.ABB))
                    {
                        tblAbbredemption = tbllogistic.OrderTrans.Abbredemption;
                        if (tblAbbredemption != null)
                        {
                            tblAbbregistration = tblAbbredemption.Abbregistration;
                            if (tblAbbregistration != null)
                            {
                                productTypeDesc = tblAbbregistration.NewProductCategoryTypeNavigation.DescriptionForAbb;
                                productCatDesc = tblAbbregistration.NewProductCategory.DescriptionForAbb;
                            }
                        }
                    }
                }
                #endregion

                lGCOrderViewModel.RegdNo = regdNo;
                lGCOrderViewModel.ProductCategory = productCatDesc;
                lGCOrderViewModel.ProductType = productTypeDesc;
                lGCOrderViewModel.TicketNumber = tbllogistic.TicketNumber;

                tblEvcPodDetails = _context.TblEvcpoddetails.Where(x => x.IsActive == true && x.RegdNo == lGCOrderViewModel.RegdNo).FirstOrDefault();
                if (tblEvcPodDetails != null)
                {
                    string podfilePath = "DBFiles/EVC/POD/";
                    string podPdfWithPath = url + podfilePath + tblEvcPodDetails.Podurl;

                    string debitNotefilePath = "DBFiles/EVC/DebitNote/";
                    string debitNotePdfWithPath = url + debitNotefilePath + tblEvcPodDetails.DebitNotePdfName;

                    string invoicefilePath = "DBFiles/EVC/Invoice/";
                    string invoicePdfWithPath = url + invoicefilePath + tblEvcPodDetails.InvoicePdfName;

                    lGCOrderViewModel.podVM.FullPoDUrl = podPdfWithPath;
                    lGCOrderViewModel.podVM.FullDebitNoteUrl = debitNotePdfWithPath;
                    lGCOrderViewModel.podVM.FullInvoiceUrl = invoicePdfWithPath;

                    tblOrderLgc = _context.TblOrderLgcs.Where(x => x.IsActive == true && x.OrderTransId == OrderTransId).FirstOrDefault();
                    if (tblOrderLgc != null)
                    {
                        string custDeclfilePath = "DBFiles/EVC/CustomerDeclaration/";
                        string custDeclPdfWithPath = url + custDeclfilePath + tblOrderLgc.CustDeclartionpdfname;
                        lGCOrderViewModel.podVM.FullCustDeclUrl = custDeclPdfWithPath;
                    }
                }
            }

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
            return Page();
        }
    }
}



