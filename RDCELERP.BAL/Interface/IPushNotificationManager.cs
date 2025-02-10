using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.MobileApplicationModel;
using RDCELERP.Model.PushNotification;
using RDCELERP.Model.ResponseModel;

namespace RDCELERP.BAL.Interface
{
    public interface IPushNotificationManager
    {
        public ResponseModel SendNotification(int? ServicePartnerId, int? DriverId, string? Title, string ordercount, string? regdNo);
        public bool SaveDeviceId(string deviceId, string deviceType, int userId);
        public ResponseResult GetNotificationListById(int Id, int? page, int? pageSize);
    }
}
        
