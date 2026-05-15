# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM borislechev/base-runtime:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Build.props", "."]
COPY ["Identity/Web/TizianaTerenzi.Identity.Web/TizianaTerenzi.Identity.Web.csproj", "Identity/Web/TizianaTerenzi.Identity.Web/"]
COPY ["Common/TizianaTerenzi.Common.Messages/TizianaTerenzi.Common.Messages.csproj", "Common/TizianaTerenzi.Common.Messages/"]
COPY ["Common/TizianaTerenzi.Common.Web/TizianaTerenzi.Common.Web.csproj", "Common/TizianaTerenzi.Common.Web/"]
COPY ["Common/TizianaTerenzi.Common/TizianaTerenzi.Common.csproj", "Common/TizianaTerenzi.Common/"]
COPY ["Common/TizianaTerenzi.Common.Web.Infrastructure/TizianaTerenzi.Common.Web.Infrastructure.csproj", "Common/TizianaTerenzi.Common.Web.Infrastructure/"]
COPY ["Common/TizianaTerenzi.Common.Data/TizianaTerenzi.Common.Data.csproj", "Common/TizianaTerenzi.Common.Data/"]
COPY ["Common/TizianaTerenzi.Common.Services/TizianaTerenzi.Common.Services.csproj", "Common/TizianaTerenzi.Common.Services/"]
COPY ["Common/TizianaTerenzi.Common.Services.Mapping/TizianaTerenzi.Common.Services.Mapping.csproj", "Common/TizianaTerenzi.Common.Services.Mapping/"]
COPY ["Identity/Data/TizianaTerenzi.Identity.Data/TizianaTerenzi.Identity.Data.csproj", "Identity/Data/TizianaTerenzi.Identity.Data/"]
COPY ["Identity/Services/TizianaTerenzi.Identity.Services.Data/TizianaTerenzi.Identity.Services.Data.csproj", "Identity/Services/TizianaTerenzi.Identity.Services.Data/"]
COPY ["Identity/Data/TizianaTerenzi.Identity.Data.Models/TizianaTerenzi.Identity.Data.Models.csproj", "Identity/Data/TizianaTerenzi.Identity.Data.Models/"]
COPY ["Identity/Web/TizianaTerenzi.Identity.Web.Models/TizianaTerenzi.Identity.Web.Models.csproj", "Identity/Web/TizianaTerenzi.Identity.Web.Models/"]
COPY ["Identity/Services/TizianaTerenzi.Identity.Services/TizianaTerenzi.Identity.Services.csproj", "Identity/Services/TizianaTerenzi.Identity.Services/"]
COPY ["Identity/Services/TizianaTerenzi.Identity.Services.Models/TizianaTerenzi.Identity.Services.Models.csproj", "Identity/Services/TizianaTerenzi.Identity.Services.Models/"]
RUN dotnet restore "./Identity/Web/TizianaTerenzi.Identity.Web/TizianaTerenzi.Identity.Web.csproj"
COPY . .
WORKDIR "/src/Identity/Web/TizianaTerenzi.Identity.Web"
RUN dotnet build "./TizianaTerenzi.Identity.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TizianaTerenzi.Identity.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
CMD ["dotnet", "TizianaTerenzi.Identity.Web.dll"]