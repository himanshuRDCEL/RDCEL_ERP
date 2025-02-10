using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.DAL.AbstractRepository;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;

namespace RDCELERP.DAL.Repository
{
    public class AbbRegistrationRepository : AbstractRepository<TblAbbregistration>, IAbbRegistrationRepository
    {
        Digi2l_DevContext _context;
    
        public AbbRegistrationRepository(Digi2l_DevContext dbContext)
         : base(dbContext)
        {
            _context = dbContext;            
        }

        /// <summary>
        /// Method to get the ABB Registration order details by id 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public TblAbbregistration GetSingleOrder(int Id)
        {
            TblAbbregistration TblAbbregistration = _context.TblAbbregistrations
                        .Include(x => x.BusinessPartner).ThenInclude(x => x.BusinessUnit).ThenInclude(X => X.Login)
                        .Include(x => x.NewProductCategoryTypeNavigation)
                        .Include(x => x.NewProductCategory).FirstOrDefault(x => x.IsActive == true && x.AbbregistrationId == Id);

            return TblAbbregistration;
        }

        public TblAbbregistration GetRegdNo(string regdno)
        {
            TblAbbregistration TblAbbregistration = _context.TblAbbregistrations.FirstOrDefault(x => x.IsActive == true && x.RegdNo == regdno);

            return TblAbbregistration;
        }

        public TblAbbregistration GetAllRegdno()
        {
            TblAbbregistration TblAbbregistration = _context.TblAbbregistrations.FirstOrDefault(x => x.IsActive == true);

            return TblAbbregistration;
        }

        public List<TblAbbregistration> GetOrderCountForDashboard(int BusinessUnitId)
        {
            List<TblAbbregistration> tblAbbregistratioObj = new List<TblAbbregistration>();
            if (BusinessUnitId > 0)
            {
                tblAbbregistratioObj = _context.TblAbbregistrations.Where(x=>x.IsActive==true && x.BusinessUnitId!=null && x.BusinessUnitId==BusinessUnitId).ToList();
            }
            else 
            {
                tblAbbregistratioObj = _context.TblAbbregistrations.Where(x => x.IsActive == true && x.BusinessUnitId != null).ToList();
            }
            return tblAbbregistratioObj;
        }

        public List<TblAbbregistration> GetNotApprovedOrders(int BusinessUnitId)
        {
            List<TblAbbregistration> tblAbbregistratioObj = new List<TblAbbregistration>();
            if (BusinessUnitId > 0)
            {
                tblAbbregistratioObj = _context.TblAbbregistrations.Where(x => x.IsActive == true && x.BusinessUnitId != null && x.BusinessUnitId == BusinessUnitId && x.AbbApprove!=null &&  x.AbbApprove==false).ToList();
            }
            else  
            {
                tblAbbregistratioObj = _context.TblAbbregistrations.Where(x => x.IsActive == true && x.BusinessUnitId != null && x.AbbApprove != null && x.AbbApprove == false).ToList();
            }
            return tblAbbregistratioObj;
        }

        public List<TblAbbregistration> GetApprovedOrders(int BusinessUnitId)
        {
            List<TblAbbregistration> tblAbbregistratioObj = new List<TblAbbregistration>();
            if (BusinessUnitId > 0)
            {
                tblAbbregistratioObj = _context.TblAbbregistrations.Where(x => x.IsActive == true && x.BusinessUnitId != null && x.BusinessUnitId == BusinessUnitId && x.AbbApprove != null && x.AbbApprove == true).ToList();
            }
            else  
            {
                tblAbbregistratioObj = _context.TblAbbregistrations.Where(x => x.IsActive == true && x.BusinessUnitId != null && x.AbbApprove != null && x.AbbApprove == true).ToList();
            }
            return tblAbbregistratioObj;
        }

        public List<TblAbbregistration> GetAllOrderList(int? BusinessUnitId,string regdNo,string SponsorOrderNumber,string PhoneNumber,string EmployeeId,string Abbstorecode)
        {
            List<TblAbbregistration> tblAbbregistrations = new List<TblAbbregistration>();
            if (BusinessUnitId > 0)
            {
                tblAbbregistrations = _context.TblAbbregistrations.Include(x => x.BusinessUnit)
                    .Include(x => x.NewProductCategory)
                    .Include(x => x.NewProductCategoryTypeNavigation)
                    .Where(x => x.BusinessUnitId != null && x.BusinessUnitId == BusinessUnitId && x.IsActive != null && x.IsActive == true && (string.IsNullOrEmpty(regdNo) || x.RegdNo == regdNo) && (string.IsNullOrEmpty(SponsorOrderNumber) || x.SponsorOrderNo == SponsorOrderNumber) && (string.IsNullOrEmpty(PhoneNumber) || x.CustMobile == PhoneNumber) && (string.IsNullOrEmpty(EmployeeId) || x.EmployeeId == EmployeeId) && (string.IsNullOrEmpty(Abbstorecode) || x.StoreCode == Abbstorecode) && x.NewProductCategoryTypeId != null && x.NewProductCategoryId != null).OrderByDescending(x => x.AbbregistrationId).ToList();
            }
            else
            {
                tblAbbregistrations = _context.TblAbbregistrations.Include(x=>x.BusinessUnit)
                     .Include(x => x.NewProductCategory)
                    .Include(x => x.NewProductCategoryTypeNavigation)
                    .Where(x => x.BusinessUnitId != null  && x.IsActive != null && x.IsActive == true && (string.IsNullOrEmpty(regdNo) || x.RegdNo == regdNo) && (string.IsNullOrEmpty(SponsorOrderNumber) || x.SponsorOrderNo == SponsorOrderNumber) && (string.IsNullOrEmpty(PhoneNumber) || x.CustMobile == PhoneNumber) && (string.IsNullOrEmpty(EmployeeId) || x.EmployeeId == EmployeeId) && (string.IsNullOrEmpty(Abbstorecode) || x.StoreCode == Abbstorecode) && x.NewProductCategoryTypeId != null && x.NewProductCategoryId != null).OrderByDescending(x => x.AbbregistrationId).ToList();
            }
            return tblAbbregistrations;
        }
        public TblBusinessPartner GetStoreEmail(string email)
        {
            TblBusinessPartner TblBusinessPartner = _context.TblBusinessPartners.FirstOrDefault(x => x.IsActive == true && x.Email.Contains(email));
            
            return TblBusinessPartner;
        }
    }
}
