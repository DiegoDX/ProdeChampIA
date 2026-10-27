# Testing

Run from the repository root:

```powershell
dotnet build ProdeChamp.slnx
dotnet test ProdeChamp.slnx
```

`ProdeChamp.UnitTests` references Domain and Application. `ProdeChamp.IntegrationTests` references API and Infrastructure. Both use xUnit, Microsoft.NET.Test.Sdk and the Visual Studio test adapter; no mocking, coverage, database or test-host package is added.

Sprint 0 originally contained no tests. PRODE-001 now has 44 Domain unit tests. The original 34 in `PredictionPointsCalculatorTests` cover exact scores, correct winners, draws, incorrect outcomes, integer boundaries and independent repeated calls; this file remains unchanged. `PredictionPointsCalculatorValidationTests` adds 8 cases for -1/int.MinValue in each argument, asserting `ArgumentOutOfRangeException` and its parameter name, and 2 cases ensuring exact negative scores are rejected before scoring. The validation RED run passed the original 34 and failed the 10 new cases because no exception was thrown; the GREEN and full regression runs passed all 44 with zero failures or skips. See the [engineering report](../engineering-reports/PRODE-001-engineering-report.md) for commands, approval and results.

The integration project still contains no tests. Zero tests discovered there establishes no integration coverage. Local tests do not establish CI status.

For approved behavior changes, follow the red/green workflow in `AGENTS.md`: write a meaningful failing test, confirm the expected failure, implement the behavior and rerun relevant tests. Add integration fixtures when a story actually needs them.

## Continuous integration

Use the [ci skill](../../.agents/skills/ci/SKILL.md) to configure or verify the existing [.NET CI workflow](../../.github/workflows/ci.yml). It restores and builds in Release, runs unit and integration projects separately, requires at least one executed unit test, and retains TRX evidence. Integration currently has zero tests; this is reported separately.

The local workflow configuration covers pushes to branches, PRs targeting `main` and manual execution after publication. Current local verification passed 44 unit tests and the zero-execution guard's negative check. Remote execution of these changes is pending; prior successful `main` runs do not verify this working tree. See the [CI report](../stories/PRODE-001/ci-report.md) for exact results and limitations. Deployment is outside this Skill and workflow.
