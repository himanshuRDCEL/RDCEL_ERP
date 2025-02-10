using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.DAL.AbstractRepository;
using RDCELERP.DAL.Entities;

namespace RDCELERP.DAL.IRepository
{
    public interface IExchangeOrderRepository : IAbstractRepository<TblExchangeOrder>
    {
        /// <summary>
        /// Method to get the exchange order by id 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        TblExchangeOrder GetSingleOrder(int Id);

        /// <summary>
        /// Method to get the order price list by type id and price code
        /// </summary>
        /// <param name="typeId">typeId</param>
        /// <param name="priceCode">priceCode</param>
        /// <returns>List of TblPriceMaster</returns>
        TblPriceMaster GetOrderPrices(int? typeId, string priceCode, string companyName);

        /// <summary>
        /// Method to get the price for EVC by type id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        TblEvcPriceMaster GetOrderEVCPrices(int? typeid);

        /// <summary>
        /// Method to get rno details
        /// </summary>
        /// <param name="regdno"></param>
        /// <returns></returns>
        TblExchangeOrder GetRegdNo(string regdno);

        TblExchangeOrder GetAllExchangeList();

        //List<TblExchangeOrder> GetExchangeList();

        List<TblExchangeOrder> GetOrderCount(string AssociateCode,string CompanyName);

        ///<summary>
        ///Method to get Order Count For exchange order count where Voucher is Redeemed
        ///</summary>
        ///<param name="AssociateCode"></param>
        ///</summary>

        List<TblExchangeOrder> GetVoucherRedeemedCount(string AssociateCode, string CompanyName);

        ///<summary>
        ///Method to get Order Count For Issued Voucher 
        ///</summary>
        ///<param name="AssociateCode"></param>
        ///</summary>

        List<TblExchangeOrder> GetVoucherIssuedCount(string AssociateCode, string CompanyName);

        List<TblExchangeOrder> GetOrderDetailsForDashBoard(DateTime startdate, DateTime enddate, int bpid,string CompanyName);


        List<TblExchangeOrder> GetOrderDataforsingeldealer(string AssociateCode, string CompanyName);
        public List<TblExchangeOrder> GetOrderDataforsingeldealerToExport(string AssociateCode);

        TblExchangeOrder GetExchangeorder(int? exchangeid);
        public int UpdateExchangeRecordStatus(string RegNo, int? status, int? userid);

        List<TblExchangeOrder> GetOrderDetailsForBUDashboard(int? BusinessUnitId, string RegdNo, string SponsorOrderNumber, string PhoneNumber, string ReferenceId, string Storecode, string VoucherStatus, string LatestStatusDescription, string OrderId);

        List<TblExchangeOrder> GetOrderCountBU(int BusinessUnitId);
        List<TblExchangeOrder> GetVoucherIssuedCountBU(int BusinessUnitId);
        List<TblExchangeOrder> GetVoucherredeemedCountBU(int BusinessUnitId);
        List<TblExchangeOrder> GetCompletedOrderBU(int BusinessUnitId);
        List<TblExchangeOrder> GetCancelledOrderBu(int BusinessUnitId);

        /// <summary>
        /// Method to Get ExchOrder By RegdNo
        /// </summary>
        /// <param name="regdno"></param>
        /// <returns></returns>
        public TblExchangeOrder GetExchOrderByRegdNo(string regdno);

        #region Check Sponsor Order Number Is Exist or not
        /// <summary>
        /// Check Sponsor Order Number Is Exist or not
        /// </summary>
        /// <param name="sponsorOrderNumber"></param>
        /// <returns></returns>
        public bool CheckSponsorOrderNumber(string? sponsorOrderNumber);
        #endregion
        public List<TblExchangeOrder> GetOrderDetailsForDealerDashBoard(DateTime? startdate, DateTime? enddate, int? bpid, string CompanyName, string AssociateCode, int? BusinessUnitId, string? state, string? city, string? userRole);
    }
}
