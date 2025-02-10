using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.DAL.AbstractRepository;
using RDCELERP.DAL.Entities;

namespace RDCELERP.DAL.IRepository
{
    public interface IWhatsAppMessageRepository : IAbstractRepository<TblWhatsAppMessage>
    {
        TblWhatsAppMessage Getbytempname(string phoneno);
        TblWhatsAppMessage Getbytempname(string phoneno, int Id);
        public TblWhatsAppMessage GetbytempnameLast(string phoneno);
    }
}
