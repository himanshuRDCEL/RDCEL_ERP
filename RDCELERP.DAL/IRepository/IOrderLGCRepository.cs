using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.DAL.AbstractRepository;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.Repository;

namespace RDCELERP.DAL.IRepository
{
    public interface IOrderLGCRepository : IAbstractRepository<TblOrderLgc>
    {
        public TblOrderLgc GetExchangeDetailsByRegdno(string Regdno);
        public List<TblOrderLgc> GetOrderLGCListByDriverIdEVCId(int? DriverId, int? EVCRegistrationId);
        public List<TblOrderLgc> GetOrderLGCListForGenerateInvoice(int? EvcRegistrationId, int? StatusId);
        public List<TblOrderLgc> GetOrderLGCListByServicePartnerId(int UserId, int StatusId);
        public List<TblOrderLgc> GetOrderLGCListByStatusId(int? StatusId1 = null, int? StatusId2 = null);
        public TblOrderLgc GetOrderLGCByOrderTransId(int orderTransId, int statusId);

        #region Get list of City & EVC by UserId and Status
        /// <summary>
        /// Get list of City & EVC by UserId and Status
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns>driverDetailsViewModel</returns>
        public List<TblOrderLgc> GetCityAndEvcList(int UserId, int statusId = 0);
        #endregion
        #region Get Order LGC list by Driver Id and EVC Partner Id
        /// <summary>
        /// Get Order LGC list by Driver Id and EVC Partner Id
        /// </summary>
        /// <param name="DriverId"></param>
        /// <param name="EVCPartnerId"></param>
        /// <returns></returns>
        public List<TblOrderLgc> GetOrderLGCListByDriverIdEVCPId(int? DriverId, int? EVCPartnerId);
        #endregion

        public TblOrderLgc GetOrderDetailsByOrderTransId(int orderTransId);
    }
}
