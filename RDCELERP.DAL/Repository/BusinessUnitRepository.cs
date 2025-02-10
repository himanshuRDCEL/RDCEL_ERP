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
    public class BusinessUnitRepository : AbstractRepository<TblBusinessUnit>, IBusinessUnitRepository
    {
        Digi2l_DevContext _context;
        public BusinessUnitRepository(Digi2l_DevContext dbContext)
      : base(dbContext)
        {
            _context= dbContext;
        }

        public TblBusinessUnit GetBusinessunitDetails(int buid)
        {
            TblBusinessUnit? BusinessUnit = null;
            if(buid>0)
            {
                BusinessUnit = _context.TblBusinessUnits.FirstOrDefault(x => x.BusinessUnitId==buid && x.IsActive==true);
            }
            return BusinessUnit;
        }

        public TblBusinessUnit Getbyid(int? id)
        {
            TblBusinessUnit BusinessUnit = null;
            if (id > 0)
            {
                BusinessUnit = _context.TblBusinessUnits.FirstOrDefault(x => x.BusinessUnitId == id && x.IsActive == true);
            }
            return BusinessUnit;
        }

        public List<TblBusinessUnit> GetSponsorList()
        {
            List<TblBusinessUnit> SponsorList = null;
            SponsorList=_context.TblBusinessUnits.Where(x=>x.IsActive == true).ToList();

            return SponsorList;
        }

        public int UpdateBusinessUnit(int? businessUnitId, decimal? DtoC, decimal? DtoD, int? userid)
        {
            TblBusinessUnit TblBusinessUnit = _context.TblBusinessUnits.FirstOrDefault(x => x.IsActive == true && x.BusinessUnitId == businessUnitId);

            TblBusinessUnit.BusinessUnitId = (int)businessUnitId;  //(int)OrderStatusEnum.Posted;
            TblBusinessUnit.ModifiedBy = userid;
            TblBusinessUnit.SweetnerForDtc = DtoC;
            TblBusinessUnit.SweetnerForDtd = DtoD;
            TblBusinessUnit.ModifiedDate = DateTime.Now;
            _context.Update(TblBusinessUnit);
            _context.SaveChanges();
            return TblBusinessUnit.BusinessUnitId;
        }

        #region Method to get BusinessUnitDetails by Order Trans Id 
        public TblBusinessUnit GetBUDetailsByTransId(int transId)
        {
            TblOrderTran? TblOrderTran = null;
            TblBusinessUnit? tblBusinessUnit = null;
            if (transId > 0)
            {
                TblOrderTran = _context.TblOrderTrans
                         .Include(x => x.Exchange).ThenInclude(x => x.BusinessPartner).ThenInclude(x => x.BusinessUnit)
                         .Include(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.BusinessUnit)
                         .Where(x => x.IsActive == true
                          && x.OrderTransId == transId).FirstOrDefault();
                if (TblOrderTran != null && TblOrderTran.OrderType == 17)
                {
                    tblBusinessUnit = TblOrderTran.Exchange?.BusinessPartner?.BusinessUnit;
                }
                else
                {
                    tblBusinessUnit = TblOrderTran?.Abbredemption?.Abbregistration?.BusinessUnit;
                }
            }
            return tblBusinessUnit;
        }
        #endregion

        #region Method to get Get BusinessUnit Config Details by Order Trans Id
        public TblBuconfigurationMapping GetBUConfigDetailsByTransId(int transId, string key)
        {
            TblOrderTran? TblOrderTran = null;
            TblBusinessUnit? tblBusinessUnit = null;
            TblBuconfigurationMapping? tblBuconfigMapping = null;
            if (transId > 0 && !string.IsNullOrEmpty(key))
            {
                tblBusinessUnit = GetBUDetailsByTransId(transId);
                if (tblBusinessUnit != null)
                {
                    tblBuconfigMapping = _context.TblBuconfigurationMappings
                        .Include(x => x.BusinessUnit)
                        .Include(x => x.Config)
                        .Where(x => x.IsActive == true && x.Config != null
                    && x.BusinessUnitId == tblBusinessUnit.BusinessUnitId && x.Config.Key == key).FirstOrDefault();
                }
            }
            return tblBuconfigMapping;
        }
        #endregion

        #region Method to get Get BusinessUnit Config List by Order Trans Id
        public List<TblBuconfigurationMapping> GetBUConfigListByTransId(int transId)
        {
            TblBusinessUnit? tblBusinessUnit = null;
            List<TblBuconfigurationMapping>? tblBuConfigMappingList = null;
            if (transId > 0)
            {
                tblBusinessUnit = GetBUDetailsByTransId(transId);
                if (tblBusinessUnit != null)
                {
                    tblBuConfigMappingList = _context.TblBuconfigurationMappings
                        .Include(x=>x.BusinessUnit)
                        .Include(x => x.Config)
                        .Where(x => x.IsActive == true && x.Config != null
                    && x.BusinessUnitId == tblBusinessUnit.BusinessUnitId).ToList();
                }
            }
            return tblBuConfigMappingList;
        }
        #endregion

        #region Get sponsor list for Reporting
        public List<TblBusinessUnit> GetSponsorListForReporting()
        {
            List<TblBusinessUnit>? SponsorList = null;
            try
            {
                SponsorList = _context.TblBusinessUnits.Where(x => x.IsActive == true && x.IsReportingOn == true).ToList();
            }
            catch (Exception ex)
            {

            }
            return SponsorList;
        }

        public TblBusinessUnit GetBUByName(string name)
        {
            TblBusinessUnit? tblBusinessUnit = null;
            tblBusinessUnit = _context.TblBusinessUnits.FirstOrDefault(x=>x.Name == name);
            return tblBusinessUnit;
        }
        #endregion
    }
}
