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
    public class DriverDetailsRepository : AbstractRepository<TblDriverDetail>,IDriverDetailsRepository
    {
        private readonly Digi2l_DevContext _context;
        public DriverDetailsRepository(Digi2l_DevContext dbContext)
         : base(dbContext)
        {
            _context = dbContext;
        }

        #region Get Driver Details By DriverDetailsId
        public TblDriverDetail GetDriverDetailsById(int driverDetailsId)
        {
            TblDriverDetail? tblDriverDetail = null;
            if (driverDetailsId > 0)
            {
                tblDriverDetail = _context.TblDriverDetails
                    .Include(x=>x.Driver).ThenInclude(x=>x.City)
                    .Include(x => x.Driver).ThenInclude(x => x.ServicePartner)
                    .Include(x=>x.Vehicle).ThenInclude(x => x.City)
                    .Include(x=>x.Driver)
                    .Where(x => x.IsActive == true && x.DriverDetailsId == driverDetailsId).FirstOrDefault();
            }
            return tblDriverDetail;
        }
        #endregion

        #region Get Driver Details List By ServicePartnerId
        public TblDriverDetail GetDriverDetailsByUserId(int userId)
        {
            TblDriverDetail? tblDriverDetail = null;
            if (userId > 0)
            {
                tblDriverDetail = _context.TblDriverDetails.Where(x => x.IsActive == true && x.UserId == userId).FirstOrDefault();
            }
            return tblDriverDetail;
        }
        #endregion

        #region Get Details of service partner by servicePartnerId
        public List<TblDriverDetail> GetDriverDetailsBySPId(int servicePartnerId)
        {
            List<TblDriverDetail>? tblDriverDetailsList = null;
            if (servicePartnerId > 0)
            {
                tblDriverDetailsList = _context.TblDriverDetails.Where(x => x.IsActive == true && x.ServicePartnerId == servicePartnerId).ToList();
            }
            return tblDriverDetailsList;
        }
        #endregion

        //#region get details of service partner by servicePartnerId
        //public List<TblDriverDetail> GetDriverDetailsExist(string? phone)
        //{
        //    List<TblDriverDetail>? tblDriverDetailsList = null;
        //    if (!string.IsNullOrEmpty(phone))
        //    {
        //        phone = phone.ToLower().Trim();
        //    }
        //    else { phone = null; }

        //    if (phone != null)
        //    {
        //        tblDriverDetailsList = _context.TblDriverDetails.Where(x => x.IsActive == true && (x.DriverPhoneNumber??"").Contains(phone)).ToList();
        //    }
        //    return tblDriverDetailsList;
        //}
        //#endregion

        #region Get All Drivers Details List
        public List<TblDriverDetail> GetAllDriverDetailsList()
        {
            List<TblDriverDetail>? tblDriverDetailsList = null;
            tblDriverDetailsList = _context.TblDriverDetails.Where(x => x.IsActive == true).ToList();
            return tblDriverDetailsList;
        }
        #endregion
    }
}
