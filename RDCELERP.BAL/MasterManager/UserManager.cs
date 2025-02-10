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
using RDCELERP.DAL.Helper;
using RDCELERP.DAL.Repository;
using RDCELERP.Common.Constant;
using System.IO;
using Microsoft.Extensions.Options;
using RDCELERP.Model.Base;
using RDCELERP.Model.InfoMessage;
using RDCELERP.Model.Role;
using RDCELERP.Model.MobileApplicationModel;
using System.Net;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Mailjet.Client.Resources;
using RDCELERP.Model.MobileApplicationModel.Common;
using RDCELERP.Model.MobileApplicationModel.LGC;
using RDCELERP.Model.DriverDetails;

namespace RDCELERP.BAL.MasterManager
{
    public class UserManager : IUserManager
    {
        #region  Variable Declaration
        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();
        private CustomDataProtection _protector;
        IUserRepository _userRepository;
        ICompanyRepository _companyRepository;
        IUserRoleRepository _userRoleRepository;
        IRoleRepository _roleRepository;
        IRoleAccessRepository _roleAccessRepository;
        IAccessListRepository _accessListRepository;
        IErrorLogManager _errorLogManager;
        IMailManager _mailManager;
        IMapper _mapper;
        ILogging _logging;
        IOptions<ApplicationSettings> _config;
        ILogin_MobileRepository _Login_MobileRepository;
        IServicePartnerManager _servicePartnerManager;
        IServicePartnerRepository _servicePartnerRepository;
        IDriverDetailsRepository _driverDetailsRepository;
        IDriverDetailsManager _driverDetailsManager;
        IMapLoginUserDeviceRepository _mapLoginUserDeviceRepository;
        #endregion

        #region Constructor
        public UserManager(IUserRepository userRepository, ICompanyRepository companyRepository, IUserRoleRepository userRoleRepository, IRoleRepository roleRepository, IRoleAccessRepository roleAccessRepository, IAccessListRepository accessListRepository, IErrorLogManager errorLogManager, IMailManager mailManager, IMapper mapper, ILogging logging, IOptions<ApplicationSettings> config, ILogin_MobileRepository login_MobileRepository, IServicePartnerManager servicePartnerManager, IServicePartnerRepository servicePartnerRepository, IDriverDetailsRepository driverDetailsRepository, IDriverDetailsManager driverDetailsManager, CustomDataProtection protector, IMapLoginUserDeviceRepository mapLoginUserDeviceRepository)
        {
            _userRepository = userRepository;
            _companyRepository = companyRepository;
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;
            _roleAccessRepository = roleAccessRepository;
            _accessListRepository = accessListRepository;
            _errorLogManager = errorLogManager;
            _mailManager = mailManager;
            _mapper = mapper;
            _logging = logging;
            _config = config;
            _Login_MobileRepository = login_MobileRepository;
            _servicePartnerManager = servicePartnerManager;
            _servicePartnerRepository = servicePartnerRepository;
            _driverDetailsRepository = driverDetailsRepository;
            _driverDetailsManager = driverDetailsManager;
            _protector = protector;
            _mapLoginUserDeviceRepository = mapLoginUserDeviceRepository;
        }

        #endregion

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
                    TblUser.Password = TblUser.Password?.Trim();
                    TblUser.Email = TblUser.Email?.Trim();
                    TblUser.Phone = TblUser.Phone?.Trim();
                    TblUser.LastName = TblUser.LastName?.Trim();
                    TblUser.FirstName = TblUser.FirstName?.Trim();


                    if (TblUser.UserId > 0)
                    {
                        //Code to update the object
                        TblUser.Phone = SecurityHelper.EncryptString(TblUser.Phone, _config.Value.SecurityKey);
                        TblUser.Email = SecurityHelper.EncryptString(TblUser.Email, _config.Value.SecurityKey);
                        TblUser.Password = SecurityHelper.EncryptString(TblUser.Password, _config.Value.SecurityKey);
                        
                        TblUser.ModifiedBy = userId;
                        TblUser.ModifiedDate = _currentDatetime;
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
                            _userRepository.Create(TblUser);

                            //comment the code as per the disscussion

                            //if (UserVM.UserId == 0)
                            //{
                            //    TblCompany company = _companyRepository.GetSingle(x => x.CompanyId == TblUser.CompanyId);
                            //    TblUser.Password = !string.IsNullOrEmpty(UserVM.UnEncPassword) ? UserVM.UnEncPassword : string.Empty;
                            //    TblUser.Email = SecurityHelper.DecryptString(TblUser.Email, _config.Value.SecurityKey);
                            //    UserNotification(new List<TblUser> { TblUser }, company, "Welcome to Digi2L", EmailTemplateConstant.NewUserAdded_User);

                            //}
                            //TblUser.Email = SecurityHelper.EncryptString(TblUser.Email, _config.Value.SecurityKey);
                            //TblUser.Password = SecurityHelper.EncryptString(UserVM.UnEncPassword, _config.Value.SecurityKey);
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


        /// <summary>
        /// Method to get the user object by login detail
        /// </summary>
        /// <param name="username">username</param>
        /// <param name="password">password</param>
        /// <returns>LoginViewModel</returns>
        public LoginViewModel GetUserByLogin(string username, string password)
        {
            UserViewModel UserVM = null;
            TblUser TblUser = null;
            LoginViewModel loginVM = null;
            try
            {
                TblUser = _userRepository.GetSingle(x => x.IsActive == true && (x.Email != null && x.Email.ToLower().Equals(username.ToLower())) && (x.Password != null && x.Password.Equals(password)));
                if (TblUser != null)
                {

                    UserVM = _mapper.Map<TblUser, UserViewModel>(TblUser);
                    if (UserVM != null)
                    {
                        TblUser.LastLogin = _currentDatetime;

                        _userRepository.Update(TblUser);
                        _userRepository.SaveChanges();

                        loginVM = new LoginViewModel();
                        loginVM.UserViewModel = UserVM;

                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("UserManager", "GetUserByLogin", ex);
            }
            return loginVM;
        }

        public ExecutionResult UserByLogin(string email, string pwd)
        {
            UserViewModel UserVM = null;
            TblUser TblUser = null;
            LoginViewModel loginVM = null;
            try
            {

                TblUser = _userRepository.GetSingle(x => x.IsActive == true && (x.Email != null && x.Email.ToLower().Equals(email.ToLower())) && (x.Password != null && x.Password.Equals(pwd)));
                if (TblUser != null)
                {

                    TblUser.Phone = SecurityHelper.DecryptString(TblUser.Phone, _config.Value.SecurityKey);
                    UserVM = _mapper.Map<TblUser, UserViewModel>(TblUser);
                    if (UserVM != null)
                    {
                        TblUser.LastLogin = _currentDatetime;
                        TblUser.Phone = SecurityHelper.EncryptString(TblUser.Phone, _config.Value.SecurityKey);
                        _userRepository.Update(TblUser);
                        _userRepository.SaveChanges();

                        loginVM = new LoginViewModel();
                        loginVM.UserViewModel = UserVM;
                        loginVM.UserViewModel.Email = SecurityHelper.DecryptString(TblUser.Email, _config.Value.SecurityKey);
                    }

                }

                else
                {
                    return new ExecutionResult<TblUser>(new InfoMessage(true, "Please enter correct email and Password"));
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("UserManager", "UserByLogin", ex);
            }
            return new ExecutionResult(new InfoMessage(true, "Success", loginVM));
        }

        public ExecutionResult RoleByLogin(string email, string pwd, int roleId, int userId)
        {
            UserRoleLoginViewModel UserVM = null;
            TblUser TblUser = null;
            TblUserRole TblUserRole = null;
            IList<TblRoleAccess> TblRoleAccess = null;

            TblAccessList TblAccessList = null;
            TblRole TblRole = null;

            try
            {
                TblUser = _userRepository.GetSingle(x => x.IsActive == true && (x.Email != null && x.Email.ToLower().Equals(email.ToLower())) && (x.Password != null && x.Password.Equals(pwd)));
                if (TblUser != null)
                {
                    TblUser.Phone = SecurityHelper.DecryptString(TblUser.Phone, _config.Value.SecurityKey);
                    TblUserRole = _userRoleRepository.GetSingle(x => x.IsActive == true && (x.RoleId != null && (x.RoleId == roleId)) && (x.UserId != null && x.UserId == userId));
                    TblRole = _roleRepository.GetSingle(x => x.IsActive == true && (x.RoleId != null && (x.RoleId == roleId)));
                    TblRoleAccess = _roleAccessRepository.GetList(x => x.IsActive == true && (x.RoleId != null && (x.RoleId == roleId))).ToList();
                    TblAccessList = _accessListRepository.GetSingle(x => x.IsActive == true);
                    if (TblUserRole != null)
                    {
                        TblUser.LastLogin = _currentDatetime;
                        TblUser.Phone = SecurityHelper.EncryptString(TblUser.Phone, _config.Value.SecurityKey);
                        _userRepository.Update(TblUser);
                        _userRepository.SaveChanges();

                        UserVM = new UserRoleLoginViewModel();
                        UserVM.User = new UserViewModel();
                        UserVM.UserRole = new UserRoleViewModel();


                        UserVM.User.UserId = TblUser.UserId;
                        UserVM.User.ZohoUserId = TblUser.ZohoUserId;
                        UserVM.User.UserStatus = TblUser.UserStatus;
                        UserVM.User.RoleName = TblRole.RoleName;
                        UserVM.User.Phone = SecurityHelper.DecryptString(TblUser.Phone, _config.Value.SecurityKey);
                        UserVM.User.Password = TblUser.Password;
                        UserVM.User.Email = SecurityHelper.DecryptString(TblUser.Email, _config.Value.SecurityKey);
                        UserVM.User.CompanyId = TblUser.CompanyId;

                        UserVM.UserRole.CompanyId = TblUserRole.CompanyId;
                        UserVM.UserRole.RoleId = TblUserRole.RoleId;
                        UserVM.UserRole.Role = TblRole.RoleName;

                        UserVM.UserRole.UserId = TblUserRole.UserId;
                        UserVM.UserRole.UserRoleId = TblUserRole.UserRoleId;
                        UserVM.RoleAccesses = new List<RoleAccessViewModel>();
                        //IList <RoleAccessViewModel objRoleAssess = new List<RoleAccessViewModel>();

                        if (TblRole.RoleId == roleId)
                        {
                            foreach (var items in TblRoleAccess)
                            {
                                RoleAccessViewModel objRoleAssess = new RoleAccessViewModel();
                                objRoleAssess.RoleAccessId = items.RoleAccessId;
                                objRoleAssess.RoleId = items.RoleId;
                                objRoleAssess.AccessListId = items.AccessListId;

                                //objRoleAssess.Add(UserVM.RoleAccesses);
                                UserVM.RoleAccesses.Add(objRoleAssess);


                            }

                        }

                    }
                }
                else
                {
                    return new ExecutionResult<TblUser>(new InfoMessage(true, "Please enter correct email and Password"));
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("UserManager", "GetUserByLogin", ex);
            }
            return new ExecutionResult(new InfoMessage(true, "Success", UserVM));
        }

        public ExecutionResult UpdateUserPassword(string encpwd1, string encpwd, string pwd)
        {
            TblUser TblUser = new TblUser();
            try
            {
                if (!string.IsNullOrEmpty(encpwd1))
                {
                    TblUser = _userRepository.GetSingle(x => x.Email == encpwd1);
                    if (TblUser != null)
                    {

                        TblUser.Password = encpwd;

                        //Code to update the object
                        TblUser.ModifiedBy = TblUser.UserId;
                        TblUser.ModifiedDate = _currentDatetime;
                        _userRepository.Update(TblUser);
                        _userRepository.SaveChanges();
                        string encpwd3 = SecurityHelper.DecryptString(TblUser.Email, _config.Value.SecurityKey);
                        TblUser.Email = encpwd3;
                        TblUser.Password = pwd;


                        TblCompany company = _companyRepository.GetSingle(x => x.CompanyId == TblUser.CompanyId);
                        UserNotification(new List<TblUser> { TblUser }, company, "Password Reset", EmailTemplateConstant.ForgotPassword_User);
                    }
                    else
                    {
                        return new ExecutionResult(new InfoMessage(true, "Please enter correct email"));
                    }
                }
                else
                {
                    return new ExecutionResult(new InfoMessage(true, "Please enter correct email"));
                }
            }
            catch (Exception ex)
            {
                _errorLogManager.WriteErrorToLog("UserManager", "ManageUser", ex);
            }

            return new ExecutionResult(new InfoMessage(true, "Password reset link has been sent successfully", pwd));
        }

        /// <summary>
        /// MEthod to send the notification for User
        /// </summary>
        /// <param name="userObj">userObj</param>
        /// <param name="company">company</param>
        /// <param name="subject">subject</param>
        /// <param name="tempateName">tempateName</param>
        /// <returns>void</returns>
        public void UserNotification(List<TblUser> userObjList, TblCompany company, string subject, string tempateName)
        {
            //bool flag = false;
            string toEmails = string.Empty;
            string content = string.Empty;
            try
            {
                if (userObjList != null && userObjList.Count > 0)
                {
                    foreach (TblUser userObj in userObjList)
                    {
                        var file = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "HTMLTemplate", tempateName);
                        content = File.ReadAllText(file);
                        content = content.Replace("[ToName]", userObj.FirstName);
                        content = content.Replace("[Company]", company != null && !string.IsNullOrEmpty(company.CompanyName) ? company.CompanyName : "Digi2L");
                        content = content.Replace("[Email]", userObj.Email);
                        content = content.Replace("[Password]", userObj.Password);
                        content = content.Replace("[BaseURL]", _config.Value.BaseURL);
                        content = content.Replace("[SupportEmail]", "Digi2L");
                        toEmails = string.IsNullOrEmpty(toEmails) ? userObj.Email : toEmails + ";" + userObj.Email;
                    }
                    _mailManager.SendEmailAsync(toEmails, content, subject);
                }
            }
            catch (Exception ex)
            {
                _errorLogManager.WriteErrorToLogAsync("DPIAMasterManager", "UserNotification", ex);
            }
        }

        public void UserNotifications(List<TblUser> userObjList, TblCompany company, string subject, string tempateName)
        {
            //bool flag = false;
            string toEmails = string.Empty;
            string content = string.Empty;
            try
            {
                if (userObjList != null && userObjList.Count > 0)
                {
                    foreach (TblUser userObj in userObjList)
                    {
                        var file = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "HTMLTemplate", tempateName);
                        content = File.ReadAllText(file);
                        content = content.Replace("[ToName]", userObj.FirstName);
                        content = content.Replace("[Company]", company != null && !string.IsNullOrEmpty(company.CompanyName) ? company.CompanyName : "Digi2L");
                        content = content.Replace("[Email]", userObj.Email);
                        content = content.Replace("[Password]", userObj.Password);
                        content = content.Replace("[BaseURL]", _config.Value.BaseURL);
                        content = content.Replace("[SupportEmail]", "Digi2L");
                        toEmails = string.IsNullOrEmpty(toEmails) ? userObj.Email : toEmails + ";" + userObj.Email;
                        toEmails = SecurityHelper.DecryptString(userObj.Email, _config.Value.SecurityKey);
                    }
                    _mailManager.SendEmailAsync(toEmails, content, subject);
                }
            }
            catch (Exception ex)
            {

                _logging.WriteErrorToDB("UserManager", "UserNotifications", ex);
            }
        }

        public bool UserNotificationEVC(UserViewModel userObj, string subject, string tempateName)
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


                    sendMail = _mailManager.SendEmailforevcAsync(toEmails, content, subject).IsCompletedSuccessfully.ToString();
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
        /// Method to change password
        /// </summary>
        /// <param name="userid">userid</param>
        /// <param name="oldPassword">oldPassword</param>
        /// <param name="password">password</param>
        /// <returns>int</returns>
        public ExecutionResult ChangePassword(int userid, string oldPassword, string NewPassword, string ConfirmPassword)
        {

            TblUser TblUser = new TblUser();
            string pwd = string.Empty;
            try
            {

                if (userid > 0 && !string.IsNullOrEmpty(oldPassword) && !string.IsNullOrEmpty(NewPassword))
                {
                    TblUser = _userRepository.GetSingle(x => x.UserId == userid);
                    if (TblUser != null)
                    {
                        string encpwd = SecurityHelper.DecryptString(TblUser.Password, _config.Value.SecurityKey);

                        if (encpwd == oldPassword)
                        {
                            if (encpwd != NewPassword)
                            {
                                TblUser.Password = SecurityHelper.EncryptString(NewPassword, _config.Value.SecurityKey);
                                //Code to update the object
                                TblUser.ModifiedBy = TblUser.UserId;
                                TblUser.ModifiedDate = _currentDatetime;
                                _userRepository.Update(TblUser);
                                _userRepository.SaveChanges();
                                return new ExecutionResult(new InfoMessage(true, "Password updated successfully"));

                            }
                            else
                            {
                                return new ExecutionResult(new InfoMessage(true, "New and old password should be different"));
                            }

                        }
                        else
                        {
                            return new ExecutionResult(new InfoMessage(true, "Please enter correct old password"));
                        }

                    }
                    else
                    {
                        return new ExecutionResult(new InfoMessage(true, "Something went wrong"));
                    }
                }
                else
                {
                    return new ExecutionResult(new InfoMessage(true, "Something went wrong"));
                }
            }
            catch (Exception ex)
            {
                _errorLogManager.WriteErrorToLog("UserManager", "UpdateUserPassword", ex);
            }

            return new ExecutionResult(new InfoMessage(true, "Something went wrong"));
        }

        public int ForgotPassword(string email, string encpwd, string pwd)
        {
            TblUser TblUser = new TblUser();
            try
            {

                if (!string.IsNullOrEmpty(email))
                {
                    TblUser = _userRepository.GetSingle(x => x.Email == email);
                    if (TblUser != null)
                    {

                        TblUser.Password = encpwd;
                        //Code to update the object
                        TblUser.ModifiedBy = TblUser.UserId;
                        TblUser.ModifiedDate = _currentDatetime;
                        _userRepository.Update(TblUser);
                        _userRepository.SaveChanges();
                        TblUser.Password = pwd;
                        TblCompany company = _companyRepository.GetSingle(x => x.CompanyId == TblUser.CompanyId);
                        UserNotifications(new List<TblUser> { TblUser }, company, "Password Reset", EmailTemplateConstant.ForgotPassword_User);
                    }
                    else
                    {
                        return -1;
                    }
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception ex)
            {
                _errorLogManager.WriteErrorToLog("UserManager", "ManageUser", ex);
            }

            return TblUser.UserId;
        }

        public int NotificationAllUsers(UserViewModel UserVM, string subject, string tempateName)
        {
           
            //string sendMail = null;
            string toEmails = string.Empty;
            string content = string.Empty;
            TblUser TblUser = new TblUser();
            foreach (var item in UserVM.UserIdList)
            {
                TblUser = _userRepository.GetSingle(x => x.UserId == item);
                if(TblUser.Email != null)
                {
                    var file = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "HTMLTemplate", tempateName);
                    content = File.ReadAllText(file);
                    content = content.Replace("[ToName]", TblUser.FirstName);
                    content = content.Replace("[Company]", "Digi2L");
                    TblUser.Email = SecurityHelper.DecryptString(TblUser.Email, _config.Value.SecurityKey);
                    TblUser.Password = SecurityHelper.DecryptString(TblUser.Password, _config.Value.SecurityKey);
                    content = content.Replace("[Email]", TblUser.Email);
                    //item.Password = SecurityHelper.DecryptString(item.Password, _config.Value.SecurityKey);
                    content = content.Replace("[Password]", TblUser.Password);
                    content = content.Replace("[BaseURL]", _config.Value.BaseURL);
                    content = content.Replace("[SupportEmail]", "Digi2L");
                    toEmails = string.IsNullOrEmpty(toEmails) ? TblUser.Email : toEmails + ";" + TblUser.Email;
                    //toEmails = SecurityHelper.DecryptString(TblUser.Email, _config.Value.SecurityKey);
                    _mailManager.SendEmailAsync(toEmails, content, subject);
                    
                }
                else
                {
                       return -1;
                    
                }
               
               
            }


            return TblUser.UserId;

        }



        //public int Mailsend(string email)
        //{
        //    IList<TblUser> tblUserlist = null;
        //    try
        //    {
        //        tblUserlist = _userRepository.GetList(x => x.IsActive == true ).ToList(); 

        //        TblCompany company = _companyRepository.GetSingle(x => x.CompanyId == TblUser.CompanyId);
        //        UserNotifications(new List<TblUser> { TblUser }, company, "Password Reset", EmailTemplateConstant.ForgotPassword_User);

        //    }
        //    catch (Exception ex)
        //    {
        //        _errorLogManager.WriteErrorToLog("UserManager", "ManageUser", ex);
        //    }

        //    return TblUser.UserId;
        //}

        public UserViewModel GetUserByEmailandPasswordLogin(string username, string password)
        {
            UserViewModel UserVM = null;
            TblUser TblUser = null;
            try
            {
                TblUser = _userRepository.GetSingle(x => x.IsActive == true && (x.Email != null && x.Email.ToLower().Equals(username.ToLower())) && (x.Password != null && x.Password.Equals(password)));
                if (TblUser != null)
                {
                    UserVM = _mapper.Map<TblUser, UserViewModel>(TblUser);
                }
            }
            catch (Exception ex)
            {
                _errorLogManager.WriteErrorToLog("UserManager", "GetUserByEmailandPasswordLogin", ex);
            }
            return UserVM;
        }

        public int UpdateUserChangePassword(int userid,int? companyId, string oldPassword, string password)
        {
            TblUser TblUser = new TblUser();
            string pwd = string.Empty;
            try
            {

                if (userid > 0 && !string.IsNullOrEmpty(oldPassword) && !string.IsNullOrEmpty(password))
                {
                    TblUser = _userRepository.GetSingle(x => x.UserId == userid);
                    if (TblUser != null)
                    {
                        //Code to update the object
                        TblUser.Password = password;
                        TblUser.CompanyId = companyId;
                       
                        TblUser.ModifiedBy = TblUser.UserId;
                        TblUser.ModifiedDate = _currentDatetime;
                        _userRepository.Update(TblUser);
                        _userRepository.SaveChanges();
                    }
                    else
                    {
                        return -1;
                    }
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception ex)
            {
                _errorLogManager.WriteErrorToLog("UserManager", "UpdateUserPassword", ex);
            }

            return TblUser.UserId;
        }



        /// <summary>
        /// Method to get the list of all user by role id
        /// </summary>
        /// <returns>List of UserViewModel</returns>
        public IList<UserViewModel> GetAllUsersByRole(int roleId)
        {
            IList<UserViewModel> userVMList = null;
            IList<TblUser> tblUserlist = null;

            try
            {
                tblUserlist = _userRepository.GetList(x => x.IsActive == true).ToList();
                if (tblUserlist != null && tblUserlist.Count > 0)
                {
                    userVMList = _mapper.Map<IList<TblUser>, IList<UserViewModel>>(tblUserlist);
                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ComplianceRiskManager", "GetAllComplianceRisk", ex);
            }
            return userVMList;
        }

        /// <summary>
        /// Method to get the User by id 
        /// </summary>
        /// <param name="id">UserId</param>
        /// <returns>UserViewModel</returns>
        public UserViewModel GetUserById(int id)
        {
            UserViewModel UserVM = null;
            TblUser TblUser = null;

            try
            {

                TblUser = _userRepository.GetSingle(x => x.IsActive == true && x.UserId == id);

                if (TblUser != null)
                {
                    //TblUser.Email = SecurityHelper.DecryptString(TblUser.Email, _config.Value.SecurityKey);
                    UserVM = _mapper.Map<TblUser, UserViewModel>(TblUser);

                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("UserManager", "GetUserById", ex);
            }
            return UserVM;
        }


        public UserViewModel UserById(int id)
        {
            UserViewModel UserVM = null;
            TblUser TblUser = null;

            try
            {
                TblUser = _userRepository.GetSingle(x => x.IsActive == true && x.UserId == id);

                if (TblUser != null)
                {
                    TblUser.Email = SecurityHelper.DecryptString(TblUser.Email, _config.Value.SecurityKey);
                    TblUser.Password = SecurityHelper.DecryptString(TblUser.Password, _config.Value.SecurityKey);
                    TblUser.Phone = SecurityHelper.DecryptString(TblUser.Phone, _config.Value.SecurityKey);
                    UserVM = _mapper.Map<TblUser, UserViewModel>(TblUser);


                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("UserManager", "GetUserById", ex);
            }
            return UserVM;
        }
        /// <summary>
        /// Method to delete User by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        public bool DeletUserById(int id)
        {
            bool flag = false;
            try
            {
                TblUser TblUser = _userRepository.GetSingle(x => x.IsActive == true && x.UserId == id);
                if (TblUser != null)
                {
                    TblUser.IsActive = false;
                    _userRepository.Update(TblUser);
                    _userRepository.SaveChanges();
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("UserManager", "DeletUserById", ex);
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
        /// Method to get List of Sales user list
        /// </summary>
        /// <returns>UserViewModel</returns>
        public IList<UserViewModel> GetSalesSUserList(int? companyId)
        {
            IList<UserViewModel> UserVMList = null;
            IList<TblUser> TblUserList = null;
            IList<TblUserRole> TblUserRoleList = null;
            TblRole TblRoleList = null;
            try
            {
                if (companyId > 0)
                {
                    //string roleName = "Sales Executive";
                    TblRoleList = _roleRepository.GetSingle(x => x.IsActive == true && x.CompanyId == companyId);
                    if (TblRoleList != null)
                    {
                        TblUserRoleList = _userRoleRepository.GetList(x => x.IsActive == true && x.RoleId == TblRoleList.RoleId && x.CompanyId == companyId).ToList();

                    }
                }

                if (TblUserRoleList != null && TblUserRoleList.Count > 0)
                {
                    TblUserList = _userRepository.GetList(x => x.IsActive == true).ToList();

                    TblUserList = (from al in TblUserRoleList
                                   join ral in TblUserList on al.UserId equals ral.UserId
                                   select new TblUser
                                   {
                                       UserId = ral.UserId,
                                       FirstName = ral.FirstName,
                                       LastName = ral.LastName,

                                   }).ToList();
                }
                if (TblUserList != null && TblUserList.Count > 0)
                {
                    UserVMList = _mapper.Map<IList<TblUser>, IList<UserViewModel>>(TblUserList);
                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("UserManager", "GetSalesSUserList", ex);
            }
            return UserVMList;
        }

        /// <summary>
        /// Method to get the User Role by id 
        /// </summary>
        /// <param name="id">UserRoleId</param>
        /// <returns>UserRoleViewModel</returns>
        public UserRoleViewModel GetUserRoleById(int id)
        {
            UserRoleViewModel UserRoleVM = null;
            TblUserRole TblUserRole = null;

            try
            {
                TblUserRole = _userRoleRepository.GetSingle(x => x.IsActive == true && x.UserRoleId == id);
                if (TblUserRole != null)
                {
                    UserRoleVM = _mapper.Map<TblUserRole, UserRoleViewModel>(TblUserRole);
                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("UserManager", "GetUserRoleById", ex);
            }
            return UserRoleVM;
        }

        /// <summary>
        /// Method to get List of users list with Role
        /// </summary>
        /// <returns>UserViewModel</returns>
        public IList<UserViewModel> GetUsersListWithRole(int? companyId)
        {
            IList<UserViewModel> UserVMList = null;
            IList<TblUser> TblUserList = null;
            //IList<TblUserRole> TblUserRoleList = null;
            //TblRole TblRoleList = null;
            try
            {
                if (companyId > 0)
                {
                    TblUserList = _userRepository.GetList(x => x.IsActive == true && x.CompanyId == companyId).OrderByDescending(x => x.CreatedDate).ToList();
                    if (TblUserList != null && TblUserList.Count > 0)
                    {
                        UserVMList = _mapper.Map<IList<TblUser>, IList<UserViewModel>>(TblUserList);
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("UserManager", "GetSalesSUserList", ex);
            }
            return UserVMList;
        }

        public IList<UserViewModel> GetAllUser()
        {

            IList<UserViewModel> UserVMList = null;
            List<TblUser> TblUserlist = new List<TblUser>();
            // TblUseRole TblUseRole = null;
            try
            {

                TblUserlist = _userRepository.GetList(x => x.IsActive == true).ToList();

                if (TblUserlist != null && TblUserlist.Count > 0)
                {
                    UserVMList = _mapper.Map<IList<TblUser>, IList<UserViewModel>>(TblUserlist);
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("UserManager", "GetAllUser", ex);
            }
            return UserVMList;
        }

        /// <summary>
        /// Method to get the User by id in decrypt form
        /// </summary>
        /// <param name="id">UserId</param>F
        /// <returns>UserViewModel</returns>
        public UserViewModel GetUserByIdInDecrypt(int id)
        {
            UserViewModel UserVM = null;
            TblUser TblUser = null;

            try
            {
                TblUser = _userRepository.GetSingle(x => x.IsActive == true && x.UserId == id);
                if (TblUser != null)
                {
                    TblUser.Email = SecurityHelper.DecryptString(TblUser.Email, _config.Value.SecurityKey);
                    UserVM = _mapper.Map<TblUser, UserViewModel>(TblUser);
                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("UserManager", "GetUserById", ex);
            }
            return UserVM;
        }

        public int ManageEvcUser(TblEvcregistration tblEvcReg, int loginUserId, int? loginUserCompId)
        {
            int userId = 0;
            int userRoleId = 0;
            UserViewModel userVM = new UserViewModel();
            TblCompany tblCompany = null;
            TblRole tblRole = null;
            UserRoleViewModel userRoleVM = new UserRoleViewModel();
            try
            {
                tblCompany = _companyRepository.GetSingle(x => x.IsActive == true && x.CompanyName == "UTC Digital");
                tblRole = _roleRepository.GetSingle(x => x.IsActive == true && x.RoleName == "EVC Portal");

                string fullName = tblEvcReg.ContactPerson;
                userVM.SplitFullName(fullName);
                userVM.Email = tblEvcReg.EmailId;
                /*userVM.Password = StringHelper.RandomStrByLength(6);*/
                userVM.Password = _config.Value.EVCLoginPossword;
                userVM.Phone = tblEvcReg.EvcmobileNumber;
                userVM.ImageName = tblEvcReg.ProfilePic;
                userVM.UserStatus = "Active";
                userVM.CompanyId = tblCompany.CompanyId;
                userVM.FirstName = tblEvcReg.ContactPerson;
                userVM.LastName = tblEvcReg.ContactPerson;
                userId = ManageUser(userVM, loginUserId, loginUserCompId);
                if (userId > 0)
                {
                    userRoleVM.RoleId = tblRole.RoleId;
                    userRoleVM.UserId = userId;
                    userRoleVM.CompanyId = tblCompany.CompanyId;
                    userRoleId = ManageUserRole(userRoleVM, loginUserId, loginUserCompId);
                    if (userRoleId > 0 && _config.Value.SendEmailFlag)
                    {
                        bool sendNotif = UserNotificationEVC(userVM, "EVC Registration", EmailTemplateConstant.NewEVCUser);
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("UserManager", "ManageEvcUser", ex);
            }
            return userId;
        }

        //Created By Priyanshi--
        public int ManageServicePartnerUser(TblServicePartner tblServicePartner, int loginUserId, int? loginUserCompId)
        {
            int userId = 0;
            int userRoleId = 0;
            UserViewModel userVM = new UserViewModel();
            TblCompany? tblCompany = null;
            TblRole? tblRole = null;
            UserRoleViewModel userRoleVM = new UserRoleViewModel();
            try
            {
                tblCompany = _companyRepository.GetSingle(x => x.IsActive == true && x.CompanyName == "UTC Digital");
                tblRole = _roleRepository.GetSingle(x => x.IsActive == true && x.RoleName == "Service Partner");
                string fullName = tblServicePartner.ServicePartnerName;
                userVM.SplitFullName(fullName);
                userVM.Email = tblServicePartner.ServicePartnerEmailId;
                /*userVM.Password = StringHelper.RandomStrByLength(6);*/
                userVM.Password = _config.Value.LGCLoginPossword;
                userVM.Phone = tblServicePartner.ServicePartnerMobileNumber;
                userVM.ImageName = tblServicePartner.ServicePartnerProfilePic;
                userVM.UserStatus = "Active";
                userVM.CompanyId = tblCompany.CompanyId;
                userVM.FirstName = tblServicePartner.ServicePartnerFirstName;
                userVM.LastName = tblServicePartner.ServicePartnerLastName;

                userId = ManageUser(userVM, loginUserId, loginUserCompId);
                if (userId > 0)
                {
                    userRoleVM.RoleId = tblRole.RoleId;
                    userRoleVM.UserId = userId;
                    userRoleVM.CompanyId = tblCompany.CompanyId;
                    userRoleId = ManageUserRole(userRoleVM, loginUserId, loginUserCompId);

                    #region update Mobile Login Table 
                    //not in use
                    //TblLoginMobile tblLoginMobile = new TblLoginMobile();
                    //tblLoginMobile.Username = SecurityHelper.EncryptString(userVM.Email, _config.Value.SecurityKey);
                    //tblLoginMobile.Password = SecurityHelper.EncryptString(userVM.Password, _config.Value.SecurityKey);
                    //tblLoginMobile.UserRoleName = tblRole.RoleName;
                    //tblLoginMobile.UserId = userId;
                    //_Login_MobileRepository.Create(tblLoginMobile);
                    //_Login_MobileRepository.SaveChanges();

                    #endregion
                    if (userRoleId > 0 && _config.Value.SendEmailFlag)
                    {
                        bool sendNotif = UserNotificationLGC(userVM, "ServicePartner Registration", EmailTemplateConstant.NewLGCUser);
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("UserManager", "ManageServicePartnerUser", ex);
            }
            return userId;
        }
        public bool UserNotificationLGC(UserViewModel userObj, string subject, string tempateName)
        {
            bool flag = false;
            string? sendMail = null;
            string toEmails = string.Empty;
            string content = string.Empty;

            try
            {
                if (userObj != null)
                {
                    toEmails = string.IsNullOrEmpty(userObj.Email) ? toEmails : userObj.Email;

                    var file = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "HTMLTemplate", tempateName);
                    content = File.ReadAllText(file);
                    content = content.Replace("[VendorName]", userObj.FirstName);
                    //content = content.Replace("[VendorID]", "Digi2L");
                    content = content.Replace("[VendorID]", toEmails);
                    content = content.Replace("[Password]", userObj.Password);
                    //  content = content.Replace("[BaseURL]", _config.Value.BaseURL);

                    content = content.Replace("[SupportEmail]", _config.Value.SupportEmail);


                    sendMail = _mailManager.SendEmailforLGCAsync(toEmails, content, subject).IsCompletedSuccessfully.ToString();
                    if (sendMail != null)
                    {
                        flag = true;
                    }

                }
            }
            catch (Exception ex)
            {
                _errorLogManager.WriteErrorToLogAsync("UserManager", "UserNotificationLGC", ex);
            }
            return flag;
        }


        /// <summary>
        /// Get UserDetails from tblUser by login Api -Added by Ashwin
        /// used in Login Controller
        /// </summary>
        /// <param name="email"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public ResultResponse LoginUserDetails(userDataViewModal userDataViewModal)
        {
            ResultResponse responseResult = new ResultResponse();
            ResponseResult result = new ResponseResult();
            LoginUserDetailsDataViewModal loginUserDetailsDataViewModal = new LoginUserDetailsDataViewModal();
            TblUser TblUser = null;
            UserDetailsDataModel UserVM = null;
            LGCUserViewDataModel ServicePartnerdDetails = null;
            try
            {
                #region Fill tbluserdetails
                if (!string.IsNullOrEmpty(userDataViewModal.Email) && !string.IsNullOrEmpty(userDataViewModal.Password))
                {
                    TblUser = _userRepository.GetSingle(x => x.IsActive == true && (x.Email != null && x.Email.ToLower().Equals(userDataViewModal.Email.ToLower())) && (x.Password != null && x.Password.Equals(userDataViewModal.Password)));
                }
                else if (!string.IsNullOrEmpty(userDataViewModal.Phone) && string.IsNullOrEmpty(userDataViewModal.Email))
                {
                    TblUser = _userRepository.GetSingle(x => x.IsActive == true && x.Phone == userDataViewModal.Phone);
                }
                else
                {
                    responseResult.message = "UserData NotFound";
                    responseResult.Status = false;
                    responseResult.Status_Code = HttpStatusCode.BadRequest;
                    return responseResult;
                }
                if (TblUser != null)
                {
                    TblUser.Phone = SecurityHelper.DecryptString(TblUser.Phone, _config.Value.SecurityKey);
                    UserVM = _mapper.Map<TblUser, UserDetailsDataModel>(TblUser);
                    if (UserVM != null)
                    {
                        if (!string.IsNullOrEmpty(UserVM.Email))
                        {
                            UserVM.Email = SecurityHelper.DecryptString(TblUser.Email, _config.Value.SecurityKey);
                        }
                        loginUserDetailsDataViewModal.userDetails = UserVM;

                        TblUser.LastLogin = _currentDatetime;
                        TblUser.Phone = SecurityHelper.EncryptString(TblUser.Phone, _config.Value.SecurityKey);
                        _userRepository.Update(TblUser);
                        _userRepository.SaveChanges();
                    }
                    else
                    {
                        responseResult.message = "Error Occurs while mapping tbluser details";
                        responseResult.Status = false;
                        responseResult.Status_Code = HttpStatusCode.BadRequest;
                        return responseResult;
                    }
                }
                else
                {
                    responseResult.message = "UserData NotFound";
                    responseResult.Status = false;
                    responseResult.Status_Code = HttpStatusCode.BadRequest;
                    return responseResult;
                }
                #endregion

                #region Fill Registeration details
                if (UserVM != null && UserVM.UserId > 0)
                {
                    List<TblUserRole> tblUserRole = _userRoleRepository.GetList(x => x.IsActive == true && x.UserId == UserVM.UserId).ToList();
                    foreach (var roleId in tblUserRole)
                    {
                        TblRole tblRole = _roleRepository.GetSingle(x => x.IsActive == true && x.RoleId == roleId.RoleId);

                        if (tblRole.RoleName == EnumHelper.DescriptionAttr(ApiUserRoleEnum.LGC_Admin).ToString()|| tblRole.RoleName == EnumHelper.DescriptionAttr(ApiUserRoleEnum.Service_Partner).ToString() || tblRole.RoleName == EnumHelper.DescriptionAttr(RoleEnum.EVCLGC))
                        {
                            TblServicePartner tblServicePartner = _servicePartnerRepository.GetSingle(x => x.IsActive == true && x.ServicePartnerEmailId == UserVM.Email);
                            result = _servicePartnerManager.ServicePartnerDetails(tblServicePartner.ServicePartnerId);

                            if (result.Status == true)
                            {
                                ServicePartnerdDetails = new LGCUserViewDataModel();
                                ServicePartnerdDetails = (LGCUserViewDataModel)result.Data;
                                loginUserDetailsDataViewModal.servicePatnerDetails = ServicePartnerdDetails;
                            }
                        }
                        if (tblRole.RoleName == EnumHelper.DescriptionAttr(ApiUserRoleEnum.Service_Partner_Driver).ToString())
                        {
                            //TblDriverDetail tblDriverDetail = _driverDetailsRepository.GetSingle(x => x.IsActive == true && x.UserId== UserVM.UserId);
                            DriverResponseViewModal driverDetails = new DriverResponseViewModal();
                            driverDetails = _driverDetailsManager.GetDriverDetailsById(UserVM.UserId);
                            if (driverDetails != null)
                            {
                                loginUserDetailsDataViewModal.driverDetails = driverDetails;
                            }
                        }
                    }
                    if (loginUserDetailsDataViewModal != null && loginUserDetailsDataViewModal.servicePatnerDetails != null || loginUserDetailsDataViewModal.driverDetails != null)
                    {
                        responseResult.message = "Success";
                        responseResult.Status = true;
                        responseResult.Status_Code = HttpStatusCode.OK;
                        responseResult.Data = loginUserDetailsDataViewModal;
                        return responseResult;
                    }
                    else
                    {
                        responseResult.message = "No Valid Role Assign to User";
                        responseResult.Status = false;
                        responseResult.Status_Code = HttpStatusCode.BadRequest;
                        return responseResult;
                    }
                }
                else
                {
                    responseResult.message = "UserData NotFound";
                    responseResult.Status = false;
                    responseResult.Status_Code = HttpStatusCode.BadRequest;
                    return responseResult;
                }
                #endregion
            }
            catch (Exception ex)
            {
                responseResult.message = ex.Message;
                responseResult.Status = false;
                responseResult.Status_Code = HttpStatusCode.InternalServerError;
                _logging.WriteErrorToDB("UserManager", "LoginUserDetails", ex);
            }
            return responseResult;
        }

        /// <summary>
        /// Update Password - Used in Core webapi
        /// Added by ashwin
        /// </summary>
        /// <param name="encpwd1"></param>
        /// <param name="encpwd"></param>
        /// <param name="pwd"></param>
        /// <returns>responseResult</returns>
        public ResponseResult UpdatePassword(string encpwd1, string encpwd, string pwd)
        {
            ResponseResult responseResult = new ResponseResult();
            TblUser TblUser = new TblUser();
            TblLoginMobile tblLoginMobile = new TblLoginMobile();
            try
            {
                if (!string.IsNullOrEmpty(encpwd1))
                {
                    TblUser = _userRepository.GetSingle(x => x.Email == encpwd1);

                    string decrep = SecurityHelper.DecryptString(encpwd1, _config.Value.SecurityKey);
                    tblLoginMobile = _Login_MobileRepository.GetSingle(x => x.Username == decrep);
                    if (TblUser != null && tblLoginMobile != null)
                    {

                        TblUser.Password = encpwd;

                        //Code to update the object
                        TblUser.ModifiedBy = TblUser.UserId;
                        TblUser.ModifiedDate = _currentDatetime;
                        _userRepository.Update(TblUser);


                        string LoginMobilepass = SecurityHelper.DecryptString(encpwd, _config.Value.SecurityKey);
                        var obj = "Your Updated Password is  : { '" + LoginMobilepass + "' }";
                        tblLoginMobile.Password = LoginMobilepass;
                        _Login_MobileRepository.Update(tblLoginMobile);
                        if (TblUser.ModifiedDate != null && tblLoginMobile.Password != string.Empty)
                        {
                            _userRepository.SaveChanges();
                            _Login_MobileRepository.SaveChanges();
                            responseResult.Status = true;
                            responseResult.Status_Code = HttpStatusCode.OK;
                            responseResult.message = "Password Updated Successfully...";
                            responseResult.Data = obj;
                        }
                        else
                        {
                            responseResult.Status = false;
                            responseResult.Status_Code = HttpStatusCode.BadRequest;
                            responseResult.message = "Password Not updated";
                        }

                        string encpwd3 = SecurityHelper.DecryptString(TblUser.Email, _config.Value.SecurityKey);
                        TblUser.Email = encpwd3;
                        TblUser.Password = pwd;

                        #region send updated password notification to user on email 
                        TblCompany company = _companyRepository.GetSingle(x => x.CompanyId == TblUser.CompanyId);
                        if (company != null)
                        {
                            UserNotification(new List<TblUser> { TblUser }, company, "Password Reset", EmailTemplateConstant.ForgotPassword_User);
                            responseResult.message = "Password updated successfully...password reset link has been sent to user";
                        }
                        else
                        {
                            responseResult.message = "Password Updated Successfully without sending email to user";
                        }
                        #endregion
                    }
                    else
                    {
                        //return new ExecutionResult(new InfoMessage(true, "Please enter correct email"));
                        responseResult.Status = false;
                        responseResult.Status_Code = HttpStatusCode.BadRequest;
                        responseResult.message = "Please enter correct email";
                        return responseResult;
                    }
                }
                else
                {
                    //return new ExecutionResult(new InfoMessage(true, "Please enter correct email"));
                    responseResult.Status = false;
                    responseResult.Status_Code = HttpStatusCode.BadRequest;
                    responseResult.message = "Please enter correct email";
                    return responseResult;
                }
            }
            catch (Exception ex)
            {
                _errorLogManager.WriteErrorToLog("UserManager", "ManageUser", ex);
                responseResult.Status = false;
                responseResult.Status_Code = HttpStatusCode.InternalServerError;
                responseResult.message = ex.Message;
            }

            //return new ExecutionResult(new InfoMessage(true, "Password reset link has been sent successfully", pwd));
            return responseResult;
        }

        #region Method to Manage Data in tbl Role, UserRole, User and send Mail to respective User email
        public int ManageUserAndUserRole(UserViewModel userVM, int loginUserId)
        {
            int userId = 0;
            int userRoleId = 0;
            UserViewModel userVM1 = new UserViewModel();
            TblCompany tblCompany = null;
            TblRole tblRole = null;
            UserRoleViewModel userRoleVM = new UserRoleViewModel();
            try
            {
                if (userVM != null)
                {
                    tblCompany = _companyRepository.GetSingle(x => x.IsActive == true && x.BusinessUnitId == userVM.BusinessUnitId);
                    if (tblCompany != null)
                    {
                        tblRole = _roleRepository.GetSingle(x => x.IsActive == true && x.RoleName == userVM.RoleName && x.CompanyId == tblCompany.CompanyId);
                        if (tblRole == null)
                        {
                            tblRole = new TblRole();
                            tblRole.RoleName = userVM.RoleName;
                            tblRole.CompanyId = tblCompany.CompanyId;
                            tblRole.CreatedBy = loginUserId;
                            tblRole.CreatedDate = _currentDatetime;
                            tblRole.IsActive = true;
                            _roleRepository.Create(tblRole);
                            _roleRepository.SaveChanges();
                        }
                        string fullName = userVM.Name;
                        if (userVM.Name != null)
                        {
                            userVM.SplitFullName(fullName);
                        }
                        if (userVM.Password == null)
                        {
                            userVM.Password = "Digi2L@123";
                        }
                        userVM.UserStatus = "Active";
                        userVM.CompanyId = tblCompany.CompanyId;
                        userId = ManageUser(userVM, loginUserId, tblCompany.CompanyId);
                        if (userId > 0)
                        {
                            userRoleVM.RoleId = tblRole.RoleId;
                            userRoleVM.UserId = userId;
                            userRoleVM.CompanyId = tblCompany.CompanyId;
                            userRoleId = ManageUserRole(userRoleVM, loginUserId, tblCompany.CompanyId);
                            if (userRoleId > 0 && _config.Value.SendEmailFlag && userVM.MailTemplate != null && userVM.MailSubject != null)
                            {
                                bool sendNotif = UserNotificationEVC(userVM, userVM.MailSubject, userVM.MailTemplate);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("UserManager", "ManageUserAndUserRole", ex);
            }
            return userId;
        }
        #endregion

       

        #region Get User Details by mobile number
        /// <summary>
        /// Get UserDetails by Mobile Number
        /// </summary>
        /// <param name="mobNumber"></param>
        /// <returns></returns>
        public userDataViewModal GetUserByNumber(string mobNumber)
        {
            userDataViewModal UserVM = null;
            //  UserDetailsDataModel UserVM = null;
            try
            {
                if (mobNumber != null && mobNumber.Length > 0)
                {
                    TblUser tbluser = _userRepository.GetSingle(x => x.IsActive == true && x.Phone == mobNumber);

                    if (tbluser != null)
                    {
                        UserVM = _mapper.Map<TblUser, userDataViewModal>(tbluser);
                        if (UserVM != null)
                        {
                            return UserVM;
                        }
                        else
                        {
                            UserVM = null;
                            return UserVM;
                        }
                    }
                    else
                    {
                        UserVM = null;
                        return UserVM;
                    }
                }
                else
                {
                    UserVM = null;
                    return UserVM;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("UserManager", "GetUserByNumber", ex);
            }
            return UserVM;
        }
        #endregion

        #region getuserdetailsbyMobileNumber
        //under construction
        public ResultResponse GetLoginDetailsByMobileNumber(string email, string pwd)
        {
            ResultResponse responseResult = new ResultResponse();
            ResponseResult result = new ResponseResult();
            LoginUserDetailsDataViewModal loginUserDetailsDataViewModal = new LoginUserDetailsDataViewModal();
            TblUser TblUser = null;
            UserDetailsDataModel UserVM = null;
            LGCUserViewDataModel ServicePartnerdDetails = null;
            try
            {
                #region Fill tbluserdetails
                TblUser = _userRepository.GetSingle(x => x.IsActive == true && (x.Email != null && x.Email.ToLower().Equals(email.ToLower())) && (x.Password != null && x.Password.Equals(pwd)));
                if (TblUser != null)
                {
                    TblUser.Phone = SecurityHelper.DecryptString(TblUser.Phone, _config.Value.SecurityKey);
                    UserVM = _mapper.Map<TblUser, UserDetailsDataModel>(TblUser);
                    if (UserVM != null)
                    {
                        UserVM.Email = SecurityHelper.DecryptString(TblUser.Email, _config.Value.SecurityKey);
                        loginUserDetailsDataViewModal.userDetails = UserVM;

                        TblUser.LastLogin = _currentDatetime;
                        TblUser.Phone = SecurityHelper.EncryptString(TblUser.Phone, _config.Value.SecurityKey);
                        _userRepository.Update(TblUser);
                        _userRepository.SaveChanges();
                    }
                    else
                    {
                        responseResult.message = "Error Occurs while mapping tbluser details";
                        responseResult.Status = false;
                        responseResult.Status_Code = HttpStatusCode.BadRequest;
                        return responseResult;
                    }
                }
                else
                {
                    responseResult.message = "UserData NotFound";
                    responseResult.Status = false;
                    responseResult.Status_Code = HttpStatusCode.BadRequest;
                    return responseResult;
                }
                #endregion

                #region Fill Registeration details
                if (UserVM != null && UserVM.UserId > 0)
                {
                    List<TblUserRole> tblUserRole = _userRoleRepository.GetList(x => x.IsActive == true && x.UserId == UserVM.UserId).ToList();
                    foreach (var roleId in tblUserRole)
                    {
                        TblRole tblRole = _roleRepository.GetSingle(x => x.IsActive == true && x.RoleId == roleId.RoleId);
                        if (tblRole.RoleName == EnumHelper.DescriptionAttr(ApiUserRoleEnum.Service_Partner).ToString())
                        {
                            TblServicePartner tblServicePartner = _servicePartnerRepository.GetSingle(x => x.IsActive == true && x.ServicePartnerEmailId == UserVM.Email);
                            result = _servicePartnerManager.ServicePartnerDetails(tblServicePartner.ServicePartnerId);

                            if (result.Status == true)
                            {
                                ServicePartnerdDetails = new LGCUserViewDataModel();
                                ServicePartnerdDetails = (LGCUserViewDataModel)result.Data;
                                loginUserDetailsDataViewModal.servicePatnerDetails = ServicePartnerdDetails;
                            }
                        }
                        if (tblRole.RoleName == EnumHelper.DescriptionAttr(ApiUserRoleEnum.Service_Partner_Driver).ToString())
                        {
                            //TblDriverDetail tblDriverDetail = _driverDetailsRepository.GetSingle(x => x.IsActive == true && x.UserId== UserVM.UserId);
                            DriverResponseViewModal driverDetails = new DriverResponseViewModal();
                            driverDetails = _driverDetailsManager.GetDriverDetailsById(UserVM.UserId);
                            if (driverDetails != null)
                            {
                                loginUserDetailsDataViewModal.driverDetails = driverDetails;
                            }
                        }
                    }
                    if (loginUserDetailsDataViewModal != null && loginUserDetailsDataViewModal.servicePatnerDetails != null || loginUserDetailsDataViewModal.driverDetails != null)
                    {
                        responseResult.message = "Success";
                        responseResult.Status = true;
                        responseResult.Status_Code = HttpStatusCode.OK;
                        responseResult.Data = loginUserDetailsDataViewModal;
                        return responseResult;
                    }
                    else
                    {
                        responseResult.message = "No Valid Role Assign to User";
                        responseResult.Status = false;
                        responseResult.Status_Code = HttpStatusCode.BadRequest;
                        return responseResult;
                    }
                }
                else
                {
                    responseResult.message = "UserData NotFound";
                    responseResult.Status = false;
                    responseResult.Status_Code = HttpStatusCode.BadRequest;
                    return responseResult;
                }
                #endregion
            }
            catch (Exception ex)
            {
                responseResult.message = ex.Message;
                responseResult.Status = false;
                responseResult.Status_Code = HttpStatusCode.InternalServerError;
                _logging.WriteErrorToDB("UserManager", "LoginUserDetails", ex);
            }
            return responseResult;
        }
        #endregion

        #region Update DeviceId
        public ResponseResult UpdateDeviceId(int userId)
        {
            ResponseResult responseResult = new ResponseResult();
            int result = 0;
            try
            {
                result = _mapLoginUserDeviceRepository.UpdateDeviceId(userId);
                if(result != 0)
                {
                    responseResult.message = "Logout Successful!!";
                    responseResult.Status = true;
                    responseResult.Status_Code = HttpStatusCode.OK; 
                    return responseResult;
                }
                else
                {
                    responseResult.message = "Error Occurred";
                    responseResult.Status = false;
                    responseResult.Status_Code = HttpStatusCode.BadRequest;
                }
            }
            catch(Exception ex)
            {
                responseResult.message = ex.Message;
                responseResult.Status = false;
                responseResult.Status_Code = HttpStatusCode.InternalServerError;
                _logging.WriteErrorToDB("UserManager", "UpdateDeviceId", ex);
            }
            return responseResult;
        }
        #endregion


    }
}