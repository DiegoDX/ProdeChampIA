# PRODE-001 — Implementation plan

Status: Domain-only design, QA/RED closure and negative-input rejection approved by the user on 2026-10-26. Developer/GREEN verified: 44 passing unit tests, successful full solution build and regression. Independent code review and human approval of the implementation remain pending; no publication performed.

Date: 2026-10-23.

GitHub Issue: [DiegoDX/ProdeChampIA#1 — Calculate prediction points](https://github.com/DiegoDX/ProdeChampIA/issues/1).

## Current approval and execution — 2026-10-26

- The user explicitly approved QA/RED closure and the synchronous Domain calculator with four `int` parameters, no interface, endpoint, or new dependencies, and authorized implementation of the existing 5/3/3/0 awards without accumulation.
- The user prohibited negative goal values and subsequently confirmed the proposed contract: the calculator rejects any negative argument with `ArgumentOutOfRangeException`, identifying the offending parameter. This explicit conversation approval resolves the prior ambiguity; the GitHub Issue and its original five acceptance criteria are unchanged.
- This approval supersedes the original pending approval for the Domain-only design and entry into development. The original planning sections below describe the 2026-10-23 baseline; references to future QA or empty projects are historical, not current state. The original five Issue acceptance criteria remain unchanged.
- Validation execution plan: preserve the 34 original tests byte-for-byte; add a separate `PredictionPointsCalculatorValidationTests` class with eight cases for -1/int.MinValue in each argument (assert exception type and parameter name) and two exact negative-score cases (assert rejection before scoring without imposing multi-invalid argument ordering). Run all 44 tests for RED, then add four standard .NET guards before scoring in `PredictionPointsCalculator.Calculate`. Rerun for GREEN, build and test the solution, verify SDK compile inclusion/visibility, review the diff, and update the engineering report. No new dependency or layer is required.
- Preserve pre-existing local solution/documentation changes. No commits, pushes, Pull Requests, or merges are authorized. Independent code review and human approval remain later boundaries.
- Observed results: 34/34 existing tests failed with `NotImplementedException` before implementation, then 34/34 passed after implementation; full solution build passed with zero warnings/errors, and full solution tests passed 34 unit tests with zero integration tests discovered. See the [implementation engineering report](../../engineering-reports/PRODE-001-engineering-report.md). No additional refactoring was needed for this small rule.

### Approved validation outcome

The execution plan above is complete. The additional validation run demonstrated RED (34 original tests passed, 10 new tests failed because no exception was thrown), followed by GREEN (44 passed). Four `ArgumentOutOfRangeException.ThrowIfNegative` guards now run before exact-score/outcome evaluation. V01/V02 from the historical proposal are approved and covered in the separate validation test class, including `int.MinValue` and exact negative-score rejection. Full solution build passed with 0 warnings/errors and regression passed 44 unit tests; the integration project still has no tests. No extra refactoring, dependencies, interfaces or endpoints were needed. The following original planning sections retain their historical context; the previous validation question and approval prerequisites have been resolved by the recorded user approval.

## Source and repository verification

- Read [AGENTS.md](../../../AGENTS.md) and the [architect skill](../../../.agents/skills/architect/SKILL.md).
- Retrieved the issue using `gh issue view 1 --repo DiegoDX/ProdeChampIA --json number,title,url,body,state,labels,comments,updatedAt`.
- The issue is open, was updated at `2026-10-20T23:31:29Z`, and has labels `type:story`, `priority:p0`, and `area:backend`. It has no comments or explicit approval label. The user's instruction establishes the approved story for this planning task; these labels alone do not approve this technical plan.
- Reviewed [product documentation](../../product/README.md), [user-story documentation](../../user-stories/README.md), the [architecture overview](../../architecture/README.md), the [ADR directory](../../architecture/decisions/README.md), [testing guidance](../../testing/README.md), and the [Sprint 0 report](../../engineering-reports/sprint-0.md). Product and story documents contain scaffold guidance only. No additional scoring rules or numbered ADRs exist; the issue supplies the requirements.
- Inspected `ProdeChamp.slnx`, `global.json`, all six project files, and the only existing production C# file, `src/ProdeChamp.API/Program.cs`.
- Branch: `feature/PRODE-001-scoring-engine`. HEAD: `9d6b476` (`chore: add architect Codex skill`). The working tree was clean before this document was created. No branch change was necessary.

## Business requirements

As a ProdeChamp participant, I want to receive points based on my prediction and the final match score, so that I can compete against other participants.

Calculate one points award for one prediction against one supplied final result:

| Condition | Points |
| --- | ---: |
| Exact score | 5 |
| Correct winner | 3 |
| Correct draw, wrong score | 3 |
| Incorrect prediction | 0 |

Categories are mutually exclusive. An exact score awards 5 points in total, including an exact draw; it must never also award the 3 points for the matching outcome.

## Acceptance criteria

The criteria below are transcribed from the issue without changing their meaning or adding requirements:

1. Given a prediction of 2-1 and result 2-1, return 5.
2. Given a prediction of 2-1 and result 3-0, return 3.
3. Given a prediction of 1-1 and result 2-2, return 3.
4. Given a prediction of 2-0 and result 0-2, return 0.
5. Points must not accumulate across categories.

Validation proposals and additional QA scenarios below are distinguished from these approved criteria.

## Existing architecture impact

All six projects target `net10.0`, enable nullable reference types and implicit usings, and follow the documented dependency graph:

| Project | Current direct project references | Proposed impact |
| --- | --- | --- |
| `ProdeChamp.Domain` | None | Own the scoring rule in an independent calculator. |
| `ProdeChamp.Application` | Domain | No use case, port, or wrapper needed for this isolated calculation. |
| `ProdeChamp.Infrastructure` | Application, Domain | No adapter or persistence changes. |
| `ProdeChamp.API` | Application, Infrastructure | Keep the empty host; the issue defines no HTTP contract. |
| `ProdeChamp.UnitTests` | Domain, Application | Future calculator behavior tests after plan approval, QA design, and authorization to begin development. |
| `ProdeChamp.IntegrationTests` | API, Infrastructure | No integration behavior introduced. |

Domain, Application, and Infrastructure contain no source classes. Production projects have no NuGet package references. The API only builds and runs the host. Test projects contain no test source files and already reference Microsoft.NET.Test.Sdk 17.14.1, xUnit 2.9.3, and xunit.runner.visualstudio 3.1.4.

No project references, target frameworks, SDK settings, or architectural boundaries need to change. `global.json` requests SDK 10.0.100 with `latestFeature` roll-forward and prereleases disabled. No SDK or runtime execution was performed during this task.

## Proposed classes and responsibilities

| Component | Proposed location | Responsibility |
| --- | --- | --- |
| `PredictionPointsCalculator` | `src/ProdeChamp.Domain/Scoring/PredictionPointsCalculator.cs` | Public sealed, stateless Domain service. Compare a prediction with a final result and return one integer points award. |
| `PredictionPointsCalculatorTests` | `tests/ProdeChamp.UnitTests/Scoring/PredictionPointsCalculatorTests.cs` | Future xUnit tests for approved scoring and validation behavior. This file is not created in the architecture phase. |

Proposed contract: a synchronous instance method `Calculate` accepting four named `int` parameters, `predictedHomeGoals`, `predictedAwayGoals`, `actualHomeGoals`, and `actualAwayGoals`, and returning `int`.

Four explicitly named scalar inputs suffice for this story. Home and away identify consistent positions in both score pairs. No match entities, prediction aggregates, score value objects, outcome enums, or configurable scoring policies are needed.

No calculator interface is proposed: there is one deterministic implementation, no external boundary, and no need to mock a pure rule. Tests can instantiate the concrete service directly. A later approved caller can receive it through dependency injection; this story introduces no caller that warrants DI registration.

## Implementation approach and execution flow

After human approval of this plan and completion of QA design, follow the repository's test-first workflow:

1. Write calculator tests before production implementation. Capture the expected initial failure. Distinguish a missing-type compilation failure from a behavioral failure; if a minimal nonfunctional shape is needed to reach assertions, record it and demonstrate a behavioral failure before implementing the scoring rule.
2. Implement only the Domain calculator and approved input validation.
3. Confirm approved criteria and QA scenarios pass, then refactor without changing observable behavior.
4. Execute the planned verification commands, review scope and dependency direction, and record actual results and limitations in the implementation engineering report.

Intended runtime flow:

1. Receive the predicted and actual score pairs. The caller supplies the intended final result; this calculator does not determine match completion.
2. Apply input validation once its policy is approved, as described below.
3. Compare both home values and both away values for exact equality. If both match, return 5 immediately.
4. Compare home goals with away goals within each pair using `CompareTo`. Positive, zero, and negative comparisons identify home win, draw, and away win. If the two outcomes match, return 3.
5. Otherwise return 0.

Ordered returns prevent accumulation. Direct comparison avoids unnecessary subtraction and potential overflow. Valid inputs yield only 0, 3, or 5. Repeated calls retain no total or previous result.

The operation has constant time and space costs and performs no I/O. It needs no async method or cancellation token. No mutable state or shared resources are required, so concurrent calls need no locks, transactions, or coordination.

### Proposed validation and error handling — requires approval

The issue does not define invalid inputs. Propose accepting nonnegative integer goals, including zero, and throwing `ArgumentOutOfRangeException` identifying the offending parameter when any goal value is negative. This is a proposed contract, not an approved acceptance criterion; QA must await the human decision before treating it as required.

Do not invent a maximum below `int.MaxValue`, silently clamp values, or treat invalid input as an incorrect prediction worth zero. No HTTP error mapping is needed because no endpoint is proposed. Resolve any alternative invalid-input policy before validation tests or implementation begin.

## Required dependencies

- Existing .NET 10 SDK and base class library for the implementation.
- Existing xUnit tooling in `ProdeChamp.UnitTests` for future verification.
- Human approval of this plan and resolution of the validation policy before QA begins.
- No new NuGet packages, project references, SQL Server connection, configuration, secrets, or external services.
- No EF Core, MediatR, mocking framework, or DI abstraction.

## Test scenarios for QA

These are design scenarios only. No tests are created or modified in this phase.

| ID | Traceability | Prediction | Final result | Expected result / purpose |
| --- | --- | --- | --- | --- |
| S01 | AC1, AC5 | 2-1 | 2-1 | 5, never 8; exact home win takes precedence. |
| S02 | AC2 | 2-1 | 3-0 | 3; correct home winner without exact score. |
| S03 | AC3 | 1-1 | 2-2 | 3; correct draw with different scores. |
| S04 | AC4 | 2-0 | 0-2 | 0; opposite winner. |
| S05 | Exact score rule, AC5 | 0-2 | 0-2 | 5; exact away win. |
| S06 | Exact score rule, AC5 | 0-0 and 2-2 | Matching result in each case | 5 each; exact draws receive one award. |
| S07 | Correct winner rule | 0-1 | 1-3 | 3; correct away winner without exact score. |
| S08 | Incorrect prediction rule | 1-1 | 2-1 and 1-2 | 0 each; predicted draw versus either actual winner. |
| S09 | Incorrect prediction rule | 2-1 and 1-2 | 1-1 | 0 each; either predicted winner versus an actual draw. |
| S10 | Incorrect prediction rule | 0-2 | 2-0 | 0; reversed incorrect-winner direction. |
| S11 | Zero-goal boundary | 0-0 | 1-1 | 3; zero is a valid goal value. |
| S12 | Stateless operation | Reuse one instance for S01, S04, then S01 | As above | 5, 0, 5; no retained total or influence between calls. |
| S13 | Numeric robustness, proposed integer contract | `int.MaxValue`-0 | (`int.MaxValue` - 1)-0 | 3; no arbitrary upper limit or overflow. |
| V01 | Proposed validation; pending approval | Each predicted-goal argument set to -1 in turn | Valid values elsewhere | `ArgumentOutOfRangeException` with the corresponding parameter name. |
| V02 | Proposed validation; pending approval | Valid values | Each actual-goal argument set to -1 in turn | Same exception and parameter-name expectations. |

Use xUnit parameterized cases where useful and assert observable results. No private-structure assertions, database, API test host, mocks, or integration fixtures are warranted for this pure calculation.

Planned commands for the later implementation phase, not executed during planning:

- `dotnet test tests/ProdeChamp.UnitTests/ProdeChamp.UnitTests.csproj --filter FullyQualifiedName~PredictionPointsCalculatorTests` for focused Red/Green evidence.
- `dotnet build ProdeChamp.slnx` for solution compilation.
- `dotnet test ProdeChamp.slnx` for the full existing suite after implementation. Record discovered and executed counts; zero tests in the untouched integration project do not establish integration coverage.

## Risks and assumptions

| Item | Treatment |
| --- | --- |
| Invalid score behavior is unspecified. | Obtain a human decision on the proposed contract before QA. Do not silently add it to approved acceptance criteria. |
| Exact scores also match the outcome. | Evaluate equality first and test exact wins and draws to prevent 8-point awards. |
| Positional inputs can be swapped. | Use explicit home/away names and asymmetric scenarios. Avoid a model hierarchy for hypothetical future callers. |
| Final-score interpretation across competitions is unspecified. | Assume the caller supplies the authoritative score pair. Regulation time, extra time, penalties, and match status are upstream concerns; revisit if a later story requires their interpretation. |
| No HTTP interface is requested. | Propose Domain-only delivery. Approval confirms that no endpoint or Application wrapper is required for PRODE-001. |
| Historical verification is not current verification. | Sprint 0 records successful compilation and zero discovered tests. This task does not rerun or reassert those results; future development must capture fresh evidence. |
| Restricted environments may lack access to user NuGet configuration. | Sprint 0 describes a local-cache workaround. Use an authorized environment for future checks and report access failures. No restore is needed for this plan. |

## Out of scope

The issue explicitly excludes persisting scores, rankings, authentication, and frontend.

This technical proposal also excludes HTTP endpoints and DTOs, Application orchestration, match or participant storage, total-points aggregation, recalculation jobs, configurable points tables, bonus points, competition-specific result interpretation, new infrastructure, and project-dependency changes. Any expansion requires separate scope approval.

## Open questions and human approval

1. Approve the Domain-only calculator, its four-integer synchronous contract, and the absence of an interface, API endpoint, Application wrapper, or new external dependency.
2. Should negative goals throw `ArgumentOutOfRangeException` with the offending parameter name, as proposed? Approve or replace this unspecified contract before QA designs validation cases.

No change to scoring values, category precedence, or the five approved acceptance criteria is proposed. No architectural dependency exception or additional ADR is needed for this design. Record approval and answers explicitly before advancing to QA; this document does not grant approval itself.

## Definition of Done

### Architecture phase — this deliverable

- Inspect the issue, instructions, documentation, solution, source, branch, and initial repository status.
- Record requirement traceability, affected layers, contract and flow, dependencies, QA scenarios, risks, scope boundaries, and approval questions in this plan.
- Create only `docs/stories/PRODE-001/implementation-plan.md`.
- Create no production implementation, tests, dependencies, commits, pushes, or Pull Requests.
- Stop and submit the plan for human review. Approval is required before QA begins.

### Later story delivery — after approval and QA design

- Resolve and record the validation contract and approval of this design.
- Complete QA scenario design and begin development only when authorized by the workflow.
- Capture Red–Green–Refactor evidence and implement all five issue acceptance criteria with the approved contract.
- Verify passing tests, successful solution compilation, preserved layer boundaries, and no unapproved packages.
- Review scope, correctness, category precedence, error handling, and regression risk.
- Synchronize documentation and produce an engineering report listing changed files, decisions, actual commands/results and test counts, limitations, and interview questions.
- Observe human approval boundaries for later publication; merging remains prohibited.

## Actual verification for this planning task

- GitHub CLI retrieval succeeded; requirements were inspected directly.
- Repository and source inspection succeeded. Initial Git ownership protection was resolved for read-only inspection with command-local `-c safe.directory=C:/ProdeChampIA/ProdeChampIA`; no persistent Git configuration changed.
- Build, restore, and tests: not executed. No production or test changes were requested or made. This plan claims no verified scoring implementation.

## Interview questions generated

1. Why does this rule belong in Domain, and why is an Application wrapper unnecessary for the approved scope?
2. How does evaluating exact scores first prevent category accumulation?
3. Why is an interface unnecessary here, and what later requirement would justify one?
4. How would you distinguish an approved scoring rule from a proposed invalid-input policy in test design?
5. Why should this calculation be synchronous and stateless, and what does that imply for concurrency?

These questions concern the proposed design. Implementation evidence and final interview answers belong to the later delivery phase.
