# Base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src


COPY ["FullStackCrud.Server/FullStackCrud.Server.csproj", "FullStackCrud.Server/"]
RUN dotnet restore "FullStackCrud.Server/FullStackCrud.Server.csproj"


COPY ./FullStackCrud.Server ./FullStackCrud.Server/

WORKDIR "/src/FullStackCrud.Server"
RUN dotnet build "FullStackCrud.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "FullStackCrud.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FullStackCrud.Server.dll"]

