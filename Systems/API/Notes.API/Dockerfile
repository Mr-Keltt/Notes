#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Systems/API/Notes.API/Notes.API.csproj", "Systems/API/Notes.API/"]
COPY ["Application/Services/Notes.Services.Logger/Notes.Services.Logger.csproj", "Application/Services/Notes.Services.Logger/"]
COPY ["Application/Services/Notes.Services.Settings/Notes.Services.Settings.csproj", "Application/Services/Notes.Services.Settings/"]
COPY ["Shared/Notes.Common/Notes.Common.csproj", "Shared/Notes.Common/"]
COPY ["Infrastructure/Data/Migrations/Notes.Context.Migrations.PgSql/Notes.Context.Migrations.PgSql.csproj", "Infrastructure/Data/Migrations/Notes.Context.Migrations.PgSql/"]
COPY ["Infrastructure/Repositories/Notes.Entities/Notes.Entities.csproj", "Infrastructure/Repositories/Notes.Entities/"]
COPY ["Infrastructure/Data/Notes.Context/Notes.Context.csproj", "Infrastructure/Data/Notes.Context/"]
RUN dotnet restore "./Systems/API/Notes.API/Notes.API.csproj"
COPY . .
WORKDIR "/src/Systems/API/Notes.API"
RUN dotnet build "./Notes.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Notes.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Notes.API.dll"]