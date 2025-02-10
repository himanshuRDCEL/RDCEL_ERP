using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.IO;
using RDCELERP.BAL.Interface;
using RDCELERP.Model.ImageLabel;
using RDCELERP.Model.OrderImageUpload;
using RDCELERP.Model.QC;
using static Org.BouncyCastle.Math.EC.ECCurve;
using RDCELERP.Model.Base;
using MediaToolkit;
using MediaToolkit.Model;
using MediaToolkit.Options;
using RDCELERP.DAL.IRepository;
using RDCELERP.DAL.Entities;
using RDCELERP.Common.Enums;

namespace RDCELERP.Core.App.Pages.QCPortal
{
    public class SelfQCModel : PageModel
    {
        #region variable Declartion
        private readonly IQCManager _QCManager;
        IOptions<ApplicationSettings> _config;
        ITemplateConfigurationRepository _templateConfigurationRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        #endregion

        #region Constructor
        public SelfQCModel(IQCManager qCManager, IOptions<ApplicationSettings> config, ITemplateConfigurationRepository templateConfigurationRepository, IWebHostEnvironment webHostEnvironment)
        {
            _QCManager = qCManager;
            _config = config;
            _templateConfigurationRepository = templateConfigurationRepository;
            _webHostEnvironment = webHostEnvironment;
        }
        #endregion

        #region BindProperty
        [BindProperty(SupportsGet = true)]
        public SelfQcVideoImageViewModel selfQcVideoImageViewModel { get; set; }

        [BindProperty(SupportsGet = true)]
        public string isVideoTrue { get; set; }

        [BindProperty(SupportsGet = true)]
        public SelfQCExchangeDetailsViewModel selfQCExchangeDetailsView { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? URLPrefixforProd { get; set; }
        public TblConfiguration? tblConfiguration { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? VideotimerSec { get; set; }
        #endregion
        public IActionResult OnGet(string regdno)
        {
            bool flag = false;
            URLPrefixforProd = _config?.Value?.URLPrefixforProd;

            #region Get Configuration for Set Video timer
            tblConfiguration = _templateConfigurationRepository.GetConfigByKeyName(ConfigurationEnum.VideoRecordingTimerSec.ToString());
            if (tblConfiguration != null)
            {
                VideotimerSec = tblConfiguration?.Value;
            }
            else
            {
                VideotimerSec = "30";
            }
            #endregion
            if (!string.IsNullOrEmpty(regdno))
            {
                flag = _QCManager.verifyDuplicateSelfQC(regdno);
                if (flag == true)
                {
                    return RedirectToPage("./Details");
                }
                else
                {
                    selfQcVideoImageViewModel.imageLabelViewModels = _QCManager.GetQCImageLabels(regdno);
                    selfQcVideoImageViewModel.LoginId = 3;
                    selfQCExchangeDetailsView = _QCManager.getOrderDetailsbyRegdno(regdno);
                }
            }
            else
            {
                return Page();
            }

            return Page();
        }

        public IActionResult OnPostAsync()
        {
            bool flag = false;
            URLPrefixforProd = _config?.Value?.URLPrefixforProd;
            //if (ModelState.IsValid)
            //{
            if (selfQcVideoImageViewModel.imageLabelViewModels != null && selfQcVideoImageViewModel.imageLabelViewModels.Count > 0)
            {
                flag = _QCManager.UpdateSelfQCImageToDB(selfQcVideoImageViewModel);
                if (flag == true)
                {
                    return RedirectToPage("./Details");
                }
                else
                {
                    string? regdNo = regdNo = selfQcVideoImageViewModel?.imageLabelViewModels?.FirstOrDefault()?.RegdNo;
                    return OnGet(regdNo ?? "");
                }
            }
            //}
            return Page();
        }

        #region Compress Video
        public async Task<JsonResult> OnPostCompressVideo(string? regdNo, string? base64String, bool isMediaTypeVideo, int srNum, int? orderTransId, int? statusId, int? imageLabelId)
        {
            if (string.IsNullOrEmpty(base64String))
            {
                return new JsonResult("Base64 string is null or empty.");
            }

            try
            {
                string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string tempInputFilePath = Path.Combine(documentsPath, "input_video.mp4");
                string outputFilePath = Path.Combine(documentsPath, "compressed_video.mp4");

                byte[] videobyte = Convert.FromBase64String(base64String);

                await using (var fileStream = new FileStream(tempInputFilePath, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true))
                {
                    await fileStream.WriteAsync(videobyte, 0, videobyte.Length);
                }

                using (var engine = new Engine())
                {
                    var inputFile = new MediaFile { Filename = tempInputFilePath };
                    var outputFile = new MediaFile { Filename = outputFilePath };

                    engine.GetMetadata(inputFile);

                    var options = new ConversionOptions
                    {
                        VideoAspectRatio = VideoAspectRatio.R3_2,
                        VideoSize = VideoSize.Hd480,
                        AudioSampleRate = AudioSampleRate.Hz22050,
                        VideoBitRate = 620,
                        VideoFps = 54,
                    };

                    engine.Convert(inputFile, outputFile, options);
                }

                byte[] compressedfile = await System.IO.File.ReadAllBytesAsync(outputFilePath);

                // Delete files asynchronously
                await Task.WhenAll(
                    DeleteFileAsync(tempInputFilePath),
                    DeleteFileAsync(outputFilePath)
                );

                string bytedata = Convert.ToBase64String(compressedfile);

                if (!string.IsNullOrEmpty(regdNo) && !string.IsNullOrEmpty(bytedata) && srNum > 0)
                {
                    bool flag = _QCManager.SaveMediaFile(regdNo, bytedata, isMediaTypeVideo, srNum, orderTransId, statusId, imageLabelId);
                    if (!flag)
                    {
                        return new JsonResult("Failed to save compressed file");
                    }
                }

                return new JsonResult(bytedata);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new JsonResult("An error occurred during processing.");
            }
        }
        #endregion

        #region Save Uploaded Media Files
        public JsonResult OnPostSaveMediaFile(string? regdNo, string? base64String, bool isMediaTypeVideo, int srNum, int? orderTransId, int? statusId, int? imageLabelId)
        {
            bool flag = false;
            try
            {
                if (regdNo != null && base64String != null && srNum > 0)
                {
                    flag = _QCManager.SaveMediaFile(regdNo, base64String, isMediaTypeVideo, srNum, orderTransId, statusId, imageLabelId);
                }
            }
            catch (Exception ex)
            {
            }
            return new JsonResult(flag);
        }
        #endregion

        #region Delete Media Files
        public JsonResult OnPostDeleteMediaFile(string? regdNo, bool isMediaTypeVideo, int srNum)
        {
            bool flag = false;
            var fileName = "";
            try
            {
                if (regdNo != null && srNum > 0)
                {
                    flag = _QCManager.DeleteMediaFile(regdNo, isMediaTypeVideo, srNum);
                }
            }
            catch (Exception ex)
            {
            }
            return new JsonResult(flag);
        }
        private async Task DeleteFileAsync(string filePath)
        {
            try
            {
                if (System.IO.File.Exists(filePath))
                {
                    await Task.Run(() => System.IO.File.Delete(filePath));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting file: {ex.Message}");
            }
        }
        #endregion
    }
}
