using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Helper;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.Model.Program;

namespace RDCELERP.BAL.MasterManager
{
    public class ProgramMasterManager : IProgramMasterManager
    {
        #region  Variable Declaration
        IProgramMasterRepository _ProgramMasterRepository;
        private readonly IMapper _mapper;
        ILogging _logging;
        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();
        IUserRoleRepository _userRoleRepository;

        #endregion

        public ProgramMasterManager(IProgramMasterRepository ProgramMasterRepository, IUserRoleRepository userRoleRepository, IMapper mapper, ILogging logging)
        {
            _ProgramMasterRepository = ProgramMasterRepository;
            _userRoleRepository = userRoleRepository;
            _mapper = mapper;
            _logging = logging;
        }
        /// <summary>
        /// Method to manage (Add/Edit) ProgramMaster
        /// <param name="ProductTypeVM">ProgramMasterVM</param>
        /// <param name="userId">userId</param>
        /// <returns>int</returns>
        public int ManageProgramMaster(ProgramMasterViewModel ProgramMasterVM, int userId)
        {
            TblProgramMaster TblProgramMaster = new TblProgramMaster();

            try
            {
                if (ProgramMasterVM != null)
                {
                    TblProgramMaster = _mapper.Map<ProgramMasterViewModel, TblProgramMaster>(ProgramMasterVM);


                    if (TblProgramMaster.ProgramMasterId > 0)
                    {
                        //Code to update the object                      
                        TblProgramMaster.ModifiedBy = userId;
                        TblProgramMaster.ModifiedDate = _currentDatetime;
                        _ProgramMasterRepository.Update(TblProgramMaster);
                    }
                    else
                    {
                        var Check = _ProgramMasterRepository.GetSingle(x => x.LoginCredentials == ProgramMasterVM.LoginCredentials);
                        if (Check == null)
                        {
                            //Code to Insert the object 
                            TblProgramMaster.IsActive = true;
                            TblProgramMaster.CreatedDate = _currentDatetime;
                            TblProgramMaster.CreatedBy = userId;
                            _ProgramMasterRepository.Create(TblProgramMaster);
                        }
                           
                    }
                    _ProgramMasterRepository.SaveChanges();


                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ProgramMasterManager", "ManageProgramMaster", ex);
            }

            return TblProgramMaster.ProgramMasterId;
        }
        /// <summary>
        /// Method to get the ProgramMaster by id 
        /// </summary>
        /// <param name="id">ProgramMasterId</param>
        /// <returns>ProgramMasterViewModel</returns>
        public Model.Program.ProgramMasterViewModel GetProgramMasterById(int id)
        {
            Model.Program.ProgramMasterViewModel ProgramMasterVM = null;
            TblProgramMaster TblProgramMaster = null;

            try
            {

                TblProgramMaster = _ProgramMasterRepository.GetSingle(where: x => x.IsActive == true && x.ProgramMasterId == id);
                if (TblProgramMaster != null)
                {
                    ProgramMasterVM = _mapper.Map<TblProgramMaster, ProgramMasterViewModel>(TblProgramMaster);
                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ProgramMasterManager", "GetProgramMasterById", ex);
            }
            return ProgramMasterVM;
        }
        /// <summary>
        /// Method to delete ProgramMaster by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        public bool DeletProgramMasterById(int id)
        {
            bool flag = false;
            try
            {
                TblProgramMaster TblProgramMaster = _ProgramMasterRepository.GetSingle(x => x.IsActive == true && x.ProgramMasterId == id);
                if (TblProgramMaster != null)
                {
                    TblProgramMaster.IsActive = false;
                    _ProgramMasterRepository.Update(TblProgramMaster);
                    _ProgramMasterRepository.SaveChanges();
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ProgramMasterManager", "DeletProgramMasterById", ex);
            }
            return flag;
        }

        /// <summary>
        /// Method to get the All ProgramMaster
        /// </summary>     
        /// <returns>ProgramMasterViewModel</returns>
        public IList<ProgramMasterViewModel> GetAllProgramMaster()
        {
            IList<ProgramMasterViewModel> ProgramMasterVMList = null;
            List<TblProgramMaster> TblProgramMasterlist = new List<TblProgramMaster>();
            // TblUseRole TblUseRole = null;
            try
            {

                TblProgramMasterlist = _ProgramMasterRepository.GetList(x => x.IsActive == true).ToList();

                if (TblProgramMasterlist != null && TblProgramMasterlist.Count > 0)
                {
                    ProgramMasterVMList = _mapper.Map<IList<TblProgramMaster>, IList<ProgramMasterViewModel>>(TblProgramMasterlist);
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ProgramMasterManager", "GetAllProgramMaster", ex);
            }
            return ProgramMasterVMList;
        }
    }
}
