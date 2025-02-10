using RDCELERP.DAL.AbstractRepository;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;

namespace RDCELERP.DAL.Repository
{
    public class QuestionerLOVRepository : AbstractRepository<TblQuestionerLov>, IQuestionerLOVRepository
    {
        private readonly Digi2l_DevContext _context;
        public QuestionerLOVRepository(Digi2l_DevContext dbContext)
       : base(dbContext)
        {
            _context = dbContext;
        }

        #region Get Questioner LoV Rating Weightage
        /// <summary>
        /// Get Questioner LoV Rating Weightage
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TblQuestionerLov? GetRatingWeightage(int? id)
        {
            TblQuestionerLov? tblQuestionerLov = null;
            if (id > 0)
            {
                tblQuestionerLov = _context.TblQuestionerLovs
                    .Where(x => x.IsActive == true && x.QuestionerLovid == id)
                    .FirstOrDefault();
            }
            return tblQuestionerLov;
        }
        #endregion
        

        #region Get Questioner LoV List by Parent Id
        /// <summary>
        /// Get Questioner LoV List by Parent Id
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public List<TblQuestionerLov>? GetQuestionerLovListByPId(int? parentId)
        {
            List<TblQuestionerLov>? tblQuestionerLovList = null;
            if (parentId > 0)
            {
                tblQuestionerLovList = _context.TblQuestionerLovs
                    .Where(x => x.IsActive == true && x.QuestionerLovparentId == parentId)
                    .ToList();
            }
            return tblQuestionerLovList;
        }
        #endregion

        #region
        public List<TblQuestionerLov> GetQuestionerLovs(int? QuestionerLovid) /// yash rathod
        {
            List<TblQuestionerLov> tblQuestionerLOV = new List<TblQuestionerLov>();
            if (QuestionerLovid != null && QuestionerLovid > 0)
            {
                tblQuestionerLOV = _context.TblQuestionerLovs.Where(x => x.IsActive == true && x.QuestionerLovparentId == QuestionerLovid).ToList();
            }
            return tblQuestionerLOV;
        }
        #endregion
    }
}