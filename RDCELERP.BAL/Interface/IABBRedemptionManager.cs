using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.ABBPlanMaster;
using RDCELERP.Model.ABBRedemption;

namespace RDCELERP.BAL.Interface
{
    public interface IABBRedemptionManager
    {
        int SaveABBRedemptionDetails(ABBRedemptionViewModel abbRedemptionViewModel);
        public ABBRedemptionViewModel GetABBRedemptionById(int id);
        public ABBRedemptionViewModel GetAllABBRedemptionDetails();
        public ABBRedemptionViewModel GetABBDetailsByRegdNo(string regdno);
        public List<TblAbbregistration> GetAutoCompleteRegdNo(string regdNum);
        public List<TblLoV> GetRedemptionPeriod();
        public List<TblLoV> GetRedemptionPercentage();
        public ABBRedemptionViewModel GetABBRedemptionByRegdNo(string rno);
        public List<TblExchangeOrderStatus> GetExchangeOrderStatus();        
        List<TblExchangeOrderStatus> GetExchangeOrderStatusByDepartment(string statusName);

        public ABBPlanMasterViewModel GetABBPlanMasterDetails(int? BuId, int? productCatId, int? productTypeId, int? monthsdiff);

        public ABBRedemptionViewModel GetAbbOrderDetails(string regdNo);
        public RedemptionDataContract GetOrderData(int Id);
    }
}
