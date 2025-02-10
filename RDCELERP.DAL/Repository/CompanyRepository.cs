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
   public  class CompanyRepository : AbstractRepository<TblCompany>, ICompanyRepository
    {
        Digi2l_DevContext _context;
        public CompanyRepository(Digi2l_DevContext dbContext)
         : base(dbContext)
        {
            _context = dbContext;
        }

        public TblCompany GetByBUId(int id)
        {
            TblCompany TblCompany = _context.TblCompanies.FirstOrDefault(x => x.BusinessUnitId == id);

            return TblCompany;
        }

        public TblCompany GetCompanyByBUId(int? BUId)
        {
            TblCompany TblCompany = _context.TblCompanies.FirstOrDefault(x => x.BusinessUnitId == BUId);

            return TblCompany;
        }
        public TblCompany GetCompanyId(int? id)
        {
            TblCompany TblCompany = _context.TblCompanies.FirstOrDefault(x => x.CompanyId == id);

            return TblCompany;
        }

        public TblCompany GetCompanyIdPairedtoBU(int? id)
        {
            TblCompany TblCompany = new TblCompany();
            if (id > 0)
            {
                TblCompany = _context.TblCompanies.Include(x => x.BusinessUnit).FirstOrDefault(x => x.CompanyId == id);
            }
            return TblCompany;
        }
    }
}
