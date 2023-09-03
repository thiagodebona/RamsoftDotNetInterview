using System.Text.Json.Serialization;
using Dotnet.MiniJira.API.Middleware;
using Dotnet.MiniJira.API.Notifications;
using Dotnet.MiniJira.Application.Interface;
using Dotnet.MiniJira.Domain.Helpers;
using MiniJira.API.MIddleware;

namespace Dotnet.MiniJira.API.Extensions
{
    public static class AppStartUp
    {
        public static async Task ConfigureServer(this IServiceCollection services, WebApplicationBuilder builder)
        {
            builder.WebHost.UseUrls(builder.Configuration.GetSection("AppSettings").GetValue<string>("ServerUrl"));

            services.AddCors();
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            await services.InjectRequiredDependencies(builder.Configuration);

            // Extension that do all the required config to enable swagger v2
            services.ConfigureSwagger();
        }

        public static void ConfigureApp(this WebApplication app, IConfiguration settings)
        {
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

            var appSettings = settings.GetSection(nameof(AppSettings)).Get<AppSettings>()!;

            var webSocketUrl = appSettings.ServerUrl.Replace("http", "ws");

            app.Services.GetService<IWebSocketRegister>()?.SetUpLiveNotificationsServer(app, $"{webSocketUrl}/ws");
        }
    }
}
