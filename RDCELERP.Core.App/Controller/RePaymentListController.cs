using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Ocsp;
using RDCELERP.Common.Helper;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.Repository;
using RDCELERP.Model.Base;
using RDCELERP.Model.Paymant;

namespace RDCELERP.Core.App.Controller
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RePaymentListController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogging _logging;
        private readonly Digi2l_DevContext _context;
        private IOptions<ApplicationSettings> _config;
        public RePaymentListController(IMapper mapper, Digi2l_DevContext context, ILogging logging, IOptions<ApplicationSettings> config)
        {
            _context = context;
            _mapper = mapper;
            _logging = logging;
            _config = config;
        }
        public async Task<ActionResult> GetRepaymentDataList(DateTime? startDate, DateTime? endDate)
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                List<TblPaymentLeaser> FailedPaymentsList = new List<TblPaymentLeaser>();

                string? TransactionType = "Dr";
                var count = _context.TblPaymentLeasers.Count(x => x.IsActive == true && x.PaymentStatus == false && x.Amount > 0  && x.IsDeleted != true && 
                    ((startDate == null && endDate == null) || (x.CreatedDate >= startDate && x.CreatedDate <= endDate)) && x.TransactionType == TransactionType);

                if (count > 0)
                {
                    FailedPaymentsList = _context.TblPaymentLeasers.Where(x => x.IsActive == true && x.PaymentStatus == false && x.Amount > 0 && x.IsDeleted != true &&
                    ((startDate == null && endDate == null)
                                            || (x.CreatedDate >= startDate && x.CreatedDate <= endDate)) && x.TransactionType == TransactionType).OrderByDescending(x=>x.CreatedDate).ToList();

                }

                recordsTotal = FailedPaymentsList != null ? FailedPaymentsList.Count : 0;
                if (FailedPaymentsList != null)
                {
                    FailedPaymentsList = FailedPaymentsList.Skip(skip).Take(pageSize).ToList();
                }
                else
                    FailedPaymentsList = new List<TblPaymentLeaser>();


                //List<RePaymentViewModel> ListOfFailedPayments = _mapper.Map<List<TblPaymentLeaser>, List<RePaymentViewModel>>(FailedPaymentsList);

                List<RePaymentViewModel> ViewModelList = new List<RePaymentViewModel>();

                foreach (var item in FailedPaymentsList)
                {
                    RePaymentViewModel viewDataDc = new RePaymentViewModel();
                    if (item != null)
                    {
                        string URL = _config.Value.BaseURL + "PayOut/paymentStatus";
                        string actionURL = string.Empty;
                        actionURL = " <div class='actionbtns'>";
                        actionURL = "<a href ='" + URL + "?RegdNo=" + item.RegdNo + "&UtcReferenceId=" + item.UtcreferenceId + "'><button onclick='RecordView(" + item.RegdNo + ")' class='btn btn-sm btn-primary' data-bs-toggle='tooltip' data-bs-placement='top' title='View'><i class='fa-solid fa-eye'></i></button></a>";
                        actionURL = actionURL + "</div>";
                        viewDataDc.ActionURL = actionURL;
                        viewDataDc.Action = actionURL;
                        viewDataDc.RegdNo = item.RegdNo;
                        viewDataDc.Id = item.Id;
                        viewDataDc.OrderId = item.OrderId;
                        viewDataDc.Amount = item.Amount;
                        viewDataDc.PaymentDate = item.PaymentDate;
                        viewDataDc.PaymentResponse = item.PaymentResponse;
                        viewDataDc.PaymentStatus = item.PaymentStatus;
                        viewDataDc.IsActive = item.IsActive;
                        viewDataDc.ModuleType = item.ModuleType;
                        viewDataDc.TransactionId = item.TransactionId;
                        viewDataDc.TransactionType = item.TransactionType;
                        viewDataDc.CreatedDate = item.CreatedDate;
                        viewDataDc.UtcReferenceId = item.UtcreferenceId;

                        ViewModelList.Add(viewDataDc);
                    }

                }

                var data = ViewModelList;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("RePaymentListController", "RePaymentDataList", ex);
            }
            return Ok();
        }

        public async Task<ActionResult> GetRepaymentDataListForSuccessPayment(DateTime? startDate, DateTime? endDate)
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                List<TblPaymentLeaser> SuccessPaymentsList = new List<TblPaymentLeaser>();

                string? TransactionType = "Dr";
                var count = _context.TblPaymentLeasers.Count(x => x.IsActive == true && x.PaymentStatus == true && x.Amount > 0 && x.IsDeleted != true &&
                    ((startDate == null && endDate == null) || (x.CreatedDate >= startDate && x.CreatedDate <= endDate)) && x.TransactionType == TransactionType);

                if (count > 0)
                {
                    SuccessPaymentsList = _context.TblPaymentLeasers.Where(x => x.IsActive == true && x.PaymentStatus == true && x.Amount > 0 && x.IsDeleted != true &&
                    ((startDate == null && endDate == null)
                                            || (x.CreatedDate >= startDate && x.CreatedDate <= endDate)) && x.TransactionType == TransactionType).OrderByDescending(x => x.CreatedDate).ToList();

                }

                recordsTotal = SuccessPaymentsList != null ? SuccessPaymentsList.Count : 0;
                if (SuccessPaymentsList != null)
                {
                    SuccessPaymentsList = SuccessPaymentsList.Skip(skip).Take(pageSize).ToList();
                }
                else
                    SuccessPaymentsList = new List<TblPaymentLeaser>();


                //List<RePaymentViewModel> ListOfFailedPayments = _mapper.Map<List<TblPaymentLeaser>, List<RePaymentViewModel>>(FailedPaymentsList);

                List<RePaymentViewModel> ViewModelList = new List<RePaymentViewModel>();

                foreach (var item in SuccessPaymentsList)
                {
                    RePaymentViewModel viewDataDc = new RePaymentViewModel();
                    if (item != null)
                    {
                        string URL = _config.Value.BaseURL  + "PayOut/paymentStatus";
                        string actionURL = string.Empty;
                        actionURL = " <div class='actionbtns'>";
                        actionURL = "<a href ='" + URL + "?RegdNo=" + item.RegdNo + "&UtcReferenceId=" + item.UtcreferenceId + "'><button onclick='RecordView(" + item.RegdNo + ")' class='btn btn-sm btn-primary' data-bs-toggle='tooltip' data-bs-placement='top' title='View'><i class='fa-solid fa-eye'></i></button></a>";
                        viewDataDc.ActionURL = actionURL;
                        viewDataDc.Action = actionURL;
                        viewDataDc.RegdNo = item.RegdNo;
                        viewDataDc.Id = item.Id;
                        viewDataDc.OrderId = item.OrderId;
                        viewDataDc.Amount = item.Amount;
                        viewDataDc.PaymentDate = item.PaymentDate;
                        viewDataDc.PaymentResponse = item.PaymentResponse;
                        viewDataDc.PaymentStatus = item.PaymentStatus;
                        viewDataDc.IsActive = item.IsActive;
                        viewDataDc.ModuleType = item.ModuleType;
                        viewDataDc.TransactionId = item.TransactionId;
                        viewDataDc.TransactionType = item.TransactionType;
                        viewDataDc.CreatedDate = item.CreatedDate;
                        viewDataDc.UtcReferenceId = item.UtcreferenceId;

                        ViewModelList.Add(viewDataDc);
                    }

                }

                var data = ViewModelList;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("RePaymentListController", "RePaymentSuccessDataList", ex);
            }
            return Ok();
        }
    }
}