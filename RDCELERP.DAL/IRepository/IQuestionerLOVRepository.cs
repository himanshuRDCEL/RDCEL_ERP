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

    public interface IQuestionerLOVRepository : IAbstractRepository<TblQuestionerLov>
    {
        #region Get Questioner LoV Rating Weightage
        /// <summary>
        /// Get Questioner LoV Rating Weightage
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TblQuestionerLov? GetRatingWeightage(int? id);
        #endregion

        #region Get Questioner LoV List by Parent Id
        /// <summary>
        /// Get Questioner LoV List by Parent Id
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public List<TblQuestionerLov>? GetQuestionerLovListByPId(int? parentId);
        #endregion
    }
}
