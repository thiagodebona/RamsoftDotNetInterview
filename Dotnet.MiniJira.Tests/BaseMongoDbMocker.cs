using Dotnet.MiniJira.Application;
using Dotnet.MiniJira.Application.Authorization;
using Dotnet.MiniJira.Application.Interface;
using Dotnet.MiniJira.Application.Seeder;
using Dotnet.MiniJira.Domain.Entities;
using Dotnet.MiniJira.Domain.Helpers;
using Dotnet.MiniJira.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Mongo2Go;
using MongoDB.Driver;

namespace Dotnet.MiniJira.Tests
{
    public class BaseMongoDbMocker
    {
        internal static IConfiguration _configuration;
        internal static IServiceProvider _serviceProvider;
        internal static MongoDbRunner _runner;
        internal static IMongoCollection<Board> _boardCollection;
        internal static IMongoCollection<User> _userCollection;



        internal static void ConfigureServicesAndStuff()
        {
            _runner = MongoDbRunner.Start();

            MongoClient client = new MongoClient(_runner.ConnectionString);
            IMongoDatabase database = client.GetDatabase("IntegrationTest");
            _boardCollection = database.GetCollection<Board>("Board");
            _userCollection = database.GetCollection<User>("User");


            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                 .AddEnvironmentVariables()
                 .Build();

            var services = new ServiceCollection();

            services.AddLogging();

            services.Configure<AppSettings>(_configuration.GetSection("AppSettings"));

            // configure DI for application services
            var mongoClientSettings = MongoClientSettings.FromConnectionString(_runner.ConnectionString);
            services.AddSingleton<IMongoClient>(new MongoClient(mongoClientSettings));
            services.AddSingleton(typeof(IMongoBaseRepository<>), typeof(MongoBaseRepository<>));

            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IBoardService, BoardService>();
            services.AddScoped<ITaskService, TaskService>();
            services.AddSingleton<IInitialDataSeederService, InitialDataSeeder>();

            services.AddSingleton<ILoggerFactory, NullLoggerFactory>();

            _serviceProvider = services.BuildServiceProvider();

            _serviceProvider.GetService<IInitialDataSeederService>()?.SeedDatabase().Wait();
        }
    }
}
