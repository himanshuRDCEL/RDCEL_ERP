using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Helper;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.Model.InfoMessage;
using RDCELERP.Model.MobileApplicationModel;
using RDCELERP.Model.PinCode;

namespace RDCELERP.BAL.MasterManager
{
    public class PinCodeManager : IPinCodeManager
    {

        #region  Variable Declaration
        IPinCodeRepository _pincodeRepository;
        ICityRepository _cityRepository;
        IUserRoleRepository _userRoleRepository;
        private readonly IMapper _mapper;
        private readonly Digi2l_DevContext _context;
        ILogging _logging;
        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();
        #endregion
        public PinCodeManager(IPinCodeRepository PinCodeRepository, ICityRepository cityRepository, IUserRoleRepository userRoleRepository, IMapper mapper, ILogging logging, Digi2l_DevContext context)
        {
            _pincodeRepository = PinCodeRepository;
            _userRoleRepository = userRoleRepository;
            _cityRepository = cityRepository;
            _mapper = mapper;
            _logging = logging;
            _context = context;
        }

        /// <summary>
        /// Method to manage (Add/Edit) Pincode 
        /// </summary>
        /// <param name="PinCodeVM">PinCodeVM</param>
        /// <param name="Id">Id</param>
        /// <returns>int</returns>
        public int ManagePinCode(PinCodeViewModel PinCodeVM, int Id)
        {
            TblPinCode TblPinCode = new TblPinCode();

            try
            {
                if (PinCodeVM != null)
                {
                    TblPinCode = _mapper.Map<PinCodeViewModel, TblPinCode>(PinCodeVM);
                    if (TblPinCode.Id > 0)
                    {
                        //Code to update the object
                        var city = _cityRepository.GetSingle(where: x => x.Name == PinCodeVM.Location);
                        if (city != null)
                        {
                            TblPinCode.CityId = city.CityId;
                        }
                        TblPinCode.ModifiedBy = Id;
                        TblPinCode.ModifiedDate = _currentDatetime;
                        TblPinCode = TrimHelper.TrimAllValuesInModel(TblPinCode);
                        _pincodeRepository.Update(TblPinCode);
                    }
                    else
                    {
                        var Check = _pincodeRepository.GetSingle(x => x.ZipCode == PinCodeVM.ZipCode);
                        if (Check == null)
                        {
                            //Code to Insert the object 
                            var city = _cityRepository.GetSingle(where: x => x.Name == PinCodeVM.Location);
                            if (city != null)
                            {
                                TblPinCode.CityId = city.CityId;
                            }
                            TblPinCode.IsActive = true;
                            TblPinCode.CreatedDate = _currentDatetime;
                            TblPinCode.CreatedBy = Id;
                            TblPinCode = TrimHelper.TrimAllValuesInModel(TblPinCode);
                            _pincodeRepository.Create(TblPinCode);
                        }
                            
                    }
                    _pincodeRepository.SaveChanges();

                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("PincodeManager", "ManagePinCode", ex);
            }
            return TblPinCode.Id;
        }

        public PinCodeViewModel ManagePinCodeBulk(PinCodeViewModel PinCodeVM, int userId)
        {
            List<TblPinCode> tblPinCode = new List<TblPinCode>();

            if (PinCodeVM != null && PinCodeVM.PinCodeVMList != null && PinCodeVM.PinCodeVMList.Count > 0)
            {
                var ValidatedPinCodeList = PinCodeVM.PinCodeVMList.Where(x => x.Remarks == null || string.IsNullOrEmpty(x.Remarks)).ToList();
                PinCodeVM.PinCodeVMErrorList = PinCodeVM.PinCodeVMList.Where(x => x.Remarks != null && !string.IsNullOrEmpty(x.Remarks)).ToList();

                tblPinCode = _mapper.Map<List<PinCodeVMExcel>, List<TblPinCode>>(ValidatedPinCodeList);
                if (tblPinCode != null && tblPinCode.Count > 0)
                {
                    foreach (var item in ValidatedPinCodeList)
                    {
                        try
                        {
                            if (item.Id > 0)
                            {
                               
                                TblPinCode TblPinCodes = new TblPinCode();
                                //Code to update the object 
                                TblPinCodes.ZipCode = item.PinCode;

                                TblPinCodes.Location = item.Location;
                                TblPinCodes.State = item.State;
                                var city = _cityRepository.GetSingle(where: x => x.Name == item.Location);
                                if (city != null)
                                {
                                    TblPinCodes.CityId = city.CityId;
                                }
                                TblPinCodes.IsActive = item.IsActive;
                                TblPinCodes.IsAbb = item.IsAbb;
                                TblPinCodes.IsExchange = item.IsExchange;
                                TblPinCodes.ModifiedDate = _currentDatetime;
                                TblPinCodes.ModifiedBy = userId;
                                _pincodeRepository.Update(TblPinCodes);
                                _pincodeRepository.SaveChanges();

                            }
                            else
                            {


                                TblPinCode TblPinCodes = new TblPinCode();
                                //Code to update the object 
                                TblPinCodes.ZipCode = item.PinCode;
                                TblPinCodes.Location = item.Location;
                                TblPinCodes.State = item.State;
                                var city = _cityRepository.GetSingle(where: x => x.Name == item.Location);
                                if (city != null)
                                {
                                    TblPinCodes.CityId = city.CityId;
                                }
                                TblPinCodes.IsActive = item.IsActive;
                                TblPinCodes.IsAbb = item.IsAbb;
                                TblPinCodes.IsExchange = item.IsExchange;
                                TblPinCodes.CreatedDate = _currentDatetime;
                                TblPinCodes.CreatedBy = userId;
                                _pincodeRepository.Create(TblPinCodes);
                                _pincodeRepository.SaveChanges();

                            }
                        }

                        catch (Exception ex)
                        {
                            item.Remarks += ex.Message + ", ";
                            PinCodeVM.PinCodeVMErrorList.Add(item);
                        }
                    }
                }
            }

            return PinCodeVM;
        }

        /// <summary>
        /// Method to get the Pincode by id 
        /// </summary>
        /// <param name="id">PincodeId</param>
        /// <returns>TblPinCode</returns>
        public PinCodeViewModel GetPinCodeById(int id)
        {
            PinCodeViewModel PinCodeVM = null;
            TblPinCode TblPinCode = null;

            try
            {
                TblPinCode = _pincodeRepository.GetSingle(where: x => x.Id == id);
                if (TblPinCode != null)
                {
                    PinCodeVM = _mapper.Map<TblPinCode, PinCodeViewModel>(TblPinCode);
                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("PinCodeManager", "GetPinCodeById", ex);
            }
            return PinCodeVM;
        }
        /// <summary>
        /// Method to delete Pincode by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        public bool DeletPinCodeById(int id)
        {
            bool flag = false;
            try
            {
                TblPinCode TblPinCode = _pincodeRepository.GetSingle(x => x.IsActive == true && x.Id == id);
                if (TblPinCode != null)
                {
                    TblPinCode.IsActive = false;
                    _pincodeRepository.Update(TblPinCode);
                    _pincodeRepository.SaveChanges();
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("PincodeManager", "DeletPinCodeById", ex);
            }
            return flag;
        }
        /// <summary>
        /// Method to get the All Pincode
        /// </summary>     
        /// <returns>List  PinCodeViewModel</returns>
        public IList<PinCodeViewModel> GetAllPinCode()
        {
            IList<PinCodeViewModel> PinCodeVMList = null;
            List<TblPinCode> TblPinCodelist = new List<TblPinCode>();
            // TblUseRole TblUseRole = null;
            try
            {

                TblPinCodelist = _pincodeRepository.GetList(x => x.IsActive == true).ToList();

                if (TblPinCodelist != null && TblPinCodelist.Count > 0)
                {
                    PinCodeVMList = _mapper.Map<IList<TblPinCode>, IList<PinCodeViewModel>>(TblPinCodelist);
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("PincodeManager", "GetAllPinCode", ex);
            }
            return PinCodeVMList;
        }

        public ExecutionResult GetPinCode()
        {
            IList<PinCodeViewModel> PinCodeVMList = null;
            List<TblPinCode> TblPinCodelist = new List<TblPinCode>();


            // TblUseRole TblUseRole = null;
            try
            {

                TblPinCodelist = _pincodeRepository.GetList(x => x.IsActive == true).ToList();

                if (TblPinCodelist != null && TblPinCodelist.Count > 0)
                {
                    PinCodeVMList = _mapper.Map<IList<TblPinCode>, IList<PinCodeViewModel>>(TblPinCodelist);
                    return new ExecutionResult(new InfoMessage(true, "Success", PinCodeVMList));

                }

                else
                {
                    return new ExecutionResult(new InfoMessage(true, "No data found"));


                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("PinCodeManager", "GetPinCode", ex);
            }
            return new ExecutionResult(new InfoMessage(true, "No data found"));
        }

        /// <summary>
        /// Method to get the Brand by id 
        /// </summary>
        /// <param name="id">BrandId</param>
        /// <returns>BrandViewModel</returns>
        public ExecutionResult PinCodeById(int id)
        {
            PinCodeViewModel PinCodeVM = null;
            TblPinCode TblPinCode = null;

            try
            {
                TblPinCode = _pincodeRepository.GetSingle(where: x => x.IsActive == true && x.Id == id);
                if (TblPinCode != null)
                {
                    PinCodeVM = _mapper.Map<TblPinCode, PinCodeViewModel>(TblPinCode);
                    return new ExecutionResult(new InfoMessage(true, "Success", PinCodeVM));

                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("PinCodeManager", "PinCodeById", ex);
            }
            return new ExecutionResult(new InfoMessage(true, "No data found"));
        }



        /// <summary> 
        /// Method to get pincodes according to list of city  id's
        /// added by ashwin 
        /// </summary>
        /// <param name="id">CityIdList</param>
        /// <returns>responseResult</returns>
        public ResponseResult PinCodesByCities(CitiesID id)
        {
            #region Variable
            ResponseResult responseResult = new ResponseResult();
            List<TblPinCode> tblPinCodes = null;
            List<PinCodesDataModel> pinCodesDatas = new List<PinCodesDataModel>();
            List<PinCodesDataModel> finalpinCodesDatas = null;
            #endregion 
            try
            {
                if (id != null && id.cityIdlists.Count > 0)
                {
                    finalpinCodesDatas = new List<PinCodesDataModel>();
                    #region Mapping data into Response Model
                    foreach (var cityID in id.cityIdlists)
                    {
                        tblPinCodes = _pincodeRepository.GetList(where: x => x.IsActive == true && x.CityId == cityID.Id).ToList();
                        if (tblPinCodes != null && cityID.Id > 0 && tblPinCodes.Count >0 )
                        {
                            pinCodesDatas = _mapper.Map<List<TblPinCode>, List<PinCodesDataModel>>(tblPinCodes);

                            if (pinCodesDatas.Count > 0)
                            {
                                finalpinCodesDatas.AddRange(pinCodesDatas);
                            }
                            else
                            {
                                responseResult.message = "failed,an error occurs while mapping the data";
                                responseResult.Status = false;
                                responseResult.Status_Code = HttpStatusCode.BadRequest;
                                return responseResult;
                            }
                        }
                        else
                        {
                            if (cityID.Id == 0 && tblPinCodes.Count == 0)
                            {
                                responseResult.message = "failed,Parameters must be greater than zero";
                                responseResult.Status = false;
                                responseResult.Status_Code = HttpStatusCode.BadRequest;
                                return responseResult;
                            }
                            continue;
                        }
                    }
                    #endregion
                    if (finalpinCodesDatas.Count > 0)
                    {
                        responseResult.Status = true;
                        responseResult.Status_Code = HttpStatusCode.OK;
                        responseResult.message = "Success";
                        responseResult.Data = finalpinCodesDatas;
                    }
                    else
                    {
                        responseResult.Status = false;
                        responseResult.Status_Code = HttpStatusCode.NotFound;
                        responseResult.message = "No data found for request parameters";
                    }
                }
                else
                {
                    responseResult.message = "failed,Request parameter should not be null";
                    responseResult.Status = false;
                    responseResult.Status_Code = HttpStatusCode.BadRequest;
                    return responseResult;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("PinCodeManager", "PinCodesByCities", ex);
                responseResult.message = ex.ToString();
                responseResult.Status = false;
                responseResult.Status_Code = HttpStatusCode.InternalServerError;
            }
            return responseResult;
        }

        public IList<PinCodeViewModel> GetPinCodeBYCityId(int? Id)
        {
            IList<PinCodeViewModel> PinCodeVMList = null;
            List<TblPinCode> TblPinCodelist = new List<TblPinCode>();
            TblCity TblCity = null;
            // TblUseRole TblUseRole = null;
            try
            {
                //TblState = _context.TblStates.Where(x => x.IsActive == true && x.Name == State);
                TblCity = _cityRepository.GetSingle(x => x.IsActive == true && x.CityId == Id);
                TblPinCodelist = _pincodeRepository.GetList(x =>x.Location == TblCity.Name).ToList();

                if (TblPinCodelist != null && TblPinCodelist.Count > 0)
                {
                    PinCodeVMList = _mapper.Map<IList<TblPinCode>, IList<PinCodeViewModel>>(TblPinCodelist);
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("PinCodeManager", "GetPinCodeBYCity", ex);
            }
            return PinCodeVMList;
        }

        #region
        /// <summary> 
        /// Method to get pincodes according to service partner Id & city Id
        /// added by Kranti 
        /// </summary>
        /// <param name="id">CityId & servivepartnerId</param>
        /// <returns>responseResult</returns>
        public ResponseResult PinCodesByServicePartner(int cityId, int servicepartnerId)
        {
            #region Variable
            ResponseResult responseResult = new ResponseResult();
            List<TblPinCode> tblPinCodes = null;
            List<MapServicePartnerCityState> mapServicePartnerCityStates = null;
            List<PinCodesModel> pinCodes = new List<PinCodesModel>();

            PinCodesModel finalPinCodes;

            #endregion

            try
            {
                if (cityId >0 && servicepartnerId > 0)
                {
                    mapServicePartnerCityStates = _context.MapServicePartnerCityStates
                        .Where(x => x.CityId == cityId && x.ServicePartnerId == servicepartnerId && x.IsActive == true)
                        .ToList();

                    tblPinCodes = _pincodeRepository.GetList(x => x.CityId == cityId && x.IsActive == true).ToList();

                    foreach (var item in tblPinCodes)
                    {
                        if (mapServicePartnerCityStates.Count > 0)
                        {

                            finalPinCodes = new PinCodesModel(); // Create a new instance for each zip code

                            finalPinCodes.PinCode = item.ZipCode.ToString();

                            if (mapServicePartnerCityStates[0].ListOfPincodes.Split(',').Contains(item.ZipCode.ToString()))
                            {
// <<<<<<< Dev
//                                 var listOfPincodes = item.ListOfPincodes.Split(','); // Split the string into individual zip codes
//                                 foreach (var item2 in listOfPincodes)
//                                 {
//                                     finalPinCodes = new PinCodesModel(); // Create a new instance for each zip code

//                                     finalPinCodes.PinCode = item2;

//                                     if (item2 == tblPinCodes[0].ZipCode.ToString())
//                                     {
//                                         finalPinCodes.Active = true;
//                                     }
//                                     else
//                                     {
//                                         finalPinCodes.Active = false;
//                                     }

//                                     pinCodes.Add(finalPinCodes);

//                                     if (pinCodes.Count > 0)
//                                     {
//                                         PinCodesModelList allPincodeList = new PinCodesModelList
//                                         {
//                                             AllPinCodelistViewModel = pinCodes
//                                         };

//                                         responseResult.Status = true;
//                                         responseResult.Status_Code = HttpStatusCode.OK;
//                                         responseResult.message = "Success";
//                                         responseResult.Data = allPincodeList;
//                                     }
//                                     else
//                                     {
//                                         responseResult.Status = false;
//                                         responseResult.Status_Code = HttpStatusCode.NotFound;
//                                         responseResult.message = "No data found";
//                                     }
//                                 }
// =======
                                finalPinCodes.Active = true;
// >>>>>>> Silawat_Kranti
                            }
                            else
                            {
                                finalPinCodes.Active = false;
                            }

                            pinCodes.Add(finalPinCodes);

                            if (pinCodes.Count > 0)
                            {
                                PinCodesModelList allPincodeList = new PinCodesModelList
                                {
                                    AllPinCodelistViewModel = pinCodes
                                };

                                responseResult.Status = true;
                                responseResult.Status_Code = HttpStatusCode.OK;
                                responseResult.message = "Success";
                                responseResult.Data = allPincodeList;
                            }

                        }
                    
                        else
                        {

                            finalPinCodes = new PinCodesModel(); // Create a new instance for each zip code

                            finalPinCodes.PinCode = item.ZipCode.ToString();


                            finalPinCodes.Active = false;


                            pinCodes.Add(finalPinCodes);


                            PinCodesModelList allPincodeList = new PinCodesModelList
                            {
                                AllPinCodelistViewModel = pinCodes
                            };

                            responseResult.Status = true;
                            responseResult.Status_Code = HttpStatusCode.OK;
                            responseResult.message = "Success";
                            responseResult.Data = allPincodeList;

                        }
                      
                    }

                }
                else
                {
                    responseResult.message = "Failed. Request parameters should not be null";
                    responseResult.Status = false;
                    responseResult.Status_Code = HttpStatusCode.BadRequest;
                    return responseResult;
                }

               
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("PinCodeManager", "PinCodesByServicePartner", ex);
                responseResult.message = ex.ToString();
                responseResult.Status = false;
                responseResult.Status_Code = HttpStatusCode.InternalServerError;
            }
            return responseResult;
        }
        #endregion

    }

}
