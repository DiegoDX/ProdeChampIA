# PRODE-003 — Implementation plan

- Version: **1**, 2026-11-17.
- Status: **APPROVED by the user** — “esta bien”, in direct response to the plan-v1 approval request, before QA design began. Approved original SHA256: `3C3F2CB952E87C81B7925D101D52CFB03302C4F70126E34BAC9FC08D6741515C`. This status annotation changes no technical content. QA was subsequently approved and implementation verified locally; see [implementation evidence](implementation-evidence.md) for actual execution rather than treating the planning-stage future tense below as current status.
- Issue: [#5 — Registrar predicciones de partidos](https://github.com/DiegoDX/ProdeChampIA/issues/5), OPEN, refreshed at `2026-11-17T22:27:10Z`; `updatedAt` remains `2026-11-12T21:12:59Z`, no comments.
- Source: `feature/PRODE-003`, base/HEAD `273690364bd014a13aff8ecfa394b1dcbed76df8`. Existing uncommitted work is this story's refinement, ledger, report and solution entries; no production/test edits.
- The five unchanged criteria, seven rules and exclusions are faithfully preserved in [workflow-status.md](workflow-status.md). They are not replaced by this technical proposal.

## 1. Business requirements and separately approved decisions

Create a prediction associated with a participant and a valid existing match, preserve predicted nonnegative integer goals and UTC registration date/time, reject invalid participant/match inputs and duplicates, and do not calculate points on creation.

The user's latest answers, recorded separately from the Issue:

1. Participant identifier must be neither empty nor null: “si, debe ser distinto a vacio o nulo”. This approves identifier validation, not a participant-catalog lookup.
2. A valid match must exist: “si no se si deba recibir un match pero tiene que haber un match valido”. The user leaves the technical input shape open; a Match argument is proposed below.
3. “no registres solo rechaza las duplicadas”. The earlier in-memory registry proposal is **rejected**. Do not create a registry, cache, repository or persistence adapter. Duplicates must still be rejected.

No approval of an unseen plan or QA design was inferred from those answers. The user subsequently explicitly approved this concrete plan with “esta bien”; that approval covers the existing Match, caller-snapshot duplicate boundary and supplied UTC-time contract described below. Existing authorization for later local commits, feature-branch push and PR creation is retained. No Issue write or merge will occur.

## 2. Acceptance criteria and proposed coverage

| Criterion / rule | Planned observable contract |
| --- | --- |
| AC-01 / BR1, BR2, BR6 | Given valid inputs and no matching active prediction, return a Prediction retaining participant ID, Match ID, home/away goals and supplied registration timestamp. |
| AC-02 / BR3 | Reject either negative goals argument; allow zero and the full nonnegative int range. |
| AC-03 | Reject nullable participant ID when null or Guid.Empty; retain a nonempty GUID. |
| AC-04 / BR1 | Require an already constructed Match; reject null. Its existing constructor ensures a nonempty Match ID. A raw GUID alone is insufficient for this contract. |
| AC-05 / BR4 | Require a zero-offset DateTimeOffset supplied by the caller and preserve it exactly. The future caller supplies the actual registration instant; no clock reads hidden in the domain method. |
| BR5 | Reject an active prediction with the same participant ID and Match ID in the caller-supplied active-prediction collection, regardless of predicted score. |
| BR7 | Creation has no scoring call or points field. |

## 3. Architecture impact

Domain-only increment. Existing Match and scoring behavior remain unchanged. Keep the existing six projects and dependency directions. No Application, Infrastructure, API or frontend implementation is needed for this contract. Tests belong to the existing UnitTests project, which already references Domain.

Use a single immutable `Prediction` with a static factory. The factory enforces the duplicate rule using explicit caller-provided context rather than hidden mutable state. A private constructor prevents a public construction path from skipping the checks. No interface is justified for this synchronous, in-process rule. No I/O, async wrapper, service registration, dependency-injection abstraction or new package is introduced.

## 4. Proposed class and public contract

File: `src/ProdeChamp.Domain/Predictions/Prediction.cs`, namespace `ProdeChamp.Domain.Predictions`, public sealed class.

Proposed creation shape (documentation only; not implemented):

```csharp
public static Prediction Create(
    Guid? participantId,
    Match? match,
    int predictedHomeGoals,
    int predictedAwayGoals,
    DateTimeOffset registeredAtUtc,
    IReadOnlyCollection<Prediction> activePredictions);
```

Public get-only properties: `Guid ParticipantId`, `Guid MatchId`, `int PredictedHomeGoals`, `int PredictedAwayGoals`, `DateTimeOffset RegisteredAtUtc`. No separate generated prediction ID is required by this Issue. No points or active-state lifecycle property is added.

| Invalid input / condition | Proposed result |
| --- | --- |
| participantId is null | ArgumentNullException, parameter participantId. |
| participantId is Guid.Empty | ArgumentException, parameter participantId. |
| match is null | ArgumentNullException, parameter match. |
| either predicted goals argument is negative | ArgumentOutOfRangeException identifying that argument. |
| registeredAtUtc has a nonzero offset | ArgumentException, parameter registeredAtUtc; zero-offset UTC is consistent with Match's existing contract. |
| activePredictions is null | ArgumentNullException, parameter activePredictions; missing context must not silently disable checking. |
| collection contains a null entry despite its nonnullable element annotation | ArgumentException, parameter activePredictions; malformed context is rejected. |
| same participant/match pair exists in supplied active predictions | InvalidOperationException; return no new prediction and preserve input collection. |

Exception message wording and ordering when several inputs are invalid are not acceptance contracts. Individual invalid arguments must be identifiable. DateTimeOffset precision is preserved; no undocumented future/past-date or match-status restriction is introduced.

## 5. Execution flow and duplicate boundary

1. A caller supplies participant identity, an existing Match, the score, UTC registration instant and a complete snapshot of relevant active predictions.
2. Validate required values, nonnegative goals, UTC offset and usable collection context.
3. Compare existing entries by the participant/match pair. A different participant or a different match is not a duplicate; a different score for the same pair remains a duplicate.
4. Reject a match, otherwise construct and return one immutable Prediction. Do not add to, remove from or retain the input collection.

**Guarantee requiring explicit plan approval:** duplicate rejection applies to the supplied active-prediction snapshot. The caller owns obtaining a complete/current set and any serialization around concurrent operations. This factory cannot guarantee uniqueness across omitted entries, concurrent snapshots, application instances or process restarts. No persistence or in-memory registry is implemented. If the user requires a stronger guarantee, revise the plan before implementation; do not claim global BR5 completion from a local collection check.

Receiving a Match object proves a valid existing domain object was supplied; it does not prove membership in an external database/catalog. Match team validation remains deferred under PRODE-002 and is not added here.

## 6. Implementation approach and files

After scoped plan approval and the later QA-design gate:

1. Have `test-design` produce the acceptance matrix and fixtures at `test-design.md`; obtain the required QA-design approval under the shared factory contract.
2. Use `tdd-implementation` with `backend-implementation`. Write focused tests first. Add only a nonfunctional public shape/private constructor if necessary to compile; observe assertion/exception-based RED, not compiler/setup failure.
3. Implement the smallest behavior for each RED: valid creation, input guards and then duplicate/context handling. Observe GREEN and rerun after any refactor. Record exact counts and source evidence in `implementation-evidence.md`.
4. Preserve existing scoring/Match tests. Add `tests/ProdeChamp.UnitTests/Predictions/PredictionTests.cs` or a similarly focused test split justified by the QA matrix.
5. Update product/architecture/testing docs to describe approved and verified behavior and the exact duplicate boundary. Add new documentation to solution items; rely on SDK default source inclusion without duplicate Compile entries.
6. Run the solution build and unit/integration projects with the existing runner. Obtain independent architecture and code review on a frozen diff, correct in-scope findings, and consolidate the engineering report.
7. Under existing user authorization, commit only inspected story changes, push the feature branch, create a PR against main, verify required CI against its actual head and stop for final human review. Never merge.

Likely production/test changes: the one new Domain class and focused UnitTests files. Supporting artifacts: existing refinement/ledger/report, new QA/implementation/review/CI evidence and synchronized docs/solution entries. Do not modify the CI pipeline unless an evidenced in-scope defect requires it.

## 7. Dependencies and test strategy

No NuGet/npm dependencies. Use installed .NET SDK 10.0.401 under existing net10.0/global.json settings, xUnit 2.9.3, Microsoft.NET.Test.Sdk 17.14.1 and existing adapter 3.1.4. No SQL, HTTP or Angular boundary is introduced; no new integration framework is needed. The integration project currently has no tests; a zero-discovery run must remain an explicit limitation.

QA will own detailed cases and ordering after approval. Required boundaries include null/empty participant ID, null Match, negative goals in either position, zero/int.MaxValue goals, UTC preservation/nonzero-offset rejection, empty active collection, duplicate pair with same/different score, different participant/match, null/malformed context and collection nonmutation. Existing 83 unit cases must remain green. No test is authored or executed as part of this planning stage.

## 8. Risks, assumptions and out of scope

- The caller-snapshot uniqueness boundary and supplied UTC-time contract need scoped plan approval; they are proposed technical contracts, not previously approved business decisions.
- Linear scanning is sufficient for this small pure domain contract; no index/cache or speculative optimization is added.
- No durable/distributed uniqueness, data catalog, autonomous in-memory storage, participant management or lifecycle is promised.
- Respect all original exclusions: Angular, REST, SQL persistence, authentication, automatic scoring, edit/delete and kickoff locks.
- Existing Match validity does not include team/self-play validation; do not extend it under this story.

## 9. Open decisions and approval gate

Approve or revise version 1, particularly the use of an existing Match and caller-supplied active-prediction snapshot with no storage. If global uniqueness or catalog existence is intended instead, the current proposed boundary is insufficient and requires a revised approved design.

`architect/SKILL.md` states: “The plan must receive human approval before QA begins.” This plan is the reviewable deliverable for that gate. QA design remains a later gate. No approval is inferred from artifact creation or the general implementation request.

## 10. Definition of Done

- Required scope/plan/QA approvals are recorded and remain applicable.
- Every unchanged criterion and BR5/BR7 has traceable executed coverage within the explicitly approved boundary.
- Behavioral RED/GREEN/refactor evidence, Release build and executed unit regression results are recorded; integration discovery is reported truthfully.
- Independent architecture/code reviews cover the final source and have no unresolved blockers.
- Product/architecture/testing docs and solution visibility match the implementation.
- Inspected story changes are committed/pushed; a PR targets main and its required CI passes for the exact current head, ready for final human review.
- Limitations about supplied context and lack of persistence are visible in report/PR; no merge, Issue closure, deployment or unverified completion claim.
