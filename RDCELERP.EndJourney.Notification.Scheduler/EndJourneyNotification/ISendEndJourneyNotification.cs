using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.EndJourney.Notification.Scheduler.EndJourneyNotification
{
    public interface ISendEndJourneyNotification : IDisposable
    {
        public void SendNotification();
    }
}
