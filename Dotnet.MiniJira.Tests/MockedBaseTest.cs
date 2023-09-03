namespace Dotnet.MiniJira.Tests;

using Dotnet.MiniJira.Application;
using Dotnet.MiniJira.Application.Authorization;
using Dotnet.MiniJira.Application.Interface;
using Dotnet.MiniJira.Application.Seeder;
using Dotnet.MiniJira.Domain.Entities;
using Dotnet.MiniJira.Domain.Helpers;
using Dotnet.MiniJira.Domain.Models.Users;
using Dotnet.MiniJira.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Mongo2Go;
using MongoDB.Driver;
using NUnit.Framework;
using System.Net;

public class MockedBaseTest
{
    internal static IConfiguration _configuration;
    internal static IServiceProvider _serviceProvider;
    internal static MongoDbRunner _runner;
    internal static IMongoCollection<Board> _boardCollection;
    internal static IMongoCollection<User> _userCollection;

    public static IBoardService? _boardService;
    public static IUserService? _userService;
    public User _adminUser;
    public User _devUser;
    public User _testUser;
    HttpListener server = new HttpListener();
    public CancellationTokenSource miniHttpSeverCancelToken = new CancellationTokenSource();

    [OneTimeSetUp]
    public void SetUp()
    {
        ConfigureServicesAndStuff();

        _boardService = ServiceProviderServiceExtensions.GetService<IBoardService>(_serviceProvider);
        _userService = ServiceProviderServiceExtensions.GetService<IUserService>(_serviceProvider);
        _taskService = ServiceProviderServiceExtensions.GetService<ITaskService>(_serviceProvider);

        // Mock broadcast calls
        server.Prefixes.Add("http://localhost:7100/");
        server.Start();
        System.Threading.Tasks.Task.Run(() =>
        {
            while (true)
            {
                if (miniHttpSeverCancelToken.IsCancellationRequested)
                    break;

                HttpListenerContext ctx = server.GetContext();
                using HttpListenerResponse resp = ctx.Response;

                resp.StatusCode = (int)HttpStatusCode.OK;
                resp.StatusDescription = "Status OK";
            }
        }, miniHttpSeverCancelToken.Token);

        // Mock admin user
        _adminUser = _userService.GetById(_userService.CreateUser(new CreateUserRequest
        {
            Email = $"TestAdmin",
            Username = "TestAdmin",
            Name = $"TestAdmin",
            Password = $"TestAdmin",
            Profile = Domain.Enums.User.UserProfile.Administrator
        }, "").Result.Id).Result;

        // Mock dev user
        _devUser = _userService.GetById(_userService.CreateUser(new CreateUserRequest
        {
            Email = $"Dev2",
            Username = "Dev2",
            Name = $"Dev2",
            Password = $"Dev2",
            Profile = Domain.Enums.User.UserProfile.Developer
        }, "").Result.Id).Result;

        // Mock dev user
        _testUser = _userService.GetById(_userService.CreateUser(new CreateUserRequest
        {
            Email = $"Test2",
            Username = "Test2",
            Name = $"Test2",
            Password = $"Test2",
            Profile = Domain.Enums.User.UserProfile.Tester
        }, "").Result.Id).Result;
    }

    [OneTimeTearDown]
    public void BaseTearDown()
    {
        miniHttpSeverCancelToken.Cancel();
        server.Stop();
    }


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
        services.AddSingleton(typeof(IUserRepository<>), typeof(UserRepository<>));

        services.AddSingleton<JwtRepository, JwtRepository>();
        services.AddSingleton<IUserService, UserService>();
        services.AddSingleton<IBoardService, BoardService>();
        services.AddSingleton<ITaskService, TaskService>();
        services.AddSingleton<IInitialDataSeederService, InitialDataSeeder>();
        services.AddSingleton<IBroadcasterService, BroadcasterService>();

        services.AddSingleton<ILoggerFactory, NullLoggerFactory>();

        _serviceProvider = services.BuildServiceProvider();

        _serviceProvider.GetService<IInitialDataSeederService>()?.SeedDatabase().Wait();
    }
}