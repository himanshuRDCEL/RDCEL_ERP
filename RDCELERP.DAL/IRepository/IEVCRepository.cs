using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.DAL.AbstractRepository;
using RDCELERP.DAL.Entities;
//using RDCELERP.DAL.Models;
//using TblEvcregistration = RDCELERP.DAL.Models.TblEvcregistration;

namespace RDCELERP.DAL.IRepository
{
    public interface IEVCRepository : IAbstractRepository<TblEvcregistration>
    {
        List<TblEvcregistration> GetEVCList(string? StateName, string? CityName, string? Pin);
        TblEvcregistration GetEVCDetailsById(int EvcRegistrationId);
        List<TblEvcregistration> GetEVCListforEVCReassign(string StateName, string CityName, string Pin, int EVCRegId);
    }
}
