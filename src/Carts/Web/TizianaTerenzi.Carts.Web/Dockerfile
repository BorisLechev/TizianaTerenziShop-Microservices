#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Debug
WORKDIR /src
COPY ["Carts/Web/TizianaTerenzi.Carts.Web/TizianaTerenzi.Carts.Web.csproj", "Carts/Web/TizianaTerenzi.Carts.Web/"]
COPY ["Common/TizianaTerenzi.Common.Messages/TizianaTerenzi.Common.Messages.csproj", "Common/TizianaTerenzi.Common.Messages/"]
COPY ["Common/TizianaTerenzi.Common.Web/TizianaTerenzi.Common.Web.csproj", "Common/TizianaTerenzi.Common.Web/"]
COPY ["Common/TizianaTerenzi.Common/TizianaTerenzi.Common.csproj", "Common/TizianaTerenzi.Common/"]
COPY ["Common/TizianaTerenzi.Common.Web.Infrastructure/TizianaTerenzi.Common.Web.Infrastructure.csproj", "Common/TizianaTerenzi.Common.Web.Infrastructure/"]
COPY ["Common/TizianaTerenzi.Common.Data/TizianaTerenzi.Common.Data.csproj", "Common/TizianaTerenzi.Common.Data/"]
COPY ["Common/TizianaTerenzi.Common.Services/TizianaTerenzi.Common.Services.csproj", "Common/TizianaTerenzi.Common.Services/"]
COPY ["Common/TizianaTerenzi.Common.Services.Mapping/TizianaTerenzi.Common.Services.Mapping.csproj", "Common/TizianaTerenzi.Common.Services.Mapping/"]
COPY ["Carts/Data/TizianaTerenzi.Carts.Data/TizianaTerenzi.Carts.Data.csproj", "Carts/Data/TizianaTerenzi.Carts.Data/"]
COPY ["Carts/Data/TizianaTerenzi.Carts.Data.Models/TizianaTerenzi.Carts.Data.Models.csproj", "Carts/Data/TizianaTerenzi.Carts.Data.Models/"]
COPY ["Carts/Services/TizianaTerenzi.Carts.Services.Data/TizianaTerenzi.Carts.Services.Data.csproj", "Carts/Services/TizianaTerenzi.Carts.Services.Data/"]
COPY ["Carts/Web/TizianaTerenzi.Carts.Web.Models/TizianaTerenzi.Carts.Web.Models.csproj", "Carts/Web/TizianaTerenzi.Carts.Web.Models/"]
RUN dotnet restore "./Carts/Web/TizianaTerenzi.Carts.Web/TizianaTerenzi.Carts.Web.csproj"
COPY . .
WORKDIR "/src/Carts/Web/TizianaTerenzi.Carts.Web"
RUN dotnet build "./TizianaTerenzi.Carts.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Debug
RUN dotnet publish "./TizianaTerenzi.Carts.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TizianaTerenzi.Carts.Web.dll"]