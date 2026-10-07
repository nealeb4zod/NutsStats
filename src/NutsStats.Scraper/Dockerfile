FROM mcr.microsoft.com/dotnet/runtime:10.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["src/NutsStats.Scraper/NutsStats.Scraper.csproj", "src/NutsStats.Scraper/"]
COPY ["src/NutsStats.Application/NutsStats.Application.csproj", "src/NutsStats.Application/"]
COPY ["src/NutsStats.Domain/NutsStats.Domain.csproj", "src/NutsStats.Domain/"]
COPY ["src/NutsStats.Infrastructure/NutsStats.Infrastructure.csproj", "src/NutsStats.Infrastructure/"]
RUN dotnet restore "src/NutsStats.Scraper/NutsStats.Scraper.csproj"
COPY . .
WORKDIR "/src/src/NutsStats.Scraper"
RUN dotnet publish "NutsStats.Scraper.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "NutsStats.Scraper.dll"]
