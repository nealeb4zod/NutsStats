# NutsStats

NutsStats is a .NET 10 solution that will evolve into a layered application with:

- a background data scraper
- an ASP.NET Core API
- a Blazor WebAssembly frontend
- EF Core persistence via an Infrastructure project

## Architecture overview

The solution is structured to follow a clean architecture approach:

- `src/NutsStats.Domain` — domain model, entities, and business rules
- `src/NutsStats.Application` — use cases, contracts, DTOs, validation
- `src/NutsStats.Infrastructure` — EF Core, repositories, external integrations, migrations
- `src/NutsStats.Scraper` — background worker for scheduled scraping
- `src/NutsStats.Api` — API surface for clients and frontend consumption
- `src/NutsStats.Web` — Blazor frontend

## Repository status

This repository is currently being scaffolded from the existing scraper-based starting point. The current scraper already contains EF Core model files and migrations, and those will be moved into the Infrastructure layer as the solution is reorganized.

## Local development

### Prerequisites

- .NET 10 SDK
- SQL Server (or Dockerized SQL Server for local runs)
- Docker Desktop (optional but recommended for local environment setup)

### Restore and build

```bash
dotnet restore
dotnet build
```

### Run migrations

```bash
dotnet ef database update -p src/NutsStats.Infrastructure -s src/NutsStats.Api
```

### Docker-based local environment

```bash
docker compose -f docker-compose.dev.yml up --build
```

## Notes

- The solution is being gradually refactored toward a clean layered architecture.
- All new projects target .NET 10.
- EF Core migrations live in the Infrastructure layer.
- The API and frontend are planned to consume the same domain contracts and DTOs.

For the full roadmap and implementation phases, see [plan.md](./plan.md).
