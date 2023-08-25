#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

# create user in the docker image
USER administrator

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Dotnet.MiniJira.API/Dotnet.MiniJira.API.csproj", "Dotnet.MiniJira.API/"]
COPY ["Dotnet.MiniJira.Application/Dotnet.MiniJira.Application.csproj", "Dotnet.MiniJira.Application/"]
COPY ["Dotnet.MiniJira.Domain/Dotnet.MiniJira.Domain.csproj", "Dotnet.MiniJira.Domain/"]
COPY ["Dotnet.MiniJira.Infrastructure/Dotnet.MiniJira.Infrastructure.csproj", "Dotnet.MiniJira.Infrastructure/"]
RUN dotnet restore "Dotnet.MiniJira.API/Dotnet.MiniJira.API.csproj"
COPY . .
WORKDIR "/src/Dotnet.MiniJira.API"
RUN dotnet build "Dotnet.MiniJira.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Dotnet.MiniJira.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Dotnet.MiniJira.API.dll"]