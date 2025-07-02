using AutoMapper;
using RDCELERP.BAL.Interface;
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

namespace RDCELERP.BAL.MasterManager
{
    public class ItemMasterManager : IItemMasterManager
    {
        IItemMasterRepository _itemMasterRepository;
        IItemRepository _itemRepository;
        IMapper _mapper;
        ILogging _logging;
        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();
        ISynchronizedManager _syncManager;

        public ItemMasterManager(IMapper mapper,
        ILogging logging, IItemRepository itemRepository, IItemMasterRepository itemMasterRepository, ISynchronizedManager syncManager)
        {
            _itemMasterRepository = itemMasterRepository;
            _itemRepository = itemRepository;
            _mapper = mapper;
            _logging = logging;
            _syncManager = syncManager;
        }


        /// <summary>
        /// method to manager item
        /// </summary>
        /// <param name="itemcartVM"></param>
        /// <returns></returns>
        public int ManageMasterItem(ItemMasterViewModel itemcartVM, int userid)
        {
            int result = 0;
            TblItemMaster TblItemMaster = null;
            try
            {
                if (itemcartVM != null)
                {
                    TblItemMaster = _mapper.Map<ItemMasterViewModel, TblItemMaster>(itemcartVM);

                    if (TblItemMaster.ItemMasterId > 0)
                    {
                        TblItemMaster.CreatedBy = userid;
                        TblItemMaster.CreatedDate = _currentDatetime;
                        _itemMasterRepository.Create(TblItemMaster);
                    }
                    else
                    {
                        TblItemMaster.ModifiedBy = userid;
                        TblItemMaster.ModifiedDate = _currentDatetime;
                        _itemMasterRepository.Update(TblItemMaster);

                    }
                }
                _itemMasterRepository.SaveChanges();
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ItemMasterManager", "ManageItemCart", ex);
            }

            return result;
        }

        /// <summary>
        /// method to get master item details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ItemMasterViewModel GetMasterItemById(int id)
        {
            ItemMasterViewModel itemMasterVM = null;
            TblItemMaster TblItemMaster = null;

            try
            {

                TblItemMaster = _itemMasterRepository.GetSingle(x => x.IsActive == true && x.ItemMasterId == id);

                if (TblItemMaster != null)
                {
                    itemMasterVM = _mapper.Map<TblItemMaster, ItemMasterViewModel>(TblItemMaster);

                   List<TblItem> tblItem=_itemRepository.GetList(x=>x.IsActive==true && x.ItemMasterId == id).ToList();

                    if (tblItem != null)
                    {

                        itemMasterVM.ItemViewModel=_mapper.Map<List<TblItem>,List<ItemViewModel>>(tblItem);
                    }
                    else
                    {
                        itemMasterVM.ItemViewModel = new List<ItemViewModel>();

                    }

                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ItemMasterManager", "GetUserById", ex);
            }
            return itemMasterVM;
        }

    }
}
