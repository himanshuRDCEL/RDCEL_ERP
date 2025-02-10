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
    public class ExchangeOrderRepository : AbstractRepository<TblExchangeOrder>, IExchangeOrderRepository
    {
        Digi2l_DevContext _context;
        public ExchangeOrderRepository(Digi2l_DevContext dbContext)
         : base(dbContext)
        {
            _context = dbContext;
        }

        /// <summary>
        /// Method to get the exchange order by id 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public TblExchangeOrder GetSingleOrder(int Id)
        {
            TblExchangeOrder TblExchangeOrder = _context.TblExchangeOrders
                        .Include(x => x.Status)
                        .Include(x => x.Brand)
                        .Include(x => x.BusinessPartner).ThenInclude(x => x.BusinessUnit).ThenInclude(X => X.Login)
                        .Include(x => x.CustomerDetails)
                        .Include(x => x.ProductType).ThenInclude(x => x.ProductCat)
                        .Include(x => x.CustomerDetails).FirstOrDefault(x => x.IsActive == true && x.Id == Id);


            return TblExchangeOrder;
        }

        /// <summary>
        /// Method to get the order price list by type id and price code
        /// </summary>
        /// <param name="typeId">typeId</param>
        /// <param name="priceCode">priceCode</param>
        /// <returns>List of TblPriceMaster</returns>
        public TblPriceMaster GetOrderPrices(int? typeId, string priceCode, string companyName)
        {
            TblPriceMaster priceMaster = null;
            if (!string.IsNullOrEmpty(priceCode))
            {
                priceMaster = _context.TblPriceMasters.FirstOrDefault(x => x.ProductTypeId == typeId
               && x.ExchPriceCode == priceCode);
            }
            else if (!string.IsNullOrEmpty(companyName))
            {
                TblBusinessUnit businessUnit = _context.TblBusinessUnits.Include(i => i.Login).FirstOrDefault(x => x.Name.ToLower().Equals(companyName.ToLower()));
                if (businessUnit != null)
                {
                    priceMaster = _context.TblPriceMasters.FirstOrDefault(x => x.ProductTypeId == typeId
                       && x.ExchPriceCode == businessUnit.Login.PriceCode);
                }

            }
            return priceMaster;
        }

        /// <summary>
        /// Method to get the price for EVC by type id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public TblEvcPriceMaster GetOrderEVCPrices(int? typeid)
        {
            TblEvcPriceMaster eVCPricemaster = _context.TblEvcPriceMasters.FirstOrDefault(x => x.ProductTypeId == typeid);
            return eVCPricemaster;
        }

        /// <summary>
        /// Method to get regdno
        /// </summary>
        /// <param name="regdno"></param>
        /// <returns></returns>
        public TblExchangeOrder GetRegdNo(string regdno)
        {
            TblExchangeOrder TblExchangeOrder = _context.TblExchangeOrders.Include(x => x.BusinessPartner).ThenInclude(x => x.BusinessUnit).ThenInclude(x => x.Login).FirstOrDefault(x => x.RegdNo == regdno && x.IsActive == true);
            
            return TblExchangeOrder;
        }

        /// <summary>
        /// Method to get Exchange details
        /// </summary>
        /// <param name="regdno"></param>
        /// <returns></returns>
        public TblExchangeOrder GetAllExchangeList()
        {
            TblExchangeOrder TblExchangeOrder = _context.TblExchangeOrders.FirstOrDefault(x => x.IsActive == true);

            return TblExchangeOrder;
        }
        //Code to get list of count for Total orders 
        public List<TblExchangeOrder> GetOrderCount(string AssociateCode, string CompanyName)
        {
            List<TblExchangeOrder> exchangelist = null;

            if (AssociateCode != null && CompanyName != null)
            {
                exchangelist = _context.TblExchangeOrders.Include(x => x.BusinessPartner).Where(x => x.BusinessPartner.AssociateCode == AssociateCode && x.CompanyName == CompanyName).ToList();
            }

            return exchangelist;
        }
        //Code to get list of count for voucher Redeemed count
        public List<TblExchangeOrder> GetVoucherRedeemedCount(string AssociateCode, string CompanyName)
        {
            List<TblExchangeOrder> exchangelist = null;

            if (AssociateCode != null)
            {
                exchangelist = _context.TblExchangeOrders.Include(x => x.BusinessPartner).Where(x => x.BusinessPartner.AssociateCode == AssociateCode && x.IsVoucherused == true && x.CompanyName == CompanyName).ToList();
            }

            return exchangelist;
        }
        //Code to get list of count for voucher issued count for dealer dashboard
        public List<TblExchangeOrder> GetVoucherIssuedCount(string AssociateCode, string CompanyName)
        {
            List<TblExchangeOrder> exchangelist = null;

            if (AssociateCode != null)
            {
                exchangelist = _context.TblExchangeOrders.Include(x => x.BusinessPartner).Where(x => x.BusinessPartner.AssociateCode == AssociateCode && x.VoucherCode != null && x.CompanyName == CompanyName).ToList();
            }
            return exchangelist;
        }


        //Code to get exxchange order details for dealer dashboard
        public List<TblExchangeOrder> GetOrderDetailsForDashBoard(DateTime startdate, DateTime enddate, int bpid, string CompanyName)
        {
            List<TblExchangeOrder> exchangeList = null;
            if (bpid > 0 && startdate.Year > 0 && enddate.Year > 0)
            {
                exchangeList = _context.TblExchangeOrders
                    .Include(x => x.ProductType).ThenInclude(x => x.ProductCat)
                    .Include(x => x.TblVoucherVerfications)
                    .Include(x => x.CustomerDetails)
                    .Where(x => x.CreatedDate >= startdate && x.CreatedDate <= enddate && x.BusinessPartnerId == bpid && x.ProductType.Id == x.ProductTypeId && x.ProductType.ProductCat.Id == x.ProductType.ProductCatId && x.CustomerDetails.Id == x.CustomerDetailsId && x.CompanyName == CompanyName && x.IsActive == true).OrderByDescending(x => x.Id).ToList();
            }

            return exchangeList;
        }


        public List<TblExchangeOrder> GetOrderDetailsForDealerDashBoard(DateTime? startdate, DateTime? enddate, int? businessPartnerId, string CompanyName, string AssociateCode, int? BusinessUnitId, string? state, string? city, string? userRole)
        {
            List<TblExchangeOrder> exchangeList = null;

            if (BusinessUnitId > 0 && userRole == "Super Admin")
            {
                exchangeList = _context.TblExchangeOrders
                    .Include(x => x.ProductType).ThenInclude(x => x.ProductCat)
                    .Include(x => x.TblVoucherVerfications)
                    .Include(x => x.CustomerDetails)
                    .Include(x => x.BusinessPartner)
                    .Where(x => x.IsActive == true && ((startdate == null && enddate == null) || (x.CreatedDate >= startdate && x.CreatedDate <= enddate))
                        && x.BusinessUnitId == BusinessUnitId && (businessPartnerId == null || x.BusinessPartner.BusinessPartnerId == businessPartnerId)
                        && x.CompanyName != null && x.CompanyName == CompanyName
                        && (state == null || x.BusinessPartner.State == state) && (city == null || x.BusinessPartner.City == city))
                    .OrderByDescending(x => x.Id).ToList();
            }
            else if (BusinessUnitId > 0)
            {
                exchangeList = _context.TblExchangeOrders
                    .Include(x => x.ProductType).ThenInclude(x => x.ProductCat)
                    .Include(x => x.TblVoucherVerfications)
                    .Include(x => x.CustomerDetails)
                    .Include(x => x.BusinessPartner)
                    .Where(x => x.IsActive == true && ((startdate == null && enddate == null) || (x.CreatedDate >= startdate && x.CreatedDate <= enddate))
                        && x.BusinessUnitId == BusinessUnitId && (businessPartnerId == null || x.BusinessPartner.BusinessPartnerId == businessPartnerId)
                        && x.CompanyName != null && x.CompanyName == CompanyName && x.BusinessPartner.AssociateCode != null && x.BusinessPartner.AssociateCode == AssociateCode
                        && (state == null || x.BusinessPartner.State == state) && (city == null || x.BusinessPartner.City == city))
                    .OrderByDescending(x => x.Id).ToList();
            }
            else
            {
                exchangeList = _context.TblExchangeOrders
                    .Include(x => x.ProductType).ThenInclude(x => x.ProductCat)
                    .Include(x => x.TblVoucherVerfications)
                    .Include(x => x.CustomerDetails)
                    .Include(x => x.BusinessPartner)
                    .Where(x => x.IsActive == true && ((startdate == null && enddate == null) || (x.CreatedDate >= startdate && x.CreatedDate <= enddate))
                     && (businessPartnerId == null || x.BusinessPartner.BusinessPartnerId == businessPartnerId)
                     && (state == null || x.BusinessPartner.State == state) && (city == null || x.BusinessPartner.City == city))
                    .OrderByDescending(x => x.Id).ToList();
            }

            return exchangeList;
        }


        public List<TblExchangeOrder> GetOrderDataforsingeldealer(string AssociateCode, string CompanyName)
        {
            List<TblExchangeOrder> exchangeList = null;
            if (AssociateCode != null)
            {
                exchangeList = _context.TblExchangeOrders
               .Include(x => x.ProductType).ThenInclude(x => x.ProductCat)
               .Include(x => x.TblVoucherVerfications)
               .Include(x => x.CustomerDetails)
               .Include(x => x.BusinessPartner)
               .Where(x => x.CompanyName != null && x.CompanyName == CompanyName && x.BusinessPartner.AssociateCode != null && x.BusinessPartner.AssociateCode == AssociateCode && x.IsActive == true).OrderByDescending(x => x.Id).ToList();

            }
            return exchangeList;
        }

        public List<TblExchangeOrder> GetOrderDataforsingeldealerToExport(string AssociateCode)
        {
            List<TblExchangeOrder> exchangeList = null;
            if (AssociateCode != null)
            {
                exchangeList = _context.TblExchangeOrders
               .Include(x => x.ProductType).ThenInclude(x => x.ProductCat)
               .Include(x => x.TblVoucherVerfications)
               .Include(x => x.CustomerDetails)
               .Include(x => x.BusinessPartner)
               .Where(x => x.BusinessPartner.AssociateCode != null && x.BusinessPartner.AssociateCode == AssociateCode && x.IsActive == true).OrderByDescending(x => x.CreatedDate).ToList();

            }
            return exchangeList;
        }
        public TblExchangeOrder GetExchangeorder(int? exchangeid)
        {
            TblExchangeOrder TblExchangeOrder = _context.TblExchangeOrders.FirstOrDefault(x => x.IsActive == true && x.Id == exchangeid);

            return TblExchangeOrder;
        }
        public int UpdateExchangeRecordStatus(string RegNo, int? status, int? userid)
        {
            int result = 0;
            TblExchangeOrder exchangeObj = _context.TblExchangeOrders.FirstOrDefault(x => x.IsActive == true && x.RegdNo == RegNo);
            if (exchangeObj != null)
            {
                exchangeObj.StatusId = status;
                exchangeObj.ModifiedBy = userid;
                exchangeObj.ModifiedDate = DateTime.Now;
                _context.Update(exchangeObj);
                _context.SaveChanges();
                result = 1;
            }
            return result;
        }

        public List<TblExchangeOrder> GetOrderDetailsForBUDashboard(int? BusinessUnitId, string RegdNo, string SponsorOrderNumber, string PhoneNumber, string ReferenceId, string Storecode,string VoucherStatus,string LatestStatusDescription,string OrderId)
        {
            List<TblExchangeOrder> ExchangeList = new List<TblExchangeOrder>();
            if (BusinessUnitId > 0)
            {
                ExchangeList = _context.TblExchangeOrders.Include(x => x.ProductType)
                     .ThenInclude(x => x.ProductCat)
                     .Include(x => x.BusinessPartner)
                     .Include(x => x.CustomerDetails)
                     .Include(x => x.Status)
                     .Include(x => x.TblOrderTrans)
                     .Include(x => x.TblOrderTrans).ThenInclude(x => x.TblOrderLgcs)
                     .Include(x => x.TblOrderTrans).ThenInclude(x => x.TblOrderQcs)
                     .Include(x => x.TblVoucherVerfications).Where(x => x.IsActive == true && x.StatusId != null && x.CustomerDetailsId != null && x.BusinessPartner.BusinessUnitId == BusinessUnitId && (string.IsNullOrEmpty(RegdNo) || x.RegdNo == RegdNo) && (string.IsNullOrEmpty(PhoneNumber) || x.CustomerDetails.PhoneNumber == PhoneNumber) && (string.IsNullOrEmpty(SponsorOrderNumber) || x.SponsorOrderNumber == SponsorOrderNumber) && (string.IsNullOrEmpty(ReferenceId) || x.SponsorServiceRefId == ReferenceId) && (string.IsNullOrEmpty(Storecode) || x.StoreCode == Storecode) && (string.IsNullOrEmpty(VoucherStatus) || x.VoucherStatus.VoucherStatusName == VoucherStatus) && (string.IsNullOrEmpty(LatestStatusDescription) || x.Status.StatusDescription == LatestStatusDescription) && (string.IsNullOrEmpty(OrderId) || x.Id == Convert.ToInt32(OrderId))).OrderByDescending(x => x.Id).ToList();
            }
            else
            {
                ExchangeList = _context.TblExchangeOrders.Include(x => x.ProductType)
                     .ThenInclude(x => x.ProductCat)
                     .Include(x => x.BusinessPartner)
                     .Include(x => x.CustomerDetails)
                     .Include(x => x.Status)
                     .Include(x => x.TblOrderTrans)
                     .Include(x => x.TblOrderTrans).ThenInclude(x => x.TblOrderLgcs)
                     .Include(x => x.TblOrderTrans).ThenInclude(x => x.TblOrderQcs)
                     .Include(x => x.TblVoucherVerfications).Where(x => x.IsActive == true && x.StatusId != null && x.CustomerDetailsId != null && x.BusinessPartner.BusinessUnitId != null && (string.IsNullOrEmpty(RegdNo) || x.RegdNo == RegdNo) && (string.IsNullOrEmpty(PhoneNumber) || x.CustomerDetails.PhoneNumber == PhoneNumber) && (string.IsNullOrEmpty(SponsorOrderNumber) || x.SponsorOrderNumber == SponsorOrderNumber) && (string.IsNullOrEmpty(ReferenceId) || x.SponsorServiceRefId == ReferenceId) && (string.IsNullOrEmpty(Storecode) || x.StoreCode == Storecode) && (string.IsNullOrEmpty(VoucherStatus) || x.VoucherStatus.VoucherStatusName == VoucherStatus) && (string.IsNullOrEmpty(LatestStatusDescription) || x.Status.StatusDescription == LatestStatusDescription) && (string.IsNullOrEmpty(OrderId) || x.Id == Convert.ToInt32(OrderId))).OrderByDescending(x => x.Id).ToList();

            }
            return ExchangeList;
        }

        public List<TblExchangeOrder> GetOrderCountBU(int BusinessUnitId)
        {
            List<TblExchangeOrder> exchangelist = new List<TblExchangeOrder>();
            if (BusinessUnitId > 0)
            {
                exchangelist = _context.TblExchangeOrders
                    .Include(x=>x.BusinessPartner)
                    .Where(x => x.IsActive == true && x.CompanyName != null && x.BusinessPartnerId != null && x.BusinessPartner.BusinessUnitId == BusinessUnitId).ToList();
            }
            else
            {
                exchangelist = _context.TblExchangeOrders
                    .Include(x => x.BusinessPartner)
                    .Where(x => x.IsActive == true && x.CompanyName != null && x.BusinessPartnerId != null).ToList();
            }
            return exchangelist;
        }

        public List<TblExchangeOrder> GetVoucherIssuedCountBU(int BusinessUnitId)
        {
            List<TblExchangeOrder> exchangelist = new List<TblExchangeOrder>();
            if (BusinessUnitId > 0)
            {
                exchangelist = _context.TblExchangeOrders
                    .Include(x => x.BusinessPartner)
                    .Where(x => x.IsActive == true && x.CompanyName != null && x.BusinessPartnerId != null && x.BusinessPartner.BusinessUnitId == BusinessUnitId && x.VoucherCode!=null ).ToList();
            }
            else
            {
                exchangelist = _context.TblExchangeOrders
                    .Include(x => x.BusinessPartner)
                    .Where(x => x.IsActive == true && x.CompanyName != null && x.BusinessPartnerId != null && x.VoucherCode != null).ToList();
            }
            return exchangelist;
        }

        public List<TblExchangeOrder> GetVoucherredeemedCountBU(int BusinessUnitId)
        {
            List<TblExchangeOrder> exchangelist = new List<TblExchangeOrder>();
            if (BusinessUnitId > 0)
            {
                exchangelist = _context.TblExchangeOrders
                 .Include(x => x.BusinessPartner)
                 .Where(x => x.IsActive == true && x.CompanyName != null && x.BusinessPartnerId != null && x.VoucherCode != null && x.IsVoucherused==true && x.BusinessPartner.BusinessUnitId==BusinessUnitId).ToList();
            }
            else
            {
                exchangelist = _context.TblExchangeOrders
                 .Include(x => x.BusinessPartner)
                 .Where(x => x.IsActive == true && x.CompanyName != null && x.BusinessPartnerId != null && x.VoucherCode != null && x.IsVoucherused == true).ToList();
            }
            return exchangelist;
        }

        public List<TblExchangeOrder> GetCompletedOrderBU(int BusinessUnitId)
        {
            List<TblExchangeOrder> exchangelist = new List<TblExchangeOrder>();
            if (BusinessUnitId > 0)
            {
                exchangelist = _context.TblExchangeOrders
                 .Include(x => x.BusinessPartner)
                 .Where(x => x.IsActive == true && x.CompanyName != null && x.BusinessPartnerId != null && x.BusinessPartner.BusinessUnitId == BusinessUnitId && (x.StatusId == 30 || x.StatusId == 32)).ToList();
            }
            else
            {
                exchangelist = _context.TblExchangeOrders
                 .Include(x => x.BusinessPartner)
                 .Where(x => x.IsActive == true && x.CompanyName != null && x.BusinessPartnerId != null && (x.StatusId == 30 || x.StatusId == 32)).ToList();
            }
            return exchangelist;
        }

        public List<TblExchangeOrder> GetCancelledOrderBu(int BusinessUnitId)
        {
            List<TblExchangeOrder> exchangelist = new List<TblExchangeOrder>();
            if (BusinessUnitId > 0)
            {
                exchangelist = _context.TblExchangeOrders
                 .Include(x => x.BusinessPartner)
                 .Where(x => x.IsActive == true && x.CompanyName != null && x.BusinessPartnerId != null  && x.BusinessPartner.BusinessUnitId == BusinessUnitId && (x.StatusId == 6 || x.StatusId == 16)).ToList();
            }
            else
            {
                exchangelist = _context.TblExchangeOrders
                 .Include(x => x.BusinessPartner)
                 .Where(x => x.IsActive == true && x.CompanyName != null && x.BusinessPartnerId != null  && (x.StatusId == 6 || x.StatusId == 16)).ToList();
            }
            return exchangelist;
        }
        /// <summary>
        /// Method to Get ExchOrder By RegdNo
        /// </summary>
        /// <param name="regdno"></param>
        /// <returns></returns>
        public TblExchangeOrder GetExchOrderByRegdNo(string regdno)
        {
            TblExchangeOrder TblExchangeOrder = _context.TblExchangeOrders.FirstOrDefault(x => x.IsActive == true && x.RegdNo == regdno);
            
            return TblExchangeOrder;
        }

        #region Check Sponsor Order Number Is Exist or not
        /// <summary>
        /// Check Sponsor Order Number Is Exist or not
        /// </summary>
        /// <param name="sponsorOrderNumber"></param>
        /// <returns></returns>
        public bool CheckSponsorOrderNumber(string? sponsorOrderNumber)
        {
            bool flag = false;
            if (!string.IsNullOrEmpty(sponsorOrderNumber))
            {
                flag = _context.TblExchangeOrders.ToList().Exists(x => x.SponsorOrderNumber == sponsorOrderNumber);
            }
            return flag;
        }
        #endregion
    }
}
