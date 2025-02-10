using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Enums;
using RDCELERP.Common.Helper;
using RDCELERP.DAL.Entities;

namespace RDCELERP.EndJourney.Notification.Scheduler.EndJourneyNotification
{
    public class SendEndJourneyNotification : ISendEndJourneyNotification
    {
        Digi2l_DevContext _devContext;
        DateTime _currentDatetime = DateTime.Now;
        IPushNotificationManager _pushNotificationManager;

        public SendEndJourneyNotification(Digi2l_DevContext digi2L_DevContext, IPushNotificationManager pushNotificationManager)
        {
            _devContext = digi2L_DevContext;
            _pushNotificationManager = pushNotificationManager;
        }
        public void SendNotification()
        {
            List<TblVehicleJourneyTracking> tblVehicleJourneyTrackings = new List<TblVehicleJourneyTracking>();
            tblVehicleJourneyTrackings = _devContext.TblVehicleJourneyTrackings.Where(x => x.JourneyPlanDate.Value.Date == _currentDatetime.Date).ToList();

            if(tblVehicleJourneyTrackings != null )
            {
                foreach(var item in tblVehicleJourneyTrackings)
                {
                    if(item.JourneyEndTime == null && item.JourneyStartDatetime != null)
                    {
                        _pushNotificationManager.SendNotification(item.ServicePartnerId, item.DriverId, EnumHelper.DescriptionAttr(NotificationEnum.PendingDeliveriesAlert), null, null);
                    }
                }
            }

        }

        public void Dispose()
        {
            
        }

    }
}
