using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Program;

namespace RDCELERP.BAL.Interface
{
    public interface IProgramMasterManager
    {
        /// <summary>
        /// Method to manage (Add/Edit) ProgramMaster
        /// <param name="ProductTypeVM">ProgramMasterVM</param>
        /// <param name="userId">userId</param>
        /// <returns>int</returns>
        public int ManageProgramMaster(ProgramMasterViewModel ProgramMasterVM, int userId);
        /// <summary>
        /// Method to get the ProgramMaster by id 
        /// </summary>
        /// <param name="id">ProgramMasterId</param>
        /// <returns>ProgramMasterViewModel</returns>
        public Model.Program.ProgramMasterViewModel GetProgramMasterById(int id);
        /// <summary>
        /// Method to delete ProgramMaster by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        public bool DeletProgramMasterById(int id);

        /// <summary>
        /// Method to get the All ProgramMaster
        /// </summary>     
        /// <returns>ProgramMasterViewModel</returns>
        public IList<ProgramMasterViewModel> GetAllProgramMaster();
    }
}
