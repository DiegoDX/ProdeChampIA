# PRODE-003 — Implementation evidence

- Status: **Passed — local implementation/build/tests; independent reviews and remote CI recorded separately**.
- Base/HEAD: `273690364bd014a13aff8ecfa394b1dcbed76df8`; branch `feature/PRODE-003`.
- Approved plan v1: original SHA256 `3C3F2CB952E87C81B7925D101D52CFB03302C4F70126E34BAC9FC08D6741515C`, user “esta bien”.
- Approved QA v1: original SHA256 `1219B42D1FF801BD6913AFB5CDCE6CCC636FD7B3991CC4B0E7F81306FD8C6E37`, user “si esta bien”.
- Actual source snapshots and commands are appended below as executions complete. No historical test result counts establish current acceptance.
- Method: `tdd-implementation` with `backend-implementation`; no packages, storage, registry, external catalog, API or Angular changes.
- Initial production shape is added only after the first test file and remains nonfunctional until observed RED. SDK default inclusion owns both new files.

## Command convention

All checks run from `C:\ProdeChampIA\ProdeChampIA`. Focused execution command for each actual label L below:

```powershell
dotnet test tests/ProdeChamp.UnitTests/ProdeChamp.UnitTests.csproj --configuration Release --filter FullyQualifiedName~PredictionTests --logger 'trx;LogFileName=L.trx' --results-directory .local/PRODE-003/TestResults --verbosity minimal
```

The label substitutes only the log filename; actual executions, counts and failure reasons are recorded in the journal. Raw TRX/logs/snapshots stay under ignored `.local/PRODE-003/`, not committed or solution-linked.

## TDD journal

| Cycle / label | Cases | Exit / actual execution | Observation |
| --- | --- | --- | --- |
| 01-red | P01 | 1; 1 executed, 0 passed, 1 failed, 0 skipped | Compiled/discovered normally; Create threw NotImplementedException. Valid behavioral RED. Source SHA F80CCA04FEB4D0E60958B9DFA2F13DE469E3DAE3F17F6B4D3A8D03E519B25D91; test SHA F8ADEB482268B37EC26DED0218246C2BCA42BE32D0698AE68A0FEBD5EF4E8287. |
| 01-green | P01 | 0; 1 executed/passed, 0 failed/skipped | Property-preserving construction implemented. Source SHA 8F5B11F9E882CB3CFEAEDD7F176225E7B5B84BA5F0162CF38D20502D773DE664; test unchanged. No refactor needed. |
| 02-red | P03/04/06 | 1; 4 executed, 1 passed, 3 failed, 0 skipped | All three new guards fail because no exception is thrown; P01 stays green. Source same as 01-green; test SHA B5866AD3EAC24FC54B2F806353524BE1D5EC07336621FB1AAFDAA9BC79209F08. |
| 02-green | P01/03/04/06 | 0; 4 executed/passed, 0 failed/skipped | Explicit participant guards and Match null guard implemented. Source SHA AFAF0370CBEE7A09CAD3E357CB0C966705BE09E56FD5F2AD46D643D9CC35F644; test unchanged. No refactor needed. |
| 03-red | P07/08 | 1; 8 executed, 4 passed, 4 failed, 0 skipped | New negative-goal cases (-1/int.MinValue in each argument) threw no exception. Source same as 02-green; test SHA 78D870C70E70C611D33AB67EDC6E2BFF16A78E3549C39A64F4AECDF483DD0998. |
| 03-green | P01/03/04/06/07/08 | 0; 8 executed/passed, 0 failed/skipped | Added ThrowIfNegative for each goals argument. Source SHA 3DFE9F77F7B02DCD8C09018E936133E433F9E38B97B39C4660CDFFDE7DC3660C; test unchanged. No refactor needed. |
| 04-red | P10/11/12 | 1; 17 executed, 8 passed, 9 failed, 0 skipped | Six offset cases, null context and two null-entry contexts threw no exception. Source same as 03-green; test SHA F9281FB8D7D84B159DE44049B3FC1C23E1792307BB88ECEC6606600D9979F507. |
| 04-green | P01/03/04/06–08/10–12 | 0; 17 executed/passed, 0 failed/skipped | UTC offset, context null and null-entry guards implemented. Source SHA D61B2DDC31EE53C7E4FFE56B82EF587BD596E7FB48E666833AEB6DEEE6E23CEB; test unchanged. No refactor needed. |
| 05-red | P13–17 | 1; 24 executed, 20 passed, 4 failed, 0 skipped | Same-pair cases (2), duplicate last (1) and rejection path (1) threw no exception. Different-pair and success/nonmutation cases immediately passed, accurately recorded as already supported behavior. Source same as 04-green; test SHA F766984CD989EE4D6C25EF60BD1E7C177E0DCC0C7B3B243ABC9DE090DE90D4CF. |
| 05-green | P01/03/04/06–08/10–17 | 0; 24 executed/passed, 0 failed/skipped | Added same-pair comparison within existing validation scan. Source SHA F2594907C9AFBEE604CEF2BC62A8926EEC3153AC87DA4AB30CE79992370943CC; test unchanged. No extra abstraction/refactor warranted. |
| 06-final | All P01–P20 | 0; 43 executed/passed, 0 failed/skipped | Remaining 19 boundary/statelessness executions passed immediately as existing supported behavior. No artificial RED. Only production change after 05-green was XML contract documentation; no behavior refactor needed. Final source SHA B4FF2CCFD92861522DC3190223ACCC3642C22299DAACCC8E5867841BB604A967; final test SHA 4F51A12E1FAA24EA0E1E5C5F41C13090CD6B791652197A55F3A71D29CA2A142A. |

The first test file was authored before the nonfunctional public shape. A sandboxed attempt to create the Domain subfolder was denied before writing the file; scoped authorized directory creation resolved the environment prerequisite, not a test correction. dotnet first-run initialization occurred automatically on this first test run; no certificate trust command or deployment was requested/executed.

Every labeled focused command used this exact wrapper, substituting its actual label (`01-red`, `01-green`, `02-red`, `02-green`, `03-red`, `03-green`, `04-red`, `04-green`, `05-red`, `05-green`, `06-final`) in file paths:

```powershell
Copy-Item src/ProdeChamp.Domain/Predictions/Prediction.cs .local/PRODE-003/L-Prediction.cs
Copy-Item tests/ProdeChamp.UnitTests/Predictions/PredictionTests.cs .local/PRODE-003/L-PredictionTests.cs
dotnet test tests/ProdeChamp.UnitTests/ProdeChamp.UnitTests.csproj --configuration Release --filter FullyQualifiedName~PredictionTests --logger 'trx;LogFileName=L.trx' --results-directory .local/PRODE-003/TestResults --verbosity minimal 2>&1 | Tee-Object -FilePath .local/PRODE-003/L.log
$prodeCheckExit = $LASTEXITCODE
Get-FileHash src/ProdeChamp.Domain/Predictions/Prediction.cs,tests/ProdeChamp.UnitTests/Predictions/PredictionTests.cs -Algorithm SHA256 | Format-List
exit $prodeCheckExit
```

PowerShell sometimes formatted dotnet stderr as NativeCommandError during expected test failures; successful compilation, TRX counts and actual failing assertions above establish behavioral RED. Full source/test copies and failure logs exist locally for each label, with durable hashes/counts retained here. Generated snapshots are ignored and will not be committed.

## Local build and regression

Final tested production/test hashes are those of 06-final above. All subsequent product/report changes are documentation only unless explicitly noted.

| Command | Exit / result | Local artifact |
| --- | --- | --- |
| `dotnet build ProdeChamp.slnx --configuration Release` | 0; all six projects built; 0 warnings, 0 errors | `.local/PRODE-003/build-release.log` |
| `dotnet test tests/ProdeChamp.UnitTests/ProdeChamp.UnitTests.csproj --configuration Release --no-build --no-restore --logger 'trx;LogFileName=unit-regression.trx' --results-directory .local/PRODE-003/TestResults --verbosity minimal` | 0; 126 discovered/executed/passed, 0 failed/skipped (43 new + 83 existing) | `.local/PRODE-003/TestResults/unit-regression.trx`, `.local/PRODE-003/unit-regression.log` |
| `dotnet test tests/ProdeChamp.IntegrationTests/ProdeChamp.IntegrationTests.csproj --configuration Release --no-build --no-restore --logger 'trx;LogFileName=integration.trx' --results-directory .local/PRODE-003/TestResults --verbosity minimal` | 0; zero discovered/executed/passed/failed/skipped. No integration coverage | `.local/PRODE-003/TestResults/integration.trx`, `.local/PRODE-003/integration.log` |
| `git -c safe.directory=C:/ProdeChampIA/ProdeChampIA diff --exit-code HEAD -- tests/ProdeChamp.UnitTests/Matches/MatchTests.cs tests/ProdeChamp.UnitTests/Scoring src/ProdeChamp.Domain/Matches src/ProdeChamp.Domain/Scoring` | 0; all existing Match/scoring source and tests unchanged | Git diff observed directly |

Build/tests output was captured using `2>&1 | Tee-Object -FilePath <listed log>`, followed by `exit $LASTEXITCODE`; the unit/integration wrapper captured the unit exit and stopped before integration on failure. --no-build was used only after the matching successful Release solution build, with no source/test change between them.

TRX verification at `2026-11-17T22:59:54Z` read all files under `.local/PRODE-003/TestResults/*.trx` and extracted `TestRun.ResultSummary.Counters` using PowerShell XML. Each RED/GREEN and final count above matched the raw report; integration counters were all zero. No legitimate tests were weakened/deleted/skipped. No unsuccessful correction round was consumed (0/3).

## Criterion → case → executable test → code

All new methods below are in `tests/ProdeChamp.UnitTests/Predictions/PredictionTests.cs`; the production target for all is `src/ProdeChamp.Domain/Predictions/Prediction.cs` / Create and private property-preserving construction.

| Criterion / rule | QA cases | Executable test method(s), prefix Create_ |
| --- | --- | --- |
| AC-01, BR2/6 | P01 | WithValidInputs_PreservesAssociationsScoreAndUtcTimestamp |
| AC-01/02, BR3 | P02 | WithNonnegativeGoalBoundaries_PreservesGoals |
| AC-03 | P03/04/05 | WithNullParticipantId_ThrowsForParticipantId; WithEmptyParticipantId_ThrowsForParticipantId; WithNonemptyParticipantGuidBoundaries_PreservesId |
| AC-04, BR1 | P06 | WithNullMatch_ThrowsForMatch; inherited valid/empty-ID Match construction covered by unchanged MatchTests in 126-case regression |
| AC-02 | P07/08 | WithNegativeHomeGoals_ThrowsForHomeGoals; WithNegativeAwayGoals_ThrowsForAwayGoals |
| AC-05, BR4 | P09/10 | WithUtcDateBoundaries_PreservesExactTimestamp; WithNonzeroRegistrationOffset_ThrowsForRegisteredAtUtc |
| BR5 / context | P11/12 | WithNullActivePredictions_ThrowsForActivePredictions; WithNullContextEntry_ThrowsForActivePredictions |
| BR5 / pair identity | P13/14 | WithDuplicatePair_ThrowsRegardlessOfScoreOrMatchInstance; WithDuplicateAfterUnrelatedEntries_Throws |
| BR5 / distinct pairs | P15/16 | WithSameParticipantAndDifferentMatch_Succeeds; WithDifferentParticipantAndSameMatch_Succeeds |
| No registry / caller ownership | P17/18 | WithCallerOwnedList_DoesNotModifyIt; RepeatedWithUnchangedEmptyContext_DoesNotRegisterHiddenState |
| BR6 / independent values | P19 | WhenCallerContextChanges_PreservesIndependentPredictionValues |
| Explicit exclusions / no status lock | P20 | WithEachDefinedMatchStatus_Succeeds |
| BR7 / immutable contract | R01/R02 | Required independent source-review obligations, not invented executable tests; verdicts recorded separately |

## Decisions and limitations

One Domain class, private constructor and get-only values; synchronous pure factory, no interfaces/packages/I/O/DI wrappers. Participant validation covers null/empty GUID; valid Match object ensures its existing ID invariant. Caller supplies actual UTC time and complete/current active context. Duplicate rejection compares both IDs, independent of scores and Match reference identity. No collection mutation/retention or storage. No global/concurrent/durable uniqueness or external catalog guarantee. Nonzero offsets are rejected rather than normalized. Original Issue criteria unchanged; scoped decisions and plan/QA approvals are in the workflow ledger.
