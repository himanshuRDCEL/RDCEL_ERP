using RDCELERP.BAL.Interface;
using RDCELERP.Common.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Mailjet.Client.TransactionalEmails.Response;
using Mailjet.Client;
using Microsoft.Extensions.Options;
using Mailjet.Client.Resources;
using Mailjet.Client.TransactionalEmails;
using RDCELERP.Model.Base;
using Microsoft.AspNetCore.Hosting.Server;
using Attachment = System.Net.Mail.Attachment;
using RDCELERP.Common.Enums;
using Microsoft.AspNetCore.Hosting;
using System.Net;
using RDCELERP.Model.TemplateConfiguration;
using NPOI.HPSF;

namespace RDCELERP.BAL.MasterManager
{
    public class MailManager : IMailManager
    {

        #region  Variable Declaration

        ILogging _logging;
        //private readonly MailSettings _mailSettings;
        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();
        private readonly IWebHostEnvironment _webHostEnvironment;
        public readonly IOptions<ApplicationSettings> _config;
        #endregion
        public MailManager(IWebHostEnvironment webHostEnvironment,ILogging logging, IOptions<ApplicationSettings> config)
        {
            _logging = logging;
            _config = config;
            _webHostEnvironment = webHostEnvironment;
        }

        /// <summary>
        /// Send Email Locally
        /// </summary>
        /// <param name="to">To</param>
        /// <param name="body">Body</param>
        /// <param name="subject">Subject</param>
        public void SendEmailLocally(string to, string body, string subject, byte[] quoteTemplate, string TemplateName)
        {

            try
            {
                //int EnableMail = Convert.ToInt32(ConfigurationManager.AppSettings["EnableMail"]);
                //if (EnableMail == 1)
                //{
                //string bccEmailAddress = ConfigurationManager.AppSettings["BCCEmail"].ToString();
                //string SupportDisplayName = ConfigurationManager.AppSettings["SupportDisplayName"].ToString();
                //string fromEmail = ConfigurationManager.AppSettings["FromEmail"].ToString();
                //string userName = ConfigurationManager.AppSettings["UserName"].ToString();
                //string password = ConfigurationManager.AppSettings["Password"].ToString();
                //string hostName = ConfigurationManager.AppSettings["HostName"].ToString();
                //int portNumber = Convert.ToInt32(ConfigurationManager.AppSettings["PortNumber"]);
                //bool isSSL = Convert.ToBoolean(ConfigurationManager.AppSettings["IsSSL"]);
                //bool UseDefaultCredentials = Convert.ToBoolean(ConfigurationManager.AppSettings["UseDefaultCredentials"]);

                    //string bccEmailAddress = "";
                    //string SupportDisplayName = "";
                    string fromEmail = "kranti@utcdigital.com";
                    //string userName = "dosolar80@gmail.com";
                    string password = "Kranti@37";
                    string hostName = "smtp.Zoho.com";
                    int portNumber = 465;
                    bool isSSL = true;
                    bool UseDefaultCredentials = false;
                    MailMessage msg = new MailMessage();
                    msg.To.Add(new MailAddress(to));
                    //msg.Bcc.Add(new MailAddress(bccEmailAddress));
                    msg.From = new MailAddress(fromEmail);
                    msg.Subject = subject;                  
                    msg.Body = body;

                //msg.Attachments.Add(new System.Net.Mail.Attachment(new MemoryStream(quoteTemplate), TemplateName + ".pdf"));
                    msg.IsBodyHtml = true;
                System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
                    client.Host = hostName;
                    client.UseDefaultCredentials = UseDefaultCredentials;
                    client.Credentials = new System.Net.NetworkCredential(fromEmail, password);
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.Port = portNumber;
                    client.EnableSsl = isSSL;

                client.Send(msg);
                //}
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("MailManager", "SendEmail", ex);
                // LibLogging.WriteErrorToDB("MailManager", "SendEmailLocally", ex);
            }

        }

        /// <summary>
        /// Send Email for deal steps
        /// </summary>
        /// <param name="to">To</param>
        /// <param name="body">Body</param>
        /// <param name="subject">Subject</param>
        public void SendEmail(string to, string body, string subject)
        {
            try
            {

                MailMessage message = new MailMessage();
                //SmtpClient smtp = new SmtpClient("mx.zohomail.com");
                SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                message.From = new MailAddress("krantisilawat93293@gmail.com");
                message.To.Add(new MailAddress(to));
                //message.From = new MailAddress("kranti@utcdigital.com");
                //message.To.Add(new MailAddress(to));
                message.Subject = subject;
                message.IsBodyHtml = true; //to make message body as html
                message.Body = body;
                smtp.Port = 25;
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = new NetworkCredential("krantisilawat93293@gmail.com", "Kranti@370");
                //smtp.Credentials = new NetworkCredential("kranti@utcdigital.com", "Kranti@37");
                smtp.EnableSsl = true;
                smtp.Send(message);
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("CommonManager", "SendMailList", ex);
            }



        }
        #region for forgot password
        /// <summary>
        /// Send Email Locally
        /// </summary>
        /// <param name="to">To</param>
        /// <param name="body">Body</param>
        /// <param name="subject">Subject</param>
        public async Task<bool> SendEmailAsync(string to, string body, string subject)
        {
            bool flag = false;

            try
            {
                Task<TransactionalEmailResponse> task = JetMailSend(to, body, subject);
                if (task != null)
                    flag = true;            
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return flag;

        }
        public async Task<TransactionalEmailResponse> JetMailSend(string to, string body, string subject)
        {
            bool flag = false;
            TransactionalEmailResponse response = null;

            if (to.Length > 0)
            {
                MailjetClient client = new MailjetClient(_config.Value.MailjetAPIKey, _config.Value.MailjetAPISecret);

                MailjetRequest request = new MailjetRequest
                {
                    Resource = Send.Resource
                };

                List<SendContact> contactList = new List<SendContact>();
                to = to.Replace(",", ";");
                String[] recipient = to.Split(';');
                for (int n = 0; n < recipient.Length; n++)
                    contactList.Add(new SendContact(recipient[n].ToString()));


                List<SendContact> bccContactList = new List<SendContact>();
                if (_config.Value.BccEmailAddress != null)
                {
                    _config.Value.BccEmailAddress = _config.Value.BccEmailAddress.Replace(",", ";");
                    String[] BccEmailAddresses = _config.Value.BccEmailAddress.Split(';');
                    for (int n = 0; n < BccEmailAddresses.Length; n++)
                        bccContactList.Add(new SendContact(BccEmailAddresses[n].ToString()));
                }

                // construct your email with builder
                //.WithReplyTo(new SendContact(this.SenderEmail, this.DisplayName))
                var email = new TransactionalEmail();

                email.From = new SendContact(_config.Value.FromEmail, _config.Value.FromDisplayName);
                email.ReplyTo = new SendContact(_config.Value.FromEmail, _config.Value.FromDisplayName);
                email.To = contactList;

                if (bccContactList != null && bccContactList.Count > 0)
                    email.Bcc = bccContactList;

                email.Subject = subject;
                email.HTMLPart = body;
                try
                {
                    // invoke API to send email
                    response = await client.SendTransactionalEmailAsync(email);//SendTransactionalEmailAsync(email);
                    if (response != null && response.Messages != null && response.Messages.Length > 0 && !string.IsNullOrEmpty(response.Messages[0].Status) && response.Messages[0].Status.ToLower().Equals("success"))
                        flag = true;
                }
                catch (Exception ex)
                {
                    flag = false;
                }

            }
            return response;
        }

        #endregion

        #region use for EVC welcome Messege
        public async Task<bool> SendEmailforevcAsync(string to, string body, string subject)
        {
            bool flag = false;

            try
            {
                Task<TransactionalEmailResponse> task = JetMailSendforevc(to, body, subject);
                if (task != null)
                    flag = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return flag;

        }
        public async Task<TransactionalEmailResponse> JetMailSendforevc(string to, string body, string subject)
        {
            bool flag = false;
            TransactionalEmailResponse response = null;

            if (to.Length > 0)
            {
                MailjetClient client = new MailjetClient(_config.Value.MailjetAPIKey, _config.Value.MailjetAPISecret);

                MailjetRequest request = new MailjetRequest
                {
                    Resource = Send.Resource
                };

                List<SendContact> contactList = new List<SendContact>();
                to = to.Replace(",", ";");
                String[] recipient = to.Split(';');
                for (int n = 0; n < recipient.Length; n++)
                    contactList.Add(new SendContact(recipient[n].ToString()));


                List<SendContact> bccContactList = new List<SendContact>();
                if (_config.Value.BccEmailAddress != null)
                {
                    _config.Value.BccEmailAddress = _config.Value.BccEmailAddress.Replace(",", ";");
                    String[] BccEmailAddresses = _config.Value.BccEmailAddress.Split(';');
                    for (int n = 0; n < BccEmailAddresses.Length; n++)
                        bccContactList.Add(new SendContact(BccEmailAddresses[n].ToString()));
                }
                //Add Attachment 
                List<Mailjet.Client.TransactionalEmails.Attachment> AttachmentsList = new List<Mailjet.Client.TransactionalEmails.Attachment>();
                string FilePath = EnumHelper.DescriptionAttr(FilePathEnum.EVCAttachedFolder);
                var attachmentsT_CPath = string.Concat(_webHostEnvironment.WebRootPath, "\\", FilePath);
                if (Directory.Exists(attachmentsT_CPath))
                {
                    string[] filesTC = Directory.GetFiles(attachmentsT_CPath);
                    foreach (string fileName in filesTC)
                    {
                        Byte[] bytes = File.ReadAllBytes(fileName);
                        String file = Convert.ToBase64String(bytes);
                        AttachmentsList.Add(new Mailjet.Client.TransactionalEmails.Attachment(fileName, ".pdf", file));
                    }
                }      
                var email = new TransactionalEmail();

                email.From = new SendContact(_config.Value.FromEmail, _config.Value.FromDisplayName);
                email.ReplyTo = new SendContact(_config.Value.FromEmail, _config.Value.FromDisplayName);
                email.To = contactList;

                if (bccContactList != null && bccContactList.Count > 0)
                    email.Bcc = bccContactList;

                email.Subject = subject;
                email.HTMLPart = body;
                email.Attachments = AttachmentsList;

                try
                {
                    // invoke API to send email
                    response = await client.SendTransactionalEmailAsync(email);
                    if (response != null && response.Messages != null && response.Messages.Length > 0 && !string.IsNullOrEmpty(response.Messages[0].Status) && response.Messages[0].Status.ToLower().Equals("success"))
                        flag = true;
                }
                catch (Exception ex)
                {
                    flag = false;
                }

            }
            return response;
        }

        #endregion

        #region use for LGC welcome Messege
        public async Task<bool> SendEmailforLGCAsync(string to, string body, string subject)
        {
            bool flag = false;

            try
            {
                Task<TransactionalEmailResponse> task = JetMailSendforLGC(to, body, subject);
                if (task != null)
                    flag = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return flag;

        }
        public async Task<TransactionalEmailResponse> JetMailSendforLGC(string to, string body, string subject)
        {
            bool flag = false;
            TransactionalEmailResponse response = null;

            if (to.Length > 0)
            {
                MailjetClient client = new MailjetClient(_config.Value.MailjetAPIKey, _config.Value.MailjetAPISecret);

                MailjetRequest request = new MailjetRequest
                {
                    Resource = Send.Resource
                };

                List<SendContact> contactList = new List<SendContact>();
                to = to.Replace(",", ";");
                String[] recipient = to.Split(';');
                for (int n = 0; n < recipient.Length; n++)
                    contactList.Add(new SendContact(recipient[n].ToString()));


                List<SendContact> bccContactList = new List<SendContact>();
                if (_config.Value.BccEmailAddress != null)
                {
                    _config.Value.BccEmailAddress = _config.Value.BccEmailAddress.Replace(",", ";");
                    String[] BccEmailAddresses = _config.Value.BccEmailAddress.Split(';');
                    for (int n = 0; n < BccEmailAddresses.Length; n++)
                        bccContactList.Add(new SendContact(BccEmailAddresses[n].ToString()));
                }
                ////Add Attachment 
                //List<Mailjet.Client.TransactionalEmails.Attachment> AttachmentsList = new List<Mailjet.Client.TransactionalEmails.Attachment>();
                //string FilePath = EnumHelper.DescriptionAttr(FilePathEnum.EVCAttachedFolder);
                //var attachmentsT_CPath = string.Concat(_webHostEnvironment.WebRootPath, "\\", FilePath);
                //if (Directory.Exists(attachmentsT_CPath))
                //{
                //    string[] filesTC = Directory.GetFiles(attachmentsT_CPath);
                //    foreach (string fileName in filesTC)
                //    {
                //        Byte[] bytes = File.ReadAllBytes(fileName);
                //        String file = Convert.ToBase64String(bytes);
                //        AttachmentsList.Add(new Mailjet.Client.TransactionalEmails.Attachment(fileName, ".pdf", file));
                //    }
                //}
                var email = new TransactionalEmail();

                email.From = new SendContact(_config.Value.FromEmail, _config.Value.FromDisplayName);
                email.ReplyTo = new SendContact(_config.Value.FromEmail, _config.Value.FromDisplayName);
                email.To = contactList;

                if (bccContactList != null && bccContactList.Count > 0)
                    email.Bcc = bccContactList;

                email.Subject = subject;
                email.HTMLPart = body;
               // email.Attachments = AttachmentsList;

                try
                {
                    // invoke API to send email
                    response = await client.SendTransactionalEmailAsync(email);
                    if (response != null && response.Messages != null && response.Messages.Length > 0 && !string.IsNullOrEmpty(response.Messages[0].Status) && response.Messages[0].Status.ToLower().Equals("success"))
                        flag = true;
                }
                catch (Exception ex)
                {
                    flag = false;
                }

            }
            return response;
        }

        #endregion
        public async Task<bool> SendSMTPEmailAsync(string to, string body, string subject)
        {
            bool flag = true;

            try
            {
                string from = _config.Value.FromEmail;
                string fromDisplayName = _config.Value.FromDisplayName;
                //string from = new SendContact(_config.Value.FromEmail, _config.Value.FromDisplayName);
                //Create the object of MailMessage
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(from, fromDisplayName); //From Email Id & Display name
                mailMessage.Subject = subject; //Subject of Email
                mailMessage.Body = body; //body or message of Email
                mailMessage.IsBodyHtml = true;
                mailMessage.Priority = MailPriority.High; //if you want to send High importance email


                if (to.Length > 0)
                {
                    MailjetClient client = new MailjetClient(_config.Value.MailjetAPIKey, _config.Value.MailjetAPISecret);

                    MailjetRequest request = new MailjetRequest
                    {
                        Resource = Send.Resource
                    };
                    List<SendContact> contactList = new List<SendContact>();
                    List<MailAddress> mailAddress = new List<MailAddress>();
                    Mailjet.Client.TransactionalEmails.SendContact sendContact;
                    to = to.Replace(",", ";");
                    String[] recipient = to.Split(';');
                    for (int n = 0; n < recipient.Length; n++)
                        contactList.Add(new SendContact(recipient[n].ToString()));
                    //for (int n = 0; n < recipient.Length; n++)
                    //    mailAddress.Add(new MailAddress(recipient[n]));

                    List<SendContact> bccContactList = new List<SendContact>();
                    if (_config.Value.BccEmailAddress != null)
                    {
                        _config.Value.BccEmailAddress = _config.Value.BccEmailAddress.Replace(",", ";");
                        String[] BccEmailAddresses = _config.Value.BccEmailAddress.Split(';');
                        for (int n = 0; n < BccEmailAddresses.Length; n++)
                            bccContactList.Add(new SendContact(BccEmailAddresses[n].ToString()));
                    }
                    //mailAddress.Add(contactList);
                    //You can add either multiple To,CC,BCC or single emails
                    foreach (var Item in contactList)
                    {
                        mailMessage.To.Add(new MailAddress(Item.Email));
                    }

                    foreach (var Item in bccContactList)
                    {
                        mailMessage.Bcc.Add(new MailAddress(Item.Email));
                    }



                    //mailMessage.To.Add(new MailAddress("ToEmailId1@domain.com;ToEmailId2@domain.com;ToEmailId3@domain.com;")); //adding multiple TO Email Id
                    //    mailMessage.CC.Add(new MailAddress("CCEmailId1@domain.com;CCEmailId2@domain.com;CCEmailId3@domain.com;")); //Adding Multiple CC email Id
                    //mailMessage.Bcc.Add(new MailAddress("BCCEmailId1@domain.com;BCCEmailId2@domain.com;BCCEmailId3@domain.com;")); //Adding Multiple BCC email Id

                    //Add Attachments, here i gave one folder name, and it will add all files in that folder as attachments, Or if you want only one file also can add
                    //string attachmentsT_CPath = @"wwwroot/DBFiles/ECVAttached/EVCT_C.pdf";
                    //if (Directory.Exists(attachmentsT_CPath))
                    //{
                    //    string[] filesTC = Directory.GetFiles(attachmentsT_CPath);
                    //    foreach (string fileName in filesTC)
                    //    {
                    //        Attachment fileTC = new Attachment(fileName);
                    //        mailMessage.Attachments.Add(fileTC);
                    //    }
                    //}
                    //string attachmentsBrochurePath = @"wwwroot/DBFiles/ECVAttached/EVC_Brochure.pdf";
                    //if (Directory.Exists(attachmentsBrochurePath))
                    //{
                    //    string[] filesB = Directory.GetFiles(attachmentsBrochurePath);
                    //    foreach (string fileName in filesB)
                    //    {
                    //        Attachment fileB = new Attachment(fileName);
                    //        mailMessage.Attachments.Add(fileB);
                    //    }
                    //}


                    //Assign the SMTP address and port
                    string hostName = "smtp.gmail.com";
                    int portNumber = 465;
                    bool isSSL = true;
                    bool UseDefaultCredentials = false;
                    string fromEmail = "kranti@utcdigital.com";
                    //string userName = "dosolar80@gmail.com";
                    string password = "Kranti@37";
                    System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
                    smtp.Host = hostName;
                    smtp.UseDefaultCredentials = UseDefaultCredentials;
                    smtp.Credentials = new System.Net.NetworkCredential(fromEmail, password);
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.Port = portNumber;
                    smtp.EnableSsl = isSSL;

                    //Finally send Email
                    smtp.Send(mailMessage);
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                flag = false;
                _logging.WriteErrorToDB("CommonManager", "GetAllAccessList", ex);
            }

            return flag;
        }

        #region Mail Send with Attatchment using JetMail Added By VK
        public async Task<bool> JetMailSendWithAttachment(TemplateViewModel templateVM)
        {
            bool flag = false;
            TransactionalEmailResponse response = new TransactionalEmailResponse();

            if (templateVM != null && templateVM.To.Length > 0)
            {
                try
                {
                    #region Mail Send To
                    List<SendContact> contactList = new List<SendContact>();
                    templateVM.To = templateVM.To.Replace(",", ";");
                    String[] recipient = templateVM.To.Split(';');
                    for (int n = 0; n < recipient.Length; n++)
                        contactList.Add(new SendContact(recipient[n].Trim().ToString()));
                    #endregion

                    #region Add Cc
                    List<SendContact> ccContactList = new List<SendContact>();
                    if (templateVM.Cc != null && templateVM.Cc.Length > 0)
                    {
                        templateVM.Cc = templateVM.Cc.Replace(",", ";");
                        String[] ccEmailAddresses = templateVM.Cc.Split(';');
                        for (int n = 0; n < ccEmailAddresses.Length; n++)
                            ccContactList.Add(new SendContact(ccEmailAddresses[n].Trim().ToString()));
                    }
                    #endregion 

                    #region Add Bcc
                    List<SendContact> bccContactList = new List<SendContact>();
                    if (templateVM.Bcc != null)
                    {
                        templateVM.Bcc = templateVM.Bcc.Replace(",", ";");
                        String[] BccEmailAddresses = templateVM.Bcc.Split(';');
                        for (int n = 0; n < BccEmailAddresses.Length; n++)
                            bccContactList.Add(new SendContact(BccEmailAddresses[n].Trim().ToString()));
                    }
                    #endregion

                    List<Mailjet.Client.TransactionalEmails.Attachment> AttachmentsList = new List<Mailjet.Client.TransactionalEmails.Attachment>();
                    #region Add Attachment ABB E-Certificate
                    if (templateVM.IsCertificateGenerated == true)
                    {
                        var attachmentsT_CPath = string.Concat(_webHostEnvironment.WebRootPath, "\\", templateVM.AttachmentFilePath);
                        if (Directory.Exists(attachmentsT_CPath))
                        {
                            string[] filesTC = Directory.GetFiles(attachmentsT_CPath, templateVM.AttachmentFileName);
                            foreach (string fileName in filesTC)
                            {
                                Byte[] bytes = File.ReadAllBytes(fileName);
                                String file = Convert.ToBase64String(bytes);
                                AttachmentsList.Add(new Mailjet.Client.TransactionalEmails.Attachment(templateVM.AttachmentFileName, ".pdf", file));
                            }
                        }
                    }
                    #endregion

                    #region Add Attachment ABB Customer Invoice
                    if (templateVM.IsInvoiceGenerated == true)
                    {
                        var attachmentsT_CPath = string.Concat(_webHostEnvironment.WebRootPath, "\\", templateVM.InvAttachFilePath);
                        if (Directory.Exists(attachmentsT_CPath))
                        {
                            string[] filesTC = Directory.GetFiles(attachmentsT_CPath, templateVM.InvAttachFileName);
                            foreach (string fileName in filesTC)
                            {
                                Byte[] bytes = File.ReadAllBytes(fileName);
                                String file = Convert.ToBase64String(bytes);
                                AttachmentsList.Add(new Mailjet.Client.TransactionalEmails.Attachment(templateVM.InvAttachFileName, ".pdf", file));
                            }
                        }
                    }
                    #endregion

                    #region Mail Configurations
                    var email = new TransactionalEmail();
                    email.From = new SendContact(_config.Value.FromEmail, _config.Value.FromDisplayName);
                    email.ReplyTo = new SendContact(_config.Value.FromEmail, _config.Value.FromDisplayName);
                    email.To = contactList;

                    if (AttachmentsList != null && AttachmentsList.Count > 0)
                    {
                        email.Attachments = AttachmentsList;
                    }

                    if (ccContactList != null && ccContactList.Count > 0)
                        email.Cc = ccContactList;

                    if (bccContactList != null && bccContactList.Count > 0)
                        email.Bcc = bccContactList;

                    email.Subject = templateVM.Subject;
                    email.HTMLPart = templateVM.HtmlBody;
                    #endregion

                    #region Invoke API to send email
                    MailjetClient client = new MailjetClient(_config.Value.MailjetAPIKey, _config.Value.MailjetAPISecret);
                    MailjetRequest request = new MailjetRequest
                    {
                        Resource = Send.Resource
                    };
                    response = await client.SendTransactionalEmailAsync(email);
                    if (response != null && response.Messages != null && response.Messages.Length > 0 && !string.IsNullOrEmpty(response.Messages[0].Status) && response.Messages[0].Status.ToLower().Equals("success"))
                        flag = true;
                    #endregion
                }
                catch (Exception ex)
                {
                    flag = false;
                    _logging.WriteErrorToDB("MailManager", "JetMailSendWithAttachment", ex);
                }
            }
            return flag;
        }
        #endregion

        #region Mail Send with Attatchment using JetMail Added By VK
        public async Task<bool> JetMailSendWithAttachedFile(TemplateViewModel templateVM)
        {
            bool flag = false;
            TransactionalEmailResponse response = new TransactionalEmailResponse();

            if (templateVM != null && templateVM.To.Length > 0)
            {
                try
                {
                    #region Mail Send To
                    List<SendContact> contactList = new List<SendContact>();
                    templateVM.To = templateVM.To.Replace(",", ";");
                    String[] recipient = templateVM.To.Split(';');
                    for (int n = 0; n < recipient.Length; n++)
                        contactList.Add(new SendContact(recipient[n].Trim().ToString()));
                    #endregion

                    #region Add Cc
                    List<SendContact> ccContactList = new List<SendContact>();
                    if (templateVM.Cc != null && templateVM.Cc.Length > 0)
                    {
                        templateVM.Cc = templateVM.Cc.Replace(",", ";");
                        String[] ccEmailAddresses = templateVM.Cc.Split(';');
                        for (int n = 0; n < ccEmailAddresses.Length; n++)
                            ccContactList.Add(new SendContact(ccEmailAddresses[n].Trim().ToString()));
                    }
                    #endregion 

                    #region Add Bcc
                    List<SendContact> bccContactList = new List<SendContact>();
                    if (templateVM.Bcc != null)
                    {
                        templateVM.Bcc = templateVM.Bcc.Replace(",", ";");
                        String[] BccEmailAddresses = templateVM.Bcc.Split(';');
                        for (int n = 0; n < BccEmailAddresses.Length; n++)
                            bccContactList.Add(new SendContact(BccEmailAddresses[n].Trim().ToString()));
                    }
                    #endregion

                    List<Mailjet.Client.TransactionalEmails.Attachment> AttachmentsList = new List<Mailjet.Client.TransactionalEmails.Attachment>();
                    #region Add Direct File Attachment
                    if (templateVM.byteArrayExch != null)
                    {
                        string fileNameExch = templateVM.AttachFileNameExch ?? "PendingOrdersExch";
                        Byte[]? bytes = templateVM.byteArrayExch;
                        String file = Convert.ToBase64String(bytes);
                        AttachmentsList.Add(new Mailjet.Client.TransactionalEmails.Attachment(fileNameExch, ".xls", file));
                    }
                    if (templateVM.byteArrayAbb != null)
                    {
                        string fileNameAbb = templateVM.AttachFileNameAbb ?? "PendingOrdersAbb";
                        Byte[]? bytes = templateVM.byteArrayAbb;
                        String file = Convert.ToBase64String(bytes);
                        AttachmentsList.Add(new Mailjet.Client.TransactionalEmails.Attachment(fileNameAbb, ".xls", file));
                    }
                    #endregion

                    #region Mail Configurations
                    var email = new TransactionalEmail();
                    email.From = new SendContact(_config.Value.FromEmail, _config.Value.FromDisplayName);
                    email.ReplyTo = new SendContact(_config.Value.FromEmail, _config.Value.FromDisplayName);
                    email.To = contactList;

                    if (AttachmentsList != null && AttachmentsList.Count > 0)
                    {
                        email.Attachments = AttachmentsList;
                    }

                    if (ccContactList != null && ccContactList.Count > 0)
                        email.Cc = ccContactList;

                    if (bccContactList != null && bccContactList.Count > 0)
                        email.Bcc = bccContactList;

                    email.Subject = templateVM.Subject;
                    email.HTMLPart = templateVM.HtmlBody;
                    #endregion

                    #region Invoke API to send email
                    MailjetClient client = new MailjetClient(_config.Value.MailjetAPIKey, _config.Value.MailjetAPISecret);
                    MailjetRequest request = new MailjetRequest
                    {
                        Resource = Send.Resource
                    };
                    response = await client.SendTransactionalEmailAsync(email);
                    if (response != null && response.Messages != null && response.Messages.Length > 0 && !string.IsNullOrEmpty(response.Messages[0].Status) && response.Messages[0].Status.ToLower().Equals("success"))
                        flag = true;
                    #endregion
                }
                catch (Exception ex)
                {
                    flag = false;
                    _logging.WriteErrorToDB("MailManager", "JetMailSendWithAttachedFile", ex);
                }
            }
            return flag;
        }
        #endregion
    }
}



      
    

