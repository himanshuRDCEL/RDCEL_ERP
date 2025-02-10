using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.DAL.AbstractRepository;
using RDCELERP.DAL.Entities;

namespace RDCELERP.DAL.IRepository
{
    public interface IEVCPartnerRepository : IAbstractRepository<TblEvcPartner>
    {
        public List<TblEvcPartner> GetEvcPartnerListByPincode(string? custPincode, int? productCatId, string? productQuality, int ordertype);
        public List<TblEvcPartner> GetEvcPartnerListHavingClearBalance(List<TblEvcPartner> tblEvcPartnersList, int? evcPrice);
        public List<TblEvcPartner> GetEvcPartnerListHavingOldRecharge(List<TblEvcPartner> tblEvcPartnersList);
        public List<TblEvcPartner> GetNonInHouseEvcPartnerListByPincode(string? custPincode, int? productCatId, string? productQuality, int ordertype);        
        public TblEvcPartner GetEVCPartnerDetails(int EVCPartnerId);
        public List<TblEvcPartner> GetAllEvcPartnerListByPincode(string? custPincode, int? productCatId, string? productQuality, int ordertype);
        public List<TblEvcPartner> GetEvcPartnerListbyEVC(int EVCId);
    }
}
