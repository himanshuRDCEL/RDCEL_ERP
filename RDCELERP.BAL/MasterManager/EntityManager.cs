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
using RDCELERP.Model;
using RDCELERP.Model.State;

namespace RDCELERP.BAL.MasterManager
{
    public class EntityManager : IEntityManager
    {
        #region  Variable Declaration
       
        private readonly IMapper _mapper;
        ILogging _logging;
        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();
        IUserRoleRepository _userRoleRepository;
        IEntityRepository _entityRepository;
        #endregion

        public EntityManager( IEntityRepository EntityRepository, IUserRoleRepository userRoleRepository, IMapper mapper, ILogging logging)
        {

            _entityRepository = EntityRepository;
            _userRoleRepository = userRoleRepository;
            _mapper = mapper;
            _logging = logging;
        }
        public IList<EntityViewModel> GetAllEntity()
        {
            IList<EntityViewModel> EntityVMList = null;
            List<TblEntityType> TblEntitytype = new List<TblEntityType>();
            // TblUseRole TblUseRole = null;
            try
            {

                TblEntitytype = _entityRepository.GetList(x => x.IsActive == true).ToList();

                if (TblEntitytype != null && TblEntitytype.Count > 0)
                {
                    EntityVMList = _mapper.Map<IList<TblEntityType>, IList<EntityViewModel>>(TblEntitytype);
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("EntityManager", "GetAllEntuty", ex);
            }
            return EntityVMList;
            throw new NotImplementedException();
        }
    }
}
