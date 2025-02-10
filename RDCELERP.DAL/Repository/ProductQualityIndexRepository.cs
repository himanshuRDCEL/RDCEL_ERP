using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
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
    public class ProductQualityIndexRepository : AbstractRepository<TblProductQualityIndex>, IProductQualityIndexRepository
    {
        Digi2l_DevContext _context;
        IErrorLogRepository _errorLogRepository;
        public ProductQualityIndexRepository(Digi2l_DevContext dbContext, IErrorLogRepository errorLogRepository)
       : base(dbContext)
        {
            _context = dbContext;
            _errorLogRepository = errorLogRepository;
        }

        public TblProductQualityIndex GetProductQualityIndexByProductCategoryId(int productCategoryId)
        {
            TblProductQualityIndex? result = new TblProductQualityIndex();

            try
            {
                result = _context.TblProductQualityIndices.Where(x => x.ProductCategoryId == productCategoryId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _errorLogRepository.WriteErrorToDB("ProductQualityIndexRepository", "GetProductQualityIndexByProductCategoryId", ex);
            }

            return result;
        }

        public void UpdateRecord(TblProductQualityIndex item, TblProductCategory Category)
        {
            try
            {
                var trackedReference = _context.TblProductQualityIndices.Local.FirstOrDefault(x => x.ProductQualityIndexId == item.ProductQualityIndexId);

                if (trackedReference == null)
                {
                    var ExistingRecord = _context.TblProductQualityIndices.Where(x => x.ProductQualityIndexId == item.ProductQualityIndexId).FirstOrDefault();

                    if (ExistingRecord != null)
                    {
                        ExistingRecord.AverageDesc = item?.AverageDesc?.Trim();
                        ExistingRecord.GoodDesc = item?.GoodDesc?.Trim();
                        ExistingRecord.Name = item?.Name?.Trim();
                        ExistingRecord.NonWorkingDesc = item?.NonWorkingDesc?.Trim();
                        ExistingRecord.ExcellentDesc = item?.ExcellentDesc?.Trim();
                        ExistingRecord.ModifiedDate = item?.ModifiedDate;
                        ExistingRecord.CategoryName = Category?.Description;
                        ExistingRecord.ModifiedBy = item?.ModifiedBy;
                        ExistingRecord.IsActive = item?.IsActive;
                        ExistingRecord.ProductQualityIndexId = item.ProductQualityIndexId;
                        ExistingRecord.ProductCategoryId = item.ProductCategoryId;
                    }
                    _context.TblProductQualityIndices.Update(ExistingRecord);
                }
                else if (!Object.ReferenceEquals(trackedReference, item))
                {
                    trackedReference.AverageDesc = item?.AverageDesc?.Trim();
                    trackedReference.GoodDesc = item?.GoodDesc?.Trim();
                    trackedReference.Name = item.Name;
                    trackedReference.NonWorkingDesc = item?.NonWorkingDesc?.Trim();
                    trackedReference.ExcellentDesc = item?.ExcellentDesc?.Trim();
                    trackedReference.ModifiedDate = item?.ModifiedDate;
                    trackedReference.CategoryName = Category?.Description;
                    trackedReference.ModifiedBy = item?.ModifiedBy;
                    trackedReference.IsActive = true;
                    trackedReference.ProductQualityIndexId = item.ProductQualityIndexId;
                    trackedReference.ProductCategoryId = item.ProductCategoryId;
                    _context.TblProductQualityIndices.Update(trackedReference);
                }
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                _errorLogRepository.WriteErrorToDB("ProductConditionLabelRepository", "GetProductConditionByBUBP", ex);
            }
        }
    }
}
