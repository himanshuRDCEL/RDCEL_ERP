using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using RDCELERP.DAL.Entities;
using RDCELERP.BAL.Interface;
using RDCELERP.BAL.MasterManager;
using RDCELERP.DAL.IRepository;
using RDCELERP.DAL.Repository;
using RDCELERP.BAL.Common;
using RDCELERP.Common.Helper;
using RDCELERP.Model.Base;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;
using RDCELERP.BAL.Helper;
using Microsoft.AspNetCore.Authentication.Cookies;
using CorePush.Google;
using RDCELERP.DAL.Helper;


namespace RDCELERP.Core.App
{
    public class Startup
    {
        public Startup(Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
             .SetBasePath(env.ContentRootPath)
             .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ADOHelper>();
            services.AddControllers();
            //services.AddMvc();
            services.AddRazorPages();
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddSession();
            services.AddMemoryCache();
            services.AddMvc()
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            //.AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());
            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");

            //new changes
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(120);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });




            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.LoginPath = "/Index";
                options.LogoutPath = "/Index";
                options.Cookie.Name = "Remember";
                options.ExpireTimeSpan = TimeSpan.FromDays(30);
                options.SlidingExpiration = true;



            });



            ////Old 
            //services.AddSession();

            services.AddMemoryCache();

            // Add our Config object so it can be injected
            services.Configure<ApplicationSettings>(Configuration.GetSection("ApplicationSettings"));
            services.Configure<ConnectionStrings>(Configuration.GetSection("ConnectionStrings"));

            //Add DB context
            services.AddDbContext<Digi2l_DevContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("Digi2l_DevContext"), o => o.CommandTimeout(3600)), ServiceLifetime.Transient);

            //Added to allow to runtime change cshtml page 
            services.AddControllersWithViews(x => x.SuppressAsyncSuffixInActionNames = false).AddRazorRuntimeCompilation();
            services.AddSingleton<UniqueCode>();
            services.AddSingleton<CustomDataProtection>();
            services.AddSingleton<SecurityHelper>();

            #region For Automapper

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MapperManager());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            #endregion

            #region Adding scope for BAL and DAL

            // User, Role and Company management
            services.AddScoped<IUserManager, UserManager>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            services.AddScoped<ICompanyManager, CompanyManager>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IProductCategoryManager, ProductCategoryManager>();
            services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
            services.AddScoped<IProductTypeManager, ProductTypeManager>();
            services.AddScoped<IProductTypeRepository, ProductTypeRepository>();
            services.AddScoped<IProductQualityIndexManager, ProductQualityIndexManager>();
            services.AddScoped<IProductQualityIndexRepository, ProductQualityIndexRepository>();
            services.AddScoped<IPinCodeManager, PinCodeManager>();
            services.AddScoped<IPinCodeRepository, PinCodeRepository>();
            services.AddScoped<ICityManager, CityManager>();
            services.AddScoped<ICityRepository, CityRepository>();
            services.AddScoped<IStateManager, StateManager>();
            services.AddScoped<IStateRepository, StateRepository>();
            services.AddScoped<IStoreCodeManager, StoreCodeManager>();
            services.AddScoped<IStoreCodeRepository, StoreCodeRepository>();
            services.AddScoped<IProgramMasterManager, ProgramMasterManager>();
            services.AddScoped<IProgramMasterRepository, ProgramMasterRepository>();
            services.AddScoped<IBusinessPartnerManager, BusinessPartnerManager>();
            services.AddScoped<IBusinessPartnerRepository, BusinessPartnerRepository>();
            services.AddScoped<IErrorLogManager, ErrorLogManager>();
            services.AddScoped<IRoleManager, RoleManager>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IBrandManager, BrandManager>();
            services.AddScoped<IBrandRepository, BrandRepository>();
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<IRoleAccessRepository, RoleAccessRepository>();
            services.AddScoped<IAccessListRepository, AccessListRepository>();
            services.AddScoped<IAbbRegistrationManager, AbbRegistrationManager>();
            services.AddScoped<IAbbRegistrationRepository, AbbRegistrationRepository>();
            services.AddScoped<IABBRedemptionManager, ABBRedemptionManager>();
            services.AddScoped<IABBRedemptionRepository, ABBRedemptionRepository>();
            services.AddScoped<IListofValueRepository, ListofValueRepository>();
            services.AddScoped<IQCCommentManager, QCCommentManager>();
            services.AddScoped<IOrderQCRepository, OrderQCRepository>();
            services.AddScoped<IExchangeOrderStatusRepository, ExchangeOrderStatusRepository>();
            services.AddScoped<IBusinessUnitRepository, BusinessUnitRepository>();
            services.AddScoped<IBusinessUnitManager, BusinessUnitManager>();
            services.AddScoped<IPriceMasterRepository, PriceMasterRepository>();
            services.AddScoped<IPriceMasterManager, PriceMasterManager>();
            services.AddScoped<ILoginRepository, LoginRepository>();
            services.AddScoped<IVoucherRepository, VoucherRepository>();
            services.AddScoped<IEVCPriceMasterRepository, EVCPriceMasterRepository>();
            services.AddScoped<IOrderImageUploadRepository, OrderImageUploadRepository>();
            services.AddScoped<IVoucherStatusRepository, VoucherStatusRepository>();
            services.AddScoped<IModelNumberRepository, ModelNumberRepository>();
            services.AddScoped<IModelNumberManager, ModelNumberManager>();
            services.AddScoped<IHistoryRepository, HistoryRepository>();
            services.AddScoped<IBillCloudServiceCall, BillCloudServiceCall>();
            services.AddScoped<ITimeLineMappingStatusRepository, TimeLineStatusMappingRepository>();
            services.AddScoped<ITimeLineRepository, TimeLineRepository>();
            services.AddScoped<IDropdownManager, DropdownManager>();
            services.AddScoped<ITicketGenrateManager, TicketGenrateManager>();
            services.AddScoped<IBizlogTicketRepository, BizlogTicketRepository>();
            services.AddScoped<IDriverDetailsManager, DriverDetailsManager>();
            services.AddScoped<IPluralGatewayManager, PluralManager>();
            services.AddScoped<IManageUserForServicePartnerManager, ManageUserForServicePartnerManager>();
            services.AddScoped<ITechGuard, TechGuardManager>();
            services.AddScoped<IUserMappingRepository, UserMappingRepository>();

            //Vehicle Incentive Chart
            services.AddScoped<IVehicleIncentiveManager, VehicleIncentiveManager>();
            services.AddScoped<IVehicleIncentiveRepository, VehicleIncentiveRepository>();

            // Mail manager
            services.AddScoped<IMailManager, MailManager>();

            services.AddScoped<ICommonManager, CommonManager>();

            // Log and Error
            services.AddScoped<ILogging, Logging>();
            services.AddScoped<IErrorLogRepository, ErrorLogRepository>();

            //EVC
            services.AddScoped<IEVCManager, EVCManager>();
            services.AddScoped<IEVCRepository, EVCRepository>();
            //EVCWallet
            services.AddScoped<IEVCWalletAdditionRepository, EVCWalletAdditionRepository>();
            //Entity
            services.AddScoped<IEntityManager, EntityManager>();
            services.AddScoped<IEntityRepository, EntityRepository>();
            services.AddScoped<IWalletTransactionRepository, WalletTransactionRepository>();
            services.AddScoped<IExchangeOrderRepository, ExchangeOrderRepository>();
            services.AddScoped<IExchangeOrderManager, ExchangeOrderManager>();
            services.AddScoped<ICustomerDetailsRepository, CustomerDetailsRepository>();
            services.AddScoped<IEVCDisputeRepository, EVCDisputeRepository>();
            services.AddScoped<IEVCDisputeManager, EVCDisputeManager>();
            /*services.AddScoped<IQCManager, QCManager>();
            services.AddScoped<ISelfQCRepository, SelfQCRepository>();*/

            services.AddScoped<ILovRepository, LovRepository>();
            services.AddScoped<IOrderImageUploadRepository, OrderImageUploadRepository>();


            //LGC
            services.AddScoped<ILogisticManager, LogisticManager>();
            services.AddScoped<IServicePartnerRepository, ServicePartnerRepository>();
            services.AddScoped<ILogisticsRepository, LogisticsRepository>();
            services.AddScoped<IOrderLGCRepository, OrderLGCRepository>();
            //PoD
            services.AddScoped<IEVCPODDetailsRepository, EVCPODDetailsRepository>();
            //ImageLabelMaster
            services.AddScoped<IImageLabelRepository, ImageLabelRepository>();
            services.AddScoped<IImageLabelMasterManager, ImageLabelMasterManager>();

            services.AddScoped<IImageHelper, ImageHelper>();
            services.AddScoped<IHtmlToPDFConverterHelper, HtmlToPDFConverterHelper>();
            //QC
            services.AddScoped<IQCManager, QCManager>();
            services.AddScoped<ISelfQCRepository, SelfQCRepository>();
            //ABB Plan Master
            services.AddScoped<IABBPlanMasterRepository, ABBPlanMasterRepository>();
            services.AddScoped<IABBPlanMasterManager, ABBPlanMasterManager>();
            //ABB  Price Master
            services.AddScoped<IABBPriceMasterRepository, ABBPriceMasterRepository>();
            services.AddScoped<IABBPriceMasterManager, ABBPriceMasterManager>();
            services.AddScoped<IBUProductCategoryMapping, BUProductCategoryMapping>();


            //AbbRedemption
            services.AddScoped<IABBRedemptionRepository, ABBRedemptionRepository>();
            services.AddScoped<ILovRepository, LovRepository>();
            services.AddScoped<IExchangeABBStatusHistoryRepository, ExchangeABBStatusHistoryRepository>();
            services.AddScoped<IExchangeABBStatusHistoryManager, ExchangeABBStatusHistoryManager>();

            services.AddScoped<IOrderTransRepository, OrderTransRepository>();
            services.AddScoped<IOrderTransactionManager, OrderTransactionManager>();

            //MessageDetails
            services.AddScoped<INotificationManager, NotificationManager>();
            services.AddScoped<IMessageDetailRepository, MessageDetailRepository>();

            //ExchangeOrderStatus
            services.AddScoped<IExchangeOrderStatusRepository, ExchangeOrderStatusRepository>();
            services.AddScoped<IEVCWalletHistoryRepository, EVCWalletHistoryRepository>();
            services.AddScoped<IWhatsappNotificationManager, WhatsappNotificationManager>();
            services.AddScoped<IWhatsAppMessageRepository, WhatsAppMessageRepository>();
            services.AddScoped<IDriverDetailsRepository, DriverDetailsRepository>();

            //Service Partner
            services.AddScoped<IServicePartnerRepository, ServicePartnerRepository>();
            services.AddScoped<IServicePartnerManager, SevicePartnerManager>();
            //Dealer Dashboard 
            services.AddScoped<IDealerManager, DealerDashBoardManager>();
            services.AddScoped<ILogin_MobileRepository, Login_MobileRepository>();

            //Payout Repository
            services.AddScoped<ICashfreePayoutCall, CashFreeServiceCall>();
            services.AddScoped<IPaymentLeaser, PaymentLeaser>();
            services.AddScoped<ImapServicePartnerCityStateRepository, mapServicePartnerCityStateRepository>();
            services.AddScoped<IUPIIdVerification, UPIIdVerification>();

            services.AddScoped<IOrderQCRatingRepository, OrderQCRatingRepository>();

            //ALL Template configuration
            services.AddScoped<ITemplateConfigurationRepository, TemplateConfigurationRepository>();
            services.AddScoped<IDropdownManager, DropdownManager>();
            services.AddScoped<IEmailTemplateManager, EmailTemplateManager>();

            //ABB registrationManger added for DashBoard
            services.AddScoped<IAbbRegistrationManager, AbbRegistrationManager>();
            services.AddScoped<IABBSponsorManager, ABBSponsorManager>();

            //Brand SmartBuy Repository and Brand Repository
            services.AddScoped<IBrandSmartBuyRepository, BrandSmartBuyRepository>();
            services.AddScoped<IOrderQCManager, OrderQCManager>();

            //use for Driver journy[ update by priyanshi]
            services.AddScoped<IVehicleJourneyTrackingRepository, VehicleJourneyTrackingRepository>();
            services.AddScoped<IVehicleJourneyTrackingDetailsRepository, VehicleJourneyTrackingDetailsRepository>();
            services.AddScoped<IBrandSmartBuyRepository, BrandSmartBuyRepository>();

            //use for price master mapping
            services.AddScoped<IPriceMasterNameRepository, PriceMasterNameRepository>();
            services.AddScoped<IPriceMasterNameManager, PriceMasterNameManager>();
            services.AddScoped<IPriceMasterMappingRepository, PriceMasterMappingRepository>();
            services.AddScoped<IPriceMasterMappingManager, PriceMasterMappingManager>();


            services.AddHttpClient<FcmSender>();
            services.AddHttpClient<CorePush.Apple.ApnSender>();

            //use for ValidationBasedSweetner[ update by priyanshi]
            services.AddScoped<IBUBasedSweetnerValidationRepository, BUBasedSweetnerValidationRepository>();
            services.AddScoped<IQuestionsForSweetnerRepository, QuestionsForSweetnerRepository>();

            //AreaLocality Added by PJ
            services.AddScoped<IAreaLocalityRepository, AreaLocalityRepository>();
            services.AddScoped<IProductConditionLabelRepository, ProductConditionLabelRepository>();           
            services.AddScoped<ISweetenerManager, SweetenerManager>();           
            services.AddScoped<IModelMappingRepository, ModelMappingRepository>();           
            services.AddScoped<IOrderBasedConfigRepository, OrderBasedConfigRepository>();           

            //MapLoginUserDeviceRepository Added by Kranti Silawat
            services.AddScoped<IMapLoginUserDeviceRepository, LoginUserDeviceRepository>();
            services.AddScoped<IPushNotificationManager, PushNotificationManager>();


            //ProductTechnologyManagerRepository Added by Abhishek Sharma
            services.AddScoped<IProductTechnologyManager, ProductTechnologyManager > ();
            services.AddScoped<IProductTechnologyRepository, ProductTechnologyRepository>();
            services.AddScoped<IUniversalPriceMasterManager, UniversalPriceMasterManager>();
            services.AddScoped<IUniversalPriceMasterRepository, UniversalPriceMasterRepository>();

            services.AddScoped<IPushNotificationMessageDetailRepository, PushNotificationMessageDetailRepository>();
            services.AddScoped<IPushNotificationSavedDetailsRepository, PushNotificationSavedDetailsRepository>();
            //EVC Auto Allocation phase 2 priyanshi
            services.AddScoped<IEVCPartnerRepository, EVCPartnerRepository>();
            services.AddScoped<IEvcPartnerPreferenceRepository, EvcPartnerPreferenceRepository>();

            // Customer files for ABB
            services.AddScoped<ICustomerFilesRepository, CustomerFilesRepository>();

            //ProductConditionLabelRepository Added by Kranti Silawat
            services.AddScoped<IProductConditionLabelRepository, ProductConditionLabelRepository>();
            services.AddScoped<IProductConditionLabelManager, ProductConditionLabelManager>();
            services.AddScoped<IVoucherRedemptionManager, VoucherRedemptionManager>();

            services.AddScoped<ITermsAndConditionsForVoucherRepository, TermsAndConditionsForVoucherRepository>();
            // Daikin
            services.AddScoped<IDaikinManager, DaikinManager>();

            // Added by VK
            services.AddScoped<IEVCPriceRangeMasterRepository, EVCPriceRangeMasterRepository>();
//<<<<<<< Dev
            //Refurbisher
            services.AddScoped<IRefurbisherRepository, RefurbisherRepository>();
            services.AddScoped<IRefurbisherManager, RefurbisherManager>();

//=======
            //Diagnose V2 added by VK
            services.AddScoped<IProdCatBrandMappingRepository, ProdCatBrandMappingRepository>();
            services.AddScoped<IPriceMasterQuestionersRepository, PriceMasterQuestionersRepository>();
            services.AddScoped<IBrandGroupRepository, BrandGroupRepository>();
            services.AddScoped<IQuestionerLOVRepository, QuestionerLOVRepository>();
            services.AddScoped<IQCRatingMasterRepository, QCRatingMasterRepository>();
            services.AddScoped<IQcratingMasterMappingRepository, QcratingMasterMappingRepository>();
            services.AddScoped<IQuestionerLovmappingRepository, QuestionerLovmappingRepository>();
            //>>>>>>> DiagnoseV2_API_10-Jan

            //CreditRequestRepository added by priyanshi
            services.AddScoped<ICreditRequestRepository, CreditRequestRepository>();
            services.AddScoped<IDriverListRepository, DriverListRepository>();
            services.AddScoped<IVehicleListRepository, VehicleListRepository>();

            //>>>>>>> DiagnoseV2_API_10-Jan

            //Self QC Temp Data
            services.AddScoped<ITempDataRepository, TempDataRepository>();
            services.AddScoped< IApiCallsRepository, ApiCallsRepository>();

            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
                app.UseStatusCodePagesWithRedirects("/Error?statusCode={0}");
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });
        }
    }
}

