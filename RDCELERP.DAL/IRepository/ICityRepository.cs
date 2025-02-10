using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.DAL.AbstractRepository;
using RDCELERP.DAL.Entities;

namespace RDCELERP.DAL.IRepository
{
    public interface ICityRepository : IAbstractRepository<TblCity>
    {

        /// <summary>
        /// Method to get the city by name 
        /// </summary>
        /// <param name="Id">Id</param>
        /// <returns>TblCity</returns>
        TblCity GetSingleCityByName(string name);
        TblCity GetCityById(int id);
    }
}
