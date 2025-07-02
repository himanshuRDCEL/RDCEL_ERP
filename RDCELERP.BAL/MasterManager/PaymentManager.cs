using AutoMapper;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Helper;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.DAL.Repository;
using RDCELERP.Model.BusinessCustomer;
using RDCELERP.Model.RazorPay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.BAL.MasterManager
{
  public class PaymentManager : IPaymentManager
    {
        IBtoBpaymentRepository _btoBpaymentRepository;
        IItemCartManager _ItemCartManager;
        IBookingItemRepository _BookingItemRepository;
       IRazorPayManager _razorPayManager;
        IMapper _mapper;
        ILogging _logging;
        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();
        IItemRepository _itemRepository;
        public PaymentManager(IMapper mapper,
        ILogging logging, IBtoBpaymentRepository btoBpaymentRepository,IRazorPayManager razorPayManager, IItemCartManager itemCartManager, IBookingItemRepository bookingItemRepository,IItemRepository itemRepository)
        {
            _btoBpaymentRepository = btoBpaymentRepository;
            _mapper = mapper;
            _logging = logging;
            _razorPayManager = razorPayManager;
            _ItemCartManager = itemCartManager;
            _BookingItemRepository = bookingItemRepository;
            _itemRepository= itemRepository;
        }

        /// <summary>
        /// method to sev razorpay payment detail
        /// </summary>
        /// <param name="payVM"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public  async Task<int> ManagePayment(string paymentId, string orderId, int userid)
        {
            int result = 0;
            TblBtoBpayment TblBtoBpayment = new TblBtoBpayment();
            try
            {
                if (paymentId != null)
                {            // Get payment details from Razorpay
                    ResponseRazorpayPaymentViewModel responseRazorpayPayVM = await _razorPayManager.GetPaymentDetails(paymentId);


                    if (responseRazorpayPayVM != null)
                    {
                        TblBookingItem tblBookingItem=_BookingItemRepository.GetSingle(x=>x.RazorOrderId==orderId);
                       
                        TblBtoBpayment.Amount = TblBtoBpayment.Amount = responseRazorpayPayVM.amount / 100.0M;
                        ;
                        TblBtoBpayment.RazorpayPaymentId = responseRazorpayPayVM.id;
                        TblBtoBpayment.RazorpayOrderId = responseRazorpayPayVM.order_id;
                       TblBtoBpayment.Method = responseRazorpayPayVM.method;         
                        TblBtoBpayment.PaymentStatus = responseRazorpayPayVM.status;
                       TblBtoBpayment.OrderNo = tblBookingItem.OrderNo;
                        TblBtoBpayment.IsActive = true;
                        TblBtoBpayment.CreatedBy = userid;
                        TblBtoBpayment.CreatedDate = _currentDatetime;
                        _btoBpaymentRepository.Create(TblBtoBpayment);

                    }
                    _btoBpaymentRepository.SaveChanges();
                    result = TblBtoBpayment.PaymentId;

                    if (result > 0)
                    {
                        // Update booking table
                        var bookings = _BookingItemRepository.GetList(x => x.CustomerId == userid && x.IsActive == true && x.RazorOrderId == responseRazorpayPayVM.order_id).ToList();

                        if (bookings != null && bookings.Count > 0)
                        {
                            foreach (var booking in bookings)
                            {
                                booking.IsPaymentDone = true;
                                booking.PaymentDate = _currentDatetime;
                                booking.BookingStatus = "Paid";
                                booking.ModifiedBy = userid;
                                booking.ModifiedDate = _currentDatetime;
                                _BookingItemRepository.Update(booking);
                                _BookingItemRepository.SaveChanges();

                                bool flags = true;

                                if (flags == true)
                                {
                                    //Update inventory if booking inserted
                                    if (booking.BookingItemId > 0)
                                    {
                                        TblItem tblItem = _itemRepository.GetSingle(X => X.ItemId == booking.ItemId);
                                        decimal remainingQty = Convert.ToDecimal(tblItem.Qty) - Convert.ToDecimal(booking.Quantity);
                                        tblItem.Qty = remainingQty.ToString();
                                        tblItem.ModifiedDate = _currentDatetime;

                                        _itemRepository.Update(tblItem);
                                        _itemRepository.SaveChanges();
                                    }
                                }

                            }

                        }




                        bool flag = _ItemCartManager.UpdateCartPaymentStatus(userid);

                       
                    }
                }


            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("PaymentManager", "ManagePayment", ex);
            }

            return result;
        }

        public async Task<List<PaymentHistoryViewModel>> GetCustomerPaymentsAsync(int customerId)
        {
            var payments = await _btoBpaymentRepository.GetPaymentsByCustomerAsync(customerId);

            return payments.Select(p => new PaymentHistoryViewModel
            {
                OrderNo = p.OrderNo,
                Amount = p.Amount,
                PaymentStatus = p.PaymentStatus,
                PaymentMode = p.Method,
                //TransactionId = p.PaymentId,
                PaymentDate = Convert.ToDateTime(p.CreatedDate)
            }).ToList();
        }
    }
}
