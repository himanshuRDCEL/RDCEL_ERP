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
using RDCELERP.Model.VehicleIncentive;

namespace RDCELERP.BAL.MasterManager
{
    public class VehicleIncentiveManager : IVehicleIncentiveManager
    {
        #region  Variable Declaration
        IProductCategoryRepository _ProductCategoryRepository;
        IProductTypeRepository _ProductTypeRepository;
        private readonly IMapper _mapper;
        ILogging _logging;
        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();
        IVehicleIncentiveRepository _vehicleIncentiveRepository;
        IBusinessUnitRepository _businessUnitRepository;
        IProductCategoryRepository _productCategoryRepository;
        #endregion

        public VehicleIncentiveManager(IProductCategoryRepository productCategoryRepository, IProductTypeRepository ProductTypeRepository, IVehicleIncentiveRepository vehicleIncentiveRepository, IBusinessUnitRepository businessUnitRepository, IMapper mapper, ILogging logging)
        {
            _ProductCategoryRepository = productCategoryRepository;
          
            _mapper = mapper;
            _logging = logging;
            _vehicleIncentiveRepository = vehicleIncentiveRepository;
            _businessUnitRepository = businessUnitRepository;
            _productCategoryRepository = productCategoryRepository;
            _ProductTypeRepository = ProductTypeRepository;
        }

        /// <summary>
        /// Method to manage (Add/Edit) VehicleIncentive 
        /// </summary>
        /// <param name="VehicleIncentiveVM">VehicleIncentiveVM</param>
        /// <param name="userId">userId</param>
        /// <returns>int</returns>
        public int ManageVehicleIncentive(VehicleIncentiveViewModel VehicleIncentiveVM, int userId)
        {
            TblVehicleIncentive TblVehicleIncentive = new TblVehicleIncentive();

            try
            {
                if (VehicleIncentiveVM != null)
                {
                    TblVehicleIncentive = _mapper.Map<VehicleIncentiveViewModel, TblVehicleIncentive>(VehicleIncentiveVM);


                    if (TblVehicleIncentive.IncentiveId > 0)
                    {
                        //Code to update the object                      
                        TblVehicleIncentive.ModifiedBy = userId;
                        TblVehicleIncentive.ModifiedDate = _currentDatetime;
                        TblVehicleIncentive = TrimHelper.TrimAllValuesInModel(TblVehicleIncentive);
                        _vehicleIncentiveRepository.Update(TblVehicleIncentive);
                    }
                    else
                    {

                        //Code to Insert the object 
                        TblVehicleIncentive.IsActive = true;
                        TblVehicleIncentive.CreatedDate = _currentDatetime;
                        TblVehicleIncentive.CreatedBy = userId;
                        TblVehicleIncentive = TrimHelper.TrimAllValuesInModel(TblVehicleIncentive);
                        _vehicleIncentiveRepository.Create(TblVehicleIncentive);
                      
                    }
                    _vehicleIncentiveRepository.SaveChanges();


                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("VehicleIncentiveManager", "ManageVehicleIncentive", ex);
            }

            return TblVehicleIncentive.IncentiveId;
        }

        #region
        //Method for Bulk Upload
        public VehicleIncentiveViewModel ManageVehicleIncentiveBulk(VehicleIncentiveViewModel VehicleIncentiveVM, int userId)
        {
            TblBusinessUnit TblBusinessUnit = new TblBusinessUnit();
            TblProductType TblProductType = new TblProductType();
            TblProductCategory TblProductCategory = new TblProductCategory();
            List<TblVehicleIncentive> tblVehicleIncentive = new List<TblVehicleIncentive>();

            if (VehicleIncentiveVM != null && VehicleIncentiveVM.VehicleIncentiveVMList != null && VehicleIncentiveVM.VehicleIncentiveVMList.Count > 0)
            {
                var ValidatedVehicleIncentiveList = VehicleIncentiveVM.VehicleIncentiveVMList.Where(x => x.Remarks == null || string.IsNullOrEmpty(x.Remarks)).ToList();
                VehicleIncentiveVM.VehicleIncentiveVMErrorList = VehicleIncentiveVM.VehicleIncentiveVMList.Where(x => x.Remarks != null && !string.IsNullOrEmpty(x.Remarks)).ToList();
                

                if (ValidatedVehicleIncentiveList != null && ValidatedVehicleIncentiveList.Count > 0)
                {
                    foreach (var item in ValidatedVehicleIncentiveList)
                    {
                        try
                        {
                            TblBusinessUnit = _businessUnitRepository.GetSingle(where: x => x.Name == item.Company);
                            TblProductCategory = _productCategoryRepository.GetSingle(where: x => x.Description == item.ProductCategory);
                            TblProductType = _ProductTypeRepository.GetSingle(where: x => x.Description == item.ProductType);
                            if (item.IncentiveId > 0)
                            {

                                TblVehicleIncentive TblVehicleIncentive = new TblVehicleIncentive();
                                //Code to update the object 
                                TblVehicleIncentive.BusinessUnitId = TblBusinessUnit.BusinessUnitId;
                                TblVehicleIncentive.ProductCategoryId = TblProductCategory.Id;
                                TblVehicleIncentive.ProductTypeId = TblProductType.Id;
                                TblVehicleIncentive.BasePrice = item.BasePrice;
                                if (item.PickupTatinHr != null)
                                {

                                    string pickuptime = (string)item.PickupTatinHr;

                                    if (TimeSpan.TryParse(pickuptime, out TimeSpan pickupTimeSpan))
                                    {
                   
                                        TblVehicleIncentive.PickupTatinHr = pickupTimeSpan;
                                    }

                                }
                                TblVehicleIncentive.PickupIncAmount = item.PickupIncAmount;
                                if (item.DropTatinHr != null)
                                {

                                    string droptime = (string)item.DropTatinHr;

                                    if (TimeSpan.TryParse(droptime, out TimeSpan pickupTimeSpan))
                                    {

                                        TblVehicleIncentive.DropTatinHr = pickupTimeSpan;
                                    }

                                }
                                TblVehicleIncentive.DropIncAmount = item.DropIncAmount;
                                TblVehicleIncentive.PackagingIncentive = item.PackagingIncentive;
                                TblVehicleIncentive.DropImageIncentive = item.DropImageIncentive;
                                TblVehicleIncentive.ModifiedDate = _currentDatetime;
                                TblVehicleIncentive.ModifiedBy = userId;
                                _vehicleIncentiveRepository.Update(TblVehicleIncentive);
                                _vehicleIncentiveRepository.SaveChanges();

                            }
                            else
                            {


                                TblVehicleIncentive TblVehicleIncentive = new TblVehicleIncentive();
                                //Code to update the object 
                                TblVehicleIncentive.BusinessUnitId = TblBusinessUnit.BusinessUnitId;
                                TblVehicleIncentive.ProductCategoryId = TblProductCategory.Id;
                                TblVehicleIncentive.ProductTypeId = TblProductType.Id;
                                TblVehicleIncentive.BasePrice = item.BasePrice;
                                if (item.PickupTatinHr != null)
                                {

                                    string pickuptime = (string)item.PickupTatinHr;

                                    if (double.TryParse(pickuptime, out double hours))
                                    {
                                        TimeSpan pickupTimeSpan = TimeSpan.FromHours(hours);
                                        TblVehicleIncentive.PickupTatinHr = pickupTimeSpan;
                                    }

                                }
                                TblVehicleIncentive.PickupIncAmount = item.PickupIncAmount;
                                if (item.DropTatinHr != null)
                                {

                                    string droptime = (string)item.DropTatinHr;

                                    if (double.TryParse(droptime, out double hours))
                                    {
                                        TimeSpan dropTimeSpan = TimeSpan.FromHours(hours);
                                        TblVehicleIncentive.DropTatinHr = dropTimeSpan;
                                    }

                                }
                                TblVehicleIncentive.DropIncAmount = item.DropIncAmount;
                                TblVehicleIncentive.PackagingIncentive = item.PackagingIncentive;
                                TblVehicleIncentive.DropImageIncentive = item.DropImageIncentive;
                                TblVehicleIncentive.IsActive = true;
                                TblVehicleIncentive.CreatedDate = _currentDatetime;
                                TblVehicleIncentive.CreatedBy = userId;
                                _vehicleIncentiveRepository.Create(TblVehicleIncentive);
                                _vehicleIncentiveRepository.SaveChanges();

                            }
                        }

                        catch (Exception ex)
                        {
                            item.Remarks += ex.Message + ", ";
                            VehicleIncentiveVM.VehicleIncentiveVMList.Add(item);
                        }
                    }
                }
            }

            return VehicleIncentiveVM;
        }

        #endregion

        /// <summary>
        /// Method to get the VehicleIncentive by id 
        /// </summary>
        /// <param name="id">VehicleIncentiveId</param>
        /// <returns>VehicleIncentiveViewModel</returns>
        public VehicleIncentiveViewModel GetVehicleIncentiveById(int id)
        {
            VehicleIncentiveViewModel VehicleIncentiveVM = null;
            TblVehicleIncentive TblVehiceIncentive = null;

            try
            {
                TblVehiceIncentive = _vehicleIncentiveRepository.GetSingle(where: x => x.IsActive == true && x.IncentiveId == id);
                if (TblVehiceIncentive != null)
                {
                    VehicleIncentiveVM = _mapper.Map<TblVehicleIncentive,VehicleIncentiveViewModel>(TblVehiceIncentive);
                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("VehicleIncentiveManager", "GetVehicleIncentiveById", ex);
            }
            return VehicleIncentiveVM;
        }

        /// <summary>
        /// Method to delete VehicleIncentive by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        public bool DeletVehicleIncentiveById(int id)
        {
            bool flag = false;
            try
            {
                TblVehicleIncentive TblVehicleIncentive = _vehicleIncentiveRepository.GetSingle(x => x.IsActive == true && x.IncentiveId == id);
                if (TblVehicleIncentive != null)
                {
                    TblVehicleIncentive.IsActive = false;
                    _vehicleIncentiveRepository.Update(TblVehicleIncentive);
                    _vehicleIncentiveRepository.SaveChanges();
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("VehicleIncentiveManager", "DeleteVehicleIncentiveById", ex);
            }
            return flag;
        }

        /// <summary>
        /// Method to get the All Vehicle Incentive
        /// </summary>     
        /// <returns>VehiclIncentiveViewModel</returns>
        public IList<VehicleIncentiveViewModel> GetAllVehiclIncentive()
        {
            IList<VehicleIncentiveViewModel> VehicleIncentiveVMList = null;
            List<TblVehicleIncentive> TblVehicleIncentivelist = new List<TblVehicleIncentive>();
            // TblUseRole TblUseRole = null;
            try
            {

                TblVehicleIncentivelist = _vehicleIncentiveRepository.GetList(x => x.IsActive == true).ToList();

                if (TblVehicleIncentivelist != null && TblVehicleIncentivelist.Count > 0)
                {
                    VehicleIncentiveVMList = _mapper.Map<IList<TblVehicleIncentive>, IList<VehicleIncentiveViewModel>>(TblVehicleIncentivelist);
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("VehicleIncentiveManager", "GetAllVehicleIncentive", ex);
            }
            return VehicleIncentiveVMList;
        }

      
       
    }
}
