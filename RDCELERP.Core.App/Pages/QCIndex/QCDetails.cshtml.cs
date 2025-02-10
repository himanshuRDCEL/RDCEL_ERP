using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Enums;
using RDCELERP.Common.Helper;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.Model.Base;
using RDCELERP.Model.ExchangeOrder;
using RDCELERP.Model.ImagLabel;
using RDCELERP.Model.QC;

namespace RDCELERP.Core.App.Pages.QCIndex
{
    public class QCDetailsModel : BasePageModel
    {
        #region Variable Declaration
        private readonly IQCCommentManager _QcCommentManager;
        private readonly ILogisticManager _logisticManager;
        private CustomDataProtection _protector;
        IQCManager _qcManager;
        ITempDataRepository _tempDataRepository;
        #endregion

        #region Constructor
        public QCDetailsModel(IQCCommentManager qcCommentManager, IOptions<ApplicationSettings> config, CustomDataProtection protector, Digi2l_DevContext context, ILogisticManager logisticManager, IQCManager qcManager, ITempDataRepository tempDataRepository) : base(config)
        {
            _QcCommentManager = qcCommentManager;
            _protector = protector;
            _logisticManager = logisticManager;
            _qcManager = qcManager;
            _tempDataRepository = tempDataRepository;
        }
        #endregion

        #region Model Binding
        [BindProperty(SupportsGet = true)]
        public ExchangeOrderViewModel exchangeOrderViewModel { get; set; }

        [BindProperty(SupportsGet = true)]
        public IList<SelfQCViewModel> selfQCList { get; set; }

        [BindProperty(SupportsGet = true)]
        public IList<ImageLabelViewModel> imageLabelViewModels { get; set; }
        public bool? IsSelfQCDone { get; set; }
        #endregion
        public IActionResult OnGet(int id)
        {
            string url = _baseConfig.Value.BaseURL;
            if (id > 0)
            {
                exchangeOrderViewModel = _QcCommentManager.GetOrderDetailsById(id);
                #region Code for Get Self QC Images
                if (exchangeOrderViewModel != null && exchangeOrderViewModel.RegdNo != null)
                {

                    selfQCList = _QcCommentManager.GetImagesUploadedBySelfQC(exchangeOrderViewModel.RegdNo);
                    imageLabelViewModels = _logisticManager.GetImageLabelUploadByProductCat(exchangeOrderViewModel.RegdNo);
                    if (imageLabelViewModels == null)
                    {
                        imageLabelViewModels = new List<ImageLabelViewModel>();
                    }
                }

                #region Display Self QC Images
                IsSelfQCDone = _qcManager.verifyDuplicateSelfQC(exchangeOrderViewModel?.RegdNo);
                if (IsSelfQCDone == true)
                {
                    if (selfQCList != null && selfQCList.Count > 0)
                    {
                        foreach (var item in selfQCList)
                        {
                            if (item != null)
                            {
                                item.FilePath = EnumHelper.DescriptionAttr(FileAddressEnum.SelfQC);
                                item.ImageWithPath = url + item.FilePath + item.ImageName;
                                var extn = item?.ImageName?.Split(".").Last();
                                if (extn == "webm")
                                {
                                    item.IsMediaTypeVideo = true;
                                }
                            }
                        }
                    }
                }
                else
                {
                    selfQCList = new List<SelfQCViewModel>();
                    SelfQCViewModel? selfQCViewModel = null;
                    ArrayList list = new ArrayList();
                    list = GetMediaFiles(exchangeOrderViewModel?.RegdNo);
                    if (list != null && list.Count > 0)
                    {
                        foreach (var item in list)
                        {
                            selfQCViewModel = new SelfQCViewModel();
                            selfQCViewModel.ImageWithPath = item?.ToString();
                            var extn = selfQCViewModel.ImageWithPath?.Split(".").Last();
                            if (extn == "webm")
                            {
                                selfQCViewModel.IsMediaTypeVideo = true;
                            }
                            selfQCList.Add(selfQCViewModel);
                        }
                    }
                }
                #endregion

                #endregion

                #region statusId List for QC
                var statusflag = _QcCommentManager.GetQcFlag();
                if (statusflag != null)
                {
                    ViewData["statusflag"] = new SelectList(statusflag, "Id", "StatusCode");
                }
                #endregion

                if (exchangeOrderViewModel != null)
                {
                    return Page();
                }
                else
                {
                    return RedirectToPage("/index");
                }
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            string result = string.Empty;
            bool flag = false;
            if (exchangeOrderViewModel != null && exchangeOrderViewModel.QCCommentViewModel != null && (exchangeOrderViewModel.QCCommentViewModel.StatusId == Convert.ToInt32(OrderStatusEnum.Waitingforcustapproval) || exchangeOrderViewModel.QCCommentViewModel.StatusId == Convert.ToInt32(OrderStatusEnum.QCByPass)))
            {
                if (imageLabelViewModels != null && imageLabelViewModels.Count > 0)
                {
                    result = _QcCommentManager.FinalQCImagesUploadedByQC(imageLabelViewModels, exchangeOrderViewModel, Convert.ToInt32(_loginSession.UserViewModel.UserId));
                    if (!string.IsNullOrEmpty(result) && exchangeOrderViewModel.IsDiagnosev2 == null)
                    {
                        return RedirectToPage("/QCIndex/Questioners", new { regdNo = result });
						
					}
                    else if (!string.IsNullOrEmpty(result) && exchangeOrderViewModel.IsDiagnosev2==true)
                    {
						return RedirectToPage("/QCIndex/Questionersv2", new { regdNo = result });
					}
                    else
                    {
                        return RedirectToPage("/QCIndex/OrdersForQC");
                    }
                }
            }
            else
            {
                flag = _QcCommentManager.SaveStatuIdForQcDetails(exchangeOrderViewModel, Convert.ToInt32(_loginSession.UserViewModel.UserId));
                if (flag)
                {
                    return RedirectToPage("/QCIndex/OrdersForQC");
                }
                else
                {
                    return RedirectToPage("/QCIndex/OrdersForQC");
                }
            }
            return RedirectToPage("/OrdersForQC"); ;
        }

        #region SaveSelfQCImageAsFinalQCImage 
        public JsonResult OnPostSaveSelfQCImageAsFinalQCImage(string RegdNo)
        {
            return new JsonResult(_QcCommentManager.saveSelfQCImageAsFinalImage(RegdNo, Convert.ToInt32(_loginSession.UserViewModel.UserId)));
        }
        #endregion

        #region Get Media Files
        public JsonResult OnPostGetMediaFiles(string? regdNo)
        {
            ArrayList list = new ArrayList();
            JsonResult jsonResult = null;
            try
            {
                if (!string.IsNullOrEmpty(regdNo))
                {
                    list = GetMediaFiles(regdNo);
                }
                jsonResult = new JsonResult(list);
            }
            catch (Exception ex)
            {
            }
            return jsonResult;
        }

        public ArrayList GetMediaFiles(string? regdNo)
        {
            var fileName = "";
            var count = 1;
            List<ImageLabelViewModel>? imageLabelViewModels = null;
            List<TblTempDatum>? tempData = null;
            var placeholderPath = "";
            string fullFilePath = "";
            ArrayList list = new ArrayList();
            try
            {

                string? baseUrl = _baseConfig.Value.BaseURL;
                string documentsPath = string.Concat(baseUrl, "DBFiles/QC/SelfQC/");
                tempData = _tempDataRepository.GetMediaFilesTempDataList(regdNo);
                imageLabelViewModels = _qcManager.GetQCImageLabels(regdNo);
                if (imageLabelViewModels != null && imageLabelViewModels.Count > 0)
                {
                    foreach (var imageLabel in imageLabelViewModels)
                    {
                        if (imageLabel != null)
                        {
                            if (imageLabel.IsMediaTypeVideo == true)
                            {
                                fileName = regdNo + "_Video_" + count + ".webm";
                            }
                            else
                            {
                                fileName = regdNo + "_Image_" + count + ".jpg";
                            }
                            var tempDataObj = tempData?.FirstOrDefault(x => x.FileName == fileName);
                            if (tempDataObj != null && !string.IsNullOrEmpty(tempDataObj.FileName))
                            {
                                fullFilePath = string.Concat(documentsPath, tempDataObj.FileName);
                            }
                            else
                            {
                                if (imageLabel.IsMediaTypeVideo == true)
                                {
                                    placeholderPath = "Images/VIDEO-placeholder.png";
                                }
                                else
                                {
                                    placeholderPath = "Images/placeholder-01.png";
                                }
                                fullFilePath = string.Concat(baseUrl, placeholderPath);
                            }
                            list.Add(fullFilePath);
                        }
                        count++;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return list;
        }
        #endregion
    }
}
