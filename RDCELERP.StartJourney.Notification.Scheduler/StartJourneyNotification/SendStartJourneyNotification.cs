using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Enums;
using RDCELERP.Common.Helper;
using RDCELERP.DAL.Entities;

namespace RDCELERP.StartJourney.Notification.Scheduler.StartJourneyNotification
{
    public class SendStartJourneyNotification : ISendStartJourneyNotification
    {
        Digi2l_DevContext _devContext;
        DateTime _currentDatetime = DateTime.Now;
        IPushNotificationManager _pushNotificationManager;

        public SendStartJourneyNotification(Digi2l_DevContext digi2L_DevContext, IPushNotificationManager pushNotificationManager)
        {
            _devContext = digi2L_DevContext;
            _pushNotificationManager = pushNotificationManager;
        }

        public void SendNotification()
        {
            List<TblVehicleJourneyTracking> tblVehicleJourneyTrackings = null;

            tblVehicleJourneyTrackings = _devContext.TblVehicleJourneyTrackings.Where(x => x.JourneyPlanDate.Value.Date == _currentDatetime.Date).ToList();
            if(tblVehicleJourneyTrackings != null)
            {
                foreach(var item  in tblVehicleJourneyTrackings)
                {
                    if(item.JourneyStartDatetime == null)
                    {
                        _pushNotificationManager.SendNotification(item.ServicePartnerId, item.DriverId, EnumHelper.DescriptionAttr(NotificationEnum.JourneyStartReminder), null, null);
                    }
                }
            }
        }

        public void Dispose()
        {
            
        }

    }
}
