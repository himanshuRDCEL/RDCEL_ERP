using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.DAL.Repository;
using RDCELERP.Model.Base;


namespace Web.API
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

            // Add our Config object so it can be injected
            // Add our Config object so it can be injected
            services.Configure<ApplicationSettings>(Configuration.GetSection("ApplicationSettings"));
            //services.Configure<ConnectionStrings>(Configuration.GetSection("ConnectionStrings"));

            //Add DB context
            services.AddDbContext<Digi2l_DevContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("Digi2l_DevContext")), ServiceLifetime.Transient);
            #region Adding scope for BAL and DAL

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
            services.AddScoped<ILoginRepository, LoginRepository>();
            services.AddScoped<IVoucherRepository, VoucherRepository>();
            //services.AddScoped<IEVCPriceMAster, EVCPriceMasterRepository>();
            services.AddScoped<IOrderImageUploadRepository, OrderImageUploadRepository>();
            services.AddScoped<IVoucherStatusRepository, VoucherStatusRepository>();
            services.AddScoped<IModelNumberRepository, ModelNumberRepository>();
            services.AddScoped<IModelNumberManager, ModelNumberManager>();
            services.AddScoped<IHistoryRepository, HistoryRepository>();
            services.AddScoped<IBillCloudServiceCall, BillCloudServiceCall>();


            services.AddScoped<ITimeLineMappingStatusRepository, TimeLineStatusMappingRepository>();
            services.AddScoped<ITimeLineRepository, TimeLineRepository>();




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


            services.AddControllers();
            services.AddTransient<IUserRepository, UserRepository>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = Configuration["Jwt:ValidAudience"],
                    ValidIssuer = Configuration["Jwt:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Secret"]))

                };
            });

            #region For Automapper

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MapperManager());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            #endregion

            // Mail manager
            services.AddScoped<IMailManager, MailManager>();

            services.AddScoped<ICommonManager, CommonManager>();


            // Log and Error
            services.AddScoped<ILogging, Logging>();
            services.AddScoped<IErrorLogRepository, ErrorLogRepository>();


            services.AddControllers();

            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "Web.API", Version = "v1" });
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    //Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Web.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}