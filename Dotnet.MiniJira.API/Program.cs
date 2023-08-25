using Dotnet.MiniJira.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

builder.WebHost.UseUrls(builder.Configuration.GetSection("AppSettings").GetValue<string>("ServerUrl"));

services.ConfigureServer(builder);

var app = builder.Build();

app.ConfigureApp();

await app.RunAsync();