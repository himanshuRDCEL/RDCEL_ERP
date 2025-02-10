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
    public interface IPriceMasterQuestionersRepository : IAbstractRepository<TblPriceMasterQuestioner>
    {
        #region Get Product Category List
        /// <summary>
        /// Get Product Category List
        /// </summary>
        /// <returns>List<TblProductCategory></returns>
        public List<TblPriceMasterQuestioner>? GetProdCatList();
        #endregion

        #region Get Product Type List
        /// <summary>
        /// Get Product Type List
        /// </summary>
        /// <param name="prodCatId"></param>
        /// <returns></returns>
        public List<TblPriceMasterQuestioner>? GetProdTypeListByCatId(int? prodCatId);
        #endregion
        public List<TblPriceMasterQuestioner>? GetProdTechnologyListByCatId(int? prodCatId);

        #region Get ASP by Prod Type and Technology
        public TblPriceMasterQuestioner? GetBaseASPByTypeAndTech(int? prodTypeId, int? prodTechId);
        #endregion
    }
}
