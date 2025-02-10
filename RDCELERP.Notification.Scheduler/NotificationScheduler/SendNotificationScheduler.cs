using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Enums;
using RDCELERP.Common.Helper;
using RDCELERP.DAL.Entities;

namespace RDCELERP.Notification.Scheduler.NotificationScheduler
{
    public class SendNotificationScheduler : ISendNotificationScheduler
    {
        IPushNotificationManager _pushNotificationManager;
        Digi2l_DevContext _devContext;
        DateTime _currentDatetime = DateTime.Now;

        public SendNotificationScheduler(IPushNotificationManager pushNotificationManager, Digi2l_DevContext digi2L_DevContext)
        {
            _pushNotificationManager = pushNotificationManager;
            _devContext = digi2L_DevContext;
        }

        public void SendNotification()
        {
            List<TblLogistic> tblLogistics = new List<TblLogistic>();

            tblLogistics = _devContext.TblLogistics.Where(x => x.ModifiedDate.Value.Date == _currentDatetime.Date).ToList();

            if (tblLogistics != null)
            {
                foreach (var item in tblLogistics)
                {
                    if (item.StatusId == Convert.ToInt32(OrderStatusEnum.VehicleAssignbyServicePartner))
                    {
                        _pushNotificationManager.SendNotification(item.ServicePartnerId, item.DriverDetailsId, EnumHelper.DescriptionAttr(NotificationEnum.OrderCancellationAlert), null, null);
                    }
                }
            }
        }

        public void Dispose()
        {
            
        }

    }
}
