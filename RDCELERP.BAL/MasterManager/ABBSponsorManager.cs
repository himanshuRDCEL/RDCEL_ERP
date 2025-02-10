using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Helper;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.Model.ABBDashBoardModel;

namespace RDCELERP.BAL.MasterManager
{
    public class ABBSponsorManager : IABBSponsorManager
    {
        #region variable declaration

        public ILogging _logging;
        IBusinessPartnerRepository _businessPartnerRepository;
        IAbbRegistrationRepository _abbRegistrationRepository;
        #endregion
        public ABBSponsorManager(ILogging logging, IBusinessPartnerRepository businessPartnerRepository, IAbbRegistrationRepository abbRegistrationRepository)
        {
            _logging = logging;
            _businessPartnerRepository = businessPartnerRepository;
            _abbRegistrationRepository = abbRegistrationRepository;
        }
        public ABBDashBoardCountModel GetOrderCounts(UserDetailsForABBDashBoard userDetailsDC)
        {
            List<TblAbbregistration> tblABBregistrationOBJ = new List<TblAbbregistration>();
            List<TblAbbregistration> ApprovedOrders = new List<TblAbbregistration>();
            List<TblAbbregistration> NotApprocedORders = new List<TblAbbregistration>();
             ABBDashBoardCountModel abbOrderContDC = new ABBDashBoardCountModel();
            try
            {
                tblABBregistrationOBJ = _abbRegistrationRepository.GetOrderCountForDashboard(userDetailsDC.BusinessUnitId);
                if (tblABBregistrationOBJ.Count > 0)
                {
                    abbOrderContDC.TotalABBOrdersRecieved=tblABBregistrationOBJ.Count();
                }
                else
                {
                    abbOrderContDC.TotalABBOrdersRecieved = 0;
                }
                ApprovedOrders = _abbRegistrationRepository.GetApprovedOrders(userDetailsDC.BusinessUnitId);
                if (ApprovedOrders.Count > 0)
                {
                    abbOrderContDC.OrdersApproved = ApprovedOrders.Count();
                }
                else
                {
                    abbOrderContDC.OrdersApproved = 0;
                }
                NotApprocedORders = _abbRegistrationRepository.GetNotApprovedOrders(userDetailsDC.BusinessUnitId);
                if (NotApprocedORders.Count > 0)
                {
                    abbOrderContDC.OrdersNotApproved = NotApprocedORders.Count();
                }
                else
                {
                    abbOrderContDC.OrdersNotApproved = 0;
                }
            }
            catch(Exception ex)
            {
                _logging.WriteErrorToDB("ABBSponsorManager", "GetOrderCounts", ex);
            }
            return abbOrderContDC;
        }
    }
}
