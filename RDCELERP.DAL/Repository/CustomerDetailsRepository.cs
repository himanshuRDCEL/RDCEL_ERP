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
    public class CustomerDetailsRepository : AbstractRepository<TblCustomerDetail>, ICustomerDetailsRepository
    {
        Digi2l_DevContext _context;
        public CustomerDetailsRepository(Digi2l_DevContext dbContext)
         : base(dbContext)
        {
            _context = dbContext;
        }

        public TblCustomerDetail GetCustDetails(int? Id)
        {
            TblCustomerDetail TblCustomerDetail = null;
            if (Id > 0)
            {
                TblCustomerDetail = _context.TblCustomerDetails.FirstOrDefault(x => x.IsActive == true && x.Id == Id);
            }
           
            return TblCustomerDetail;
        }
    }
}
