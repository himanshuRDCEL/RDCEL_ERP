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
    public class QcratingMasterMappingRepository : AbstractRepository<TblQcratingMasterMapping>, IQcratingMasterMappingRepository
    {
        private readonly Digi2l_DevContext _context;
        public QcratingMasterMappingRepository(Digi2l_DevContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        #region Get New Question List for Diagnose 2.0
        /// <summary>
        /// Get New Question List for Diagnose 2.0
        /// </summary>
        /// <param name="prodcatId"></param>
        /// <returns></returns>
        public List<TblQcratingMasterMapping>? GetNewQueV2(int prodCatId, int prodtypeid, int prodtechid)
        {
            List<TblQcratingMasterMapping>? tblQcratingMasterMappings = null;
            if (prodCatId > 0 && prodtypeid > 0 && prodtechid > 0)
            {
                tblQcratingMasterMappings = _context.TblQcratingMasterMappings
                .Where(x => x.IsActive == true && x.ProductCatId == prodCatId && x.ProductTechnologyId == prodtechid && x.ProductTypeId == prodtypeid).ToList();
            }
            return tblQcratingMasterMappings;
        }

        //public List<TblQcratingMaster>? GetNewQueListV2(int? prodtypeid, int? prodtechid)
        //{
        //    List<TblQcratingMasterMapping>? tblQcratingMasterMappings = null;
        //    List<TblQcratingMaster>? tblQcratingMasters = null;
        //    if (prodtypeid > 0 && prodtechid > 0)
        //    {
        //       tblQcratingMasterMappings = _context.TblQcratingMasterMappings
        //           .Include(x=>x.Qcrating)
        //       .Where(x => x.IsActive == true && x.ProductTechnologyId == prodtechid && x.ProductTypeId == prodtypeid).ToList();
        //        if (tblQcratingMasterMappings != null && tblQcratingMasterMappings.Count > 0)
        //        {
        //            tblQcratingMasters = tblQcratingMasterMappings?.Select(x => x.Qcrating)?.ToList();
        //        }
        //    }
        //    return tblQcratingMasters;
        //}

        public List<TblQcratingMasterMapping>? GetNewQueListV2(int? prodtypeid, int? prodtechid)
        {
            List<TblQcratingMasterMapping>? tblQcratingMasterMappings = null;
            List<TblQcratingMaster>? tblQcratingMasters = null;
            if (prodtypeid > 0 && prodtechid > 0)
            {
                tblQcratingMasterMappings = _context.TblQcratingMasterMappings
                    .Include(x => x.Qcrating)
                .Where(x => x.IsActive == true && x.ProductTechnologyId == prodtechid && x.ProductTypeId == prodtypeid).ToList();
            }
            return tblQcratingMasterMappings;
        }
        #endregion
    }
}
