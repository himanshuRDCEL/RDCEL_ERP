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
    public class ModelNumberRepository : AbstractRepository<TblModelNumber>, IModelNumberRepository
    {
        Digi2l_DevContext _context;
        public ModelNumberRepository(Digi2l_DevContext dbContext)
         : base(dbContext)
        {
            _context = dbContext;
        }

        public (List<TblModelNumber>, List<int>) GetListOfModelNumbersForBU(int? Buid)
        {
            List<TblModelNumber> modelNumberRecords = new List<TblModelNumber>();
            List<TblModelNumber> DefaultEntries = new List<TblModelNumber>();
            List<int> defaultModelNumberIds = new List<int>();

            if (Buid > 0)
            {
                modelNumberRecords = _context.TblModelNumbers.Where(x => x.BusinessUnitId == Buid && x.IsActive == true && x.IsDefaultProduct != true).ToList();

                if(modelNumberRecords.Count > 0)
                {
                    var presentedProductTypeIds = _context.TblModelNumbers.Where(x => x.BusinessUnitId == Buid && x.IsActive == true).Select(x => x.ProductTypeId).Distinct().ToList();
                    if (presentedProductTypeIds != null)
                    {
                        var GetDefaultEntriesForProductIds = _context.TblModelNumbers.Where(x => x.BusinessUnitId == Buid && presentedProductTypeIds.Contains(x.ProductTypeId) && x.ModelName == null && x.IsDefaultProduct == true && x.IsActive == true).ToList() ?? new List<TblModelNumber>();
                        DefaultEntries = GetDefaultEntriesForProductIds;
                    }
                }
                modelNumberRecords.AddRange(DefaultEntries);
                defaultModelNumberIds = DefaultEntries.Select(x=>x.ModelNumberId).Distinct().ToList();  
            }

            return (modelNumberRecords,defaultModelNumberIds);
        }

        /// <summary>
        /// Method to manage (Add/Edit) Model Number 
        /// </summary>
        /// <param name="ModelNumberVM">ModelNumberVM</param>
        /// <param name="userId">userId</param>
        /// <returns>int</returns>

        public int UpdateModelNumber(int? modelNumberId, decimal? DtoC, decimal? DtoD, int? BusinessUnitId, int? userid)
        {
            TblModelNumber TblModelNumber = _context.TblModelNumbers.FirstOrDefault(x => x.IsActive == true && x.ModelNumberId == modelNumberId);

            TblModelNumber.ModelNumberId = (int)modelNumberId;
            TblModelNumber.BusinessUnitId = (int)BusinessUnitId;
            TblModelNumber.ModifiedBy = userid;
            TblModelNumber.SweetnerForDtc = DtoC;
            TblModelNumber.SweetnerForDtd = DtoD;
            TblModelNumber.ModifiedDate = DateTime.Now;
            _context.Update(TblModelNumber);
            _context.SaveChanges();
            return TblModelNumber.ModelNumberId;

        }
    }
}
