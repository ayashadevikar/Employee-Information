# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copying the backend .csproj file
COPY ["server/FullStackCrud.Server/FullStackCrud.Server.csproj", "server/FullStackCrud.Server/"]

# Restore dependencies for backend project
RUN dotnet restore "./server/FullStackCrud.Server/FullStackCrud.Server.csproj"

# Copy the entire backend code (since we're focusing on the backend)
COPY ./server/FullStackCrud.Server ./server/FullStackCrud.Server/

# Set the working directory for building
WORKDIR "/src/server/FullStackCrud.Server"
RUN dotnet build "./FullStackCrud.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./server/FullStackCrud.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FullStackCrud.Server.dll"]
