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
    public class TempDataRepository : AbstractRepository<TblTempDatum>, ITempDataRepository
    {
        Digi2l_DevContext _context;
        public TempDataRepository(Digi2l_DevContext dbContext)
      : base(dbContext)
        {
            _context = dbContext;
        }
        public TblTempDatum GetTempDataByFileName(string? filename)
        {
            TblTempDatum? tblTempDatum = null;
            if (!string.IsNullOrEmpty(filename))
            {
                tblTempDatum = _context.TblTempData.FirstOrDefault(x => x.FileName == filename && x.IsActive == true);
            }
            return tblTempDatum;
        }

        public List<TblTempDatum>? GetMediaFilesTempDataList(string? regdNo)
        {
            List<TblTempDatum>? tblTempDatum = null;
            if (!string.IsNullOrEmpty(regdNo))
            {
                tblTempDatum = _context.TblTempData.Where(x => x.IsActive == true && x.RegdNo == regdNo && x.ImageLabelid > 0).ToList();
            }
            return tblTempDatum;
        }
    }
}
