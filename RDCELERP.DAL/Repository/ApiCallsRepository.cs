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
    public class ApiCallsRepository : AbstractRepository<TblApicall>, IApiCallsRepository
    {
        Digi2l_DevContext _context;
        public ApiCallsRepository(Digi2l_DevContext dbContext)
         : base(dbContext)
        {
            _context = dbContext;
        }

        public void SaveApicall(TblApicall apicall)
        {
            if (apicall != null)
            {
                _context.TblApicalls.Add(apicall);
                _context.SaveChanges();
            }
        }

    }
}
