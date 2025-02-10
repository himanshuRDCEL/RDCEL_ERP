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
//using RDCELERP.DAL.Models;

namespace RDCELERP.DAL.Repository
{
    public class EVCRepository : AbstractRepository<Entities.TblEvcregistration>, IEVCRepository
    {
        private readonly Digi2l_DevContext _dbContext;
        public EVCRepository(Entities.Digi2l_DevContext dbContext)
         : base(dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Metod to get the list of EVC as per the loaction avaiable
        /// </summary>
        /// <param name="cityName">cityName</param>
        /// <returns>List of TblEvcregistration</returns>
        public List<TblEvcregistration> GetEVCList(string StateName,string  CityName,string Pin)
        {
            List<TblEvcregistration> evcregistrations = _dbContext.TblEvcregistrations
                              .Include(x => x.TblWalletTransactions)
                              .Include(x => x.City)
                              .Include(x => x.State)
                              .Where(x => x.IsActive == true                              
                              && x.Isevcapprovrd == true
                              && ((x.State != null && x.State.Name.ToLower().Equals(StateName.ToLower()))
                              || (x.City != null && x.City.Name.ToLower().Equals(CityName.ToLower()))
                              || (x.PinCode == Pin))
                              )
                              .ToList();
            //List<TblEvcregistration> evcregistrations = _dbContext.TblEvcregistrations
            //               .Include(x => x.TblWalletTransactions)
            //               .Include(x => x.City)
            //               .Include(x => x.State)
            //               .Where(x => x.IsActive == true
            //            && x.Isevcapprovrd == true
            //            && (x.State.Name.ToLower().Equals(StateName.ToLower())
            //|| (x.City.Name.ToLower().Equals(CityName.ToLower()))
            //|| (x.PinCode == Pin))
            //               ).ToList();

            return evcregistrations;
        }
        /// <summary>
        /// Metod to get the list of EVC as per the loaction avaiable
        /// </summary>
        /// <param name="cityName">cityName</param>
        /// <returns>List of TblEvcregistration</returns>
        public List<TblEvcregistration> GetEVCListforEVCReassign(string StateName, string CityName, string Pin,int EVCRegId)
        {
            List<TblEvcregistration> evcregistrations = _dbContext.TblEvcregistrations
                           .Include(x => x.TblWalletTransactions)
                           .Include(x => x.City)
                           .Include(x => x.State)
                           .Where(x => x.IsActive == true
                           && x.EvcregistrationId!= EVCRegId
                        && x.Isevcapprovrd == true
                         && (x.State.Name.ToLower().Equals(StateName.ToLower()) || x.City.Name.ToLower().Equals(CityName.ToLower()) || x.PinCode == Pin)
                           ).ToList();
            //|| x.EvcwalletAmount > 0
            return evcregistrations;
        }
        public TblEvcregistration GetEVCDetailsById(int EvcRegistrationId)
        {
            TblEvcregistration tblEvcregistration = _dbContext.TblEvcregistrations
                           .Include(x => x.City)
                           .Include(x => x.State)
                           .Where(x => x.IsActive == true
                        && x.Isevcapprovrd == true
                        && x.EvcregistrationId == EvcRegistrationId).FirstOrDefault();

            return tblEvcregistration;
        }

    }
}
