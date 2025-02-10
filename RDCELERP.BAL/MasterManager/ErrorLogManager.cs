using AutoMapper;
using RDCELERP.Common.Constant;
using RDCELERP.Common.Helper;
using RDCELERP.DAL;
using RDCELERP.DAL.IRepository;
using RDCELERP.DAL.Repository;
using RDCELERP.Model.Company;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.DAL.Entities;
using RDCELERP.BAL.Interface;

namespace RDCELERP.BAL.MasterManager
{
    public class ErrorLogManager : IErrorLogManager
    {
        #region  Variable Declaration
        IErrorLogRepository _errorLogRepository;
        private readonly IMapper _mapper;
        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        #endregion


        public ErrorLogManager(IErrorLogRepository errorLogRepository, RDCELERP.DAL.Entities.Digi2l_DevContext context , IMapper mapper)
        {
            _errorLogRepository = errorLogRepository;
            _mapper = mapper;
            _context = context;
        }

        /// <summary>
        /// Method to write error log in to DB
        /// </summary>
        /// <param name="className"></param>
        /// <param name="methodName"></param>
        /// <param name="ex"></param>
        public void WriteErrorToLog(string className, string methodName, Exception ex = null)
        {
            TblErrorLog TblErrorLog = new TblErrorLog();
            try
            {
                TblErrorLog.ClassName = className;
                TblErrorLog.MethodName = methodName;
                string message = ex != null ? ex.Message : string.Empty;
                string stackTrace = ex != null ? ex.StackTrace : string.Empty;
                TblErrorLog.ErrorMessage = "Message :" + message + Environment.NewLine + "StackTrace: " + stackTrace;
                TblErrorLog.CreatedDate = DateTime.Now.TrimMilliseconds();
                _errorLogRepository.Create(TblErrorLog);
                _errorLogRepository.SaveChanges();

            }
            catch (Exception exi)
            {
                TblErrorLog.ClassName = className;
                TblErrorLog.MethodName = methodName;
                string message = exi != null ? exi.Message : string.Empty;
                string stackTrace = exi != null ? exi.StackTrace : string.Empty;
                TblErrorLog.ErrorMessage = "Message :" + message + Environment.NewLine + "StackTrace: " + stackTrace;
                TblErrorLog.CreatedDate = DateTime.Now.TrimMilliseconds();
                _errorLogRepository.Create(TblErrorLog);
                _errorLogRepository.SaveChanges();
            }

        }

        /// <summary>
        /// Method to write error log in to DB
        /// </summary>
        /// <param name="className"></param>
        /// <param name="methodName"></param>
        /// <param name="ex"></param>
        public async Task WriteErrorToLogAsync(string className, string methodName, Exception ex = null)
        {
            TblErrorLog TblErrorLog = new TblErrorLog();
            try
            {
                TblErrorLog.ClassName = className;
                TblErrorLog.MethodName = methodName;
                string message = ex != null ? ex.Message : string.Empty;
                string stackTrace = ex != null ? ex.StackTrace : string.Empty;
                TblErrorLog.ErrorMessage = "Message :" + message + Environment.NewLine + "StackTrace: " + stackTrace;
                TblErrorLog.CreatedDate = DateTime.Now.TrimMilliseconds();

                await _context.TblErrorLogs.AddAsync(TblErrorLog);
                await _context.SaveChangesAsync();


            }
            catch (Exception exi)
            {
                TblErrorLog.ClassName = className;
                TblErrorLog.MethodName = methodName;
                string message = exi != null ? exi.Message : string.Empty;
                string stackTrace = exi != null ? exi.StackTrace : string.Empty;
                TblErrorLog.ErrorMessage = "Message :" + message + Environment.NewLine + "StackTrace: " + stackTrace;
                TblErrorLog.CreatedDate = DateTime.Now.TrimMilliseconds();
                await _context.TblErrorLogs.AddAsync(TblErrorLog);
                await _context.SaveChangesAsync();
            }

        }

    }
}
