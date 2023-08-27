using Amazon.Util.Internal;
using Dotnet.MiniJira.API.Extensions;
using Dotnet.MiniJira.Domain.Helpers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureServer(builder);

var app = builder.Build();

app.ConfigureApp(builder.Configuration);

app.UseWebSockets();


//var ssss = new List<Test>()
//{
//    new Test { Favorite = false, Name = "AAAA" },
//    new Test { Favorite = true, Name = "BBBB" },
//    new Test { Favorite = true, Name = "CCCC" },
//    new Test { Favorite = false, Name = "DDDD" },
//    new Test { Favorite = false, Name = "EEEE" },
//};

//var orderedAsc = ssss.OrderByDescending(s => s.Favorite).ThenBy(s => s.Name);
//var orderedDesc = ssss.OrderByDescending(s => s.Favorite).ThenByDescending(s => s.Name);


//var result1 = ssss.CustomSortBy("Name", true);
//var result12 = ssss.CustomSortBy("Name", false);
//var resultFavoriteTopFirstAsc = ssss.CustomSortBy("Name", false, "Favorite");
//var resultFavoriteTopFirstDesc = ssss.CustomSortBy("Name", true, "Favorite");

await app.RunAsync();