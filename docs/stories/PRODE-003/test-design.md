# PRODE-003 — Test design

- Version: **1**, 2026-11-17.
- Status: **APPROVED by the user** — “si esta bien”, in direct response to the QA-v1 approval request. Approved original SHA256: `1219B42D1FF801BD6913AFB5CDCE6CCC636FD7B3991CC4B0E7F81306FD8C6E37`. This annotation changes no test cases or expected outcomes. Executable work now proceeds under this approved design; design-stage statements below retain their historical meaning.
- Owner: primary agent applying `test-design`; this is design, not independent review.
- Issue: [#5 — Registrar predicciones de partidos](https://github.com/DiegoDX/ProdeChampIA/issues/5), refreshed `2026-11-17T22:45:16Z`; OPEN, `updatedAt` `2026-11-12T21:12:59Z`, no comments, criteria unchanged.
- Plan basis: [implementation plan v1](implementation-plan.md), approved by the user's “esta bien” in direct response to the scoped plan-approval question, including caller-context duplicate checking. Original approved document SHA256 before approval annotation: `3C3F2CB952E87C81B7925D101D52CFB03302C4F70126E34BAC9FC08D6741515C`.
- Source: `feature/PRODE-003`, HEAD/base `273690364bd014a13aff8ecfa394b1dcbed76df8`, plus this story's uncommitted documentation/solution entries only.

## 1. Story reference and test basis

The unchanged five acceptance criteria and seven business rules are faithfully retained in [workflow-status.md](workflow-status.md). Separately approved clarifications: reject a null/empty participant identifier, require a valid Match, reject duplicates without an in-memory registry. The approved plan supplies a static immutable Prediction factory, valid existing Match input, explicit UTC timestamp and a complete caller-provided active-prediction snapshot. Tests must not silently strengthen that boundary into catalog, durable or distributed existence/uniqueness guarantees.

Reviewed repository inputs: AGENTS, shared factory contract, refinement/plan, product/architecture/testing docs, current Match and scoring source, Match unit-test conventions, net10.0 project references and existing xUnit/Test SDK/adapter versions. The Match constructor already rejects Guid.Empty and exposes get-only properties; a null Match is the invalid prediction-factory input. Existing Match tests remain unchanged and are part of later regression verification.

## 2. Scope and exclusions

All new executable cases are **Domain unit tests** for `Prediction.Create` in the existing `ProdeChamp.UnitTests` project. Proposed automation file: `tests/ProdeChamp.UnitTests/Predictions/PredictionTests.cs`; use `[Trait("Story", "PRODE-003")]`, existing xUnit Fact/Theory conventions, explicit fixtures and exception ParamName assertions. Split the file only if readability warrants it; keep case IDs traceable.

No Application orchestration, API, SQL Server, Angular, authentication or E2E boundary is introduced. No new framework, package, external fixture or secret is needed. Existing integration project may discover zero tests; that must be reported accurately, not described as integration coverage.

## 3. Ambiguities and blockers

No unresolved behavior decision within the approved plan. QA must still receive the human approval required by the shared factory contract before executable test implementation. This design's approval is not inferred from approval of the earlier plan.

Boundaries already accepted with plan v1:

- Participant validity is null/nonempty identifier validation; no participant catalog lookup.
- A supplied valid Match is required; no database/catalog membership check.
- Active predictions are supplied by the caller; no lifecycle flag/filtering/storage is invented.
- Duplicate rejection is conditional on the supplied complete/current snapshot. No concurrent/global uniqueness guarantee is tested or claimed.
- Caller supplies the actual registration instant; domain requires and preserves a zero UTC offset. No clock reads, deadline locks or future/past-date policy.

## 4. Risk assessment

| Risk | Priority | Planned mitigation |
| --- | --- | --- |
| Duplicate participant/match accepted, or different pairs rejected | P0 | Same/different score, equal IDs in different Match objects, irrelevant entries before duplicate, each independent pair component. |
| Invalid identity, goals or missing context accepted | P1 | Isolated invalid inputs with approved exception type/parameter assertions; null collection must not mean empty context. |
| Roles/identifiers/timestamp lost or altered | P1 | Happy-path property assertions, distinct home/away scores and exact UTC tick/offset checks. |
| Hidden registration or mutation added despite user rejection | P1 | Mutable input collection preserved after both success and rejection; repeated calls with unchanged context stay stateless. |
| Undocumented numeric/time/status constraint | P2 | Zero/int.MaxValue goals, UTC date bounds and all existing Match statuses accepted. |
| Hidden point calculation or invariant bypass path | P1 review obligation | Review source/public contract and dependencies; do not create brittle synthetic behavior tests for absent methods. |

## 5. Acceptance coverage matrix

“Covered” here means **designed**, not executed or verified.

| AC / rule | Cases or review checks | Level | Design coverage |
| --- | --- | --- | --- |
| AC-01, BR1/2/6 | P01, P02, P05, P15, P16, P19 | Domain unit | Covered within approved Match/input boundary. |
| AC-02, BR3 | P02, P07, P08 | Domain unit | Covered, including each negative argument independently. |
| AC-03 | P03, P04, P05 | Domain unit | Covered for null/empty/nonempty GUID semantics. |
| AC-04, BR1 | P06 plus unchanged MatchTests constructor guards | Domain unit / regression | Covered within caller-held Match boundary; external catalog untested by design. |
| AC-05, BR4 | P01, P09, P10 | Domain unit | Covered; approved zero-offset contract and exact preservation. |
| BR5 | P11–P18 | Domain unit | Covered against supplied active snapshot only. |
| BR7 | R01, unchanged scoring regression | Source/architecture review + regression | Covered by required review obligation; no fake scoring side-effect test. |
| Immutable creation invariants | R02, P01/P19 | Source review + observable unit behavior | Covered by required contract review and preserved values. |
| No kickoff lock/lifecycle extension | P09/P20, R01 | Domain unit + review | Explicitly excluded behavior remains absent. |

## 6. Detailed cases

Shared setup for each independent case: participant A `11111111-1111-1111-1111-111111111111`, participant B `22222222-2222-2222-2222-222222222222`; Match A ID `aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa`, Match B ID `bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb`. Construct fresh Match objects with teams Argentina/Bolivia, zero-offset kickoff `2026-06-11T19:00:00Z`, and Scheduled unless a case states otherwise. Registration time U is zero-offset `2026-06-10T12:34:56Z` plus 1,234,567 ticks. Default score 2–1; default active context a fresh empty collection. Existing Prediction fixtures must be created through the public factory using a fresh empty context. No database fixtures, shared mutable collections, sleeps or current-clock assertions.

Every P case below is **Ready**, **Domain unit**, with **.NET 10/existing xUnit** as its only execution environment. These fields apply to every table row. Priorities follow the risk assessment. Multiple data rows may share one Theory while retaining the case ID. Planned data counts are not execution results.

| ID / basis | Priority | Given / input data | When / action | Then / observable expectation | Planned data rows |
| --- | --- | --- | --- | --- | --- |
| P01 / AC-01, AC-05, BR6 | P1 | Participant A, Match A, score 2–1, U, empty context | Create prediction | Return participant A, Match A ID, home 2, away 1, U with identical ticks and zero offset. No role swap. | 1 |
| P02 / AC-01/02, BR3 | P2 | Goal pairs (0,0), (0,2), (2,0), (int.MaxValue,0), (0,int.MaxValue), (int.MaxValue,int.MaxValue) | Create each prediction with otherwise valid inputs | Accept and preserve both goal values. No arbitrary upper bound or overflow from score arithmetic. | 6 |
| P03 / AC-03 | P1 | participantId null; otherwise valid | Create | ArgumentNullException with ParamName participantId. | 1 |
| P04 / AC-03 | P1 | participantId Guid.Empty; otherwise valid | Create | ArgumentException with ParamName participantId. | 1 |
| P05 / AC-01/03 | P2 | Participant GUIDs 00000000-0000-0000-0000-000000000001 and ffffffff-ffff-ffff-ffff-ffffffffffff | Create | Accept and preserve each ID; no GUID version/format restriction beyond nonempty. | 2 |
| P06 / AC-04 | P1 | match null; otherwise valid | Create | ArgumentNullException with ParamName match. Existing Match constructor rejection of empty IDs is exercised by unchanged MatchTests, not bypassed by reflection. | 1 |
| P07 / AC-02 | P1 | Home goals -1 and int.MinValue; away 1 | Create | ArgumentOutOfRangeException with ParamName predictedHomeGoals. | 2 |
| P08 / AC-02 | P1 | Away goals -1 and int.MinValue; home 2 | Create | ArgumentOutOfRangeException with ParamName predictedAwayGoals. | 2 |
| P09 / AC-05 | P2 | UTC values DateTimeOffset.MinValue, MinValue+1 tick, leap-day 2000-02-29T23:59:59Z+1 tick, MaxValue-1 tick and MaxValue | Create | Preserve exact ticks and zero offset; no undocumented clock/date restriction. | 5 |
| P10 / AC-05, plan UTC guard | P1 | U expressed at offsets -840, -240, -1, +1, +330 and +840 minutes, preserving its instant | Create | ArgumentException with ParamName registeredAtUtc; do not silently normalize nonzero-offset input. | 6 |
| P11 / BR5, plan context guard | P1 | activePredictions null | Create | ArgumentNullException with ParamName activePredictions; no silent empty fallback. | 1 |
| P12 / BR5, malformed context | P1 | Context containing only null, or an unrelated valid prediction followed by null (use null! only in deliberate invalid fixtures) | Create | ArgumentException with ParamName activePredictions. All other inputs valid; no duplicate competes with this guard. | 2 |
| P13 / BR5 | P0 | Active prediction for participant A/Match A, scores either 2–1 or 0–0. Candidate uses a distinct Match instance with the same Match A ID and score 2–1 | Create candidate | InvalidOperationException; duplicate identified by IDs regardless of scores or Match object identity. | 2 |
| P14 / BR5 | P0 | Context has unrelated predictions first and matching participant A/Match A entry last | Create participant A/Match A | InvalidOperationException; check beyond first entry. | 1 |
| P15 / BR5 | P0 | Existing participant A/Match A, candidate participant A/Match B | Create candidate | Accept; preserve participant A and Match B ID. | 1 |
| P16 / BR5 | P0 | Existing participant A/Match A, candidate participant B/Match A | Create candidate | Accept; preserve participant B and Match A ID. | 1 |
| P17 / user no-registry decision, BR5 | P1 | Caller-owned mutable List with known entries; one successful nonduplicate candidate or one rejected duplicate candidate | Capture entry references/order/count, call Create | Success returns prediction or duplicate throws as applicable; input count/order/entry references unchanged in both paths. | 2 |
| P18 / user no-registry decision | P1 | Same valid inputs and unchanged empty active snapshot | Create twice | Both succeed and return distinct objects with preserved values. No hidden registration is allowed; the caller did not include the first in the second context. | 1 |
| P19 / BR6, immutable values | P2 | First successful prediction, caller's mutable context, then a second call for a different participant/match/time/score after modifying only caller context | Create second | First prediction retains its participant, match, goals and timestamp; second has its own supplied values. | 1 |
| P20 / exclusions, plan no-status restriction | P2 | Match objects in Scheduled, InProgress, Finished and Cancelled states | Create valid prediction with each | Accept under current scope; no kickoff/status lock added. | 4 |

Total **20 planned case groups**, with **43 planned parameterized executions** if implemented as listed. These are design counts only: **0 tests executed in this stage**.

Required review checks (not executable test claims):

- **R01 / BR7 and exclusions, P1:** independent code reviewer inspects the final factory and diff for no scoring invocation, points calculation/state, hidden registry, persistence, deadline locks or out-of-scope layer changes. Keep existing calculator/Match regressions unchanged. Explicitly record review evidence/verdict.
- **R02 / approved class contract, P1:** reviewer verifies private construction and get-only public values, with no public setter/init or unchecked creation path. This is a contract/security-boundary review, not a reflection test invented to mirror source structure.

## 7. TDD implementation order

1. Implement P01 test first. If needed, add only a throwing nonfunctional Prediction public shape so compilation succeeds. First meaningful RED: P01 expects preserved values but Create throws NotImplementedException. Compilation/setup/discovery failures do not count. Implement the minimal property-preserving factory and observe GREEN.
2. Add/observe RED for participant and Match guards (P03/04/06), then goal guards (P07/08), then UTC/context guards (P10/11/12). Keep valid-input cases green. Do not implement guards before their RED evidence.
3. Add/observe RED for duplicate rejection and untouched caller input (P13/14/17), then minimal pair checking. P15/16 may pass immediately because valid creation already exists; record that accurately, do not break production to manufacture RED.
4. Add remaining boundary and statelessness cases P02/05/09/18/19/20. If they pass immediately, assess that they exercise existing intended behavior and record them as verified characterization/regression, not newly achieved RED.
5. Refactor only for clarity and rerun focused tests. Record exact commands/results, source fingerprints, case/test/code traceability and refactor decision in implementation-evidence.md. No forced refactor or arbitrary extra package.
6. Run Release solution build and the unit/integration projects with the existing runner; confirm discovered/executed/passed/failed/skipped counts. Obtain independent review for R01/R02 and all criteria on the stable final source.

## 8. Environment and proposed commands

Confirmed installed SDK: 10.0.401; global.json 10.0.100/latestFeature; all projects net10.0. Existing xUnit 2.9.3, Microsoft.NET.Test.Sdk 17.14.1 and runner adapter 3.1.4; no package changes. SDK default compile inclusion covers new source/test files.

The following are **proposed, NOT EXECUTED** commands from repository root. The implementer must reconcile build outputs and may add --no-build only after a matching successful build.

```powershell
dotnet test tests/ProdeChamp.UnitTests/ProdeChamp.UnitTests.csproj --configuration Release --filter FullyQualifiedName~PredictionTests --logger 'trx;LogFileName=PRODE-003-focused.trx' --results-directory .local/PRODE-003/TestResults/focused
dotnet build ProdeChamp.slnx --configuration Release
dotnet test tests/ProdeChamp.UnitTests/ProdeChamp.UnitTests.csproj --configuration Release --logger 'trx;LogFileName=PRODE-003-unit.trx' --results-directory .local/PRODE-003/TestResults/unit
dotnet test tests/ProdeChamp.IntegrationTests/ProdeChamp.IntegrationTests.csproj --configuration Release --logger 'trx;LogFileName=PRODE-003-integration.trx' --results-directory .local/PRODE-003/TestResults/integration
```

Use uniquely named RED/GREEN evidence files per cycle to avoid overwriting failure evidence. Generated results/logs stay in ignored .local/TestResults paths, never solution items or committed artifacts. Record durable result summaries and source identities in markdown.

## 9. Uncovered risks and assumptions

- No authoritative external participant/match catalog is verified by this contract.
- No durable, distributed or concurrent duplicate prevention is implemented; a stale/incomplete caller snapshot can omit duplicates. This limitation was part of the approved plan, not a hidden waiver.
- No active-state lifecycle is introduced; the caller decides which existing predictions are active before supplying them.
- No time-source correctness is verified: the caller supplies actual registration time. The domain enforces only the approved zero-offset/preservation contract.
- Match team validation and self-play prevention remain deferred under PRODE-002.
- Existing 83 unit cases are historical expected regression scope, not a fresh passing result. Integration currently has no executable cases; no integration coverage is asserted.

## 10. Handoff summary

- Cases: **20 Ready**, **0 blocked by unresolved product decisions** within the approved plan; executable work awaits **QA-design human approval**.
- Files changed in this stage: this design, approval/status annotations in plan/refinement/ledger/report, and solution item entry. No production/test changes.
- Decisions: test coverage derives only from the unchanged Issue, explicit clarifications and approved plan. No new dependencies/contracts/requirements.
- Tests executed: **none — design only**. Issue/read-only repository/documentation checks are separate operational verification.
- Next owner: `tdd-implementation` with `backend-implementation` after approved QA design is recorded. Later architecture/code reviewers must independently cover the final source and required review checks.
