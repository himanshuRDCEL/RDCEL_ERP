using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.DAL.AbstractRepository;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;

namespace RDCELERP.DAL.Repository
{
    public class BUBasedSweetnerValidationRepository : AbstractRepository<TblBubasedSweetnerValidation>, IBUBasedSweetnerValidationRepository
    {
        private readonly Digi2l_DevContext _dbContext;
        public BUBasedSweetnerValidationRepository(Entities.Digi2l_DevContext dbContext)
         : base(dbContext)
        {
            _dbContext = dbContext;
        }       
        public List<TblBubasedSweetnerValidation> GetQustionList(int BusinessUnitId)
        {
            List<TblBubasedSweetnerValidation> tblBubasedSweetnerValidation = _dbContext.TblBubasedSweetnerValidations
                .Include(x => x.Question)
                .Where(x => x.IsActive == true && x.BusinessUnitId == BusinessUnitId).ToList();
            return tblBubasedSweetnerValidation;
        }
    }
}
