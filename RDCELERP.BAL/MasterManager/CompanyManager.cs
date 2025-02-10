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
using RDCELERP.Model.BusinessUnit;
using RDCELERP.BAL.Helper;

namespace RDCELERP.BAL.MasterManager
{
    public class CompanyManager : ICompanyManager
    {
        #region  Variable Declaration
        ICompanyRepository _companyRepository;
        IBusinessUnitRepository _businessUnitRepository;
        IUserRoleRepository _userRoleRepository;
        private readonly IMapper _mapper;
        ILogging _logging;
        IImageHelper _imageHelper;
        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();
        IUserMappingRepository _userMappingRepository;
        IRoleRepository _roleRepository;
        #endregion

        public CompanyManager(ICompanyRepository companyRepository, IUserRoleRepository userRoleRepository, IMapper mapper, ILogging logging, IImageHelper imageHelper , IBusinessUnitRepository businessUnitRepository, IUserMappingRepository userMappingRepository, IRoleRepository roleRepository)
        {
            _companyRepository = companyRepository;
            _userRoleRepository = userRoleRepository;
            _mapper = mapper;
            _logging = logging;
            _imageHelper = imageHelper;
            _businessUnitRepository = businessUnitRepository;
            _userMappingRepository = userMappingRepository;
            _roleRepository = roleRepository;
        }

        /// <summary>
        /// Method to manage (Add/Edit) Company 
        /// </summary>
        /// <param name="CompanyVM">CompanyVM</param>
        /// <param name="userId">userId</param>
        /// <returns>int</returns>
        public int ManageCompany(CompanyViewModel CompanyVM, int userId)
        {
            TblCompany TblCompany = new TblCompany();
            bool ImageSaved = false; var FileName = "";
            try
            {
                if (CompanyVM != null)
                {
                    TblCompany = _mapper.Map<CompanyViewModel, TblCompany>(CompanyVM);
                    if (CompanyVM.CompanyLogo != null)
                    {
                        FileName = Guid.NewGuid().ToString("N") + CompanyVM.CompanyLogo.FileName;
                        string filePath = EnumHelper.DescriptionAttr(FilePathEnum.Company);
                        ImageSaved = _imageHelper.SaveFile(CompanyVM.CompanyLogo, filePath, FileName);
                    }

                    if (TblCompany.CompanyId > 0)
                    {
                        //Code to update the object
                        if (ImageSaved)
                        {
                            TblCompany.CompanyLogoUrl = FileName;
                        }
                       
                        //Code to update the object                      
                        TblCompany.ModifiedBy = userId;
                        TblCompany.ModifiedDate = _currentDatetime;
                        _companyRepository.Update(TblCompany);
                    }
                    else
                    {
                        //Code to Insert the object 
                        if (ImageSaved)
                        {
                            TblCompany.CompanyLogoUrl = FileName;
                        }
                       
                        TblCompany.IsActive = true;
                        TblCompany.CreatedDate = _currentDatetime;
                        TblCompany.CreatedBy = userId;
                        _companyRepository.Create(TblCompany);
                    }
                    _companyRepository.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("CompanyManager", "ManageCompany", ex);
            }

            return TblCompany.CompanyId;
        }

        /// <summary>
        /// Method to get the Company by id 
        /// </summary>
        /// <param name="id">CompanyId</param>
        /// <returns>CompanyViewModel</returns>
        public CompanyViewModel GetCompanyById(int id)
        {
            CompanyViewModel CompanyVM = null;
            TblCompany TblCompany = null;

            try
            {
                TblCompany = _companyRepository.GetSingle(x => x.IsActive == true && x.CompanyId == id);
                if (TblCompany != null)
                {
                    CompanyVM = _mapper.Map<TblCompany, CompanyViewModel>(TblCompany);
                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("CompanyManager", "GetCompanyById", ex);
            }
            return CompanyVM;
        }

        /// <summary>
        /// Method to get the Company by BUId 
        /// </summary>
        /// <param name="id">CompanyId</param>
        /// <returns>CompanyViewModel</returns>
        public CompanyViewModel GetCompanyByBUId(int id)
        {
            CompanyViewModel CompanyVM = null;
            TblCompany TblCompany = null;

            try
            {
                TblCompany = _companyRepository.GetSingle(x => x.IsActive == true && x.BusinessUnitId == id);
                if (TblCompany != null)
                {
                    CompanyVM = _mapper.Map<TblCompany, CompanyViewModel>(TblCompany);
                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("CompanyManager", "GetCompanyById", ex);
            }
            return CompanyVM;
        }

        /// <summary>
        /// Method to delete Company by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        public bool DeletCompanyById(int id)
        {
            bool flag = false;
            try
            {
                TblCompany TblCompany = _companyRepository.GetSingle(x => x.IsActive == true && x.CompanyId == id);
                if (TblCompany != null)
                {
                    TblCompany.IsActive = false;
                    _companyRepository.Update(TblCompany);
                    _companyRepository.SaveChanges();
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("CompanyManager", "DeletCompanyById", ex);
            }
            return flag;
        }

        /// <summary>
        /// Method to get the list of Company
        /// </summary>
        /// <param name="startIndex">startIndex</param>
        /// <param name="maxRow">maxRow</param>
        /// <param name="sidx">sidx</param>
        /// <param name="sord">sord</param>
        /// <param name="txt">txt</param>
        /// <returns>CompanyListViewModel</returns>
        public CompanyListViewModel GetCompanyList(int startIndex, int maxRow, string sidx, string sord, string txt)
        {
            List<CompanyViewModel> CompanyVMs = null;
            CompanyListViewModel CompanyList = new CompanyListViewModel();
            List<TblCompany> TblCompanylist = null;
            try
            {
                TblCompanylist = _companyRepository.GetList(x => x.IsActive == true
                && (string.IsNullOrEmpty(txt) || x.CompanyName.ToLower().Contains(txt.ToLower()))).ToList();

                if (TblCompanylist != null)
                {
                    CompanyList.Count = TblCompanylist.Count;
                    TblCompanylist = sord.Equals(SortingOrder.ASCENDING) ? TblCompanylist.OrderBy(o => o.GetType().GetProperty(sidx).GetValue(o, null)).ToList() : TblCompanylist.OrderByDescending(o => o.GetType().GetProperty(sidx).GetValue(o, null)).ToList();
                    TblCompanylist = TblCompanylist.Skip(startIndex).Take(maxRow).ToList();
                    CompanyVMs = _mapper.Map<List<TblCompany>, List<CompanyViewModel>>(TblCompanylist);
                    CompanyList.CompanyViewModelList = CompanyVMs != null ? CompanyVMs : null;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("CompanyManager", "GetCompanyList", ex);
            }
            return CompanyList;
        }

        /// <summary>
        /// Method to get the All Company
        /// </summary>     
        /// <returns>CompanyViewModel</returns>
        public IList<CompanyViewModel> GetAllCompany(int? companyId, int? roleId, int? userId)
        {
            IList<CompanyViewModel> CompanyVMs = null;
            //IList<TblCompany> TblCompanylist = null;
            List<TblCompany> TblCompanylist = new List<TblCompany>();
            TblCompany TblCompany = null;
            IList<TblUserRole> userRoleList = null;
            // TblUseRole TblUseRole = null;
            TblUserMapping tblUserMapping = null;
            List<TblUserMapping> userMappingList = null;
            TblRole tblRole = null;
            try
            {

                if (roleId != null && roleId == Convert.ToInt32(RoleEnum.SuperAdmin))
                {
                    TblCompanylist = _companyRepository.GetList(x => x.IsActive == true).ToList();
                }
                else
                {
                    if (userId != null)
                    {
                        tblRole = _roleRepository.GetSingle(x => x.IsActive == true && x.RoleId == roleId);

                        if (tblRole?.IsRoleMultiBrand == true)
                        {
                            userMappingList = _userMappingRepository.GetList(x => x.IsActive == true && x.UserId == userId).ToList();
                            if (userMappingList != null && userMappingList.Count > 0)
                            {
                                foreach (var item in userMappingList)
                                {
                                    TblCompany = _companyRepository.GetSingle(x => x.IsActive == true && x.BusinessUnitId == item.BusinessUnitId);
                                    TblCompanylist.Add(TblCompany);
                                }
                            }
                        }

                        else
                        {
                            userRoleList = _userRoleRepository.GetList(x => x.IsActive == true && x.UserId == userId).ToList();
                            if (userRoleList != null)
                                foreach (var item in userRoleList)
                                {
                                    TblCompany = _companyRepository.GetSingle(x => x.IsActive == true && x.CompanyId == item.CompanyId);
                                    TblCompanylist.Add(TblCompany);
                                }
                        }
                        //TblCompanylist = TblCompanylist1;
                    }

                }

                if (TblCompanylist != null && TblCompanylist.Count > 0)
                {
                    CompanyVMs = _mapper.Map<IList<TblCompany>, IList<CompanyViewModel>>(TblCompanylist);
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("CompanyManager", "GetAllCompany", ex);
            }
            return CompanyVMs;
        }

        /// <summary>
        /// Method to get the All Company for Assign Role to User
        /// </summary>     
        /// <returns>CompanyViewModel</returns>
        public IList<CompanyViewModel> GetCompanyToAssignRole(int? companyId, int? roleId, int? userId, int? selecteduserid)
        {
            IList<CompanyViewModel> CompanyVMs = null;
            //IList<TblCompany> TblCompanylist = null;
            List<TblCompany> TblCompanylist = new List<TblCompany>();
            TblCompany TblCompany = null;
            IList<TblUserRole> userRoleList = null;
            // TblUseRole TblUseRole = null;
            try
            {
                if (roleId != null && roleId == Convert.ToInt32(RoleEnum.SuperAdmin))
                {
                    //for SuperAdmin
                    TblCompanylist = _companyRepository.GetList(x => x.IsActive == true).ToList();
                    userRoleList = _userRoleRepository.GetList(x => x.UserId == selecteduserid).ToList();
                    List<int> companyIds = new List<int>();
                    if (userRoleList != null && userRoleList.Count > 0)
                    {
                        foreach (TblUserRole tblUserRole in userRoleList)
                        {
                            if (tblUserRole.CompanyId != null && tblUserRole.CompanyId > 0)
                                companyIds.Add(Convert.ToInt32(tblUserRole.CompanyId));
                        }
                    }
                    //
                    //TblCompanylist = TblCompanylist.Where(x => !companyIds.Contains(x.CompanyId)).ToList();
                }
                else
                {
                    if (userId != null)
                    {
                        // for Admin
                        //all company list
                        TblCompanylist = _companyRepository.GetList(x => x.IsActive == true).ToList();
                        //user role list by login userId
                        userRoleList = _userRoleRepository.GetList(x => x.IsActive == true && x.UserId == userId).ToList();
                        List<int> companyIdsforadmin = new List<int>();
                        if (userRoleList != null && userRoleList.Count > 0)
                            foreach (TblUserRole tblUserRole in userRoleList)
                            {
                                if (tblUserRole.CompanyId != null && tblUserRole.CompanyId > 0)
                                    companyIdsforadmin.Add(Convert.ToInt32(tblUserRole.CompanyId));

                            }
                        // admin companies list
                        //TblCompanylist = TblCompanylist.Where(x => companyIdsforadmin.Contains(x.CompanyId)).ToList();              

                        //for selected user
                        //selected user role list
                        userRoleList = _userRoleRepository.GetList(x => x.IsActive == true && x.UserId == selecteduserid).ToList();
                        List<int> companyIds = new List<int>();
                        if (userRoleList != null && userRoleList.Count > 0)
                        {
                            foreach (TblUserRole tblUserRole in userRoleList)
                            {                               
                                if (tblUserRole.CompanyId != null && tblUserRole.CompanyId > 0)
                                    companyIds.Add(Convert.ToInt32(tblUserRole.CompanyId));
                            }
                        }
                        //
                        //TblCompanylist = TblCompanylist.Where(x => !companyIds.Contains(x.CompanyId)).ToList();

                    }

                }

                if (TblCompanylist != null && TblCompanylist.Count > 0)
                {
                    CompanyVMs = _mapper.Map<IList<TblCompany>, IList<CompanyViewModel>>(TblCompanylist);
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("CompanyManager", "GetAllCompany", ex);
            }
            return CompanyVMs;
        }
        //Code to Get Business unit data on basis of Business unit id paird with company id
        public BusinessUnitViewModel GetBusinessUnitByCompanyId(int BusinessUnitId)
        {
            BusinessUnitViewModel businessUnitViewModel = new BusinessUnitViewModel();
            TblBusinessUnit BussinessUnitObj = null;
            try
            {
                if(BusinessUnitId > 0)
                {
                    BussinessUnitObj = _businessUnitRepository.GetSingle(x=>x.BusinessUnitId== BusinessUnitId);
                    if(BussinessUnitObj!=null)
                    {
                        businessUnitViewModel.BusinessUnitId=BussinessUnitObj.BusinessUnitId;
                        businessUnitViewModel.Name= BussinessUnitObj.Name;
                    }
                }
            }
            catch(Exception ex)
            {
                _logging.WriteErrorToDB("CompanyManager", "GetBusinessUnitByCompanyId", ex);
            }
            return businessUnitViewModel;
        }
    }
}
