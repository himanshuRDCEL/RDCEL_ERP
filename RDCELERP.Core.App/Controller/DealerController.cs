using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Helper;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.Base;
using RDCELERP.Model.DealerDashBoard;
using RDCELERP.Model.Users;

namespace RDCELERP.Core.App.Controller
{
    public class DealerController : ControllerBase
    {
        private Digi2l_DevContext _context;
        private IMapper _mapper;
        private CustomDataProtection _protector;
        private IOptions<ApplicationSettings> _config;
        private LoginViewModel _loginSession;
        public IDealerManager _dealerManager;
        public ILogging _logging;

        public DealerController(IMapper mapper, Digi2l_DevContext context, CustomDataProtection protector, IOptions<ApplicationSettings> config, IDealerManager dealerManager, ILogging logging)
        {
            _context = context;
            _mapper = mapper;
            _protector = protector;
            _config = config;
            _dealerManager = dealerManager;
            _logging = logging;
        }
        [HttpGet]
        // public JsonResult OrderDataTableForDealer(ExchangeOrderDataContract exchageOrderObj)
        public JsonResult OrderDataTableForDealer(ExchangeOrderDataContract exchageOrderObj)
        {
            var draw = Request.Form["draw"].FirstOrDefault();
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            searchValue = searchValue.Trim();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;
            List<ExchangeOrderDataContract> exchangeObj = new List<ExchangeOrderDataContract>();
            List<DealerDashboardViewModel> dealerdatalist = null;
            try
            {
                //ExchangeOrderDataContract exchageOrderObj = new ExchangeOrderDataContract();
                //exchageOrderObj.startdate = startdate;
                //exchageOrderObj.enddate = enddate;
                //exchageOrderObj.BusinessPartnerId = businessPartnerId;
                //exchageOrderObj.AssociateCode = AssociateCode;
                //exchageOrderObj.userCompany = userCompany;
                //exchageOrderObj.userRole = userRole;
                //exchageOrderObj.BusinessUnitId = BusinessUnitId;
                //exchageOrderObj.CompanyName = CompanyName;

                if (exchageOrderObj.startdate != null && exchageOrderObj.enddate != null)
                {
                    exchageOrderObj.StartRangedate = Convert.ToDateTime(exchageOrderObj.startdate);
                    exchageOrderObj.EndRangeDate = Convert.ToDateTime(exchageOrderObj.enddate);
                }


                dealerdatalist = _dealerManager.GetDashboardList(exchageOrderObj, skip, pageSize);

                recordsTotal = dealerdatalist.Count;

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("DealerDashboardModel", "OnGetStoreList", ex);
            }

            var data = dealerdatalist;
            var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
            return new JsonResult(jsonData);
        }
    }
}
