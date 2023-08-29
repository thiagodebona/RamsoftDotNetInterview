using Dotnet.MiniJira.API.Middleware;
using Dotnet.MiniJira.API.Notifications;
using Dotnet.MiniJira.Application.Interface;
using Microsoft.Extensions.Options;
using MiniJira.API.MIddleware;
using System.Text.Json.Serialization;

namespace Dotnet.MiniJira.API.Extensions
{
    public static class AppStartUp
    {
        public static void ConfigureServer(this IServiceCollection services, WebApplicationBuilder builder)
        {
            builder.WebHost.UseUrls(builder.Configuration.GetSection("AppSettings").GetValue<string>("ServerUrl"));

            services.AddCors();
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            services.InjectRequiredDependencies(builder.Configuration);

            // Extension that do all the required config to enable swagger v2
            services.ConfigureSwagger();
        }

        public static void ConfigureApp(this WebApplication app, IConfiguration settings)
        {
            /* Seeding the db */

            app.Services.GetService<IInitialDataSeederService>()?.SeedDatabase().Wait();

            app.ConfigureSwagger();

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

            var webSocketUrl = settings.GetSection("AppSettings").GetValue<string>("ServerUrl").Replace("http", "ws");

            app.Services.GetService<IWebSocketRegister>()?.SetUpLiveNotificationsServer(app, $"{webSocketUrl}/ws");
        }
    }
}
