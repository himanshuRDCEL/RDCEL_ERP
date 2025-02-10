using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.DAL.AbstractRepository;
using RDCELERP.DAL.Entities;

namespace RDCELERP.DAL.IRepository
{
    public interface IAbbRegistrationRepository : IAbstractRepository<TblAbbregistration>
    {
        TblAbbregistration GetSingleOrder(int Id);

        TblAbbregistration GetRegdNo(string regdno);

        TblAbbregistration GetAllRegdno();
        //List<TblAbbregistration> GetAllList();
        public TblBusinessPartner GetStoreEmail(string email);

        List<TblAbbregistration> GetOrderCountForDashboard(int BusinessUnitId);
        List<TblAbbregistration> GetNotApprovedOrders(int BusinessUnitId);
        List<TblAbbregistration> GetApprovedOrders(int BusinessUnitId);
        List<TblAbbregistration> GetAllOrderList(int? BusinessUnitId, string regdNo, string SponsorOrderNumber, string PhoneNumber, string EmployeeId, string Abbstorecode);
    }
}
