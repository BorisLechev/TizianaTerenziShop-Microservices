#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Debug
WORKDIR /src
COPY ["Notifications/Web/TizianaTerenzi.Notifications.Web/TizianaTerenzi.Notifications.Web.csproj", "Notifications/Web/TizianaTerenzi.Notifications.Web/"]
COPY ["Common/TizianaTerenzi.Common.Messages/TizianaTerenzi.Common.Messages.csproj", "Common/TizianaTerenzi.Common.Messages/"]
COPY ["Common/TizianaTerenzi.Common.Web/TizianaTerenzi.Common.Web.csproj", "Common/TizianaTerenzi.Common.Web/"]
COPY ["Common/TizianaTerenzi.Common/TizianaTerenzi.Common.csproj", "Common/TizianaTerenzi.Common/"]
COPY ["Notifications/Data/TizianaTerenzi.Notifications.Data/TizianaTerenzi.Notifications.Data.csproj", "Notifications/Data/TizianaTerenzi.Notifications.Data/"]
COPY ["Common/TizianaTerenzi.Common.Data/TizianaTerenzi.Common.Data.csproj", "Common/TizianaTerenzi.Common.Data/"]
COPY ["Common/TizianaTerenzi.Common.Services/TizianaTerenzi.Common.Services.csproj", "Common/TizianaTerenzi.Common.Services/"]
COPY ["Notifications/Data/TizianaTerenzi.Notifications.Data.Models/TizianaTerenzi.Notifications.Data.Models.csproj", "Notifications/Data/TizianaTerenzi.Notifications.Data.Models/"]
COPY ["Notifications/Services/TizianaTerenzi.Notifications.Services.Data/TizianaTerenzi.Notifications.Services.Data.csproj", "Notifications/Services/TizianaTerenzi.Notifications.Services.Data/"]
COPY ["Common/TizianaTerenzi.Common.Services.Mapping/TizianaTerenzi.Common.Services.Mapping.csproj", "Common/TizianaTerenzi.Common.Services.Mapping/"]
COPY ["Common/TizianaTerenzi.Common.Web.Infrastructure/TizianaTerenzi.Common.Web.Infrastructure.csproj", "Common/TizianaTerenzi.Common.Web.Infrastructure/"]
COPY ["Notifications/Web/TizianaTerenzi.Notifications.Web.Models/TizianaTerenzi.Notifications.Web.Models.csproj", "Notifications/Web/TizianaTerenzi.Notifications.Web.Models/"]
COPY ["Notifications/Web/TizianaTerenzi.Notifications.Web.Infrastructure/TizianaTerenzi.Notifications.Web.Infrastructure.csproj", "Notifications/Web/TizianaTerenzi.Notifications.Web.Infrastructure/"]
RUN dotnet restore "./Notifications/Web/TizianaTerenzi.Notifications.Web/TizianaTerenzi.Notifications.Web.csproj"
COPY . .
WORKDIR "/src/Notifications/Web/TizianaTerenzi.Notifications.Web"
RUN dotnet build "./TizianaTerenzi.Notifications.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Debug
RUN dotnet publish "./TizianaTerenzi.Notifications.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TizianaTerenzi.Notifications.Web.dll"]