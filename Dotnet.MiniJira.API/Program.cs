using Amazon.Util.Internal;
using Dotnet.MiniJira.API.Extensions;
using Dotnet.MiniJira.Domain.Helpers;

var builder = WebApplication.CreateBuilder(args);

await builder.Services.ConfigureServer(builder);

var app = builder.Build();

app.ConfigureApp(builder.Configuration);

app.UseWebSockets();

await app.RunAsync();