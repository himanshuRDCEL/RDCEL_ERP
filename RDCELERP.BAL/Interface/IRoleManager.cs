using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Role;

namespace RDCELERP.BAL.Interface
{
    public interface IRoleManager
    {
        /// <summary>
        /// Method to manage (Add/Edit) Role 
        /// </summary>
        /// <param name="RoleVM">RoleVM</param>
        /// <param name="userId">userId</param>
        /// <returns>int</returns>
        public int ManageRole(RoleViewModel RoleVM, int userId, int? companyId);

        /// <summary>
        /// Method to get the Role by id 
        /// </summary>
        /// <param name="id">RoleId</param>
        /// <returns>RoleViewModel</returns>
        public RoleViewModel GetRoleById(int? id, int? tabid);

        /// <summary>
        /// Method to get Role by user id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        RoleViewModel GetRoleByUserId(int? userId);
        /// <summary>
        /// Method to delete Role by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        bool DeletRoleById(int id);

        /// <summary>
        /// Method to get the list of Role
        /// </summary>
        /// <param name="startIndex">startIndex</param>
        /// <param name="maxRow">maxRow</param>
        /// <param name="sidx">sidx</param>
        /// <param name="sord">sord</param>
        /// <param name="txt">txt</param>
        /// <returns>RoleListViewModel</returns>
        RoleListViewModel GetRoleList(int startIndex, int maxRow, string sidx, string sord, string txt);

        /// <summary>
        /// Get Role List for Dropdown select option 
        /// </summary>
        public IList<RoleViewModel> GetAllRole(int? companyId);

        /// <summary>
        /// Get Role List for Dropdown by CompanyId
        /// </summary>
        IList<RoleViewModel> GetRoleByCompanyId(int? companyId);

        /// <summary>
        /// Method to get the All Role
        /// </summary>     
        /// <returns>RoleViewModel</returns>
        public IList<RoleViewModel> GetRoleListByUserId(int? companyId, int? roleId, int? userId);

        /// <summary>
        /// Method to get Role by userId and CompanyId
        /// </summary>
        /// <param name="userId and CompanyId"></param>
        /// <returns>bool</returns>
        public RoleViewModel GetRoleByUserIdAndCompanyId(int? userId, int? companyId);
        public RoleViewModel GetRoleByUserIdAndCompanyIdBUId(int? userId, int? companyId, int? BUId);
        //public int GetSaveAcess([FromBody] List<RoleAccessViewModel> checkboxValues);

    }
}
