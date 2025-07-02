using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Helper;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.DAL.Repository;
using RDCELERP.Model.BusinessCustomer;
using RDCELERP.Model.Company;
using RDCELERP.Model.SynchronizedModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.BAL.MasterManager
{
    public class ItemCartManager : IItemCartManager
    {
        IItemRepository _ItemRepository;
        IItemCartRepository _ItemCartRepository;
        IMapper _mapper;
        ILogging _logging;
        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();
        IItemMasterRepository _ItemMasterRepository;
        ISynchronizedManager _SynchronizedManager;
        public ItemCartManager(IMapper mapper,
        ILogging logging, IItemRepository itemRepository, IItemCartRepository ItemCartRepository, IItemMasterRepository itemMasterRepository, ISynchronizedManager synchronizedManager)
        {
            _ItemRepository = itemRepository;
            _ItemCartRepository = ItemCartRepository;
            _mapper = mapper;
            _logging = logging;
            _ItemMasterRepository = itemMasterRepository;
            _SynchronizedManager = synchronizedManager;
        }
        /// <summary>
        /// method to get cart items
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public List<ItemCartViewModel> GetItemCartList(int userid)
        {
            List<ItemCartViewModel> ItemListVM = null;
            List<TblItemCart> TblItemCart = null;

            try
            {
                TblItemCart = _ItemCartRepository.GetList(x => x.IsActive == true && x.CreatedBy==userid && x.IsPaymentComplete!=true).ToList();
                if (TblItemCart != null)
                {

                    ItemListVM = _mapper.Map<List<TblItemCart>, List<ItemCartViewModel>>(TblItemCart);

                    foreach (ItemCartViewModel item in ItemListVM)
                    {
                        TblItemMaster tblItemMaster = _ItemMasterRepository.GetSingle(X => X.ItemMasterId == item.ItemMasterId);
                        if (tblItemMaster != null)
                            item.ImageUrl = tblItemMaster.ItemImage1;
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ItemCartManager", "GetItemCartList", ex);
            }
            return ItemListVM;
        }

        /// <summary>
        /// method to add Cart item
        /// </summary>
        /// <param name="itemcartVM"></param>
        /// <returns></returns>
        public int ManageItemCart(ItemCartViewModel itemcartVM, int userid)
        {
            int result = 0;
            TblItemCart TblItemCart = null;
            try
            {
                if (itemcartVM != null)
                {
                    TblItemCart= _mapper.Map<ItemCartViewModel, TblItemCart>(itemcartVM);

                    if (Convert.ToInt32(TblItemCart.PurchaseQty) > 0 && TblItemCart.PurchaseQty !=null)
                    {

                        TblItemCart.SubTotalPrice = TblItemCart.B2bPrice * TblItemCart.PurchaseQty;
                    }

                    if (TblItemCart.ItemCartId > 0)
                    {
                        TblItemCart.ModifiedBy = userid;
                        TblItemCart.ModifiedDate = _currentDatetime;
                        _ItemCartRepository.Update(TblItemCart);

                    }
                    else
                    {
                        TblItemCart.IsActive = true;
                        TblItemCart.CreatedBy = userid;
                        TblItemCart.CreatedDate = _currentDatetime;
                        _ItemCartRepository.Create(TblItemCart);
                       
                    }
                    _ItemCartRepository.SaveChanges();
                    result = TblItemCart.ItemCartId;
                }          

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ItemCartManager", "AddItemCart", ex);
            }

            return result;
        }

        /// <summary>
        /// remove item from cart
        /// </summary>
        /// <param name="itemCartId"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
       
        public ResponseCartVM RemoveItem(RemoveItemVM removeItemVM, int userid)
        {
            var response = new ResponseCartVM();

            try
            {
                var cartItem = _ItemCartRepository.GetSingle(x =>
                    x.IsActive == true && x.ItemCartId == removeItemVM.ItemCartId && x.CreatedBy == userid);

                if (cartItem != null)
                {
                    cartItem.IsActive = false;
                    cartItem.ModifiedBy = userid;
                    cartItem.ModifiedDate = _currentDatetime;

                    _ItemCartRepository.Update(cartItem);
                    _ItemCartRepository.SaveChanges();

                    var activeItems = _ItemCartRepository.GetList(x =>
                        x.IsActive == true && x.CreatedBy == userid && x.IsPaymentComplete != true).ToList();

                    decimal cartTotal = activeItems.Sum(x =>Convert.ToDecimal(x.PurchaseQty) * Convert.ToDecimal(x.B2bPrice));

                    response.Result = 1;
                    //response.ItemCartId = removeItemVM.ItemCartId;
                    response.TotalPrice = cartTotal;
                }
                else
                {
                    response.Result = 0;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ItemCartManager", "RemoveItem", ex);
                response.Result = 0;
            }

            return response;
        }

        /// <summary>
        /// check item qnty
        /// </summary>
        /// <param name="itemid"></param>
        /// <param name="qnty"></param>
        /// <returns></returns>
        public async Task<bool> CheckItemQnty(int itemid,int qnty)
        {
            TblItem tblItem = null;
            bool flag = false;
            var stockQty = string.Empty;
            try
            {
                tblItem = _ItemRepository.GetSingle(x => x.IsActive == true && x.ItemId == itemid);
                if (tblItem != null)
                {
                    //get item qnty by sync
                    List<LstStockReport> stockReports = await _SynchronizedManager.GetStockDataAsync(tblItem.Itemcode, tblItem.Ean);

                    if (stockReports != null && stockReports.Count>0)
                    {
                        stockQty = stockReports.First().Qty.ToString();

                       // if (Convert.ToInt32(tblItem.Qty) >= qnty)
                        if (Convert.ToInt32(stockQty) >= qnty)
                        {
                            flag = true;
                        }
                        else
                        {
                            flag = false;
                        }
                        tblItem.Qty = stockQty;
                        tblItem.ModifiedDate = _currentDatetime;
                        _ItemRepository.Update(tblItem);
                        _ItemRepository.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ItemCartManager", "CheckItemQnty", ex);
            }
            return flag;
        }
        /// <summary>
        /// method to update cart item qnty
        /// </summary>
        /// <param name="quantityVM"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public async Task<ResponseCartVM> ManageItemQuantity(QuantityVM quantityVM, int userid)
        {
            ResponseCartVM responseCartVM = new ResponseCartVM();
            try
            {
                var tblItemCart = _ItemCartRepository.GetSingle(x =>
                    x.IsActive == true &&
                    x.CreatedBy == userid &&
                    x.ItemCartId == quantityVM.ItemCartId);

                if (tblItemCart != null)
                {
                    bool isAvailable = await CheckItemQnty(quantityVM.ItemId, quantityVM.Quantity);

                    if (isAvailable)
                    {
                        var tblItemMaster = _ItemMasterRepository.GetSingle(x => x.ItemMasterId == quantityVM.ItemMasterId);

                        if (tblItemMaster != null)
                        {
                            tblItemCart.PurchaseQty = quantityVM.Quantity;
                            tblItemCart.SubTotalPrice = tblItemMaster.B2bPrice * quantityVM.Quantity;
                            tblItemCart.TotalPrice = tblItemCart.SubTotalPrice;
                            tblItemCart.ModifiedDate = _currentDatetime;
                            tblItemCart.ModifiedBy = userid;

                            _ItemCartRepository.Update(tblItemCart);
                            _ItemCartRepository.SaveChanges();

                            var allItems = _ItemCartRepository.GetList(x =>
                                x.IsActive == true &&
                                x.CreatedBy == userid &&
                                x.IsPaymentComplete != true
                            ).ToList();

                            decimal totalCartAmount = allItems.Sum(x => x.SubTotalPrice ?? 0);

                            responseCartVM.Result = 1;
                           // responseCartVM.item = tblItemCart.ItemCartId;
                            responseCartVM.Quantity = quantityVM.Quantity;
                            responseCartVM.SubTotalPrice = tblItemCart.SubTotalPrice ?? 0;
                            responseCartVM.TotalPrice = totalCartAmount;
                        }
                    }
                    else
                    {
                        responseCartVM.Result = -1;
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ItemCartManager", "ManageItemQuantity", ex);
                responseCartVM.Result = 0;
            }

            return responseCartVM;
        }

        public bool UpdateCartPaymentStatus(int businessCustomerId)
        {
            try
            {
                List<TblItemCart> TblItemCart = _ItemCartRepository.GetList(x => x.IsActive == true && x.CreatedBy == businessCustomerId && x.IsPaymentComplete !=true).ToList();

                if (TblItemCart == null || TblItemCart.Count == 0)
                    return false;

                foreach (var cart in TblItemCart)
                {
                    cart.IsPaymentComplete = true;
                    cart.ModifiedDate = DateTime.UtcNow;
                    _ItemCartRepository.Update(cart);
                }
                _ItemCartRepository.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ItemManager", "UpdateCartPaymentStatus", ex);
                return false;
            }
        }
    }
}
