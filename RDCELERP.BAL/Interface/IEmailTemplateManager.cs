using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.BAL.Interface
{
    public interface IEmailTemplateManager
    {
        public bool CommonEmailBody(string too, string body, string subject);
    }
}
