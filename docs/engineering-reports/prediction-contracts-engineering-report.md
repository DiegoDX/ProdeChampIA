# Engineering Report - Prediction contracts correction

- Date: 2026-11-19.
- Scope/reference: user's explicit correction in this conversation; scoped-maintenance exception in the Development Factory contract. Related historical stories: PRODE-003 and PRODE-004. No Issue or acceptance criteria edited.
- Outcome: Complete for the requested local implementation and test changes.
- Evidence boundary: uncommitted working tree on `feature/PRODE-004`, baseline `33caab5a34717d2b058bc64f9bbe4bbe5af3e77b`; tested source SHA-256 snapshot and exact execution evidence in [implementation evidence](prediction-contracts-implementation-evidence.md).

## 1. Executive summary

Removed PredictionEditCutoff and moved scheduled-cutoff evaluation to `prediction.CanEdit(DateTimeOffset matchStartTimeUtc)`, using current UTC server time internally. Prediction.Create now accepts `Guid matchId` and rejects Guid.Empty without requiring a Match object. Tests and current product/architecture/testing documentation reflect both replacement contracts. Release build succeeded and all 146 unit cases passed.

## 2. Requested-change traceability

| Criterion | Status | Implementation evidence | Verification evidence |
| --- | --- | --- | --- |
| Remove separate cutoff class and expose instance CanEdit with only kickoff | Verified | Prediction.cs; old class deleted | 22 PredictionCanEditTests cases and source review |
| Compare scheduled kickoff with internal/server time; allow strictly before only | Verified | Public TimeProvider.System wrapper; internal single-capture comparison | Tick-before/equal/tick-after, date boundaries, public clock and advancing-clock tests |
| Create takes Guid match ID and retains remaining validations | Verified | Guid parameter, empty-ID guard, ID comparison/construction | 41 PredictionTests cases |
| Update tests and keep docs/Visual Studio visibility synchronized | Verified | Replacement cutoff tests, migrated factory tests, READMEs, solution links | Full unit regression, Release build, link/project verification |

## 3. Files changed

| File | Change / purpose |
| --- | --- |
| src/ProdeChamp.Domain/Predictions/Prediction.cs | Add CanEdit; replace Match argument/dependency with Guid; document server-time and association boundaries |
| src/ProdeChamp.Domain/Predictions/PredictionEditCutoff.cs | Deleted; rule belongs to Prediction |
| src/ProdeChamp.Domain/ProdeChamp.Domain.csproj | Add SDK InternalsVisibleTo for deterministic clock testing |
| tests/ProdeChamp.UnitTests/Predictions/PredictionTests.cs | Migrate factory calls/guards/identity fixtures to Guid contract |
| tests/ProdeChamp.UnitTests/Predictions/PredictionEditCutoffTests.cs | Deleted; old contract replaced |
| tests/ProdeChamp.UnitTests/Predictions/PredictionCanEditTests.cs | Add 22 instance-rule cases, including test-local TimeProvider |
| docs/product/README.md | Describe replacement public behavior and caller responsibilities |
| docs/architecture/README.md | Document entity behavior, primitive inputs and built-in clock test seam |
| docs/testing/README.md | Document current tests; distinguish historical evidence |
| docs/engineering-reports/prediction-contracts-implementation-evidence.md | Record authorization, plan, behavioral RED/GREEN, results and source hashes |
| docs/engineering-reports/prediction-contracts-engineering-report.md | Consolidate this handoff |
| docs/engineering-reports/prediction-contracts-code-review.md | Independent correction code review |
| docs/engineering-reports/prediction-contracts-architecture-review.md | Independent correction architecture review |
| docs/stories/PRODE-004/workflow-status.md | Resume lifecycle for correction publication |
| docs/stories/PRODE-004/ci-report.md | Distinguish correction CI from original rule CI |
| docs/engineering-reports/PRODE-004-engineering-report.md | Point original story report to current correction |
| ProdeChamp.slnx | Add the two new documents as solution items; retain all six projects |

Pre-existing edits to two skills are preserved and excluded from publication. The three pre-existing PRODE-004 reporting/workflow edits were independently compared with remote parent `48fd50992998e3f4a0f2663a1e958dbdbb1b0c54` and matched its published metadata. New correction annotations preserve that history.

## 4. Technical decisions and trade-offs

The public CanEdit method receives only `DateTimeOffset matchStartTimeUtc`; its internal overload receives built-in TimeProvider for deterministic tests. Production uses TimeProvider.System, captures time once and requires a zero UTC kickoff offset, preserving the validation previously provided by Match. There is no new package, custom clock service, interface, project reference or DI registration. SDK friendship permits access by the unit assembly without adding a production dependency on tests. New .cs files use default SDK inclusion with no duplicate compile entries.

Create's matchId is a nonnullable Guid. Empty is rejected explicitly; duplicate checking remains based on participant/match IDs in caller-supplied active context. Tests of null Match, Match object identity and status fixtures were replaced because those inputs no longer exist. All other creation validations and nonmutation checks remain. Creation does not invoke CanEdit.

CanEdit evaluates only the deadline and does not mutate or save predictions. Callers retain the boolean for an already-validated attempt; a subsequent method call reads current server time for a new evaluation. Historical plans/reviews/CI remain intact and do not certify these changed contracts.

## 5. Verification results

Exact commands, test failures and source snapshot: [implementation evidence](prediction-contracts-implementation-evidence.md).

| Check | Status | Actual result |
| --- | --- | --- |
| Factory behavioral RED | Observed expected failure | 41 failed with NotImplementedException against temporary Guid shape |
| Factory GREEN | Passed | 41 passed, 0 failed/skipped |
| CanEdit behavioral RED | Observed expected failure | 22 failed with NotImplementedException against temporary instance shape |
| Combined Prediction GREEN | Passed | 63 passed, 0 failed/skipped |
| Release solution build | Passed | Six projects, 0 warnings/errors |
| Final unit regression | Passed | 146 executed/passed, 0 failed/skipped |
| Integration project | Zero tests discovered | 0 executed; exit 0 does not establish integration coverage |
| Diff check and self-review | Passed in inspected scope | No whitespace errors or remaining old production/test references; no independent-review gate claimed |
| Publication revalidation | Passed | Restored the missing new test file to its identical recorded SHA-256; fresh Release build 0 warnings/errors and publication-unit.trx 146 passed, 0 failed/skipped |
| Independent code / architecture reviews | APPROVED | Distinct review-only agents independently reconciled all four source hashes and fresh raw build/TRX evidence; no findings |
| Solution visibility | Passed | Linked files exist, six projects retained, SDK default inclusion preserved |

## 6. Risks, limitations and follow-up actions

- A Guid alone does not establish database/catalog match existence. The caller must supply the associated match ID and scheduled kickoff; persistence/lookup is outside this task.
- Public evaluation uses the system clock of the process executing it, so it must run on the server. No API or server deployment was added or verified.
- No editing/saving, authorization or integration behavior is implemented. The integration project contains no tests.
- Local `.git` is read-only. The user subsequently authorized publication to feature/PRODE-004 and a PR to main. Existing PR #13 will be updated rather than duplicated, using authenticated `gh api` Git Data operations with a guarded, non-forced feature-ref update; local Git metadata will remain unchanged. No merge or Issue update is authorized.
- Independent correction code and architecture reviews are APPROVED. Exact-head remote CI is pending publication; historical story reviews/CI do not establish correction CI.
- The first automatic restore attempt failed reading the user NuGet configuration. Builds/tests succeeded with existing restored assets using `--no-restore`; clean restore was not verified.

## 7. References

- [Implementation evidence and local plan](prediction-contracts-implementation-evidence.md)
- [Independent code review](prediction-contracts-code-review.md), [independent architecture review](prediction-contracts-architecture-review.md)
- [Development Factory contract](../development-factory/README.md)
- [Current architecture](../architecture/README.md), [product](../product/README.md), [testing](../testing/README.md)
