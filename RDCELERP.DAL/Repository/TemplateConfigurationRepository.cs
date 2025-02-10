using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.DAL.AbstractRepository;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;

namespace RDCELERP.DAL.Repository
{
    public class TemplateConfigurationRepository : AbstractRepository<TblConfiguration>, ITemplateConfigurationRepository
    {
        Digi2l_DevContext _context;
        public TemplateConfigurationRepository(Digi2l_DevContext dbContext)
         : base(dbContext)
        {
            _context = dbContext;
        }

        public TblConfiguration GetSingleTemplate(string TemplateName)
        {
            TblConfiguration TblConfiguration = null;
            if(TemplateName != null)
            {
                TblConfiguration = _context.TblConfigurations.FirstOrDefault(x => x.IsActive == true && x.Name== TemplateName);
            }
            return TblConfiguration;
        }

        #region Get Configurations by Config Key Name added by VK
        public TblConfiguration? GetConfigByKeyName(string configKeyName)
        {
            TblConfiguration? tblConfiguration = null;
            if (!string.IsNullOrEmpty(configKeyName))
            {
                tblConfiguration = _context.TblConfigurations.Where(x => x.IsActive == true && x.Name == configKeyName).FirstOrDefault();
            }
            return tblConfiguration;
        }
        #endregion

        #region Get List of Configurations added by VK
        public List<TblConfiguration> GetConfigurationList()
        {
            List<TblConfiguration>? TblConfigurationList = null;
            TblConfigurationList = _context.TblConfigurations.Where(x => x.IsActive == true).ToList();
            return TblConfigurationList;
        }
        #endregion
    }
}
