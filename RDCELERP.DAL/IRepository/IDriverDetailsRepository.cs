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
    public interface IDriverDetailsRepository : IAbstractRepository<TblDriverDetail>
    {
        #region Get Driver Details By DriverDetailsId
        /// <summary>
        /// Get Driver Details By DriverDetailsId
        /// </summary>
        /// <param name="driverDetailsId"></param>
        /// <returns></returns>
        public TblDriverDetail GetDriverDetailsById(int driverDetailsId);
        #endregion

        #region Get Driver Details List By ServicePartnerId
        /// <summary>
        /// Get Driver Details List By ServicePartnerId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public TblDriverDetail GetDriverDetailsByUserId(int userId);
        #endregion

        #region get details of service partner by servicePartnerId
        public List<TblDriverDetail> GetDriverDetailsBySPId(int servicePartnerId);
        #endregion

        #region Get All Drivers Details List
        public List<TblDriverDetail> GetAllDriverDetailsList();
        #endregion
    }
}
