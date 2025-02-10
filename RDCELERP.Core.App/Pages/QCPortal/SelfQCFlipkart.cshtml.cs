using MediaToolkit.Model;
using MediaToolkit.Options;
using MediaToolkit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.IO;
using RDCELERP.BAL.Interface;
using RDCELERP.BAL.MasterManager;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.Model.Base;
using RDCELERP.Model.ImageLabel;
using RDCELERP.Model.OrderImageUpload;
using RDCELERP.Model.QC;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace RDCELERP.Core.App.Pages.QCPortal
{
    public class SelfQCFlipkartModel : BasePageModel
    {
        #region variable Declartion
        private readonly ILogger<IndexModel> _logger;
        IQCManager _QCManager;
        RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        IOptions<ApplicationSettings> _config;
        #endregion

        #region Constructor
        public SelfQCFlipkartModel(ILogger<IndexModel> logger, IUserManager userManager, RDCELERP.DAL.Entities.Digi2l_DevContext context, IOptions<ApplicationSettings> config, IQCManager qCManager)
        : base(config)

        {
            _logger = logger;
            _context = context;
            _QCManager = qCManager;
            _config = config;
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
        public string baseurl { get; set; }
        #endregion
        public IActionResult OnGet(string regdno)
        {
            bool flag = false;
            if (_loginSession == null)
            {
                return RedirectToPage("/index");
            }
            else
            {
                if (_loginSession.UserViewModel.UserId > 0)
                {
                    selfQcVideoImageViewModel.LoginId = _loginSession.UserViewModel.UserId;
                }

            }
            if (!string.IsNullOrEmpty(regdno))
            {
                flag = _QCManager.verifyDuplicateSelfQC(regdno);
                if (flag == true)
                {
                    return RedirectToPage("./Details");
                }
                else
                {
                    baseurl = _config.Value.BaseURL.Trim('/');
                    selfQcVideoImageViewModel.imageLabelViewModels = _QCManager.SelfQCFlipkart(regdno);

                    selfQCExchangeDetailsView = _QCManager.getOrderDetailsbyRegdno(regdno);
                    isVideoTrue = string.Empty;
                    foreach (var item in selfQcVideoImageViewModel.imageLabelViewModels)
                    {
                        string productLabel = item.ProductImageLabel.ToLower();
                        if (productLabel.Contains("video"))
                        {
                            isVideoTrue = item.ProductImageLabel;
                        }
                    }
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
            if (selfQcVideoImageViewModel.imageLabelViewModels != null && selfQcVideoImageViewModel.imageLabelViewModels.Count > 0)
            {
                flag = _QCManager.AddSelfQCImageToDB(selfQcVideoImageViewModel);
                if (flag == true)
                {
                    return RedirectToPage("./Details");
                }
                else
                {
                    return Page();
                }
            }
            return Page();
        }

        [ValidateAntiForgeryToken]
        public JsonResult OnPostCompressVideo(string fileName)
        {
            try
            {
                string bytedata = null;
                if (fileName != null)
                {
                    baseurl = _config.Value.BaseURL;

                    byte[] videobyte = Convert.FromBase64String(fileName);

                    string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    string tempInputFilePath = System.IO.Path.Combine(documentsPath, "input_video.mp4");
                    System.IO.File.WriteAllBytes(tempInputFilePath, videobyte);
                    string outputFilePath = System.IO.Path.Combine(documentsPath, "compressed_video.mp4");
                    var inputFile = new MediaFile { Filename = tempInputFilePath };
                    var outputFile = new MediaFile { Filename = outputFilePath };
                    //using (var engine = new Engine())
                    using (var engine = new Engine())
                    {
                        engine.GetMetadata(inputFile);
                        var options = new ConversionOptions
                        {
                            VideoAspectRatio = VideoAspectRatio.R3_2,
                            VideoSize = VideoSize.Hd480,
                            //AudioSampleRate = AudioSampleRate.Hz22050,
                            VideoBitRate = 620,
                            VideoFps = 54,
                        };
                        engine.Convert(inputFile, outputFile, options);
                    }

                    Task<byte[]> compressedfile = System.IO.File.ReadAllBytesAsync(outputFilePath);

                    System.IO.File.Delete(tempInputFilePath);
                    System.IO.File.Delete(outputFilePath);

                    bytedata = Convert.ToBase64String(compressedfile.Result);
                    return new JsonResult(bytedata);
                }
                else
                {
                    return new JsonResult(fileName);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
