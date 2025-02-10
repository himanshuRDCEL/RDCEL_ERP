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
    public interface IBusinessPartnerRepository : IAbstractRepository<TblBusinessPartner>
    {
        TblBusinessPartner GetBPId(int? Id);
        TblBusinessPartner GetSingleOrder(int businessPartnerId);

        List<TblBusinessPartner> GetAllBusinessPartner(string AssociteCode);

        TblBusinessPartner GetDealerData(string AssociteCode);

        List<TblBusinessPartner> GetCityByState(string state, int buid);
        List<TblBusinessPartner> GetCityByStateandAssociate(string state, int buid,string AsociateCode);
        List<TblBusinessPartner> GetStoreList(string City, string AssociteCode, int buid);
        List<TblBusinessPartner> GetStateList( int buid);
        List<TblBusinessPartner> GetStateListDelaer( int buid,string CompanyName,string AssociateCode);
        List<TblBusinessPartner> GetStoreListForInternalUser(string city, int buid);
        public TblBusinessPartner GetStoreDetails(string StoreCode);

        #region Get Business Partner Details by Id
        /// <summary>
        /// Get Business Partner Details by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public TblBusinessPartner? GetById(int? Id);
        #endregion
    }
}
