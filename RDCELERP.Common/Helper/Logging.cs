using AutoMapper;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.DAL.Repository;

namespace RDCELERP.Common.Helper
{
   public  class Logging : ILogging
    {
        #region  Variable Declaration
        private readonly IErrorLogRepository _errorLogRepository;
        private readonly IMapper _mapper;
        #endregion


        public Logging(IErrorLogRepository errorLogRepository, IMapper mapper)
        {
            _errorLogRepository = errorLogRepository;
            _mapper = mapper;
        }

        public void WriteAPIRequestToDB(string Source, string Code, string sponsorOrderNo, string jsonString)
        {
            TblErrorLog errorLog = null;

            try
            {
                errorLog = new TblErrorLog();
                errorLog.ClassName = Source;
                errorLog.MethodName = Code;
                errorLog.SponsorOrderNo = sponsorOrderNo;
                errorLog.ErrorMessage = jsonString;
                errorLog.CreatedDate = DateTime.Now;
                _errorLogRepository.Create(errorLog);
                _errorLogRepository.SaveChanges();
            }
            catch (Exception ex1)
            {
                string ex = ex1.Message;
                errorLog = new TblErrorLog();
                errorLog.ClassName = Source;
                errorLog.MethodName = Code;
                errorLog.ErrorMessage = jsonString;
                errorLog.CreatedDate = DateTime.Now;
                _errorLogRepository.Create(errorLog);
                _errorLogRepository.SaveChanges();
            }
        }

        /// <summary>
        /// Method to write error to DB
        /// </summary>
        /// <param name="Source">Source</param>
        /// <param name="Code">Code</param>
        /// <param name="ex">ex</param>
        public  void WriteErrorToDB(string Source, string Code, Exception ex = null)
        {
            //ErrorLogRepository errorLogRepository = new ErrorLogRepository();
            TblErrorLog errorLog = null;

            try
            {
               // int addLog = Convert.ToInt32(ConfigurationManager.AppSettings["AddLog"]);
                //int addLog = 1;
                //if (addLog == 1)
                //{
                    string message = ex != null ? ex.Message : string.Empty;
                    string stackTrace = ex != null ? ex.StackTrace : string.Empty;
                    message = message + Environment.NewLine + "StackTrace :" + stackTrace;
                    errorLog = new TblErrorLog();
                    errorLog.ClassName = Source;
                    errorLog.MethodName = Code;
                    errorLog.ErrorMessage = message;
                    errorLog.CreatedDate = DateTime.Now;
                    _errorLogRepository.Create(errorLog);
                    _errorLogRepository.SaveChanges();
              //}

            }
            catch (Exception ex1)
            {
                string message = ex != null ? ex1.Message : string.Empty;
                string stackTrace = ex != null ? ex1.StackTrace : string.Empty;
                message = message + Environment.NewLine + "StackTrace :" + stackTrace;
                errorLog = new TblErrorLog();
                errorLog.ClassName = Source;
                errorLog.MethodName = Code;
                errorLog.ErrorMessage = message;
                errorLog.CreatedDate = DateTime.Now;
                _errorLogRepository.Create(errorLog);
                _errorLogRepository.SaveChanges();
            }
        }
    }
}
