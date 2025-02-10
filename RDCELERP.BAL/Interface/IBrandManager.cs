using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.Company;
using RDCELERP.Model.InfoMessage;
using RDCELERP.Model.MobileApplicationModel;

namespace RDCELERP.BAL.Interface
{
    public interface IBrandManager
    {
        /// <summary>
        /// Method to manage (Add/Edit) Brand 
        /// </summary>
        /// <param name="BrandVM">BrandVM</param>
        /// <param name="userId">userId</param>
        /// <returns>int</returns>
        public int ManageBrand(BrandViewModel BrandVM, int userId);

        /// <summary>
        /// Method to get the Brand by id 
        /// </summary>
        /// <param name="id">BrandId</param>
        /// <returns>BrandViewModel</returns>
        public Model.Company.BrandViewModel GetBrandById(int id);

        /// <summary>
        /// Method to delete Brand by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        public bool DeletBrandById(int id);

        /// <summary>
        /// Method to get the All Brand
        /// </summary>     
        /// <returns>BrandViewModel</returns>
        public IList<BrandViewModel> GetAllBrand();
        public ExecutionResult GetBrand();
        public BrandViewModel BrandById(int id);
        public ResponseResult GetBrandsByBUId(string username, int catid, int typeid);
        public BrandViewModel ManageBrandBulk(BrandViewModel BrandVM, int userId);
        public List<BrandViewModel> GetAllBrandForAbb(int NewProductCategoryId, int BuiD);

        #region GetBrands by procatid added by VK
        /// <summary>
        /// GetBrands by procatid added by VK
        /// </summary>
        /// <param name="username"></param>
        /// <param name="catid"></param>
        /// <returns></returns>
        public ResponseResult GetBrandsByCatIdV2(string username, int catid);
        #endregion
    }
}
