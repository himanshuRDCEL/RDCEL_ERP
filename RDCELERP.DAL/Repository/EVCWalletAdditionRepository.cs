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
    public class EVCWalletAdditionRepository : AbstractRepository<TblEvcwalletAddition>, IEVCWalletAdditionRepository
    {
        private readonly Digi2l_DevContext _dbContext;
        public EVCWalletAdditionRepository(Entities.Digi2l_DevContext dbContext)
         : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public List<TblEvcwalletAddition> GetEVCwalletCreaditDetailsById(int EvcRegistrationId)
        {
            List<TblEvcwalletAddition>? tblEvcwalletAddition = new List<TblEvcwalletAddition>();
             tblEvcwalletAddition = _dbContext.TblEvcwalletAdditions
                           .Include(x => x.EvcregistrationId)                           
                           .Where(x => x.IsActive == true                        
                        && x.EvcregistrationId == EvcRegistrationId && x.IsCreaditNote==true).ToList();

            return tblEvcwalletAddition;
        }
    }
}
