using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Helper;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.DAL.Repository;
using RDCELERP.Model.Base;
using RDCELERP.Model.BusinessCustomer;
using RDCELERP.Model.PriceMaster;
using static AutoMapper.Internal.CollectionMapperExpressionFactory;
using Microsoft.EntityFrameworkCore;

namespace RDCELERP.BAL.MasterManager
{
    public class ItemManager : IItemManager
    {
        #region  Variable Declaration
        IItemRepository _itemRepository;
        IItemMasterRepository   _itemMasterRepository;
        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();
       ISynchronizedManager _synchronizedManager;
        IErrorLogManager _errorLogManager;
        IMapper _mapper;
        ILogging _logging;
        IOptions<ApplicationSettings> _config;
        #endregion
        #region Constructor
        public ItemManager(IItemRepository itemRepository, 
        IErrorLogManager errorLogManager,
        IMapper mapper, ILogging logging, IOptions<ApplicationSettings> config,ISynchronizedManager synchronizedManager,IItemMasterRepository itemMasterRepository)
        {
            _itemRepository = itemRepository;
            _errorLogManager = errorLogManager;
            _mapper = mapper;
            _logging = logging;
            _config = config;
            _synchronizedManager = synchronizedManager;
            _itemMasterRepository = itemMasterRepository;

        }
        #endregion

        /// <summary>
        /// Method to get Item list
        /// </summary>
        /// <returns>List<ItemViewModel></returns>
        public List<ItemViewModel> GetItemList()
        {
            List<ItemViewModel>ItemListVM = null;
           List<TblItem> TblItem = null;

            try
            {
                TblItem = _itemRepository.GetList(where: x => x.IsActive == true).ToList();
                if (TblItem != null)
                {
                    ItemListVM = _mapper.Map<List<TblItem>, List<ItemViewModel>>(TblItem);
                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ItemManager", "GetItemList", ex);
            }
            return ItemListVM;
        }
        /// <summary>
        /// method to get item by item code
        /// </summary>
        /// <param name="itemcode"></param>
        /// <returns></returns>
        public ItemViewModel GetItemByItemCode(string itemcode)
        {
            ItemViewModel ItemListVM = null;
            TblItem TblItem = null;

            try
            {
                TblItem = _itemRepository.GetSingle(where: x => x.IsActive == true && x.Itemcode==itemcode);
                if (TblItem != null)
                {
                    ItemListVM = _mapper.Map<TblItem, ItemViewModel>(TblItem);
                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ItemManager", "GetItemByItemCode", ex);
            }
            return ItemListVM;
        }


        public async Task<bool> UpsertItemsAsync(IEnumerable<ItemViewModel> items)
        {
            bool flag = false;
            try
            {
                foreach (var item in items)
                {
                    var existing = _itemRepository.GetSingle(x => x.Itemcode == item.Itemcode && x.IsActive == true);

                    if (existing != null)
                    {
                        // Update existing item
                        existing.ItemDesc = item.ItemDesc;
                        existing.Ean = item.Ean;
                        existing.Qty = item.Qty;
                        existing.ModifiedDate = _currentDatetime;
                    }
                    else
                    {
                        TblItem TblItem = _mapper.Map<ItemViewModel, TblItem>(item);

                        if (TblItem != null)
                        {
                            TblItem.IsActive = true;
                            TblItem.CreatedDate = _currentDatetime;
                            await _itemRepository.CreateAsync(TblItem);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return flag;
        }

        /// <summary>
        /// method to sync item stock and master item
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public async Task SyncStockReportDataAsync(int userid)
        {
            var stockItems = await _synchronizedManager.GetStockDataAsync(string.Empty,string.Empty);

            _logging.WriteErrorToDB("count", stockItems.Count.ToString(), null);
            //var first10Items = stockItems.Take(10).ToList();
            var first10Items = stockItems.Skip(10).ToList();
            try {
                if (first10Items != null && first10Items.Count > 0)

                // if (stockItems != null &&  stockItems.Count < 10)
                {
                    int masterItemId=0;
                    // Updated loop with proper EF tracking prevention
                    foreach (var stockItem in first10Items)
                    {
                        string itemCode = stockItem.Itemcode;

                        // Check if item exists in MasterItem
                        var existingItem = _itemMasterRepository.Query().AsNoTracking().FirstOrDefault(x => x.Itemcode.ToLower() == itemCode.ToLower());

                        TblItemMaster newItemMaster = null;
                        if (existingItem == null)
                        {
                            var itemDetail = await _synchronizedManager.GetItemDetailsByItemCodeFromApiAsync(null, null, itemCode);
                            if (itemDetail == null)
                                continue;

                            newItemMaster = new TblItemMaster
                            {
                                Sno = itemDetail.SNo,
                                LocationCode = itemDetail.LocationCode?.ToString(),
                                Itemcode = itemDetail.Itemcode,
                                Ean = itemDetail.EAN,
                                ItemDesc = itemDetail.ItemDesc,
                                Mrp = itemDetail.MRP,
                                Qty = itemDetail.Qty,
                                PendingDispatch = itemDetail.PendingDispatch,
                                AvailableStock = itemDetail.AvailableStock,
                                Brand = itemDetail.Brand,
                                SubCat = itemDetail.SubCat,
                                Size = itemDetail.Size,
                                Colour = itemDetail.Colour,
                                Condition = itemDetail.Condition,
                                Gender = itemDetail.Gender,
                                Division = itemDetail.Division,
                                Section = itemDetail.Section,
                                Department = itemDetail.Department,
                                CostPrice = (decimal?)itemDetail.CostPrice,
                                Rsp = (decimal?)itemDetail.RSP,
                                ManageBatchItem = itemDetail.ManageBatchItem,
                                ExpiryDate = string.IsNullOrEmpty(itemDetail.ExpiryDate) ? null : DateTime.Parse(itemDetail.ExpiryDate),
                                Uom = itemDetail.UOM,
                                Hsn = itemDetail.HSN,
                                Hsnid = itemDetail.HSNID,
                                IsActive = true,
                                CreatedBy = userid,
                                CreatedDate = _currentDatetime
                            };

                            await _itemMasterRepository.CreateAsync(newItemMaster);
                            _itemMasterRepository.SaveChanges();
                            masterItemId = newItemMaster.ItemMasterId;
                        }
                        else
                        {
                            masterItemId = existingItem.ItemMasterId;
                        }

                        _logging.WriteErrorToDB("new stock", masterItemId.ToString(), null);

                        if (masterItemId > 0)
                        {
                            var existingStock = _itemRepository.Query().FirstOrDefault(x => x.ItemMasterId == masterItemId && x.Itemcode == stockItem.Itemcode);

                            if (existingStock != null)
                            {
                                existingStock.Sno = stockItem.SNo;
                                existingStock.LocationCode = stockItem.LocationCode;
                                existingStock.Itemcode = stockItem.Itemcode;
                                existingStock.Ean = stockItem.EAN;
                                existingStock.ItemDesc = stockItem.ItemDesc;
                                existingStock.Mrp = (decimal?)stockItem.MRP;
                                existingStock.Qty = stockItem.Qty.ToString();
                                existingStock.PendingDispatch = stockItem.PendingDispatch.ToString();
                                existingStock.AvailableStock = stockItem.AvailableStock.ToString();
                                existingStock.Brand = stockItem.Brand;
                                existingStock.SubCat = stockItem.SubCat;
                                existingStock.Size = stockItem.Size;
                                existingStock.Colour = stockItem.Colour;
                                existingStock.Condition = stockItem.Condition;
                                existingStock.Gender = stockItem.Gender;
                                existingStock.Division = stockItem.Division;
                                existingStock.Section = stockItem.Section;
                                existingStock.Department = stockItem.Department;
                                existingStock.CostPrice = (decimal?)stockItem.CostPrice;
                                existingStock.Rsp = stockItem.RSP.ToString();
                                existingStock.ManageBatchItem = stockItem.ManageBatchItem;
                                existingStock.ExpiryDate = string.IsNullOrWhiteSpace(stockItem.ExpiryDate) ? null : DateTime.Parse(stockItem.ExpiryDate);
                                existingStock.Uom = stockItem.UOM;
                                existingStock.Hsn = stockItem.HSN;
                                existingStock.Hsnid = stockItem.HSNID;
                                existingStock.ModifiedBy = userid;
                                existingStock.ModifiedDate = _currentDatetime;
                                existingStock.IsActive = true;

                                await _itemRepository.UpdateAsync(existingStock.ItemId, existingStock);
                            }
                            else
                            {
                                var newStockItem = new TblItem
                                {
                                    Sno = stockItem.SNo,
                                    LocationCode = stockItem.LocationCode,
                                    Itemcode = stockItem.Itemcode,
                                    Ean = stockItem.EAN,
                                    ItemDesc = stockItem.ItemDesc,
                                    Mrp = (decimal?)stockItem.MRP,
                                    Qty = stockItem.Qty.ToString(),
                                    PendingDispatch = stockItem.PendingDispatch.ToString(),
                                    AvailableStock = stockItem.AvailableStock.ToString(),
                                    Brand = stockItem.Brand,
                                    SubCat = stockItem.SubCat,
                                    Size = stockItem.Size,
                                    Colour = stockItem.Colour,
                                    Condition = stockItem.Condition,
                                    Gender = stockItem.Gender,
                                    Division = stockItem.Division,
                                    Section = stockItem.Section,
                                    Department = stockItem.Department,
                                    CostPrice = (decimal?)stockItem.CostPrice,
                                    Rsp = stockItem.RSP.ToString(),
                                    ManageBatchItem = stockItem.ManageBatchItem,
                                    ExpiryDate = string.IsNullOrWhiteSpace(stockItem.ExpiryDate) ? null : DateTime.Parse(stockItem.ExpiryDate),
                                    Uom = stockItem.UOM,
                                    Hsn = stockItem.HSN,
                                    Hsnid = stockItem.HSNID,
                                    CreatedBy = userid,
                                    CreatedDate = _currentDatetime,
                                    IsActive = true,
                                    ItemMasterId = masterItemId
                                };

                                await _itemRepository.CreateAsync(newStockItem);
                            }
                            _itemRepository.SaveChanges();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("SyncStockReportDataAsync", "ItemManager", ex);
              
            }
        }

    }


}
