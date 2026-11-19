# PRODE-004 - Implementation evidence

## Scope, authority and source

Issue #10, unchanged updatedAt `2026-11-18T23:50:54Z`; all five criteria and eight rules preserved in workflow-status.md. The user explicitly requested implementation of the complete workflow after reviewing plan v1, authorizing files, local commits, feature push and PR creation, with final human review and no merge. This supersedes separate intermediate signoff pauses for unchanged plan/QA scope; no claim of a separately signed unseen QA design. [QA design](test-design.md) was persisted before executable tests.

Baseline: `950ec8963012f2367ae2a95a1891f8b10ba84458`; branch feature/PRODE-004; cwd `C:\ProdeChampIA\ProdeChampIA`. Production/test scope is exactly the two new files below; existing Match/Prediction/scoring files, tests, projects and packages are unchanged. Pre-existing architect/tdd-implementation skill edits remain excluded.

Frozen source SHA256:

- `src/ProdeChamp.Domain/Predictions/PredictionEditCutoff.cs`: `85DD15280971346DD09759850CC4CB7FD2F7C06A5E53C8DBAC5B0B1DC70E3818`.
- `tests/ProdeChamp.UnitTests/Predictions/PredictionEditCutoffTests.cs`: `A6AB5104C0693294E9E2ACC4363BA4F2B72594E9F14B7C947D1D60364076767D`.

## Executed TDD commands and observations

All focused commands used this exact pattern; replace LABEL with the literal label in the table:

```powershell
dotnet test tests/ProdeChamp.UnitTests/ProdeChamp.UnitTests.csproj --configuration Release --no-restore --filter FullyQualifiedName~PredictionEditCutoffTests --logger 'trx;LogFileName=LABEL.trx' --results-directory .local/PRODE-004/TestResults
```

Each invocation piped `2>&1 | Tee-Object -FilePath .local/PRODE-004/LABEL.log`; the wrapper returned `$LASTEXITCODE`. Raw TRX counters and failure messages were inspected. PowerShell additionally labels xUnit stderr as NativeCommandError; the behavioral failure evidence is the discovered/executed TRX and assertion/exception below, not that wrapper label.

| Label | Change before run / observed result | Exit | Executed | Passed | Failed | Skipped |
| --- | --- | --- | --- | --- | --- | --- |
| 01-red | C01 written first; separately recorded nonfunctional public skeleton throws NotImplementedException. All 5 boundary rows fail on that exception; build/discovery succeed. | 1 | 5 | 0 | 5 | 0 |
| 01-green | Implement strict comparison against Match.StartTimeUtc. | 0 | 5 | 5 | 0 | 0 |
| 02-red | Add C02 before guard. Assert.Throws expected ArgumentNullException, actual NullReferenceException. Existing 5 still pass. | 1 | 6 | 5 | 1 | 0 |
| 02-green | Add ArgumentNullException.ThrowIfNull(match). | 0 | 6 | 6 | 0 | 0 |
| 03-red | Add C03 before UTC guard. Four rows fail because no ArgumentException was thrown. Existing 6 pass. | 1 | 10 | 6 | 4 | 0 |
| 03-green | Add zero-offset guard and caller-boundary XML documentation. | 0 | 10 | 10 | 0 | 0 |
| 04-final | Add C04-C06; 11 new boundary/status/statelessness rows pass immediately on existing behavior; no artificial RED or unnecessary refactor. | 0 | 21 | 21 | 0 | 0 |

Pre-change source snapshots: `.local/PRODE-004/01-skeleton.cs`, `02-before-null-guard.cs`, `03-before-utc-guard.cs`. Raw TRX under `.local/PRODE-004/TestResults/`; these and logs are ignored/generated and excluded from solution items/publication. No legitimate test was weakened or rewritten to force a pass. No unsuccessful correction rounds: **0/3**.

## Traceability

| Criterion / rule | QA / executable method or review obligation | Implementation |
| --- | --- | --- |
| AC-01, AC-02, AC-03; BR1/3/4 | C01 IsAllowed_AtCutoffBoundary_UsesStrictComparison (5 rows); C04 IsAllowed_AtDateBoundaries_ComparesFullInstant (6 rows) | Strict full-instant `<` return, no rounding/subtraction. |
| AC-03; BR6/8 | C05 IsAllowed_ForEveryMatchStatus_UsesScheduledTimeOnly (4 rows); R01 | Only scheduled kickoff and validation instant are read; no status/form/actual-kickoff restriction. |
| AC-04; BR2/5 | C06 IsAllowed_ForIndependentAttempts_UsesEachSuppliedValidationInstant (1 case), fixed historical instants in C01/C06; R01 | Explicit captured validation instant; no wall-clock/save-time access or retained decision state. No save-integration claim. |
| AC-05; BR7 | R02 independent source review required | No Prediction parameter/reference, creation, edit, save or shared mutable state; existing Match is get-only. |
| Technical null/UTC input contracts | C02 IsAllowed_WithNullMatch_ThrowsForMatch (1); C03 IsAllowed_WithNonzeroValidationOffset_ThrowsForValidatedAtUtc (4) | Named argument exceptions before comparison. |

R01/R02 are source-review obligations, not automated test counts; independent reports must assess them before the review gate passes.

## Final local verification

```powershell
dotnet build ProdeChamp.slnx --configuration Release --no-restore
dotnet test tests/ProdeChamp.UnitTests/ProdeChamp.UnitTests.csproj --configuration Release --no-build --no-restore --logger 'trx;LogFileName=unit-regression.trx' --results-directory .local/PRODE-004/TestResults
dotnet test tests/ProdeChamp.IntegrationTests/ProdeChamp.IntegrationTests.csproj --configuration Release --no-build --no-restore --logger 'trx;LogFileName=integration.trx' --results-directory .local/PRODE-004/TestResults
```

Build exit 0: six projects, 0 warnings/errors. Unit exit 0: **147 executed/passed**, 0 failed/skipped, comprising 21 new plus 126 existing cases. Integration exit 0: **0 discovered/executed**; no integration coverage. Unit/integration used the freshly successful Release build; no source changes occurred between build and these runs. Logs are build.log, unit-regression.log and integration.log under .local/PRODE-004. SDK `10.0.401` verified by `dotnet --version`.

## Decisions and limitations

Pure synchronous static Domain rule; no package/interface/DI/I/O added. Correct prediction-to-match association and trustworthy server-time provenance remain caller prerequisites. A true cutoff result is not complete edit permission. Saving, API/SQL/UI, authorization, rescheduling and additional status policy remain excluded. Product/architecture/testing docs are synchronized with this implemented boundary. Remote CI is separate and will be recorded after publication; local green results do not establish remote results.
