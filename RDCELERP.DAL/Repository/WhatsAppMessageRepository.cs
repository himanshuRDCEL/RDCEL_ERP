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
    public class WhatsAppMessageRepository : AbstractRepository<TblWhatsAppMessage>, IWhatsAppMessageRepository
    {
        Digi2l_DevContext _context;
        public WhatsAppMessageRepository(Digi2l_DevContext dbContext)
           : base(dbContext)
        {
            _context = dbContext;
        }

        public TblWhatsAppMessage Getbytempname(string phoneno)
        {
            TblWhatsAppMessage TblWhatsAppMessage = null;
            if (phoneno != null)
            {
                TblWhatsAppMessage = _context.TblWhatsAppMessages.FirstOrDefault(x => x.IsActive == true && x.PhoneNumber == phoneno && x.TemplateName == "cust_upi_no");
            }                        
            return TblWhatsAppMessage;
        }

        public TblWhatsAppMessage GetbytempnameLast(string phoneno)
        {
            TblWhatsAppMessage TblWhatsAppMessage = null;
            if (phoneno != null)
            {
                TblWhatsAppMessage = _context.TblWhatsAppMessages.Where(x => x.IsActive == true && x.PhoneNumber == phoneno && x.TemplateName == "cust_upi_no").LastOrDefault();
            }
            return TblWhatsAppMessage;
        }

        public TblWhatsAppMessage Getbytempname(string phoneno, int Id)
        {
            TblWhatsAppMessage TblWhatsAppMessage = null;
            if (phoneno != null)
            {
                TblWhatsAppMessage = _context.TblWhatsAppMessages.FirstOrDefault(x => x.IsActive == true && x.PhoneNumber == phoneno && x.TemplateName == "cust_upi_no" && x.Id == Id);
            }
            return TblWhatsAppMessage;
        }
    }
}
