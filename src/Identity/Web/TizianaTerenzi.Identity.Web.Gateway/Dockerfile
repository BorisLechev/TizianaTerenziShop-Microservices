# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Build.props", "."]
COPY ["Identity/Web/TizianaTerenzi.Identity.Web.Gateway/TizianaTerenzi.Identity.Web.Gateway.csproj", "Identity/Web/TizianaTerenzi.Identity.Web.Gateway/"]
COPY ["Common/TizianaTerenzi.Common.Web.Infrastructure/TizianaTerenzi.Common.Web.Infrastructure.csproj", "Common/TizianaTerenzi.Common.Web.Infrastructure/"]
COPY ["Common/TizianaTerenzi.Common.Data/TizianaTerenzi.Common.Data.csproj", "Common/TizianaTerenzi.Common.Data/"]
COPY ["Common/TizianaTerenzi.Common.Services/TizianaTerenzi.Common.Services.csproj", "Common/TizianaTerenzi.Common.Services/"]
COPY ["Common/TizianaTerenzi.Common/TizianaTerenzi.Common.csproj", "Common/TizianaTerenzi.Common/"]
COPY ["Common/TizianaTerenzi.Common.Services.Mapping/TizianaTerenzi.Common.Services.Mapping.csproj", "Common/TizianaTerenzi.Common.Services.Mapping/"]
COPY ["Common/TizianaTerenzi.Common.Web/TizianaTerenzi.Common.Web.csproj", "Common/TizianaTerenzi.Common.Web/"]
RUN dotnet restore "./Identity/Web/TizianaTerenzi.Identity.Web.Gateway/TizianaTerenzi.Identity.Web.Gateway.csproj"
COPY . .
WORKDIR "/src/Identity/Web/TizianaTerenzi.Identity.Web.Gateway"
RUN dotnet build "./TizianaTerenzi.Identity.Web.Gateway.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TizianaTerenzi.Identity.Web.Gateway.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TizianaTerenzi.Identity.Web.Gateway.dll"]