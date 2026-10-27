# ProdeChamp Development Factory

ProdeChamp is a football prediction platform and an AI-assisted software engineering portfolio. Sprint 0 establishes the repository structure only.

## Prerequisites and commands

Install a stable .NET 10 SDK. `global.json` allows installed .NET 10 feature bands; this scaffold was generated with SDK 10.0.401. NuGet access or a populated package cache is required for the xUnit tooling.

Run from this directory:

```powershell
dotnet build ProdeChamp.slnx
dotnet test ProdeChamp.slnx
dotnet run --project src/ProdeChamp.API
```

The API starts an empty ASP.NET Core host with no application endpoints. PRODE-001 has a Domain scoring calculator and 44 passing unit tests: 34 for scoring and 10 for rejecting negative goals with `ArgumentOutOfRangeException` identifying the parameter. The integration test project is empty. Independent code review returned APPROVED, and GitHub CI passed for the published implementation. [PR #3](https://github.com/DiegoDX/ProdeChampIA/pull/3) awaits final human review; see the [implementation report](docs/engineering-reports/PRODE-001-engineering-report.md) and [publication/CI evidence](docs/stories/PRODE-001/ci-report.md).

## Repository layout

```text
.agents/skills/                 Eleven Codex engineering workflow skills
docs/                          Product, architecture and engineering documentation
src/
  ProdeChamp.Domain/           Prediction points calculator
  ProdeChamp.Application/      Use cases and ports (empty)
  ProdeChamp.Infrastructure/   External adapters (empty)
  ProdeChamp.API/              ASP.NET Core composition root
tests/
  ProdeChamp.UnitTests/        xUnit project for Domain and Application
  ProdeChamp.IntegrationTests/ xUnit project for API and Infrastructure
ProdeChamp.slnx                .NET 10 solution
```

## Architecture and workflow

Project references point inward: Application references Domain; Infrastructure references Application and Domain; API references Application and Infrastructure for composition. Domain has no project or package dependencies. Production projects have no NuGet package references. Test projects use only Microsoft.NET.Test.Sdk, xUnit and its Visual Studio adapter.

Read [AGENTS.md](AGENTS.md) before working. Use [docs/](docs/README.md) as the source of truth for product and architecture decisions. The [architecture overview](docs/architecture/README.md) describes the reference graph. The [Sprint 0 report](docs/engineering-reports/sprint-0.md) records validation results and the complete source-file inventory.

The eleven skills cover story refinement, architecture design and review, test design, backend and frontend implementation, TDD implementation, code review, CI, engineering reporting and interview preparation. Each lives in `.agents/skills/<skill-name>/SKILL.md`. Use the [ci skill](.agents/skills/ci/SKILL.md) to configure or verify GitHub Actions builds and test evidence; deployment is a separate future scope.

SQL Server and Angular are planned technologies; neither is configured in Sprint 0. EF Core, MediatR, Redis, Docker and external AI integrations are outside this scaffold.
