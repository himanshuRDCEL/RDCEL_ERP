using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Helper;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.Model.InfoMessage;
using RDCELERP.Model.MobileApplicationModel;
using RDCELERP.Model.State;

namespace RDCELERP.BAL.MasterManager
{
    public class StateManager : IStateManager
    {
        #region  Variable Declaration
        IStateRepository _StateRepository;
        private readonly IMapper _mapper;
        ILogging _logging;
        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();
        IUserRoleRepository _userRoleRepository;

        


        #endregion

        public StateManager(IStateRepository StateRepository, IUserRoleRepository userRoleRepository, IMapper mapper, ILogging logging)
        {

            _StateRepository = StateRepository;
            _userRoleRepository = userRoleRepository;
            _mapper = mapper;
            _logging = logging;
        }

        /// <summary>
        /// Method to manage (Add/Edit) State 
        /// </summary>
        /// <param name="StateVM">StateVM</param>
        /// <param name="userId">userId</param>
        /// <returns>int</returns>
        public int ManageState(StateViewModel StateVM, int userId)
        {
            TblState TblState = new TblState();
            
            try
            {
                if (StateVM != null)
                {
                    TblState = _mapper.Map<StateViewModel, TblState>(StateVM);
                    TblState.Name = TblState.Name.Trim();

                    if (TblState.StateId > 0)
                    {
                        //Code to update the object                      
                        TblState.ModifiedBy = userId;
                        TblState.ModifiedDate = _currentDatetime;
                        _StateRepository.Update(TblState);
                    }
                    else
                    {
                        var Check = _StateRepository.GetSingle(x => x.Name == StateVM.Name);
                        if (Check == null)
                        {
                            //Code to Insert the object 
                            TblState.IsActive = true;
                            TblState.CreatedDate = _currentDatetime;
                            TblState.CreatedBy = userId;
                            _StateRepository.Create(TblState);
                        }                        
                    }
                    _StateRepository.SaveChanges();
                    
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("StateManager", "ManageState", ex);
            }

            return TblState.StateId;       
        }

        public StateViewModel ManageStateBulk(StateViewModel StateVM, int userId)
        {
            List<TblState> tblState = new List<TblState>();

            if (StateVM != null && StateVM.StateVMList != null && StateVM.StateVMList.Count > 0)
            {
                var ValidatedStateList = StateVM.StateVMList.Where(x => x.Remarks == null || string.IsNullOrEmpty(x.Remarks)).ToList();
                StateVM.StateVMErrorList = StateVM.StateVMList.Where(x => x.Remarks != null && !string.IsNullOrEmpty(x.Remarks)).ToList();

                if (ValidatedStateList != null && ValidatedStateList.Count > 0)
                {
                    foreach (var item in ValidatedStateList)
                    {
                        try
                        {
                            TblState TblStates = new TblState();
                            if (item.StateId > 0)
                            {
                                TblStates.Name = item.Name;
                                TblStates.Code = item.Code;
                                TblStates.IsActive = item.IsActive;
                                TblStates.ModifiedDate = _currentDatetime;
                                TblStates.ModifiedBy = userId;
                                _StateRepository.Update(TblStates);
                                _StateRepository.SaveChanges();
                            }
                            else
                            {
                                TblStates.Name = item.Name;
                                TblStates.Code = item.Code;
                                TblStates.IsActive = item.IsActive;
                                TblStates.CreatedDate = _currentDatetime;
                                TblStates.CreatedBy = userId;
                                _StateRepository.Create(TblStates);
                                _StateRepository.SaveChanges(); 
                            }
                        }
                        catch (Exception ex)
                        {
                            item.Remarks += ex.Message + ", ";
                            StateVM.StateVMErrorList.Add(item);
                        }
                    }
                }
            }    
            return StateVM;
        }
        /// <summary>
        /// Method to get the State by id 
        /// </summary>
        /// <param name="id">StateId</param>
        /// <returns>StateViewModel</returns>
        public StateViewModel GetStateById(int id)
        {
            StateViewModel StateVM = null;
            TblState TblState = null;

            try
            {
                TblState = _StateRepository.GetSingle(where: x => x.StateId == id);
                if (TblState != null)
                {
                    StateVM = _mapper.Map<TblState, StateViewModel>(TblState);
                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("StateManager", "GetStateById", ex);
            }
            return StateVM;
        }

        /// <summary>
        /// Method to delete State by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        public bool DeleteStateById(int id)
        {
            bool flag = false;
            try
            {
                TblState TblState = _StateRepository.GetSingle(x => x.IsActive == true && x.StateId == id);
                if (TblState != null)
                {
                    TblState.IsActive = false;
                    _StateRepository.Update(TblState);
                    _StateRepository.SaveChanges();
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("StateManager", "DeleteStateById", ex);
            }
            return flag;
        }

        /// <summary>
        /// Method to get the All State
        /// </summary>     
        /// <returns>List  StateViewModel</returns>

        public IList<StateViewModel> GetAllState()
        {

            IList<StateViewModel> StateVMList = null;
            List<TblState> TblStatelist = new List<TblState>();
            // TblUseRole TblUseRole = null;
            try
            {

                TblStatelist = _StateRepository.GetList(x => x.IsActive == true).ToList();

                if (TblStatelist != null && TblStatelist.Count > 0)
                {
                    StateVMList = _mapper.Map<IList<TblState>, IList<StateViewModel>>(TblStatelist);
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("StateManager", "GetAllState", ex);
            }
            return StateVMList;
        }
        /// <summary>
        /// GetState - Added by Ashwin
        /// Used for Api
        /// </summary>
        /// <returns>responseResult</returns>
        public ResponseResult GetState()
        {
            ResponseResult responseResult = new ResponseResult();
            //IList<StateViewModel> StateVMList = null;
            List<TblState> TblStatelist = new List<TblState>();
            IList<StateName> stateNames = null;

            // TblUseRole TblUseRole = null;
            try
            {

                TblStatelist = _StateRepository.GetList(x => x.IsActive == true).ToList();

                if (TblStatelist != null && TblStatelist.Count > 0)
                {
                    stateNames = _mapper.Map<IList<TblState>, IList<StateName>>(TblStatelist);
                    if (stateNames != null)
                    {
                        responseResult.Data = stateNames;
                        responseResult.Status_Code = HttpStatusCode.OK;
                        responseResult.message = "Success";
                        responseResult.Status = true;
                    }
                    else
                    {
                        //responseResult.Data = StateVMList;
                        responseResult.Status_Code = HttpStatusCode.BadRequest;
                        responseResult.message = "Not Success , error while mapping the object";
                        responseResult.Status = false;
                    }
                    return responseResult;
                }
                else
                {
                    responseResult.Status_Code = HttpStatusCode.BadRequest;
                    responseResult.message = "Not Success , error while getting list";
                    responseResult.Status = false;
                    return responseResult;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("StateManager", "GetState", ex);
                responseResult.Status_Code = HttpStatusCode.InternalServerError;
                responseResult.message = ex.ToString();
                responseResult.Status = false;
            }
            return  responseResult;
        }

        
        public ExecutionResult StateById(int id)
        {
            StateViewModel StateVM = null;
            TblState TblState = null;

            try
            {
                TblState = _StateRepository.GetSingle(where: x => x.IsActive == true && x.StateId == id);
                if (TblState != null)
                {
                    StateVM = _mapper.Map<TblState, StateViewModel>(TblState);
                    return new ExecutionResult(new InfoMessage(true, "Success", StateVM));

                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("StateManager", "StateById", ex);
            }
            return new ExecutionResult(new InfoMessage(true, "No data found"));
        }

    }
}
