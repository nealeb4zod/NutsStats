FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["src/NutsStats.Api/NutsStats.Api.csproj", "src/NutsStats.Api/"]
COPY ["src/NutsStats.Application/NutsStats.Application.csproj", "src/NutsStats.Application/"]
COPY ["src/NutsStats.Domain/NutsStats.Domain.csproj", "src/NutsStats.Domain/"]
COPY ["src/NutsStats.Infrastructure/NutsStats.Infrastructure.csproj", "src/NutsStats.Infrastructure/"]
RUN dotnet restore "src/NutsStats.Api/NutsStats.Api.csproj"
COPY . .
WORKDIR "/src/src/NutsStats.Api"
RUN dotnet publish "NutsStats.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "NutsStats.Api.dll"]
