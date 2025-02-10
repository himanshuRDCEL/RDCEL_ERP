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
    public class QCRatingMasterRepository : AbstractRepository<TblQcratingMaster>, IQCRatingMasterRepository
    {
        private readonly Digi2l_DevContext _context;
        public QCRatingMasterRepository(Digi2l_DevContext dbContext)
      : base(dbContext)
        {
            _context = dbContext;
        }

        #region Get Deciding Question List for Diagnose 2.0
        /// <summary>
        /// Get Deciding Question List for Diagnose 2.0
        /// </summary>
        /// <param name="catId"></param>
        /// <returns></returns>
        public List<TblQcratingMaster>? GetDecidingQuestList(int catId)
        {
            List<TblQcratingMaster>? tblQcratingMasterList = null;
            if (catId > 0)
            {
                tblQcratingMasterList = _context.TblQcratingMasters
                    .Where(x => x.IsActive == true && x.IsAgeingQues != true && x.IsDecidingQues == true && x.ProductCatId == catId).ToList();
            }
            return tblQcratingMasterList;
        }
        #endregion

        #region Get New Question List for Diagnose 2.0
        /// <summary>
        /// Get New Question List for Diagnose 2.0
        /// </summary>
        /// <param name="prodcatId"></param>
        /// <returns></returns>
        public List<TblQcratingMaster>? GetNewQue(int prodCatId)
        {
            List<TblQcratingMaster> tblQcratingMasterList = null;
            tblQcratingMasterList = _context.TblQcratingMasters.Where(x => x.IsActive == true && x.ProductCatId == prodCatId && x.IsAgeingQues == null && x.IsDiagnoseV2==true).ToList();
            return tblQcratingMasterList;
        }
        #endregion
        #region Get New Question List for Diagnose 2.0
        /// <summary>
        /// Get New Question List for Diagnose 2.0
        /// </summary>
        /// <param name="prodcatId"></param>
        /// <returns></returns>
        public TblQcratingMaster GetAgeQueweightage(int prodCatId)
        {
            TblQcratingMaster? tblQcratingMaster = null;
            tblQcratingMaster = _context.TblQcratingMasters.Where(x => x.IsActive == true && x.ProductCatId == prodCatId && x.IsAgeingQues == true).FirstOrDefault();
            return tblQcratingMaster;
        }
        #endregion
    }
}
