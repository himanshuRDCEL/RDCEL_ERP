using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.Repository;

namespace RDCELERP.Common.Helper
{
   public  interface ILogging
    {
        /// <summary>
        /// Method to write error to DB
        /// </summary>
        /// <param name="Source">Source</param>
        /// <param name="Code">Code</param>
        /// <param name="ex">ex</param>
        void WriteErrorToDB(string Source, string Code, Exception ex = null);
        public void WriteAPIRequestToDB(string Source, string Code, string sponsorOrderNo, string jsonString);
    }
}
