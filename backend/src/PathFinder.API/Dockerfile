# Base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /app

# Copy csproj and restore
COPY *sln .
COPY src/PathFinder.API/PathFinder.API.csproj src/PathFinder.API/
COPY src/PathFinder.Application/PathFinder.Application.csproj src/PathFinder.Application/
COPY src/PathFinder.Domain/PathFinder.Domain.csproj src/PathFinder.Domain/
COPY src/PathFinder.Infrastructure/PathFinder.Infrastructure.csproj src/PathFinder.Infrastructure/

RUN dotnet restore src/PathFinder.API/PathFinder.API.csproj

COPY src ./src

# Build
WORKDIR /app/src/PathFinder.API
RUN dotnet build PathFinder.API.csproj -c $BUILD_CONFIGURATION -o /app/build

# Publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish PathFinder.API.csproj -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PathFinder.API.dll"]