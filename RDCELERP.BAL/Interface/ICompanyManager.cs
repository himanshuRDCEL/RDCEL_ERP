using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.BusinessUnit;
using RDCELERP.Model.Company;

namespace RDCELERP.BAL.Interface
{
  public  interface ICompanyManager
    {
        /// <summary>
        /// Method to manage (Add/Edit) Company 
        /// </summary>
        /// <param name="CompanyVM">CompanyVM</param>
        /// <param name="userId">userId</param>
        /// <returns>int</returns>
        public int ManageCompany(CompanyViewModel CompanyVM, int userId);

        /// <summary>
        /// Method to get the Company by id 
        /// </summary>
        /// <param name="id">CompanyId</param>
        /// <returns>CompanyViewModel</returns>
        public CompanyViewModel GetCompanyById(int id);

        /// <summary>
        /// Method to get the Company by BUId 
        /// </summary>
        /// <param name="id">CompanyId</param>
        /// <returns>CompanyViewModel</returns>
        public CompanyViewModel GetCompanyByBUId(int id);

        /// <summary>
        /// Method to delete Company by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        public bool DeletCompanyById(int id);

        /// <summary>
        /// Method to get the list of Company
        /// </summary>
        /// <param name="startIndex">startIndex</param>
        /// <param name="maxRow">maxRow</param>
        /// <param name="sidx">sidx</param>
        /// <param name="sord">sord</param>
        /// <param name="txt">txt</param>
        /// <returns>CompanyListViewModel</returns>
        public CompanyListViewModel GetCompanyList(int startIndex, int maxRow, string sidx, string sord, string txt);

        /// <summary>
        /// Method to get the all Company
        /// </summary>       
        /// <returns>CompanyViewModel</returns>
        public IList<CompanyViewModel> GetAllCompany(int? companyId, int? roleId, int? userId);

        /// <summary>
        /// Method to get the All Company for Assign Role to User
        /// </summary>     
        /// <returns>CompanyViewModel</returns>
        public IList<CompanyViewModel> GetCompanyToAssignRole(int? companyId, int? roleId, int? userId, int? selecteduserid);


        /// <summary>
        /// Method to get the BusinessUnit Data from selected Company data for Assign Role to User
        /// </summary>     
        /// <returns>BusinessUnitViewModel</returns>
        public BusinessUnitViewModel GetBusinessUnitByCompanyId(int CompanyId);
      
    }
}
