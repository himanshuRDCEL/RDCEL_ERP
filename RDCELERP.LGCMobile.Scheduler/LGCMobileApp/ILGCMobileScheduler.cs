using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.LGCMobile.Scheduler.LGCMobileApp
{
    public interface ILGCMobileScheduler : IDisposable
    {
        public void RollbackOrderFromDriver();
        //public void Dispose();
    }
}
