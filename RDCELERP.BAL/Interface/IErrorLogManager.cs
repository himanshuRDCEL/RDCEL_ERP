using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.BAL.Interface
{
    public interface IErrorLogManager
    {

        /// <summary>
        /// Method to write error log in to DB
        /// </summary>
        /// <param name="className"></param>
        /// <param name="methodName"></param>
        /// <param name="ex"></param>
        void WriteErrorToLog(string className, string methodName, Exception ex = null);



        /// <summary>
        /// Method to write error log in to DB async
        /// </summary>
        /// <param name="className"></param>
        /// <param name="methodName"></param>
        /// <param name="ex"></param>
        Task WriteErrorToLogAsync(string className, string methodName, Exception ex = null);
    }
}
