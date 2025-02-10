using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Notification.Scheduler.NotificationScheduler
{
    public interface ISendNotificationScheduler : IDisposable
    {
        public void SendNotification();
    }
}
