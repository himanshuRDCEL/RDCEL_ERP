using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RDCELERP.Common.Helper;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.Model.Base;
using RDCELERP.Model.Refurbisher;

namespace RDCELERP.Core.App.Controller
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RefurbisherController : ControllerBase
    {
        #region Variable
        private Digi2l_DevContext _context;
        private IMapper _mapper;
        private CustomDataProtection _protector;
        private IOptions<ApplicationSettings> _config;
        ILogging _logging;
        IStateRepository _stateRepository;
        ICityRepository _cityRepository;
        #endregion

        #region Constructor
        public RefurbisherController(Digi2l_DevContext context, CustomDataProtection protector, IMapper mapper, IOptions<ApplicationSettings> config, ILogging logging, IStateRepository stateRepository, ICityRepository cityRepository)
        {
            _context = context;
            _protector = protector;
            _mapper = mapper;
            _config = config;
            _logging = logging;
            _stateRepository = stateRepository;
            _cityRepository = cityRepository;
        }
        #endregion

        #region Model
        string actionURL = string.Empty;
        #endregion

        #region ABB Registration Approve List
        [HttpPost]
        public async Task<ActionResult> RefurbisherList(int companyId)
        {
            #region Variable declaration            
            string URL = _config.Value.URLPrefixforProd;
            string MVCURL = _config.Value.MVCBaseURLForExchangeInvoice;
            List<TblRefurbisherRegistration> tblRefurbisherRegistrations = null;
            TblState tblState = null;
            TblCity tblCity = null;
            int count = 0;
            #endregion

            try
            {
                #region Datatable form variables
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                int searchByBusinessUnitId = 0;
                #endregion

                #region table object Initialization
                count = _context.TblRefurbisherRegistrations.Count(x => x.IsActive == true);

                if (count > 0)
                {
                    tblRefurbisherRegistrations = await _context.TblRefurbisherRegistrations.
                                Where(x => x.IsActive == true).OrderByDescending(x => x.CreatedDate).ThenByDescending(x => x.RefurbisherId).Skip(skip).Take(pageSize).ToListAsync();

                    recordsTotal = count;
                }
                #endregion

                recordsTotal = tblRefurbisherRegistrations != null ? tblRefurbisherRegistrations.Count : 0;
                if (tblRefurbisherRegistrations != null)
                {
                    tblRefurbisherRegistrations = tblRefurbisherRegistrations.Skip(skip).Take(pageSize).ToList();
                }
                else
                    tblRefurbisherRegistrations = new List<TblRefurbisherRegistration>();

                List<RefurbisherRegViewModel> RefurbisherRegList = _mapper.Map<List<TblRefurbisherRegistration>, List<RefurbisherRegViewModel>>(tblRefurbisherRegistrations);

                foreach (RefurbisherRegViewModel item in RefurbisherRegList)
                {
                    tblState = _stateRepository.GetStateById(item.StateId);
                    if (tblState != null)
                    {
                        item.StateName = tblState.Name != null ? tblState.Name : null;
                    }

                    tblCity=_cityRepository.GetCityById(item.cityId); 
                    if (tblCity != null)
                    {
                        item.CityName = tblCity.Name != null ? tblCity.Name : null;
                    }

                    string actionURL = " <div class='actionbtns'>";
                    //actionURL = "<a href ='" + URL + "/Refurbisher/RefurbisherRegistration?id=" + item.RefurbisherId + "' >" +
                    //    "<button onclick='RecordView(" + item.RefurbisherId + ")' class='btn btn-sm btn-primary' data-bs-toggle='tooltip' data-bs-placement='top' title='View'><i class='fa-solid fa-eye'></i></button></a>";
                    actionURL = "<a class='mx-1' href ='" + URL + "/Refurbisher/RefurbisherRegistration?id=" + item.RefurbisherId + "' >" +
                        "<button onclick='RefurbisherEdit(" + item.RefurbisherId + ")' class='btn btn-sm btn-primary'data-bs-toggle='tooltip' data-bs-placement='op' title='Edit'><i class='fa-solid fa-pen'></i></button></a>" +
                        "<button onclick='RefurbisherDelete(" + item.RefurbisherId + ")' class='btn btn-sm btn-danger'data-bs-toggle='tooltip' data-bs-placement='top' title='Reject'><i class='fa-solid fa-xmark'></i></button>";

                    actionURL += "</div>";
                    item.Action = actionURL;
                }

                var data = RefurbisherRegList;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("RefurbisherController", "RefurbisherList", ex);
            }
            return Ok();


        }
        #endregion
    }
}
