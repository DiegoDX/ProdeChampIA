# Testing

## Prediction contracts correction - 2026-11-19

`PredictionCanEditTests` replaces PredictionEditCutoffTests with 22 cases: five strict-boundary rows including adjacent ticks, six nonzero kickoff UTC offsets, six date extremes/year-crossing rows, public server-time behavior, repeated attempts with an advancing clock, single clock capture, prediction nonmutation and an internal null-TimeProvider guard. A test-local TimeProvider supplies deterministic instants without sleeps or new packages. The public method still takes only scheduled kickoff and uses TimeProvider.System.

`PredictionTests` now has 41 cases using Guid match IDs. Null-Match rejection becomes Guid.Empty rejection; four Match-status construction rows become two nonempty Guid boundary rows. Duplicate-pair tests retain score-independent rejection without obsolete object-identity fixtures. All participant, goal, registration UTC/date, context, duplicate and nonmutation checks remain. Status cannot influence APIs that no longer accept Match/status.

Observed behavioral RED: 41 factory failures and 22 cutoff failures against separately recorded throwing shapes. Factory GREEN passed 41 and combined prediction GREEN passed 63. Current full verification and source identity are recorded in the [correction evidence](../engineering-reports/prediction-contracts-implementation-evidence.md) and [engineering report](../engineering-reports/prediction-contracts-engineering-report.md). Earlier PRODE-003/004 counts, reviews and CI below/under story folders apply to historical source; they are not evidence for the corrected contracts. Saving integration remains outside this evaluation-only rule.

Run from the repository root:

```powershell
dotnet build ProdeChamp.slnx
dotnet test ProdeChamp.slnx
```

`ProdeChamp.UnitTests` references Domain and Application. `ProdeChamp.IntegrationTests` references API and Infrastructure. Both use xUnit, Microsoft.NET.Test.Sdk and the Visual Studio test adapter; no mocking, coverage, database or test-host package is added.

Sprint 0 originally contained no tests. PRODE-001 now has 44 Domain unit tests. The original 34 in `PredictionPointsCalculatorTests` cover exact scores, correct winners, draws, incorrect outcomes, integer boundaries and independent repeated calls; this file remains unchanged. `PredictionPointsCalculatorValidationTests` adds 8 cases for -1/int.MinValue in each argument, asserting `ArgumentOutOfRangeException` and its parameter name, and 2 cases ensuring exact negative scores are rejected before scoring. The validation RED run passed the original 34 and failed the 10 new cases because no exception was thrown; the GREEN and full regression runs passed all 44 with zero failures or skips. See the [engineering report](../engineering-reports/PRODE-001-engineering-report.md) for commands, approval and results.

The integration project still contains no tests. Zero tests discovered there establishes no integration coverage. Local tests do not establish CI status.

For approved behavior changes, follow the red/green workflow in `AGENTS.md`: write a meaningful failing test, confirm the expected failure, implement the behavior and rerun relevant tests. Add integration fixtures when a story actually needs them.

## PRODE-002 — constructor correction, 2026-11-17

The existing 39 `MatchTests` cases were reused unchanged. A fresh run against the skeleton produced 31 behavioral failures and 8 passes; after property assignment and ID/UTC/status guards, all 39 passed. The Release solution build succeeded with 0 warnings/errors and all 83 unit tests passed (39 PRODE-002 plus the unchanged 44 PRODE-001 cases), with 0 failures/skips. Integration executed 0 cases. AC3 and team validation remain deferred and untested; green scoped tests do not establish full Issue completion. These are local results on `fix/PRODE-002-match-construction`, not a remote CI result. Exact commands, source identities and TRX paths are in [implementation evidence](../stories/PRODE-002/implementation-evidence.md); see also the [engineering report](../engineering-reports/PRODE-002-correction-engineering-report.md).

## Continuous integration

Use the [ci skill](../../.agents/skills/ci/SKILL.md) to configure or verify the existing [.NET CI workflow](../../.github/workflows/ci.yml). It restores and builds in Release, runs unit and integration projects separately, requires at least one executed unit test, and retains TRX evidence. Integration currently has zero tests; this is reported separately.

The workflow covers pushes to branches, PRs targeting `main` and manual execution. Local verification passed 44 unit tests and the zero-execution guard's negative check. GitHub run [40529598727](https://github.com/DiegoDX/ProdeChampIA/actions/runs/40529598727) passed restore, Release build, 44 unit tests, the execution guard and TRX upload for implementation commit `fe9155f`. Integration executed zero tests. See the [CI report](../stories/PRODE-001/ci-report.md) for exact results, publication and limitations. Deployment is outside this Skill and workflow.

## PRODE-003 — original creation verification (historical)

The [QA matrix](../stories/PRODE-003/test-design.md) maps 20 case groups to 43 executed PredictionTests cases: preserved IDs/score/UTC ticks, null/empty participant, null Match, negative/zero/int.MaxValue goals, UTC/date boundaries, required usable context, duplicate ID pairs with same/different scores, different pairs, input nonmutation, stateless repeated calls, independent values and all existing Match statuses. Snapshot completeness and external catalog existence are outside the accepted boundary; no integration/clock/distributed uniqueness coverage is claimed.

Five observed behavioral RED→GREEN increments introduced valid creation, identity guards, goal guards, UTC/context guards and duplicate rejection. Remaining boundary cases passed immediately without artificial RED. Final local Release build succeeded with 0 warnings/errors; all 126 unit cases passed (43 new + unchanged 83 existing), 0 failed/skipped. Integration discovered/executed 0 cases. [Implementation evidence](../stories/PRODE-003/implementation-evidence.md) records exact commands, hashes, TRX/log paths and traceability. No packages/frameworks changed. Required independent reviews and remote CI are tracked in [workflow status](../stories/PRODE-003/workflow-status.md).
