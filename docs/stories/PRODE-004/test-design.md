# PRODE-004 - Test design v1

## 1. Story reference and test basis

[Issue #10](https://github.com/DiegoDX/ProdeChampIA/issues/10), unchanged updatedAt `2026-11-18T23:50:54Z`; baseline in workflow-status.md. Plan v1 was presented before the user's instruction to implement the complete workflow, run build/tests/reviews, commit, push and create a PR, stopping for final review without merging. That instruction authorizes progression through this design and implementation within the presented contract; no separate interim signoff is claimed. Business changes still require approval.

Target: `PredictionEditCutoff.IsAllowed(Match match, DateTimeOffset validatedAtUtc)` as in [plan](implementation-plan.md). Domain unit tests use existing xUnit 2.9.3, VSTest, .NET 10 and Match fixtures. No new package or production dependency.

## 2. Scope / out of scope

Test the strict UTC cutoff, technical argument guards and stateless evaluation. Review absence of mutation/clock rechecks. No edit/save, API, SQL, UI, authorization, rescheduling or status-policy implementation/tests.

## 3. Ambiguities and blockers

None within the approved evaluation-only scope. Correct associated-match selection and trustworthy server-time capture are caller preconditions. AC-04 cannot establish future saving behavior because saving is explicitly excluded.

## 4. Risk assessment

Equality/precision errors can unfairly permit late attempts (P0). Using wall-clock time instead of the validation input can invalidate an already permitted attempt (P0). Invalid time offsets and null inputs risk inconsistent contracts (P1). Status filtering would add unapproved policy (P1). Date boundaries and retained state are P1 regression risks.

## 5. Acceptance coverage

| Criterion | Cases | Level | Intended coverage |
| --- | --- | --- | --- |
| AC-01 | C01, C04, C05 | Domain unit | Before kickoff passes including one tick before. |
| AC-02 | C01, C04, C05 | Domain unit | Equality blocks, without rounding. |
| AC-03 | C01, C04, C06; R01 | Domain unit / source review | Later validation blocks; opening time is not an input. |
| AC-04 | C01, C06; R01 | Domain unit / source review | Fixed historical validation input controls the decision; no current-clock/save-time recheck. No persistence integration claim. |
| AC-05 | R02 | Source review | No access/reference to Prediction, storage or mutation; no tautological untouched-fixture test. |

## 6. Detailed cases

All C cases are Ready, Domain unit, suggested location `tests/ProdeChamp.UnitTests/Predictions/PredictionEditCutoffTests.cs`; no external infrastructure. R cases are Ready review obligations for independent reviewers.

| ID / priority | Given / test data | When | Expected outcome | Rows |
| --- | --- | --- | --- | --- |
| C01 / P0 | Scheduled kickoff 2026-06-10 15:00:00 UTC; validation offsets in ticks: -10,000,000, -1, 0, +1, +10,000,000 | Evaluate cutoff | true for negative offsets, false for zero/positive | 5 |
| C02 / P1 | Null match; valid zero-offset validation instant | Evaluate cutoff | ArgumentNullException, ParamName match | 1 |
| C03 / P1 | Valid Match; validation offset -840, -240, +1, +840 minutes representing the same instant as kickoff minus one second | Evaluate cutoff | ArgumentException, ParamName validatedAtUtc | 4 |
| C04 / P1 | Kickoff/validation pairs: Min/Min; Max/Max; Max/(Max-1 tick); (Min+1 tick)/Min; Jan 1 midnight/(Dec 31 last tick); Dec 31 last tick/Jan 1 midnight | Evaluate cutoff | false, false, true, true, true, false respectively | 6 |
| C05 / P1 | Each defined MatchStatus with same kickoff | Evaluate at one tick before and exactly at kickoff | true then false for every status; status adds no policy | 4 |
| C06 / P0 | Same Match and fixed historical instants | Evaluate before, after, then the original before instant | true, false, true; original returned bool remains true | 1 |
| R01 / P0 | Frozen final rule and input contract | Inspect implementation and call surface | Only supplied validation time and scheduled kickoff control the comparison; no UtcNow/clock/save/form-opening/actual-kickoff input or recheck. Caller boundary documented. | Review |
| R02 / P0 | Frozen final rule plus unchanged Prediction/Match baseline | Inspect mutation/dependency paths | Rule neither receives/accesses/retains Prediction nor invokes creation/edit/scoring/save; Match has get-only properties; no shared mutable state. | Review |

Total planned automation: **21 cases**. Counts are design expectations, not execution results.

## 7. TDD order

1. Write C01 against a separately recorded nonfunctional throwing public skeleton; expect 5 behavioral failures (NotImplementedException), not a compiler failure. Implement strict comparison and verify GREEN.
2. Add C02; expect wrong exception (NullReferenceException), add null guard and verify GREEN.
3. Add C03; expect missing ArgumentException, add zero-offset guard and verify GREEN.
4. Add C04-C06. They may already pass; record that accurately rather than manufacturing RED. No speculative refactor required.
5. Build Release, run focused final/unit regression/integration discovery, then freeze source for independent architecture/code review including R01/R02.

## 8. Environment / data

Use fixed GUID, teams, UTC DateTimeOffset values, explicit MatchStatus values and no sleeps, network, clock mocks or credentials. Exact ticks test precision. Store TRX/log/snapshot evidence under ignored `.local/PRODE-004/`; expose source/tests through SDK default inclusion and documents through solution items.

## 9. Uncovered risks and assumptions

Server-time provenance, associated-match lookup and future edit/save concurrency are outside this isolated rule. Integration project currently has no cases; report zero discovery separately. Passing this cutoff is not full edit permission. No external catalog, score or deferred team-validation coverage is claimed.

## 10. Handoff

21 automated rows and 2 independent review obligations Ready; none Blocked. Tests executed during design: none. The user's explicit full-workflow progression instruction authorizes the TDD handoff within this scope; no business requirements changed.
