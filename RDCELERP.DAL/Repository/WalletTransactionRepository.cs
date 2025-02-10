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
    public class WalletTransactionRepository : AbstractRepository<TblWalletTransaction>, IWalletTransactionRepository
    {
        Digi2l_DevContext _context;
        public WalletTransactionRepository(Digi2l_DevContext dbContext)
          : base(dbContext)
        {
            _context = dbContext;
        }
        /// <summary>
        /// Method to get the WalletTransactio by orderid with all evcdetalis
        /// </summary>
        /// <param name="Id">Id</param>
        /// <returns>TblWalletTransaction</returns>
        public TblWalletTransaction GetSingleEvcDetails(int Id)
        {
            TblWalletTransaction tblWalletTransaction = new TblWalletTransaction();
            if (Id > 0)
            {
                tblWalletTransaction = _context.TblWalletTransactions.FirstOrDefault(x => x.IsActive == true && x.OrderTransId == Id);
                if (tblWalletTransaction != null)
                {
                    tblWalletTransaction.Evcregistration = _context.TblEvcregistrations
                               .Include(x => x.State)
                               .Include(x => x.City)
                               .Include(x=>x.TblEvcPartners).ThenInclude(x => x.State)
                               .Include(x => x.TblEvcPartners).ThenInclude(x => x.City)
                               .FirstOrDefault(x => x.IsActive == true &&x.Isevcapprovrd == true && x.EvcregistrationId == tblWalletTransaction.EvcregistrationId);
                    if (tblWalletTransaction.Evcregistration != null)
                    {
                        return tblWalletTransaction;
                    }
                    else
                    {
                        tblWalletTransaction = null;
                    }
                }
                
            }
            
            return tblWalletTransaction;
           
        }

        public int UpdateWalletTransRecordStatus(int transid, int? status, int? userid,int Ordertype)
        {
            int result = 0;
            TblWalletTransaction wallettramsactionObj = _context.TblWalletTransactions.FirstOrDefault(x => x.IsActive == true && x.OrderTransId == transid);
            if (wallettramsactionObj != null)
            {
                var typeOfOrder = Convert.ToInt32(wallettramsactionObj.OrderType);
                wallettramsactionObj.OrderOfInprogressDate = DateTime.Now;
                wallettramsactionObj.ModifiedDate = DateTime.Now;
                wallettramsactionObj.ModifiedBy = userid;
                wallettramsactionObj.OrderType = Ordertype.ToString();
                wallettramsactionObj.StatusId = status.ToString();
                _context.Update(wallettramsactionObj);
                _context.SaveChanges();
                result= 1;
            }
            return result;
        }

        public int UpdateWalletTransStatus(int transid, int? status, int? userid)
        {
            int result = 0;
            TblWalletTransaction wallettramsactionObj = _context.TblWalletTransactions.FirstOrDefault(x => x.IsActive == true && x.OrderTransId == transid);
            if (wallettramsactionObj != null)
            {                             
                wallettramsactionObj.ModifiedDate = DateTime.Now;
                wallettramsactionObj.ModifiedBy = userid;               
                wallettramsactionObj.StatusId = status.ToString();
                _context.Update(wallettramsactionObj);
                _context.SaveChanges();
                result = 1;
            }
            return result;
        }

        public TblWalletTransaction GetdatainOrdertransh(int Ordertransid) {
            TblWalletTransaction? tblWalletTransaction=null;
            tblWalletTransaction=_context.TblWalletTransactions.Where(x=>x.IsActive == true && x.OrderTransId == Ordertransid).FirstOrDefault();
            return tblWalletTransaction;
        }
    }
}
