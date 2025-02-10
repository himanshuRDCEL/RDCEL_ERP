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
    public class PriceMasterMappingRepository : AbstractRepository<TblPriceMasterMapping>, IPriceMasterMappingRepository
    {
        Digi2l_DevContext _context;
        IErrorLogRepository _errorLogRepository;

        public PriceMasterMappingRepository(Digi2l_DevContext dbContext, IErrorLogRepository errorLogRepository)
         : base(dbContext)
        {
            _context = dbContext;
            _errorLogRepository = errorLogRepository;
        }

        public TblPriceMasterMapping GetPriceMasterMappingById(int? PriceMasterMappingId)
        {
            TblPriceMasterMapping priceMasterMapping = null;
            try
            {
                if (PriceMasterMappingId > 0)
                {
                    priceMasterMapping = _context.TblPriceMasterMappings.FirstOrDefault(x => x.IsActive == true && x.PriceMasterMappingId == PriceMasterMappingId);
                }
                else
                    priceMasterMapping = null;
            }
            catch (Exception ex)
            {
                WriteError("PriceMasterMappingRepository", "GetPriceMasterMappingById", ex);
            }

            return priceMasterMapping;
        }

        public TblPriceMasterMapping GetProductPriceByBUIdBPIdBrandId(int? BUId, int? BPId, int? NewBrandId)
        {
            TblPriceMasterMapping priceMasterMapping = null;
            try
            {
                if(BUId > 0 && BPId > 0 && NewBrandId > 0)
                {
                    priceMasterMapping = _context.TblPriceMasterMappings.Where(x => x.IsActive == true && x.BusinessUnitId == BUId && x.BusinessPartnerId == BPId && x.BrandId == NewBrandId).FirstOrDefault();
                    if(priceMasterMapping == null)
                    {
                        priceMasterMapping = _context.TblPriceMasterMappings.Where(x => x.IsActive == true && x.BusinessUnitId == BUId && x.BusinessPartnerId == BPId && x.BrandId == null).FirstOrDefault();
                        if(priceMasterMapping == null)
                        {
                            priceMasterMapping = _context.TblPriceMasterMappings.Where(x => x.IsActive == true && x.BusinessUnitId == BUId && x.BusinessPartnerId == null).FirstOrDefault();
                        }
                        else
                        {
                            priceMasterMapping = new TblPriceMasterMapping();
                        }
                    }
                }
                else
                {
                    priceMasterMapping = new TblPriceMasterMapping();
                }
                //else if (BUId > 0 && BPId > 0)
                //{
                //    priceMasterMapping = _context.TblPriceMasterMappings.Where(x => x.IsActive == true && x.BusinessUnitId == BUId && x.BusinessPartnerId == BPId && x.BrandId == null).FirstOrDefault();
                //}
                //else if (BUId > 0)
                //{
                //    priceMasterMapping = _context.TblPriceMasterMappings.Where(x => x.IsActive == true && x.BusinessUnitId == BUId && x.BusinessPartnerId == null).FirstOrDefault();
                //}
                //else
                //{
                //    priceMasterMapping = null;
                //}
            }
            catch (Exception ex)
            {
                WriteError("PriceMasterMappingRepository", "GetPriceMasterMappingById", ex);
            }

            return priceMasterMapping;
        }


        public TblPriceMasterMapping GetProductPriceByBUIdBPId(int? BUId, int? BPId)
        {
            TblPriceMasterMapping priceMasterMapping = null;
            try
            {
                if (BUId > 0 && BPId > 0)
                {
                    priceMasterMapping = _context.TblPriceMasterMappings.Where(x => x.IsActive == true && x.BusinessUnitId == BUId && x.BusinessPartnerId == BPId && x.BrandId == null).FirstOrDefault();
                    if (priceMasterMapping == null)
                    {
                        priceMasterMapping = _context.TblPriceMasterMappings.Where(x => x.IsActive == true && x.BusinessUnitId == BUId && x.BusinessPartnerId == null).FirstOrDefault();
                    }
                    else
                    {
                        priceMasterMapping = new TblPriceMasterMapping();
                    }
                }
                else
                {
                    priceMasterMapping = new TblPriceMasterMapping();
                }
            }
            catch (Exception ex)
            {
                WriteError("PriceMasterMappingRepository", "GetPriceMasterMappingById", ex);
            }

            return priceMasterMapping;
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
