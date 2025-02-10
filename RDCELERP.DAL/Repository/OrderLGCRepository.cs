using Microsoft.AspNetCore.Mvc.Rendering;
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
    public class OrderLGCRepository : AbstractRepository<TblOrderLgc>, IOrderLGCRepository
    {
        private Digi2l_DevContext _dbContext;
        IErrorLogRepository _errorLogRepository;
        public OrderLGCRepository(Digi2l_DevContext dbContext, IErrorLogRepository errorLogRepository)
         : base(dbContext)
        {
            _dbContext = dbContext;
            _errorLogRepository = errorLogRepository;
        }

        public TblOrderLgc GetExchangeDetailsByRegdno(string Regdno)
        {
            TblOrderLgc? TblOrderLgc = null;
            try
            {
                TblOrderLgc = _dbContext.TblOrderLgcs
                    .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
                    .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.CustomerDetails)
                    .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.Brand)
                    .Where(x => x.IsActive == true && x.OrderTrans.Exchange.RegdNo == Regdno).FirstOrDefault();
             }
            catch (Exception ex)
            {
                throw;
            }
            return TblOrderLgc;
        }
        public List<TblOrderLgc> GetOrderLGCListByDriverIdEVCId(int? DriverId, int? EVCRegistrationId)
        {
            List<TblOrderLgc> tblOrderLgcList = null;
            try
            {
                if (DriverId != null && EVCRegistrationId != null)
                {
                    tblOrderLgcList = _dbContext.TblOrderLgcs
                        .Include(x => x.Evcpod)
                        .Include(x => x.Logistic).ThenInclude(x=>x.ServicePartner).Include(x=>x.Evcregistration)
                        .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
                        .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x=> x.CustomerDetails)
                        .Include(x => x.OrderTrans).ThenInclude(x => x.TblWalletTransactions)
                        .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
                        .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                        .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                        .Where(x => x.IsActive == true && x.DriverDetailsId == DriverId && x.EvcregistrationId == EVCRegistrationId
                        && x.Logistic != null && x.Logistic.ServicePartner != null && x.Evcregistration !=null && x.OrderTrans != null
                        && (x.OrderTrans.Exchange != null || x.OrderTrans.Abbredemption != null) && x.OrderTrans.TblWalletTransactions != null
                        ).ToList();
                }
                if (tblOrderLgcList == null)
                {
                    tblOrderLgcList = new List<TblOrderLgc>();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return tblOrderLgcList;
        }

        public List<TblOrderLgc> GetOrderLGCListForGenerateInvoice(int? EvcRegistrationId, int? StatusId)
        {
            List<TblOrderLgc> tblOrderLgcList = null;
            try
            {
                tblOrderLgcList = _dbContext.TblOrderLgcs
                        .Include(x => x.Logistic).Include(x => x.Evcpod)
                        .Include(x => x.Evcregistration).ThenInclude(x => x.City)
                        .Include(x => x.Evcregistration).ThenInclude(x => x.City).ThenInclude(x => x.State)
                        .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange)
                        .Include(x => x.OrderTrans).ThenInclude(x => x.TblWalletTransactions)
                        .Where(x => x.IsActive == true && x.IsInvoiceGenerated != true && x.StatusId == StatusId && x.Logistic != null
                        && x.Evcpod != null && x.Evcregistration != null && x.OrderTrans != null && x.OrderTrans.Exchange != null
                        && x.OrderTrans.TblWalletTransactions != null && x.OrderTrans.TblWalletTransactions.Count > 0
                        && (EvcRegistrationId == null ? true : x.EvcregistrationId == EvcRegistrationId)
                        ).ToList();

                if (tblOrderLgcList == null)
                {
                    tblOrderLgcList = new List<TblOrderLgc>();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return tblOrderLgcList;
        }

        public List<TblOrderLgc> GetOrderLGCListByServicePartnerId(int UserId, int StatusId)
        {
            List<TblOrderLgc> tblOrderLgcList = null;
            List<SelectListItem> CityList = new List<SelectListItem>();

            try
            {
                tblOrderLgcList = _dbContext.TblOrderLgcs
                    .Include(x => x.Logistic).ThenInclude(x => x.ServicePartner)
                    .Include(x => x.Evcregistration).ThenInclude(x => x.City)
                    .Where(x => x.IsActive == true && x.Logistic.ServicePartner.UserId == UserId 
                    && x.StatusId == Convert.ToInt32(StatusId) && x.DriverDetailsId == null
                    && x.Logistic != null && x.Logistic.ServicePartner != null
                    && x.Evcregistration != null && x.Evcregistration.City != null
                    ).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
            return tblOrderLgcList;

        }

        public List<TblOrderLgc> GetOrderLGCListByStatusId(int? StatusId1 = null, int? StatusId2=null)
        {
            List<TblOrderLgc> tblOrderLgcList = null;
            List<SelectListItem> CityList = new List<SelectListItem>();

            try
            {
                tblOrderLgcList = _dbContext.TblOrderLgcs
                    .Include(x => x.Evcregistration).ThenInclude(x => x.City)
                    .Where(x => x.IsActive == true && ((StatusId1 == null ? true : x.StatusId == Convert.ToInt32(StatusId1))
                    || (StatusId2 == null ? true : x.StatusId == Convert.ToInt32(StatusId2)))
                    && x.Evcregistration != null && x.Evcregistration.City != null
                    ).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
            return tblOrderLgcList;

        }
        public TblOrderLgc GetOrderLGCByOrderTransId(int orderTransId, int statusId)
        {
            TblOrderLgc TblOrderLgc = null;
            try
            {
                TblOrderLgc = _dbContext.TblOrderLgcs
                    .Where(x => x.IsActive == true && x.OrderTransId == orderTransId
                    && x.StatusId == statusId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw;
            }
            return TblOrderLgc;
        }

        #region Get list of city & EVC with respect to UserId
        public List<TblOrderLgc> GetCityAndEvcList(int UserId, int statusId = 0)
        {
            List<TblOrderLgc>? tblOrderLgcList = null;
            List<SelectListItem> CityList = new List<SelectListItem>();

            try
            {
                tblOrderLgcList = _dbContext.TblOrderLgcs
                        .Include(x => x.Logistic).ThenInclude(x => x.ServicePartner)
                        .Include(x => x.Evcregistration).ThenInclude(x => x.City)
                        .Include(x => x.Evcpartner).ThenInclude(x => x.City)
                        .Include(x => x.Evcpartner).ThenInclude(x => x.Evcregistration)
                        .Where(x => x.IsActive == true && x.Logistic != null && x.Logistic.ServicePartner != null
                        && x.Logistic.ServicePartner.UserId == UserId && x.StatusId == statusId && x.DriverDetailsId == null).ToList();
            }
            catch (Exception ex)
            {
                _errorLogRepository.WriteErrorToDB("LogisticManager", "GetCityAndEvcList", ex);
            }
            return tblOrderLgcList;

        }
        #endregion

        #region Get Order LGC list by Driver Id and EVC Partner Id
        public List<TblOrderLgc> GetOrderLGCListByDriverIdEVCPId(int? DriverId, int? EVCPartnerId)
        {
            List<TblOrderLgc> tblOrderLgcList = null;
            try
            {
                if (DriverId != null && EVCPartnerId != null)
                {
                    tblOrderLgcList = _dbContext.TblOrderLgcs
                        .Include(x => x.Evcpod)
                        .Include(x => x.Logistic).ThenInclude(x => x.ServicePartner).Include(x => x.Evcregistration)
                        .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
                        .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.CustomerDetails)
                        .Include(x => x.OrderTrans).ThenInclude(x => x.TblWalletTransactions)
                        .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
                        .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                        .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                        .Include(x => x.Evcpartner)
                        .Where(x => x.IsActive == true && x.DriverDetailsId == DriverId && x.EvcpartnerId == EVCPartnerId
                        && x.Logistic != null && x.Logistic.ServicePartner != null && x.Evcregistration != null && x.OrderTrans != null
                        && (x.OrderTrans.Exchange != null || x.OrderTrans.Abbredemption != null) && x.OrderTrans.TblWalletTransactions != null
                        ).ToList();
                }
                if (tblOrderLgcList == null)
                {
                    tblOrderLgcList = new List<TblOrderLgc>();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return tblOrderLgcList;
        }
        #endregion


        public TblOrderLgc GetOrderDetailsByOrderTransId(int orderTransId)
        {
            TblOrderLgc TblOrderLgc = null;
            try
            {
                TblOrderLgc = _dbContext.TblOrderLgcs
                    .Where(x => x.IsActive == true && x.OrderTransId == orderTransId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw;
            }
            return TblOrderLgc;
        }
    }
}
