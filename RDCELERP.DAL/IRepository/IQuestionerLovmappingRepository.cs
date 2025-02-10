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
    public interface IQuestionerLovmappingRepository : IAbstractRepository<TblQuestionerLovmapping>
    {
        public TblQuestionerLovmapping? GetRatingWeightageV2(int? id);

        #region Get QuestionerLoVMapping by ProdCatId and lovParentId
        /// <summary>
        /// Get QuestionerLoVMapping by ProdCatId and lovParentId
        /// </summary>
        /// <param name="prodCatId"></param>
        /// <param name="lovParentId"></param>
        /// <returns></returns>
        public List<TblQuestionerLovmapping>? GetQuestionerLovMapping(int? prodCatId, int? lovParentId);
        #endregion
    }
}
