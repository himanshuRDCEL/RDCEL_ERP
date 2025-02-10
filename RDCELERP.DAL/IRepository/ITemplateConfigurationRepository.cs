using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.DAL.AbstractRepository;
using RDCELERP.DAL.Entities;

namespace RDCELERP.DAL.IRepository
{
    public interface ITemplateConfigurationRepository : IAbstractRepository<TblConfiguration>
    {
        public TblConfiguration GetSingleTemplate(string TemplateName);

        #region Get Configurations by Config Key Name added by VK
        /// <summary>
        /// Get Configurations by Config Key Name added by VK
        /// </summary>
        /// <param name="configKeyName"></param>
        /// <returns>TblConfiguration</returns>
        public TblConfiguration? GetConfigByKeyName(string configKeyName);
        #endregion

        #region Get List of Configurations added by VK
        /// <summary>
        /// Get List of Configurations added by VK
        /// </summary>
        /// <returns>List<TblConfiguration></returns>
        public List<TblConfiguration> GetConfigurationList();
        #endregion
    }
}
