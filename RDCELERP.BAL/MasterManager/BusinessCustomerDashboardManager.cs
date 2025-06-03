using AutoMapper;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Helper;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.Model.BusinessCustomer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.BAL.MasterManager
{
    public class BusinessCustomerDashboardManager :IBusinessCustomerDashboardManager
    {
        #region  Variable Declaration
        IBussinessCustomerRepository _bussinessCustomerRepository;
        private readonly IMapper _mapper;
        ILogging _logging;
        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();
        Digi2l_DevContext _context;
       
        #endregion
        public BusinessCustomerDashboardManager(IBussinessCustomerRepository bussinessCustomerRepository, ILogging logging,  Digi2l_DevContext context)
        {
            
            _logging = logging;
            _bussinessCustomerRepository = bussinessCustomerRepository;
            _context = context;
        }
        /// <summary>
        /// method to get dashboard detail by id
        /// </summary>
        /// <param name="BusinessCustomerId"></param>
        /// <returns>DashboardViewModel</returns>
        public DashboardViewModel GetCustomerDashboardById(int BusinessCustomerId)
        {
            try
            {
                DataTable dt = _bussinessCustomerRepository.GetCustomerDashboardById(BusinessCustomerId);

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0]; 

                    return new DashboardViewModel
                    {
                        BusinessCustomerId = row["BusinessCustomerId"] != DBNull.Value ? Convert.ToInt32(row["BusinessCustomerId"]) : 0,
                        CustomerName = row["CustomerName"] != DBNull.Value ? row["CustomerName"].ToString() : string.Empty,
                        TotalItemCount = row["TotalItemCount"] != DBNull.Value ? Convert.ToInt32(row["TotalItemCount"]) : 0,
                        HotDealsCount = row["HotDealsCount"] != DBNull.Value ? Convert.ToInt32(row["HotDealsCount"]) : 0,
                        BookingOrderCount = row["TotalBookingCount"] != DBNull.Value ? Convert.ToInt32(row["TotalBookingCount"]) : 0
                    };
                }
                else
                {
                    return null; 
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("BusinessCustomerDashBoardManager", "GetCustomerDashboardbyid", ex);
            }

            return null; 
        }

    }
}
