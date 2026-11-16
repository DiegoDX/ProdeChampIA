# PRODE-002 — QA report

Date: 2026-11-03.

Status: **TDD Red verified for the authorized increment.** 39 Match cases executed: **31 failed for missing behavior, 8 passed, 0 skipped**. AC3 and team validation remain deferred by explicit user direction. No Match business logic has been implemented.

## Sources and authorization

- Read [AGENTS.md](../../../AGENTS.md), including the requirement to ask when business rules are ambiguous.
- Retrieved [Issue #4 — Create Match Domain Model](https://github.com/DiegoDX/ProdeChampIA/issues/4) with `gh issue list` and `gh issue view 4 --repo DiegoDX/ProdeChampIA --json number,title,url,body,state,labels,comments,updatedAt`. It is OPEN, labelled `type:story`, updated `2026-11-03T18:58:59Z`, with no comments. No remote changes were made.
- Read the [implementation plan](implementation-plan.md), product and architecture documentation, ADR guidance, testing strategy, solution, Domain source and existing xUnit tests.
- The current user request identifies the plan as approved and authorizes QA, tests and a minimal compile-only skeleton, without business implementation. The plan's historical pending-approval wording has not been edited. No additional general plan approval is requested.
- The initially requested `$qa` skill was unavailable. The user subsequently instructed: **“no lo tomes en cuenta y usa test-design”**, resolving both the AC3 scope question and skill selection. Applied [test-design](../../../.agents/skills/test-design/SKILL.md) for traceability, risk assessment and scenario design. The original request explicitly authorizes executable xUnit tests, actual execution and a minimal production skeleton; that authorization takes precedence over the skill's design-only/no-production-code defaults. Work stops at Red.
- Working branch: `feature/PRODE-002-match-domain-model`. Existing modifications to the solution and untracked retrospective were present before QA and are preserved.

## Scope decisions and exclusions

1. **AC3 is deferred for this increment**, following the user's clarification and the plan's team deferral. The Issue criterion remains unchanged and is not claimed as covered or satisfied. No self-play, team identity, null/empty/whitespace rejection or normalization tests are introduced. M04 tests only the approved preservation of raw values and roles.
2. Use the plan's non-team contract: caller-supplied nonempty GUID; strict zero UTC offset rather than conversion; explicit status with all four defined states accepted; standard parameter-specific exceptions; read-only public properties. No validation order or clock policy is added.
3. Domain unit tests only: no SQL Server, API host, DI container, mock library or external service. SQL persistence, REST endpoints, football API synchronization and authentication remain out of scope, as do lifecycle methods, scoring integration and global ID uniqueness.

No authoring blocker remains for the authorized coverage. Requirements, GitHub acceptance criteria and the implementation plan were not edited. This report records the resolved delivery scope separately from the unchanged Issue baseline; full Issue completion is not claimed.

## Acceptance criteria coverage matrix

All automated cases are in [MatchTests.cs](../../../tests/ProdeChamp.UnitTests/Matches/MatchTests.cs), with `Trait("Story", "PRODE-002")`. Coverage here means tests were authored and executed; failed tests do not establish implemented correctness.

| Criterion / approved proposal | Plan scenarios | Expected contract | Actual coverage |
| --- | --- | --- | --- |
| AC1: A match has an identifier, home team, and away team. | M01, M04, M05, M14, M15 | Preserve caller-supplied identity, raw team values and roles; reject empty GUID. | Covered by executed Domain unit tests; behavioral Red |
| AC2: A match has a start date and time in UTC. | M01, M08, M09, M15 | Preserve exact zero-offset dates; reject nonzero offsets, including equivalent instants. | Covered by executed Domain unit tests; behavioral Red |
| AC3: A team cannot play itself. | Explicitly deferred | No inferred string-identity or validation rules. | Uncovered by user direction |
| P1: Status is Scheduled, In Progress, Finished or Cancelled. | M10, M11, M15, M16 | Preserve all four defined states; reject undefined casts. | Covered by executed Domain unit tests; behavioral Red |
| P2: Reject invalid creation information, with team validation deferred. | M05, M08, M11, M12, M15 | Parameter-specific non-team guards; combined invalid inputs without required guard order; no public mutation bypass. | Covered within authorized non-team scope; behavioral Red |
| P3: Domain logic is testable without a database. | M13 and all construction tests | Direct construction with deterministic in-memory inputs and independent state; no global uniqueness lookup. | Tests execute without external dependencies; state behavior Red |
| Existing scoring regression | R01 | Execute both existing PRODE-001 test classes unchanged. | 44 passed |

## Risk assessment and detailed test cases

Highest risks are accepting invalid non-team state, silently converting UTC input, losing supplied values and allowing public mutation to bypass guards. P0 covers essential representation/invariants; P1 covers important boundary and isolation risks; P2 covers supplementary enum shape. All cases below are **Ready / automated**, at Domain unit level, targeting `Match` except M16 (`MatchStatus`). Every row uses independent fixed inputs, the existing .NET 10/xUnit tooling, and no external fixture. Counts include each theory data row.

For each case: **Given** the data below, **When** the constructor is called (or the public shape is inspected for M15/M16), **Then** the stated result is required.

| ID / traceability / priority | Data and boundary rationale | Expected outcome / test method | Actual passed / failed |
| --- | --- | --- | --- |
| M01 / AC1, AC2, P1 / P0 | Fixed nonempty GUID, Argentina/Bolivia, `2026-06-11T19:00:00+00:00`, Scheduled. | Preserve all five properties and zero offset. `Constructor_WithValidInputs_PreservesAllProperties` | 0 / 1 |
| M04 / AC1, team deferral / P1 | `"  ArGenTina "`, `"\tBOLIVIA  "`, then swap their roles. | Preserve exact spelling, casing, whitespace and roles. `Constructor_WithRawTeamValues_PreservesValuesAndRoles` | 0 / 2 |
| M05 / AC1, P2 / P0 | Only `id` is `Guid.Empty`; other inputs valid. | Exact `ArgumentException`, `ParamName = "id"`. `Constructor_WithEmptyId_ThrowsArgumentExceptionForId` | 0 / 1 |
| M08 / AC2, P2 / P0 | Same instant as fixed kickoff via `ToOffset`; offsets in minutes: -840, -240, -1, 1, 330, 345, 840. Covers both extremes, nearest nonzero offsets and fractional-hour offsets. | Exact `ArgumentException`, `ParamName = "startTimeUtc"`; reject rather than normalize. `Constructor_WithNonzeroOffset_ThrowsArgumentExceptionForStartTimeUtc` | 0 / 7 |
| M09 / AC2 / P1 | Default (= MinValue), MinValue + 1 tick, 1960-01-01, leap day 2000-02-29, 2099-12-31, MaxValue - 1 tick, MaxValue; all zero-offset. | Preserve exact timestamp and offset without date-window or precision restrictions. `Constructor_WithUtcDateBoundaries_PreservesExactStartTime` | 1 / 6 |
| M10 / P1 / P0 | Each of Scheduled, InProgress, Finished, Cancelled. | Accept and preserve the supplied state. `Constructor_WithEachDefinedStatus_PreservesStatus` | 1 / 3 |
| M11 / P1, P2 / P0 | Enum casts -1, 4, `int.MinValue`, `int.MaxValue`. | Exact `ArgumentOutOfRangeException`, `ParamName = "status"`. `Constructor_WithUndefinedStatus_ThrowsArgumentOutOfRangeExceptionForStatus` | 0 / 4 |
| M12 / P2 / P1 | Invalid ID + offset; ID + status; offset + status; all three. | Throw `ArgumentException` or a derived argument exception; no first-error ordering contract. `Constructor_WithMultipleInvalidNonTeamInputs_RejectsWithoutRequiringGuardOrder` | 0 / 4 |
| M13 / P3 / P1 | Two constructions reuse the same nonempty GUID but use different teams, UTC dates and statuses. | Distinct instances retain independent supplied state; no global ID check or database. `Constructor_WhenIdIsReused_CreatesIndependentMatchesWithoutExternalState` | 0 / 1 |
| M14 / AC1, P2 / P1 | GUID with only the lowest bit set; GUID with all bits set. | Preserve both nonempty values; no inferred GUID-version constraint. `Constructor_WithNonemptyGuidBoundaries_PreservesId` | 0 / 2 |
| M15 / read-only invariant / P0 | Inspect Id, HomeTeam, AwayTeam, StartTimeUtc and Status. | Each property has a public getter and no public setter or init accessor. `PublicState_HasNoPublicSetterOrInitAccessor` | 5 / 0 |
| M16 / P1, approved API / P2 | Inspect enum names and values. | Exactly Scheduled=0, InProgress=1, Finished=2, Cancelled=3. `MatchStatus_DefinesExactlyTheFourApprovedValues` | 1 / 0 |

M02/M03 (self-play) and M06/M07 (team validation) remain deferred as in the plan. R01 is the unchanged PRODE-001 scoring suite, verified separately below. Tests assert exception types and parameter names, not localized messages. No arbitrary time limit, goal cap, team rule or status transition is added.

## Test-first sequence and minimal skeleton

1. Wrote `MatchTests.cs` before adding either production file. The API did not previously exist; no compile/discovery failure is counted as Red.
2. Added [Match.cs](../../../src/ProdeChamp.Domain/Matches/Match.cs) with the approved constructor signature, an **empty constructor body**, and get-only properties left at CLR defaults. `null!` initializers suppress nullable warnings for unimplemented string properties; they provide no runtime validation or assignment of input values.
3. Added [MatchStatus.cs](../../../src/ProdeChamp.Domain/Matches/MatchStatus.cs) containing only the four approved enum members so tests compile. No guards, factories, setters, state transitions, normalization or other business logic were implemented.
4. Executed focused Red, inspected all failure messages in the TRX, built the full solution and executed full regression. No test was weakened or changed after observing Red.

The first recommended Green increment is M01 (input preservation), followed by M04/M09/M10/M13/M14, then M05/M08/M11/M12 guards while retaining M15/M16. Future implementation should proceed in small Red–Green–Refactor steps; this task does not authorize Green.

## Actual commands and results

Executed from the repository root with these environment settings:

```powershell
$env:DOTNET_CLI_HOME=Join-Path (Get-Location) '.local/dotnet'
$env:DOTNET_SKIP_FIRST_TIME_EXPERIENCE='1'
$env:DOTNET_CLI_TELEMETRY_OPTOUT='1'
$env:DOTNET_ADD_GLOBAL_TOOLS_TO_PATH='0'
dotnet test tests/ProdeChamp.UnitTests/ProdeChamp.UnitTests.csproj --no-restore --filter 'Story=PRODE-001' --logger 'trx;LogFileName=PRODE-002-qa-baseline.trx' --results-directory tests/TestResults --verbosity minimal
```

| Execution | Exit code | Actual outcome |
| --- | --- | --- |
| Initial sandbox execution | 1 | Build blocked by MSB3491: access denied writing a temporary assembly-info cache under `src/ProdeChamp.Domain/obj/Debug/net10.0`. No tests executed; this is an environment failure, not TDD Red. |
| Same command rerun outside the sandbox after runtime approval | 0 | Domain, Application and UnitTests built successfully. **44 passed, 0 failed, 0 skipped, 44 total**; reported test duration 194 ms. |
| Focused Red, initial sandbox attempt | 1 | MSB3491 access denied writing `ProdeChamp.Domain.csproj.CoreCompileInputs.cache` under `obj`; no tests ran. Environment failure, not Red. |
| Focused Red, approved rerun outside sandbox | 1 | **39 executed: 31 failed, 8 passed, 0 skipped**, duration 58 ms. Build/discovery succeeded; every failure is an expected assertion about missing assignment or guards. |
| Full solution build outside sandbox | 0 | All six projects built; **0 warnings, 0 errors**. |
| Full solution regression, sandbox / quiet output | 1 | Unit summary: **83 total, 52 passed, 31 failed, 0 skipped**. Requested TRX files were absent; reran with runtime approval to retain evidence. |
| Full solution regression, approved rerun / minimal output | 1 | Unit tests: **83 executed, 52 passed, 31 failed, 0 skipped**, duration 60 ms. All **44 existing PRODE-001 tests passed** and all 31 failures belong to Match. Integration discovered/executed **0 tests**. |

Commands after the baseline, with the same four environment settings above:

```powershell
# Focused Red; first blocked in sandbox, then executed with runtime approval.
dotnet test tests/ProdeChamp.UnitTests/ProdeChamp.UnitTests.csproj --no-restore --filter 'Story=PRODE-002' --logger 'trx;LogFileName=PRODE-002-qa-red.trx' --results-directory tests/TestResults --verbosity minimal

dotnet build ProdeChamp.slnx --no-restore --verbosity minimal

# First attempted with --verbosity quiet; rerun below retains TRX evidence.
dotnet test ProdeChamp.slnx --no-build --no-restore --logger 'trx;LogFilePrefix=PRODE-002-qa-regression' --results-directory tests/TestResults --verbosity minimal

dotnet msbuild src/ProdeChamp.Domain/ProdeChamp.Domain.csproj -getItem:Compile -getProperty:EnableDefaultCompileItems
dotnet msbuild tests/ProdeChamp.UnitTests/ProdeChamp.UnitTests.csproj -getItem:Compile -getProperty:EnableDefaultCompileItems
git -c safe.directory=C:/ProdeChampIA/ProdeChampIA diff --check
```

Local evidence under `tests/TestResults/`:

- `PRODE-002-qa-baseline.trx`: 44 passed.
- `PRODE-002-qa-red.trx`: 39 total, 8 passed, 31 failed.
- `PRODE-002-qa-regression_net10.0_20261103164124.trx`: integration, 0 executed.
- `PRODE-002-qa-regression_net10.0_20261103164125.trx`: unit suite, 83 total, 52 passed, 31 failed.

Generated results remain ignored and are not solution items. Local execution establishes no CI result.

### Why this is behavioral Red

Inspection of every focused failure shows only these expected assertion groups:

| Failure group | Count | Missing behavior |
| --- | --- | --- |
| `Assert.Throws`: no exception was thrown | 12 | Empty-ID guard (1), nonzero-offset guard (7), undefined-status guard (4). |
| `Assert.ThrowsAny`: no exception was thrown | 4 | Rejection of combinations of invalid non-team inputs. |
| `Assert.Equal`: values differ | 13 | ID preservation (M01/M13/M14: 4), UTC timestamp preservation (6), nondefault status preservation (3). |
| `Assert.Equal`: strings differ | 2 | Raw team value/role preservation. |

The eight passes are expected: five public read-only checks, enum shape, default/MinValue UTC preservation, and Scheduled status (the enum default). Default-valued assertions alone do not demonstrate working construction. There are no compile, fixture, discovery, database or unexpected-exception failures in the verified Red run.

## Changes and technical decisions

- Added `tests/ProdeChamp.UnitTests/Matches/MatchTests.cs`: 12 test methods expanding to 39 cases.
- Added only the compile-time skeletons `src/ProdeChamp.Domain/Matches/Match.cs` and `MatchStatus.cs`.
- Updated this QA report, including coverage design, actual Red evidence, limitations and interview questions; added its solution item under `/docs/stories/PRODE-002/`.
- Preserved all six existing solution projects and pre-existing solution/retrospective changes.
- No packages, project references, explicit compile entries or architectural abstractions were added. Domain remains dependency-free. Existing scoring source/tests are unchanged.
- Kept actual execution results separate from proposed tests; no compilation or infrastructure failure is labelled as behavioral Red.
- No commit, push, Issue update or PR creation was performed.

### Structural verification

MSBuild evaluation confirms `EnableDefaultCompileItems = true` in both owning projects and includes each new C# file exactly once through SDK defaults. No project-file change is necessary for Solution Explorer inclusion. The first metadata query lacked `DOTNET_ADD_GLOBAL_TOOLS_TO_PATH=0` and failed on a denied toolpath-sentinel write; the approved rerun used all four environment settings and succeeded. This was an inspection/environment failure and executed no tests.

Solution XML, linked-file existence, retention of all six projects and diff hygiene were checked. The QA report is a solution item; generated TRX/build artifacts are excluded. Visual Studio UI was not inspected.

## Limitations and handoff

The authorized QA/Red deliverable is complete. The production `Match` type is intentionally nonfunctional and must not be used as a completed domain model. AC3 and team validation remain uncovered; full Issue completion and Green are not claimed. Parameter-name assertions are present but cannot be reached successfully until guards exist. Integration has zero coverage, appropriately for this Domain-only increment. No CI run, coverage percentage, Visual Studio UI verification or product sign-off is claimed.

Handoff: 39 automated cases ready for later implementation, 31 in verified Red and 8 already passing due to skeleton shape/defaults; AC3 and team-validation scenarios deferred by user direction. Later authorized development should implement only the approved assignments/guards, preserve these assertions and rerun focused tests and regression. No further approval is requested for this completed QA stage.

## Interview questions generated

1. Why is a build or environment failure insufficient evidence of TDD Red?
2. Why must a `DateTimeOffset` equality assertion also check the UTC offset?
3. How can UTC offsets and date boundaries be tested deterministically without a clock service or database?
4. Why are default-valued assertions insufficient to prove a constructor stores its inputs?
5. How can combined-invalid-input tests verify rejection without fixing validation order?
6. How should QA track a deliberately deferred acceptance criterion without rewriting the Issue?

These are preparation questions based on the inspected contract and verification process; no completed Match implementation is claimed.
