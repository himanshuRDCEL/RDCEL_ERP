using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.BAL.Helper
{
   public  interface IImageHelper
    {
        #region Save Image file from the File Object
        /// <summary>
        /// Save Image file from the File Object
        /// </summary>
        /// <param name="requestFile"></param>
        /// <param name="requestPath"></param>
        /// <param name="customFileName"></param>
        /// <returns></returns>
        public bool SaveFile(IFormFile requestFile, string requestPath, string customFileName);
        #endregion

        #region Save Image file from the Base 64 String Object
        /// <summary>
        /// Save Image file from the Base 64 String Object
        /// </summary>
        /// <param name="imageBase64"></param>
        /// <param name="requestPath"></param>
        /// <param name="customFileName"></param>
        /// <returns></returns>
        public bool SaveFileFromBase64(string imageBase64, string requestPath, string customFileName);
        #endregion

        public bool SaveFileDefRoot(IFormFile requestFile, string requestPath, string customFileName);

        #region Get Image Path by Base Path and ImageName
        /// <summary>
        /// Get Image Path by Base Path and ImageName
        /// </summary>
        /// <param name="basePath"></param>
        /// <param name="imageName"></param>
        /// <returns></returns>
        public string GetImageSrc(string basePath, string imageName);
        #endregion

        public bool SaveVideoFileFromBase64(string videoBase64, string requestPath, string customFileName);
    }
}
