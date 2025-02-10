using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.DAL.AbstractRepository;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;

namespace RDCELERP.DAL.Repository
{
    public class ProductConditionLabelRepository : AbstractRepository<TblProductConditionLabel>, IProductConditionLabelRepository
    {
        Digi2l_DevContext _context;
        IErrorLogRepository _errorLogRepository;

        public ProductConditionLabelRepository(Digi2l_DevContext dbContext, IErrorLogRepository errorLogRepository)
         : base(dbContext)
        {
            _context = dbContext;
            _errorLogRepository = errorLogRepository;
        }

        public List<TblProductConditionLabel> GetProductConditionByBUBP(int? BPId, int? BUId, int? proCatId = null)
        {
            List<TblProductConditionLabel>? TblProductConditionLabel = null;
            try
            {
                if (BUId > 0 && BPId > 0)
                {
                    TblProductConditionLabel = _context.TblProductConditionLabels.Where(x => x.IsActive == true && x.BusinessUnitId == BUId && x.BusinessPartnerId == BPId && x.ProductCatId == proCatId).OrderBy(x => x.OrderSequence).ToList();
                    if (!(TblProductConditionLabel != null && TblProductConditionLabel.Count > 0))
                    {
                        TblProductConditionLabel = _context.TblProductConditionLabels.Where(x => x.IsActive == true && x.BusinessUnitId == BUId && x.BusinessPartnerId == BPId && x.ProductCatId == null).OrderBy(x => x.OrderSequence).ToList();
                    }
                }
                else
                    TblProductConditionLabel = null;
            }
            catch (Exception ex)
            {
                WriteError("ProductConditionLabelRepository", "GetProductConditionByBUBP", ex);
            }

            return TblProductConditionLabel;
        }

        public TblProductConditionLabel GetOrderSequenceNo(int? PClableId)
        {
            TblProductConditionLabel TblProductConditionLabel = null;
            try
            {
                if (PClableId > 0)
                {
                    TblProductConditionLabel = _context.TblProductConditionLabels.FirstOrDefault(x => x.IsActive == true && x.Id == PClableId);
                }
                else
                    TblProductConditionLabel = null;
            }
            catch (Exception ex)
            {
                WriteError("ProductConditionLabelRepository", "GetProductConditionByBUBP", ex);
            }

            return TblProductConditionLabel;
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

        public bool InsertBulkReords(List<TblProductConditionLabel> tblProductConditionLabels)
        {
            bool result = false;
            try
            {
                if (tblProductConditionLabels != null)
                {
                    _context.TblProductConditionLabels.AddRange(tblProductConditionLabels);
                    _context.SaveChanges();
                    result = true;
                }
            }
            catch (DbUpdateException DbException)
            {
                result = false;
                WriteError("ProductConditionLabelRepository", "InsertRecords", DbException);
            }
            catch (Exception ex)
            {
                result = false;
                WriteError("ProductConditionLabelRepository", "InsertRecords", ex);
            }
            return result;
        }
        public async Task UpdateBulkReords(List<TblProductConditionLabel> tblProductConditionLabels)
        {
            try
            {
                if (tblProductConditionLabels != null)
                {
                    _context.TblProductConditionLabels.UpdateRange(tblProductConditionLabels);
                    await _context.SaveChangesAsync();
                }
            }
            catch (DbUpdateException DbException)
            {
                WriteError("ProductConditionLabelRepository", "InsertRecords", DbException);
            }
            catch (Exception ex)
            {
                WriteError("ProductConditionLabelRepository", "InsertRecords", ex);
            }
        }

        public List<TblProductConditionLabel> GetProductConditionLabelByBusinessPartnerId(int? BusinessPartnerId)
        {
            List<TblProductConditionLabel> listOfProductConditionLabel = new List<TblProductConditionLabel>();
            try
            {

                listOfProductConditionLabel = _context.TblProductConditionLabels.Where(x => x.BusinessPartnerId.Equals(BusinessPartnerId)).ToList();

                return listOfProductConditionLabel;
            }
            catch(DbException ex)
            {
                WriteError("ProductConditionLabelRepository", "GetProductConditionLabelByBusinessPartnerId", ex);
                return listOfProductConditionLabel;
            }
        }
        public void DeleteProductConditionLabelsForBP(int BusinessPartnerId)
        {
            var labels = GetProductConditionLabelByBusinessPartnerId(BusinessPartnerId);

            if (labels != null && labels.Count > 0)
            {
                try
                {
                    _context.TblProductConditionLabels.RemoveRange(labels);
                    _context.SaveChanges();
                }
                catch (DbException dbException)
                {
                    WriteError("ProductConditionLabelRepository", "DeleteProductConditionLabelsForBP", dbException);
                }
                catch (Exception ex)
                {
                    WriteError("ProductConditionLabelRepository", "DeleteProductConditionLabelsForBP", ex);
                }
            }

            
        }

        public int updateProductConditionLabel(TblProductConditionLabel TblProductConditionLabel)
        {
            TblProductConditionLabel tblProductConditionLabel = _context.TblProductConditionLabels.Where(x => x.Id == TblProductConditionLabel.Id).FirstOrDefault();

            try
            {
                if (TblProductConditionLabel != null && tblProductConditionLabel != null)
                {
                    tblProductConditionLabel.Id = TblProductConditionLabel.Id;
                    tblProductConditionLabel.IsActive = TblProductConditionLabel.IsActive;
                    tblProductConditionLabel.ModifiedBy = TblProductConditionLabel.ModifiedBy;
                    tblProductConditionLabel.Modifieddate = TblProductConditionLabel.Modifieddate;
                    tblProductConditionLabel.CreatedBy = TblProductConditionLabel.CreatedBy;
                    tblProductConditionLabel.CreatedDate = TblProductConditionLabel.CreatedDate;
                    tblProductConditionLabel.IsSweetenerApplicable = TblProductConditionLabel.IsSweetenerApplicable;
                    tblProductConditionLabel.PclabelName = TblProductConditionLabel.PclabelName;
                    tblProductConditionLabel.OrderSequence = TblProductConditionLabel.OrderSequence;
                    tblProductConditionLabel.BusinessUnitId = TblProductConditionLabel.BusinessUnitId;
                    tblProductConditionLabel.BusinessPartnerId = TblProductConditionLabel.BusinessPartnerId;
                    _context.TblProductConditionLabels.UpdateRange(tblProductConditionLabel);
                    _context.SaveChanges();

                }
            }
            catch (Exception ex)
            {

                WriteError("ProductConditionLabelRepository", "updateProductConditionLabel", ex);
            }

            return TblProductConditionLabel.Id;
        }
    }
}
