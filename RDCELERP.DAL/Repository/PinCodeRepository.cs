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
    public class PinCodeRepository : AbstractRepository<TblPinCode>, IPinCodeRepository
    {
        Digi2l_DevContext _dbContext;
        public PinCodeRepository(Digi2l_DevContext dbContext)
      : base(dbContext)
        {
            _dbContext = dbContext;
        }
        /// <summary>
        /// Get Pincode details by Pincode
        /// </summary>
        /// <param name="pincode"></param>
        /// <returns></returns>
        public TblPinCode GetByPincode(int pincode)
        {
            TblPinCode? tblPinCode = _dbContext.TblPinCodes
                .Where(x => x.IsActive == true && x.ZipCode == pincode)
                .FirstOrDefault();
            return tblPinCode;
        }
        public TblPinCode GetPincodebycity(int pincode, string Editcityname, string Editstatename)
        {
            TblPinCode? tblPinCode = null;
            if (pincode > 0 && !string.IsNullOrEmpty(Editcityname) && !string.IsNullOrEmpty(Editstatename))
            {
                tblPinCode = _dbContext.TblPinCodes.Where(x => x.IsActive == true && x.ZipCode == pincode && x.Location == Editcityname && x.State == Editstatename).FirstOrDefault();
            }
             
            return tblPinCode;
        }
    }
}
