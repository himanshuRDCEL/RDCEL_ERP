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
    public interface IServicePartnerRepository : IAbstractRepository<TblServicePartner>
    {
        public IList<TblServicePartner> GetSelectedServicePartner(string term);

        #region get details of service partner by userid
        /// <summary>
        /// get details of service partner by userid
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public TblServicePartner GetServicePartnerByUserId(int userId);
        #endregion

        #region get details of service partner by servicePartnerId
        /// <summary>
        /// get details of service partner by servicePartnerId
        /// </summary>
        /// <param name="servicePartnerId"></param>
        /// <returns></returns>
        public TblServicePartner GetServicePartnerById(int servicePartnerId);
        #endregion

        #region Get Service Partner List for Auto complete dropdown
        /// <summary>
        /// Get Service Partner List for Auto complete dropdown
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        public List<TblServicePartner> GetSPListByBusinessName(string? term);
        #endregion
    }
}
