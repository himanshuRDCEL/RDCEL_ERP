using AutoMapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.DAL.Entities;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Helper;
using RDCELERP.DAL.IRepository;
using RDCELERP.Model.Base;
using RDCELERP.Model.Users;
using System.IO;

namespace RDCELERP.BAL.MasterManager
{
    public class ManageUserForServicePartnerManager : IManageUserForServicePartnerManager
    {
        #region  Variable Declaration
        IUserRepository _userRepository;
        ICompanyRepository _companyRepository;
        IUserRoleRepository _userRoleRepository;
        IRoleRepository _roleRepository;
        IRoleAccessRepository _roleAccessRepository;
        IAccessListRepository _accessListRepository;
        IErrorLogManager _errorLogManager;
        IMailManager _mailManager;
        private readonly IMapper _mapper;
        ILogging _logging;
        public readonly IOptions<ApplicationSettings> _config;
        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();
        private CustomDataProtection _protector;
        
        #endregion

        #region Constructor
        public ManageUserForServicePartnerManager(IUserRepository userRepository, IAccessListRepository accessListRepository, IRoleAccessRepository roleAccessRepository, IMailManager mailManager, IOptions<ApplicationSettings> config, CustomDataProtection protector, IErrorLogManager errorLogManager, IUserRoleRepository userRoleRepository, ICompanyRepository companyRepository, IRoleRepository roleRepository, IMapper mapper, ILogging logging)
        {
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;
            _roleAccessRepository = roleAccessRepository;
            _accessListRepository = accessListRepository;
            _companyRepository = companyRepository;
            _errorLogManager = errorLogManager;
            _config = config;
            _protector = protector;
            _mailManager = mailManager;
            _mapper = mapper;
            _logging = logging;
            
        }
        #endregion

        public bool UserNotificationServicePartner(UserViewModel userObj, string subject, string tempateName)
        {
            bool flag = false;
            string sendMail = null;
            string toEmails = string.Empty;
            string content = string.Empty;

            try
            {
                if (userObj != null)
                {
                    toEmails = string.IsNullOrEmpty(userObj.Email) ? toEmails : userObj.Email;

                    var file = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "HTMLTemplate", tempateName);
                    content = File.ReadAllText(file);
                    content = content.Replace("[ToName]", userObj.FirstName);
                    content = content.Replace("[Company]", "Digi2L");
                    content = content.Replace("[Email]", toEmails);
                    content = content.Replace("[Password]", userObj.Password);
                    content = content.Replace("[BaseURL]", _config.Value.BaseURL);

                    content = content.Replace("[SupportEmail]", _config.Value.SupportEmail);


                    sendMail = _mailManager.SendEmailAsync(toEmails, content, subject).IsCompletedSuccessfully.ToString();
                    if (sendMail != null)
                    {
                        flag = true;
                    }

                }
            }
            catch (Exception ex)
            {

                _logging.WriteErrorToDB("UserManager", "UserNotificationEVC", ex);
            }
            return flag;
        }

        /// <summary>
        /// Method to manage (Add/Edit) User Role
        /// </summary>
        /// <param name="UserRoleVM">UserRoleVM</param>
        /// <param name="userId">userId</param>
        /// <returns>int</returns>
        public int ManageUserRole(UserRoleViewModel UserRoleVM, int userId, int? companyId)
        {
            TblUserRole TblUserRole = new TblUserRole();

            try
            {
                if (UserRoleVM != null)
                {
                    TblUserRole = _mapper.Map<UserRoleViewModel, TblUserRole>(UserRoleVM);


                    if (TblUserRole.UserRoleId > 0)
                    {
                        TblUserRole.ModifiedBy = userId;
                        TblUserRole.ModifiedDate = _currentDatetime;
                        TblUserRole = TrimHelper.TrimAllValuesInModel(TblUserRole);
                        _userRoleRepository.Update(TblUserRole);
                    }
                    else
                    {
                        //Code to Insert the object 
                        var check = _userRoleRepository.GetSingle(x => x.IsActive == true && x.UserId == UserRoleVM.UserId && x.RoleId == UserRoleVM.RoleId && x.CompanyId == UserRoleVM.CompanyId);
                        if (check == null)
                        {
                            TblUserRole.IsActive = true;
                            TblUserRole.CreatedDate = _currentDatetime;
                            TblUserRole.CreatedBy = userId;
                            TblUserRole = TrimHelper.TrimAllValuesInModel(TblUserRole);
                            _userRoleRepository.Create(TblUserRole);
                        }
                    }
                    _userRoleRepository.SaveChanges();


                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("UserManager", "ManageUserRole", ex);
            }

            return TblUserRole.UserRoleId;
        }


        /// <summary>
        /// Method to manage (Add/Edit) User 
        /// </summary>
        /// <param name="UserVM">UserVM</param>
        /// <param name="userId">userId</param>
        /// <returns>int</returns>
        public int ManageUser(UserViewModel UserVM, int userId, int? companyId)
        {
            TblUser TblUser = new TblUser();
            try
            {
                if (UserVM != null)
                {

                    TblUser = _mapper.Map<UserViewModel, TblUser>(UserVM);

                    if (TblUser.UserId > 0)
                    {
                        //Code to update the object
                        TblUser.Phone = SecurityHelper.EncryptString(TblUser.Phone, _config.Value.SecurityKey);
                        TblUser.Email = SecurityHelper.EncryptString(TblUser.Email, _config.Value.SecurityKey);
                        TblUser.Password = SecurityHelper.EncryptString(TblUser.Password, _config.Value.SecurityKey);
                        TblUser.ModifiedBy = userId;
                        TblUser.ModifiedDate = _currentDatetime;
                        TblUser = TrimHelper.TrimAllValuesInModel(TblUser);
                        _userRepository.Update(TblUser);
                    }
                    else
                    {
                        TblUser.Email = SecurityHelper.EncryptString(TblUser.Email, _config.Value.SecurityKey);
                        var Check = _userRepository.GetSingle(x => x.Email == TblUser.Email);
                        if (Check == null)
                        {
                            //Code to Insert the object
                            TblUser.Phone = SecurityHelper.EncryptString(TblUser.Phone, _config.Value.SecurityKey);
                            //TblUser.Email = SecurityHelper.EncryptString(TblUser.Email, _config.Value.SecurityKey);
                            TblUser.Password = SecurityHelper.EncryptString(TblUser.Password, _config.Value.SecurityKey);
                            TblUser.IsActive = true;
                            TblUser.CreatedDate = _currentDatetime;
                            TblUser.CreatedBy = userId;
                            TblUser.CompanyId = companyId;
                            TblUser = TrimHelper.TrimAllValuesInModel(TblUser);
                            _userRepository.Create(TblUser);
                        }

                    }
                    _userRepository.SaveChanges();


                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("UserManager", "ManageUser", ex);
            }

            return TblUser.UserId;
        }

        #region Method to Manage Data in tbl Role, UserRole, User and send Mail to respective User email
        public int ManageUserAndUserRole(UserViewModel userVM, int loginUserId)
        {
            int userId1 = 0;
            int userRoleId = 0;
            UserViewModel userVM1 = new UserViewModel();
            TblUser tblUser = null;
            TblUserRole tblUserRole = null;
            TblUserRole tblUserRoles = null;
            TblRole tblRole = null;
            TblRole tblRoles = null;

            UserRoleViewModel userRoleVM = new UserRoleViewModel();
            try
            {
                if (userVM != null)
                {
                    var email = SecurityHelper.EncryptString(userVM.Email, _config.Value.SecurityKey);
                    tblUser = _userRepository.GetSingle(x => x.IsActive == true && x.Email == email);
                    if (tblUser != null)
                    {
                        tblUserRole = _userRoleRepository.GetSingle(x => x.IsActive == true && x.UserId == tblUser.UserId);
                    }
                    if (tblUserRole != null)
                    {
                        tblRole = _roleRepository.GetSingle(x => x.IsActive == true && x.RoleId == tblUserRole.RoleId);
                    }
                    string fullName = userVM.Name;
                    if (userVM.Name != null)
                    {
                        userVM.SplitFullName(fullName);
                    }
                    if (userVM.Password == null)
                    {
                        userVM.Password = _config.Value.LGCLoginPossword;
                    }
                    userVM.UserStatus = "Active";
                    userVM.IsActive = true;
                    if (tblRole != null)
                    {
                        userVM.CompanyId = tblRole.CompanyId;
                    }

                    if (tblUser != null)
                    {
                        userVM.UserId = tblUser.UserId;

                    }

                    userId1 = ManageUser(userVM, loginUserId, userVM.CompanyId);
                    if (userId1 > 0)
                    {

                        tblRoles = _roleRepository.GetSingle(x => x.IsActive == true && x.RoleName == userVM.RoleName);
                        if(tblUserRole != null)
                        {
                            tblUserRoles = _userRoleRepository.GetSingle(x => x.IsActive == true && x.RoleId == tblRole.RoleId && x.UserId == userId1);
                        }
                        if(tblUserRoles != null)
                        {

                            userRoleVM.UserRoleId = tblUserRoles.UserRoleId;
                        }

                        userRoleVM.RoleId = tblRoles.RoleId;
                        userRoleVM.UserId = userId1;
                        userRoleVM.IsActive = true;
                        userRoleVM.CompanyId = tblRoles.CompanyId;
                        userRoleId = ManageUserRole(userRoleVM, loginUserId, tblRoles.CompanyId);
                        if (userRoleId > 0 && _config.Value.SendEmailFlag && userVM.MailTemplate != null && userVM.MailSubject != null)
                        {
                            bool sendNotif = UserNotificationServicePartner(userVM, userVM.MailSubject, userVM.MailTemplate);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("UserManager", "ManageUserAndUserRole", ex);
            }
            return userId1;
        }
        #endregion
    }
}
