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
    public class EVCPODDetailsRepository : AbstractRepository<TblEvcpoddetail>, IEVCPODDetailsRepository
    {
        private Digi2l_DevContext _dbContext;
        public EVCPODDetailsRepository(Digi2l_DevContext dbContext)
       : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public List<TblEvcpoddetail> GetEVCPODDetailsList()
        {
            List<TblEvcpoddetail> tblEvcpoddetails = null;
            
            try
            {
                tblEvcpoddetails = _dbContext.TblEvcpoddetails
                    .Include(x => x.Evc).ThenInclude(x => x.City)
                    .Where(x => x.IsActive == true && x.Evc != null && x.Evc.City != null
                    ).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
            return tblEvcpoddetails;

        }

        #region Get EVC POD Details By Id
        public TblEvcpoddetail GetEVCPODDetailsById(int? id)
        {
            TblEvcpoddetail? tblEvcpoddetails = null;

            try
            {
                if (id > 0)
                {
                    tblEvcpoddetails = _dbContext.TblEvcpoddetails
                    .Where(x => x.IsActive == true && x.Id == id
                    ).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return tblEvcpoddetails;

        }
        #endregion

        #region Get EVC POD Details By order trans Id
        public TblEvcpoddetail GetEVCPODDetailsByOrderTransId(int? id)
        {
            TblEvcpoddetail? tblEvcpoddetails = null;

            try
            {
                if (id > 0)
                {
                    tblEvcpoddetails = _dbContext.TblEvcpoddetails
                    .Where(x => x.IsActive == true && x.OrderTransId == id
                    ).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return tblEvcpoddetails;

        }
        #endregion
    }
}
