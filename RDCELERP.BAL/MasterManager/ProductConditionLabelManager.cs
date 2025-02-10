using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Helper;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.Model.Base;
using RDCELERP.Model.InfoMessage;
using RDCELERP.Model.Master;
using RDCELERP.Model.ProductConditionLabel;

namespace RDCELERP.BAL.MasterManager
{
    public class ProductConditionLabelManager : IProductConditionLabelManager
    {
        #region  Variable Declaration
        IProductConditionLabelRepository _ProductConditionLabelRepository;
        private readonly IMapper _mapper;
        ILogging _logging;
        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();
        Digi2l_DevContext _context;
       
        public readonly IOptions<ApplicationSettings> _baseConfig;

        #endregion

        public ProductConditionLabelManager(IProductConditionLabelRepository productConditionLabelRepository, IMapper mapper, ILogging logging, IOptions<ApplicationSettings> baseConfig, Digi2l_DevContext context)
        {
            _ProductConditionLabelRepository = productConditionLabelRepository;
            _mapper = mapper;
            _logging = logging;
            _baseConfig = baseConfig;
            _context = context;
        }

        #region Method for Add/Edit ProductContionLabel (Added by Kranti)
        /// <summary>
        /// Method to manage (Add/Edit) ProductConditionLabel 
        /// </summary>
        /// <param name="ProductConditionLabelVM">ProductConditionLabelVM</param>
        /// <param name="userId">userId</param>
        /// <returns>int</returns>
        public int ManageProductConditionLabel(ProductConditionLabelViewModel ProductConditionLabelVM, int userId)
        {
            TblProductConditionLabel TblProductConditionLabel = new TblProductConditionLabel();

            try
            {
                if (ProductConditionLabelVM != null)
                {
                    TblProductConditionLabel = _mapper.Map<ProductConditionLabelViewModel, TblProductConditionLabel>(ProductConditionLabelVM);

                    TblProductConditionLabel = TrimHelper.TrimAllValuesInModel(TblProductConditionLabel);

                    bool? IsExceedLimit = false;
                    // if bpid getting as null then fetch by PartnerName...

                    int BusinessPartnerId = TblProductConditionLabel.BusinessPartnerId ?? 0;
                    var Labels = _ProductConditionLabelRepository.GetProductConditionByBUBP(BusinessPartnerId, TblProductConditionLabel.BusinessUnitId);
                    if(Labels != null && Labels.Count >= 4)
                    {
                        IsExceedLimit = true;
                    }

                    if (IsExceedLimit != null && IsExceedLimit != false)
                    {
                        if (TblProductConditionLabel.Id > 0)
                        { 
                            TblProductConditionLabel.ModifiedBy = userId;
                            TblProductConditionLabel.Modifieddate = _currentDatetime;
                            _ProductConditionLabelRepository.updateProductConditionLabel(TblProductConditionLabel);
                        }
                        else
                        {
                            return 1;
                        }
                    }
                    else
                    {
                        var allExistingLabels = _ProductConditionLabelRepository.GetProductConditionLabelByBusinessPartnerId(TblProductConditionLabel.BusinessPartnerId);
                        var allActiveLabels = allExistingLabels?.Where(x=>x.IsActive == true)?.Select(x=>x.OrderSequence);
                        if (allActiveLabels == null || allActiveLabels != null && allActiveLabels.Count() > 0 && !allActiveLabels.Contains(TblProductConditionLabel.OrderSequence))
                        {
                            TblProductConditionLabel.IsActive = true;
                            TblProductConditionLabel.CreatedDate = _currentDatetime;
                            TblProductConditionLabel.CreatedBy = userId;
                            _ProductConditionLabelRepository.Create(TblProductConditionLabel);

                            if (allExistingLabels != null)
                            {
                                var LabeltoDelete = allExistingLabels.Where(x => x.OrderSequence == TblProductConditionLabel.OrderSequence && x.IsActive == false).ToList();
                                if (LabeltoDelete != null && LabeltoDelete.Count > 0)
                                {
                                    foreach (var x in LabeltoDelete)
                                    {
                                        _ProductConditionLabelRepository.Delete(x);
                                    }
                                }
                            }
                        }
                    }
                    _ProductConditionLabelRepository.SaveChanges();                   
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ProductCategoryManager", "ManageProductCategory", ex);
            }

            return TblProductConditionLabel.Id;
        }

        #endregion

        #region Method for Get ProductConditionLabel by Id (Added by Kranti)
        /// <summary>
        /// Method to get the ProductConditionLabel by id 
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>ProductConditionLabelViewModel</returns>
        public ProductConditionLabelViewModel GetProductConditionLabelById(int id)
        {
            ProductConditionLabelViewModel ProductConditionLabelVM = null;
            TblProductConditionLabel TblProductConditionLabel = null;

            try
            {
                TblProductConditionLabel = _ProductConditionLabelRepository.GetSingle(where: x => x.Id == id);
                if (TblProductConditionLabel != null)
                {
                    ProductConditionLabelVM = _mapper.Map<TblProductConditionLabel, ProductConditionLabelViewModel>(TblProductConditionLabel);
                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ProductConditionLabelManager", "GetProductConditionLabelById", ex);
            }
            return ProductConditionLabelVM;
        }

        #endregion

        #region Method for Delete ProductCondionLabel by id (Added by Kranti)
        /// <summary>
        /// Method to delete ProductConditionLabel by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        public bool DeletProductConditionLabelById(int id)
        {
            bool flag = false;
            try
            {
                TblProductConditionLabel TblProductConditionLabel = _ProductConditionLabelRepository.GetSingle(x => x.IsActive == true && x.Id == id);
                if (TblProductConditionLabel != null)
                {
                    TblProductConditionLabel.IsActive = false;
                    _ProductConditionLabelRepository.Update(TblProductConditionLabel);
                    _ProductConditionLabelRepository.SaveChanges();
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ProductConditionLabelManager", "DeletProductConditionLabelById", ex);
            }
            return flag;
        }

        #endregion

        #region Method for Get all ProductConditionLabel list (Added by Kranti)
        /// <summary>
        /// Method to get the All ProductConditionLabel
        /// </summary>     
        /// <returns>ProductConditionLabelViewModel</returns>
        public IList<ProductConditionLabelViewModel> GetAllProductConditionLabel()
        {
            IList<ProductConditionLabelViewModel> ProductConditionLabelVMList = null;
            List<TblProductConditionLabel> TblProductConditionLabellist = new List<TblProductConditionLabel>();
            // TblUseRole TblUseRole = null;
            try
            {

                TblProductConditionLabellist = _ProductConditionLabelRepository.GetList(x => x.IsActive == true).ToList();

                if (TblProductConditionLabellist != null && TblProductConditionLabellist.Count > 0)
                {
                    ProductConditionLabelVMList = _mapper.Map<IList<TblProductConditionLabel>, IList<ProductConditionLabelViewModel>>(TblProductConditionLabellist);
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ProductConditionLabelManager", "GetAllProductConditionLabel", ex);
            }
            return ProductConditionLabelVMList;
        }
        #endregion



    }
}
