using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.InfoMessage;
using RDCELERP.Model.MobileApplicationModel;
using RDCELERP.Model.State;

namespace RDCELERP.BAL.Interface
{
    public interface IStateManager
    {
          
        /// <summary>
        /// Method to manage (Add/Edit) State 
        /// </summary>
        /// <param name="StateVM">StateVM</param>
        /// <param name="userId">userId</param>
        /// <returns>int</returns>
        int ManageState(StateViewModel StateVM, int userId);

        /// <summary>
        /// Method to get the State by id 
        /// </summary>
        /// <param name="id">StateId</param>
        /// <returns>StateViewModel</returns>
        StateViewModel GetStateById(int id);

        /// <summary>
        /// Method to delete State by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        bool DeleteStateById(int id);

        /// <summary>
        /// Method to get the All State
        /// </summary>     
        /// <returns>List  StateViewModel</returns>

        IList<StateViewModel> GetAllState();

        public ResponseResult GetState();


        public ExecutionResult StateById(int id);
        public StateViewModel ManageStateBulk(StateViewModel StateVM, int userId);

    }
}

