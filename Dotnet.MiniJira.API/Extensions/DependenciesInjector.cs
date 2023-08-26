using Dotnet.MiniJira.API.Notifications;
using Dotnet.MiniJira.Application;
using Dotnet.MiniJira.Application.Authorization;
using Dotnet.MiniJira.Application.Interface;
using Dotnet.MiniJira.Application.Seeder;
using Dotnet.MiniJira.Domain.Helpers;
using Dotnet.MiniJira.Infrastructure;
using MongoDB.Driver;

namespace Dotnet.MiniJira.API.Extensions
{
    public static class DependenciesInjector
    {
        public static void InjectRequiredDependencies(this IServiceCollection services, ConfigurationManager configurationManager)
        {
            // configure strongly typed settings object
            services.Configure<AppSettings>(configurationManager.GetSection("AppSettings"));

            // configure DI for application services
            var mongoClientSettings = MongoClientSettings.FromConnectionString(configurationManager.GetConnectionString("Mongo"));
            services.AddSingleton<IMongoClient>(new MongoClient(mongoClientSettings));
            services.AddSingleton(typeof(IMongoBaseRepository<>), typeof(MongoBaseRepository<>));

            services.AddSingleton<IJwtService, JwtService>();
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IBoardService, BoardService>();
            services.AddSingleton<ITaskService, TaskService>();
            services.AddSingleton<IInitialDataSeederService, InitialDataSeeder>();
            services.AddSingleton<IWebSocketRegister, WebSocketRegister>();
            services.AddSingleton<IBroadcasterService, BroadcasterService>();
            
        }
    }
}
