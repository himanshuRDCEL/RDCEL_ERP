using AutoMapper;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Constant;
using RDCELERP.Common.Helper;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.DAL.Repository;
using RDCELERP.Model.BusinessCustomer;
using RDCELERP.Model.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace RDCELERP.BAL.MasterManager
{
    public class ItemBookingManager : IItemBookingManager
    {
        IItemRepository _ItemRepository;
        IBookingItemRepository _BookingItemRepository;
        IItemMasterRepository _ItemMasterRepository;
        IMapper _mapper;
        ILogging _logging;
        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();
        Interface.IRazorPayManager _paymentManager;
        ISynchronizedManager _syncManager;
        public ItemBookingManager(IMapper mapper,
        ILogging logging, IItemRepository itemRepository, IBookingItemRepository bookingItemRepository, Interface.IRazorPayManager paymentManager,ISynchronizedManager synchronizedManager,IItemMasterRepository itemMasterRepository) 
        {
            _ItemRepository= itemRepository;
            _BookingItemRepository= bookingItemRepository;
            _mapper = mapper;
            _logging = logging;
            _paymentManager = paymentManager;
            _syncManager = synchronizedManager;
            _ItemMasterRepository = itemMasterRepository;
        }

        /// <summary>
        /// method to add order in DB,Razorpay,Sync
        /// </summary>
        /// <param name="itemlistVM"></param>
        /// <param name="businessCustomerViewModel"></param>
        /// <param name="userid"></param>
        /// <returns></returns>

        public async Task<string> AddBookingItem(List<ItemCartViewModel> itemlistVM, BusinessCustomerViewModel businessCustomerViewModel, int userid)
        {
            string razorpayOrderId = string.Empty;

            try
            {
                if (itemlistVM != null && itemlistVM.Count > 0)
                {
                    //  Calculate total
                    decimal totalAmount = Convert.ToDecimal(itemlistVM[0]?.TotalPrice ?? 0);
                    decimal B2BtotalAmount = Convert.ToDecimal(itemlistVM[0]?.B2BTotalPrice ?? 0);
                    string FullName = businessCustomerViewModel.FirstName + " " + businessCustomerViewModel.LastName;
                    string orderNo = $"ORD{DateTime.Now:ddHHmm}-{new Random().Next(100, 999)}";

                    //  Call sync api import and get order ID
                    string syncRefrNo = await _syncManager.ImportOrdersToWmsAsync(itemlistVM, userid, orderNo);

                    if (syncRefrNo == null || string.IsNullOrEmpty(syncRefrNo))
                    {
                        return null;
                    }

                    //  Call Razorpay and get order ID
                    // var orderModel = _paymentManager.CreateOrder((int)totalAmount, "Test User", "test@example.com", "9999999999");
                    var orderModel = _paymentManager.CreateOrder((int)B2BtotalAmount, FullName, businessCustomerViewModel.Email, businessCustomerViewModel.PhoneNo, orderNo);
                    if (orderModel == null || string.IsNullOrEmpty(orderModel.OrderId))
                    {
                        return null;
                    }
                    razorpayOrderId = orderModel.OrderId;        
                    //  Save bookings in DB
                    foreach (var model in itemlistVM)
                    {
                        var tblitemMaster = _ItemMasterRepository.GetSingle(x => x.ItemMasterId == model.ItemMasterId);
                        if (tblitemMaster == null) continue;
                        var tblItem = _ItemRepository.GetSingle(x => x.ItemId == model.ItemId);
                        if (tblItem == null) continue;
                        TblBookingItem booking = new TblBookingItem
                        {
                            ItemId = model.ItemId,
                            OrderNo = orderNo,
                            BookingStatus="Pending",
                            ItemCode = tblItem.Itemcode,
                            EanNo = tblItem.Ean,
                            ItemMrp = tblItem.Mrp,
                            B2bPrice = tblitemMaster.B2bPrice,
                            BillingAmount = model.B2BTotalPrice,
                            Quantity = Convert.ToInt32(model.PurchaseQty),
                            RazorOrderId = razorpayOrderId,
                            PendingQty= itemlistVM.Count,
                            IsActive = true,
                            CustomerId = userid,
                            CreatedBy = userid,
                            SyncOrderNo = syncRefrNo,
                            CreatedDate = _currentDatetime
                        };

                        _BookingItemRepository.Create(booking);
                        _BookingItemRepository.SaveChanges();                        
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ItemBookingManager", "AddBookingItem", ex);
            }

            return razorpayOrderId;
        }

        /// <summary>
        /// method to get booking item detail with respect to customerId
        /// </summary>
        /// <param name="customerid"></param>
        /// <returns></returns>

        public List<BookingItemViewModel> GetBookingItemDetailByCustomerId(int customerid)
        {
            List<BookingItemViewModel> BookingItemViewModel = null;
            try
            {
               List<TblBookingItem> TblBookingItem = _BookingItemRepository.GetList(x=>x.CustomerId == customerid && x.IsActive==true && x.RazorOrderId !=null).ToList();
                if (TblBookingItem != null)
                {
                    BookingItemViewModel = _mapper.Map <List<TblBookingItem>,List< BookingItemViewModel>>(TblBookingItem);

                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ItemBookingManager", "GetBookingItemDetailByCustomerId", ex);
            }
            return BookingItemViewModel;
        }

        /// <summary>
        /// method to update order status
        /// </summary>
        /// <param name="bookingitemVMs"></param>
        /// <returns></returns>
        public int ManageOrderStatus(UpdateBookingItemViewModel bookingitemVMs)
        {
            List<TblBookingItem> TblBookingItem = new List<TblBookingItem>();
            int result = 0;
            try
            {
                if (bookingitemVMs != null)
                {
                    TblBookingItem=_BookingItemRepository.GetList(x=>x.SyncOrderNo== bookingitemVMs.SyncOrderNo).ToList();
                    if (TblBookingItem != null && TblBookingItem.Count > 0)
                    {
                        foreach (TblBookingItem bookingitemVM in TblBookingItem)
                        {
                            TblBookingItem temp = _BookingItemRepository.GetSingle(x => x.BookingItemId == bookingitemVM.BookingItemId);

                            if (temp != null)
                            {
                                temp.PendingQty = bookingitemVM.PendingQty;
                                temp.AdjustedQty = bookingitemVM.AdjustedQty;
                                temp.DispatchedQty = bookingitemVM.DispatchedQty;
                                temp.PickedQty = bookingitemVM.PickedQty;
                                temp.DeliveryDate = bookingitemVM.DeliveryDate;
                                temp.ModifiedDate = _currentDatetime;
                                _BookingItemRepository.Update(temp);
                                _BookingItemRepository.SaveChanges();

                                result = temp.BookingItemId;
                                
                            }
                        }
                    }

                    else
                    {
                        result = -1;
                    }
                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("UserManager", "ManageUser", ex);
            }

            return result;
        }

        /// <summary>
        /// method to get all order by customer id
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public async Task<List<OrderHistoryViewModel>> GetOrderHistoryAsync(int customerId)
        {
            var allItems = await _BookingItemRepository.GetAllBookingItemsByCustomerAsync(customerId);

            var groupedOrders = allItems
                .GroupBy(x => x.OrderNo)
                .Select(g => new OrderHistoryViewModel
                {
                    OrderId = g.First().OrderNo,
                    BookingDate = g.First().CreatedDate,
                    Items = g.Select(i => new ItemDetail
                    {
                        ItemName = i.Item.ItemDesc,
                        Quantity = i.Quantity,
                        BillingAmount = i.BillingAmount,
                        BookingStatusSummary = ResolveItemStatusSummary(i)
                    }).ToList()
                }).ToList();

            // Payment logic (optional)
            foreach (var order in groupedOrders)
            {
                var payment = await _BookingItemRepository.GetPaymentByOrderNoAsync(order.OrderId);
                if (payment != null)
                {
                    order.Payment = new PaymentDetail
                    {
                        Amount = payment.Amount,
                        PaymentStatus = payment.PaymentStatus,
                        PaymentMode = payment.Method
                    };
                }
            }

            return groupedOrders;
        }
        /// <summary>
        /// method to set item status
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private string ResolveItemStatusSummary(TblBookingItem item)
        {
            List<string> parts = new List<string>();

            int dispatched = item.DispatchedQty ?? 0;
            int picked = item.PickedQty ?? 0;
            int underPicking = item.UnderPicking ?? 0;
            int adjusted = item.AdjustedQty ?? 0;
            int pending = item.PendingQty ?? 0; 

            if (dispatched > 0)
                parts.Add($"Dispatched: {dispatched}");

            if (picked > 0)
                parts.Add($"Picked: {picked}");

            if (underPicking > 0)
                parts.Add($"UnderPicking: {underPicking}");

            if (adjusted > 0)
                parts.Add($"Adjusted: {adjusted}");

            if (pending > 0)
                parts.Add($"Pending: {pending}");

            return string.Join(", ", parts);
        }


        #region Admin 

       /// <summary>
       /// method to get order detail by order no
       /// </summary>
       /// <param name="orderNo"></param>
       /// <returns></returns>
        public async Task<AdminOrderDetailViewModel> GetOrderDetailsAsync(string orderNo)
        {
            var items = await _BookingItemRepository.GetOrderItemsAsync(orderNo);
            var payment = await _BookingItemRepository.GetPaymentByOrderNoAsync(orderNo);

            var first = items.FirstOrDefault();
            if (first == null) return null;

            // Map customer and manually build full name
            var customerVM = _mapper.Map<BusinessCustomerViewModel>(first.Customer);
            customerVM.FullName = $"{first.Customer.FirstName} {first.Customer.LastName}";

            // Map payment
            var paymentVM = _mapper.Map<BtoBPaymentViewModel>(payment ?? new TblBtoBpayment());

            // Map items
            var itemVMs = _mapper.Map<List<BookingItemViewModel>>(items);
            foreach (var vm in itemVMs)
            {
                var entity = items.FirstOrDefault(x => x.BookingItemId == vm.BookingItemId);

                if (entity != null && entity.Item != null)
                {
                    vm.ItemDesc = entity.Item.ItemDesc;
                    vm.ItemStatusName = ResolveItemStatusSummary(entity);
                }
            }



            var viewModel = new AdminOrderDetailViewModel
            {
                OrderNo = first.OrderNo,
                OrderDate = first.CreatedDate ?? DateTime.MinValue,
                OrderStatus = items.Any(x => x.PickedQty < x.Quantity) ? "Pending" : "Picked",
                CustomerVM = customerVM,
                PaymentVM = paymentVM,
                BookingItemsVm = itemVMs
            };

            return viewModel;
        }




        #endregion


    }
}
