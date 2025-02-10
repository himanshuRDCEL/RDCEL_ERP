using Mailjet.Client.TransactionalEmails;
using Mailjet.Client;
using Mailjet.Client.TransactionalEmails.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.TemplateConfiguration;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace RDCELERP.BAL.Interface
{
    public interface IMailManager
    {

        /// <summary>
        /// Method to send mail
        /// </summary>       
        /// <returns></returns>
        public void SendEmailLocally(string to, string body, string subject, byte[] quoteTemplate, string TemplateName);

        /// <summary>
        /// Send Email for deal steps
        /// </summary>
        /// <param name="to">To</param>
        /// <param name="body">Body</param>
        /// <param name="subject">Subject</param>
        public void SendEmail(string to, string body, string subject);
        public  Task<bool> SendEmailAsync(string to, string body, string subject);  
        public  Task<bool> SendEmailforevcAsync(string to, string body, string subject);
        public Task<bool> SendEmailforLGCAsync(string to, string body, string subject);

        public Task<TransactionalEmailResponse> JetMailSend(string to, string body, string subject);
        public Task<TransactionalEmailResponse> JetMailSendforevc(string to, string body, string subject);
        public Task<TransactionalEmailResponse> JetMailSendforLGC(string to, string body, string subject);

        public Task<bool> SendSMTPEmailAsync(string to, string body, string subject);

        #region Mail Send with Attatchment using JetMail Added By VK
        /// <summary>
        /// Mail Send with Attatchment using JetMail Added By VK
        /// </summary>
        /// <param name="to"></param>
        /// <param name="body"></param>
        /// <param name="subject"></param>
        /// <param name="templateVM"></param>
        /// <returns></returns>
        public Task<bool> JetMailSendWithAttachment(TemplateViewModel templateVM);
        #endregion

        #region Mail Send with Attatchment using JetMail Added By VK
        public Task<bool> JetMailSendWithAttachedFile(TemplateViewModel templateVM);
        #endregion
    }
}
