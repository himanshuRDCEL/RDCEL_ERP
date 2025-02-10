using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RDCELERP.BAL.Common;
using RDCELERP.BAL.Interface;
using RDCELERP.BAL.MasterManager;
using RDCELERP.Common.Helper;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.DAL.Repository;
using RDCELERP.EndJourney.Notification.Scheduler.EndJourneyNotification;
using RDCELERP.Model.Base;

namespace RDCELERP.EndJourney.Notification.Scheduler
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

            var serviceProvider = new ServiceCollection()
                .AddScoped<ISendEndJourneyNotification, SendEndJourneyNotification>()
                .AddScoped<Digi2l_DevContext>()
                .AddScoped<IPushNotificationManager, PushNotificationManager>()
                .AddScoped<IMapLoginUserDeviceRepository, LoginUserDeviceRepository>()
                .AddScoped<IDriverDetailsRepository, DriverDetailsRepository>()
                .AddScoped<IServicePartnerRepository, ServicePartnerRepository>()
                .AddScoped<IPushNotificationMessageDetailRepository, PushNotificationMessageDetailRepository>()
                .AddScoped<IPushNotificationSavedDetailsRepository, PushNotificationSavedDetailsRepository>()
                .AddScoped<ILogging, Logging>()
                .AddScoped<IUserRepository, UserRepository>()
                .AddScoped<IErrorLogRepository, ErrorLogRepository>()
                .AddSingleton(mapper)
                .Configure<ApplicationSettings>(configuration.GetSection("ApplicationSettings"))
                .Configure<ConnectionStrings>(configuration.GetSection("ConnectionStrings"))
                .BuildServiceProvider();

            using (var notificationScheduler = serviceProvider.GetService<ISendEndJourneyNotification>())
            {
                notificationScheduler.SendNotification();
            }
        }
    }
}