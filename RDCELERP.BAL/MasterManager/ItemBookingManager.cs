using AutoMapper;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Constant;
using RDCELERP.Common.Helper;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.Model.BusinessCustomer;
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
        IMapper _mapper;
        ILogging _logging;
        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();

        public ItemBookingManager(IMapper mapper,
        ILogging logging, IItemRepository itemRepository, IBookingItemRepository bookingItemRepository ) 
        {
            _ItemRepository= itemRepository;
            _BookingItemRepository= bookingItemRepository;
            _mapper = mapper;
            _logging = logging;
        }


        /// <summary>
        /// method to add booking item
        /// </summary>
        /// <param name="ItemlistVM"></param>
        /// <returns></returns>
        public async Task<bool> AddBookingItem(List<ItemViewModel> itemlistVM,int userid)
        {
            bool flag = false;
            try
            {
                if (itemlistVM != null && itemlistVM.Count>0)
                {
                    foreach (ItemViewModel model in itemlistVM)
                    {
                        TblBookingItem TblBookingItem = new TblBookingItem();

                        TblBookingItem.ItemId= model.ItemId;
                        TblBookingItem.CustomerId = userid;
                        TblBookingItem.Quantity = model.Quantity;
                        TblBookingItem.TotalPrice = model.Quantity * model.Mrp;

                  //Code to Insert the object
                        TblBookingItem.IsActive = true;
                        //TblBookingItem.CreatedBy = userid;
                        TblBookingItem.CreatedDate = _currentDatetime;
                        _BookingItemRepository.Create(TblBookingItem);
                    }
                    _BookingItemRepository.SaveChanges();
                    flag = true;
                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ItemBookingManager", "ManageBookingItem", ex);
            }

            return flag;
        }
    }
}
