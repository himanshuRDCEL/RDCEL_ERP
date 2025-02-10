using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Extensions.Options;
using System.Data;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.Helper;
using RDCELERP.Model.Base;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace RDCELERP.Core.App.Pages.ExchangeRegistration
{
    public class IndexModel : BasePageModel
    {
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        public readonly IOptions<ApplicationSettings> _config;
        public readonly ADOHelper _adoHelper;

        public IndexModel(IOptions<ApplicationSettings> config, Digi2l_DevContext context, ADOHelper adoHelper) : base(config)
        {
            _context = context;
            _adoHelper = adoHelper;
        }

        public void OnGet()
        
        {
            IList<TblUniversalPriceMaster> productCategories = new List<TblUniversalPriceMaster>();
            int priceMasterNameId = 5; // Pass the integer value
            productCategories = GetTblProductCategories(priceMasterNameId);
        }

        public IList<TblUniversalPriceMaster> GetTblProductCategories(int priceMasterNameId)
        {
            IList<TblUniversalPriceMaster> inthr = new List<TblUniversalPriceMaster>();
            
            DataTable dt = new DataTable();
            try
            {
                string dbConnectionString = string.Empty;
                //ADOHelper obj = new ADOHelper();
                SqlParameter[] sqlParam =  {

                        new SqlParameter("@PriceMasterNameId", priceMasterNameId)

                        };
                dt = _adoHelper.ExecuteDataTable("sp_GetProductCategoryByPriceMasterNameId", dbConnectionString, sqlParam);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            return inthr;
        }
    }
}
