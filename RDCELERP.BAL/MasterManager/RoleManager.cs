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
using RDCELERP.Model.Role;
using RDCELERP.Common.Constant;
using RDCELERP.DAL.Helper;
using RDCELERP.Model.BusinessUnit;
using System.IO;

namespace RDCELERP.BAL.MasterManager
{
    public class RoleManager : IRoleManager
    {
        #region  Variable Declaration
        IRoleRepository _roleRepository;
        IRoleAccessRepository _roleAccessRepository;
        IUserRoleRepository _userRoleRepository;
        ICompanyRepository _companyRepository;
        IAccessListRepository _accessListRepository;
        IBusinessUnitRepository _businessUnitRepository;
        private readonly IMapper _mapper;
        ILogging _logging;
        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();
        #endregion

        public RoleManager(IRoleRepository roleRepository, IUserRoleRepository userRoleRepository, ICompanyRepository companyRepository, IRoleAccessRepository roleAccessRepository, IAccessListRepository accessListRepository, IMapper mapper, ILogging logging, IBusinessUnitRepository businessUnitRepository)
        {
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
            _roleAccessRepository = roleAccessRepository;
            _companyRepository = companyRepository;
            _accessListRepository = accessListRepository;
            _mapper = mapper;
            _logging = logging;
            _businessUnitRepository = businessUnitRepository;
        }

        /// <summary>
        /// Method to manage (Add/Edit) Role 
        /// </summary>
        /// <param name="RoleVM">RoleVM</param>
        /// <param name="userId">userId</param>
        /// <returns>int</returns>
        public int ManageRole(RoleViewModel RoleVM, int userId, int? companyId)
        {
            TblRole TblRole = new TblRole();

            try
            {
                if (RoleVM != null)
                {
                    if (RoleVM.RoleAccessViewModelList[0].RoleId > 0)
                    {
                        //delete all RoleAccess
                        List<TblRoleAccess> roleAccesses = _roleAccessRepository.GetList(x => x.RoleId == RoleVM.RoleAccessViewModelList[0].RoleId).ToList();
                        //if(roleAccesses != null && roleAccesses.Count> 0 )
                        //{
                        //    foreach (TblRoleAccess item in roleAccesses)
                        //    {
                        //        _roleAccessRepository.Delete(item);
                        //    }
                        //    _roleAccessRepository.SaveChanges();
                        //}
                        //Code to update the object
                        TblRole = _roleRepository.GetSingle(x => x.IsActive == true && x.RoleId == RoleVM.RoleAccessViewModelList[0].RoleId);
                        if (TblRole.RoleId > 0)
                        {
                            TblRole.ModifiedBy = userId;
                            TblRole.ModifiedDate = _currentDatetime;
                            _roleRepository.Update(TblRole);
                        }
                    }
                    else
                    {
                        
                        //Code to Insert the object .c
                        if(RoleVM.RoleAccessViewModelList[0].CompanyId > 0)
                        {
                            TblRole.CompanyId = RoleVM.RoleAccessViewModelList[0].CompanyId;
                        }
                        else
                        {
                            var company = _companyRepository.GetSingle(x => x.CompanyName == RoleVM.RoleAccessViewModelList[0].CompanyName);
                            TblRole.CompanyId = company.CompanyId;
                        }
                       
                        TblRole.RoleName = RoleVM.RoleAccessViewModelList[0].RoleName;
                        TblRole.IsActive = true;
                        TblRole.CreatedDate = _currentDatetime;
                        TblRole.CreatedBy = userId;

                        _roleRepository.Create(TblRole);
                    }
                    _roleRepository.SaveChanges();

                    #region Code to save Role access list

                    if (RoleVM.RoleAccessViewModelList != null && RoleVM.RoleAccessViewModelList.Count > 0)
                    {
                        
                        if (RoleVM.RoleAccessViewModelList[0].ParentAccessListId > 0)
                        {
                            TblRoleAccess TblRoleAccess = _roleAccessRepository.GetSingle(x => x.IsActive == true && x.RoleId == TblRole.RoleId && x.AccessListId == RoleVM.RoleAccessViewModelList[0].ParentAccessListId);

                            if (TblRoleAccess != null)
                            {
                                //Code to edit
                                TblRoleAccess.CanAdd = true;
                                TblRoleAccess.CanDelete = true;
                                TblRoleAccess.CanView = true;
                                TblRoleAccess.ModifiedDate = _currentDatetime;
                                _roleAccessRepository.Update(TblRoleAccess);
                            }
                            else
                            {
                                // Create a new TblRoleAccess object
                                    TblRoleAccess = new TblRoleAccess();
                                    TblRoleAccess.AccessListId = RoleVM.RoleAccessViewModelList[0].ParentAccessListId;
                                    TblRoleAccess.CreatedDate = _currentDatetime;
                                    TblRoleAccess.CreatedBy = userId;
                                    TblRoleAccess.CanAdd = true;
                                    TblRoleAccess.CanView = true;
                                    TblRoleAccess.CanDelete = true;
                                    TblRoleAccess.RoleId = TblRole.RoleId;
                                    TblRoleAccess.IsActive = true;
                                    TblRoleAccess.CompanyId = TblRole.CompanyId;
                                    _roleAccessRepository.Create(TblRoleAccess);
                                }
                                int roleAccessId = _roleAccessRepository.SaveChanges();
                            }
                       

                        foreach (RoleAccessViewModel roleAccess in RoleVM.RoleAccessViewModelList)
                        {
                            
                             TblRoleAccess TblRoleAccess1 = _roleAccessRepository.GetSingle(x => x.IsActive == true && x.RoleId == TblRole.RoleId && x.AccessListId == roleAccess.AccessListId);
                              
                                if (TblRoleAccess1 != null)
                                {
                                    //Code to edit
                                    TblRoleAccess1.CanAdd = roleAccess.CanAdd;
                                    TblRoleAccess1.CanDelete = roleAccess.CanDelete;
                                    TblRoleAccess1.CanView = roleAccess.CanView;
                                if(roleAccess.Selected == true)
                                {
                                    TblRoleAccess1.IsActive = true;
                                }
                                else
                                {
                                    TblRoleAccess1.IsActive = false;
                                }

                                TblRoleAccess1.ModifiedDate = _currentDatetime;
                                _roleAccessRepository.Update(TblRoleAccess1);

                            }
                                else
                                {

                                    //Code to Add 
                                    TblRoleAccess1 = _mapper.Map<RoleAccessViewModel, TblRoleAccess>(roleAccess);
                                    TblRoleAccess1.CreatedDate = _currentDatetime;
                                    TblRoleAccess1.CreatedBy = userId;
                                    TblRoleAccess1.CanAdd = roleAccess.CanAdd;
                                    TblRoleAccess1.CanView = roleAccess.CanView;
                                    TblRoleAccess1.CanDelete = roleAccess.CanDelete;
                                    TblRoleAccess1.RoleId = TblRole.RoleId;
                                if (roleAccess.Selected == true)
                                {
                                    TblRoleAccess1.IsActive = true;
                                }
                                else
                                {
                                    TblRoleAccess1.IsActive = false;
                                }
                                TblRoleAccess1.CompanyId = TblRole.CompanyId;
                                _roleAccessRepository.Create(TblRoleAccess1);
                                }
                                int roleAccessId = _roleAccessRepository.SaveChanges();
                            }

                           
                       
                    }
                    #endregion


                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("RoleManager", "ManageRole", ex);
            }

            return TblRole.RoleId;
        }
        /// <summary>
        /// Method to get the Role by id 
        /// </summary>
        /// <param name="id">RoleId</param>
        /// <returns>RoleViewModel</returns>
        public RoleViewModel GetRoleById(int? id, int? tabid)
        {
            RoleViewModel RoleVM = null;
            TblRole TblRole = null;
            List<RoleAccessViewModel> ravm = null;
            try
            {
                if (id > 0)
                {
                    TblRole = _roleRepository.GetSingle(x => x.IsActive == true && x.RoleId == id);
                }
                if (TblRole != null)
                {
                    RoleVM = _mapper.Map<TblRole, RoleViewModel>(TblRole);
                }
                else
                {
                    RoleVM = new RoleViewModel();
                }
                List<TblAccessList> accessList = _accessListRepository.GetList(x => x.IsActive == true).ToList();

                List<TblRoleAccess> roleAccessList = _roleAccessRepository.GetList(x => x.IsActive == true && x.RoleId == id).ToList();

                ravm = (from al in accessList
                        join ral in roleAccessList on al.AccessListId equals ral.AccessListId
                        select new RoleAccessViewModel
                        {
                            RoleAccessId = ral.RoleAccessId,
                            AccessListId = ral.AccessListId,
                            CanAdd = (bool)ral.CanAdd,
                            CanView = (bool)ral.CanView,
                            CanDelete = (bool)ral.CanDelete,
                            Name = al.Name,
                            Description = al.Description,
                            ActionName = al.ActionName,
                            ActionUrl = al.ActionUrl,
                            ParentAccessListId = al.ParentAccessListId,

                        }).ToList();

                if (roleAccessList != null && roleAccessList.Count > 0)
                {
                    RoleVM.RoleAccessViewModelList = _mapper.Map<List<TblAccessList>, List<RoleAccessViewModel>>(accessList);
                    //RoleVM.RoleAccessViewModelList = _mapper.Map<List<TblAccessList>, List<RoleAccessViewModel>>(accessList);
                    foreach (RoleAccessViewModel item in RoleVM.RoleAccessViewModelList)
                    {
                        RoleAccessViewModel tempRoleAcc = ravm.FirstOrDefault(x => x.AccessListId == item.AccessListId);
                        if (tempRoleAcc != null)
                        {
                            if (tempRoleAcc.CanView != null)
                            {
                                item.CanView = Convert.ToBoolean(tempRoleAcc.CanView);
                            }
                            if (tempRoleAcc.CanView != null)
                            {
                                item.CanAdd = Convert.ToBoolean(tempRoleAcc.CanAdd);
                            }
                            if (tempRoleAcc.CanView != null)
                            {
                                item.CanDelete = Convert.ToBoolean(tempRoleAcc.CanDelete);
                            }
                            if (tempRoleAcc.CanView != null)
                            {
                                item.Name = tempRoleAcc.Name;
                            }
                            if (tempRoleAcc.CanView == true || tempRoleAcc.CanAdd == true || tempRoleAcc.CanDelete == true)
                                item.Selected = true;
                        }

                    }
                }
                else
                {
                    RoleVM.RoleAccessViewModelList = _mapper.Map<List<TblAccessList>, List<RoleAccessViewModel>>(accessList);
                    foreach(var item in RoleVM.RoleAccessViewModelList)
                    {
                        item.RoleId = id;
                    }
                    

                }


                //#region Logic to build Role Access VM
                //List<RoleAccessViewModel> roleAccessArrange = new List<RoleAccessViewModel>();
                //List<RoleAccessViewModel> roleAccessFullList = RoleVM.RoleAccessViewModelList;
                //roleAccessArrange = RoleVM.RoleAccessViewModelList.Where(x => x.ParentAccessListId == 10).ToList();
                //if (roleAccessArrange != null && roleAccessArrange.Count > 0)
                //{
                //    List<RoleAccessViewModel> roleAccessArrangeLevel1 = roleAccessArrange;
                //    for (int i = 0; i < roleAccessArrangeLevel1.Count; i++)
                //    {
                //        List<RoleAccessViewModel> roleAccessArrangeLevel2 = roleAccessFullList.Where(x => x.ParentAccessListId != null && x.ParentAccessListId == roleAccessArrangeLevel1[i].AccessListId).ToList();
                //        roleAccessArrange[i].ChildRoleAccessViewModelList = roleAccessArrangeLevel2;

                //    }

                //    List<RoleAccessViewModel> roleAccessArrangeLevel3 = roleAccessArrange;
                //    for (int i = 0; i < roleAccessArrangeLevel3.Count; i++)
                //    {
                //        if (roleAccessArrangeLevel3[i].ChildRoleAccessViewModelList != null && roleAccessArrangeLevel3[i].ChildRoleAccessViewModelList.Count > 0)
                //        {
                //            for (int j = 0; j < roleAccessArrangeLevel3[i].ChildRoleAccessViewModelList.Count; j++)
                //            {
                //                List<RoleAccessViewModel> roleAccessArrangeLevel4 = roleAccessFullList.Where(x => x.AccessListId == roleAccessArrangeLevel3[i].ChildRoleAccessViewModelList[j].AccessListId).ToList();
                //                roleAccessArrangeLevel3[i].ChildRoleAccessViewModelList[j].ChildRoleAccessViewModelList = roleAccessArrangeLevel4;
                //            }
                //        }
                //    }

                //    roleAccessArrange = roleAccessArrangeLevel3;
                //    RoleVM.RoleAccessViewModelList = roleAccessArrange;
                //}
                //#endregion


                List<RoleAccessViewModel> roleAccessArrange = new List<RoleAccessViewModel>();
                List<RoleAccessViewModel> roleAccessFullList = RoleVM.RoleAccessViewModelList;
                if(tabid != null)
                {
                    roleAccessArrange = roleAccessFullList.Where(x => x.ParentAccessListId == tabid).ToList();
                    if (roleAccessArrange != null && roleAccessArrange.Count > 0)
                    {
                        foreach (var level1Item in roleAccessArrange)
                        {
                            level1Item.ChildRoleAccessViewModelList = roleAccessFullList
                                .Where(x => x.ParentAccessListId == level1Item.AccessListId)
                                .ToList();

                            if (level1Item.ChildRoleAccessViewModelList != null && level1Item.ChildRoleAccessViewModelList.Count > 0)
                            {
                                foreach (var level2Item in level1Item.ChildRoleAccessViewModelList)
                                {
                                    level2Item.ChildRoleAccessViewModelList = roleAccessFullList
                                        .Where(x => x.ParentAccessListId == level2Item.AccessListId)
                                        .ToList();
                                }
                            }
                        }

                        RoleVM.RoleAccessViewModelList = roleAccessArrange;
                    }
                }
               
                


                //#region Logic to build Role Access VM
                //List<RoleAccessViewModel> roleAccessFullList = RoleVM.RoleAccessViewModelList;
                //List<RoleAccessViewModel> roleAccessArrangeLevel1 = roleAccessFullList.Where(x => x.ParentAccessListId == 10).ToList();

                //foreach (var level1Item in roleAccessArrangeLevel1)
                //{
                //    List<RoleAccessViewModel> roleAccessArrangeLevel2 = roleAccessFullList
                //        .Where(x => x.ParentAccessListId != null && x.ParentAccessListId == level1Item.AccessListId)
                //        .ToList();

                //    level1Item.ChildRoleAccessViewModelList = roleAccessArrangeLevel2;

                //    foreach (var level2Item in roleAccessArrangeLevel2)
                //    {
                //        List<RoleAccessViewModel> roleAccessArrangeLevel3 = roleAccessFullList
                //            .Where(x => x.ParentAccessListId == level2Item.AccessListId)
                //            .ToList();

                //        level2Item.ChildRoleAccessViewModelList = roleAccessArrangeLevel3;
                //    }
                //}

                //RoleVM.RoleAccessViewModelList = roleAccessArrangeLevel1;
                //#endregion


                // Initialize the top-level items
                //List<RoleAccessViewModel> roleAccessFullList = RoleVM.RoleAccessViewModelList;
                //List<RoleAccessViewModel> roleAccessArrange = roleAccessFullList.Where(x => x.ParentAccessListId == 10).ToList();

                //if (roleAccessArrange != null && roleAccessArrange.Count > 0)
                //{
                //    for (int i = 0; i < roleAccessArrange.Count; i++)
                //    {
                //        // Level 1: Find level 2 items and populate child list
                //        List<RoleAccessViewModel> roleAccessArrangeLevel2 = roleAccessFullList
                //            .Where(x => x.ParentAccessListId == roleAccessArrange[i].AccessListId)
                //            .ToList();
                //        roleAccessArrange[i].ChildRoleAccessViewModelList = roleAccessArrangeLevel2;

                //        for (int j = 0; j < roleAccessArrangeLevel2.Count; j++)
                //        {
                //            // Level 2: Find level 3 items and populate child list
                //            List<RoleAccessViewModel> roleAccessArrangeLevel3 = roleAccessFullList
                //                .Where(x => x.AccessListId == roleAccessArrangeLevel2[j].AccessListId)
                //                .ToList();
                //            roleAccessArrangeLevel2[j].ChildRoleAccessViewModelList = roleAccessArrangeLevel3;

                //            for (int k = 0; k < roleAccessArrangeLevel3.Count; k++)
                //            {
                //                // Level 3: Find level 4 items and populate child list
                //                List<RoleAccessViewModel> roleAccessArrangeLevel4 = roleAccessFullList
                //                    .Where(x => x.AccessListId == roleAccessArrangeLevel3[k].AccessListId)
                //                    .ToList();
                //                roleAccessArrangeLevel3[k].ChildRoleAccessViewModelList = roleAccessArrangeLevel4;
                //            }
                //        }
                //    }

                //    // Set the resulting hierarchy
                //    RoleVM.RoleAccessViewModelList = roleAccessArrange;
                //}

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("RoleManager", "GetRoleById", ex);
            }
            return RoleVM;
        }

        /// <summary>
        /// Method to get Role by userId
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        public RoleViewModel GetRoleByUserId(int? userId)
        {
             RoleViewModel RoleVM = null;
             TblRole TblRole = null;
            TblUserRole TblUserRole = null;
            try
            {
                if (userId > 0)
                {
                    TblUserRole = _userRoleRepository.GetSingle(x => x.IsActive == true && x.UserId == userId);
                    if (TblUserRole != null)
                    {
                        TblRole = _roleRepository.GetSingle(x => x.IsActive == true && x.RoleId == TblUserRole.RoleId);                        
                    }
                }
                if (TblRole != null)
                {
                    RoleVM = _mapper.Map<TblRole, RoleViewModel>(TblRole);
                    if (TblRole.CompanyId != null)
                    {
                        TblCompany TblCompany = _companyRepository.GetCompanyId(TblRole.CompanyId);
                        if (TblCompany != null)
                        {
                            //int Buid = 1;
                            TblCompany = _companyRepository.GetCompanyByBUId(TblCompany.BusinessUnitId);
                           // TblCompany = _companyRepository.GetCompanyByBUId(Buid);
                            TblBusinessUnit TblBusinessUnit = _businessUnitRepository.Getbyid(TblCompany.BusinessUnitId);
                            if (TblBusinessUnit != null)
                            {
                                RoleVM.BusinessUnitViewModel = new BusinessUnitViewModel();
                                RoleVM.BusinessUnitViewModel.BusinessUnitId = TblBusinessUnit.BusinessUnitId;
                            }
                        }
                    }
                }
                else
                {
                    RoleVM = new RoleViewModel();
                }
                List<TblAccessList> accessList = _accessListRepository.GetList(x => x.IsActive == true).ToList();
                List<TblRoleAccess> roleAccessList = null;

                //Sir ka code
                if (RoleVM.RoleId != Convert.ToInt32(RoleEnum.SuperAdmin))
                    roleAccessList = _roleAccessRepository.GetList(x => x.IsActive == true && x.RoleId == RoleVM.RoleId).ToList();

                //new
                // roleAccessList = _roleAccessRepository.GetList(x => x.IsActive == true).ToList();

                if (roleAccessList != null && roleAccessList.Count > 0)
                {
                    RoleVM.RoleAccessViewModelList = _mapper.Map<List<TblRoleAccess>, List<RoleAccessViewModel>>(roleAccessList);
                    foreach (RoleAccessViewModel item in RoleVM.RoleAccessViewModelList)
                    {
                        TblAccessList accessListtemp = accessList.FirstOrDefault(x => x.IsActive == true && x.AccessListId == item.AccessListId);
                        if(accessListtemp != null)
                        {
                            item.SetIcon = !string.IsNullOrEmpty(accessListtemp.SetIcon) ? accessListtemp.SetIcon : string.Empty;
                            item.Name = !string.IsNullOrEmpty(accessListtemp.Name) ? accessListtemp.Name : string.Empty; 
                            item.ParentAccessListId = accessListtemp.ParentAccessListId != null ? accessListtemp.ParentAccessListId : null;
                            item.ActionUrl = !string.IsNullOrEmpty(accessListtemp.ActionUrl) ? accessListtemp.ActionUrl : string.Empty; 
                            item.ActionName = !string.IsNullOrEmpty(accessListtemp.ActionName) ? accessListtemp.ActionName : string.Empty; 
                            item.IsMenu = accessListtemp.IsMenu != null ? accessListtemp.IsMenu :false;  
                        }
                    }
                }
                else
                {
                    RoleVM.RoleAccessViewModelList = _mapper.Map<List<TblAccessList>, List<RoleAccessViewModel>>(accessList);
                    foreach (RoleAccessViewModel item in RoleVM.RoleAccessViewModelList)
                    {
                        item.Selected = true;
                        item.CanAdd = true;
                        item.CanDelete = true;
                        item.CanView = true;
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("RoleManager", "GetRoleByUserId", ex);
            }
            return RoleVM;
        }

        /// <summary>
        /// Method to delete Role by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        public bool DeletRoleById(int id)
        {
            bool flag = false;
            try
            {
                TblRole TblRole = _roleRepository.GetSingle(x => x.IsActive == true && x.RoleId == id);
                if (TblRole != null)
                {
                    TblRole.IsActive = false;
                    _roleRepository.Update(TblRole);
                    _roleRepository.SaveChanges();
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("RoleManager", "DeletRoleById", ex);
            }
            return flag;
        }

        /// <summary>
        /// Method to get the list of Role
        /// </summary>
        /// <param name="startIndex">startIndex</param>
        /// <param name="maxRow">maxRow</param>
        /// <param name="sidx">sidx</param>
        /// <param name="sord">sord</param>
        /// <param name="txt">txt</param>
        /// <returns>RoleListViewModel</returns>
        public RoleListViewModel GetRoleList(int startIndex, int maxRow, string sidx, string sord, string txt)
        {
            List<RoleViewModel> RoleVMs = null;
            RoleListViewModel RoleList = new RoleListViewModel();
            List<TblRole> TblRolelist = null;
            try
            {
                TblRolelist = _roleRepository.GetList(x => x.IsActive == true
                && (string.IsNullOrEmpty(txt) || x.RoleName.ToLower().Contains(txt.ToLower()))).ToList();

                if (TblRolelist != null)
                {
                    RoleList.Count = TblRolelist.Count;
                    TblRolelist = sord.Equals(SortingOrder.ASCENDING) ? TblRolelist.OrderBy(o => o.GetType().GetProperty(sidx).GetValue(o, null)).ToList() : TblRolelist.OrderByDescending(o => o.GetType().GetProperty(sidx).GetValue(o, null)).ToList();
                    TblRolelist = TblRolelist.Skip(startIndex).Take(maxRow).ToList();
                    RoleVMs = _mapper.Map<List<TblRole>, List<RoleViewModel>>(TblRolelist);
                    RoleList.RoleViewModelList = RoleVMs != null ? RoleVMs : null;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("RoleManager", "GetRoleList", ex);
            }
            return RoleList;
        }

        public IList<RoleViewModel> GetAllRole(int? companyId)
        {
            IList<RoleViewModel> RoleVMList = null;
            IList<TblRole> TblRoleList = null;

            try
            {
                if (companyId > 0)

                    TblRoleList = _roleRepository.GetList(x => x.IsActive == true && x.CompanyId == companyId).ToList();

                if (TblRoleList != null && TblRoleList.Count > 0)
                {
                    RoleVMList = _mapper.Map<IList<TblRole>, IList<RoleViewModel>>(TblRoleList);
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("RoleManager", "GetAllRole", ex);
            }
            return RoleVMList;
        }

        public IList<RoleViewModel> GetRoleByCompanyId(int? companyId)
        {
            IList<RoleViewModel> RoleVM = null;
            //RoleViewModel RoleVM = null;
            //IList<TblRole> TblRoleList = null;

            try
            {

                if (companyId > 0)
                {
                    // List<TblUserRole> userRoleList = _userRoleRepository.GetList(x => x.IsActive == true).ToList();

                    List<TblRole> roleList = _roleRepository.GetList(x => x.IsActive == true && x.CompanyId == companyId).ToList();

                    //if (userRoleList != null && roleList != null)
                    //{
                    //    RoleVM = (from al in userRoleList
                    //              join ral in roleList on al.CompanyId equals ral.CompanyId
                    //                select new RoleViewModel
                    //                {
                    //                    RoleId = ral.RoleId,
                    //                    RoleName = ral.RoleName,
                    //                }).ToList();                     
                    //}

                    if (roleList != null && roleList.Count > 0)
                    {
                        RoleVM = _mapper.Map<IList<TblRole>, IList<RoleViewModel>>(roleList);
                    }
                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("RoleManager", "GetRoleByCompanyId", ex);
            }
            return RoleVM;
        }

        /// <summary>
        /// Method to get the All Role
        /// </summary>     
        /// <returns>RoleViewModel</returns>
        public IList<RoleViewModel> GetRoleListByUserId(int? companyId, int? roleId, int? userId)
        {
            IList<RoleViewModel> RoleVM = null;
            List<TblRole> TblRolelist = new List<TblRole>();
            List<TblRole> TblRole = null;
            TblRole TblRoleObj = null;
            List<TblCompany> TblCompanylist = new List<TblCompany>();
            TblCompany TblCompany = null;
            IList<TblUserRole> userRoleList = null;

            try
            {

                //this case for only Super Admin
                if (userId == 3)
                {
                    TblRolelist = _roleRepository.GetList(x => x.IsActive == true && x.CompanyId != null).OrderByDescending(x => x.CompanyId).ToList();
                }
                else
                {
                    if (userId != null)
                        userRoleList = _userRoleRepository.GetList(x => x.IsActive == true && x.UserId == userId).ToList();

                    if (userRoleList != null)
                    {
                        foreach (var item in userRoleList)
                        {
                            //if(companyId != null)
                            TblCompany = _companyRepository.GetSingle(x => x.IsActive == true && x.CompanyId == item.CompanyId);

                            TblCompanylist.Add(TblCompany);
                        }
                    }

                    if (TblCompanylist != null)
                    {
                        foreach (var item in TblCompanylist)
                        {
                            if (item.CompanyId > 0)
                                TblRole = _roleRepository.GetList(x => x.IsActive == true && x.CompanyId == item.CompanyId).ToList();

                            if (TblRole != null)
                                foreach (var role in TblRole)
                                {
                                    TblRolelist.Add(role);
                                }
                        }
                    }
                }

                if (TblRolelist != null && TblRolelist.Count > 0)
                {
                    TblRole = new List<TblRole>();
                    foreach (var item in TblRolelist)
                    {
                        TblRoleObj = new TblRole();
                        if (item.CompanyId > 0)
                        {
                            TblCompany = _companyRepository.GetSingle(x => x.IsActive == true && x.CompanyId == item.CompanyId);

                            if (TblCompany != null)
                            {
                                TblRoleObj = item;
                                TblRole.Add(TblRoleObj);
                            }
                        }

                    }

                    //Role filtered list
                    TblRolelist = TblRole;
                }

                if (TblRolelist != null && TblRolelist.Count > 0)
                {
                    RoleVM = _mapper.Map<IList<TblRole>, IList<RoleViewModel>>(TblRolelist);
                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("RoleManager", "GetRoleListByUserId", ex);
            }
            return RoleVM;
        }

        /// <summary>
        /// Method to get Role by userId and CompanyId
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        public RoleViewModel GetRoleByUserIdAndCompanyId(int? userId, int? companyId)
        {
            RoleViewModel RoleVM = null;
            TblRole TblRole = null;
            TblUserRole TblUserRole = null;
            try
            {
                if (userId == 3 && companyId > 0)
                {
                    TblUserRole = _userRoleRepository.GetSingle(x => x.IsActive == true && x.UserId == 3);
                    if (TblUserRole != null)
                    {
                        TblRole = _roleRepository.GetSingle(x => x.IsActive == true && x.RoleId == TblUserRole.RoleId);
                        TblRole.CompanyId = companyId;
                    }
                }
                else
                {                   
                    if (userId > 0 && companyId > 0)
                    {
                        TblUserRole = _userRoleRepository.GetSingle(x => x.IsActive == true && x.UserId == userId && x.CompanyId == companyId);
                        if (TblUserRole != null)
                        {
                            TblRole = _roleRepository.GetSingle(x => x.IsActive == true && x.RoleId == TblUserRole.RoleId);
                        }
                    }
                }

                if (TblRole != null)
                {
                    RoleVM = _mapper.Map<TblRole, RoleViewModel>(TblRole);
                }
                else
                {
                    RoleVM = new RoleViewModel();
                }
                List<TblAccessList> accessList = _accessListRepository.GetList(x => x.IsActive == true).ToList();
                List<TblRoleAccess> roleAccessList = null;

                //Sir ka code
                if (RoleVM.RoleId != Convert.ToInt32(RoleEnum.SuperAdmin))
                    roleAccessList = _roleAccessRepository.GetList(x => x.IsActive == true && x.RoleId == RoleVM.RoleId).ToList();

                //new
                // roleAccessList = _roleAccessRepository.GetList(x => x.IsActive == true).ToList();

                if (roleAccessList != null && roleAccessList.Count > 0)
                {
                    RoleVM.RoleAccessViewModelList = _mapper.Map<List<TblRoleAccess>, List<RoleAccessViewModel>>(roleAccessList);
                    foreach (RoleAccessViewModel item in RoleVM.RoleAccessViewModelList)
                    {
                        TblAccessList accessListtemp = accessList.FirstOrDefault(x => x.IsActive == true && x.AccessListId == item.AccessListId);
                        if (accessListtemp != null)
                        {
                            item.SetIcon = !string.IsNullOrEmpty(accessListtemp.SetIcon) ? accessListtemp.SetIcon : string.Empty;
                            item.Name = !string.IsNullOrEmpty(accessListtemp.Name) ? accessListtemp.Name : string.Empty;
                            item.ParentAccessListId = accessListtemp.ParentAccessListId != null ? accessListtemp.ParentAccessListId : null;
                            item.ActionUrl = !string.IsNullOrEmpty(accessListtemp.ActionUrl) ? accessListtemp.ActionUrl : string.Empty;
                            item.ActionName = !string.IsNullOrEmpty(accessListtemp.ActionName) ? accessListtemp.ActionName : string.Empty;
                            item.IsMenu = accessListtemp.IsMenu != null ? accessListtemp.IsMenu : false;
                        }
                    }
                }
                else
                {
                    RoleVM.RoleAccessViewModelList = _mapper.Map<List<TblAccessList>, List<RoleAccessViewModel>>(accessList);
                    foreach (RoleAccessViewModel item in RoleVM.RoleAccessViewModelList)
                    {
                        item.Selected = true;
                        item.CanAdd = true;
                        item.CanDelete = true;
                        item.CanView = true;
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("RoleManager", "GetRoleByUserId", ex);
            }
            return RoleVM;
        }

        /// <summary>
        /// Method to get Role by userId, CompanyId and BusinessUnitId
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        public RoleViewModel GetRoleByUserIdAndCompanyIdBUId(int? userId, int? companyId, int? BUId)
        {
            RoleViewModel RoleVM = null;
            TblRole TblRole = null;
            TblUserRole TblUserRole = null;
            try
            {
                TblRole roleObj = null;
                TblUserRole userRole = _userRoleRepository.GetSingle(x => x.IsActive == true && x.UserId == userId);
                if (userRole != null)
                {
                    roleObj = _roleRepository.GetSingle(x => x.IsActive == true && x.RoleId == userRole.RoleId);
                }


                if (userId == 3 && companyId > 0)
                {
                    TblUserRole = _userRoleRepository.GetSingle(x => x.IsActive == true && x.UserId == 3);
                    if (TblUserRole != null)
                    {
                        TblRole = _roleRepository.GetSingle(x => x.IsActive == true && x.RoleId == TblUserRole.RoleId);
                        TblRole.CompanyId = companyId;
                    }
                }
                else if (roleObj.IsRoleMultiBrand == true)
                {
                    TblUserRole = _userRoleRepository.GetSingle(x => x.IsActive == true && x.UserId == userId);
                    if (TblUserRole != null)
                    {
                        TblRole = _roleRepository.GetSingle(x => x.IsActive == true && x.RoleId == TblUserRole.RoleId);
                        TblRole.CompanyId = companyId;
                    }
                }
                else
                {
                    if (userId > 0 && companyId > 0)
                    {
                        TblCompany TblCompany = _companyRepository.GetCompanyByBUId(BUId);
                        if (TblCompany != null)
                        {
                            TblUserRole = _userRoleRepository.GetSingle(x => x.IsActive == true && x.UserId == userId && x.CompanyId == companyId);
                            if (TblUserRole != null)
                            {
                                TblRole = _roleRepository.GetSingle(x => x.IsActive == true && x.RoleId == TblUserRole.RoleId);
                                if (TblRole != null)
                                {
                                    RoleVM = _mapper.Map<TblRole, RoleViewModel>(TblRole);
                                }
                                else
                                {
                                        RoleVM = new RoleViewModel();
                                }
                                  //added
                                TblBusinessUnit TblBusinessUnit = _businessUnitRepository.Getbyid(BUId);
                                if (TblBusinessUnit != null)
                                {
                                    RoleVM.BusinessUnitViewModel = new BusinessUnitViewModel();
                                    RoleVM.BusinessUnitViewModel.BusinessUnitId = TblBusinessUnit.BusinessUnitId;
                                    RoleVM.BusinessUnitViewModel.Name= TblBusinessUnit.Name;
                                }
                                //end
                            }
                        }                           
                    }
                }

                if (TblRole != null)
                {
                    RoleVM = _mapper.Map<TblRole, RoleViewModel>(TblRole);
                }
                else
                {
                    RoleVM = new RoleViewModel();
                }
                List<TblAccessList> accessList = _accessListRepository.GetList(x => x.IsActive == true).ToList();
                List<TblRoleAccess> roleAccessList = null;

                //Sir ka code
                if (RoleVM.RoleId != Convert.ToInt32(RoleEnum.SuperAdmin))
                    roleAccessList = _roleAccessRepository.GetList(x => x.IsActive == true && x.RoleId == RoleVM.RoleId).ToList();

                //new
                // roleAccessList = _roleAccessRepository.GetList(x => x.IsActive == true).ToList();

                if (roleAccessList != null && roleAccessList.Count > 0)
                {
                    RoleVM.RoleAccessViewModelList = _mapper.Map<List<TblRoleAccess>, List<RoleAccessViewModel>>(roleAccessList);
                    foreach (RoleAccessViewModel item in RoleVM.RoleAccessViewModelList)
                    {
                        item.SetIcon = accessList.FirstOrDefault(x => x.IsActive == true && x.AccessListId == item.AccessListId)?.SetIcon;
                        item.Name = accessList.FirstOrDefault(x => x.IsActive == true && x.AccessListId == item.AccessListId)?.Name;
                        item.ParentAccessListId = accessList.FirstOrDefault(x => x.IsActive == true && x.AccessListId == item.AccessListId)?.ParentAccessListId;
                        item.ActionUrl = accessList.FirstOrDefault(x => x.IsActive == true && x.AccessListId == item.AccessListId)?.ActionUrl;
                        item.ActionName = accessList.FirstOrDefault(x => x.IsActive == true && x.AccessListId == item.AccessListId)?.ActionName;
                        item.IsMenu = accessList.FirstOrDefault(x => x.IsActive == true && x.AccessListId == item.AccessListId)?.IsMenu;
                    }
                }
                else
                {
                    RoleVM.RoleAccessViewModelList = _mapper.Map<List<TblAccessList>, List<RoleAccessViewModel>>(accessList);
                    foreach (RoleAccessViewModel item in RoleVM.RoleAccessViewModelList)
                    {
                        item.Selected = true;
                        item.CanAdd = true;
                        item.CanDelete = true;
                        item.CanView = true;
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("RoleManager", "GetRoleByUserId", ex);
            }
            return RoleVM;
        }

    }
}
