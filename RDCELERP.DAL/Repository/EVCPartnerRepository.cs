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
    public class EVCPartnerRepository : AbstractRepository<TblEvcPartner>, IEVCPartnerRepository
    {
        Digi2l_DevContext _context;
        public EVCPartnerRepository(Digi2l_DevContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }


        #region method to get in-house evc partner list by pincode
        /// <summary>
        /// method to get in-house evc partner list by pincode
        /// </summary>
        /// <param name="custPincode"></param>
        /// <returns>tblEvcPartnerslist</returns>
        public List<TblEvcPartner> GetEvcPartnerListByPincode(string? custPincode, int? productCatId, string? productQuality,int ordertype)
        {
            List<TblEvcPartner>? tblEvcPartnerslist = null;
            List<TblEvcPartner>? EvcPartnersWithPreferancelist = new List<TblEvcPartner>();
            try
            {
                if (!string.IsNullOrEmpty(custPincode))
                {
                    tblEvcPartnerslist = _context.TblEvcPartners
                        .Include(x => x.Evcregistration)
                        .Where(x => x.IsActive == true && x.ListOfPincode.Contains(custPincode) && x.Evcregistration.IsInHouse == true && x.IsApprove == true
                        && x.Evcregistration.Isevcapprovrd == true && x.Evcregistration.IsActive == true).ToList();

                    if (tblEvcPartnerslist != null && tblEvcPartnerslist.Count > 0)
                    {
                        if (ordertype != 16)
                        {
                            EvcPartnersWithPreferancelist = GetEvcPartnerListbyPreferance(tblEvcPartnerslist, productCatId, productQuality);
                            if (EvcPartnersWithPreferancelist != null && EvcPartnersWithPreferancelist.Count > 0)
                            {
                                return EvcPartnersWithPreferancelist;
                            }
                        }
                        else
                        {
                            return tblEvcPartnerslist;
                        }
                    }
                    else
                    {
                        return tblEvcPartnerslist;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return EvcPartnersWithPreferancelist;
        }
        #endregion

        #region Method to get the list of evc partner on the basis of product quality & ProductCatId
        public List<TblEvcPartner> GetEvcPartnerListbyPreferance(List<TblEvcPartner> tblEvcPartnerslist, int? productCatId, string? productQuality)
        {
            TblConfiguration? tblConfiguration = null;
            TblEvcPartnerPreference? tblEvcPartnerPreference = null;
            List<TblEvcPartner> EvcPartnersWithPreferancelist = new List<TblEvcPartner>();
            try
            {
                if (tblEvcPartnerslist != null && tblEvcPartnerslist.Count > 0)
                {
                    tblConfiguration = _context.TblConfigurations.Where(x => x.IsActive == true && x.Name == productQuality).FirstOrDefault();
                    if (tblConfiguration != null)
                    {
                        foreach (var items in tblEvcPartnerslist)
                        {

                            tblEvcPartnerPreference = _context.TblEvcPartnerPreferences
                                .Include(x=>x.Evcpartner).ThenInclude(x=>x.Evcregistration)
                                .Where(x => x.IsActive == true && x.EvcpartnerId == items.EvcPartnerId && x.ProductCatId == productCatId && x.ProductQualityId == Convert.ToInt32(tblConfiguration.Value)).FirstOrDefault();
                            if (tblEvcPartnerPreference != null)
                            {
                                EvcPartnersWithPreferancelist.Add(items);
                            }
                        }
                    }
                    return EvcPartnersWithPreferancelist;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return EvcPartnersWithPreferancelist;
        }
        #endregion

        #region Method to get the list of evc partners having running balance greater than EVC Amount
        /// <summary>
        /// Method to get the list of evc partners having running balance greater than EVC Amount
        /// </summary>
        /// <param name="tblEvcPartnersList"></param>
        /// <param name="evcPrice"></param>
        /// <returns>tblEvcPartners</returns>
        public List<TblEvcPartner> GetEvcPartnerListHavingClearBalance(List<TblEvcPartner> tblEvcPartnersList, int? evcPrice)
        {
            List<TblEvcPartner> tblEvcPartners = new List<TblEvcPartner>();
            try
            {
                if (tblEvcPartnersList != null && tblEvcPartnersList.Count > 0 && evcPrice > 0)
                {
                    foreach (var evc in tblEvcPartnersList)
                    {
                        List<TblWalletTransaction> TblWalletTransactions = _context.TblWalletTransactions
                            .Where(x => x.EvcregistrationId == evc.Evcregistration.EvcregistrationId && x.IsActive == true && x.StatusId != "26" && x.StatusId != "21" && x.StatusId != "44")
                            .ToList();
                        decimal? IsProgress = 0;
                        decimal? IsDeleverd = 0;
                        decimal? RunningBlns = 0;

                        if (TblWalletTransactions != null && TblWalletTransactions.Count > 0)
                        {
                            foreach (var walletTransaction in TblWalletTransactions)
                            {
                                if (walletTransaction.OrderOfInprogressDate != null && walletTransaction.OrderOfDeliverdDate == null && walletTransaction.OrderOfCompleteDate == null && walletTransaction.StatusId != "26" && walletTransaction.StatusId != "21" && walletTransaction.StatusId != "44")
                                {
                                    if (walletTransaction.OrderAmount != null)
                                    {
                                        IsProgress += walletTransaction.OrderAmount;
                                    }
                                }
                                if (walletTransaction.OrderOfInprogressDate != null && walletTransaction.OrderOfDeliverdDate != null && walletTransaction.OrderOfCompleteDate == null && walletTransaction.StatusId != "26" && walletTransaction.StatusId != "21" && walletTransaction.StatusId != "44")
                                {
                                    if (walletTransaction.OrderAmount != null)
                                    {
                                        IsDeleverd += walletTransaction.OrderAmount;
                                    }
                                }
                            }
                            RunningBlns = evc.Evcregistration.EvcwalletAmount - (IsProgress + IsDeleverd);
                            if (RunningBlns > 0 && RunningBlns > evcPrice)
                            {
                                tblEvcPartners.Add(evc);
                            }
                        }
                        else
                        {
                            RunningBlns = evc.Evcregistration.EvcwalletAmount - (IsProgress + IsDeleverd);
                            if (RunningBlns > 0 && RunningBlns > evcPrice)
                            {
                                tblEvcPartners.Add(evc);
                            }
                        }
                    }

                }
                else
                {
                    return tblEvcPartners;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return tblEvcPartners;
        }
        #endregion

        #region Method to get the evc partner having oldest recharge date
        /// <summary>
        /// Method to get the evc partner having oldest recharge date
        /// </summary>
        /// <param name="tblEvcPartnersList"></param>
        /// <returns></returns>
        public List<TblEvcPartner> GetEvcPartnerListHavingOldRecharge(List<TblEvcPartner> tblEvcPartnersList)
        {
            List<TblEvcPartner> tblEvcPartnerOldList = new List<TblEvcPartner>();
            TblEvcwalletAddition? tblEvcwalletAddition = null;
            TblEvcPartner? tblEvcPartner = null;
            int? evcPartnerIdWithOldestDate = 0;
            DateTime oldestDate = DateTime.MaxValue;
            try
            {
                if (tblEvcPartnersList != null && tblEvcPartnersList.Count > 0)
                {
                    foreach (var evc in tblEvcPartnersList)
                    {
                        tblEvcwalletAddition = _context.TblEvcwalletAdditions
                            .Where(x => x.IsActive == true && x.EvcregistrationId == evc.EvcregistrationId)
                            .OrderByDescending(x => x.CreatedDate)
                            .FirstOrDefault();
                        if (tblEvcwalletAddition != null)
                        {
                            DateTime dateTime = Convert.ToDateTime(tblEvcwalletAddition.CreatedDate).Date;
                            if (dateTime < oldestDate)
                            {
                                oldestDate = dateTime;
                                evcPartnerIdWithOldestDate = evc.EvcPartnerId;
                            }
                        }
                    }
                    if (evcPartnerIdWithOldestDate > 0)
                    {
                        tblEvcPartner = _context.TblEvcPartners.Where(x => x.IsActive == true && x.EvcPartnerId == evcPartnerIdWithOldestDate).FirstOrDefault();
                        if (tblEvcPartner != null)
                        {
                            tblEvcPartnerOldList.Add(tblEvcPartner);
                            return tblEvcPartnerOldList;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return tblEvcPartnerOldList;
        }
        #endregion

        #region method to get in-house evc partner list by pincode for other than in-house
        /// <summary>
        /// method to get in-house evc partner list by pincode for other than in-house
        /// </summary>
        /// <param name="custPincode"></param>
        /// <returns>tblEvcPartnerslist</returns>
        public List<TblEvcPartner> GetNonInHouseEvcPartnerListByPincode(string? custPincode, int? productCatId, string? productQuality, int ordertype)
        {
            List<TblEvcPartner>? tblEvcPartnerslist = null;
            List<TblEvcPartner>? EvcPartnersWithPreferancelist = new List<TblEvcPartner>();
            try
            {
                if (!string.IsNullOrEmpty(custPincode))
                {
                    tblEvcPartnerslist = _context.TblEvcPartners
                        .Include(x => x.Evcregistration)
                        .Where(x => x.IsActive == true && x.ListOfPincode.Contains(custPincode) && (x.Evcregistration.IsInHouse == false || x.Evcregistration.IsInHouse == null) && x.IsApprove == true
        && x.Evcregistration.Isevcapprovrd == true && x.Evcregistration.IsActive == true).ToList();
                    if (tblEvcPartnerslist != null && tblEvcPartnerslist.Count > 0)
                    {
                        if (ordertype != 16)
                        {
                            EvcPartnersWithPreferancelist = GetEvcPartnerListbyPreferance(tblEvcPartnerslist, productCatId, productQuality);
                            if (EvcPartnersWithPreferancelist != null && EvcPartnersWithPreferancelist.Count > 0)
                            {
                                return EvcPartnersWithPreferancelist;
                            }
                        }
                        else
                        {
                            return tblEvcPartnerslist;
                        }
                    }
                    else
                    {
                        return EvcPartnersWithPreferancelist;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return EvcPartnersWithPreferancelist;
        }
        #endregion

        #region Method to get EVC Partner Details
        /// <summary>
        /// Method to get EVC Partner Details
        /// </summary>
        /// <param name="EVCPartnerId"></param>
        /// <returns></returns>
        public TblEvcPartner GetEVCPartnerDetails(int EVCPartnerId)
        {
            TblEvcPartner? tblEvcPartner = new TblEvcPartner();
            if (EVCPartnerId != null && EVCPartnerId > 0)
            {
                tblEvcPartner = _context.TblEvcPartners
                    .Include(x => x.Evcregistration)
                    .Include(x => x.State)
                    .Include(x => x.City)
                    .FirstOrDefault(x => x.EvcPartnerId == EVCPartnerId && x.IsActive == true);
            }
            return tblEvcPartner;
        }
        #endregion

        #region method to get All evc partner list by pincode
        /// <summary>
        /// method to get All evc partner list by pincode
        /// </summary>
        /// <param name="custPincode"></param>
        /// <returns>tblEvcPartnerslist</returns>
        public List<TblEvcPartner> GetAllEvcPartnerListByPincode(string? custPincode, int? productCatId, string? productQuality,int ordertype)
        {
            List<TblEvcPartner>? tblEvcPartnerslist = null;
            List<TblEvcPartner>? EvcPartnersWithPreferancelist = new List<TblEvcPartner>();
            try
            {
                if (!string.IsNullOrEmpty(custPincode))
                {
                    tblEvcPartnerslist = _context.TblEvcPartners
                        .Include(x => x.Evcregistration)
                        .Where(x => x.IsActive == true && x.ListOfPincode.Contains(custPincode)  && x.IsApprove == true
                        && x.Evcregistration.Isevcapprovrd == true && x.Evcregistration.IsActive == true).ToList();

                    if (tblEvcPartnerslist != null && tblEvcPartnerslist.Count > 0)
                    {
                        if (ordertype != 16)
                        {
                            EvcPartnersWithPreferancelist = GetEvcPartnerListbyPreferance(tblEvcPartnerslist, productCatId, productQuality);
                            if (EvcPartnersWithPreferancelist != null && EvcPartnersWithPreferancelist.Count > 0)
                            {
                                return EvcPartnersWithPreferancelist;
                            }
                        }
                        else
                        {
                            return tblEvcPartnerslist;
                        }
                    }
                    else
                    {
                        return EvcPartnersWithPreferancelist;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return EvcPartnersWithPreferancelist;
        }
        #endregion

        #region method to get All evc partner list by EVC
        /// <summary>
        /// method to get All evc partner list by EVC
        /// </summary>
        /// <param name="EVC"></param>
        /// <returns>tblEvcPartnerslist</returns>
        public List<TblEvcPartner> GetEvcPartnerListbyEVC(int EVCId)
        {
            List<TblEvcPartner>? tblEvcPartnerslist = null;          
            try
            {
                if (EVCId != null && EVCId > 0)
                { 
                    tblEvcPartnerslist = _context.TblEvcPartners
                        .Include(x => x.Evcregistration)
                        .Where(x => x.IsActive == true  && x.IsApprove == true
                        && x.Evcregistration.Isevcapprovrd == true && x.Evcregistration.IsActive == true && x.EvcregistrationId==EVCId).ToList();

                   return tblEvcPartnerslist;
                        
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return tblEvcPartnerslist;
        }
        #endregion
    }
}
