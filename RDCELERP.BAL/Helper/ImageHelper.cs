using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Common.Helper;
using RDCELERP.Model.Base;

namespace RDCELERP.BAL.Helper
{
   public class ImageHelper : IImageHelper
    {
        #region Variable Declaration
        private readonly IWebHostEnvironment _webHostEnvironment;
        ILogging _logging;
        public readonly IOptions<ApplicationSettings> _baseConfig;
        #endregion

        #region Constructor
        public ImageHelper(IWebHostEnvironment webHostEnvironment, ILogging logging, IOptions<ApplicationSettings> baseConfig)
        {
            _webHostEnvironment = webHostEnvironment;
            _logging = logging;
            _baseConfig = baseConfig;
        }
        #endregion

        #region Save Image file from the File Object
        public bool SaveFile(IFormFile requestFile, string requestPath, string customFileName)
        {
            bool flag = false;
            string fileName = null;
            try
            {
                if (customFileName != null && customFileName != "")
                {
                    fileName = customFileName;
                }
                else
                {
                    fileName = Guid.NewGuid().ToString("N");
                }
                if (requestFile != null && requestPath != null)
                {
                    var filePath = string.Concat(_webHostEnvironment.WebRootPath, "\\", requestPath);
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath); //Create directory if it doesn't exist
                    }
                    var fileNameWithPath = string.Concat(filePath, "\\", fileName);
                    using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                    {
                        requestFile.CopyTo(stream);
                        flag = true;
                    }
                    flag = true;
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ImageHelper", "SaveFileFromBase64", ex);
            }
            return flag;
        }
        #endregion

        #region Save Image file from the Base 64 String Object
        public bool SaveFileFromBase64(string imageBase64, string requestPath, string customFileName)
        {
            bool flag = false;
            string fileName = null;
            try
            {
                if (customFileName != null && customFileName != "")
                {
                    fileName = customFileName;
                }
                else
                {
                    fileName = Guid.NewGuid().ToString("N");
                }
                if (imageBase64 != null && requestPath != null)
                {
                    var filePath = string.Concat(_webHostEnvironment.WebRootPath, "\\", requestPath);
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath); //Create directory if it doesn't exist
                    }
                    var fileNameWithPath = string.Concat(filePath, "\\", fileName);
                    byte[] imageBytes = Convert.FromBase64String(imageBase64);
                    File.WriteAllBytes(fileNameWithPath, imageBytes);
                    flag = true;
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ImageHelper", "SaveFileFromBase64", ex);
            }

            return flag;
        }
        #endregion

        public bool SaveFileDefRoot(IFormFile requestFile, string requestPath, string customFileName)
        {
            bool flag = false;
            string fileName = null;
            try
            {
                if (customFileName != null && customFileName != "")
                {
                    fileName = customFileName;
                }
                else
                {
                    fileName = Guid.NewGuid().ToString("N");
                }
                if (requestFile != null && requestPath != null)
                {

                    var filePath = requestPath;
                    string exactPath = Path.GetFullPath(filePath);
                    var directoryPath = Path.GetDirectoryName(filePath);

                    if (!Directory.Exists(exactPath))
                    {
                        Directory.CreateDirectory(exactPath); //Create directory if it doesn't exist
                    }
                    var fileNameWithPath = string.Concat(filePath, "\\", fileName);
                    using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                    {
                        requestFile.CopyTo(stream);
                        flag = true;
                    }
                    flag = true;
                }
                else
                {
                    flag = false;
                    // Handle the case when requestFile or requestPath is null
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ImageHelper", "SaveFileFromBase64", ex);
            }
            return flag;
        }
        
        #region Get Image Path by Base Path and ImageName
        public string GetImageSrc(string basePath, string imageName)
        {
            string? imagePath = null;
            try
            {
                string? baseUrl = _baseConfig.Value.BaseURL + _baseConfig.Value.URLPrefixforProd;
                imagePath = string.IsNullOrEmpty(imageName) ? string.Empty : $"{baseUrl}{basePath}{imageName}";
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ImageHelper", "SaveFileFromBase64", ex);
            }
            return imagePath;
        }
        #endregion

        #region Save Video file from the Base 64 String Object
        //public bool SaveVideoFileFromBase64(string videoBase64, string requestPath, string customFileName)
        //{
        //    bool flag = false;
        //    string fileName = null;
        //    try
        //    {
        //        if (customFileName != null && customFileName != "")
        //        {
        //            fileName = customFileName;
        //        }
        //        else
        //        {
        //            fileName = Guid.NewGuid().ToString("N");
        //        }
        //        if (videoBase64 != null && requestPath != null)
        //        {
        //            var filePath = string.Concat(_webHostEnvironment.WebRootPath, "\\", requestPath);
        //            if (!Directory.Exists(filePath))
        //            {
        //                Directory.CreateDirectory(filePath); //Create directory if it doesn't exist
        //            }
        //            var fileNameWithPath = string.Concat(filePath, "\\", fileName);
        //            byte[] videoBytes = Convert.FromBase64String(videoBase64);
        //            File.WriteAllBytes(fileNameWithPath, videoBytes);
        //            flag = true;
        //        }
        //        else
        //        {

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logging.WriteErrorToDB("ImageHelper", "SaveFileFromBase64", ex);
        //    }

        //    return flag;
        //}
        #endregion

        #region Save Video file from the Base 64 String Object
        public bool SaveVideoFileFromBase64(string videoBase64, string requestPath, string customFileName)
        {
            bool flag = false;
            string fileName = null;
            try
            {
                if (customFileName != null && customFileName != "")
                {
                    fileName = customFileName;
                }
                else
                {
                    fileName = Guid.NewGuid().ToString("N");
                }
                if (videoBase64 != null && requestPath != null)
                {
                    var filePath = string.Concat(_webHostEnvironment.WebRootPath, "\\", requestPath);
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath); //Create directory if it doesn't exist
                    }
                    var fileNameWithPath = Path.Combine(filePath, fileName); // Combine file path

                    if (File.Exists(fileNameWithPath)) // Check if file exists
                    {
                        File.Delete(fileNameWithPath); // Delete the existing file
                    }

                    byte[] videoBytes = Convert.FromBase64String(videoBase64);
                    File.WriteAllBytes(fileNameWithPath, videoBytes); // Write new file

                    flag = true;
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ImageHelper", "SaveFileFromBase64", ex);
            }

            return flag;
        }
        #endregion

    }
}
