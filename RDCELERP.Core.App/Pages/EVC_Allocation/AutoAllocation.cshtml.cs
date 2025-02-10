using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RDCELERP.BAL.Interface;
using RDCELERP.BAL.MasterManager;
using RDCELERP.Common.Enums;
using RDCELERP.Common.Helper;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.Model.EVC_Allocated;
using RDCELERP.Model.EVC_Portal;
using RDCELERP.Model.OrderTrans;

namespace RDCELERP.Core.App.Pages.EVC_Allocation
{
    public class AutoAllocationModel : PageModel
    {
        #region Variable Declaration
        IEVCManager _eVCManager;
        ILogging _logging;
        IOrderTransactionManager _orderTransactionManager;
        IBusinessUnitRepository _businessUnitRepository;
        #endregion

        #region Constructor
        public AutoAllocationModel(IEVCManager eVCManager, ILogging logging, IOrderTransactionManager orderTransactionManager, IBusinessUnitRepository businessUnitRepository)
        {
            _eVCManager = eVCManager;
            _logging = logging;
            _orderTransactionManager = orderTransactionManager;
            _businessUnitRepository = businessUnitRepository;
        }
        #endregion

        #region Model Binding
        [BindProperty(SupportsGet = true)]
        Allocate_EVCFromViewModel allocate { get; set; }
        List<EVC_PartnerViewModel> eVCPartnerViewModel { get; set; }

        #endregion

        public IActionResult OnGet(int? orderTransId, int? BuId)
        {
            bool flag = false;
            TblBusinessUnit businessUnit = new TblBusinessUnit();
            try
            {
                if(orderTransId> 0)
                {
                    if (BuId > 0)
                    {
                        businessUnit = _businessUnitRepository.GetSingle(x => x.BusinessUnitId == BuId && x.IsActive == true);
                    }
                    allocate = _orderTransactionManager.GetOrderDetailsByOrderTransId(orderTransId);
                    if (!string.IsNullOrEmpty(allocate.CustPin) && allocate.ProductCatId > 0 )
                    {
                        eVCPartnerViewModel = _eVCManager.GetListOfInHouseEvc(allocate);
                        if(eVCPartnerViewModel != null && eVCPartnerViewModel.Count > 0)
                        {
                            flag = _eVCManager.AssignEVCByPartnerId(allocate,eVCPartnerViewModel, orderTransId);
                            
                           
                            if(flag== true)
                            {
                                if(businessUnit.IsBulkOrder == true)
                                {
                                    return RedirectToPage("/Bulk_Order/Bulk_Upload");
                                }
                                else
                                {
                                    return RedirectToPage("/ThankYouPage/ThankYou");
                                }
                               
                            }
                            else
                            {
                                if (businessUnit.IsBulkOrder == true)
                                {
                                    return RedirectToPage("/Bulk_Order/Bulk_Upload");
                                }
                                else
                                {
                                    return RedirectToPage("/ThankYouPage/ThankYou");
                                }
                            }
                        }
                        else
                        {
                            eVCPartnerViewModel = _eVCManager.GetListOfEvcOtherThanInHouse(allocate);
                            if (eVCPartnerViewModel != null && eVCPartnerViewModel.Count > 0)
                            {
                                flag = _eVCManager.AssignEVCByPartnerId(allocate, eVCPartnerViewModel, orderTransId);
                                if(flag == true)
                                {
                                    if (businessUnit.IsBulkOrder == true)
                                    {
                                        return RedirectToPage("/Bulk_Order/Bulk_Upload");
                                    }
                                    else
                                    {
                                        return RedirectToPage("/ThankYouPage/ThankYou");
                                    }
                                }
                                else
                                {
                                    if (businessUnit.IsBulkOrder == true)
                                    {
                                        return RedirectToPage("/Bulk_Order/Bulk_Upload");
                                    }
                                    else
                                    {
                                        return RedirectToPage("/ThankYouPage/ThankYou");
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (businessUnit.IsBulkOrder == true)
                    {
                        return RedirectToPage("/Bulk_Order/Bulk_Upload");
                    }
                    else
                    {
                        return RedirectToPage("/ThankYouPage/ThankYou");
                    }
                }
            }
            catch(Exception ex)
            {
                _logging.WriteErrorToDB("AutoAllocationModel", "OnGet", ex);
            }
            if (businessUnit.IsBulkOrder == true)
            {
                return RedirectToPage("/Bulk_Order/Bulk_Upload");
            }
            else
            {
                return RedirectToPage("/ThankYouPage/ThankYou");
            }
        }
    }
}
