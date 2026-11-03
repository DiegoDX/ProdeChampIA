# PRODE-002 — Match Domain Model: implementation plan

Status: Revised architecture; P1–P3 approved by the user. Team logic and validation deferred by user direction. The revised plan awaits human approval; QA and development have not begun.

Date: 2026-11-03.

GitHub Issue: [#4 — PRODE-002 — Create Match Domain Model](https://github.com/DiegoDX/ProdeChampIA/issues/4).

## Source and repository verification

- Read [AGENTS.md](../../../AGENTS.md) and the [architect skill](../../../.agents/skills/architect/SKILL.md).
- Retrieved Issue #4 and its comments using `gh issue view`, following discovery with `gh issue list`. At retrieval it was OPEN, labelled `type:story`, updated `2026-11-03T18:58:59Z`, with no comments. This plan does not infer approval of proposed rules from that label or from the request to prepare a design.
- Read the [product documentation](../../product/README.md), [story index](../../user-stories/README.md), [architecture](../../architecture/README.md), [ADR guidance](../../architecture/decisions/README.md), and [testing strategy](../../testing/README.md). These sources contain no numbered ADR or additional approved match rules.
- Reviewed the [PRODE-001 plan](../PRODE-001/implementation-plan.md), [validation report](../../engineering-reports/PRODE-001-validation-engineering-report.md), calculator, both scoring test classes, all six project files, API host, solution and SDK configuration. Retrieved Issue #1, which is CLOSED. Historical documentation still describes its PR as pending; local HEAD contains its merge.
- Initial branch: `main`, HEAD `208e2288183a24d580ee95f8ff89f442ca38f0d5` (merge of PRODE-001 PR #3). Created local branch `feature/PRODE-002-match-domain-model` for this work without committing or publishing.
- At this revision, the branch is `feature/PRODE-002-match-domain-model`. Pre-existing work includes modified `ProdeChamp.slnx` and untracked retrospective and PRODE-002 documentation. Preserve it. This revision modifies only this existing plan; its solution item is already present.
- Re-retrieved Issue #4 for this revision: OPEN, updated `2026-11-03T18:58:59Z`, with unchanged criteria and no comments. Reviewed current product, architecture, ADR and testing guidance, all six project files, Domain source, API host and solution.

## User decisions recorded on 2026-11-03

- **P1 approved:** include `MatchStatus` with Scheduled, In Progress, Finished and Cancelled, represented in C# as `Scheduled`, `InProgress`, `Finished` and `Cancelled`.
- **P2 approved:** reject invalid creation input, with team-property logic and validation excluded by the user's specific instruction. The remaining detailed guard contracts below await approval of this revised plan.
- **P3 approved:** keep the Domain model testable without a database.
- **Team handling deferred:** retain `HomeTeam` and `AwayTeam` as provisional string properties and assign supplied values directly. Do not trim, normalize, compare, resolve identities, or add null/empty/whitespace guards or self-play validation in this increment; the representation will change later.

The GitHub acceptance criteria remain unchanged. Deferring team validation leaves AC3 (a team cannot play itself) unenforced in this proposed increment. This is an explicit delivery gap for human review, not a claim that AC3 is satisfied. Approval of P1–P3 does not approve the whole revised plan or authorize QA, tests or implementation.

## Business requirements

The administrator needs to represent tournament matches with their teams, start date and status so that participants can make predictions. PRODE-002 supplies an in-memory Domain model; it does not implement prediction entry or tournament administration.

### Acceptance criteria — unchanged Issue baseline

The Issue's **Acceptance Criteria** section contains exactly these three criteria:

| ID | Criterion |
| --- | --- |
| AC1 | A match has an identifier, home team, and away team. |
| AC2 | A match has a start date and time in UTC. |
| AC3 | A team cannot play itself. |

### Additional Issue proposals — user-approved scope

The Issue's **Business Rules** section is headed “Proposed acceptance criteria”. It repeats AC1–AC3 and additionally proposes:

| ID | Proposal from the Issue | Treatment in this design |
| --- | --- | --- |
| P1 | It has a status: Scheduled, In Progress, Finished, or Cancelled. | Approved by the user: include the four-value `MatchStatus` enum. |
| P2 | The creation process must reject invalid information. | Approved by the user, excluding team logic and validation in this increment. Propose guards for ID, UTC offset and enum validity below. |
| P3 | The domain logic must be testable without a database. | Approved by the user: keep the model in dependency-free Domain and instantiate it directly in later unit tests. |

These approvals are recorded separately from unchanged AC1–AC3. The remaining technical contracts and the explicit AC3 delivery gap require review of this revised plan before QA begins. The GitHub Issue remains unchanged.

## Existing architecture impact and affected projects

All six projects target `net10.0` with nullable reference types and implicit usings. Domain has no package or project references. Application references Domain; Infrastructure references Application and Domain; API references Application and Infrastructure. Test projects remain outside production dependency boundaries.

| Project/component | Planned impact after approval |
| --- | --- |
| `ProdeChamp.Domain` | Later add `Matches/Match.cs` and `Matches/MatchStatus.cs`, namespace `ProdeChamp.Domain.Matches`, after revised-plan approval and development authorization. |
| `ProdeChamp.UnitTests` | Later add `Matches/MatchTests.cs` using the existing Domain reference and xUnit tooling. No test files are created in this phase. |
| `PredictionPointsCalculator` and its two test classes | Preserve the four-integer scoring contract and validation. Match construction does not invoke scoring. |
| Application, Infrastructure, API, IntegrationTests | No changes; this story introduces no use case, external boundary or I/O. |
| `ProdeChamp.slnx` | Link this plan under `/docs/stories/PRODE-002/` and retain existing projects and solution items. Link future documentation when created. |
| Domain and UnitTests project files | SDK default compile inclusion suffices for future C# files. Verify visibility through their owning projects; if explicit visibility metadata is necessary, use `Compile Update`, never duplicate `Compile Include` entries. |

No project, reference, target framework, DI registration or architecture exception is needed. This addition follows existing boundaries and does not warrant a separate ADR unless approval changes the design materially.

## Proposed entities, properties and methods

### `Match`

A public sealed entity representing one match, with constructor guards for ID, UTC offset and status, and public get-only properties. Team properties have no runtime validation in this increment. Its identity is the supplied `Id`; no equality override, entity base class or aggregate abstraction is needed.

| Property | Proposed type | Responsibility |
| --- | --- | --- |
| `Id` | `Guid` | Caller-supplied, nonempty match identity. No database or ID generator is required. |
| `HomeTeam` | `string` | Provisional home-team value, stored exactly as supplied without logic or validation. |
| `AwayTeam` | `string` | Provisional away-team value, stored exactly as supplied without logic or validation. |
| `StartTimeUtc` | `DateTimeOffset` | Start instant with an explicit zero UTC offset. |
| `Status` | `MatchStatus` | One of the four user-approved states. |

Proposed sole public construction contract: `Match(Guid id, string homeTeam, string awayTeam, DateTimeOffset startTimeUtc, MatchStatus status)`. The constructor validates ID, UTC offset and status and assigns every property; team values are assigned directly. No separate factory or `Validate` method is needed. Requiring an explicit status avoids inventing a rule that every match must initially be Scheduled. Every defined status is accepted at construction, subject to approval of this detailed contract.

Strings remain a provisional representation because the repository has no Team model or catalogue and the user intends to change team properties later. Do not establish name-based identity or equality rules now. Preserve spelling, casing, whitespace and home/away roles exactly as supplied. Existing non-nullable `string` annotations provide compile-time guidance only; no runtime non-null guarantee is claimed for these properties. Team identity, validation and AC3 enforcement are deferred for a later approved design.

No public setters, init accessors, status-change methods, rescheduling methods or result-recording methods are proposed. The Issue requires representation, not a lifecycle. Get-only state preserves validated ID, UTC and status invariants; it does not establish team validity.

### `MatchStatus` — approved under P1

Public enum with explicit members `Scheduled = 0`, `InProgress = 1`, `Finished = 2`, `Cancelled = 3`. `InProgress` represents the Issue's “In Progress”. These are internal .NET values, not a persistence or HTTP serialization contract. Undefined cast values must be rejected; zero is a legitimate Scheduled value.

No `Team` entity, value object, repository, interface, domain service, event or DI abstraction is justified by the current criteria.

## Domain invariants and validation — revised proposed contract

P2 is approved; the detailed non-team guards below remain the proposed technical contract for final plan review. Fail construction for invalid ID, UTC offset or enum values before returning an instance. Do not silently repair input. Team logic and validation are explicitly deferred; AC3 is not enforced by these guards.

| Input/rule | Proposed observable behavior | Traceability |
| --- | --- | --- |
| `id == Guid.Empty` | `ArgumentException`, `ParamName = "id"`. | AC1; proposed valid-identity interpretation. |
| `startTimeUtc.Offset != TimeSpan.Zero` | `ArgumentException`, `ParamName = "startTimeUtc"`; require caller-supplied UTC rather than silently converting offsets. | AC2; strict UTC proposal. |
| Undefined `MatchStatus`, including negative and large cast values | `ArgumentOutOfRangeException`, `ParamName = "status"`. | P1, P2. |
| Inputs meeting the non-team guards | Preserve supplied ID, raw team values, home/away roles, UTC instant and selected status. | AC1, AC2, P1; team validity and AC3 deferred. |

Use standard .NET exceptions and guards, consistent with PRODE-001's parameter-specific validation; no validation package or custom exception hierarchy. Assert exception types and `ParamName`, not localized messages. When multiple inputs are invalid, require rejection without making validation order a public contract.

No rule requires a future kickoff, prohibits a historical date or rejects `default(DateTimeOffset)` solely because it is the default value. That value represents a UTC instant; the proposed contract accepts it. No clock service or date window is introduced. Team trimming, normalization, equality checks, null/empty/whitespace guards, name-length limits, alias resolution and self-play rejection are excluded from this increment. Home/away order is preserved. Global match-ID uniqueness cannot be enforced by a standalone entity and remains outside scope.

## Implementation approach and execution flow

After plan approval and resolution of the open questions:

1. QA translates approved criteria and contracts into cases, distinguishing unchanged Issue criteria, user-approved P1–P3 and the explicit team-validation deferral. Do not create self-play tests for this increment or report AC3 as covered; first resolve its delivery treatment at plan approval.
2. When development is authorized, write focused tests first. Record compilation/discovery failures separately from behavioral RED; if a minimal nonfunctional public shape is needed, demonstrate expected behavioral failures before completing the implementation.
3. Implement only the approved entity, enum and guards in Domain. Confirm GREEN and refactor where useful; keep PRODE-001 source and tests unchanged.
4. Build the solution, run focused tests and regression, inspect dependency direction and project visibility, and produce an engineering report with actual results. Plan approval does not authorize publication.

Runtime flow: the caller supplies identity, provisional team values, UTC time and status; the constructor validates ID, UTC offset and the enum, assigns team values unchanged, and exposes read-only properties. It performs no team comparison or validation. No I/O or scoring call occurs.

Construction is synchronous. The proposed guards and direct assignments require constant work with no team-string processing, collection scan or database query. After construction, read-only state needs no locks. Duplicate IDs, schedule conflicts, concurrent edits, transactions and persistence concurrency require future rules and are not solved here.

## Test scenarios for QA

Design scenarios only; no tests are written, changed or run in this phase. Scenarios using proposed representations or guards become binding only after revised-plan approval. Former scenarios M02/M03 (self-play comparisons) and M06/M07 (team null/empty/whitespace rejection) are deferred and must not become tests in this increment. AC3 remains uncovered.

| ID | Traceability | Scenario | Expected result under the proposed contract |
| --- | --- | --- | --- |
| M01 | AC1, AC2 | Nonempty GUID, distinct names, zero-offset time and Scheduled status. | Properties preserve the approved input representation and home/away roles. |
| M04 | AC1, user-directed team deferral | Distinct supplied team strings contain casing and surrounding whitespace; repeat with home/away swapped. | Preserve raw values and supplied roles exactly; no normalization. |
| M05 | AC1, P2 | Empty GUID. | `ArgumentException` for `id`. |
| M08 | AC2 | Nonzero positive and negative offsets, including an instant equivalent to valid UTC input. | `ArgumentException` for `startTimeUtc`. |
| M09 | AC2, no date-window rule | Fixed historical and future UTC instants and zero-offset representable boundaries, including `default(DateTimeOffset)`. | Accept and preserve the instant; no machine-clock or local-timezone dependency. |
| M10 | P1 | Construct with each of Scheduled, InProgress, Finished and Cancelled. | Accept and retain each state; no assumed initial-state policy. |
| M11 | P1, P2 | Undefined enum casts, such as -1 and `int.MaxValue`. | `ArgumentOutOfRangeException` for `status`. |
| M12 | P2 | Multiple invalid non-team inputs together (ID, UTC offset, status). | Reject construction; no required first-error order. |
| M13 | P3, isolation | Construct separate matches directly with fixed inputs and no database, host, mocks or container. | Independent state; reusing an ID does not imply a global uniqueness check. |
| R01 | PRODE-001 regression | Execute both existing scoring test classes unchanged. | Existing scoring/negative-goal behavior stays passing. Historical evidence reports 44 cases; record fresh counts later. |

Use existing xUnit conventions, parameterized cases where useful, public-behavior assertions and a `Story = PRODE-002` trait. No integration fixture is warranted.

Planned later commands, **not executed during planning**:

- `dotnet test tests/ProdeChamp.UnitTests/ProdeChamp.UnitTests.csproj --filter FullyQualifiedName~ProdeChamp.UnitTests.Matches.MatchTests` for focused RED/GREEN evidence.
- `dotnet build ProdeChamp.slnx` for compilation.
- `dotnet test ProdeChamp.slnx` for regression, reporting executed/passed/failed/skipped counts. The currently empty integration project supplies no integration coverage.

## Dependencies and risks

| Dependency/risk | Treatment |
| --- | --- |
| Human approval of revised plan | P1–P3 are approved. Remaining technical contracts and AC3 delivery treatment require plan approval before QA. Record decisions here without rewriting Issue criteria. |
| Existing .NET 10 SDK/base class library | Sufficient for GUIDs, strings, UTC offsets, enums and exceptions. No new packages or project references. |
| Existing unit-test tooling | Microsoft.NET.Test.Sdk 17.14.1, xUnit 2.9.3, xunit.runner.visualstudio 3.1.4 already present. No upgrade proposed. |
| Deferred team validation and identity | Raw team values can be null at runtime, empty, whitespace-only or identical. No team-validity guarantee is made. The planned increment does not enforce AC3; review this delivery gap before QA and revisit team representation in a later approved design. |
| UTC interpretation | `DateTimeOffset` permits nonzero offsets, so the guard is essential. Rejection versus normalization requires approval. |
| Status construction contract | P1's enum is approved separately from formal Issue criteria. Explicit status, acceptance of all defined states and representation-only scope remain proposed details; no transition or initial-state rule is inferred. |
| Global uniqueness and schedule conflicts | Entity has no external state. Global validation requires future use cases and persistence. |
| Historical verification | PRODE-001 reports 44 passing tests; this planning task does not rerun them or assert current test results. |
| Existing local changes and stale summaries | Preserve pre-existing work; do not expand into retrospective or documentation cleanup. |

PRODE-001 is a reviewed baseline, not a runtime dependency of `Match`. The Issue lists no explicit story dependencies. No SQL Server, football API, Angular, external service, secret, EF Core, MediatR, clock abstraction or mocking framework is needed.

## Out of scope

The Issue explicitly excludes SQL persistence, REST endpoints, synchronization with football APIs and authentication.

This proposal also excludes Team/Tournament catalogues or entities, tournament membership rules, predictions and deadlines, goal/result storage, scoring integration, standings, frontend, repositories, DTOs, Application wrappers, DI registrations, status transitions, cancellation workflows, rescheduling, domain events, global uniqueness, schedule-conflict checks and concurrency infrastructure. Team-property logic, normalization, validation and self-play enforcement are deferred by the user's latest direction, with the resulting AC3 gap explicitly recorded. New rules or dependencies require separate approval.

## Open questions and human approval

P1–P3 are approved and do not require another scope confirmation. The following points remain for approval of the revised plan:

1. **AC3 delivery gap:** The user directed deferral of team logic and validation. Confirm that this increment may proceed with AC3 explicitly deferred and without claiming full satisfaction of Issue #4. The plan preserves the criterion and does not update the Issue. No team-identity design is required now.
2. **Construction and errors:** Approve caller-supplied nonempty GUIDs, the non-team exception/parameter contract and strict zero-offset `DateTimeOffset` input. Confirm no date-window rule; alternatively specify normalization or additional date validity requirements.
3. **Status and mutability:** Approve explicit valid status at construction, accepting all four states, get-only properties and no lifecycle methods. The four enum members themselves are already approved. An initial Scheduled-only rule or transitions require requirements before QA.

Resolve these points and approve the plan before QA begins. This document does not grant its own approval. No dependency exception is requested. No Issue update, commit, push or PR is performed or authorized by this plan.

## Definition of Done

### Architecture phase — current deliverable

- Inspect instructions, Issue, product/architecture/ADR context, source, solution, branch and working-tree status.
- Revise this plan with unchanged criteria, recorded P1–P3 approvals, deferred team validation and explicit AC3 gap, contracts, affected components, flow, QA scenarios, risks, scope and approval questions.
- Verify the existing solution link, linked-file existence and retention of all six projects; preserve pre-existing changes without editing the solution in this revision.
- Create no production code or tests and perform no commits, pushes or PR creation.
- Stop for human review; QA and Developer phases remain unstarted.

### Later story delivery — after approval

- Obtain revised-plan approval, including treatment of the AC3 gap and remaining UTC, validation and status contracts; finish QA design before authorized TDD work. Do not claim full Issue completion while AC3 remains deferred.
- Capture meaningful RED/GREEN evidence, implement approved contracts and preserve PRODE-001 behavior.
- Verify compilation, focused tests, regression, Domain independence and project visibility using actual results.
- Review changes and synchronize documentation, including an engineering report with changed files, decisions, actual checks/results, limitations and interview questions.
- Respect publication approval boundaries; agents never merge PRs.

## Actual verification and limitations of this planning task

- GitHub CLI retrieval and repository/source inspection completed. No remote write was performed.
- Only this plan is modified in this revision. Its existing solution item and other pre-existing local work are preserved.
- Structural checks cover solution XML, linked-file existence, project retention, document links and diff hygiene. Visual Studio UI is not inspected.
- Build, restore and tests are not executed. No implementation or RED/GREEN evidence exists for PRODE-002; no proposed match behavior is claimed to pass tests.
- P1–P3 approvals are recorded. Remaining technical details and the explicit AC3 delivery gap await revised-plan approval. This plan is the architecture deliverable; implementation reporting belongs to the later phase.

## Interview questions generated

1. Why does match validation belong in Domain while global ID uniqueness belongs elsewhere?
2. Why defer team-name validation when the representation will change, and how should an unmet acceptance criterion be tracked?
3. Why use `DateTimeOffset` with a zero-offset guard, and how would normalization change the contract?
4. Which guarantees do constructor guards and get-only properties provide when team validation is explicitly deferred?
5. How should QA distinguish formal Issue criteria, separately approved proposals and deferred behavior?

These concern the proposed design; no implementation evidence or interview answers are claimed.
