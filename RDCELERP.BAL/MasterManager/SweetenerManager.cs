using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Helper;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.DAL.Repository;
using RDCELERP.Model.Sweetener;

namespace RDCELERP.BAL.MasterManager
{
    public class SweetenerManager : ISweetenerManager
    {
        #region  Variable Declaration
        private Digi2l_DevContext _context;
        ILogging _logging;
        IBusinessPartnerRepository _businessPartnerRepository;
        IModelNumberRepository _modelNumberRepository;
        IModelMappingRepository _modelMappingRepository;
        #endregion

        #region Constructor
        public SweetenerManager(Digi2l_DevContext context, ILogging logging, IBusinessPartnerRepository businessPartnerRepository, IModelNumberRepository modelNumberRepository, IModelMappingRepository modelMappingRepository)
        {
            _context = context;
            _logging = logging;
            _businessPartnerRepository = businessPartnerRepository;
            _modelNumberRepository = modelNumberRepository;
            _modelMappingRepository = modelMappingRepository;
        }
        #endregion

        public SweetenerDataViewModel GetSweetenerAmtExchange(GetSweetenerDetailsDataContract details)
        {
            SweetenerDataViewModel sweetenerDC = new SweetenerDataViewModel();
            try
            {
                if (details != null)
                {
                    if (details.IsSweetenerModalBased == true)
                    {
                        sweetenerDC = GetModalBasedSweetener(details);
                    }
                    else
                    {
                        sweetenerDC = GetBasicSweetener(details);
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ManageSweetener", "GetSweetenerAmtExchange", ex);
            }
            return sweetenerDC;
        }

        public SweetenerDataViewModel GetModalBasedSweetener(GetSweetenerDetailsDataContract details)
        {
            SweetenerDataViewModel sweetenerDC = new SweetenerDataViewModel();
            TblModelMapping? tblModelMapping = null;
            TblModelNumber? modelnumberObj = null;
            try
            {
                if (details != null)
                {
                    if (details.ModalId > 0)
                    {
                        //code to check if selected modal is absolute modal and not default
                        //tblModelMapping = _modelMappingRepository.GetSingle(x => x.ModelId == details.ModalId && x.IsActive == true && x.IsDefault == false && x.BusinessUnitId == details.BusinessUnitId && x.BusinessPartnerId == details.BusinessPartnerId);
                        tblModelMapping = _modelMappingRepository.GetbyModelnoid(details.ModalId, details.BusinessUnitId, details.BusinessPartnerId);
                        if (tblModelMapping != null)
                        {
                            sweetenerDC.SweetenerBu = tblModelMapping.SweetenerBu;
                            sweetenerDC.SweetenerBP = tblModelMapping.SweetenerBp;
                            sweetenerDC.SweetenerDigi2L = tblModelMapping.SweetenerDigi2l;
                            sweetenerDC.SweetenerTotal = (tblModelMapping.SweetenerDigi2l ?? 0) + (tblModelMapping.SweetenerBu ?? 0) + (tblModelMapping.SweetenerBp ?? 0);
                        }
                        else
                        {
                            sweetenerDC.ErrorMessage = "No Model Found for this order in Mapping table";
                        }
                    }
                    //check if modal is default or not
                    ///summary 
                    ///<>for default modal for the required bu we will change flow of getting default modal.
                    ///first we will check in modal number table for default models added for that bu 
                    ///and then search that modal id mapping table <>
                    else
                    {
                        modelnumberObj = _modelNumberRepository.GetSingle(x => x.IsActive == true && x.IsDefaultProduct == true && x.ProductCategoryId == details.NewProdCatId && x.ProductTypeId == details.NewProdTypeId && x.BusinessUnitId == details.BusinessUnitId);
                        if (modelnumberObj != null)
                        {
                            //tblModelMapping = _modelMappingRepository.GetSingle(x => x.IsActive == true && x.IsDefault == true && x.ModelId == modelnumberObj.ModelNumberId && x.BusinessUnitId == details.BusinessUnitId && x.BusinessPartnerId == details.BusinessPartnerId);
                            tblModelMapping = _modelMappingRepository.GetdefaultModelnoid(modelnumberObj.ModelNumberId, details.BusinessUnitId, details.BusinessPartnerId);
                            if (tblModelMapping != null)
                            {
                                sweetenerDC.SweetenerBu = tblModelMapping.SweetenerBu;
                                sweetenerDC.SweetenerBP = tblModelMapping.SweetenerBp;
                                sweetenerDC.SweetenerDigi2L = tblModelMapping.SweetenerDigi2l;
                                sweetenerDC.SweetenerTotal = tblModelMapping.SweetenerDigi2l + tblModelMapping.SweetenerBu + tblModelMapping.SweetenerBp;
                            }
                            else
                            {
                                sweetenerDC.ErrorMessage = "No Model found for this order in mapping table";
                            }
                        }
                        else
                        {
                            sweetenerDC.ErrorMessage = "No Model found for this order in master table";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ManageSweetener", "GetModalBasedSweetener", ex);
            }
            return sweetenerDC;
        }

        public SweetenerDataViewModel GetBasicSweetener(GetSweetenerDetailsDataContract details)
        {
            SweetenerDataViewModel sweetenerDC = new SweetenerDataViewModel();
            TblBusinessPartner businsessPartnerObj = new TblBusinessPartner();
            try
            {
                businsessPartnerObj = _businessPartnerRepository.GetSingle(x => x.IsActive == true && x.IsExchangeBp == true && x.BusinessPartnerId == details.BusinessPartnerId);
                if (businsessPartnerObj != null)
                {
                    sweetenerDC.SweetenerBu = businsessPartnerObj.SweetenerBu;
                    sweetenerDC.SweetenerBP = businsessPartnerObj.SweetenerBp;
                    sweetenerDC.SweetenerDigi2L = businsessPartnerObj.SweetenerDigi2l;
                    sweetenerDC.SweetenerTotal = businsessPartnerObj.SweetenerBu + businsessPartnerObj.SweetenerBp + businsessPartnerObj.SweetenerDigi2l;
                }
                else
                {
                    sweetenerDC.ErrorMessage = "No data found for this business partner in business partner table";
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ManageSweetener", "GetModalBasedSweetener", ex);
            }
            return sweetenerDC;
        }
    }
}
