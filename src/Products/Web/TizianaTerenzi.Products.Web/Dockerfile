# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Debug
WORKDIR /src
COPY ["Directory.Build.props", "."]
COPY ["Products/Web/TizianaTerenzi.Products.Web/TizianaTerenzi.Products.Web.csproj", "Products/Web/TizianaTerenzi.Products.Web/"]
COPY ["Common/TizianaTerenzi.Common.Messages/TizianaTerenzi.Common.Messages.csproj", "Common/TizianaTerenzi.Common.Messages/"]
COPY ["Common/TizianaTerenzi.Common.Web/TizianaTerenzi.Common.Web.csproj", "Common/TizianaTerenzi.Common.Web/"]
COPY ["Common/TizianaTerenzi.Common/TizianaTerenzi.Common.csproj", "Common/TizianaTerenzi.Common/"]
COPY ["Common/TizianaTerenzi.Common.Web.Infrastructure/TizianaTerenzi.Common.Web.Infrastructure.csproj", "Common/TizianaTerenzi.Common.Web.Infrastructure/"]
COPY ["Common/TizianaTerenzi.Common.Data/TizianaTerenzi.Common.Data.csproj", "Common/TizianaTerenzi.Common.Data/"]
COPY ["Common/TizianaTerenzi.Common.Services/TizianaTerenzi.Common.Services.csproj", "Common/TizianaTerenzi.Common.Services/"]
COPY ["Common/TizianaTerenzi.Common.Services.Mapping/TizianaTerenzi.Common.Services.Mapping.csproj", "Common/TizianaTerenzi.Common.Services.Mapping/"]
COPY ["Products/Data/TizianaTerenzi.Products.Data/TizianaTerenzi.Products.Data.csproj", "Products/Data/TizianaTerenzi.Products.Data/"]
COPY ["Products/Data/TizianaTerenzi.Products.Data.Models/TizianaTerenzi.Products.Data.Models.csproj", "Products/Data/TizianaTerenzi.Products.Data.Models/"]
COPY ["Products/Services/TizianaTerenzi.Products.Services.Data/TizianaTerenzi.Products.Services.Data.csproj", "Products/Services/TizianaTerenzi.Products.Services.Data/"]
COPY ["Products/Web/TizianaTerenzi.Products.Web.Models/TizianaTerenzi.Products.Web.Models.csproj", "Products/Web/TizianaTerenzi.Products.Web.Models/"]
COPY ["Products/Services/TizianaTerenzi.Products.Services/TizianaTerenzi.Products.Services.csproj", "Products/Services/TizianaTerenzi.Products.Services/"]
RUN dotnet restore "./Products/Web/TizianaTerenzi.Products.Web/TizianaTerenzi.Products.Web.csproj"
COPY . .
WORKDIR "/src/Products/Web/TizianaTerenzi.Products.Web"
RUN dotnet build "./TizianaTerenzi.Products.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Debug
RUN dotnet publish "./TizianaTerenzi.Products.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TizianaTerenzi.Products.Web.dll"]