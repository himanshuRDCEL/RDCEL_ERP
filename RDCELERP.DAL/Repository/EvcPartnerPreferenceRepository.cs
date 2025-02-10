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
    public class EvcPartnerPreferenceRepository : AbstractRepository<TblEvcPartnerPreference>, IEvcPartnerPreferenceRepository
    {

        private readonly Digi2l_DevContext _context;
        public EvcPartnerPreferenceRepository(Digi2l_DevContext dbContext)
       : base(dbContext)
        {
            _context = dbContext;
        }
        //#region get order data for settelment
        ///// <summary>
        ///// get order data for settelment
        ///// </summary>
        ///// <param name="RegdNo"></param>
        ///// <returns></returns>

        //public TblEvcPartner GetEVCPartnerDetails(int EVCPartnerId)
        //{
        //    TblEvcPartner? tblEvcPartner = new TblEvcPartner();
        //    if (EVCPartnerId != null && EVCPartnerId > 0)
        //    {
        //        tblEvcPartner = _context.TblEvcPartners
        //            .Include(x => x.Evcregistration)
        //            .Include(x => x.State)
        //            .Include(x => x.City)
        //            .FirstOrDefault(x => x.EvcPartnerId == EVCPartnerId && x.IsActive == true);
        //    }
        //    return tblEvcPartner;
        //}
        //#endregion


        #region Update Datain On TblEvcPartnerPreference 
        /// <summary>
        /// Update Datain On TblEvcPartnerPreference 
        /// </summary>
        /// <param name="EVCPartnetId"></param>
        /// <param name="userid"></param>     
        /// <returns></returns>
        public int Updatedetails(int PartnerPreferenceId ,int loggedInUserId)
        {
            int result = 0;
            TblEvcPartnerPreference? tblEvcPartnerPreference = null;
            tblEvcPartnerPreference = _context.TblEvcPartnerPreferences
                .Where(x => x.EvcPartnerpreferenceId == PartnerPreferenceId ).FirstOrDefault();
            if (tblEvcPartnerPreference != null)
            {
                tblEvcPartnerPreference.IsActive = true;
                tblEvcPartnerPreference.ModifiedBy = loggedInUserId;
                tblEvcPartnerPreference.ModifiedDate = DateTime.Now;              
                _context.Update(tblEvcPartnerPreference);
                _context.SaveChanges();
                result = 1;
            }
            return result;
        }
        #endregion
    }

}
