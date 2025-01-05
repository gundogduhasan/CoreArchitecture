#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CoreArchitecture.Api/CoreArchitecture.Api.csproj", "CoreArchitecture.Api/"]
COPY ["CoreArchitecture.Infrastructure/CoreArchitecture.Infrastructure.csproj", "CoreArchitecture.Infrastructure/"]
COPY ["CoreArchitecture.Abstractions/CoreArchitecture.Abstractions.csproj", "CoreArchitecture.Abstractions/"]
COPY ["CoreArchitecture.Common/CoreArchitecture.Common.csproj", "CoreArchitecture.Common/"]
COPY ["CoreArchitecture.Business/CoreArchitecture.Business.csproj", "CoreArchitecture.Business/"]
COPY ["CoreArchitecture.Data/CoreArchitecture.Data.csproj", "CoreArchitecture.Data/"]
COPY ["CoreArchitecture.DataAccess/CoreArchitecture.DataAccess.csproj", "CoreArchitecture.DataAccess/"]
RUN dotnet restore "./CoreArchitecture.Api/CoreArchitecture.Api.csproj"
COPY . .
WORKDIR "/src/CoreArchitecture.Api"
RUN dotnet build "./CoreArchitecture.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CoreArchitecture.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CoreArchitecture.Api.dll"]

