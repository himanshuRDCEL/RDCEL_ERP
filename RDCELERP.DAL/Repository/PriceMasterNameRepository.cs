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
    public class PriceMasterNameRepository : AbstractRepository<TblPriceMasterName>, IPriceMasterNameRepository
    {
        Digi2l_DevContext _context;
        IErrorLogRepository _errorLogRepository;

        public PriceMasterNameRepository(Digi2l_DevContext dbContext, IErrorLogRepository errorLogRepository)
         : base(dbContext)
        {
            _context = dbContext;
            _errorLogRepository = errorLogRepository;
        }

        //public PriceMasterName GetPriceMasterNameById(int? PriceMasterNameId)
        //{
        //    TblPriceMasterName priceMasterName = null;
        //    try
        //    {
        //        if (PriceMasterNameId > 0)
        //        {
        //            //priceMasterName = _context.PriceMasterNames.FirstOrDefault(x => x.IsActive == true && x.PriceMasterNameId == PriceMasterNameId);
        //        }
        //        else
        //            priceMasterName = null;
        //    }
        //    catch(Exception ex)
        //    {
        //        WriteError("PriceMasterNameRepository", "GetPriceMasterNameById",ex);
        //    }
            
        //    return priceMasterName;
        //}

        public void WriteError(string Source, string Code, Exception ex = null)
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
                _errorLogRepository.Create(errorLog);
                _errorLogRepository.SaveChanges();

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
