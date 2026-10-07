# TodoApp — repo guide for agents

## Stack
- **.NET 10** (`net10.0`), ASP.NET Core Minimal APIs, **.NET Aspire** orchestrator
- **PostgreSQL** via Npgsql Entity Framework Core
- **xUnit v3** (`.v3` packages, `TestContext.Current.CancellationToken`, `[assembly: AssemblyFixture]`)
- **Testcontainers.PostgreSql** (requires **Docker or Podman**), **WebApplicationFactory**
- **FluentValidation**, **Serilog**, **Scalar** (OpenAPI UI), **BetterOutcome**

## Solution structure (`src/`)
| Project | Path | Role |
|---|---|---|
| `TodoApp.AppHost` | `src/TodoApp.AppHost` | Aspire host — **always run this**, not WebApi directly |
| `TodoApp.WebApi` | `src/TodoApp.WebApi` | Minimal API endpoints, feature-folder layout |
| `TodoApp.ServiceDefaults` | `src/TodoApp.ServiceDefaults` | Shared Aspire defaults (OpenTelemetry, health checks, resilience) |
| `TodoApp.IntegrationTests` | `src/TodoApp.IntegrationTests` | xUnit v3 integration tests (Docker/Podman + Testcontainers) |

## Essential commands
```powershell
dotnet restore --nologo
dotnet build -c Release --no-restore --nologo
dotnet test -c Release --no-restore --no-build -v normal --nologo
```
CI runs in that exact order (`restore → build → test`). Release config always.

## Run the app
```powershell
dotnet run --project src/TodoApp.AppHost/TodoApp.AppHost.csproj
```
Aspire starts PostgreSQL (`postgres:18.4-alpine`, db `todoapp-db`) and WebApi automatically using **Docker or Podman**.
- OpenAPI/Scalar at `http://localhost:<port>/scalar/v1` (development only)
- Health checks at `/health` and `/alive` (from `ServiceDefaults`)

## Endpoint pattern
Each feature lives in `Features/{FeatureName}/` and implements `IEndpoint`:
```csharp
public class XxxEndpoint : IEndpoint
{
    public void Register(IEndpointRouteBuilder builder)
    {
        builder.Map{Verb}("/todos", Handler).WithTags(EndpointTags.Todos);
    }
}
```
Registered manually in `Program.cs:49-52`. **Not** using `MapXXX` chaining — each endpoint is its own class.

## Testing quirks
- Tests **require Docker or Podman** (Testcontainers pulls `postgres:18.4-alpine`)
- `WebApiFactory` is an `[AssemblyFixture]` (xUnit v3) — shared across all test classes
- Use `TestContext.Current.CancellationToken` (not `CancellationToken.None`)
- `HttpExtensions.GetResponseBody<T>()` does `JsonSerializer.DeserializeAsync` with case-insensitive options
- `TodosHelper` provides `TodoExists`, `CreateTodo`, `FindTodo` on `TodoContext`
- Postgres connection string injected via `builder.UseSetting("ConnectionStrings:Database", ...)` in the factory

## Key conventions
- `Nullable` and `ImplicitUsings` enabled throughout
- `TodoContext` exposes `DbSet<Todo> Todos` — no custom config beyond that
- `Todo` entity: `Id` (init), `CreatedAt` (init), `UpdatedAt`, `Title`, `Description`, `Status` (enum: `Pending`, `InProgress`, `Blocked`, `Done`)
- Validation: FluentValidation `AbstractValidator<T>` per command, registered via `AddValidatorsFromAssemblyContaining<Program>()`
- `appsettings.json` has placeholder `ConnectionStrings:Database` — real value comes from Aspire or user secrets
- Database seeded via `context.Database.EnsureCreated()` in `Program.cs:57-63` (no migrations)

## Adding a new endpoint
1. Create `Features/{FeatureName}/{FeatureName}Endpoint.cs` implementing `IEndpoint`
2. Create `{FeatureName}Command.cs` (request) + validator if needed
3. Register in `Program.cs` (manual `new XxxEndpoint().Register(app)`)

## Integration test structure
```
src/TodoApp.IntegrationTests/
├── Fixtures/WebApiFactory.cs      # AssemblyFixture, starts PostgreSQL container
├── Extensions/HttpExtensions.cs   # GetResponseBody<T>() helper
└── Todos/
    ├── TodosHelper.cs             # DB helpers (TodoExists, CreateTodo, FindTodo)
    └── {Create,Get,Update,Delete}TodoTests.cs
```

## Gotchas
- **No `dotnet watch`** on WebApi directly — Aspire handles hot reload via AppHost
- **No EF migrations** — uses `EnsureCreated()` for schema
- **UserSecretsId** in WebApi csproj (`TodoIntegrationTestsWebApiLocalSecrets`) for local dev connection strings
- **BetterOutcome** used for result types (not standard `Result<T>`)
- **Testcontainers auto-detects** Docker or Podman — ensure one is running