using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.BAL.Helper;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Enums;
using RDCELERP.Common.Helper;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.Model.ImagLabel;
using RDCELERP.Model.Master;

namespace RDCELERP.BAL.MasterManager
{
    public class ImageLabelMasterManager : IImageLabelMasterManager
    {
        #region  Variable Declaration
        IImageLabelRepository _ImageLabelRepository;
        private readonly IMapper _mapper;
        ILogging _logging;
        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();
        IUserRoleRepository _userRoleRepository;
        IImageHelper _imageHelper;
        IProductCategoryRepository _productCategoryRepository;
        IProductTypeRepository _productTypeRepository;
        #endregion

        public ImageLabelMasterManager(IImageLabelRepository ImageLabelRepository, IProductTypeRepository productTypeRepository, IUserRoleRepository userRoleRepository, IMapper mapper, ILogging logging, IImageHelper imageHelper, IProductCategoryRepository productCategoryRepository)
        {
            _ImageLabelRepository = ImageLabelRepository;
            _userRoleRepository = userRoleRepository;
            _mapper = mapper;
            _logging = logging;
            _imageHelper = imageHelper;
            _productCategoryRepository = productCategoryRepository;
            _productTypeRepository = productTypeRepository; 
        }

        /// <summary>
        /// Method to manage (Add/Edit) ImageLabel
        /// </summary>
        /// <param name="ImageLabelVM">ImageLabelVM</param>
        /// <param name="userId">userId</param>
        /// <returns>int</returns>
        public int ManageImageLabel(ImageLabelNewViewModel ImageLabelVM, int userId)
        {
            TblImageLabelMaster TblImageLabelMaster = new TblImageLabelMaster();

            try
            {
                if (ImageLabelVM != null)
                {
                    TblImageLabelMaster = _mapper.Map<ImageLabelNewViewModel, TblImageLabelMaster>(ImageLabelVM);

                    if (TblImageLabelMaster.ImageLabelid > 0)
                    {
                        //Code to update the object
                        if (ImageLabelVM.Base64StringValue != null)
                        {
                            var FileName = ImageLabelVM.ProductName + "_" + "Pattern_" + ImageLabelVM.Pattern + ".jpg";
                            string filePath = EnumHelper.DescriptionAttr(FilePathEnum.ImageLabelMaster);
                            var imgSave = _imageHelper.SaveFileFromBase64(ImageLabelVM.Base64StringValue, filePath, FileName);
                            if (imgSave)
                            {
                                TblImageLabelMaster.ImagePlaceHolder = FileName;
                            }
                        }
                        TblImageLabelMaster.ProductName = ImageLabelVM.ProductCategoryName;
                        TblImageLabelMaster.Modifiedby = userId;
                        TblImageLabelMaster.ModifiedDate = _currentDatetime;
                        TblImageLabelMaster = TrimHelper.TrimAllValuesInModel(TblImageLabelMaster);
                        _ImageLabelRepository.Update(TblImageLabelMaster);
                    }
                    else
                    {

                        if (ImageLabelVM.Base64StringValue != null)
                        {
                            var FileName = ImageLabelVM.ProductName + "_" + "Pattern_" + ImageLabelVM.Pattern + ".jpg";
                            string filePath = EnumHelper.DescriptionAttr(FilePathEnum.ImageLabelMaster);
                            var imgSave = _imageHelper.SaveFileFromBase64(ImageLabelVM.Base64StringValue, filePath, FileName);
                            if (imgSave)
                            {
                                TblImageLabelMaster.ImagePlaceHolder = FileName;
                            }
 
                        }
                        //Code to Insert the object 
                        TblImageLabelMaster.ProductName = ImageLabelVM.ProductCategoryName;
                        TblImageLabelMaster.IsActive = true;
                        TblImageLabelMaster.CreatedDate = _currentDatetime;
                        TblImageLabelMaster.CreatedBy = userId;
                        TblImageLabelMaster = TrimHelper.TrimAllValuesInModel(TblImageLabelMaster);
                        _ImageLabelRepository.Create(TblImageLabelMaster);
                    }
                    _ImageLabelRepository.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ImageLabelManager", "ManageImageLabel", ex);
            }

            return TblImageLabelMaster.ImageLabelid;
        }

        #region
        //Method for Bulk Upload
        public ImageLabelNewViewModel ManageImageLabelBulk(ImageLabelNewViewModel ImageLabelVM, int userId)
        {

            TblProductCategory TblProductCategory = new TblProductCategory();


            if (ImageLabelVM != null && ImageLabelVM.ImageLabelVMList != null && ImageLabelVM.ImageLabelVMList.Count > 0)
            {
                var ValidatedImageLabelList = ImageLabelVM.ImageLabelVMList.Where(x => x.Remarks == null || string.IsNullOrEmpty(x.Remarks)).ToList();
                ImageLabelVM.ImageLabelVMErrorList = ImageLabelVM.ImageLabelVMList.Where(x => x.Remarks != null && !string.IsNullOrEmpty(x.Remarks)).ToList();


                if (ValidatedImageLabelList != null && ValidatedImageLabelList.Count > 0)
                {
                    foreach (var item in ValidatedImageLabelList)
                    {
                        try
                        {

                            TblProductCategory = _productCategoryRepository.GetSingle(where: x => x.Description == item.ProductCategory);
                            TblProductType tblProductType = _productTypeRepository.GetSingle(where: x => x.Description + x.Size == item.ProductType);

                            if (item.ImageLabelid > 0)
                            {

                                TblImageLabelMaster TblImageLabelMaster = new TblImageLabelMaster();
                                //Code to update the object 
                                TblImageLabelMaster.ProductName = item.ProductCategory;
                                TblImageLabelMaster.ProductImageDescription = item.ProductImageDescription;
                                TblImageLabelMaster.ProductImageLabel = item.ProductImageLabel;
                                TblImageLabelMaster.Pattern = item.Pattern;
                                if(TblProductCategory != null)
                                {
                                    TblImageLabelMaster.ProductCatId = TblProductCategory.Id;
                                }
                                if(tblProductType != null)
                                {
                                    TblImageLabelMaster.ProductTypeId = tblProductType.Id;
                                }
                                TblImageLabelMaster.ModifiedDate = _currentDatetime;
                                TblImageLabelMaster.Modifiedby = userId;
                                _ImageLabelRepository.Update(TblImageLabelMaster);
                                _ImageLabelRepository.SaveChanges();

                            }
                            else
                            {


                                TblImageLabelMaster TblImageLabelMaster = new TblImageLabelMaster();
                                //Code to update the object 
                                TblImageLabelMaster.ProductName = item.ProductCategory;
                                TblImageLabelMaster.ProductImageDescription = item.ProductImageDescription;
                                TblImageLabelMaster.ProductImageLabel = item.ProductImageLabel;
                                TblImageLabelMaster.Pattern = item.Pattern;
                                if (TblProductCategory != null)
                                {
                                    TblImageLabelMaster.ProductCatId = TblProductCategory.Id;
                                }
                                if (tblProductType != null)
                                {
                                    TblImageLabelMaster.ProductTypeId = tblProductType.Id;
                                }
                                TblImageLabelMaster.IsActive = true;
                                TblImageLabelMaster.CreatedDate = _currentDatetime;
                                TblImageLabelMaster.CreatedBy = userId;
                                _ImageLabelRepository.Create(TblImageLabelMaster);
                                _ImageLabelRepository.SaveChanges();

                            }
                        }

                        catch (Exception ex)
                        {
                            item.Remarks += ex.Message + ", ";
                            ImageLabelVM.ImageLabelVMList.Add(item);
                        }
                    }
                }
            }

            return ImageLabelVM;
        }

        #endregion

        /// <summary>
        /// Method to get the ImageLabel by id 
        /// </summary>
        /// <param name="id">ImageLabelId</param>
        /// <returns>ImageLabelViewModel</returns>
        public ImageLabelNewViewModel GetImageLabelById(int id)
        {
            ImageLabelNewViewModel ImageLabelVM = null;
            TblImageLabelMaster TblImageLabelMaster = null;

            try
            {
                TblImageLabelMaster = _ImageLabelRepository.GetSingle(where: x => x.IsActive == true && x.ImageLabelid == id);
                if (TblImageLabelMaster != null)
                {
                    ImageLabelVM = _mapper.Map<TblImageLabelMaster, ImageLabelNewViewModel>(TblImageLabelMaster);
                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ImageLabelManager", "GetImageLabelById", ex);
            }
            return ImageLabelVM;
        }

        /// <summary>
        /// Method to delete ImageLabel by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        public bool DeletImageLabelById(int id)
        {
            bool flag = false;
            try
            {
                TblImageLabelMaster TblImageLabelMaster = _ImageLabelRepository.GetSingle(x => x.IsActive == true && x.ImageLabelid == id);
                if (TblImageLabelMaster != null)
                {
                    TblImageLabelMaster.IsActive = false;
                    _ImageLabelRepository.Update(TblImageLabelMaster);
                    _ImageLabelRepository.SaveChanges();
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ImageLabelManager", "DeletImageLabelById", ex);
            }
            return flag;
        }

        /// <summary>
        /// Method to get the All ImageLabel
        /// </summary>     
        /// <returns>ImageLabelViewModel</returns>
        public IList<ImageLabelViewModel> GetAllImageLabel()
        {
            IList<ImageLabelViewModel> ImageLabelVMList = null;
            List<TblImageLabelMaster> TblImageLabelMasterlist = new List<TblImageLabelMaster>();
            // TblUseRole TblUseRole = null;
            try
            {

                TblImageLabelMasterlist = _ImageLabelRepository.GetList(x => x.IsActive == true).ToList();

                if (TblImageLabelMasterlist != null && TblImageLabelMasterlist.Count > 0)
                {
                    ImageLabelVMList = _mapper.Map<IList<TblImageLabelMaster>, IList<ImageLabelViewModel>>(TblImageLabelMasterlist);
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ProductCategoryManager", "GetAllProductCategory", ex);
            }
            return ImageLabelVMList;
        }
    }
}
