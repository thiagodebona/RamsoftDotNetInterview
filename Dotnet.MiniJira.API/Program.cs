using Dotnet.MiniJira.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureServer(builder);

var app = builder.Build();

app.ConfigureApp(builder.Configuration);

app.UseWebSockets();

await app.RunAsync();