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
    public class LogisticsRepository : AbstractRepository<TblLogistic>, ILogisticsRepository
    {
        private Digi2l_DevContext _DbContext;
        public LogisticsRepository(Digi2l_DevContext dbContext)
        : base(dbContext)
        {
            _DbContext = dbContext;

        }
        public TblLogistic GetExchangeDetailsByRegdno(string Regdno)
        {
            TblLogistic? tblLogistic = null;
            try
            {
                tblLogistic = _DbContext.TblLogistics.Where(x => x.IsActive == true && x.RegdNo.ToLower().Equals(Regdno.ToLower()))
                    .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
                    .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.CustomerDetails)
                    .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.Brand)
                     .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
.Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
.Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
.Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.BusinessUnit)
.Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration)
                    .FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw;
            }
            return tblLogistic;
        }


        public TblLogistic GetExchangeDetailsByOID(int OrdertransId)
        {
            TblLogistic? tblLogistic = null;
            try
            {
                tblLogistic = _DbContext.TblLogistics.Where(x => x.IsActive == true && x.OrderTransId == OrdertransId)
                    .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
                    .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.CustomerDetails)
                    .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.Brand)
                     .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
.Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
.Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
.Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.BusinessUnit)
.Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration)
                    .FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw;
            }
            return tblLogistic;
        }
        public TblWalletTransaction GetEvcDetailsByRegdno(string Regdno)
        {
            TblWalletTransaction? tblWalletTransaction = null;
            try
            {
                if (!string.IsNullOrEmpty(Regdno))
                {
                    tblWalletTransaction = _DbContext.TblWalletTransactions.Where(x => x.IsActive == true
                    && (x.RegdNo ?? "").ToLower().Equals(Regdno.ToLower()))
                    .Include(x => x.Evcpartner).ThenInclude(x => x.City).ThenInclude(x => x.State)
                    .Include(x => x.Evcregistration).ThenInclude(x => x.City).ThenInclude(x => x.State)
                    .FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return tblWalletTransaction;
        }
        public int GetDriverAssignOrderCount(int DriverDetailsId, int status, int LgcId)
        {
            List<TblLogistic> tblLogistic = new List<TblLogistic>();
            tblLogistic = _DbContext.TblLogistics.Where(x => x.IsActive == true && x.DriverDetailsId == DriverDetailsId && (x.StatusId == status || x.StatusId == 23 || x.StatusId == 54) && x.ServicePartnerId == LgcId).ToList();
            if (tblLogistic.Count > 0)
            {
                return tblLogistic.Count;
            }
            return 0;
        }
        public TblLogistic GetExchangeDetailsByOrdertransId(int OrdertransId)
        {
            TblLogistic? tblLogistic = null;
            try
            {
                tblLogistic = _DbContext.TblLogistics.Where(x => x.IsActive == true && x.OrderTransId == OrdertransId)
                    .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
                    .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.CustomerDetails)
                     .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                    .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                    .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
                    .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.BusinessUnit)
                    .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration)
                    .Include(x => x.ServicePartner)
                    .FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw;
            }
            return tblLogistic;
        }
        public TblWalletTransaction GetEvcDetailsByOrdertranshId(int OrdertransId)
        {
            TblWalletTransaction? tblWalletTransaction = null;
            try
            {
                tblWalletTransaction = _DbContext.TblWalletTransactions.Where(x => x.IsActive == true && x.OrderTransId == OrdertransId)
                                   .Include(x => x.Evcregistration).ThenInclude(x => x.City).ThenInclude(x => x.State)
                                   .Include(x => x.Evcpartner).ThenInclude(x => x.City)
                                   .Include(x => x.Evcpartner).ThenInclude(x => x.State)
                                   .FirstOrDefault();
            }
            catch (Exception ex)
            {

                throw;
            }
            return tblWalletTransaction;
        }
        public int UpdateLogiticStatus(TblLogistic tblLogistic)
        {
            TblLogistic? tblLogistic1 = new TblLogistic();
            tblLogistic1 = _DbContext.TblLogistics.Where(x => x.IsActive == true && x.OrderTransId == tblLogistic.OrderTransId).FirstOrDefault();
            int result = 0;
            if (tblLogistic1 != null)
            {
                tblLogistic1.DriverDetailsId = tblLogistic.DriverDetailsId;
                tblLogistic1.ModifiedDate = DateTime.Now;
                tblLogistic1.Modifiedby = tblLogistic.Modifiedby;
                tblLogistic1.StatusId = tblLogistic.StatusId;
                _DbContext.Update(tblLogistic1);
                _DbContext.SaveChanges();
                result = 1;
            }
            return result;
        }
        public TblLogistic GetAbbRedumptionDetailsByRegdno(string Regdno)
        {
            TblLogistic? tblLogistic = null;
            try
            {
                tblLogistic = _DbContext.TblLogistics.Where(x => x.IsActive == true && x.RegdNo.ToLower().Equals(Regdno.ToLower()))
                    .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                    .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                    .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
                    .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.BusinessUnit)
                    .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration)
                    .FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw;
            }
            return tblLogistic;
        }

        public List<TblLogistic> GetOrderListByStatus(int? status)
        {
            List<TblLogistic>? tblLogistic = new List<TblLogistic>();
            try
            {
                if (status == 0)
                {
                    status = null;
                }
                tblLogistic = _DbContext.TblLogistics.Where(x => x.IsActive == true && (status == null || (x.StatusId != null && x.StatusId == status)))
                    .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
                    .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.CustomerDetails)
                    .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.Brand)
                    .Include(x => x.Status)
                    .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                    .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                    .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
                    .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.BusinessUnit)
                    .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
            return tblLogistic;
        }

        //#region Get Orders List for Auto complete dropdown
        //public List<TblLogistic> GetUnAssignedOrderListBySPId(int? SPId)
        //{
        //    List<TblLogistic>? tblLogistics = null;
        //    tblLogistics = _DbContext.TblLogistics
        //        .Include(x=>x.OrderTrans).ThenInclude(x=>x.Exchange).ThenInclude(x => x.Exchange)
        //        .Where(x => x.IsActive == true
        //    && x.ServicePartnerIsApprovrd == true && x.IsServicePartnerLocal != null
        //    && (term == "#" || (x.ServicePartnerBusinessName ?? "").Contains(term))).ToList();
        //    if (tblServicePartnerList == null)
        //    {
        //        tblServicePartnerList = new List<TblServicePartner>();
        //    }
        //    return tblServicePartnerList;
        //}
        //#endregion

        public List<TblLogistic> GetOrderListBySPIdAndStatus(int? servicePartnerId, int? status1 = null, int? status2 = null)
        {
            List<TblLogistic>? tblLogistic = new List<TblLogistic>();
            try
            {
                if (servicePartnerId > 0)
                {
                    tblLogistic = _DbContext.TblLogistics.Where(x => x.IsActive == true && x.ServicePartnerId == servicePartnerId
                    && (x.StatusId == status1 || x.StatusId == status2))
                    .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
                    .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.CustomerDetails)
                    .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.Brand)
                    .Include(x => x.Status)
                    .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                    .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                    .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
                    .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.BusinessUnit)
                    .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration)
                    .ToList();
                }
                else
                {
                    tblLogistic = _DbContext.TblLogistics.Where(x => x.IsActive == true && (x.StatusId == status1 || x.StatusId == status2))
                    .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
                    .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.CustomerDetails)
                    .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.Brand)
                    .Include(x => x.Status)
                    .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                    .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                    .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
                    .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.BusinessUnit)
                    .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration)
                    .ToList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return tblLogistic;
        }
    
    
    }
}