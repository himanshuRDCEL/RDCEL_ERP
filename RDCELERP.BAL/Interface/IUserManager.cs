using RDCELERP.Model.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using RDCELERP.Model.InfoMessage;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.MobileApplicationModel;

namespace RDCELERP.BAL.Interface
{
    public interface IUserManager
    {
        /// <summary>
        /// Method to manage (Add/Edit) User 
        /// </summary>
        /// <param name="UserVM">UserVM</param>
        /// <param name="userId">userId</param>
        /// <returns>int</returns>
        int ManageUser(UserViewModel UserVM, int userId, int? companyId);

        /// <summary>
        /// Method to get the user object by login detail
        /// </summary>
        /// <param name="username">username</param>
        /// <param name="password">password</param>
        /// <returns>LoginViewModel</returns>
        LoginViewModel GetUserByLogin(string username, string password);

        /// <summary>
        /// Method to get the list of all user Role by id 
        /// </summary>
        /// <returns>List of UserViewModel</returns>
        IList<UserViewModel> GetAllUsersByRole(int roleId);
        public ExecutionResult UserByLogin(string username, string password);

        public ExecutionResult UpdateUserPassword(string encpwd1, string encpwd, string pwd);

        /// <summary>
        /// MEthod to send the notification for User
        /// </summary>
        /// <param name="userObj">userObj</param>
        /// <param name="company">company</param>
        /// <param name="subject">subject</param>
        /// <param name="tempateName">tempateName</param>
        /// <returns>void</returns>
        public void UserNotification(List<TblUser> userObjList, TblCompany company, string subject, string tempateName);
        public ExecutionResult ChangePassword(int userid, string oldPassword, string NewPassword, string ConfirmPassword);


        /// <summary>
        /// Method to get the User by id 
        /// </summary>
        /// <param name="id">UserId</param>
        /// <returns>UserViewModel</returns>
        UserViewModel GetUserById(int id);

        /// <summary>
        /// Method to delete User by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        bool DeletUserById(int id);

        /// <summary>
        /// Method to manage (Add/Edit) User Role
        /// </summary>
        /// <param name="UserRoleVM">UserRoleVM</param>
        /// <param name="userId">userId</param>
        /// <returns>int</returns>
        int ManageUserRole(UserRoleViewModel UserRoleVM, int userId, int? companyId);

        /// <summary>
        /// Method to get Sales User list 
        /// </summary>
        /// <param name="UserVM">UserRoleVM</param>
        /// <param name="companyId">userId</param>
        /// <returns>int</returns>
        IList<UserViewModel> GetSalesSUserList(int? companyId);

        /// <summary>
        /// Method to get the User Role by id 
        /// </summary>
        /// <param name="id">UserRoleId</param>
        /// <returns>UserRoleViewModel</returns>
        UserRoleViewModel GetUserRoleById(int id);

        /// <summary>
        /// Method to get List of users list with Role
        /// </summary>
        /// <returns>UserViewModel</returns>
        public IList<UserViewModel> GetUsersListWithRole(int? companyId);
        public IList<UserViewModel> GetAllUser();
        public ExecutionResult RoleByLogin(string email, string pwd, int roleId, int userId);
        public UserViewModel UserById(int id);
        public int ForgotPassword(string email, string encpwd, string pwd);
        public int UpdateUserChangePassword(int userid, int? companyId, string oldPassword, string password);
        public UserViewModel GetUserByEmailandPasswordLogin(string username, string password);
        public int NotificationAllUsers(UserViewModel UserVM, string subject, string tempateName);

        public bool UserNotificationEVC(UserViewModel userObj, string subject, string tempateName);

        public int ManageEvcUser(TblEvcregistration tblEvcReg, int loginUserId, int? loginUserCompId);

        public int ManageUserAndUserRole(UserViewModel userVM, int loginUserId);

        public ResultResponse LoginUserDetails(userDataViewModal userDataViewModal);

        public ResponseResult UpdatePassword(string encpwd1, string encpwd, string pwd);

        public int ManageServicePartnerUser(TblServicePartner tblServicePartner, int loginUserId, int? loginUserCompId);

        public userDataViewModal GetUserByNumber(string mobNumber);
       
        public bool UserNotificationServicePartner(UserViewModel userObj, string subject, string tempateName);

        public ResponseResult UpdateDeviceId(int userId);

    }
}

