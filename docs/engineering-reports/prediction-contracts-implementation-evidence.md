# Prediction contracts correction - implementation evidence

- Date: 2026-11-19.
- Authorization: the user's explicit scoped correction in this conversation: remove PredictionEditCutoff, move cutoff evaluation to Prediction.CanEdit with only scheduled kickoff as its public input and use server time; replace Create's Match argument with a Guid; update tests.
- Baseline: `33caab5a34717d2b058bc64f9bbe4bbe5af3e77b`, existing `feature/PRODE-004` working tree.
- Pre-existing edits preserved: architect/TDD skills and PRODE-004 engineering report, CI report and workflow status.
- Branch creation attempted with `git -c safe.directory=C:/ProdeChampIA/ProdeChampIA switch -c fix/prediction-contracts`; denied creating `.git/refs/heads/fix/prediction-contracts.lock`. Continue local source edits on the existing feature branch; no Git permissions/configuration changed.

## Local plan and test design

This is explicitly authorized maintenance, using the shared factory contract's scoped-correction exception rather than creating a new story or changing GitHub acceptance criteria. Historical PRODE-003/004 plans and evidence remain historical. Current documentation will describe the replacement contracts.

1. Adapt factory tests to Guid IDs. Replace the obsolete null-Match guard with Guid.Empty rejection and object/status-dependent tests with direct ID boundary/duplicate coverage. Keep participant, goal, UTC registration, active-context, duplicate and nonmutation coverage.
2. Add a throwing Guid factory shape only if needed for compilation; demonstrate behavioral RED, then implement the same factory validations with direct matchId preservation and comparison.
3. Replace cutoff tests with instance CanEdit tests: strict before/equal/after and tick boundaries, UTC kickoff guard, date extremes, repeated calls as time advances, single clock capture, and prediction nonmutation. Verify the public one-argument method against real server time using wide time bounds.
4. Keep the public method `CanEdit(DateTimeOffset matchStartTimeUtc)`. Use built-in `TimeProvider.System`; an internal overload accepts TimeProvider for deterministic tests through SDK InternalsVisibleTo. No new dependency or clock service class.
5. Observe cutoff RED against a throwing shape, implement the rule, remove the old class/tests and all production Match dependencies from Prediction.
6. Run focused tests, Release solution build, full unit and integration discovery; review diff, solution links and source identity. Synchronize product/architecture/testing docs and deliver an engineering report.

## Evidence

All commands ran in `C:\ProdeChampIA\ProdeChampIA`, SDK 10.0.401, Release, xUnit 2.9.3/VSTest. Each .NET shell used `DOTNET_CLI_HOME=$PWD/.local/dotnet-home` and `DOTNET_CLI_TELEMETRY_OPTOUT=1`; initial/factory RED shells also set `DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1`.

The initial factory test invocation without `--no-restore` exited 1 before tests: NuGet could not read `C:\Users\PC\AppData\Roaming\NuGet\NuGet.Config` due to unauthorized access. This was an environment failure, not RED. Existing restored assets/packages were usable, so subsequent commands used `--no-restore`; no configuration, permissions or dependency changes were needed.

### Factory RED / GREEN

Tests were changed first for the explicitly requested Guid contract. A temporary nonfunctional public Guid overload throwing NotImplementedException was added solely to compile them; the existing Match overload remained during RED. The following command exited 1, executing 41 tests, all failing because the new Guid shape threw NotImplementedException, including valid creation and empty-match-ID exception assertions:

```powershell
dotnet test tests/ProdeChamp.UnitTests/ProdeChamp.UnitTests.csproj -c Release --no-restore --filter FullyQualifiedName~ProdeChamp.UnitTests.Predictions.PredictionTests --logger 'trx;LogFileName=create-red.trx' --results-directory .local/prediction-contracts
```

GREEN removed the temporary overload and replaced the original Match parameter with Guid, added Guid.Empty rejection, and used matchId for duplicate comparison and construction. The same command with `create-green.trx` exited 0: 41 executed/passed, 0 failed/skipped. No expected assertion was relaxed to obtain GREEN.

### CanEdit RED / GREEN

The 22 instance-method tests were written before implementation. A public one-argument wrapper and an internal throwing TimeProvider overload were added as the nonfunctional shape, plus SDK InternalsVisibleTo so the test assembly could compile deterministic tests. The following command exited 1: 22 executed/failed, 0 passed/skipped, due to NotImplementedException instead of expected boolean results/argument exceptions:

```powershell
dotnet test tests/ProdeChamp.UnitTests/ProdeChamp.UnitTests.csproj -c Release --no-restore --filter FullyQualifiedName~PredictionCanEditTests --logger 'trx;LogFileName=can-edit-red.trx' --results-directory .local/prediction-contracts
```

Output was captured in `.local/prediction-contracts/can-edit-red.log`. GREEN added the UTC kickoff guard, one GetUtcNow read and strict comparison. Removed PredictionEditCutoff and its old tests because their public contract was explicitly replaced. Updated tests preserve the strict cutoff/date/decision rules; old Match-null/status and caller-validation-offset tests are superseded by the new primitive-input/server-clock contract rather than silently weakened.

```powershell
dotnet test tests/ProdeChamp.UnitTests/ProdeChamp.UnitTests.csproj -c Release --no-restore --filter FullyQualifiedName~ProdeChamp.UnitTests.Predictions --logger 'trx;LogFileName=prediction-green.trx' --results-directory .local/prediction-contracts
```

Exit 0: 63 executed/passed (41 factory + 22 cutoff), 0 failed/skipped. No additional production refactoring followed GREEN. A final factory-test trailing newline normalization was rebuilt by the final unit command below.

### Final verification

```powershell
dotnet build ProdeChamp.slnx -c Release --no-restore
dotnet test tests/ProdeChamp.UnitTests/ProdeChamp.UnitTests.csproj -c Release --no-restore --logger 'trx;LogFileName=unit-regression.trx' --results-directory .local/prediction-contracts
dotnet test tests/ProdeChamp.IntegrationTests/ProdeChamp.IntegrationTests.csproj -c Release --no-build --no-restore --logger 'trx;LogFileName=integration.trx' --results-directory .local/prediction-contracts
git -c safe.directory=C:/ProdeChampIA/ProdeChampIA diff --check
```

- Build exit 0: all six projects, 0 warnings/errors. The only later source-file edit was a trailing newline in PredictionTests, rebuilt by the final unit run.
- Unit exit 0: 146 executed/passed, 0 failed/skipped: 41 factory, 22 cutoff and 83 unchanged Match/scoring tests. The previous total was 147: replacing four factory status rows with two Guid rows (-2) and 21 cutoff cases with 22 (+1) explains the difference.
- Integration exit 0: zero discovered/executed. No integration coverage is claimed. This used the previously successful Release solution build; integration/production source did not change afterward.
- TRX counters were independently read from each result file and matched the totals above. Generated evidence is ignored and excluded from Solution Explorer.
- Diff whitespace check exit 0. Git issued line-ending conversion notices, with no whitespace errors.
- Self-review inspected the changed production code, factory diff, complete new cutoff tests, obsolete-call search and documentation. No blocking issue found in that scope; this is not an independent review/approval.

### Verified source snapshot (SHA-256)

These final source files were hashed after final unit/integration execution; no later code changes are included in this evidence:

| File | SHA-256 |
| --- | --- |
| src/ProdeChamp.Domain/Predictions/Prediction.cs | `e522384b84493396fe483c9b2ae60a59f821f73836733db949273fed30186c29` |
| src/ProdeChamp.Domain/ProdeChamp.Domain.csproj | `abcfb100d90997c536fb2382f44d2d0de5fe40715fc5ad249b8b9800d6313e3f` |
| tests/ProdeChamp.UnitTests/Predictions/PredictionTests.cs | `15eba9465f1e74b6651d373db05e160a3ae8872238ce17708b8bc1dcbed7dc7b` |
| tests/ProdeChamp.UnitTests/Predictions/PredictionCanEditTests.cs | `49f3539adb8a31b116169ba5f412ed14f467ff73c21be3a47a50fb02e69d1dbc` |

PredictionEditCutoff.cs and PredictionEditCutoffTests.cs are deleted. No remaining production/test Prediction code references the old class or accepts a Match object. File inventory, decisions, limitations and final handoff are consolidated in the [engineering report](prediction-contracts-engineering-report.md).

## Publication preflight - 2026-11-19

The user's follow-up explicitly authorizes publishing these changes on PRODE-004's branch and making its PR to main. Fresh GitHub reads confirmed existing OPEN/non-draft [PR #13](https://github.com/DiegoDX/ProdeChampIA/pull/13), head feature/PRODE-004 at `48fd50992998e3f4a0f2663a1e958dbdbb1b0c54`, base main still `950ec8963012f2367ae2a95a1891f8b10ba84458`. The branch is exactly one metadata-only commit ahead of local HEAD; its three metadata documents match local contents before correction annotations. Issue #10 body/update timestamp remain unchanged.

Preflight found the new untracked PredictionCanEditTests.cs absent from disk. Its cause was not established. Restored the exact previously tested content; SHA-256 again equals `49f3539adb8a31b116169ba5f412ed14f467ff73c21be3a47a50fb02e69d1dbc`. No expected result or production code changed. Fresh verification:

```powershell
dotnet build ProdeChamp.slnx -c Release --no-restore
dotnet test tests/ProdeChamp.UnitTests/ProdeChamp.UnitTests.csproj -c Release --no-build --no-restore --logger 'trx;LogFileName=publication-unit.trx' --results-directory .local/prediction-contracts
```

Both exited 0: build six projects, 0 warnings/errors (`publication-build.log`); unit146 executed/passed, failed/skipped0. Same local .NET environment as above. Integration remains the unchanged empty project, so its existing zero-discovery evidence was not rerun redundantly.

The story-workflow skill explicitly authorizes independent review-only agents. `/root/review_prediction_code` and `/root/review_prediction_architecture` both independently inspected the restored file, all four final hashes, current source/diff/project graph, Issue and raw fresh evidence: **APPROVED, no findings**. Neither reviewer implemented this correction. See their linked reports. Coordinator gate checks confirm distinct identities, exact source identity, AC01-05/correction coverage and no blockers. No JSON requested. Unsuccessful correction count remains0/3; restoration of identical missing source is a resolved publication prerequisite, not a failed product correction.

Because existing local `.git` is read-only, publication will use authenticated `gh api` Git Data API operations on the existing remote feature branch, never main. Prepare an exact manifest, preserve remote metadata history as parent, validate expected branch/base and reviewed hashes, construct a tree containing only the correction/review/report paths, commit it, verify exact changed paths, and update the feature ref with force=false after a fresh parent check. No local Git metadata or permissions are changed. Final commit/run URLs and exact-head outcomes belong to the publication ledger/PR Checks.
