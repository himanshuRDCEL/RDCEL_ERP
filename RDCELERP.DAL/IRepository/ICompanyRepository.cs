using RDCELERP.DAL.AbstractRepository;
using RDCELERP.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace RDCELERP.DAL.IRepository
{
   public  interface ICompanyRepository : IAbstractRepository<TblCompany>
    {
        public TblCompany GetByBUId(int id);
        public TblCompany GetCompanyByBUId(int? id);
        public TblCompany GetCompanyId(int? id);
        public TblCompany GetCompanyIdPairedtoBU(int? id);
    }
}
