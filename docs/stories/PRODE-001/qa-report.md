# PRODE-001 — QA report and TDD RED evidence

> Historical QA/RED report from 2026-10-23. On 2026-10-26 the user approved closure of this stage and authorized Developer/GREEN. The same 34 tests were rerun for RED and then passed after scoring implementation, without modifying the test file. The user subsequently approved rejecting negative goals with `ArgumentOutOfRangeException` identifying the parameter; 10 additional tests in a separate validation class demonstrated RED and then passed. Current total: 44 passing local unit tests. See the [implementation engineering report](../../engineering-reports/PRODE-001-engineering-report.md) for current approval, commands and results. Statements below about deferred validation or unimplemented scoring describe the original QA stage, not the current implementation.

- Date: 2026-10-23.
- Story: [PRODE-001 — Calculate prediction points, Issue #1](https://github.com/DiegoDX/ProdeChampIA/issues/1).
- Branch: `feature/PRODE-001-scoring-engine`.
- Baseline HEAD: `9d6b4764891eec2053da78dd251b17a9439bd88f`.
- Outcome: test-only QA stage complete; **RED demonstrated**. Scoring behavior remains unimplemented.

## Test basis and authorization

Read [AGENTS.md](../../../AGENTS.md), the approved GitHub Issue, the [implementation plan](implementation-plan.md), product/story documentation, the [architecture overview](../../architecture/README.md), ADR directory, testing guidance, project files, and Sprint 0 report. Retrieved the issue with:

```powershell
gh issue view 1 --repo DiegoDX/ProdeChampIA --json number,title,url,body,state,labels,comments,updatedAt
```

The issue was open, last updated `2026-10-20T23:31:29Z`, with no comments. The user's instruction identifies it as the approved story and authorizes implementing unit tests and the minimum production API skeleton for this RED stage. The user clarified that “qa skill” means [test-design](../../../.agents/skills/test-design/SKILL.md). Applied its traceability and test-design guidance, together with [tdd-implementation](../../../.agents/skills/tdd-implementation/SKILL.md) and [engineering-report](../../../.agents/skills/engineering-report/SKILL.md). The explicit test-implementation request extends the design-only scope of test-design for this task.

The implementation plan still describes a proposed negative-input policy. This task does not approve that policy or change the plan, issue, acceptance criteria, or business rules. The pre-existing untracked implementation plan was preserved.

## Scope and technical decisions

- Use the existing xUnit 2.9.3 unit-test project, Microsoft.NET.Test.Sdk 17.14.1, and runner 3.1.4. No packages, references, SDK settings, or frameworks changed.
- Test the planned public Domain calculator directly, using named arguments for all four goal positions. No mocks, fixtures, database, HTTP host, or integration dependencies are required.
- Five theories contain 33 independently reported cases; one fact tests repeated calls on the same instance: **34 tests total**. Each case asserts the public integer award, without inspecting implementation details.
- Exact-score assertions require 5, thereby rejecting category accumulation to 8. Both winning sides, all six mismatched outcome directions, and zero-zero results are covered.
- Boundary cases use 0, 1, `int.MaxValue - 1`, and `int.MaxValue` from the planned integer API. They apply the existing 5/3/0 rules without inventing a goal cap or new scoring policy. Large-score cases include sums that would overflow an unnecessary integer goal-total calculation and one-goal winning margins.
- The calculator skeleton is necessary because the initial test could not compile without the planned type. Its only method body is `throw new NotImplementedException();`. It contains no scoring logic, input validation, or fallback award.
- Negative values and other invalid inputs are excluded: the approved issue specifies no invalid-input behavior. Plan cases V01/V02 remain deferred pending a business decision; there are no skipped placeholder tests.

## Acceptance criteria traceability

AC IDs below follow the implementation plan's numbering; the issue's criteria are unchanged.

| Criterion | Automated cases | Expected behavior | Observed implementation status |
| --- | --- | --- | --- |
| AC1: prediction 2-1, result 2-1 returns 5 | E01 | Exact score awards 5 | Not implemented; RED |
| AC2: prediction 2-1, result 3-0 returns 3 | W01 | Correct winner, different score awards 3 | Not implemented; RED |
| AC3: prediction 1-1, result 2-2 returns 3 | D01 | Correct draw, different score awards 3 | Not implemented; RED |
| AC4: prediction 2-0, result 0-2 returns 0 | I01 | Incorrect outcome awards 0 | Not implemented; RED |
| AC5: points must not accumulate across categories | E01–E04, B01–B05 | Exact wins and draws award only 5 | Not implemented; RED |

All five criteria have automated tests, but none is verified as passing. The repeated-call case R01 additionally follows plan S12; it checks independent calculations rather than adding a participant-total feature.

## Detailed automated cases

Common Given/When/Then: given the prediction and supplied final score below, instantiate `PredictionPointsCalculator`, invoke `Calculate` with home/away values in their named positions, and assert the listed award. All cases are Domain unit tests in [PredictionPointsCalculatorTests.cs](../../../tests/ProdeChamp.UnitTests/Scoring/PredictionPointsCalculatorTests.cs), require only .NET and the existing test packages, and have status **automated / executed RED**.

P0 covers the explicit acceptance criteria and precedence risk. P1 covers alternative outcomes, partial score matches, zero boundaries, and state reuse. P2 covers technical integer robustness. These priorities describe testing risk, not additional product requirements.

| Case | Basis | Prediction | Final score | Expected points | Priority |
| --- | --- | --- | --- | ---: | --- |
| E01 | AC1, AC5 / S01 | 2-1 | 2-1 | 5 | P0 |
| E02 | Exact-score rule, AC5 / S05 | 0-2 | 0-2 | 5 | P1 |
| E03 | Exact-score rule, AC5 / S06 | 0-0 | 0-0 | 5 | P0 |
| E04 | Exact-score rule, AC5 / S06 | 2-2 | 2-2 | 5 | P1 |
| W01 | AC2 / S02 | 2-1 | 3-0 | 3 | P0 |
| W02 | Correct-winner rule / S07 | 0-1 | 1-3 | 3 | P1 |
| W03 | Correct-winner rule; only home goals match | 2-1 | 2-0 | 3 | P1 |
| W04 | Correct-winner rule; only away goals match | 2-1 | 3-1 | 3 | P1 |
| W05 | Correct-winner rule; only away goals match | 1-2 | 0-2 | 3 | P1 |
| W06 | Correct-winner rule; only home goals match | 1-2 | 1-3 | 3 | P1 |
| D01 | AC3 / S03 | 1-1 | 2-2 | 3 | P0 |
| D02 | Draw rule, zero boundary / S11 | 0-0 | 1-1 | 3 | P1 |
| D03 | Draw rule, zero-zero result | 1-1 | 0-0 | 3 | P1 |
| I01 | AC4 / S04 | 2-0 | 0-2 | 0 | P0 |
| I02 | Incorrect prediction / S10 | 0-2 | 2-0 | 0 | P1 |
| I03 | Incorrect prediction / S08 | 1-1 | 2-1 | 0 | P1 |
| I04 | Incorrect prediction / S08 | 1-1 | 1-2 | 0 | P1 |
| I05 | Incorrect prediction / S09 | 2-1 | 1-1 | 0 | P1 |
| I06 | Incorrect prediction / S09 | 1-2 | 1-1 | 0 | P1 |
| I07 | Incorrect prediction, zero-zero result | 1-0 | 0-0 | 0 | P1 |
| I08 | Incorrect prediction, zero-zero result | 0-1 | 0-0 | 0 | P1 |
| B01 | Exact-score rule, AC5; zero/one boundary | 1-0 | 1-0 | 5 | P1 |
| B02 | Exact-score rule, AC5; zero/one boundary | 0-1 | 0-1 | 5 | P1 |
| B03 | Exact-score rule, AC5; integer boundary | M-0 | M-0 | 5 | P2 |
| B04 | Exact-score rule, AC5; integer boundary | 0-M | 0-M | 5 | P2 |
| B05 | Exact-score rule, AC5; integer boundary | M-M | M-M | 5 | P2 |
| B06 | Correct-winner rule / S13 | M-0 | (M - 1)-0 | 3 | P2 |
| B07 | Correct-winner rule; integer boundary | 0-M | 0-(M - 1) | 3 | P2 |
| B08 | Correct-winner rule; one-goal margin near M | M-(M - 1) | (M - 1)-0 | 3 | P2 |
| B09 | Correct-winner rule; one-goal margin near M | (M - 1)-M | 0-(M - 1) | 3 | P2 |
| B10 | Draw rule; integer boundary | M-M | (M - 1)-(M - 1) | 3 | P2 |
| B11 | Incorrect prediction; integer boundary | M-0 | 0-M | 0 | P2 |
| B12 | Incorrect prediction; integer boundary | 0-M | M-0 | 0 | P2 |
| R01 | Stateless calculation / S12 | 2-1, then 2-0, then 2-1 | 2-1, then 0-2, then 2-1 | 5, 0, 5 on one instance | P1 |

`M` means `int.MaxValue` (2,147,483,647); parentheses distinguish arithmetic expressions from score separators.

| Case IDs | Test method |
| --- | --- |
| E01–E04 | `Calculate_WithExactScore_ReturnsFiveWithoutAddingOutcomePoints` |
| W01–W06 | `Calculate_WithCorrectWinnerAndDifferentScore_ReturnsThree` |
| D01–D03 | `Calculate_WithCorrectDrawAndDifferentScore_ReturnsThree` |
| I01–I08 | `Calculate_WithIncorrectOutcome_ReturnsZero` |
| B01–B12 | `Calculate_AtIntegerBoundaries_ReturnsExpectedPoints` |
| R01 | `Calculate_WhenInstanceIsReused_ReturnsIndependentAwards` |

## Actual commands and results

SDK: `dotnet --version` returned **10.0.401**. Projects target `net10.0`. Existing restored assets were used with `--no-restore`; no restore or online package access was performed.

These process-local settings were used for the .NET commands:

```powershell
$env:DOTNET_CLI_HOME = Join-Path (Get-Location) '.local\dotnet'
$env:DOTNET_SKIP_FIRST_TIME_EXPERIENCE = '1'
$env:DOTNET_CLI_TELEMETRY_OPTOUT = '1'
$env:DOTNET_ADD_GLOBAL_TOOLS_TO_PATH = '0'
```

1. Initially wrote only the four exact-score theory cases, before creating production source. Ran:

   ```powershell
   dotnet test tests/ProdeChamp.UnitTests/ProdeChamp.UnitTests.csproj --no-restore --filter FullyQualifiedName~Calculate_WithExactScore --verbosity minimal
   ```

   First attempt: exit 1, `MSB3491`, denied write access to a Domain `obj/Debug/net10.0` build artifact. This was an environment failure, **not RED**. Reran the same command with human-approved execution outside the sandbox: exit 1, compiler error `CS0234` at the test's namespace import because the Domain scoring type did not exist. No tests executed; this compilation failure also is **not validated RED**.

2. Created the minimal throwing calculator skeleton, then ran with approved execution outside the sandbox:

   ```powershell
   dotnet test tests/ProdeChamp.UnitTests/ProdeChamp.UnitTests.csproj --no-restore --filter FullyQualifiedName~Calculate_WithExactScore --logger 'trx;LogFileName=PRODE-001-exact-red.trx' --results-directory tests/ProdeChamp.UnitTests/TestResults --verbosity minimal
   ```

   Exit 1. **4 executed, 4 failed, 0 passed, 0 skipped.** Every failure was `System.NotImplementedException` from `PredictionPointsCalculator.Calculate`, after successful compilation and test discovery. The TRX records the run from `2026-10-23T16:59:24.7711697-04:00` to `2026-10-23T16:59:26.0431012-04:00`.

3. Added the remaining cases without changing scoring requirements or the skeleton. Ran:

   ```powershell
   dotnet test tests/ProdeChamp.UnitTests/ProdeChamp.UnitTests.csproj --no-restore --filter FullyQualifiedName~PredictionPointsCalculatorTests --logger 'trx;LogFileName=PRODE-001-unit-red.trx' --results-directory tests/ProdeChamp.UnitTests/TestResults --verbosity minimal > .local/PRODE-001-unit-red.log 2>&1
   ```

   Exit 1. **34 executed, 34 failed, 0 passed, 0 skipped.** Inspected the TRX counters and grouped every failure message: all 34 reported `System.NotImplementedException : The method or operation is not implemented.` The TRX records the run from `2026-10-23T17:05:58.5056603-04:00` to `2026-10-23T17:05:59.1502942-04:00`.

   Representative failure: E01 expects 5 for prediction 2-1 and result 2-1, but invoking `Calculate` throws at the skeleton's line 10. The tests fail before their equality assertions because the required calculation is absent. They do not assert that the placeholder exception is correct behavior. This is the observed RED evidence; it is not an assertion-mismatch result or a passing scoring verification. R01 likewise stops at its first invocation; later calls are not reached in this RED run.

   Actual runner summary:

   ```text
   Failed!  - Failed:    34, Passed:     0, Skipped:     0, Total:    34
   TestCommandExitCode=1
   ```

4. Compiled the full solution after the RED run:

   ```powershell
   dotnet build ProdeChamp.slnx --no-restore --verbosity minimal
   ```

   **Exit 0; all six projects built, 0 warnings, 0 errors.** This confirms the final tests and skeleton compile; it does not mean any scoring test passes.

5. Reviewed all three new source/report files, expected values, coverage against the issue and plan, and the working-tree inventory. `git -c safe.directory=C:/ProdeChampIA/ProdeChampIA diff --check` exited 0; because these deliverables are untracked, that command alone does not check their contents. No persistent Git configuration was changed.

Local execution evidence is retained in ignored artifacts:

- `tests/ProdeChamp.UnitTests/TestResults/PRODE-001-exact-red.trx`.
- `tests/ProdeChamp.UnitTests/TestResults/PRODE-001-unit-red.trx`.
- `.local/PRODE-001-unit-red.log`.

Workspace directory/file writes and build-artifact writes required approved execution beyond the sandbox. No machine-wide configuration or dependency changes were made.

## Files changed

| File | Change and purpose |
| --- | --- |
| [PredictionPointsCalculatorTests.cs](../../../tests/ProdeChamp.UnitTests/Scoring/PredictionPointsCalculatorTests.cs) | Added 34 xUnit cases for the approved rules and planned API boundaries. |
| [PredictionPointsCalculator.cs](../../../src/ProdeChamp.Domain/Scoring/PredictionPointsCalculator.cs) | Added only the public sealed calculator and four-integer synchronous method throwing `NotImplementedException`, so tests can compile and execute. |
| [qa-report.md](qa-report.md) | Added traceability, actual RED/build evidence, decisions, limitations, and interview questions. |

The existing untracked `implementation-plan.md` was not changed. Generated `bin/`, `obj/`, `TestResults/`, and `.local/` files are ignored verification artifacts.

## Limitations and handoff

- This completes the requested QA/RED stage only. Production scoring, GREEN, and refactoring were deliberately not performed; all scoring tests remain failing.
- Negative-input behavior remains unspecified and untested. No null, exception-type, parameter-name, clamping, or maximum-goal validation contract was invented.
- No coverage percentage was measured. A compiling solution and expected RED failures do not establish business correctness.
- Full-solution test execution and integration tests were not run. The changed behavior has only a Domain unit-test boundary; API, SQL Server, rankings, authentication, persistence, and frontend remain outside this task.
- For a later authorized implementation, start with E01, then AC2/AC3/AC4 and precedence/alternative cases. Implement the approved awards, rerun the same focused suite for GREEN, then refactor and run broader regression checks. Resolve invalid-input policy separately before adding validation tests.
- No commit, push, Issue update, Pull Request, or merge was performed. Changes remain available for human review at the RED stage boundary.

## Interview questions generated

1. **What distinguishes this RED evidence from the earlier compilation error?** The final tests compile, are discovered, and call the public calculator; failure comes from intentionally absent behavior. `CS0234` and sandbox write failures cannot establish behavioral RED.
2. **How do these tests catch category accumulation or an overly broad exact-score check?** E01–E04 require exactly 5, while W03–W06 require 3 when only one team's goal count matches. See the exact-score and correct-winner theories.
3. **Why test both sides and large integer values?** Directional symmetry catches home/away bias; zero/one and near-maximum values exercise equality boundaries and discourage unnecessary arithmetic or invented limits. See the boundary theory.
4. **Why omit negative-score tests despite their presence in the plan?** The issue does not approve their behavior, and the plan labels validation as proposed. Tests encode requirements, so an unapproved exception policy must remain deferred.

## Follow-up — Visual Studio solution visibility

Date: 2026-10-23. The user requested that the deliverables be visible in Visual Studio.

- Changed `ProdeChamp.slnx` to include the implementation plan and QA report as solution items under `docs > stories > PRODE-001`.
- Updated this report to record the change. No C# source or project configuration was changed in this follow-up.
- Kept C# files in their existing projects: SDK default compile items already include both `Scoring/PredictionPointsCalculator.cs` in Domain and `Scoring/PredictionPointsCalculatorTests.cs` in UnitTests. Explicit duplicate compile entries are unnecessary.
- Executed `dotnet msbuild src/ProdeChamp.Domain/ProdeChamp.Domain.csproj -getItem:Compile -nologo` and the equivalent command for `tests/ProdeChamp.UnitTests/ProdeChamp.UnitTests.csproj`. Both succeeded and listed the respective scoring source file.
- Executed `dotnet sln ProdeChamp.slnx list`: exit 0, all six projects retained. Inspected the solution XML, verified the two linked documentation files exist, and ran `git diff --check` successfully.
- Tests executed for this follow-up: none; only solution organization and documentation changed. The preceding 34-test RED result remains the last actual test run.
- Limitation: Visual Studio's UI was not inspected. Reload or reopen `ProdeChamp.slnx` to refresh an existing session; expand Domain and UnitTests to find each `Scoring` folder.
- Additional interview question: **Why are C# source files included automatically while documentation needs solution items?** SDK projects include C# files through default compile globs; solution items expose repository documents in Solution Explorer without making them compilation inputs.

No commit, push, or Pull Request was performed.
