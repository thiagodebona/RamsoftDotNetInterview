using Dotnet.MiniJira.API.Notifications;
using Dotnet.MiniJira.Application;
using Dotnet.MiniJira.Application.Authorization;
using Dotnet.MiniJira.Application.Interface;
using Dotnet.MiniJira.Domain.Helpers;
using Dotnet.MiniJira.Infrastructure;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using MongoDB.Entities;

namespace Dotnet.MiniJira.API.Extensions
{
    public static class DependenciesInjector
    {
        public static async Task InjectRequiredDependencies(this IServiceCollection services, ConfigurationManager configurationManager)
        {
            // configure strongly typed settings object
            services.Configure<AppSettings>(configurationManager.GetSection("AppSettings"));

            BsonSerializer.RegisterSerializer(new ObjectSerializer(type => ObjectSerializer.DefaultAllowedTypes(type) || type.Name!.EndsWith("Message")));

            var appSettings = configurationManager.GetSection(nameof(AppSettings)).Get<AppSettings>()!;

           await DB.InitAsync(appSettings.Database.Name,
                MongoClientSettings.FromConnectionString(
                    $"mongodb://{appSettings.Database.Username}:{appSettings.Database.Password}@{appSettings.Database.Host}:{appSettings.Database.Port ?? 27017}/?authSource=admin"));

            await DB.MigrateAsync();

            services.AddSingleton(typeof(IUserRepository<>), typeof(UserRepository<>));

            services.AddSingleton<JwtRepository, JwtRepository>();
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IWebSocketRegister, WebSocketRegister>();
            services.AddSingleton<IBroadcasterService, BroadcasterService>();
            
        }
    }
}
