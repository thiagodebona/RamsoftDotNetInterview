using Dotnet.MiniJira.Application;
using Dotnet.MiniJira.Application.Interface;
using Dotnet.MiniJira.Application.Seeder;
using Dotnet.MiniJira.Domain.Entities;
using Dotnet.MiniJira.Domain.Helpers;
using Dotnet.MiniJira.Infrastructure;
using MiniJira.API.MIddleware;
using MongoDB.Driver;
using System.Text.Json.Serialization;
using WebApi.Authorization;
using WebApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

{
    var services = builder.Services;
    var env = builder.Environment;

    builder.WebHost.UseUrls("http://localhost:6969");
    services.AddCors();
    services.AddControllers()
        .AddJsonOptions(x => x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull);

    // configure strongly typed settings object
    services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

    // configure DI for application services
    var mongoClientSettings = MongoClientSettings.FromConnectionString(builder.Configuration.GetConnectionString("Mongo"));
    services.AddSingleton<IMongoClient>(new MongoClient(mongoClientSettings));
    services.AddSingleton(typeof(IMongoBaseRepository<>), typeof(MongoBaseRepository<>));

    services.AddScoped<IJwtService, JwtService>();
    services.AddScoped<IUserService, UserService>();
    services.AddScoped<IBoardService, BoardService>();
    services.AddSingleton<IInitialDataSeederService, InitialDataSeeder>();
}

var app = builder.Build();

{
    var userRepo = app.Services.GetService<IMongoBaseRepository<User>>();
    var boardRepo = app.Services.GetService<IMongoBaseRepository<Board>>();
    var logger = app.Services.GetService<ILogger<UserService>>();

    /* Seeding the db */
    await app.Services.GetService<IInitialDataSeederService>().SeedDatabase();

    // configure HTTP request pipeline

    // global cors policy
    app.UseCors(x => x
        .SetIsOriginAllowed(origin => true)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials());

    // global error handler
    app.UseMiddleware<ErrorHandlerMiddleware>();

    // custom jwt auth middleware
    app.UseMiddleware<JwtMiddleware>();

    app.MapControllers();
}

await app.RunAsync();