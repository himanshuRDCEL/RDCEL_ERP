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
    public class ABBRedemptionRepository : AbstractRepository<TblAbbredemption>, IABBRedemptionRepository
    {
        #region variable declaration
        Digi2l_DevContext _context;
        #endregion

        public ABBRedemptionRepository(Digi2l_DevContext dbContext)
        : base(dbContext)
        {
            _context = dbContext;
        }

        public TblAbbredemption GetRegdNo(string regdno)
        {
            TblAbbredemption TblAbbredemption = _context.TblAbbredemptions.FirstOrDefault(x => x.IsActive == true && x.RegdNo == regdno);

            return TblAbbredemption;
        }

        public TblAbbredemption GetRedemptionData(int? RedemptionId)
        {
            TblAbbredemption? redemptionObj = new TblAbbredemption();
            if (RedemptionId > 0)
            {
                redemptionObj = _context.TblAbbredemptions.
                    Include(x => x.Abbregistration)
                    .Include(x=>x.Abbregistration.BusinessUnit)
                    .Include(x=>x.VoucherStatus)
                    .Where(x => x.RedemptionId == RedemptionId && x.IsActive == true).FirstOrDefault();
            }
            return redemptionObj;
        }

        public List<TblAbbredemption> GetRedemptionDataList(int? BusinessUnitId, string regdNo, string sponsorOrderNumber, string phoneNumber, string referenceId,string storeCode)
         {
            List<TblAbbredemption> redemptionDataObj = new List<TblAbbredemption>();
            if (BusinessUnitId > 0)
            {
                redemptionDataObj = _context.TblAbbredemptions.Include(x => x.Abbregistration)
               .Include(x => x.Abbregistration.BusinessUnit)
               .Include(x => x.Abbregistration.NewProductCategory)
               .Include(x => x.Abbregistration.NewProductCategoryTypeNavigation)
               .Where(x => x.Abbregistration.BusinessUnit.BusinessUnitId == BusinessUnitId && (string.IsNullOrEmpty(referenceId) || x.ReferenceId == referenceId) && (string.IsNullOrEmpty(sponsorOrderNumber) || x.Abbregistration.SponsorOrderNo == sponsorOrderNumber) && (string.IsNullOrEmpty(regdNo) || x.Abbregistration.RegdNo == regdNo) && (string.IsNullOrEmpty(phoneNumber) || x.Abbregistration.CustMobile == phoneNumber) && (string.IsNullOrEmpty(storeCode) || x.Abbregistration.StoreCode == storeCode)).OrderByDescending(x => x.RedemptionId).ToList();
            }
            else
            {
                redemptionDataObj = _context.TblAbbredemptions.Include(x => x.Abbregistration)
               .Include(x => x.Abbregistration.BusinessUnit)
               .Include(x => x.Abbregistration.NewProductCategory)
               .Include(x => x.Abbregistration.NewProductCategoryTypeNavigation)
               .Where(x => x.Abbregistration.BusinessUnit != null && (string.IsNullOrEmpty(referenceId) || x.ReferenceId == referenceId) && (string.IsNullOrEmpty(sponsorOrderNumber) || x.Abbregistration.SponsorOrderNo == sponsorOrderNumber) && (string.IsNullOrEmpty(regdNo) || x.Abbregistration.RegdNo == regdNo) && (string.IsNullOrEmpty(phoneNumber) || x.Abbregistration.CustMobile == phoneNumber) && (string.IsNullOrEmpty(storeCode) || x.Abbregistration.StoreCode == storeCode)).OrderByDescending(x => x.RedemptionId).ToList();
            }
            return redemptionDataObj;
        }

        #region Order Details for Payout final price
        public TblAbbredemption GetOrderDetails(string RegdNo, int? ABBRedemptionId)
        {
            TblAbbredemption redemptionObj = new TblAbbredemption();
            if (!string.IsNullOrEmpty(RegdNo) && ABBRedemptionId > 0)
            {
                redemptionObj = _context.TblAbbredemptions.FirstOrDefault(x => x.IsActive == true && x.RegdNo != null && x.RegdNo == RegdNo && x.RedemptionId == ABBRedemptionId);
            }
            return redemptionObj;
          

           
        }
        #endregion
        #region get ABB Redemption order details by regdno
        /// <summary>
        /// get ABB Redemption order details by regdno
        /// </summary>
        /// <param name="regdNo"></param>
        /// <returns></returns>
        public TblAbbredemption GetAbbOrderDetails(string regdNo)
        {
            TblAbbredemption? tblAbbredemption = new TblAbbredemption();
            try
            {
                if (!string.IsNullOrEmpty(regdNo))
                {
                    tblAbbredemption = _context.TblAbbredemptions
                                    .Include(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                                    .Include(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                                    .Include(x => x.Status)
                                    .Include(x => x.CustomerDetails)
                                    .Include(x => x.Abbregistration).ThenInclude(x => x.BusinessUnit)
                                    .Include(x => x.Abbregistration).ThenInclude(x => x.BusinessPartner)                                  
                                    .Include(x => x.Abbregistration).ThenInclude(x => x.ModelNumber)
                                    .Where(x => x.IsActive == true && x.RegdNo.ToLower().Contains(regdNo)).FirstOrDefault();
                    if (tblAbbredemption != null)
                    {
                        return tblAbbredemption;
                    }
                    else
                    {
                        return tblAbbredemption = new TblAbbredemption();
                    }
                }

            }
            catch (Exception ex)
            {
                throw;
            }
            return tblAbbredemption;
        }
        #endregion

        #region get ABB Redemption order details by regdno
        /// <summary>
        /// get ABB Redemption order details by regdno
        /// </summary>
        /// <param name="regdNo"></param>
        /// <returns></returns>
        public TblAbbredemption GetAbbOrderDetailsByOrderTransId(int orderTransId)
        {
            TblAbbredemption? tblAbbredemption = null;
            TblOrderTran? tblOrderTran = null;
            try
            {
                if (orderTransId > 0)
                {
                    tblOrderTran = _context.TblOrderTrans.Where(x => x.IsActive == true && x.OrderTransId == orderTransId).FirstOrDefault();
                    if (tblOrderTran != null)
                    {
                        tblAbbredemption = _context.TblAbbredemptions
                                        .Include(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                                        .Include(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                                        .Include(x => x.Status)
                                        .Include(x => x.CustomerDetails)
                                        .Include(x => x.Abbregistration).ThenInclude(x => x.BusinessUnit)
                                        .Include(x => x.Abbregistration).ThenInclude(x => x.BusinessPartner)
                                        .Include(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                                        .Include(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                                        //.Include(x => x.Abbregistration).ThenInclude(x => x.NewBrand)
                                        .Where(x => x.IsActive == true && x.RedemptionId==tblOrderTran.AbbredemptionId && x.RegdNo.ToLower() == tblOrderTran.RegdNo.ToLower()).FirstOrDefault();
                        if (tblAbbredemption != null)
                        {
                            return tblAbbredemption;
                        }
                        else
                        {
                            return tblAbbredemption = new TblAbbredemption();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return tblAbbredemption = new TblAbbredemption();

        }
        #endregion

        #region Update StatusId On TblAbbRedemption and TblAbbRegistration
        /// <summary>
        /// Update StatusId On TblAbbRedemption and TblAbbRegistration
        /// </summary>
        /// <param name="regdNo"></param>
        /// <param name="statusId"></param>
        /// <param name="loggedInUserId"></param>
        /// <returns></returns>
        public TblAbbredemption UpdateABBOrderStatus(string regdNo, int statusId, int? loggedInUserId, string statusDesc = null)
        {
            TblAbbredemption tblAbbredemption = null;
            tblAbbredemption = _context.TblAbbredemptions
                .Where(x => x.IsActive == true && x.RegdNo == regdNo).FirstOrDefault();
            if (tblAbbredemption != null)
            {
                if (!string.IsNullOrWhiteSpace(statusDesc))
                {
                    tblAbbredemption.AbbredemptionStatus = statusDesc;
                }
                tblAbbredemption.StatusId = statusId;
                tblAbbredemption.ModifiedBy = loggedInUserId;
                tblAbbredemption.ModifiedDate = DateTime.Now;
                _context.Update(tblAbbredemption);
                _context.SaveChanges();
            }
            return tblAbbredemption;
        }
        #endregion
    }
}
