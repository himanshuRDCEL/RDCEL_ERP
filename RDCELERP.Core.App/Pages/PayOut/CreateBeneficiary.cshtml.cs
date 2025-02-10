using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using RDCELERP.BAL.Interface;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.Base;
using RDCELERP.Model.CashfreeModel;
using RDCELERP.Model.Users;

namespace RDCELERP.Core.App.Pages.PayOut
{
    public class CreateBeneficiaryModel : BasePageModel
    {

        private readonly Digi2l_DevContext _context;
        private readonly IOptions<ApplicationSettings> _config;
        private readonly ICashfreePayoutCall _cashfreePayoutCall;


        public CreateBeneficiaryModel(IOptions<ApplicationSettings> config, Digi2l_DevContext context, ICashfreePayoutCall cashfreePayoutCall) : base(config)
        {
            _context = context;
            _config = config;
            _cashfreePayoutCall = cashfreePayoutCall;
        }

        [BindProperty(SupportsGet = true)]
        public AddBeneficiary addBeneficiary { get; set; }

        [BindProperty(SupportsGet = true)]
        public TblOrderTran? OrderTrans { get; set; }

        string returnUrl = string.Empty;

        public IActionResult OnGet(string RegdNumber)
        {
            if (!string.IsNullOrEmpty(RegdNumber))
            {
                OrderTrans = _context.TblOrderTrans.Where(x => x.RegdNo == RegdNumber).FirstOrDefault();
                if (OrderTrans != null)
                {
                    if (OrderTrans?.OrderType == 17)
                    {
                        TblExchangeOrder? tblExchangeOrder = _context.TblExchangeOrders.Include(x => x.CustomerDetails).Where(x => x.IsActive == true && x.RegdNo == RegdNumber).FirstOrDefault();
                        if (tblExchangeOrder != null && tblExchangeOrder.CustomerDetails != null)
                        {
                            addBeneficiary.beneId = RegdNumber;
                            addBeneficiary.name = tblExchangeOrder?.CustomerDetails?.FirstName + " " + tblExchangeOrder?.CustomerDetails?.LastName;
                            addBeneficiary.phone = tblExchangeOrder?.CustomerDetails?.PhoneNumber;
                            addBeneficiary.email = tblExchangeOrder?.CustomerDetails?.Email;
                            addBeneficiary.address1 = tblExchangeOrder?.CustomerDetails?.Address1;
                            addBeneficiary.city = tblExchangeOrder?.CustomerDetails?.City;
                            addBeneficiary.state = tblExchangeOrder?.CustomerDetails?.State;
                            addBeneficiary.pincode = tblExchangeOrder?.CustomerDetails?.ZipCode;
                        }
                        else
                        {
                            addBeneficiary.beneId = RegdNumber;
                        }
                    }
                    else
                    {
                        TblAbbredemption? tblAbbredemption = _context.TblAbbredemptions.Include(x => x.CustomerDetails).Where(x => x.IsActive == true && x.RegdNo == RegdNumber).FirstOrDefault();
                        if (tblAbbredemption != null && tblAbbredemption.CustomerDetails != null)
                        {
                            addBeneficiary.beneId = RegdNumber;
                            addBeneficiary.name = tblAbbredemption?.CustomerDetails?.FirstName + " " + tblAbbredemption?.CustomerDetails?.LastName;
                            addBeneficiary.phone = tblAbbredemption?.CustomerDetails?.PhoneNumber;
                            addBeneficiary.email = tblAbbredemption?.CustomerDetails?.Email;
                            addBeneficiary.address1 = tblAbbredemption?.CustomerDetails?.Address1;
                            addBeneficiary.city = tblAbbredemption?.CustomerDetails?.City;
                            addBeneficiary.state = tblAbbredemption?.CustomerDetails?.State;
                            addBeneficiary.pincode = tblAbbredemption?.CustomerDetails?.ZipCode;
                        }
                        else
                        {
                            addBeneficiary.beneId = RegdNumber;
                        }
                    }
                }
            }
            return Page();
        }

        public IActionResult OnPostBeneficiary()
        {
            AddBeneficiaryResponse addBeneficiaryResponse = new AddBeneficiaryResponse();
            CashfreeAuth cashfreeAuthCall = new CashfreeAuth();
            string message = string.Empty;
            returnUrl = "PayOut/RePaymentRecords";
            var beneficiary = addBeneficiary;

            var regdNo = beneficiary?.beneId?.Split("_")[0];
            OrderTrans = _context.TblOrderTrans.Include(x=>x.TblOrderQcs).Where(x => x.RegdNo == regdNo).FirstOrDefault();

            if(OrderTrans!= null && OrderTrans.AmountPaidToCustomer != true)
            {
                if (beneficiary != null)
                {
                    cashfreeAuthCall = _cashfreePayoutCall.CashFreeAuthCall();
                    if (cashfreeAuthCall != null)
                    {
                        addBeneficiaryResponse = _cashfreePayoutCall.AddBenefiaciary(beneficiary, cashfreeAuthCall?.data?.token);

                        if (addBeneficiaryResponse.subCode == "200")
                        {
                            //Update log for UPI id 
                            TblUpiidUpdatelog updateUpiLog = new TblUpiidUpdatelog();
                            TblOrderQc tblOrderQc = new TblOrderQc();

                            if (OrderTrans.TblOrderQcs!= null && OrderTrans.TblOrderQcs.Count>0 && !string.IsNullOrEmpty(OrderTrans.TblOrderQcs.FirstOrDefault()?.Upiid))
                            {
                                updateUpiLog.OldUpiid = OrderTrans.TblOrderQcs.FirstOrDefault()?.Upiid;
                                tblOrderQc = OrderTrans.TblOrderQcs.FirstOrDefault();
                            }
                            else
                            {
                                updateUpiLog.OldUpiid = string.Empty;
                            }

                            updateUpiLog.Upiid = beneficiary?.vpa;
                            updateUpiLog.CreatedBy = _loginSession?.UserViewModel?.UserId;
                            updateUpiLog.CreatedDate = DateTime.Now;
                            updateUpiLog.IsActive = true;
                            updateUpiLog.RegdNo = regdNo;
                            updateUpiLog.PayLoad = JsonConvert.SerializeObject(beneficiary).ToString();

                            // Update UPI id in OrderQC table
                            if (tblOrderQc != null && !string.IsNullOrEmpty(beneficiary.vpa) )
                            {
                                tblOrderQc.Upiid = beneficiary.vpa;
                            }

                            _context.TblUpiidUpdatelogs.Add(updateUpiLog);
                            _context.TblOrderQcs.Update(tblOrderQc);
                            _context.SaveChanges();

                            message = "Beneficiary Added Successfully for the Regd Number " + beneficiary.beneId;
                            return Redirect(_config.Value.BaseURL + "PayOut/Details/?message= " + message + "&ReturnURL=" + returnUrl);
                        }
                        else
                        {
                            message = "Beneficiary not added due to the cashfreee status: " + addBeneficiaryResponse.message;
                        }
                    }
                    else
                    {
                        message = "Beneficiary not added due to the cashfreee authentication";
                    }
                }
                else
                {
                    message = "Beneficiary details not found";
                }
            }
            else
            {
                message = "Can not update Beneficiary UPI Id due to, the Amount Already paid to Customer";
                return Redirect(_config.Value.BaseURL + "PayOut/DetailsForFailedTransaction/?message= " + message + "&ReturnURL=" + returnUrl);
            }

            return Redirect(_config.Value.BaseURL + "PayOut/DetailsForFailedTransaction/?message= " + message + "&ReturnURL=" + returnUrl);
        }
    }
}
