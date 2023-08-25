using Dotnet.MiniJira.Application;
using Dotnet.MiniJira.Application.Interface;
using Dotnet.MiniJira.Application.Seeder;
using Dotnet.MiniJira.Domain.Helpers;
using Dotnet.MiniJira.Infrastructure;
using MongoDB.Driver;
using WebApi.Authorization;

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

            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IBoardService, BoardService>();
            services.AddScoped<ITaskService, TaskService>();
            services.AddSingleton<IInitialDataSeederService, InitialDataSeeder>();
        }
    }
}
