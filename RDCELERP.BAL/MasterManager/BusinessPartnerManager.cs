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
using RDCELERP.Model.BusinessPartner;
using RDCELERP.Model.BusinessUnit;
using RDCELERP.Model.InfoMessage;
using RDCELERP.Model.Master;
using RDCELERP.Model.UniversalPriceMaster;

namespace RDCELERP.BAL.MasterManager
{
    public class BusinessPartnerManager : IBusinessPartnerManager
    {

        #region  Variable Declaration
        IBusinessPartnerRepository _businessPartnerRepository;
        IUserRoleRepository _userRoleRepository;
        IBusinessUnitRepository _businessUnitRepository;
        IModelMappingRepository _modelMappingRepository;
        IOrderBasedConfigRepository _orderBasedConfigRepository;
        IModelNumberRepository _modelNumberRepository;
        IProductConditionLabelRepository _productConditionLabelRepository;
        private readonly IMapper _mapper;
        ILogging _logging;
        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();
        ICityRepository _cityRepository;
        IUniversalPriceMasterRepository _UniversalPriceMasterRepository;
        IPriceMasterMappingRepository _PriceMasterMappingRepository;
        #endregion
        public BusinessPartnerManager(IBusinessPartnerRepository BusinessPartnerRepository, IPriceMasterMappingRepository PriceMasterMappingRepository, IUniversalPriceMasterRepository UniversalPriceMasterRepository, IBusinessUnitRepository businessUnitRepository, IUserRoleRepository userRoleRepository, IModelNumberRepository modelNumberRepository, IMapper mapper, ILogging logging, ICityRepository cityRepository, IModelMappingRepository modelMappingRepository, IProductConditionLabelRepository productConditionLabelRepository, IOrderBasedConfigRepository orderBasedConfigRepository)
        {
            _businessPartnerRepository = BusinessPartnerRepository;
            _userRoleRepository = userRoleRepository;
            _businessUnitRepository = businessUnitRepository;
            _modelMappingRepository = modelMappingRepository;
            _orderBasedConfigRepository = orderBasedConfigRepository;
            _productConditionLabelRepository = productConditionLabelRepository;
            _modelNumberRepository = modelNumberRepository;
            _mapper = mapper;
            _logging = logging;
            _cityRepository = cityRepository;
            _UniversalPriceMasterRepository = UniversalPriceMasterRepository;
            _PriceMasterMappingRepository = PriceMasterMappingRepository;
        }

        /// <summary>
        /// Method to manage (Add/Edit) BusinessPartner
        /// </summary>
        /// <param name="BusinessPartnerVM">BusinessPartnerVM</param>
        /// <param name="userId">userId</param>
        /// <returns>int</returns>
        public int ManageBusinessPartner(BusinessPartnerViewModel BusinessPartnerVM, int Id)
        {
            TblBusinessPartner tblBusinessPartner = new TblBusinessPartner();
            TblBusinessUnit tblBusinessUnit = new TblBusinessUnit();

            try
            {
                if (BusinessPartnerVM != null)
                {
                    tblBusinessPartner = _mapper.Map<BusinessPartnerViewModel, TblBusinessPartner>(BusinessPartnerVM);
                    var Check = _businessPartnerRepository.GetSingle(x => x.StoreCode == tblBusinessPartner.StoreCode);
                    if (Check != null)
                    {
                        tblBusinessPartner.BusinessPartnerId = Check.BusinessPartnerId;
                    }

                    if (tblBusinessPartner.BusinessPartnerId > 0)
                    {

                        //Code to update the object
                        if (tblBusinessPartner.SponsorName != null)
                        {
                            tblBusinessUnit = _businessUnitRepository.GetSingle(x => x.Name == BusinessPartnerVM.SponsorName);
                            tblBusinessPartner.BusinessUnitId = tblBusinessUnit.BusinessUnitId;
                        }
                        var CityId = _cityRepository.GetSingle(x => x.Name == BusinessPartnerVM.City && x.IsActive == true);
                        if(CityId != null)
                        {
                            tblBusinessPartner.CityId = CityId.CityId;
                        }
                        tblBusinessPartner.Description = BusinessPartnerVM.Name;
                        tblBusinessPartner.IsActive = BusinessPartnerVM.IsActive;
                        tblBusinessPartner.ModifiedBy = Id;
                        tblBusinessPartner.ModifiedDate = _currentDatetime;
                        tblBusinessPartner = TrimHelper.TrimAllValuesInModel(tblBusinessPartner);
                        _businessPartnerRepository.Update(tblBusinessPartner);
                    }
                    else
                    {

                        //Code to Insert the object 
                        tblBusinessPartner.IsActive = true;
                        tblBusinessPartner.Description = BusinessPartnerVM.Name;
                        if (BusinessPartnerVM.SponsorName != null)
                        {
                            tblBusinessUnit = _businessUnitRepository.GetSingle(x => x.Name == BusinessPartnerVM.SponsorName);
                            tblBusinessPartner.BusinessUnitId = tblBusinessUnit.BusinessUnitId;
                        }
                        var CityId = _cityRepository.GetSingle(x => x.Name == BusinessPartnerVM.City && x.IsActive == true);
                        if (CityId != null)
                        {
                            tblBusinessPartner.CityId = CityId.CityId;
                        }
                        tblBusinessPartner.CreatedDate = _currentDatetime;
                        tblBusinessPartner.CreatedBy = Id;

                        //Trim the whitespaces...
                        tblBusinessPartner = TrimHelper.TrimAllValuesInModel(tblBusinessPartner);
                        _businessPartnerRepository.Create(tblBusinessPartner);

                    }
                    _businessPartnerRepository.SaveChanges();

                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("BusinessPartnerManager", "ManageBusinessPartner", ex);
            }
            return tblBusinessPartner.BusinessPartnerId;
        }


        public BusinessPartnerViewModel ManageBusinessPartnerBulk(BusinessPartnerViewModel BusinessPartnerVM, int userId)
        {
            List<TblBusinessPartner> tblBusinessPartner = new List<TblBusinessPartner>();
            
            bool IsSuccessfullyInsertedToOrderbasedConfig = false;
            bool IsSuccessfullyInsertedToPClabels = false;

            if (BusinessPartnerVM != null && BusinessPartnerVM.BusinessPartnerVMList != null && BusinessPartnerVM.BusinessPartnerVMList.Count > 0)
            {
               
                var ValidatedBusinessPartnerList = BusinessPartnerVM.BusinessPartnerVMList.Where(x => x.Remarks == null || string.IsNullOrEmpty(x.Remarks)).ToList();
                BusinessPartnerVM.BusinessPartnerVMErrorList = BusinessPartnerVM.BusinessPartnerVMList.Where(x => x.Remarks != null && !string.IsNullOrEmpty(x.Remarks)).ToList();
                
                //tblBusinessPartner = _mapper.Map<List<BusinessPartnerVMExcelModel>, List<TblBusinessPartner>>(ValidatedBusinessPartnerList);
                if (ValidatedBusinessPartnerList != null && ValidatedBusinessPartnerList.Count > 0)
                {
                    foreach (var item in ValidatedBusinessPartnerList)
                    {
                        try
                         {
                            var Check = _businessPartnerRepository.GetSingle(x => x.StoreCode == item.StoreCode);
                            if(Check != null)
                            {
                                item.BusinessPartnerId = Check.BusinessPartnerId;
                            }
                           

                            if (item.BusinessPartnerId > 0)
                            {

                                TblBusinessPartner TblBusinessPartner = new TblBusinessPartner();
                                
                                //Code to update the object 
                                TblBusinessPartner.BusinessPartnerId = item.BusinessPartnerId;
                                TblBusinessPartner.Name = item.Name;
                                TblBusinessPartner.Email = item.Email;
                                TblBusinessPartner.State = item.State;
                                TblBusinessPartner.City = item.City;
                                TblBusinessPartner.Pincode = item.Pincode;
                                TblBusinessPartner.StoreType = item.StoreType;
                                TblBusinessPartner.Bppassword = item.Bppassword;
                                TblBusinessPartner.PhoneNumber = item.PhoneNumber;
                                TblBusinessPartner.AssociateCode = item.AssociateCode;
                                TblBusinessPartner.AddressLine1 = item.AddressLine1;
                                TblBusinessPartner.AddressLine2 = item.AddressLine2;
                                TblBusinessPartner.ContactPersonFirstName = item.ContactPersonFirstName;
                                TblBusinessPartner.ContactPersonLastName = item.ContactPersonLastName;
                                TblBusinessPartner.StoreCode = item.StoreCode;
                                TblBusinessPartner.Upiid = item.Upiid;
                                if (item.VoucherType == "Discount")
                                {
                                    item.VoucherType = 1.ToString();
                                }
                                else if (item.VoucherType == "Cash")
                                {
                                    item.VoucherType = 2.ToString();
                                }
                                else
                                {
                                    item.VoucherType = null;
                                }

                                if (item.CompanyName != null)
                                {
                                    var BusinessUnit = _businessUnitRepository.GetSingle(x => x.Name == item.CompanyName);
                                    TblBusinessPartner.BusinessUnitId = BusinessUnit.BusinessUnitId;
                                }
                                var CityId = _cityRepository.GetSingle(x => x.Name == item.City && x.IsActive == true);
                                if (CityId != null)
                                {
                                    TblBusinessPartner.CityId = CityId.CityId;
                                }
                                TblBusinessPartner.Description = item.Name;
                                TblBusinessPartner.SponsorName = item.CompanyName;
                                TblBusinessPartner.IsAbbbp = item.IsAbbbp;
                                TblBusinessPartner.IsExchangeBp = item.IsExchangeBp;
                                TblBusinessPartner.IsOrc = item.IsORC;
                                TblBusinessPartner.IsD2c = item.IsD2c;
                                TblBusinessPartner.IsDealer = item.IsDealer;
                                TblBusinessPartner.IsDefferedAbb = item.IsDefferedAbb;
                                TblBusinessPartner.IsDefferedSettlement = item.IsDefferedSettlement;
                                TblBusinessPartner.IsVoucher = item.IsVoucher;
                                TblBusinessPartner.ModifiedDate = _currentDatetime;
                                TblBusinessPartner.ModifiedBy = userId;
                                TblBusinessPartner.IsActive = true;
                                TblBusinessPartner.CreatedBy = Check?.CreatedBy;
                                TblBusinessPartner.CreatedDate = Check?.CreatedDate;
                                TblBusinessPartner = TrimHelper.TrimAllValuesInModel(TblBusinessPartner);

                                _businessPartnerRepository.Update(TblBusinessPartner);
                                _businessPartnerRepository.SaveChanges();

                                // Perform operations for ProductConditionLabels for BusinessPartner -

                                List<TblProductConditionLabel> tblProductConditionLabelsForBusinessPartner = new List<TblProductConditionLabel>();

                                //tblProductConditionLabelsForBusinessPartner = _productConditionLabelRepository.GetProductConditionLabelByBusinessPartnerId(item.BusinessPartnerId);

                                // Delete the Current ProductConditionLabel Records from the table - TblProductConditionLabel
                                _productConditionLabelRepository.DeleteProductConditionLabelsForBP(item.BusinessPartnerId);

                                if (item.LabelName_Excellent_P != null)
                                {
                                    tblProductConditionLabelsForBusinessPartner.Add(new TblProductConditionLabel
                                    {
                                        PclabelName = item.LabelName_Excellent_P?.Trim(),
                                        IsSweetenerApplicable = item.IsSweetenerApplicable_P,
                                        OrderSequence = 1,
                                        BusinessPartnerId = item.BusinessPartnerId,
                                        BusinessUnitId = TblBusinessPartner.BusinessUnitId ?? 0,
                                        IsActive = true,
                                        CreatedDate = DateTime.UtcNow,
                                        CreatedBy = userId,
                                    }); ;
                                }

                                if (item.LabelName_Good_Q != null)
                                {
                                    tblProductConditionLabelsForBusinessPartner.Add(new TblProductConditionLabel
                                    {
                                        PclabelName = item.LabelName_Good_Q?.Trim(),
                                        BusinessPartnerId = item.BusinessPartnerId,
                                        BusinessUnitId = TblBusinessPartner.BusinessUnitId ?? 0,
                                        IsActive = true,
                                        OrderSequence = 2,
                                        CreatedDate = DateTime.UtcNow,
                                        CreatedBy = userId,
                                        IsSweetenerApplicable = item.IsSweetenerApplicable_Q
                                    });
                                }
                                if (item.LabelName_Average_R != null)
                                {
                                    tblProductConditionLabelsForBusinessPartner.Add(new TblProductConditionLabel
                                    {
                                        PclabelName = item.LabelName_Average_R?.Trim(),
                                        BusinessPartnerId = item.BusinessPartnerId,
                                        BusinessUnitId = TblBusinessPartner.BusinessUnitId ?? 0,
                                        OrderSequence = 3,
                                        IsActive = true,
                                        CreatedDate = DateTime.UtcNow,
                                        CreatedBy = userId,
                                        IsSweetenerApplicable = item.IsSweetenerApplicable_R
                                    });
                                }
                                if (item.LabelName_NonWorking_S != null)
                                {
                                    tblProductConditionLabelsForBusinessPartner.Add(new TblProductConditionLabel
                                    {
                                        PclabelName = item.LabelName_NonWorking_S?.Trim(),
                                        BusinessPartnerId = item.BusinessPartnerId,
                                        BusinessUnitId = TblBusinessPartner.BusinessUnitId ?? 0,
                                        IsActive = true,
                                        OrderSequence = 4,
                                        CreatedDate = DateTime.UtcNow,
                                        CreatedBy = userId,
                                        IsSweetenerApplicable = item.IsSweetenerApplicable_S
                                    });
                                }

                                tblProductConditionLabelsForBusinessPartner = TrimHelper.TrimAllValuesInModel(tblProductConditionLabelsForBusinessPartner);

                                IsSuccessfullyInsertedToPClabels = _productConditionLabelRepository.InsertBulkReords(tblProductConditionLabelsForBusinessPartner);

                                // Need to Update data for tblOrderBasedConfig table for existing Business Partner...
                                if (IsSuccessfullyInsertedToPClabels)
                                {
                                    var ExistingRecord = _orderBasedConfigRepository.GetOrderBasedConfigRecordByBusinessPartner(item.BusinessPartnerId);
                                    int buID = TblBusinessPartner.BusinessUnitId ?? 0;

                                    if (ExistingRecord != null)
                                    {
                                        var getDataFromBusinessUnit = _businessUnitRepository.GetBusinessunitDetails(buID);

                                        ExistingRecord.IsBpmultiBrand = getDataFromBusinessUnit.IsBumultiBrand;
                                        ExistingRecord.IsSweetenerModalBased = getDataFromBusinessUnit.IsSweetnerModelBased;
                                        ExistingRecord.IsValidationBasedSweetener = getDataFromBusinessUnit.IsValidationBasedSweetner;

                                        _orderBasedConfigRepository.Update(ExistingRecord);
                                        _orderBasedConfigRepository.SaveChanges();
                                    }
                                }
                            }
                            else
                            {


                                TblBusinessPartner TblBusinessPartner = new TblBusinessPartner();
                                //Code to insert the object 
                                TblBusinessPartner.Email = item.Email;
                                TblBusinessPartner.Name = item.Name;
                                TblBusinessPartner.State = item.State;
                                TblBusinessPartner.City = item.City;
                                TblBusinessPartner.Bppassword = item.Bppassword;
                                TblBusinessPartner.AssociateCode = item.AssociateCode;
                                TblBusinessPartner.Pincode = item.Pincode;
                                TblBusinessPartner.PhoneNumber = item.PhoneNumber;
                                TblBusinessPartner.StoreType = item.StoreType;
                                TblBusinessPartner.AddressLine1 = item.AddressLine1;
                                TblBusinessPartner.AddressLine2 = item.AddressLine2;
                                TblBusinessPartner.ContactPersonFirstName = item.ContactPersonFirstName;
                                TblBusinessPartner.ContactPersonLastName = item.ContactPersonLastName;
                                TblBusinessPartner.StoreCode = item.StoreCode;
                                TblBusinessPartner.Upiid = item.Upiid;
                                if (item.VoucherType == "Discount")
                                {
                                    item.VoucherType = 1.ToString();
                                }
                                else if (item.VoucherType == "Cash")
                                {
                                    item.VoucherType = 2.ToString();
                                }
                                else
                                {
                                    item.VoucherType = null;
                                }
                                if (item.CompanyName != null)
                                {
                                    var BusinessUnit = _businessUnitRepository.GetSingle(x => x.Name == item.CompanyName);
                                    TblBusinessPartner.BusinessUnitId = BusinessUnit.BusinessUnitId;
                                }
                                var CityId = _cityRepository.GetSingle(x => x.Name == item.City && x.IsActive == true);
                                if (CityId != null)
                                {
                                    TblBusinessPartner.CityId = CityId.CityId;
                                }
                                TblBusinessPartner.Description = item.Name;
                                TblBusinessPartner.SponsorName = item.CompanyName;
                                TblBusinessPartner.IsAbbbp = item.IsAbbbp;
                                TblBusinessPartner.IsExchangeBp = item.IsExchangeBp;
                                TblBusinessPartner.IsOrc = item.IsORC;
                                TblBusinessPartner.IsD2c = item.IsD2c;
                                TblBusinessPartner.IsDealer = item.IsDealer;
                                TblBusinessPartner.IsDefferedAbb = item.IsDefferedAbb;
                                TblBusinessPartner.IsDefferedSettlement = item.IsDefferedSettlement;
                                TblBusinessPartner.IsVoucher = item.IsVoucher;
                                
                                TblBusinessPartner.IsActive = true;
                                TblBusinessPartner.CreatedDate = _currentDatetime;
                                TblBusinessPartner.CreatedBy = userId;

                                //Trim all whitespaces...
                                TblBusinessPartner = TrimHelper.TrimAllValuesInModel(TblBusinessPartner);

                                _businessPartnerRepository.Create(TblBusinessPartner);
                                _businessPartnerRepository.SaveChanges();

                                // Add ProductConditionLabels for BusinessPartner -
                                List<TblProductConditionLabel> tblProductConditionLabelsForBusinessPartner = new List<TblProductConditionLabel>();


                                if (item.LabelName_Excellent_P != null)
                                {
                                    tblProductConditionLabelsForBusinessPartner.Add(new TblProductConditionLabel
                                    {
                                        PclabelName = item.LabelName_Excellent_P?.Trim(),
                                        BusinessPartnerId = TblBusinessPartner.BusinessPartnerId,
                                        BusinessUnitId = TblBusinessPartner.BusinessUnitId ?? 0,
                                        IsActive = true,
                                        OrderSequence = 1,
                                        CreatedDate = DateTime.UtcNow,
                                        CreatedBy = userId,
                                        IsSweetenerApplicable = item.IsSweetenerApplicable_P
                                    });
                                }

                                if (item.LabelName_Good_Q != null)
                                {
                                    tblProductConditionLabelsForBusinessPartner.Add(new TblProductConditionLabel
                                    {
                                        PclabelName = item.LabelName_Good_Q?.Trim(),
                                        BusinessPartnerId = TblBusinessPartner.BusinessPartnerId,
                                        BusinessUnitId = TblBusinessPartner.BusinessUnitId ?? 0,
                                        IsActive = true,
                                        OrderSequence = 2,
                                        CreatedDate = DateTime.UtcNow,
                                        CreatedBy = userId,
                                        IsSweetenerApplicable = item.IsSweetenerApplicable_Q
                                    });
                                }
                                if (item.LabelName_Average_R != null)
                                {
                                    tblProductConditionLabelsForBusinessPartner.Add(new TblProductConditionLabel
                                    {
                                        PclabelName = item.LabelName_Average_R?.Trim(),
                                        BusinessPartnerId = TblBusinessPartner.BusinessPartnerId,
                                        BusinessUnitId = TblBusinessPartner.BusinessUnitId ?? 0,
                                        IsActive = true,
                                        OrderSequence= 3,
                                        CreatedDate = DateTime.UtcNow,
                                        CreatedBy = userId,
                                        IsSweetenerApplicable = item.IsSweetenerApplicable_R,
                                    });
                                }
                                if (item.LabelName_NonWorking_S != null)
                                {
                                    tblProductConditionLabelsForBusinessPartner.Add(new TblProductConditionLabel
                                    {
                                        PclabelName = item.LabelName_NonWorking_S?.Trim(),
                                        BusinessPartnerId = TblBusinessPartner.BusinessPartnerId,
                                        BusinessUnitId = TblBusinessPartner.BusinessUnitId ?? 0,
                                        IsActive = true,
                                        OrderSequence = 4,
                                        CreatedDate = DateTime.UtcNow,
                                        CreatedBy = userId,
                                        IsSweetenerApplicable = item.IsSweetenerApplicable_S
                                    });
                                }

                                IsSuccessfullyInsertedToPClabels = _productConditionLabelRepository.InsertBulkReords(tblProductConditionLabelsForBusinessPartner);

                                //If data successfully inserted in the Product Condition Label then Add Data in OrderBasedConfig for the Business Partner -

                                if (IsSuccessfullyInsertedToPClabels)
                                {
                                    TblOrderBasedConfig tblOrderBasedConfig = new TblOrderBasedConfig();

                                    int buID = TblBusinessPartner.BusinessUnitId ?? 0;

                                    var getDataFromBusinessUnit = _businessUnitRepository.GetBusinessunitDetails(buID);

                                    if (getDataFromBusinessUnit != null)
                                    {
                                        tblOrderBasedConfig.IsActive = true;
                                        tblOrderBasedConfig.IsSweetenerModalBased = getDataFromBusinessUnit?.IsSweetnerModelBased;
                                        tblOrderBasedConfig.CreatedDate = DateTime.UtcNow;
                                        tblOrderBasedConfig.CreatedBy = userId;
                                        tblOrderBasedConfig.IsValidationBasedSweetener = getDataFromBusinessUnit?.IsValidationBasedSweetner;
                                        tblOrderBasedConfig.BusinessPartnerId = TblBusinessPartner.BusinessPartnerId;
                                        tblOrderBasedConfig.IsBpmultiBrand = getDataFromBusinessUnit?.IsBumultiBrand;
                                        tblOrderBasedConfig.BusinessUnitId = getDataFromBusinessUnit.BusinessUnitId;
                                    }

                                    IsSuccessfullyInsertedToOrderbasedConfig = _orderBasedConfigRepository.InsertOrderBasedConfigRecordForBusinessPartner(tblOrderBasedConfig,  getDataFromBusinessUnit);
                                }

                                if (IsSuccessfullyInsertedToOrderbasedConfig)
                                {
                                    // if data insert Successfully in the OrderBasedConfig then Add the data in the TblModelMapping.

                                    List<TblModelMapping> modelMappings = new List<TblModelMapping>();
                                    (List<TblModelNumber> tblModelNumberList, List<int> DefaultModelNumberIds) = _modelNumberRepository.GetListOfModelNumbersForBU(TblBusinessPartner.BusinessUnitId);

                                    foreach (var modelnumbers in tblModelNumberList)
                                    {
                                        TblModelMapping tblModelMapping = new TblModelMapping();

                                        tblModelMapping.BusinessUnitId = modelnumbers.BusinessUnitId; 
                                        tblModelMapping.BrandId = modelnumbers.BrandId;
                                        tblModelMapping.ModelId = modelnumbers.ModelNumberId;
                                        tblModelMapping.BusinessPartnerId = TblBusinessPartner.BusinessPartnerId;
                                        tblModelMapping.SweetenerBp = modelnumbers.SweetenerBp;
                                        tblModelMapping.SweetenerBu = modelnumbers.SweetenerBu;
                                        tblModelMapping.SweetenerDigi2l = modelnumbers.SweetenerDigi2l;
                                        tblModelMapping.IsActive = true;
                                        tblModelMapping.CreatedBy = userId;
                                        tblModelMapping.CreatedDate = DateTime.UtcNow;

                                        if (DefaultModelNumberIds.Contains(modelnumbers.ModelNumberId))
                                        {
                                            tblModelMapping.IsDefault = true;
                                        }

                                        modelMappings.Add(tblModelMapping);
                                    }

                                    _modelMappingRepository.InsertModelsForBusinessPartner(modelMappings);                                    
                                }
                            }
                        }

                        catch (Exception ex)
                        {
                            item.Remarks += ex.Message + ", ";
                            BusinessPartnerVM.BusinessPartnerVMList.Add(item);
                        }
                    }
                }
            }

            return BusinessPartnerVM;
        }


        /// <summary>
        /// Method to get the BusinessPartner by id 
        /// </summary>
        /// <param name="id">BusinessPartnerId</param>
        /// <returns>BusinessPartnerViewModel</returns>
        public BusinessPartnerViewModel GetBusinessPartnerById(int id)
        {
            BusinessPartnerViewModel BusinessPartnerVM = null;
            TblBusinessPartner tblBusinessPartner = null;

            try
            {
                tblBusinessPartner = _businessPartnerRepository.GetSingle(where: x => x.IsActive == true && x.BusinessPartnerId == id);
                if (tblBusinessPartner != null)
                {
                    BusinessPartnerVM = _mapper.Map<TblBusinessPartner, BusinessPartnerViewModel>(tblBusinessPartner);
                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("BusinessPartnerManager", "GetBusinessPartnerById", ex);
            }
            return BusinessPartnerVM;
        }

        /// <summary>
        /// Method to get the BusinessPartner List by BuId 
        /// </summary>
        /// <param name="id">BuId</param>
        /// <returns>List<BusinessPartnerViewModel></returns>
        public List<BusinessPartnerViewModel> GetListofBusinessPartnerByBUId(int BUId)
        {
            List<BusinessPartnerViewModel> BusinessPartnerVMList = null;
            List<TblBusinessPartner> tblBusinessPartnerList = null;

            try
            {
                tblBusinessPartnerList = _businessPartnerRepository.GetList(x => x.IsActive == true && x.BusinessUnitId == BUId).ToList();
                if (tblBusinessPartnerList != null && tblBusinessPartnerList.Count > 0)
                {
                    BusinessPartnerVMList = _mapper.Map<List<TblBusinessPartner>, List<BusinessPartnerViewModel>>(tblBusinessPartnerList);
                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("BusinessPartnerManager", "GetListofBusinessPartnerByBUId", ex);
            }
            return BusinessPartnerVMList;
        }


       

        /// <summary>
        /// Method to delete BusinessPartner by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        public bool DeleteBusinessPartnerById(int id)
        {
            bool flag = false;
            try
            {
                TblBusinessPartner tblBusinessPartner = _businessPartnerRepository.GetSingle(x => x.IsActive == true && x.BusinessPartnerId == id);
                if (tblBusinessPartner != null)
                {
                    tblBusinessPartner.IsActive = false;
                    _businessPartnerRepository.Update(tblBusinessPartner);
                    _businessPartnerRepository.SaveChanges();
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("BusinessPartnerManager", "DeleteBusinessPartnerById", ex);
            }
            return flag;
        }
        /// <summary>
        /// Method to get the All BusinessPartner
        /// </summary>     
        /// <returns>List  BusinessPartnerViewModel</returns>

        public IList<BusinessPartnerViewModel> GetAllBusinessPartner(int? BuiD)
        {
            IList<BusinessPartnerViewModel> BusinessPartnerVMList = null;
            List<TblBusinessPartner> tblBusinessPartnerlist = new List<TblBusinessPartner>();
            // TblUseRole TblUseRole = null;
            try
            {
                tblBusinessPartnerlist = _businessPartnerRepository.GetList(x => x.IsActive == true && x.IsAbbbp == true && x.BusinessUnitId == BuiD).ToList();

                if (tblBusinessPartnerlist != null && tblBusinessPartnerlist.Count > 0)
                {
                    BusinessPartnerVMList = _mapper.Map<IList<TblBusinessPartner>, IList<BusinessPartnerViewModel>>(tblBusinessPartnerlist);
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("BusinessPartnerManager", "GetAllPinCode", ex);
            }
            return BusinessPartnerVMList;
        }

        public ExecutionResult GetBusinessPartner()
        {
            IList<BusinessPartnerViewModel> BusinessPartnerVMList = null;
            List<TblBusinessPartner> TblBusinessPartnerlist = new List<TblBusinessPartner>();


            // TblUseRole TblUseRole = null;
            try
            {

                TblBusinessPartnerlist = _businessPartnerRepository.GetList(x => x.IsActive == true).ToList();

                if (TblBusinessPartnerlist != null && TblBusinessPartnerlist.Count > 0)
                {
                    BusinessPartnerVMList = _mapper.Map<IList<TblBusinessPartner>, IList<BusinessPartnerViewModel>>(TblBusinessPartnerlist);
                    return new ExecutionResult(new InfoMessage(true, "Success", BusinessPartnerVMList));

                }

                else
                {
                    return new ExecutionResult(new InfoMessage(true, "No data found"));


                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("BusinessPartnerManager", "GetBusinessPartner", ex);
            }
            return new ExecutionResult(new InfoMessage(true, "No data found"));
        }

        /// <summary>
        /// Method to get the Brand by id 
        /// </summary>
        /// <param name="id">BrandId</param>
        /// <returns>BrandViewModel</returns>
        public ExecutionResult BusinessPartnerById(int id)
        {
            BusinessPartnerViewModel BusinessPartnerVM = null;
            TblBusinessPartner TblBusinessPartner = null;

            try
            {
                TblBusinessPartner = _businessPartnerRepository.GetSingle(where: x => x.IsActive == true && x.BusinessPartnerId == id);
                if (TblBusinessPartner != null)
                {
                    BusinessPartnerVM = _mapper.Map<TblBusinessPartner, BusinessPartnerViewModel>(TblBusinessPartner);
                    return new ExecutionResult(new InfoMessage(true, "Success", BusinessPartnerVM));

                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("BusinessPartnerManager", "BusinessPartnerById", ex);
            }
            return new ExecutionResult(new InfoMessage(true, "No data found"));
        }

        
    }
}