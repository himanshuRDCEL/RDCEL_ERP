using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.DAL.AbstractRepository;
using RDCELERP.DAL.Entities;

namespace RDCELERP.DAL.IRepository
{
    public interface IQCRatingMasterRepository : IAbstractRepository<TblQcratingMaster>
    {
        #region Get Deciding Question List for Diagnose 2.0
        /// <summary>
        /// Get Deciding Question List for Diagnose 2.0
        /// </summary>
        /// <param name="catId"></param>
        /// <returns></returns>
        public List<TblQcratingMaster>? GetDecidingQuestList(int catId);
        #endregion
        public List<TblQcratingMaster>? GetNewQue(int prodCatId);
        public TblQcratingMaster GetAgeQueweightage(int prodCatId);
    }
}
