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
using RDCELERP.Reporting.Scheduler.SponsorReporting;
using CorePush.Apple;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.Extensions.Options;

namespace RDCELERP.Reporting.Scheduler
{
    public class Startup
    {
        public Startup(Microsoft.AspNetCore.Hosting.IWebHostEnvironment env)
        {
            var path1 = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent;
            var builder = new ConfigurationBuilder()
             .SetBasePath(path1.ToString())
             .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            env.ContentRootPath = path1.ToString();

            Configuration = builder.Build();
        }
        public IConfiguration Configuration { get; }
        //public ApplicationSettings? ApplicationSettings { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region Add Configuration
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddMemoryCache();
            // Add our Config object so it can be injected
            services.Configure<ApplicationSettings>(Configuration.GetSection("ApplicationSettings"));
            services.Configure<ConnectionStrings>(Configuration.GetSection("ConnectionStrings"));

            // services.AddDbContext<Digi2l_DevContext>(options => options.UseSqlServer("data source=DESKTOP-8UNQOCK\\SQLEXPRESS;initial catalog=Digi2L_Dev ;persist security info=True;user id=sa;password=admin@123;MultipleActiveResultSets=True;Connection Timeout=36000;"),ServiceLifetime.Scoped);
            services.AddDbContext<Digi2l_DevContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("Digi2l_DevContext"), o => o.CommandTimeout(3600)), ServiceLifetime.Scoped);
            #endregion

            #region For Automapper
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MapperManager());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
            #endregion

            #region Add Scope
            services.AddScoped<IPendingOrderManager, PendingOrderManager>();
            services.AddScoped<IBusinessUnitRepository, BusinessUnitRepository>();
            services.AddScoped<IOrderTransRepository, OrderTransRepository>();
            services.AddScoped<IMailManager, MailManager>();
            services.AddScoped<ITemplateConfigurationRepository, TemplateConfigurationRepository>();
            services.AddScoped<IOrderQCRepository, OrderQCRepository>();
            services.AddScoped<IErrorLogRepository, ErrorLogRepository>();
            services.AddScoped<ILogging, Logging>();
            services.AddScoped<IPendingOrderManager, PendingOrderManager>();
            #endregion

            #region Task Calling
            //var provider = services.BuildServiceProvider();
            //var obj = provider.GetService<IPendingOrderManager>();
            //obj.ReportingPendingOrders();
            //obj.Dispose();
            //Console.Read();
            #endregion
            //using (var obj1 = provider.GetService<IPendingOrderManager>())
            //{
            //    obj1.ReportingPendingOrders();
            //}
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
                app.UseHsts();
            }
            app.UseStaticFiles();
            using (var obj = app.ApplicationServices.GetRequiredService<IPendingOrderManager>())
            {
                obj.ReportingPendingOrders();
            }
        }
    }
}

