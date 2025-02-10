using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
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

namespace RDCELERP.Core.App.Pages.EVC_Portal 
{
    public class OrderdetailsViewPageForEVC_PortALModel : BasePageModel
    {

        #region Variable declartion
        private readonly ILogisticManager _logisticManager;
        private readonly IEVCManager _evcManager;
        private readonly ILogisticsRepository _logisticsRepository;
        private readonly INotificationManager _notificationManager;
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        #endregion

        #region Constructor
        public OrderdetailsViewPageForEVC_PortALModel(IOptions<ApplicationSettings> config, ILogisticManager logisticManager, ILogisticsRepository logisticsRepository, INotificationManager notificationManager, RDCELERP.DAL.Entities.Digi2l_DevContext context, IEVCManager evcManager) : base(config)
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
            TblLogistic tbllogistic = null;
            TblOrderLgc tblOrderLgc = null;
            TblWalletTransaction tblWalletTransaction = null;
            string url = _baseConfig.Value.BaseURL;
            lGCOrderViewModel = new LGCOrderViewModel();
            PODVM.evcDetailsVM = new EVCDetailsViewModel();
            lGCOrderViewModel.podVM = new PODViewModel();
            TblOrderTran tblOrderTran = _context.TblOrderTrans.Where(x => x.IsActive == true && x.OrderTransId == OrderTransId).FirstOrDefault();
            if (tblOrderTran != null)
            {
                if (tblOrderTran.OrderType == Convert.ToInt32(OrderTypeEnum.ABB))  ///Abb Orders
                {
                    tblWalletTransaction = _context.TblWalletTransactions.Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption)
                .ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory).Where(x => x.IsActive == true && x.OrderTransId == OrderTransId).FirstOrDefault();

                    if (tblWalletTransaction != null)
                    {
                        lGCOrderViewModel.RegdNo = tblWalletTransaction.RegdNo;
                        lGCOrderViewModel.ProductCategory = tblWalletTransaction.OrderTrans.Abbredemption.Abbregistration.NewProductCategory.Description;
                        if (tblWalletTransaction.OrderTrans.Abbredemption.Abbregistration != null)
                        {
                            int proTypeId =Convert.ToInt32(tblWalletTransaction.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeId);

                            TblProductType tblProductType = _context.TblProductTypes.Where(x => x.IsActive == true && x.Id == proTypeId).FirstOrDefault();
                            lGCOrderViewModel.ProductType = tblProductType!=null?tblProductType.Description:null;

                        }
                        lGCOrderViewModel.EvcAmount = (decimal)tblWalletTransaction.OrderAmount;
                        TblOrderQc details = _context.TblOrderQcs.Where(x => x.OrderTransId == OrderTransId).FirstOrDefault();
                        if (details != null)
                        {
                            lGCOrderViewModel.AfterQCProductQty = details.QualityAfterQc;
                        }
                    }


                    tbllogistic = _context.TblLogistics.Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption)
                        .ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation).ThenInclude(x => x.ProductCat).Where(x => x.IsActive == true && x.OrderTransId == OrderTransId).FirstOrDefault();
                    if (tbllogistic != null)
                    {
                        //lGCOrderViewModel.RegdNo = tbllogistic.OrderTrans.Exchange.RegdNo;
                        //lGCOrderViewModel.ProductCategory = tbllogistic.OrderTrans.Exchange.ProductType.ProductCat.Description;
                        //lGCOrderViewModel.ProductType = tbllogistic.OrderTrans.Exchange.ProductType.DescriptionForAbb;
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
                                /*foreach (var labelitem in imageLabelViewModels)
                                {
                                    item.FilePath = "DBFiles/QC/VideoQC/";
                                    item.FilePath = "DBFiles/LGC/LGCPickup/";
                                    item.ImageWithPath = url + item.FilePath + item.ImageName;
                                }*/
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
                }
                else if(tblOrderTran.OrderType == Convert.ToInt32(OrderTypeEnum.Exchange))  /////Exchange Orders
                {
                    tblWalletTransaction = _context.TblWalletTransactions.Include(x => x.OrderTrans).ThenInclude(x => x.Exchange)
              .ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat).Where(x => x.IsActive == true && x.OrderTransId == OrderTransId).FirstOrDefault();

                    if (tblWalletTransaction != null)
                    {
                        lGCOrderViewModel.RegdNo = tblWalletTransaction.OrderTrans.Exchange.RegdNo;
                        lGCOrderViewModel.ProductCategory = tblWalletTransaction.OrderTrans.Exchange.ProductType.ProductCat.Description;
                        lGCOrderViewModel.ProductType = tblWalletTransaction.OrderTrans.Exchange.ProductType.DescriptionForAbb;
                        lGCOrderViewModel.EvcAmount = (decimal)tblWalletTransaction.OrderAmount;
                        TblOrderQc details = _context.TblOrderQcs.Where(x => x.OrderTransId == OrderTransId).FirstOrDefault();
                        if (details != null)
                        {
                            lGCOrderViewModel.AfterQCProductQty = details.QualityAfterQc;
                        }
                    }


                    tbllogistic = _context.TblLogistics.Include(x => x.OrderTrans).ThenInclude(x => x.Exchange)
                        .ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat).Where(x => x.IsActive == true && x.OrderTransId == OrderTransId).FirstOrDefault();
                    if (tbllogistic != null)
                    {
                        //lGCOrderViewModel.RegdNo = tbllogistic.OrderTrans.Exchange.RegdNo;
                        //lGCOrderViewModel.ProductCategory = tbllogistic.OrderTrans.Exchange.ProductType.ProductCat.Description;
                        //lGCOrderViewModel.ProductType = tbllogistic.OrderTrans.Exchange.ProductType.DescriptionForAbb;
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
                                /*foreach (var labelitem in imageLabelViewModels)
                                {
                                    item.FilePath = "DBFiles/QC/VideoQC/";
                                    item.FilePath = "DBFiles/LGC/LGCPickup/";
                                    item.ImageWithPath = url + item.FilePath + item.ImageName;
                                }*/
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
                }

                
            }

            
            return Page();
        }
    }
}
