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
    public class QuestionerLovmappingRepository : AbstractRepository<TblQuestionerLovmapping>, IQuestionerLovmappingRepository
    {
        private readonly Digi2l_DevContext _context;
        public QuestionerLovmappingRepository(Digi2l_DevContext dbContext) : base(dbContext)
        {
            _context = dbContext;   
        }

        #region Get QuestionerLoVMapping Rating Weightage -- YR
        /// <summary>
        /// Get QuestionerLoVMapping  Rating Weightage
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TblQuestionerLovmapping? GetRatingWeightageV2(int? id)
        {
            TblQuestionerLovmapping? tblQuestionerLovmapping = null;
            if (id > 0)
            {
                tblQuestionerLovmapping = _context.TblQuestionerLovmappings
                    .Where(x => x.IsActive == true && x.QuestionerLovmappingId == id)
                    .FirstOrDefault();
            }
            return tblQuestionerLovmapping;
        }
        #endregion

        #region Get QuestionerLoVMapping by ProdCatId and lovParentId
        /// <summary>
        /// Get QuestionerLoVMapping by ProdCatId and lovParentId
        /// </summary>
        /// <param name="prodCatId"></param>
        /// <param name="lovParentId"></param>
        /// <returns></returns>
        public List<TblQuestionerLovmapping>? GetQuestionerLovMapping(int? prodCatId, int? lovParentId)
        {
            List<TblQuestionerLovmapping>? tblQuestionerLovmappList = null;
            if (prodCatId > 0 && lovParentId > 0)
            {
                tblQuestionerLovmappList = _context.TblQuestionerLovmappings
                    .Include(x=>x.QuestionerLov)
                    .Where(x => x.IsActive == true && x.ProductCatId == prodCatId && x.ParentId == lovParentId)
                    .ToList();
            }
            return tblQuestionerLovmappList;
        }
        #endregion
    }

}
