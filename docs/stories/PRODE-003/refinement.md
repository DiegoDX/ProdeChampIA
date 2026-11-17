# PRODE-003 — Refinement

- Date: 2026-11-17.
- Source: [Issue #5](https://github.com/DiegoDX/ProdeChampIA/issues/5), updated `2026-11-12T21:12:59Z`, freshly retrieved with body/state/labels/comments/URL/update timestamp.
- Source revision: `273690364bd014a13aff8ecfa394b1dcbed76df8`, on `feature/PRODE-003`.
- Readiness: **Refinement, plan v1 and QA v1 approved; implementation now verified locally**. User approved plan with “esta bien” and QA with “si esta bien”. In-memory registry remains rejected; original criteria unchanged. Findings below describe the refinement-stage decisions/history; current engineering evidence is linked from [workflow-status.md](workflow-status.md).

## 1. Story baseline

As a ProdeChamp participant, register a predicted football score to compete and later earn points. Preserve participant/match identifiers and UTC registration timestamp, reject negative goals and invalid identifiers, enforce one active prediction per participant/match, and do not calculate points on registration.

The faithful AC-01–AC-05 baseline, all seven business rules and exact exclusions are retained in [workflow-status.md](workflow-status.md). GitHub remains authoritative. There are no Issue comments supplying additional decisions.

Consulted: `AGENTS.md`, shared factory contract, `docs/product/README.md`, `docs/architecture/README.md`, architecture decisions index, `docs/testing/README.md`, current Match/MatchStatus, scoring calculator, project/package references, solution and CI workflow. No nested AGENTS instructions were found.

## 2. Assessment

| Finding | Severity | Evidence and impact |
| --- | --- | --- |
| Nonnegative integer goals, preserved identifiers, UTC timestamp and no immediate scoring are clear | Clear | BR2/3/4/6/7, AC-01/02/05. No new scoring or deadline behavior is needed. |
| Participant validity clarified | Resolved | User requires an identifier that is neither null nor empty; no participant catalog approved. |
| Valid match required; technical input left open | Design decision for plan | User confirms a valid match must exist without prescribing a Match parameter. Plan v1 proposes an existing non-null Match object. |
| In-memory registry rejected; duplicates must still be rejected | Plan approval needed | User says to reject duplicates without registering them. Plan v1 supplies existing active predictions as explicit caller context and discloses its boundary. |
| Match dependency has deferred team validation | Important, nonblocking for prediction registration | Product/architecture docs explicitly defer PRODE-002 team validation/self-play. This story must not silently complete that deferred work or assume teams are validated. |

## 3. Open questions

Original questions and actual user answers are retained here; approval of the plan is still separate.

- **Q1 / AC-03, resolved:** User: “si, debe ser distinto a vacio o nulo”. Reject null and Guid.Empty; no lookup is approved by this answer.
- **Q2 / AC-04, BR1, business objective resolved:** User: “si no se si deba recibir un match pero tiene que haber un match valido”. Require a valid match; choose the concrete input in the plan without assuming catalog access exists.
- **Q3 / BR5, registry rejected:** User: “no registres solo rechaza las duplicadas”. Do not implement the proposed in-memory registry. Retain duplicate rejection. The mechanism and caller-snapshot boundary are proposed in plan v1 for explicit review.

## 4. Proposed refinement — NOT APPROVED

Keep the original story and criteria unchanged. Updated handoff:

1. **Approved clarification:** participant identifier must be nonnull and nonempty.
2. **Plan proposal:** receive an existing Match object and reject null; preserve its validated ID. No database/catalog existence guarantee is implied.
3. **Rejected:** in-memory registry. **Replacement plan proposal:** pure creation validates against a complete caller-supplied active-prediction snapshot, rejects a duplicate pair and neither stores nor changes that collection. Uniqueness is conditional on complete/current caller context; global/concurrent uniqueness is not implied.

The user subsequently approved the replacement technical contract in [plan v1](implementation-plan.md), including the existing Match and caller-snapshot duplicate boundary. Original proposal labels above describe their status before that explicit approval; no Issue update was made. QA design remains a separate gate.

## 5. Verification outline

| Source | Illustrative scenario | Documented expected result / remaining decision |
| --- | --- | --- |
| AC-01, BR6 | Valid participant/match and score 2–1 | Preserve associations, 2–1 and registration timestamp; concrete contract proposed in plan v1. |
| AC-02, BR3 | Negative home or away goals | Reject; zero is permitted by greater-than-or-equal-to-zero rule. No lower maximum than int.MaxValue is documented. |
| AC-03 | Null or empty participant identifier | Reject per the user's explicit clarification. |
| AC-04, BR1 | Invalid/missing match | Reject; existing non-null Match is proposed as concrete input. |
| AC-05, BR4 | Successful registration | Keep UTC date/time; injected/supplied time contract belongs in the later plan. |
| BR5 | Same participant/match pair among supplied active predictions | Reject duplicate under proposed caller-context boundary; no registry approved. |
| BR7 | Register a valid prediction | No automatic point calculation. |

These are illustrative acceptance scenarios only; detailed case design, priorities and fixtures belong to `test-design` after the plan gate. None was automated or executed in refinement.

## 6. Dependencies and scope

Existing Domain contains Match/MatchStatus and scoring calculator, with no database dependencies. .NET SDK 10.0.401 is installed; existing xUnit packages and test runner can support Domain tests without additions. Backend specialization is relevant. Angular, REST, SQL persistence, authentication, point calculation, editing/deletion and kickoff locks are explicitly excluded. No NuGet/npm package or architecture decision is approved by this refinement.

## 7. Readiness

**Ready for approved-plan QA:** user explicitly approved plan v1 in response to its approval question. The coordinator retains commit/push/PR authorization. The now-prepared QA design still needs its own required approval before executable tests/implementation.

## 8. Engineering handoff

- Files: refinement, implementation plan, QA design, workflow ledger, partial report and their solution entries only; no source/tests changed.
- Technical decisions: approved plan covers participant null/empty rejection, existing Match, pure factory, supplied UTC instant and duplicate caller context without a registry.
- Tests executed: **Not run — refinement only**. SDK-version and repository/Issue checks are operational evidence, not behavior verification.
- Limitations: QA design v1 awaits approval; executable tests, TDD, independent reviews and PR/CI remain pending. Environment supports scoped reviewed escalations for previously denied repository writes and network reads.
