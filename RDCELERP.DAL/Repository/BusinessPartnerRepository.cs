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
    public class BusinessPartnerRepository : AbstractRepository<TblBusinessPartner>, IBusinessPartnerRepository
    {
        Digi2l_DevContext _context;
        public BusinessPartnerRepository(Digi2l_DevContext dbContext)
         : base(dbContext)
        {
            _context = dbContext;
        }

        public TblBusinessPartner GetSingleOrder(int businessPartnerId)
        {
            TblBusinessPartner TblBusinessPartner = _context.TblBusinessPartners.FirstOrDefault(x => x.IsActive == true && x.BusinessPartnerId == businessPartnerId);
            return TblBusinessPartner;
        }


        public TblBusinessPartner GetBPId(int? Id)
        {
            TblBusinessPartner TblBusinessPartner = _context.TblBusinessPartners.FirstOrDefault(x => x.IsActive == true && x.BusinessPartnerId == Id);
            return TblBusinessPartner;
        }

        public List<TblBusinessPartner> GetAllBusinessPartner(string AssociteCode)
        {
            List<TblBusinessPartner> businesspartnerList = null;
            if (AssociteCode != null)
            {
                businesspartnerList = _context.TblBusinessPartners.Where(x => x.AssociateCode == AssociteCode && x.IsActive == true && x.IsExchangeBp == true).ToList();
            }
            return businesspartnerList;
        }

        public TblBusinessPartner GetDealerData(string AssociteCode)
        {
            TblBusinessPartner businesspartnerobj = null;

            if (AssociteCode != null)
            {
                businesspartnerobj = _context.TblBusinessPartners.FirstOrDefault(x => x.AssociateCode == AssociteCode && x.IsActive == true && x.IsExchangeBp == true);
            }
            return businesspartnerobj;
        }

        public List<TblBusinessPartner> GetCityByState(string state, int buid)
        {
            List<TblBusinessPartner> citylist = null;
            if (state != null && buid > 0)
            {
                citylist = _context.TblBusinessPartners.Where(x => x.State != null && x.State == state && x.BusinessUnitId == buid && x.IsActive == true && x.IsExchangeBp == true).ToList();
            }
            else if (state != null)
            {
                citylist = _context.TblBusinessPartners.Where(x => x.State != null && x.State == state && x.IsActive == true && x.IsExchangeBp == true).ToList();
            }
            return citylist;
        }

        public List<TblBusinessPartner> GetStoreList(string City, string AssociteCode, int buid)
        {
            List<TblBusinessPartner> storeList = null;
            if (City != null && AssociteCode != null && buid > 0)
            {
                storeList = _context.TblBusinessPartners.Where(x => x.BusinessUnitId == buid && x.AssociateCode == AssociteCode && x.IsActive == true && x.IsExchangeBp == true && x.City != null && x.City.ToLower() == City.ToLower()).ToList();
            }
            return storeList;
        }

        public List<TblBusinessPartner> GetStateList(int buid)
        {
            List<TblBusinessPartner> stateList = null;
            if (buid != null && buid > 0)
            {
                stateList = _context.TblBusinessPartners.Where(x => x.IsActive == true && x.State != null && x.BusinessUnitId == buid).ToList();
            }
            else
            {
                stateList = _context.TblBusinessPartners.Where(x => x.IsActive == true && x.State != null).ToList();
            }
            return stateList;
        }

        public List<TblBusinessPartner> GetStoreListForInternalUser(string city, int buid)
        {
            List<TblBusinessPartner> storeList = null;
            if (buid != null && buid > 0)
            {
                storeList = _context.TblBusinessPartners.Where(x => x.IsActive == true && x.City != null && x.City == city && x.BusinessUnitId == buid && x.IsExchangeBp == true).ToList();
            }
            else
            {
                storeList = _context.TblBusinessPartners.Where(x => x.IsActive == true && x.City != null && x.City == city && x.IsExchangeBp == true).ToList();
            }
            return storeList;
        }


        public List<TblBusinessPartner> GetCityByStateandAssociate(string state, int buid, string AsociateCode)
        {
            List<TblBusinessPartner> cityList = null;
            if (state != null && buid > 0 && AsociateCode != null)
            {
                cityList = _context.TblBusinessPartners.Where(x => x.State != null && x.State == state && x.BusinessUnitId == buid && x.AssociateCode != null && x.AssociateCode == AsociateCode && x.IsActive == true && x.IsExchangeBp == true).ToList();
            }
            return cityList;
        }
        public List<TblBusinessPartner> GetStateListDelaer(int buid, string CompanyName, string AssociateCode)
        {
            List<TblBusinessPartner> stateList = null;
            if (buid > 0 && !string.IsNullOrEmpty(CompanyName) && !string.IsNullOrEmpty(AssociateCode))
            {
                stateList = _context.TblBusinessPartners.Where(x => x.BusinessUnitId == buid && x.AssociateCode == AssociateCode && x.IsActive==true && x.IsExchangeBp==true).ToList();
            }
            return stateList;
        }

        /// <summary>
        ///  Method to get Store Details 
        /// </summary>
        /// <param name="StoreCode"></param>
        /// <returns></returns>
        public TblBusinessPartner GetStoreDetails(string StoreCode)
        {
            TblBusinessPartner? tblBusinessPartner = null;
            if (!string.IsNullOrEmpty(StoreCode))
            {
                tblBusinessPartner = _context.TblBusinessPartners.FirstOrDefault(x => x.IsActive == true && x.IsExchangeBp == true && x.StoreCode == StoreCode);
            }
            return tblBusinessPartner;
        }

        #region Get Business Partner Details by Id
        /// <summary>
        /// Get Business Partner Details by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public TblBusinessPartner? GetById(int? Id)
        {
            TblBusinessPartner? tblBusinessPartner = null;
            if (Id > 0)
            {
                tblBusinessPartner = _context.TblBusinessPartners
                    .Where(x => x.IsActive == true && x.BusinessPartnerId == Id).FirstOrDefault();
            }
            return tblBusinessPartner;
        }
        #endregion
    }

}