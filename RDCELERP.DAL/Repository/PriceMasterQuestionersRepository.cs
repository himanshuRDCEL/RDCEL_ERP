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
   public  class PriceMasterQuestionersRepository : AbstractRepository<TblPriceMasterQuestioner>, IPriceMasterQuestionersRepository
    {
        private readonly Digi2l_DevContext _context;
        public PriceMasterQuestionersRepository(Digi2l_DevContext dbContext)
       : base(dbContext)
        {
            _context = dbContext;
        }

        #region Get Product Category List
        /// <summary>
        /// Get Product Category List
        /// </summary>
        /// <returns>List<TblProductCategory></returns>
        public List<TblPriceMasterQuestioner>? GetProdCatList()
        {
            List<TblPriceMasterQuestioner>? tblPriceMasterQuestList = null;
            tblPriceMasterQuestList = _context.TblPriceMasterQuestioners .Include(x => x.ProductCat)
                   .Where(x => x.IsActive == true).GroupBy(x => x.ProductCatId).Select(x => x.FirstOrDefault()).ToList();
            return tblPriceMasterQuestList;
        }
        #endregion

        #region Get Product Type List
        /// <summary>
        /// Get Product Type List
        /// </summary>
        /// <param name="prodCatId"></param>
        /// <returns></returns>
        public List<TblPriceMasterQuestioner>? GetProdTypeListByCatId(int? prodCatId)
        {
            List<TblPriceMasterQuestioner>? tblPriceMasterQuestList = null;
            if (prodCatId > 0)
            {
                tblPriceMasterQuestList = _context.TblPriceMasterQuestioners
                     .Include(x => x.ProductType)
                    .Where(x => x.IsActive == true && x.ProductType != null && x.ProductCatId == prodCatId)
                    .GroupBy(x => x.ProductTypeId)
                    .Select(x => x.FirstOrDefault())
                    .ToList();
            }
            return tblPriceMasterQuestList;
        }
        #endregion


        #region Get Product technology List
        /// <summary>
        /// Get Product Type List
        /// </summary>
        /// <param name="prodCatId"></param>
        /// <returns></returns>
        public List<TblPriceMasterQuestioner>? GetProdTechnologyListByCatId(int? prodCatId)
        {
            List<TblPriceMasterQuestioner>? tblPriceMasterQuestList = null;
            if (prodCatId > 0)
            {
                tblPriceMasterQuestList = _context.TblPriceMasterQuestioners
                     .Include(x => x.ProductTechnology)
                    .Where(x => x.IsActive == true && x.ProductTechnology != null && x.ProductCatId == prodCatId)
                    .GroupBy(x => x.ProductTechnologyId)
                    .Select(x => x.FirstOrDefault())
                    .ToList();
            }
            return tblPriceMasterQuestList;
        }
        #endregion

        #region Get ASP by Prod Type and Technology
        /// <summary>
        /// Get ASP by Prod Type and Technology
        /// </summary>
        /// <param name="prodTypeId"></param>
        /// <param name="prodTechId"></param>
        /// <returns></returns>
        public TblPriceMasterQuestioner? GetBaseASPByTypeAndTech(int? prodTypeId, int? prodTechId)
        {
            TblPriceMasterQuestioner? tblPriceMasterQuest = null;
            if (prodTypeId > 0 && prodTechId > 0)
            {
                tblPriceMasterQuest = _context.TblPriceMasterQuestioners
                    .Where(x => x.IsActive == true && x.ProductTypeId == prodTypeId && x.ProductTechnologyId == prodTechId)
                    .FirstOrDefault();
            }
            return tblPriceMasterQuest;
        }
        #endregion
    }
}
