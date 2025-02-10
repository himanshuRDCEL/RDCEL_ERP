using RDCELERP.BAL.Interface;
using RDCELERP.Common.Helper;
using RDCELERP.DAL.IRepository;
using RDCELERP.Model.Users;
using System;
using RDCELERP.DAL.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using RDCELERP.Common.Enums;
using RDCELERP.Model.Company;
using RDCELERP.Common.Constant;
using RDCELERP.DAL.Helper;
using RDCELERP.Model.Role;
using RDCELERP.Model.LGC;
using Mailjet.Client.Resources;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Org.BouncyCastle.Utilities.Collections;
using RDCELERP.DAL.Repository;
using RDCELERP.Model.EVC_Allocated;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Rendering;
using RDCELERP.Model.Product;

namespace RDCELERP.BAL.MasterManager
{
    public class DropdownManager : IDropdownManager
    {
        #region  Variable Declaration
        private readonly IMapper _mapper;
        ILogging _logging;
        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();
        IOrderLGCRepository _orderLGCRepository;
        IEVCPODDetailsRepository _eVCPODDetailsRepository;
        IProductTypeRepository _ProductTypeRepository;
        IEVCPartnerRepository _eVCPartnerRepository;
        #endregion

        public DropdownManager(IOrderLGCRepository orderLGCRepository, IEVCPODDetailsRepository eVCPODDetailsRepository, IProductTypeRepository productTypeRepository, IEVCPartnerRepository eVCPartnerRepository)
        {
            _orderLGCRepository = orderLGCRepository;
            _eVCPODDetailsRepository = eVCPODDetailsRepository;
            _ProductTypeRepository = productTypeRepository;
            _eVCPartnerRepository = eVCPartnerRepository;
        }

        #region Get EVC City Dropdown from EVCPODDetails
        /// <summary>
        /// Get EVC City Dropdown from EVCPODDetails
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="StatusId"></param>
        /// <returns></returns>
        public IEnumerable<SelectListItem> GetEVCCityDDList()
        {
            List<TblEvcpoddetail> tblEvcpoddetailsList = null;
            List<TblEvcregistration> tblEvcregistrations = null;
            IEnumerable<SelectListItem> EVCCityDDL = null;
            List<TblCity> tblCities = new List<TblCity>();

            try
            {
                tblEvcpoddetailsList = _eVCPODDetailsRepository.GetEVCPODDetailsList();
                if (tblEvcpoddetailsList != null && tblEvcpoddetailsList.Count > 0)
                {
                    foreach (TblEvcpoddetail tblEvcpoddetail in tblEvcpoddetailsList)
                    {
                        if (tblEvcpoddetail != null && tblEvcpoddetail.Evc != null && tblEvcpoddetail.Evc.City != null)
                        {
                            tblCities.Add(tblEvcpoddetail.Evc.City);
                        }
                    }
                    if (tblCities != null && tblCities.Count > 0)
                    {
                        tblCities = tblCities.Distinct().ToList();
                        EVCCityDDL = tblCities.ConvertAll(a =>
                        {
                            return new SelectListItem()
                            {
                                Text = a.Name,
                                Value = a.CityId.ToString(),
                                Selected = false
                            };
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return EVCCityDDL;

        }
        #endregion

        #region Get EVC Dropdown from EVCPODDetails by City Id
        /// <summary>
        /// Get EVC Dropdown from EVCPODDetails by City Id
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public List<SelectListItem> GetEVCDDListByCityId(int cityId)
        {
            List<TblEvcpoddetail> tblEvcpoddetailsList = null;
            List<TblEvcregistration> tblEvcregistrations = new List<TblEvcregistration>();
            List<SelectListItem> EVCDDL = new List<SelectListItem>();

            try
            {
                tblEvcpoddetailsList = _eVCPODDetailsRepository.GetEVCPODDetailsList();
                if (tblEvcpoddetailsList != null && tblEvcpoddetailsList.Count > 0)
                {
                    foreach (TblEvcpoddetail tblEvcpoddetail in tblEvcpoddetailsList)
                    {
                        if (tblEvcpoddetail != null && tblEvcpoddetail.Evc != null && tblEvcpoddetail.Evc.City != null)
                        {
                            tblEvcregistrations.Add(tblEvcpoddetail.Evc);
                        }
                    }

                    if (tblEvcregistrations != null && tblEvcregistrations.Count > 0)
                    {
                        tblEvcregistrations = tblEvcregistrations.Where(x => x.CityId == cityId).ToList();
                        if (tblEvcregistrations != null && tblEvcregistrations.Count > 0)
                        {
                            tblEvcregistrations = tblEvcregistrations.Distinct().ToList();
                            EVCDDL = tblEvcregistrations.ConvertAll(a =>
                            {
                                return new SelectListItem()
                                {
                                    Text = a.BussinessName,
                                    Value = a.EvcregistrationId.ToString(),
                                    Selected = false
                                };
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return EVCDDL;
        }
        #endregion

        #region Method to get QC Flag
        /// <summary>
        /// Method to get QC Flag
        /// </summary>
        /// <param name></param>       
        /// <returns> list</returns>
        public List<SelectListItem> GetTimeSlot()
        {
            List<SelectListItem> SelectListobject = null;
            try
            {
                SelectListobject = new List<SelectListItem>
                         {
                          new SelectListItem { Text = "10AM-12PM", Value = "1" },
                          new SelectListItem { Text = "12PM-2PM", Value = "2" },
                          new SelectListItem { Text = "2PM-4PM", Value = "3" },
                          new SelectListItem { Text = "4PM-6PM", Value = "4" },
                          new SelectListItem { Text = "6PM-8PM", Value = "5" }
                         };
                //SelectListobject = SelectListobject.OrderBy(o => o.Value).ToList();

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("DropdownManager", "GetTimeSlot", ex);
            }
            return SelectListobject;

        }
        #endregion

        #region Get Product type on the basis of product category
        /// <summary>
        /// Get ProductType By category
        /// </summary>
        /// <param name="ProductCategoryId"></param>
        /// <returns></returns>
        public IList<SelectListItem> GetProductTypeByCategoryId(int? productCategoryId)
        {
            IList<ProductTypeViewModel> productTypeList = null;
            List<TblProductType> tblProductTypesList = new List<TblProductType>();
            IList<SelectListItem> productTypeDDL = null;

            try
            {
                if (productCategoryId != null)
                {
                    tblProductTypesList = _ProductTypeRepository.GetList(x => x.IsActive == true && x.ProductCatId == productCategoryId).ToList();
                    if (tblProductTypesList != null && tblProductTypesList.Count > 0)
                    {
                        productTypeDDL = tblProductTypesList.ConvertAll(a =>
                        {
                            return new SelectListItem()
                            {
                                Text = a.Id.ToString(),
                                Value = (a.Description + (a.Size != null ? " (" + a.Size + ")" : "")).ToString(),
                                Selected = false
                            };
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("DropdownManager", "GetProductTypeByCategoryId", ex);
            }
            return productTypeDDL;
        }
        #endregion

        #region Get EVC Dropdown from EVCPODDetails by City Id
        /// <summary>
        /// Get EVC Dropdown from EVCPODDetails by City Id
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public List<SelectListItem> GetEVCPartnerlistByEVCID(int EVCID)
        {
            List<TblEvcPartner> tblEvcPartnerList = null;
            List<SelectListItem> EVCpartnerDDL = new List<SelectListItem>();

            try
            {
                tblEvcPartnerList = _eVCPartnerRepository.GetEvcPartnerListbyEVC(EVCID);
                if (tblEvcPartnerList != null && tblEvcPartnerList.Count > 0)
                {
                    tblEvcPartnerList = tblEvcPartnerList.Distinct().ToList();
                    EVCpartnerDDL = tblEvcPartnerList.ConvertAll(a =>
                    {
                        return new SelectListItem()
                        {
                            Text = a.EvcStoreCode,
                            Value = a.EvcPartnerId.ToString(),
                            Selected = false
                        };
                    });
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return EVCpartnerDDL;

        }
        #endregion

        #region Method to Get Product Condition
        /// <summary>
        /// Method to get QC Flag
        /// </summary>
        /// <param name></param>       
        /// <returns> list</returns>
        public List<SelectListItem> GetProductCondition()
        {
            List<SelectListItem>? SelectListobject = null;
            try
            {
                SelectListobject = new List<SelectListItem>
                         {
                          new SelectListItem { Text = "Excellent", Value = "1" },
                          new SelectListItem { Text = "Good", Value = "2" },
                          new SelectListItem { Text = "Average", Value = "3" },
                          new SelectListItem { Text = "Not Working", Value = "4" }
                         };
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("DropdownManager", "GetProductQuality", ex);
            }
            return SelectListobject;

        }
        #endregion
    }

}
