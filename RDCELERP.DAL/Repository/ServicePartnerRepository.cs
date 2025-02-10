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
    public class ServicePartnerRepository : AbstractRepository<TblServicePartner>, IServicePartnerRepository
    {
        private readonly Digi2l_DevContext _context;
        public ServicePartnerRepository(Digi2l_DevContext dbContext)
         : base(dbContext)
        {
            _context = dbContext;
        }

        public IList<TblServicePartner> GetSelectedServicePartner(string term)
        {
            List<TblServicePartner> tblServicePartner = new List<TblServicePartner>();

            tblServicePartner = _context.TblServicePartners.Where(x => x.IsActive == true && x.ServicePartnerIsApprovrd == true && x.IsServicePartnerLocal != null && x.ServicePartnerName.Contains(term)).ToList();
            return tblServicePartner;
        }

        #region get details of service partner by userid
        public TblServicePartner GetServicePartnerByUserId(int userId)
        {
            TblServicePartner? tblServicePartner = null;
            if (userId > 0)
            {
                tblServicePartner = _context.TblServicePartners.Where(x => x.IsActive == true && x.UserId == userId).FirstOrDefault();
            }
            return tblServicePartner;
        }
        #endregion

        #region get details of service partner by servicePartnerId
        public TblServicePartner GetServicePartnerById(int servicePartnerId)
        {
            TblServicePartner? tblServicePartner = null;
            if (servicePartnerId > 0)
            {
                tblServicePartner = _context.TblServicePartners.Where(x => x.IsActive == true && x.ServicePartnerId == servicePartnerId).FirstOrDefault();
            }
            return tblServicePartner;
        }
        #endregion

        #region Get Service Partner List for Auto complete dropdown
        public List<TblServicePartner> GetSPListByBusinessName(string? term)
        {
            List<TblServicePartner>? tblServicePartnerList = null;
            tblServicePartnerList = _context.TblServicePartners.Where(x => x.IsActive == true
            && x.ServicePartnerIsApprovrd == true && x.IsServicePartnerLocal != null
            && (term == "#" || (x.ServicePartnerBusinessName??"").Contains(term))).ToList();
            if (tblServicePartnerList == null)
            {
                tblServicePartnerList = new List<TblServicePartner>();
            }
            return tblServicePartnerList;
        }
        #endregion
    }
}
