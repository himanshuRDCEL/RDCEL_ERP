using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.DAL.AbstractRepository;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;

namespace RDCELERP.DAL.Repository
{
    public class PushNotificationSavedDetailsRepository : AbstractRepository<TblPushNotificationSavedDetail>, IPushNotificationSavedDetailsRepository
    {
        private Digi2l_DevContext _DbContext;
        public PushNotificationSavedDetailsRepository(Digi2l_DevContext dbContext)
       : base(dbContext)
        {
            _DbContext = dbContext;
        }

        public List<TblPushNotificationSavedDetail> GetNotificationListById(int id)
        {
            List<TblPushNotificationSavedDetail> tblPushNotificationSavedDetails = new List<TblPushNotificationSavedDetail>();

            tblPushNotificationSavedDetails = _DbContext.TblPushNotificationSavedDetails.Where(x => x.IsActive == true && x.SentUserId == id).ToList();

            return tblPushNotificationSavedDetails;
        }
    }

}

