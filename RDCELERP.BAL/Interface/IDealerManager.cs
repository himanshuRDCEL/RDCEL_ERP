using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.DealerDashBoard;

namespace RDCELERP.BAL.Interface
{
    public interface IDealerManager
    {
        public OrderCountViewModel GetOrderCount(string AssociateCode,string SelectedCompanyName,string RoleName,string UserComapny);

        public CityListModel GetCityList(string state,int buid,string AssociateCode,string userCompany,string userRole);

        public StoreListModel GetStoreList(int buid,string AssociateCode,string City,string UserComapny ,string userRole);


        public DealerDashboardViewModel GetOrderList(int BusinesspartnerId, string startdate, string enddate);

        public List<DealerDashboardViewModel> GetDashboardList(ExchangeOrderDataContract exchanegDC,int skip,int pageSize);

        public List<DealerDashboardViewModel> ExportDealerdata(string AssociateCode);
        public StateListModel GetStateList(int buid,string AssociateCode,string UserCompany);

        public CompanyList GetCompanyList();

        public OrderCountViewModel GetOrderCountsForBU(int BusinessUnitId);
    
    }
}
