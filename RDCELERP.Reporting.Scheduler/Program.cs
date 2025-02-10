using AutoMapper;
using Irony.Ast;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net;
using RDCELERP.BAL.Common;
using RDCELERP.BAL.Interface;
using RDCELERP.BAL.MasterManager;
using RDCELERP.Common.Helper;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.DAL.Repository;
using RDCELERP.Model.Base;
using RDCELERP.Reporting.Scheduler.SponsorReporting;

namespace RDCELERP.Reporting.Scheduler
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start Execution");
            var path1 = Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent;
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .SetBasePath(path1?.ToString())
                .Build();

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MapperManager());
            });
            IMapper mapper = mappingConfig.CreateMapper();

            var builder = WebApplication.CreateBuilder(args);
            var environment = builder.Environment;
            environment.WebRootPath = path1?.ToString();
            var serviceProvider = new ServiceCollection()
                .AddScoped<IPendingOrderManager, PendingOrderManager>()
            .AddScoped<IBusinessUnitRepository, BusinessUnitRepository>()
            .AddScoped<IOrderTransRepository, OrderTransRepository>()
            .AddScoped<IMailManager, MailManager>()
            .AddScoped<ITemplateConfigurationRepository, TemplateConfigurationRepository>()
            .AddScoped<IOrderQCRepository, OrderQCRepository>()
            .AddScoped<IErrorLogRepository, ErrorLogRepository>()
            .AddScoped<ILogging, Logging>()
            .AddScoped<IPendingOrderManager, PendingOrderManager>()
                .Configure<ApplicationSettings>(configuration.GetSection("ApplicationSettings"))
                .Configure<ConnectionStrings>(configuration.GetSection("ConnectionStrings"))
                .AddDbContext<Digi2l_DevContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("Digi2l_DevContext"), o => o.CommandTimeout(3600)), ServiceLifetime.Scoped)
                .AddSingleton(configuration)
                .AddSingleton(mapper)
                .AddSingleton(environment)
                .BuildServiceProvider();

            using (var obj = serviceProvider.GetService<IPendingOrderManager>())
            {
                obj.ReportingPendingOrders();
                obj.Dispose();
            }
        }

        //public static void Main(string[] args)
        //{
        //    CreateHostBuilder(args).Build().Run();
        //}
        //public static IHostBuilder CreateHostBuilder(string[] args) =>
        //    Host.CreateDefaultBuilder(args)
        //        .ConfigureWebHostDefaults(webBuilder =>
        //        {
        //            webBuilder.UseStartup<Startup>();
        //        });
    }
}
