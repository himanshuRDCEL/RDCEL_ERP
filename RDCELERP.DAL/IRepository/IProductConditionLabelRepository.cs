using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.DAL.AbstractRepository;
using RDCELERP.DAL.Entities;

namespace RDCELERP.DAL.IRepository
{
    public interface IProductConditionLabelRepository : IAbstractRepository<TblProductConditionLabel>
    {
        public List<TblProductConditionLabel> GetProductConditionByBUBP(int? BUId, int? BPId, int? proCatId = null);
        public TblProductConditionLabel GetOrderSequenceNo(int? PClableId);
        bool InsertBulkReords(List<TblProductConditionLabel> tblProductConditionLabels);
        Task UpdateBulkReords(List<TblProductConditionLabel> tblProductConditionLabels);
        List<TblProductConditionLabel> GetProductConditionLabelByBusinessPartnerId(int? BusinessPartnerId);
        void DeleteProductConditionLabelsForBP(int BusinessPartnerId);
        public int updateProductConditionLabel(TblProductConditionLabel TblProductConditionLabel);
    }
}
