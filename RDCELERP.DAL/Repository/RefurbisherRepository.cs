using Microsoft.EntityFrameworkCore;
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
    public class RefurbisherRepository : AbstractRepository<TblRefurbisherRegistration>, IRefurbisherRepository
    {
        Digi2l_DevContext _context;
        public RefurbisherRepository(Digi2l_DevContext dbContext)
         : base(dbContext)
        {
            _context = dbContext;
        }
        public TblRefurbisherRegistration GetSingleOrder(int? Id)
        {
            TblRefurbisherRegistration TblRefurbisherRegistration = _context.TblRefurbisherRegistrations
                        .Include(x => x.City)
                        .Include(x => x.State).FirstOrDefault(x => x.IsActive == true && x.RefurbisherId == Id);

            return TblRefurbisherRegistration;
        }

    }
}
