using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.BAL.Interface
{
    public interface INotificationManager
    {
        public bool SendNotificationSMS(string phoneNumber, string message, string OTPCode = "");
        public bool ValidateOTP(string phoneNumber, string OTP);
        public bool SendQCSMS(string phoneNumber, string message);
        //public bool SendWhatsApp(string url, string whatsappObj, string CustPhoneNo);
    }
}
