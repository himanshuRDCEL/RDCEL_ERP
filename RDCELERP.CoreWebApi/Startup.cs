using AutoMapper;
using CorePush.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.BAL.Common;
using RDCELERP.BAL.Helper;
using RDCELERP.BAL.Interface;
using RDCELERP.BAL.MasterManager;
using RDCELERP.Common.Helper;
using RDCELERP.CoreWebApi.Configuration;
using RDCELERP.CoreWebApi.Data;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.DAL.Repository;
using RDCELERP.Model.Base;

namespace RDCELERP.CoreWebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddCors();
            #region Enable Cors Added by VK for React Projects
            services.AddCors(options =>
            {
                options.AddPolicy("EnableCorsPolicy", builder => builder
                           //.WithOrigins("http://localhost:3000")
                           .AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader());
            });
            #endregion

            services.AddControllers();
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(15);
            });
            // Add our Config object so it can be injected
            services.Configure<ApplicationSettings>(Configuration.GetSection("ApplicationSettings"));
            services.Configure<ConnectionStrings>(Configuration.GetSection("ConnectionStrings"));

            services.AddDbContext<Digi2l_DevContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("Digi2l_DevContext")));

            services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<Digi2l_DevContext>();

            // configure strongly typed settings objects
            var jwtSection = Configuration.GetSection("JwtBearerTokenSettings");
            services.Configure<JwtBearerTokenSettings>(jwtSection);
            var jwtBearerTokenSettings = jwtSection.Get<JwtBearerTokenSettings>();
            var key = Encoding.ASCII.GetBytes(jwtBearerTokenSettings.SecretKey);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = jwtBearerTokenSettings.Issuer,
                    ValidAudience = jwtBearerTokenSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                };
            });
           
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "RDCELERP.CoreWebApi", Version = "v1" });
            });
           
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
            services.AddScoped<IProductTechnologyRepository,ProductTechnologyRepository>();
            services.AddScoped<IProductTechnologyManager, ProductTechnologyManager>();
            services.AddScoped<IDriverDetailsManager, DriverDetailsManager>();
            services.AddScoped<IPluralGatewayManager,PluralManager>();




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
            #endregion

            services.AddSingleton<UniqueCode>();
            services.AddSingleton<CustomDataProtection>();
            services.AddSingleton<SecurityHelper>();          
            services.AddTransient<IUserRepository, UserRepository>();           
            // Mail manager
            services.AddScoped<IMailManager, MailManager>();
            services.AddScoped<ICommonManager, CommonManager>();
            // Log and Error
            services.AddScoped<ILogging, Logging>();
            services.AddScoped<IErrorLogRepository, ErrorLogRepository>();

            services.AddScoped<ILogin_MobileRepository, Login_MobileRepository>();

            services.AddScoped<ImapServicePartnerCityStateRepository, mapServicePartnerCityStateRepository>();

            services.AddControllers().AddNewtonsoftJson();

            //Payout Repository
            services.AddScoped<ICashfreePayoutCall, CashFreeServiceCall>();
            services.AddScoped<IPaymentLeaser, PaymentLeaser>();
            services.AddScoped<ImapServicePartnerCityStateRepository, mapServicePartnerCityStateRepository>();
            services.AddScoped<IUPIIdVerification, UPIIdVerification>();
            services.AddScoped<IOrderQCRatingRepository, OrderQCRatingRepository>();
            services.AddScoped<IManageUserForServicePartnerManager, ManageUserForServicePartnerManager>();


            //ALL Template configuration
            services.AddScoped<ITemplateConfigurationRepository, TemplateConfigurationRepository>();


            services.AddScoped<ITicketGenrateManager, TicketGenrateManager>();
            services.AddScoped<IBizlogTicketRepository, BizlogTicketRepository>();
            services.AddScoped<IDriverDetailsManager, DriverDetailsManager>();
            services.AddScoped<IPluralGatewayManager, PluralManager>();

            //use for Driver journy
            services.AddScoped<IVehicleJourneyTrackingRepository, VehicleJourneyTrackingRepository>();
            services.AddScoped<IVehicleJourneyTrackingDetailsRepository, VehicleJourneyTrackingDetailsRepository>();
            services.AddScoped<IBrandSmartBuyRepository, BrandSmartBuyRepository>();
            services.AddScoped<IVehicleIncentiveRepository, VehicleIncentiveRepository>();
            //use for ValidationBasedSweetner[ update by priyanshi]
            services.AddScoped<IBUBasedSweetnerValidationRepository, BUBasedSweetnerValidationRepository>();
            services.AddScoped<IQuestionsForSweetnerRepository, QuestionsForSweetnerRepository>();

            services .AddScoped<IAreaLocalityRepository, AreaLocalityRepository>();
            services.AddScoped<IPushNotificationManager, PushNotificationManager>();

            //use for price master mapping
            services.AddScoped<IPriceMasterNameRepository, PriceMasterNameRepository>();
            services.AddScoped<IPriceMasterNameManager, PriceMasterNameManager>();
            services.AddScoped<IPriceMasterMappingRepository, PriceMasterMappingRepository>();
            services.AddScoped<IPriceMasterMappingManager, PriceMasterMappingManager>();

            services.AddHttpClient<FcmSender>();
            services.AddHttpClient<CorePush.Apple.ApnSender>();

            //use for price master mapping
            services.AddScoped<IPriceMasterNameRepository, PriceMasterNameRepository>();
            services.AddScoped<IPriceMasterMappingRepository, PriceMasterMappingRepository>();

            //MapLoginUserDeviceRepository Added by Kranti Silawat
            services.AddScoped<IMapLoginUserDeviceRepository, LoginUserDeviceRepository>();

            //ProductTechnologyManagerRepository Added by Abhishek Sharma
            services.AddScoped<IProductTechnologyManager, ProductTechnologyManager>();
            services.AddScoped<IProductTechnologyRepository, ProductTechnologyRepository>();
            services.AddScoped<IPushNotificationMessageDetailRepository, PushNotificationMessageDetailRepository>();
            services.AddScoped<IPushNotificationSavedDetailsRepository, PushNotificationSavedDetailsRepository>();

            //ProductTechnologyManagerRepository Added by Abhishek Sharma           
            services.AddScoped<IUniversalPriceMasterManager, UniversalPriceMasterManager>();
            services.AddScoped<IUniversalPriceMasterRepository, UniversalPriceMasterRepository>();
            services.AddScoped<IProductConditionLabelRepository, ProductConditionLabelRepository>();
            services.AddScoped<ISweetenerManager, SweetenerManager>();
            services.AddScoped<IModelMappingRepository, ModelMappingRepository>();
            //services.AddScoped<IModalMappingRepository, ModalMappingRepository>();

            services.AddScoped<IOrderBasedConfigRepository, OrderBasedConfigRepository>();
            //EVC Auto Allocation phase 2 priyanshi
            services.AddScoped<IEVCPartnerRepository, EVCPartnerRepository>();
            services.AddScoped<IEvcPartnerPreferenceRepository, EvcPartnerPreferenceRepository>();

            // Customer files for ABB
            services.AddScoped<ICustomerFilesRepository, CustomerFilesRepository>();

            //ProductConditionLabelRepository Added by Kranti Silawat
            services.AddScoped<IProductConditionLabelRepository, ProductConditionLabelRepository>();
            services.AddScoped<IProductConditionLabelManager, ProductConditionLabelManager>();

            services.AddScoped<ITermsAndConditionsForVoucherRepository, TermsAndConditionsForVoucherRepository>();
//<<<<<<< Dev
            // Added by VK
            services.AddScoped<IEVCPriceRangeMasterRepository, EVCPriceRangeMasterRepository>();

          

            //Diagnose V2
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

            //Self QC Temp Data
            services.AddScoped<ITempDataRepository, TempDataRepository>();
            services.AddScoped<IUserMappingRepository, UserMappingRepository>();
            services.AddScoped<IApiCallsRepository, ApiCallsRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RDCELERP.CoreWebApi v1"));
            }

            app.UseSession();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseRouting();
            #region Enable Cors Added by VK for React Projects
            app.UseCors("EnableCorsPolicy");
            #endregion
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });



            //app.UseCors(x => x
            //.AllowAnyOrigin()
            //.AllowAnyMethod()
            //.AllowAnyHeader());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseDeveloperExceptionPage();
        }
    }
}
