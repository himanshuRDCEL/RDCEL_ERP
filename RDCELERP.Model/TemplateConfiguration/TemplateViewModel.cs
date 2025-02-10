using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.TemplateConfiguration
{
    public class TemplateViewModel : BaseViewModel
    {
        public int Id { get; set; }
        public string? To { get; set; }
        public string? Cc { get; set; }
        public string? Bcc { get; set; }
        public string? Subject { get; set; }
        public string? HtmlBody { get; set; }
        public bool? IsCertificateGenerated { get; set; }
        public bool? IsInvoiceGenerated { get; set; }

        // Certificate Attachment
        public string? AttachmentFileName { get; set; }
        public string? AttachmentFilePath { get; set; }
        public string? FileNameWithPath { get; set; }

        // Invoice Attachment
        public string? InvAttachFileName { get; set; }
        public string? InvAttachFilePath { get; set; }
        public string? InvFileNameWithPath { get; set; }

        //Reporting Mail
        public string? AttachFileNameExch { get; set; }
        public string? AttachFileNameAbb { get; set; }
        public byte[]? byteArrayExch { get; set; }
        public byte[]? byteArrayAbb { get; set; }
    }
}
