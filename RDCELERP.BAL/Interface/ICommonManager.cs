using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Common.Constant;
using RDCELERP.Common.Enums;
using RDCELERP.Common.Helper;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.APICalls;
using RDCELERP.Model.CommonModel;
using RDCELERP.Model.Company;
using RDCELERP.Model.EVC;
using RDCELERP.Model.LGC;
using RDCELERP.Model.Master;
using RDCELERP.Model.Users;

namespace RDCELERP.BAL.Interface
{
    public interface ICommonManager
    {

        /// <summary>
        /// Method to get the all access list
        /// </summary>       
        /// <returns>AccessListViewModel</returns>
        public IList<AccessListViewModel> GetAllAccessList(int roleid);
        public List<OrderImageUploadViewModel> GetOrderImagesByTransIdAndLoVId(int? orderTransId, int? LoVId);
        public List<OrderImageUploadViewModel> GetOrderImagesByTransId(int? orderTransId);
        public void InsertExchangeAbbstatusHistory(TblExchangeAbbstatusHistory tblExchangeAbbstatusHistorys);
        public string GetTemplate(string Templatename);

        
        public int CalculateEVCPrice(int OrderTransId);

        public TblSelfQc GetDeleteSelfQCVideo(string regdno);
        public MapOrderTransModel MapOrderTransData(TblOrderTran tblOrderTran);
        
        public bool CalculateDriverIncentive(int OrderTransId);
        public bool CheckUpiisRequired(int ordertransid);
        public List<PriceMasterNameViewModel> GetAllPriceMasterNameList();

        #region Check Business Unit Configuration for All
        /// <summary>
        /// Check Business Unit Configuration for Debit Note
        /// </summary>
        /// <param name="orderTransId"></param>
        /// <returns></returns>
        public bool CheckBUCongigByKey(int orderTransId, string configKey);
        #endregion

        #region Get Brand Details for AbbRedemption by Id from Different Tables
        /// <summary>
        /// Get Brand Details for AbbRedemption by Id from Different Tables
        /// </summary>
        /// <param name="IsBuMultiBrand"></param>
        /// <param name="NewBrandId"></param>
        /// <returns></returns>
        public BrandViewModel GetAbbBrandDetailsById(bool? IsBuMultiBrand, int? NewBrandId);
        #endregion

        public EVCClearBalanceViewModel? CalculateEVCClearBalance(int EVCRegistrationId);
        public IList<string> CheckCityisValid(string cityname, int StateId);
        public IList<string> CheckStateisValid(string Statename);
        public IList<string> CheckProdcatValid(string Prodcat);
        public IList<string> CheckProdTypeValid(string ProdType);
        public IList<string> CheckBrandValid(string Brand);
        //public TblPinCode CheckPincodeValid(int Pincode);
        public TblPinCode CheckPincodeValid(int Pincode, string Editcityname, string Editstatename);
        public TblAreaLocality CheckArealocaityValid(string Arealocality);

        #region Methods for EVC Price Calculations Algo New Added by VK
        #region Calculate EVC Price with Detailed add by VK
        /// <summary>
        /// Calculate EVC Price with Detailed add by VK
        /// </summary>
        /// <param name="tblOrderTran"></param>
        /// <param name="IsSweetenerAmtInclude"></param>
        /// <param name="GstTypeId"></param>
        /// <returns></returns>
        public EVCPriceViewModel CalculateEVCPriceDetailed(TblOrderTran? tblOrderTran, bool? IsSweetenerAmtInclude, int? GstTypeId = null);
        #endregion
        #region Calculate the EVC Price
        /// <summary>
        /// Methods for EVC Price Calculations Algo New Added by VK
        /// </summary>
        /// <param name="OrderTransId"></param>
        /// <param name="IsSweetenerAmtInclude"></param>
        /// <param name="GstTypeId"></param>
        /// <returns></returns>
        public int CalculateEVCPriceNew(int OrderTransId, bool? IsSweetenerAmtInclude, int? GstTypeId = null);
        #endregion

        #region Get Gst Amount by Base Price and Gst Type Id
        /// <summary>
        /// Get Gst Amount by Base Price and Gst Type Id
        /// </summary>
        /// <param name="baseAmt"></param>
        /// <param name="gstTypeId"></param>
        /// <returns></returns>
        public decimal GetGstAmount(decimal baseAmt, int? gstTypeId);
        #endregion

        #region Get List of EVC Having Clear Balance (EVC Price Change Algo) Added by VK
        /// <summary>
        ///  Get List of EVC Having Clear Balance (EVC Price Change Algo) Added by VK
        /// </summary>
        /// <param name="orderTransId"></param>
        /// <param name="evcPartnerList"></param>
        /// <returns></returns>
        public List<TblEvcPartner> GetEVCPartnerListHavingClearBalance(int orderTransId, List<TblEvcPartner> evcPartnerList);
        #endregion

        #region Get LGC Cost by Product Type, Product Category Id and BUId
        /// <summary>
        /// Get LGC Cost by Product Type, Product Category Id and BUId
        /// </summary>
        /// <param name="prodTypeId"></param>
        /// <param name="prodCatId"></param>
        /// <param name="buid"></param>
        /// <returns></returns>
        public int GetLGCCost(int? prodTypeId, int? prodCatId, int? buid);
        #endregion

        #region Get UTC Cost by PriceRange and Buid
        /// <summary>
        /// Get UTC Cost by PriceRange and Buid
        /// </summary>
        /// <param name="priceRange"></param>
        /// <param name="buid"></param>
        /// <returns></returns>
        public decimal GetUTCCost(int priceRange, int? buid);
        #endregion
        #endregion

        public void SaveApiCallInfo(ApicallViewModel apicallTableViewModel);
    }
}
