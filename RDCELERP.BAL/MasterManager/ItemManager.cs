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

namespace RDCELERP.BAL.MasterManager
{
    public class ItemManager : IItemManager
    {
        #region  Variable Declaration
        IItemRepository _itemRepository;
        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();
       
        IErrorLogManager _errorLogManager;
        IMapper _mapper;
        ILogging _logging;
        IOptions<ApplicationSettings> _config;
        #endregion
        #region Constructor
        public ItemManager(IItemRepository itemRepository, 
        IErrorLogManager errorLogManager,
        IMapper mapper, ILogging logging, IOptions<ApplicationSettings> config)
        {
            _itemRepository = itemRepository;
            _errorLogManager = errorLogManager;
            _mapper = mapper;
            _logging = logging;
            _config = config;

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

    }
}
