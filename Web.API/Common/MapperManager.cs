using AutoMapper;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.Company;
using RDCELERP.Model.Users;
using RDCELERP.Model.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Master;
using RDCELERP.Model.Product;
using RDCELERP.Model.PriceMaster;
using RDCELERP.Model.ProductQuality;
using RDCELERP.Model.PinCode;
using RDCELERP.Model.City;
using RDCELERP.Model.State;
using RDCELERP.Model.StoreCode;
using RDCELERP.Model.Program;
using RDCELERP.Model.BusinessPartner;
using RDCELERP.Model.EVC;
using RDCELERP.Model;
using RDCELERP.Model.AbbRegistration;

namespace UTC.API.Common
{
    public class MapperManager : Profile
    {
        public MapperManager()
        {
            //user, role and company
            CreateMap<TblUser, UserViewModel>().ReverseMap();
            CreateMap<TblUserRole, UserRoleViewModel>().ReverseMap();
            CreateMap<TblCompany, CompanyViewModel>().ReverseMap();
            CreateMap<TblAddress, AddressListViewModel>().ReverseMap();
            CreateMap<TblRole, RoleViewModel>().ReverseMap();
            CreateMap<TblRoleAccess, RoleAccessViewModel>().ReverseMap();
            CreateMap<TblAccessList, AccessListViewModel>().ReverseMap();
            CreateMap<TblAccessList, RoleAccessViewModel>().ReverseMap();
            CreateMap<TblBrand, BrandViewModel>().ReverseMap();   
            CreateMap<TblProductCategory, ProductCategoryViewModel>().ReverseMap();
            CreateMap<TblProductType, ProductTypeViewModel>().ReverseMap();
            CreateMap<TblPriceMaster, PriceMasterViewModel>().ReverseMap();
            CreateMap<TblProductQualityIndex, ProductQualityIndexViewModel>().ReverseMap();
            CreateMap<TblPinCode, PinCodeViewModel>().ReverseMap();
            CreateMap<TblCity, CityViewModel>().ReverseMap();
            CreateMap<TblState, StateViewModel>().ReverseMap();
            CreateMap<TblStoreCode, StoreCodeViewModel>().ReverseMap();
            CreateMap<TblProgramMaster, ProgramMasterViewModel>().ReverseMap();
            CreateMap<TblBusinessPartner, BusinessPartnerViewModel>().ReverseMap();
            CreateMap<TblEntityType, EntityViewModel>().ReverseMap();
            CreateMap<TblEvcregistration, EVC_RegistrationModel>().ReverseMap();
            CreateMap<TblEvcregistration, EVC_NotApprovedViewModel>().ReverseMap();
            CreateMap<TblEvcregistration, EVC_ApprovedViewModel>().ReverseMap();
            CreateMap<TblEvcwalletAddition, EVCWalletAdditionViewModel>().ReverseMap();
            CreateMap<TblAbbregistration, AbbRegistrationModel>().ReverseMap();
            CreateMap<TblUser, UserRoleLoginViewModel>().ReverseMap();
            CreateMap<TblEvcwalletAddition, UserRoleLoginViewModel>().ReverseMap();




        }
    }
}
