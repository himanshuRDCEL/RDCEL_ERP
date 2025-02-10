using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.BAL.Helper
{
   public  interface IHtmlToPDFConverterHelper
    {
        /// <summary>
        /// Method to write error to DB
        /// </summary>
        /// <param name="Source">Source</param>
        /// <param name="Code">Code</param>
        /// <param name="ex">ex</param>
        public bool GeneratePDF(string htmlString, string requestPath, string customFileName);
        public bool GeneratePDFAPI(string htmlString, string requestPath, string customFileName);

        #region Custom layout PDF Generate
        /// <summary>
        /// Custom layout PDF Generate
        /// </summary>
        /// <param name="htmlString"></param>
        /// <param name="requestPath"></param>
        /// <param name="customFileName"></param>
        /// <returns></returns>
        public bool GenerateCustomLayoutPDF(string htmlString, string requestPath, string customFileName);
        #endregion
    }
}
