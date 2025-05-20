#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Debug
WORKDIR /src
COPY ["Orders/Web/TizianaTerenzi.Orders.Web/TizianaTerenzi.Orders.Web.csproj", "Orders/Web/TizianaTerenzi.Orders.Web/"]
COPY ["Common/TizianaTerenzi.Common.Messages/TizianaTerenzi.Common.Messages.csproj", "Common/TizianaTerenzi.Common.Messages/"]
COPY ["Common/TizianaTerenzi.Common.Web/TizianaTerenzi.Common.Web.csproj", "Common/TizianaTerenzi.Common.Web/"]
COPY ["Common/TizianaTerenzi.Common/TizianaTerenzi.Common.csproj", "Common/TizianaTerenzi.Common/"]
COPY ["Common/TizianaTerenzi.Common.Web.Infrastructure/TizianaTerenzi.Common.Web.Infrastructure.csproj", "Common/TizianaTerenzi.Common.Web.Infrastructure/"]
COPY ["Common/TizianaTerenzi.Common.Data/TizianaTerenzi.Common.Data.csproj", "Common/TizianaTerenzi.Common.Data/"]
COPY ["Common/TizianaTerenzi.Common.Services/TizianaTerenzi.Common.Services.csproj", "Common/TizianaTerenzi.Common.Services/"]
COPY ["Common/TizianaTerenzi.Common.Services.Mapping/TizianaTerenzi.Common.Services.Mapping.csproj", "Common/TizianaTerenzi.Common.Services.Mapping/"]
COPY ["Orders/Data/TizianaTerenzi.Orders.Data/TizianaTerenzi.Orders.Data.csproj", "Orders/Data/TizianaTerenzi.Orders.Data/"]
COPY ["Orders/Data/TizianaTerenzi.Orders.Data.Models/TizianaTerenzi.Orders.Data.Models.csproj", "Orders/Data/TizianaTerenzi.Orders.Data.Models/"]
COPY ["Orders/Services/TizianaTerenzi.Orders.Services.Data/TizianaTerenzi.Orders.Services.Data.csproj", "Orders/Services/TizianaTerenzi.Orders.Services.Data/"]
COPY ["Orders/Web/TizianaTerenzi.Orders.Web.Models/TizianaTerenzi.Orders.Web.Models.csproj", "Orders/Web/TizianaTerenzi.Orders.Web.Models/"]
RUN dotnet restore "./Orders/Web/TizianaTerenzi.Orders.Web/TizianaTerenzi.Orders.Web.csproj"
COPY . .
WORKDIR "/src/Orders/Web/TizianaTerenzi.Orders.Web"
RUN dotnet build "./TizianaTerenzi.Orders.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Debug
RUN dotnet publish "./TizianaTerenzi.Orders.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TizianaTerenzi.Orders.Web.dll"]