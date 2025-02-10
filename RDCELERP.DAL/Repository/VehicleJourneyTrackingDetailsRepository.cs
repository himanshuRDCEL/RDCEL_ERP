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
    public class VehicleJourneyTrackingDetailsRepository : AbstractRepository<TblVehicleJourneyTrackingDetail>, IVehicleJourneyTrackingDetailsRepository
    {
        Digi2l_DevContext _context;
        public VehicleJourneyTrackingDetailsRepository(Digi2l_DevContext dbContext)
          : base(dbContext)
        {
            _context = dbContext;
        }



        public int UpdateVehicleJourney(int OrdertranshId, int? setStatusId, int? loggedUserId)
        {
            int result = 0;
            TblVehicleJourneyTrackingDetail tblVehicleJourneyTrackingDetail = _context.TblVehicleJourneyTrackingDetails.Where(x => x.IsActive == true && x.OrderTransId == OrdertranshId).FirstOrDefault();
            if (tblVehicleJourneyTrackingDetail != null)
            {
                tblVehicleJourneyTrackingDetail.OrderDropTime = DateTime.Now;
                tblVehicleJourneyTrackingDetail.ModifiedBy = loggedUserId;
                tblVehicleJourneyTrackingDetail.ModifiedDate = DateTime.Now;
                tblVehicleJourneyTrackingDetail.StatusId = setStatusId;
                _context.Update(tblVehicleJourneyTrackingDetail);
                _context.SaveChanges();
               

                result = 1;
            }
            return result;
        }

        public void UpdateLattLong(TblVehicleJourneyTrackingDetail journeyTrackingDetail)
        {
            _context.Update(journeyTrackingDetail);
            _context.SaveChanges();
        }

        #region update vehicle insentive details
        public int UpdateVehicleInsentiveDeatils(TblVehicleJourneyTrackingDetail journeyTrackingDetail)
        {
            int result = 0;
            _context.Update(journeyTrackingDetail);
            var flag = _context.SaveChanges();
            if (flag == 1)
            {
                result = 1;
            }
            return result;
            //TblVehicleJourneyTrackingDetail tblVehicleJourneyTrackingDetail = _context.TblVehicleJourneyTrackingDetails.Where(x => x.IsActive == true && x.OrderTransId == OrdertranshId).FirstOrDefault();
            //if (tblVehicleJourneyTrackingDetail != null)
            //{
            //    tblVehicleJourneyTrackingDetail.OrderDropTime = DateTime.Now;
            //    tblVehicleJourneyTrackingDetail.ModifiedBy = loggedUserId;
            //    tblVehicleJourneyTrackingDetail.ModifiedDate = DateTime.Now;
            //    tblVehicleJourneyTrackingDetail.StatusId = setStatusId;
            //    _context.Update(tblVehicleJourneyTrackingDetail);
            //    _context.SaveChanges();

            //    result = 1;
            //}
            //return result;
        }


        #endregion


        #region Get Order specific Earning details 
        public TblVehicleJourneyTrackingDetail GetVehicleJourneyTrackingDetail(int OrderTransId)
        {
            TblVehicleJourneyTrackingDetail? tblVehicleJourneyTrackingDetail =  new TblVehicleJourneyTrackingDetail();
            if (OrderTransId > 0)
            {
                tblVehicleJourneyTrackingDetail = _context.TblVehicleJourneyTrackingDetails.Where(x => x.IsActive == true && x.OrderTransId == OrderTransId).FirstOrDefault();
                if(tblVehicleJourneyTrackingDetail != null)
                {
                    return tblVehicleJourneyTrackingDetail;
                }
                else
                {
                    return tblVehicleJourneyTrackingDetail;
                }
            }
            else
            {
                return tblVehicleJourneyTrackingDetail;
            }
        }
        #endregion

        #region Get Order specific Earning Details by Tracking Details Id and SPID
        public TblVehicleJourneyTrackingDetail GetDriverEarningDetail(int trackingDetailsId,int spid = 0)
        {
            TblVehicleJourneyTrackingDetail? tblVehicleJourneyTrackingDetail = null;
            if (trackingDetailsId > 0)
            {
                tblVehicleJourneyTrackingDetail = _context.TblVehicleJourneyTrackingDetails
                    .Include(x => x.OrderTrans)
                    .Include(x=>x.Evc)
                    .Where(x => x.IsActive == true && x.TrackingDetailsId == trackingDetailsId && (spid == 0 || x.ServicePartnerId == spid)).FirstOrDefault();
            }
            return tblVehicleJourneyTrackingDetail;
        }
        #endregion

        #region Get Order List by Tracking id
        public List<TblVehicleJourneyTrackingDetail> GetVehicleJourneyTrackingDetailByTrackingId(int TrackingId)
        {
            List<TblVehicleJourneyTrackingDetail> tblVehicleJourneyTrackingDetail = new List<TblVehicleJourneyTrackingDetail> ();
            if(TrackingId > 0)
            {
                tblVehicleJourneyTrackingDetail = _context.TblVehicleJourneyTrackingDetails.Where(x => x.IsActive == true && x.TrackingId == TrackingId).ToList();

            }
            return tblVehicleJourneyTrackingDetail;
        }
        #endregion
    }
}
