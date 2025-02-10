using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.DAL.AbstractRepository;
using RDCELERP.DAL.Entities;

namespace RDCELERP.DAL.IRepository
{
    public interface IEVCPODDetailsRepository : IAbstractRepository<TblEvcpoddetail>
    {
        public List<TblEvcpoddetail> GetEVCPODDetailsList();

        #region Get EVC POD Details By Id
        /// <summary>
        /// Get EVC POD Details By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TblEvcpoddetail GetEVCPODDetailsById(int? id);
        #endregion
        public TblEvcpoddetail GetEVCPODDetailsByOrderTransId(int? id);
    }
}

