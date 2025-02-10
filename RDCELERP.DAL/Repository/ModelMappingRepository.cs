using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.DAL.AbstractRepository;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;

namespace RDCELERP.DAL.Repository
{
    public class ModelMappingRepository : AbstractRepository<TblModelMapping>, IModelMappingRepository
    {
        Digi2l_DevContext _context;
        public ModelMappingRepository(Digi2l_DevContext dbContext)
       : base(dbContext)
        {
            _context = dbContext;
        }

        public TblModelMapping GetbyModelnoid(int? Modelnoid, int? BUid, int? BPid)
        {
            TblModelMapping? TblModelMapping = null;
            if(Modelnoid > 0 && BUid > 0 && BPid > 0)
            {
                //(x => x.IsActive == true && x.IsDefault == true && x.ModelId == modelnumberObj.ModelNumberId && x.BusinessUnitId == details.BusinessUnitId && x.BusinessPartnerId == details.BusinessPartnerId);
                TblModelMapping = _context.TblModelMappings.FirstOrDefault(x => x.IsActive == true && x.IsDefault == false && x.ModelId == Modelnoid && x.BusinessUnitId == BUid && x.BusinessPartnerId == BPid);
            }

            return TblModelMapping;
        }

        public TblModelMapping GetdefaultModelnoid(int? Modelnoid, int? BUid, int? BPid)
        {
            TblModelMapping? TblModelMapping = null;
            if (Modelnoid > 0 && BUid > 0 && BPid > 0)
            {
                //(x => x.IsActive == true && x.IsDefault == true && x.ModelId == modelnumberObj.ModelNumberId && x.BusinessUnitId == details.BusinessUnitId && x.BusinessPartnerId == details.BusinessPartnerId);
                TblModelMapping = _context.TblModelMappings.FirstOrDefault(x => x.IsActive == true && x.IsDefault == true && x.ModelId == Modelnoid && x.BusinessUnitId == BUid && x.BusinessPartnerId == BPid);
            }

            return TblModelMapping;
        }

        public bool InsertModelsForBusinessPartner(List<TblModelMapping> modelMappings)
        {
            var result = false;
            if (modelMappings != null && modelMappings.Count > 0)
            {
                try
                {
                    _context.AddRange(modelMappings);
                    _context.SaveChanges();
                    result = true;
                }
                catch (Exception) 
                {
                    result = false;
                }

            }
            return result;
        }            
    }
}
    
    
    

