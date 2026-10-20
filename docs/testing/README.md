# Testing

Run from the repository root:

```powershell
dotnet build ProdeChamp.slnx
dotnet test ProdeChamp.slnx
```

`ProdeChamp.UnitTests` references Domain and Application. `ProdeChamp.IntegrationTests` references API and Infrastructure. Both use xUnit, Microsoft.NET.Test.Sdk and the Visual Studio test adapter; no mocking, coverage, database or test-host package is added.

The projects intentionally contain no tests in Sprint 0. Expect zero tests discovered in both assemblies. This verifies build and runner setup only, not business correctness or working external integrations.

For approved behavior changes, follow the red/green workflow in `AGENTS.md`: write a meaningful failing test, confirm the expected failure, implement the behavior and rerun relevant tests. Add integration fixtures when a story actually needs them.
