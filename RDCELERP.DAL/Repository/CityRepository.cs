using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using RDCELERP.Core.App.Models;
using RDCELERP.DAL.AbstractRepository;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;

namespace RDCELERP.DAL.Repository
{
    public class CityRepository : AbstractRepository<TblCity>, ICityRepository
    {
        private readonly Digi2l_DevContext _db;
        public CityRepository(Digi2l_DevContext dbContext)
       : base(dbContext)
        {
            _db = dbContext;
        }

        /// <summary>
        /// Method to get the city by name 
        /// </summary>
        /// <param name="Id">Id</param>
        /// <returns>TblCity</returns>
        public TblCity GetSingleCityByName(string name)
        {
            TblCity TblCity = _db.TblCities.FirstOrDefault(x => x.IsActive == true && x.Name.ToLower().Equals(name.ToLower()));
            return TblCity;
        }

        public TblCity GetCityById(int id)
        {
            TblCity tblCity = null;
            if(id > 0)
            {
                tblCity = _db.TblCities.FirstOrDefault(x => x.IsActive == true && x.CityId == id);
            }
            
            return tblCity;
        }
    }
}
