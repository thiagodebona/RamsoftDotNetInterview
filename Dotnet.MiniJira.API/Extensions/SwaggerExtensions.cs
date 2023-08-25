using Microsoft.OpenApi.Models;

namespace Dotnet.MiniJira.API.Extensions
{
    public static class SwaggerExtensions
    {
        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v2", new OpenApiInfo
                {
                    Title = "Mini Jira API",
                    Version = "v2",
                    Description = "Board API for ramsoft interview",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Thaigo de bona",
                        Email = "interview@ramsoft.com",
                        Url = new Uri("https://localhost"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Employee API LICX",
                        Url = new Uri("https://localhost"),
                    }
                });

                OpenApiSecurityScheme securityDefinition = new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    BearerFormat = "JWT",
                    Scheme = "Bearer",
                    Description = "Specify the authorization token",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                };
                c.AddSecurityDefinition("Bearer", securityDefinition);

                //New code to work with .NET6
                //I've splitted in two parts for better reading
                OpenApiSecurityRequirement securityRequirement = new OpenApiSecurityRequirement();
                OpenApiSecurityScheme secondSecurityDefinition = new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };
                securityRequirement.Add(secondSecurityDefinition, new string[] { });
                c.AddSecurityRequirement(securityRequirement);
                //End of new code
            });
        }

        public static void ConfigureSwagger(this WebApplication app)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v2/swagger.json", "v2");
                options.RoutePrefix = string.Empty;
            });
        }
    }
}
