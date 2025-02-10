using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.DAL.AbstractRepository;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;

namespace RDCELERP.DAL.Repository
{
    public class ErrorLogRepository : AbstractRepository<TblErrorLog>, IErrorLogRepository
    {
        private readonly Digi2l_DevContext _dbContext;
        public ErrorLogRepository(Digi2l_DevContext dbContext)
       : base(dbContext)
        {
            _dbContext = dbContext;
        }

        //public void WriteError(string Source, string Code, Exception ex = null)
        //{
        //    //ErrorLogRepository errorLogRepository = new ErrorLogRepository();
        //    TblErrorLog errorLog = null;

        //    try
        //    {
        //        string message = ex != null ? ex.Message : string.Empty;
        //        string stackTrace = ex != null ? ex.StackTrace : string.Empty;
        //        message = message + Environment.NewLine + "StackTrace :" + stackTrace;
        //        errorLog = new TblErrorLog();
        //        errorLog.ClassName = Source;
        //        errorLog.MethodName = Code;
        //        errorLog.ErrorMessage = message;
        //        errorLog.CreatedDate = DateTime.Now;
        //        _errorLogRepository.Create(errorLog);
        //        _errorLogRepository.SaveChanges();

        //    }
        //    catch (Exception ex1)
        //    {
        //        string message = ex != null ? ex1.Message : string.Empty;
        //        string stackTrace = ex != null ? ex1.StackTrace : string.Empty;
        //        message = message + Environment.NewLine + "StackTrace :" + stackTrace;
        //        errorLog = new TblErrorLog();
        //        errorLog.ClassName = Source;
        //        errorLog.MethodName = Code;
        //        errorLog.ErrorMessage = message;
        //        errorLog.CreatedDate = DateTime.Now;
        //        _errorLogRepository.Create(errorLog);
        //        _errorLogRepository.SaveChanges();
        //    }
        //}

        #region Write Error Log To DB    Modified by VK Date 11-Oct-2023
        public void WriteErrorToDB(string Source, string Code, Exception ex = null)
        {
            //ErrorLogRepository errorLogRepository = new ErrorLogRepository();
            TblErrorLog errorLog = null;

            try
            {
                string message = ex != null ? ex.Message : string.Empty;
                string stackTrace = ex != null ? ex.StackTrace : string.Empty;
                message = message + Environment.NewLine + "StackTrace :" + stackTrace;
                errorLog = new TblErrorLog();
                errorLog.ClassName = Source;
                errorLog.MethodName = Code;
                errorLog.ErrorMessage = message;
                errorLog.CreatedDate = DateTime.Now;
                _dbContext.Add(errorLog);
                _dbContext.SaveChanges();

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
                _dbContext.Add(errorLog);
                _dbContext.SaveChanges();
            }
        }
        #endregion
    }
}
