using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Helper;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;

namespace RDCELERP.BAL.MasterManager
{
    public class OrderQCManager : IOrderQCManager
    {
        IOrderQCRepository _orderQCRepository;
        ILogging _logging;

        public OrderQCManager(IOrderQCRepository orderQCRepository, ILogging logging)
        {
            _orderQCRepository = orderQCRepository;
            _logging = logging;
        }

        public TblOrderQc GetOrderDetails(int orderTransId)
        {
            TblOrderQc tblOrderQc = null;
            try
            {
                tblOrderQc = _orderQCRepository.GetQcorderBytransId(orderTransId);
                if(tblOrderQc!= null)
                {
                    return tblOrderQc;
                }
                else
                {
                    return tblOrderQc = new TblOrderQc();
                }
            }
            catch(Exception ex)
            {
                _logging.WriteErrorToDB("OrderQCManager", "GetOrderDetails", ex);
            }
            return tblOrderQc = new TblOrderQc();
        }

    }
}
