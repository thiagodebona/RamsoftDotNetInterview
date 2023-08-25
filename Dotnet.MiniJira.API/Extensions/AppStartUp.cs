using Dotnet.MiniJira.API.Middleware;
using Dotnet.MiniJira.Application.Interface;
using MiniJira.API.MIddleware;
using System.Text.Json.Serialization;

namespace Dotnet.MiniJira.API.Extensions
{
    public static class AppStartUp
    {
        public static void ConfigureServer(this IServiceCollection services, WebApplicationBuilder builder)
        {
            services.AddCors();
            services.AddControllers()
                .AddJsonOptions(x => x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull);

            services.InjectRequiredDependencies(builder.Configuration);

            // Extension that do all the required config to enable swagger v2
            services.ConfigureSwagger();
        }

        public static void ConfigureApp(this WebApplication app)
        {
            /* Seeding the db */
            app.Services.GetService<IInitialDataSeederService>().SeedDatabase().Wait();

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
        }
    }
}
