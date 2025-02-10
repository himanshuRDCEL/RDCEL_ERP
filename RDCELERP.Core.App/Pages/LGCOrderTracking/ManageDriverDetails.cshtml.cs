using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.Base;
using RDCELERP.Model.AbbRegistration;
using RDCELERP.DAL.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;
using RDCELERP.Model.City;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Data;
using ExcelDataReader;
using RDCELERP.Model.PinCode;
using RDCELERP.BAL.Helper;
using RDCELERP.Common.Helper;
using RDCELERP.Common.Enums;
using RDCELERP.Model.LGC;
using RDCELERP.Model.ServicePartner;
using RDCELERP.Model.DriverDetails;
using DocumentFormat.OpenXml.Office2010.Excel;
using RDCELERP.Model.MobileApplicationModel;
using AutoMapper;
using Cuemon;
using RDCELERP.Model.MobileApplicationModel.LGC;
using Newtonsoft.Json;
using RDCELERP.DAL.Repository;

namespace RDCELERP.Core.App.Pages.LGCOrderTracking
{
    public class ManageDriverDetailsModel : BasePageModel
    {
        #region Variable Declaration
        private readonly IWebHostEnvironment _webHostEnvironment;
        ILogging _logging;
        IDriverDetailsManager _driverDetailsManager;
        IServicePartnerManager _servicePartnerManager;
        private readonly IMapper _mapper;
        ILogisticManager _logisticManager;
        IServicePartnerRepository _servicePartnerRepository;
        #endregion

        #region Constructor
        public ManageDriverDetailsModel(IWebHostEnvironment webHostEnvironment, IOptions<ApplicationSettings> config, ILogging logging, IDriverDetailsManager driverDetailsManager, IMapper mapper, ILogisticManager logisticManager, IServicePartnerRepository servicePartnerRepository)
            : base(config)
        {
            _webHostEnvironment = webHostEnvironment;
            _logging = logging;
            _driverDetailsManager = driverDetailsManager;
            _mapper = mapper;
            _logisticManager = logisticManager;
            _servicePartnerRepository = servicePartnerRepository;
        }
        #endregion

        [BindProperty(SupportsGet = true)]
        public DriverDetailsViewModel driverDetailsVM { get; set; }
        public ServicePartnerViewModel servicePartnerVM { get; set; }
        public int SPId { get; set; }
        public IActionResult OnGet(int? DriverDetailsId)
        {
            int userId = 0;
            SPId = 0;
            try
            {
                if (DriverDetailsId > 0)
                {
                    driverDetailsVM = _driverDetailsManager.GetDriverDetailsByDriverId(DriverDetailsId ?? 0);
                }
                if (driverDetailsVM == null || driverDetailsVM.DriverDetailsId == 0)
                {
                    driverDetailsVM = new DriverDetailsViewModel();
                }
                
                if (_loginSession != null)
                {
                    userId = _loginSession.UserViewModel.UserId;
                    servicePartnerVM = _logisticManager.GetServicePartnerByUserId(userId);
                    if (servicePartnerVM != null)
                    {
                        SPId = servicePartnerVM.ServicePartnerId;
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ManageDriverDetails", "OnGet", ex);
            }
            return Page();
        }

        public IActionResult OnPostAsync()
        {
            ResponseResult? responseResult = null;
            int? userId = _loginSession.UserViewModel.UserId;
            try
            {
                if (driverDetailsVM != null)
                {
                    responseResult = _driverDetailsManager.ManageVehicle(driverDetailsVM, userId);
                    if (responseResult != null && responseResult.Status)
                    {
                        return RedirectToPage("LGCVehicleList");
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ManageDriverDetails", "OnPost", ex);
            }
            return RedirectToPage("ManageDriverDetails", new { DriverDetailsId = driverDetailsVM?.DriverDetailsId }); ;
        }

        #region AutoComplete Dropdowns : Service Partner, Driver, Customer City
        public IActionResult OnGetSearchServicePartner(string term)
        {
            var data = Array.Empty<SelectListItem>();
            if (term == null)
            {
                return BadRequest();
            }
            try
            {
                data = _servicePartnerRepository.GetSPListByBusinessName(term)
                .Select(s => new SelectListItem
                {
                    Value = s.ServicePartnerName,
                    Text = s.ServicePartnerId.ToString()
                })
                .ToArray();
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("UnAssignedOrdersList", "OnGetSearchServicePartner", ex);
            }
            return new JsonResult(data);
        }
        #endregion
    }
}
