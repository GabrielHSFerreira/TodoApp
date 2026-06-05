# TodoApp — repo guide for agents

## Stack
- **.NET 10** (`net10.0`), ASP.NET Core Minimal APIs, **.NET Aspire** orchestrator
- **PostgreSQL** via Npgsql Entity Framework Core
- **xUnit v3** (`.v3` packages, `TestContext.Current.CancellationToken`, `[assembly: AssemblyFixture]`)
- **Testcontainers.PostgreSql** (requires Docker), **WebApplicationFactory**
- **FluentValidation**, **Serilog**, **Scalar** (OpenAPI UI), **BetterOutcome**

## Solution structure (`src/`)
| Project | Path | Role |
|---|---|---|
| `TodoApp.AppHost` | `src/TodoApp.AppHost` | Aspire host — **always run this**, not WebApi directly |
| `TodoApp.WebApi` | `src/TodoApp.WebApi` | Minimal API endpoints, feature-folder layout |
| `TodoApp.ServiceDefaults` | `src/TodoApp.ServiceDefaults` | Shared Aspire defaults (OpenTelemetry, health checks, resilience) |
| `TodoApp.IntegrationTests` | `src/TodoApp.IntegrationTests` | xUnit v3 integration tests (Docker + Testcontainers) |

## Essential commands
```powershell
dotnet restore --nologo
dotnet build -c Release --no-restore --nologo
dotnet test -c Release --no-restore --no-build -v normal --nologo
```
CI runs in that exact order (`restore → build → test`). Release config always.

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
- Tests **require Docker** (Testcontainers pulls `postgres:18.4-alpine`)
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

## AppHost
- `Program.cs` defines `postgres:18.4-alpine` container via `AddPostgres`, creates db `todoapp-db`, references `TodoApp_WebApi` project
- `dotnet run` **must** be on `TodoApp.AppHost` — Aspire starts PostgreSQL and the WebApi automatically
