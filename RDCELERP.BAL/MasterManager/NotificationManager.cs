using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Constant;
using RDCELERP.Common.Helper;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.DAL.Repository;
using RDCELERP.Model.Base;
using RDCELERP.Model.TextLocalSMS;
using RDCELERP.Model.Whatsapp;

namespace RDCELERP.BAL.MasterManager
{
    public class NotificationManager : INotificationManager
    {
        #region variable declartion
        IOptions<ApplicationSettings> _baseConfig;
        ILogging _logging;
        IMessageDetailRepository _MessageDetailRepository;
        IWhatsappNotificationManager _whatsappNotificationManager;
        IWhatsAppMessageRepository _WhatsAppMessageRepository;
        #endregion

        #region constructor
        public NotificationManager(IOptions<ApplicationSettings> baseConfig, ILogging logging, IMessageDetailRepository messageDetailRepository, IWhatsappNotificationManager whatsappNotificationManager, IWhatsAppMessageRepository whatsAppMessageRepository)
        {
            _baseConfig = baseConfig;
            _logging = logging;
            _MessageDetailRepository = messageDetailRepository;
            _whatsappNotificationManager = whatsappNotificationManager;
            _WhatsAppMessageRepository = whatsAppMessageRepository;
        }
        #endregion

        #region Method to send notification message to client
        /// <summary>
        /// Method to send notification message to client
        /// </summary>
        /// <param name="phoneNumber">phone Number</param>
        /// <param name="message">v</param>
        /// <returns>bool</returns>
        public bool SendNotificationSMS(string phoneNumber, string message, string OTPCode = "")
        {
            String result;
            bool flag = false;
            TextLocalResponseViewModel TextLocalResponseVM = null;
            string apiKey = _baseConfig.Value.SMSKey;
            string numbers = phoneNumber; //Code to trim number , remove blanks
            try
            {
                string sender = _baseConfig.Value.SenderName;
                string url = "https://api.textlocal.in/send/?apikey=" + apiKey + "&numbers=" + numbers + "&message=" + message + "&sender=" + sender;

                StreamWriter myWriter = null;
                HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(url);
                objRequest.Method = "POST";
                objRequest.ContentLength = Encoding.UTF8.GetByteCount(url);
                objRequest.ContentType = "application/x-www-form-urlencoded";
                try
                {
                    myWriter = new StreamWriter(objRequest.GetRequestStream());
                    myWriter.Write(url);
                }
                catch (Exception ex)
                {
                    _logging.WriteErrorToDB("NotificationManager", "SendNotificationSMS", ex);
                }
                finally
                {
                    myWriter.Close();
                }

                HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
                using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    TextLocalResponseVM = JsonConvert.DeserializeObject<TextLocalResponseViewModel>(result);
                    if (TextLocalResponseVM != null)
                        if (TextLocalResponseVM.status == "success")
                            flag = true;
                    // Close and clean up the StreamReader
                    sr.Close();
                }

                if (flag == true)
                {
                    TblMessageDetail messageDetailObj = new TblMessageDetail();
                    messageDetailObj.ResponseJson = string.Empty;
                    messageDetailObj.Code = OTPCode;
                    messageDetailObj.PhoneNumber = phoneNumber;
                    messageDetailObj.SendDate = DateTime.Now; ;
                    messageDetailObj.Message = message;
                    messageDetailObj.IsUsed = false;
                    _MessageDetailRepository.Create(messageDetailObj);
                    _MessageDetailRepository.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("NotoficationManager", "SendNotificationSMS", ex);
            }
            return flag;
        }
        #endregion

        #region Method to SMS send notification message to client
        /// <summary>
        /// Method to send notification message to client
        /// </summary>
        /// <param name="phoneNumber">phone Number</param>
        /// <param name="message">v</param>
        /// <returns>bool</returns>
        public bool SendQCSMS(string phoneNumber, string message)
        {
            String result;
            bool flag = false;
            TextLocalResponseViewModel TextLocalResponseVM = null;
            string apiKey = _baseConfig.Value.SMSKey;
            string numbers = phoneNumber; //Code to trim number , remove blanks
            try
            {
                string sender = _baseConfig.Value.SenderName;
                string url = "https://api.textlocal.in/send/?apikey=" + apiKey + "&numbers=" + numbers + "&message=" + message + "&sender=" + sender;

                StreamWriter myWriter = null;
                HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(url);
                objRequest.Method = "POST";
                objRequest.ContentLength = Encoding.UTF8.GetByteCount(url);
                objRequest.ContentType = "application/x-www-form-urlencoded";
                try
                {
                    myWriter = new StreamWriter(objRequest.GetRequestStream());
                    myWriter.Write(url);
                }
                catch (Exception ex)
                {
                    _logging.WriteErrorToDB("NotificationManager", "SendNotificationSMS", ex);
                }
                finally
                {
                    myWriter.Close();
                }

                HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
                using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    TextLocalResponseVM = JsonConvert.DeserializeObject<TextLocalResponseViewModel>(result);
                    if (TextLocalResponseVM != null)
                        if (TextLocalResponseVM.status == "success")
                            flag = true;
                    // Close and clean up the StreamReader
                    sr.Close();
                }

                //if (flag == true)
                //{
                //    TblMessageDetail messageDetailObj = new TblMessageDetail();
                //    messageDetailObj.ResponseJson = string.Empty;
                //    messageDetailObj.Code = OTPCode;
                //    messageDetailObj.PhoneNumber = phoneNumber;
                //    messageDetailObj.SendDate = DateTime.Now; ;
                //    messageDetailObj.Message = message;
                //    messageDetailObj.IsUsed = false;
                //    _MessageDetailRepository.Create(messageDetailObj);
                //    _MessageDetailRepository.SaveChanges();
                //}
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("NotoficationManager", "SendNotificationSMS", ex);
            }
            return flag;
        }
        #endregion

        #region Method to validate OTP
        /// <summary>
        /// Method to validate OTP
        /// </summary>
        /// <param name="phoneNumber">phone Number</param>
        /// <param name="OTP">OTP</param>
        /// <returns>bool</returns>
        public bool ValidateOTP(string phoneNumber, string OTP)
        {
            string response = string.Empty;
            string Message = string.Empty;
            bool flag = false;
            TblMessageDetail messageDetail = null;

            try
            {
                messageDetail = _MessageDetailRepository.GetSingle(x => x.PhoneNumber.Equals(phoneNumber) && (x.Code != null && x.Code.Equals(OTP)) && (x.IsUsed == false));
                DateTime start = DateTime.Now;
                if (messageDetail != null)
                {
                    DateTime oldDate = Convert.ToDateTime(messageDetail.SendDate);
                    int minDiff = Convert.ToInt32(_baseConfig.Value.OTPActivatedMin);
                    if (start.Subtract(oldDate) <= TimeSpan.FromMinutes(minDiff))
                        flag = true;
                    messageDetail.IsUsed = true;
                    _MessageDetailRepository.Update(messageDetail);
                    _MessageDetailRepository.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("NotoficationManager", "ValidateOTP", ex);
            }
            return flag;
        }
        #endregion

        #region Method to send WhatsApp notification message to client
        /// <summary>
        /// Method to send notification message to client
        /// </summary>
        /// <param name="phoneNumber">phone Number</param>
        /// <param name="message">v</param>
        /// <returns>bool</returns>
        //public bool SendWhatsApp(string url, WhatsappRescheduleViewModel.WhatsappTemplate whatsappObj, string CustPhoneNo)
        //{
        //    WhatasappResponse whatasappResponse = new WhatasappResponse();
        //    TblWhatsAppMessage tblwhatsappmessage = null;
        //    bool flag = false;
        //    try
        //    {
        //        RestResponse response = _whatsappNotificationManager.Rest_InvokeWhatsappserviceCall(url, Method.Post, whatsappObj);
        //        if (response.Content != null)
        //        {
        //            whatasappResponse = JsonConvert.DeserializeObject<WhatasappResponse>(response.Content);
        //            tblwhatsappmessage = new TblWhatsAppMessage();
        //            tblwhatsappmessage.TemplateName = NotificationConstants.RescheduleDate_Time;
        //            tblwhatsappmessage.IsActive = true;
        //            tblwhatsappmessage.PhoneNumber = tblCustomerDetail.PhoneNumber;
        //            tblwhatsappmessage.SendDate = DateTime.Now;
        //            tblwhatsappmessage.MsgId = whatasappResponse.msgId;
        //            _WhatsAppMessageRepository.Create(tblwhatsappmessage);
        //            _WhatsAppMessageRepository.SaveChanges();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logging.WriteErrorToDB("NotoficationManager", "SendNotificationSMS", ex);
        //    }
        //    return flag;
        //}
        #endregion
    }
}
