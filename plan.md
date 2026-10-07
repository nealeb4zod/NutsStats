# Project Reorganization and Scaffold for API + Blazor (NET 10)

This plan reorganizes the existing solution into a layered, maintainable .NET 10 codebase prepared for an ASP.NET Core API and a Blazor WebAssembly frontend. It creates a repository-level `plan.md`, scaffolds `src/` and `tests/` projects (Domain/Application/Infrastructure/Api/Scraper/Web), moves EF Core artifacts into `Infrastructure`, and adds CI and local dev tooling (Docker, docker-compose, GitHub Actions). The intent is a scaffolding step only — no feature implementation beyond wiring and configuration.

## Goals and priorities

- Immediate/low-effort wins:
  - enable analyzers & nullable
  - add README
  - add CI build + Dockerfile
  - add Swagger and health checks to the future API project
- Architectural work (medium effort):
  - adopt a layered/clean architecture
  - separate scraping into a background worker
  - move EF migrations into an Infrastructure project
- Future/ops (higher effort):
  - CI/CD, container orchestration, monitoring/telemetry, auth, rate limiting, caching

## Recommended project layout

- `src/`
  - `NutsStats.Domain` (entities, value objects, domain exceptions) — no external deps
  - `NutsStats.Application` (use-cases, DTOs, interfaces, validation) — depends on Domain
  - `NutsStats.Infrastructure` (EF Core, repository implementations, migrations, third-party integrations) — depends on Application
  - `NutsStats.Scraper` (worker / `IHostedService` pattern for scheduled scrapes) — depends on Infrastructure
  - `NutsStats.Api` (ASP.NET Core Web API) — depends on Application + Infrastructure
  - `NutsStats.Web` (Blazor WebAssembly) — frontend client
- `tests/`
  - `NutsStats.UnitTests`
  - `NutsStats.IntegrationTests`

## Code and configuration hygiene

- Enable nullable reference types (`<Nullable>enable</Nullable>`) across projects.
- Add `.editorconfig` and consistent C# rules.
- Enable Roslyn analyzers and treat warnings as build warnings or errors in CI.
- Use `global usings` and implicit usings where appropriate for .NET 10.
- Consolidate secrets: user-secrets for local dev, environment variables in containers, Key Vault in the cloud.

## EF Core and database guidance

- Keep EF Core `DbContext` and migrations in Infrastructure to avoid coupling the domain model to EF.
- The existing `DesignTimeScraperDbContextFactory` should be preserved but moved to `NutsStats.Infrastructure` so design-time tooling resolves the correct context.
- Run migrations in CI or during deployment; use versioned migration scripts for production.
- Add indexes for frequently queried columns and optimize slow read queries with SQL tuning and `AsNoTracking` when appropriate.

## Separation of concerns and mapping

- Use DTOs for API contracts and never return EF entities directly from controllers.
- Use `AutoMapper` or `Mapster` in the Application layer for mapping.
- Add `FluentValidation` for request validation.

## Scraper and background work

- Move scraper logic into `NutsStats.Scraper` as a hosted background service or worker scheduled with `PeriodicTimer`.
- Keep ingestion idempotent (dedupe on natural keys), log progress, and record metrics like last run, duration, and processed counts.
- If scraping grows in scale, consider a message queue (RabbitMQ, Azure Service Bus) to decouple ingestion from processing.

## API considerations

- Add OpenAPI/Swagger early.
- Add API versioning.
- Add health checks for startup, database, and downstream services; expose readiness/liveness endpoints.
- Add a CORS policy to allow the frontend origin(s).
- Use structured logging (Serilog) and correlation IDs for requests.
- Add rate limiting and request throttling for public APIs.

## Security and auth

- Plan authentication as JWT or external provider-based depending on requirements.
- Validate all input and use parameterized queries / ORM protections.
- Protect connection strings and tokens; enforce HTTPS.

## Observability and monitoring

- Instrument with OpenTelemetry or Azure Monitor / Application Insights.
- Expose Prometheus metrics if Kubernetes or other orchestrated environments are planned.
- Add error tracking (Sentry or Application Insights) for runtime exceptions.

## Testing guidance

- Add unit tests for domain and application logic.
- Add integration tests running against an ephemeral SQL Server or containerized DB.
- Add end-to-end tests for scrape -> persist -> API -> frontend flows.

## DevOps and packaging

- Add Dockerfile(s) for the API and scraper.
- Add `docker-compose.dev.yml` for local dev (DB, API, scraper, optional Redis).
- Add GitHub Actions for build/test/lint and artifact generation.
- Add deployment strategy for chosen host (App Service, AKS, or another runtime) with migration steps.

## Frontend guidance

- Preferred initial stack: Blazor WebAssembly for a .NET-first frontend.
- Use an API-first approach with typed HTTP clients or generated clients from OpenAPI.
- Implement auth flows early (login and refresh) and align CORS settings with the API.

## Performance and scaling

- Cache expensive reads with response caching or Redis.
- Use connection pooling and optimize EF queries with `AsNoTracking` when reading data.
- Add index plans and profiling for slow queries.
- Move heavy processing to background workers or queue-based flows.

## Documentation and repository hygiene

- Add README with architecture overview, local run instructions, migrations, local dev with docker-compose, and test instructions.
- Add `CONTRIBUTING.md`, issue/PR templates, a CODE_OF_CONDUCT, and repository-level ownership files if the project grows.
- Add Dependabot for dependency scanning and update cadence.
- Add a LICENSE file if relevant.

## Execution phases

### Phase A — Repo hygiene and baseline
1. Add `plan.md` and README.
2. Add `.editorconfig` and `Directory.Build.props`.
3. Add GitHub Actions CI workflow.
4. Add Dockerfile(s) and docker-compose dev file.

### Phase B — Solution architecture and project scaffolding
1. Create `src/` and `tests/` folders.
2. Add `NutsStats.Domain`, `NutsStats.Application`, `NutsStats.Infrastructure`, `NutsStats.Api`, `NutsStats.Scraper`, `NutsStats.Web` projects.
3. Update `NutsStats.slnx` with the new projects.

### Phase C — Move existing scraper code into Infrastructure
1. Move `ScraperDbContext`, `Entities`, and migration files into `NutsStats.Infrastructure`.
2. Update namespaces and project references.
3. Ensure the design-time factory resolves connection strings from configuration.

### Phase D — API and scraper wiring
1. Add minimal API startup and Swagger.
2. Add health checks and CORS.
3. Add worker pattern to the scraper.

### Phase E — Frontend and validation
1. Scaffold Blazor WebAssembly frontend.
2. Add API client abstraction.
3. Add unit/integration tests.

### Phase F — Production readiness
1. Add deployment pipeline.
2. Add monitoring and auth placeholders.
3. Profile performance, add caching and rate limiting as needed.

## Acceptance criteria

- `plan.md` exists at the repo root.
- Projects are added under `/src` and `/tests` and included in the solution.
- EF Core `DbContext` and `DesignTime` factory live in `NutsStats.Infrastructure`.
- API project exposes Swagger and health checks.
- Scraper project becomes a hosted background worker.
- Blazor frontend scaffold exists.
- `.editorconfig` and directory-level build settings are present.
- CI workflow builds and tests the solution.
- Docker/dev-compose files exist.

## Implementation notes for this repo

The current repository already contains a usable EF model and migrations in `NutsStats.Scraper`. This plan preserves and moves them into `NutsStats.Infrastructure` instead of discarding them, because the migration history already exists and is valuable for future database evolution.

The design-time factory (
`NutsStats.Scraper/DesignTimeScraperDbContextFactory.cs`
) specifically reads `appsettings.json` from parent directories and then calls `UseSqlServer()`. This functionality will be preserved but relocated to the Infrastructure project so the EF tools and the app resolve the same context correctly.
