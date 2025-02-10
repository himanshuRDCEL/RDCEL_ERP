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
    public interface IBusinessUnitRepository : IAbstractRepository<TblBusinessUnit>
    {

        TblBusinessUnit GetBusinessunitDetails(int  buid);
        TblBusinessUnit Getbyid(int? id);
        List<TblBusinessUnit> GetSponsorList();

        public int UpdateBusinessUnit(int? businessUnitId, decimal? DtoC, decimal? DtoD, int? userid);

        #region Method to get BusinessUnitDetails by Order Trans Id 
        /// <summary>
        /// Method to get BusinessUnitDetails by Order Trans Id
        /// </summary>
        /// <param name="Id">Id</param>
        /// <returns>TblOrderTran</returns>
        public TblBusinessUnit GetBUDetailsByTransId(int transId);
        #endregion

        #region Method to get Get BusinessUnit Config Details by Order Trans Id
        /// <summary>
        /// Method to get Get BusinessUnit Config Details by Order Trans Id
        /// </summary>
        /// <param name="transId"></param>
        /// <param name="key"></param>
        /// <returns>TblBuconfiguration</returns>
        public TblBuconfigurationMapping GetBUConfigDetailsByTransId(int transId, string key);
        #endregion

        #region Method to get Get BusinessUnit Config List by Order Trans Id
        /// <summary>
        /// Method to get Get BusinessUnit Config List by Order Trans Id
        /// </summary>
        /// <param name="transId"></param>
        /// <returns>tblBuConfigList</returns>
        public List<TblBuconfigurationMapping> GetBUConfigListByTransId(int transId);
        #endregion

        #region Get sponsor list for Reporting
        /// <summary>
        /// Get sponsor list for Reporting
        /// </summary>
        /// <returns></returns>
        public List<TblBusinessUnit> GetSponsorListForReporting();
        #endregion

        #region
        public TblBusinessUnit GetBUByName(string name);

        #endregion
    }
}
