using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.BusinessPartner;
using RDCELERP.Model.InfoMessage;

namespace RDCELERP.BAL.Interface
{
    public interface IBusinessPartnerManager
    {

        /// <summary>
        /// Method to manage (Add/Edit) BusinessPartner
        /// </summary>
        /// <param name="BusinessPartnerVM">BusinessPartnerVM</param>
        /// <param name="userId">userId</param>
        /// <returns>int</returns>
        int ManageBusinessPartner(BusinessPartnerViewModel BusinessPartnerVM, int Id);

        /// <summary>
        /// Method to get the BusinessPartner by id 
        /// </summary>
        /// <param name="id">BusinessPartnerId</param>
        /// <returns>BusinessPartnerViewModel</returns>
        BusinessPartnerViewModel GetBusinessPartnerById(int id);

        /// <summary>
        /// Method to delete BusinessPartner by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        bool DeleteBusinessPartnerById(int id);

        /// <summary>
        /// Method to get the All BusinessPartner
        /// </summary>     
        /// <returns>List  BusinessPartnerViewModel</returns>

        IList<BusinessPartnerViewModel> GetAllBusinessPartner(int? BuiD);
        public ExecutionResult GetBusinessPartner();

        /// <summary>
        /// Method to get the Brand by id 
        /// </summary>
        /// <param name="id">BrandId</param>
        /// <returns>BrandViewModel</returns>
        public ExecutionResult BusinessPartnerById(int id);

        /// <summary>
        /// Method to get the BusinessPartner by BuId 
        /// </summary>
        /// <param name="id">BuId</param>
        /// <returns>List<BusinessPartnerViewModel></returns>
        public List<BusinessPartnerViewModel> GetListofBusinessPartnerByBUId(int BUId);
        public BusinessPartnerViewModel ManageBusinessPartnerBulk(BusinessPartnerViewModel BusinessPartnerVM, int userId);

    }
}

