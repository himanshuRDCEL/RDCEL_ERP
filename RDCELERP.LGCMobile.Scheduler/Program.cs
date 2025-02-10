using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
//using System.Configuration;
using Microsoft.Extensions.Configuration;
using RDCELERP.BAL.Interface;
using RDCELERP.DAL.Entities;
using RDCELERP.LGCMobile.Scheduler.LGCMobileApp;
using RDCELERP.DAL.IRepository;
using RDCELERP.DAL.Repository;
using RDCELERP.BAL.MasterManager;
using AutoMapper;
using RDCELERP.BAL.Common;
using RDCELERP.Model.Base;
using RDCELERP.Common.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;

namespace RDCELERP.LGCMobile.Scheduler
{
    public class Program
    {       
        static void Main(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MapperManager());
            });
            IMapper mapper = mappingConfig.CreateMapper();

            var builder = WebApplication.CreateBuilder(args);
            var environment = builder.Environment;

            var serviceProvider = new ServiceCollection()
                .AddScoped<Digi2l_DevContext>()
                .AddScoped<ILGCMobileScheduler, LGCMobileScheduler>()
                .AddScoped<IWalletTransactionRepository, WalletTransactionRepository>()
                .AddScoped<IOrderTransRepository, OrderTransRepository>()
                .AddScoped<ILogisticsRepository, LogisticsRepository>()
                .AddScoped<IExchangeOrderRepository, ExchangeOrderRepository>()
                .AddScoped<IABBRedemptionRepository, ABBRedemptionRepository>()
                .AddScoped<IExchangeABBStatusHistoryRepository, ExchangeABBStatusHistoryRepository>()
                .AddScoped<ICommonManager, CommonManager>()
                .AddScoped<IAccessListRepository, AccessListRepository>()
                .AddScoped<IRoleRepository, RoleRepository>()
                .AddScoped<IRoleAccessRepository, RoleAccessRepository>()
                .AddScoped<ILogging, Logging>()
                .AddScoped<IOrderImageUploadRepository, OrderImageUploadRepository>()
                .AddScoped<ILovRepository, LovRepository>()
                .AddScoped<IErrorLogRepository, ErrorLogRepository>()
                .AddScoped<IExchangeABBStatusHistoryRepository, ExchangeABBStatusHistoryRepository>()
                .AddScoped<ITemplateConfigurationRepository, TemplateConfigurationRepository>()
                .AddScoped<IOrderQCRepository, OrderQCRepository>()
                .AddScoped<ISelfQCRepository, SelfQCRepository>()
                .AddScoped<IVehicleIncentiveRepository, VehicleIncentiveRepository>()
                .AddScoped<IVehicleJourneyTrackingDetailsRepository, VehicleJourneyTrackingDetailsRepository>()
                .AddScoped<IPriceMasterMappingRepository, PriceMasterMappingRepository>()
                .AddScoped<IPriceMasterNameRepository, PriceMasterNameRepository>()
                .AddScoped<IBusinessUnitRepository, BusinessUnitRepository>()
                .AddScoped<IBrandRepository, BrandRepository>()
                .AddScoped<IBrandSmartBuyRepository, BrandSmartBuyRepository>()
                .AddScoped<IEVCRepository, EVCRepository>()
                .AddScoped<IPinCodeRepository, PinCodeRepository>()
                .AddScoped<IAreaLocalityRepository, AreaLocalityRepository>()
                .Configure<ApplicationSettings>(configuration.GetSection("ApplicationSettings"))
                .AddSingleton(mapper)
                .AddSingleton(environment)
                .BuildServiceProvider();

            

            using (var _lGCMobileScheduler = serviceProvider.GetService<ILGCMobileScheduler>())
            {
                _lGCMobileScheduler.RollbackOrderFromDriver();
            }
            
        }

    }

}