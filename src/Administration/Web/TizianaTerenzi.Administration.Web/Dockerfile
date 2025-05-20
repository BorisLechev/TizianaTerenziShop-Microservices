#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Debug
WORKDIR /src
COPY ["Administration/Web/TizianaTerenzi.Administration.Web/TizianaTerenzi.Administration.Web.csproj", "Administration/Web/TizianaTerenzi.Administration.Web/"]
COPY ["Common/TizianaTerenzi.Common.Messages/TizianaTerenzi.Common.Messages.csproj", "Common/TizianaTerenzi.Common.Messages/"]
COPY ["Common/TizianaTerenzi.Common.Web/TizianaTerenzi.Common.Web.csproj", "Common/TizianaTerenzi.Common.Web/"]
COPY ["Common/TizianaTerenzi.Common/TizianaTerenzi.Common.csproj", "Common/TizianaTerenzi.Common/"]
COPY ["Common/TizianaTerenzi.Common.Services.Messaging/TizianaTerenzi.Common.Services.Messaging.csproj", "Common/TizianaTerenzi.Common.Services.Messaging/"]
COPY ["Common/TizianaTerenzi.Common.Web.Infrastructure/TizianaTerenzi.Common.Web.Infrastructure.csproj", "Common/TizianaTerenzi.Common.Web.Infrastructure/"]
COPY ["Common/TizianaTerenzi.Common.Data/TizianaTerenzi.Common.Data.csproj", "Common/TizianaTerenzi.Common.Data/"]
COPY ["Common/TizianaTerenzi.Common.Services/TizianaTerenzi.Common.Services.csproj", "Common/TizianaTerenzi.Common.Services/"]
COPY ["Common/TizianaTerenzi.Common.Services.Mapping/TizianaTerenzi.Common.Services.Mapping.csproj", "Common/TizianaTerenzi.Common.Services.Mapping/"]
COPY ["Administration/Data/TizianaTerenzi.Administration.Data/TizianaTerenzi.Administration.Data.csproj", "Administration/Data/TizianaTerenzi.Administration.Data/"]
COPY ["Administration/Data/TizianaTerenzi.Administration.Data.Models/TizianaTerenzi.Administration.Data.Models.csproj", "Administration/Data/TizianaTerenzi.Administration.Data.Models/"]
COPY ["Administration/Services/TizianaTerenzi.Administration.Services.Data/TizianaTerenzi.Administration.Services.Data.csproj", "Administration/Services/TizianaTerenzi.Administration.Services.Data/"]
COPY ["Administration/Web/TizianaTerenzi.Administration.Web.Models/TizianaTerenzi.Administration.Web.Models.csproj", "Administration/Web/TizianaTerenzi.Administration.Web.Models/"]
COPY ["Administration/Services/TizianaTerenzi.Administration.Services/TizianaTerenzi.Administration.Services.csproj", "Administration/Services/TizianaTerenzi.Administration.Services/"]
COPY ["Administration/Services/TizianaTerenzi.Administration.Services.Models/TizianaTerenzi.Administration.Services.Models.csproj", "Administration/Services/TizianaTerenzi.Administration.Services.Models/"]
RUN dotnet restore "./Administration/Web/TizianaTerenzi.Administration.Web/TizianaTerenzi.Administration.Web.csproj"
COPY . .
WORKDIR "/src/Administration/Web/TizianaTerenzi.Administration.Web"
RUN dotnet build "./TizianaTerenzi.Administration.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Debug
RUN dotnet publish "./TizianaTerenzi.Administration.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TizianaTerenzi.Administration.Web.dll"]