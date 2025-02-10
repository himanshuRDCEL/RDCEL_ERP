using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.DAL.AbstractRepository;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;

namespace RDCELERP.DAL.Repository
{
    public class UniversalPriceMasterRepository : AbstractRepository<TblUniversalPriceMaster>, IUniversalPriceMasterRepository
    {
        Digi2l_DevContext _context;
        IErrorLogRepository _errorLogRepository;

        public UniversalPriceMasterRepository(Digi2l_DevContext dbContext, IErrorLogRepository errorLogRepository)
         : base(dbContext)
        {
            _context = dbContext;
            _errorLogRepository = errorLogRepository;
        }

        public TblUniversalPriceMaster GetQCPriceByPriceMasterNameId(int? PriceMasterNameId, int? ProductTypeId)
        {
            TblUniversalPriceMaster? tblUniversalPriceMaster = null;
            try
            {
                if (PriceMasterNameId != null)
                {
                    tblUniversalPriceMaster = _context.TblUniversalPriceMasters
                        .Include(x => x.ProductType)
                        .FirstOrDefault(x => x.IsActive == true && x.ProductType != null && x.PriceMasterNameId == PriceMasterNameId && x.ProductType.Id == ProductTypeId);
                }
                else
                    tblUniversalPriceMaster = null;
            }
            catch (Exception ex)
            {
                WriteError("PriceMasterMappingRepository", "GetPriceMasterMappingById", ex);
            }

            return tblUniversalPriceMaster;
        }


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
