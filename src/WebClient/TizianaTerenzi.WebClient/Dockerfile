#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Debug
WORKDIR /src
COPY ["WebClient/TizianaTerenzi.WebClient/TizianaTerenzi.WebClient.csproj", "WebClient/TizianaTerenzi.WebClient/"]
COPY ["Common/TizianaTerenzi.Common.Web.Infrastructure/TizianaTerenzi.Common.Web.Infrastructure.csproj", "Common/TizianaTerenzi.Common.Web.Infrastructure/"]
COPY ["Common/TizianaTerenzi.Common.Data/TizianaTerenzi.Common.Data.csproj", "Common/TizianaTerenzi.Common.Data/"]
COPY ["Common/TizianaTerenzi.Common.Services/TizianaTerenzi.Common.Services.csproj", "Common/TizianaTerenzi.Common.Services/"]
COPY ["Common/TizianaTerenzi.Common/TizianaTerenzi.Common.csproj", "Common/TizianaTerenzi.Common/"]
COPY ["Common/TizianaTerenzi.Common.Services.Mapping/TizianaTerenzi.Common.Services.Mapping.csproj", "Common/TizianaTerenzi.Common.Services.Mapping/"]
COPY ["WebClient/Services/TizianaTerenzi.WebClient.Services/TizianaTerenzi.WebClient.Services.csproj", "WebClient/Services/TizianaTerenzi.WebClient.Services/"]
COPY ["WebClient/TizianaTerenzi.WebClient.ViewModels/TizianaTerenzi.WebClient.ViewModels.csproj", "WebClient/TizianaTerenzi.WebClient.ViewModels/"]
COPY ["Common/TizianaTerenzi.Common.Web/TizianaTerenzi.Common.Web.csproj", "Common/TizianaTerenzi.Common.Web/"]
COPY ["WebClient/TizianaTerenzi.WebClient.Infrastructure/TizianaTerenzi.WebClient.Infrastructure.csproj", "WebClient/TizianaTerenzi.WebClient.Infrastructure/"]
RUN dotnet restore "./WebClient/TizianaTerenzi.WebClient/TizianaTerenzi.WebClient.csproj"
COPY . .
WORKDIR "/src/WebClient/TizianaTerenzi.WebClient"
RUN dotnet build "./TizianaTerenzi.WebClient.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Debug
RUN dotnet publish "./TizianaTerenzi.WebClient.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TizianaTerenzi.WebClient.dll"]