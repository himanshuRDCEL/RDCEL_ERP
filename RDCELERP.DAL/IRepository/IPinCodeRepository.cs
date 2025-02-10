using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.DAL.AbstractRepository;
using RDCELERP.DAL.Entities;

namespace RDCELERP.DAL.IRepository
{
   public interface IPinCodeRepository : IAbstractRepository<TblPinCode>
    {
        public TblPinCode GetByPincode(int pincode);
        public TblPinCode GetPincodebycity(int pincode, string Editcityname, string Editstatename);
    }
}
