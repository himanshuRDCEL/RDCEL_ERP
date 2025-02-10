using Mailjet.Client.TransactionalEmails.Response;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.BAL.Interface;
using RDCELERP.Model.Base;

namespace RDCELERP.BAL.MasterManager
{
    public class EmailTemplateManager : IEmailTemplateManager
    {
        #region variables
        public readonly IOptions<ApplicationSettings> _baseConfig;
        IMailManager _mailManager;
        #endregion
        public EmailTemplateManager(IOptions<ApplicationSettings> baseConfig, IMailManager mailManager)
        {
            _baseConfig = baseConfig;
            _mailManager = mailManager;
        }

        public Task<TransactionalEmailResponse> TransactionalEmailResponse { get;  set; }

        public bool CommonEmailBody(string too,string body,string subject)
        {
            var filename = "ERPEmailTemplate.html";
            var file = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "MailTemplates", filename);
            string Emailbody = File.ReadAllText(file);
            Emailbody = Emailbody.Replace("[TemplateBody]", body);
            TransactionalEmailResponse= _mailManager.JetMailSend(too, Emailbody, subject);

            return TransactionalEmailResponse.IsCompletedSuccessfully;
        }
    }
}
