using RDCELERP.BAL.Interface;
using RDCELERP.Common.Helper;
using RDCELERP.DAL.IRepository;
using RDCELERP.Model.Users;
using System;
using RDCELERP.DAL.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using RDCELERP.Common.Enums;
using RDCELERP.Common.Constant;
using RDCELERP.DAL.Helper;
using RDCELERP.Model.Master;
using RDCELERP.DAL.Entities;

namespace RDCELERP.BAL.MasterManager
{
    public class ExchangeABBStatusHistoryManager : IExchangeABBStatusHistoryManager
    {
        #region  Variable Declaration
        IExchangeABBStatusHistoryRepository _exchangeABBStatusHistoryRepository;
        IRoleAccessRepository _roleAccessRepository;
        private readonly IMapper _mapper;
        ILogging _logging;
        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();
        #endregion

        public ExchangeABBStatusHistoryManager(IExchangeABBStatusHistoryRepository exchangeABBStatusHistoryRepository, IMapper mapper, ILogging logging)
        {
            _exchangeABBStatusHistoryRepository = exchangeABBStatusHistoryRepository;
            _mapper = mapper;
            _logging = logging;
        }

        /// <summary>
        /// Method to manage (Add/Edit) ExchangeABBStatusHistory 
        /// </summary>
        /// <param name="ExchangeABBStatusHistoryVM">ExchangeABBStatusHistoryVM</param>
        /// <param name="userId">userId</param>
        public int ManageExchangeABBStatusHistory(ExchangeABBStatusHistoryViewModel ExchangeABBStatusHistoryVM, int userId = 3)
        {
            TblExchangeAbbstatusHistory TblExchangeABBStatusHistory = new TblExchangeAbbstatusHistory();
            int StatusHistoryId = 0;
            try
            {
                if (ExchangeABBStatusHistoryVM != null)
                {
                    TblExchangeABBStatusHistory = _mapper.Map<ExchangeABBStatusHistoryViewModel, TblExchangeAbbstatusHistory>(ExchangeABBStatusHistoryVM);

                    //Code to Insert the object 
                    TblExchangeABBStatusHistory.IsActive = true;
                    TblExchangeABBStatusHistory.CreatedDate = _currentDatetime;
                    TblExchangeABBStatusHistory.CreatedBy = userId;
                    _exchangeABBStatusHistoryRepository.Create(TblExchangeABBStatusHistory);
                    _exchangeABBStatusHistoryRepository.SaveChanges();
                    StatusHistoryId = TblExchangeABBStatusHistory.StatusHistoryId;
                    return StatusHistoryId;
                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ExchangeABBStatusHistoryManager", "ManageExchangeABBStatusHistory", ex);
            }
            return StatusHistoryId;
        }


    }
}
