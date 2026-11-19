# PRODE-004 - Implementation plan v1

Status: **Authorized for implementation by the user's subsequent full-workflow instruction**. Prepared 2026-11-19 against `950ec8963012f2367ae2a95a1891f8b10ba84458` on feature/PRODE-004. The instruction followed presentation of concrete plan v1 and explicitly requested implementation, tests/reviews, commit, push and PR creation through passing CI, without merging. It authorizes progression through QA/TDD for unchanged scope; no separate interim QA signoff is claimed.

## Baseline and requirements

[Issue #10](https://github.com/DiegoDX/ProdeChampIA/issues/10), updated `2026-11-18T23:50:54Z`; its unchanged full body and retrieval evidence are in [workflow status](workflow-status.md#issue-baseline). [Refinement](refinement.md) found no blocking business decisions. The user's later full-workflow instruction supplies implementation/progression/publication authorization. No business additions or requirement changes were approved or introduced.

All eight business rules are preserved: use scheduled kickoff and server time at validation; allow strictly before and block equality/after; preserve a pre-kickoff decision through later processing; opening a form or delayed actual kickoff gives no extension; evaluation does not mutate predictions.

Acceptance criteria remain verbatim in the baseline. Planned mapping: AC-01 allows the pre-kickoff instant; AC-02 blocks equality; AC-03 blocks later validation; AC-04 uses only the captured validation instant; AC-05 preserves prediction state. No requirement to implement editing/saving is inferred from AC-04.

## Architecture impact

Keep the rule in ProdeChamp.Domain, independent of I/O, system clocks and outward layers. Reuse Match.StartTimeUtc: its constructor already enforces a zero UTC offset. Prediction remains immutable and unchanged, including its creation contract. Application, Infrastructure and API need no implementation for an evaluation-only rule with server time already available as an input.

No new interface, DI registration, service abstraction, project reference, package, ADR or dependency is proposed. A static rule follows the existing stateless calculator convention. Async is unnecessary because there is no I/O.

## Proposed observable contract

Add `src/ProdeChamp.Domain/Predictions/PredictionEditCutoff.cs` with a public static class in ProdeChamp.Domain.Predictions:

```csharp
public static bool IsAllowed(Match match, DateTimeOffset validatedAtUtc);
```

This declaration describes the proposal; it has not been added to production code.

- `match`: the associated existing match selected by the caller. Null throws ArgumentNullException with ParamName `match`.
- `validatedAtUtc`: server time captured when validating this attempt. Require Offset == TimeSpan.Zero, consistent with existing Match/Prediction UTC contracts; otherwise throw ArgumentException with ParamName `validatedAtUtc`. This is a proposed technical input guard, not a changed business criterion.
- Result: true exactly when validatedAtUtc is strictly less than match.StartTimeUtc; false for equality or any later instant. A cutoff rejection is a false result, not an exception.
- Preserve DateTimeOffset precision; no rounding, date-only comparison, time-zone conversion, subtraction or assumed tolerances.
- Do not inspect Match.Status or registration time. No Prediction parameter or lookup is needed for a rule that only compares time; caller selection of the associated match is an explicit precondition.
- Do not read DateTimeOffset.UtcNow, retain objects/state, invoke scoring, create/edit predictions or save anything. The XML remarks must explain the trusted-server-time input and that true passes only this cutoff rule.

## Intended flow

1. A future server caller obtains the existing prediction's match and captures the server validation instant. Those caller operations are prerequisites, not implementations in this story.
2. Call the evaluator once for the attempt, after its argument guards.
3. Use the returned cutoff decision. For an attempt validated before kickoff, subsequent processing crossing kickoff does not trigger another cutoff validation for that same decision. A new attempt has its own validation instant.

This does not prove identity, authorization, catalog membership or saving behavior. No client opening timestamp, actual kickoff timestamp or saving-completion timestamp is accepted by the evaluator.

## Implementation approach and affected files

After plan approval, test-design prepares a detailed matrix under docs/stories/PRODE-004/test-design.md. After that design is approved, tdd-implementation with backend-implementation writes executable tests first, observes behavioral RED, implements the smallest passing behavior and records GREEN/refactoring evidence. Compilation/setup errors are not RED; a minimal nonfunctional public shape may be recorded separately if compilation requires it.

Planned production/test files: PredictionEditCutoff.cs above and `tests/ProdeChamp.UnitTests/Predictions/PredictionEditCutoffTests.cs`. SDK default compile inclusion makes both visible through their owning projects; do not add duplicate Compile entries. Existing Match, Prediction, scoring and their tests remain the regression baseline.

Update docs/product/README.md, docs/architecture/README.md and docs/testing/README.md only when implemented behavior/evidence exists. Persist implementation evidence and independent reviews under this story folder, update the engineering report and link all new relevant documents in ProdeChamp.slnx. Generated local evidence stays under ignored .local/PRODE-004 and outside solution items. Preserve the two pre-existing modified skill files and exclude them from any story publication.

## Test strategy and boundaries

Use existing .NET 10/xUnit 2.9.3 tooling, not a new framework. QA owns the detailed matrix and final case count after approval. Priorities are the exact strict boundary (one tick before/equal/one tick after), all five criteria, UTC input guards, representative date boundaries and independence from MatchStatus. Use fixed UTC instants and no sleeps.

AC-04 must be assessed within evaluation scope: behavioral evidence of using the supplied validation instant plus review that no clock/save-time recheck exists. AC-05 requires review of the pure rule's absence of prediction access or mutation; do not claim persistence/edit integration coverage. The later QA design must distinguish tests from source-review obligations and avoid an assertion that merely proves an untouched fixture equals itself.

After implementation: focused story tests, Release solution build, full unit regression and integration project discovery/execution, all with actual counts. The integration project currently has no cases; zero discovery cannot establish integration coverage. Preserve exact commands and RED/GREEN failures/results in implementation-evidence.md. Independent architecture/code reviewers must inspect the frozen current source, Issue, approved plan/design and raw test evidence.

## Risks, assumptions and scope exclusions

Trusted time provenance and matching prediction-to-match selection belong to the caller. Passing a historical or client-supplied timestamp incorrectly could produce an incorrect eligibility decision; document this precondition without inventing API enforcement. The evaluator cannot guarantee eventual storage consistency or scheduling/rescheduling policy.

Explicit exclusions: implementing edit/save or new prediction creation, UI/messages, endpoints, authentication/authorization, score validation/calculation, rescheduling and status-based restrictions. Existing deferred PRODE-002 team validation remains unchanged. No new dependency is required.

## Open questions and approval

No unresolved business questions. The user instructed implementation of the complete workflow after this concrete v1 contract and caller boundary were presented, including UTC/null guards. This explicit progression instruction supersedes separate intermediate approval pauses for unchanged scope. No approval is inferred from file existence or from the unrelated skill edits. Business changes still require approval.

## Definition of Done

Approved plan and QA design; observed behavioral RED/GREEN for new behavior; all five criteria traced to meaningful test/review evidence; relevant build/regression checks completed with actual counts; independent reviews clear the frozen source; synchronized product/architecture/testing docs, solution links and engineering report. Publication and exact-head CI require separate authorization; no merge is allowed. This planning handoff alone does not meet story completion.

## Planning handoff

Original planning handoff created this plan, refinement, coordinator record, partial report and solution links before implementation. Subsequent authorization and actual QA/TDD/build evidence are recorded in workflow-status.md, test-design.md and implementation-evidence.md; original planning-stage nonexecution is not current story status.
