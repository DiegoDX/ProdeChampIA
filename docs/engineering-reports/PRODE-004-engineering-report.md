# Engineering Report - PRODE-004

- Date: 2026-11-19.

> Current correction: the user replaced the static cutoff evaluator with Prediction.CanEdit using internal server time and changed Create to Guid matchId. See the [correction report](prediction-contracts-engineering-report.md), [evidence](prediction-contracts-implementation-evidence.md) and independent review links there. Fresh local build passed0 warnings/errors and146 unit cases passed; correction publication/CI is tracked in [workflow status](../stories/PRODE-004/workflow-status.md). The original implementation results below are historical and do not verify the new source.
- Scope: [Issue #10](https://github.com/DiegoDX/ProdeChampIA/issues/10), evaluation-only scheduled edit cutoff; unchanged business requirements.
- Outcome: Local engineering complete; [PR #13](https://github.com/DiegoDX/ProdeChampIA/pull/13) published, implementation push/PR CI passed; final exact-head verification is recorded in PR body/Checks before delivery.
- Source boundary: feature/PRODE-004 from `950ec8963012f2367ae2a95a1891f8b10ba84458`; frozen source/test hashes in implementation evidence and independent reviews.

## 1. Executive summary

PredictionEditCutoff.IsAllowed now permits validation strictly before scheduled kickoff and blocks equality or later. It uses the supplied server validation instant without accessing or modifying predictions, and introduces no new dependency or outward-layer behavior. Three observed RED/GREEN cycles, the Release build and 147 unit cases passed. Independent architecture/code reviews are APPROVED with no findings. Implementation commit33caab5 is published in PR #13 with successful push CI and raw remote unit147/integration0 evidence; final metadata-head checks must be verified separately in the PR body/Checks before delivery.

## 2. Acceptance criteria traceability

| Criterion | Status | Implementation / verification evidence |
| --- | --- | --- |
| AC-01 | Verified locally | C01/C04/C05: strictly before kickoff returns true, including one tick before. |
| AC-02 | Verified locally | C01/C04/C05: exact equality returns false. |
| AC-03 | Verified locally | C01/C04/C06 and R01: later validation blocks, with no opening-time dependency. |
| AC-04 | Verified within evaluation scope | C01/C06 fixed validation instants and both independent R01 reviews: no clock/save-time recheck or retained state; preserve decision for this attempt. No saving integration claim. |
| AC-05 | Verified by independent source review | Both R02 reviews: no Prediction access/reference, mutable state, creation/edit/save/scoring path. Existing Match/Prediction unchanged. |

Exact unchanged criteria/business rules are in [workflow status](../stories/PRODE-004/workflow-status.md#issue-baseline). Full criterion-to-case-to-code mapping is in [implementation evidence](../stories/PRODE-004/implementation-evidence.md).

## 3. Files changed

| File(s) | Change / purpose |
| --- | --- |
| src/ProdeChamp.Domain/Predictions/PredictionEditCutoff.cs | New pure rule, strict comparison, UTC/null guards and caller-boundary remarks. |
| tests/ProdeChamp.UnitTests/Predictions/PredictionEditCutoffTests.cs | 21 new cases: time precision, null/offset guards, date extremes/year boundary, statuses and independent attempts. |
| docs/product/README.md, docs/architecture/README.md, docs/testing/README.md | Synchronize implemented scope, boundaries and actual evidence. |
| docs/stories/PRODE-004/workflow-status.md, refinement.md, implementation-plan.md, test-design.md | Baseline, progression authorization, scope, plan and QA design. |
| docs/stories/PRODE-004/implementation-evidence.md | Exact TDD/build/regression commands, observed failures/results, hashes and traceability. |
| docs/stories/PRODE-004/architecture-review.md, code-review.md | Independent source-bound APPROVED verdicts and review evidence. |
| docs/stories/PRODE-004/ci-report.md | Publication/CI evidence and required-check configuration. |
| docs/engineering-reports/PRODE-004-engineering-report.md | Consolidated factual handoff. |
| ProdeChamp.slnx | Link new documentation under repository folders, retaining all six projects. |

Pre-existing architect/tdd-implementation skill edits are preserved and excluded from story publication. Generated local snapshots/TRX/logs and the PR body stay ignored under .local/PRODE-004. SDK default items include new source/tests without duplicate Compile entries.

## 4. Technical decisions and trade-offs

The explicit Match and zero-offset validation-time inputs keep Domain deterministic and reuse existing UTC conventions. Named ArgumentNullException/ArgumentException guard technical invalid inputs; the actual cutoff returns a bool. No clock, interface, DI registration, async wrapper, package, project reference, status policy or persistence is needed. Correct associated-match selection and trustworthy server time remain caller prerequisites; a true result passes only this cutoff.

The user's full-workflow instruction followed the concrete plan handoff and authorized progression through QA/TDD/reviews/publication without further intermediate pauses for unchanged scope. No separate signed QA approval or changed business requirements are claimed.

## 5. Verification results

| Check | Status / actual result |
| --- | --- |
| Issue retrieval and publication refresh | Passed; full body/updatedAt unchanged, no comments; main matches base. |
| Three focused RED/GREEN cycles | Observed behavioral failures then 5/6/10 passes; final expanded focused run 21 passed. |
| Release solution build --no-restore | Exit 0; six projects, 0 warnings/errors. |
| Unit regression --no-build --no-restore | Exit 0; 147 executed/passed, failed/skipped0; 21 new and all 126 existing cases. |
| Integration project | Exit 0; 0 discovered/executed. No integration coverage. |
| Independent architecture review | APPROVED, no findings; reviewer /root/prode004_arch_review verified hashes/raw TRX and reran 21 focused passes. |
| Independent code review | APPROVED, no findings; reviewer /root/prode004_code_review verified hashes/raw TRX and reran 21 focused passes. |
| Documentation/solution/source checks | Final script .local/PRODE-004/verify-story.ps1 checks all links/projects, exact Issue body, current source/test hashes, whitespace, unchanged old source/tests and document credential patterns; actual final result belongs in workflow status. |
| Publication / GitHub CI | User-terminal commit/push succeeded as33caab5, agent created PR #13. Push44549746982 and PR44549866439 SUCCESS; downloaded raw TRX unit147 passed/failed-skipped0, integration0, including21 new cases. PR merge d6d9b5b tested, build0 warnings/errors. Final metadata-head check outcomes belong to PR body/Checks; no older-run substitution. |

Exact commands and raw evidence locations are in implementation evidence and review reports. No unsuccessful implementation correction rounds (0/3). Passing unit tests are not evidence of excluded API/SQL/edit/save behavior.

## 6. Risks, limitations and next action

The reviewed15 story files were committed/pushed by the user using the prepared helper after agent Git staging was blocked by read-only .git; the agent created PR #13. A documentation-only checkpoint may be published through the authenticated GitHub Git Data API on the same feature branch, without changing local .git. Local Git metadata stays at the implementation commit until user synchronization; final document contents and reviewed code can still match the published tree. Final exact-head check results are persisted in PR body/Checks to avoid a self-referential documentation hash. main is unprotected and its sole ruleset disabled, so no enforced required checks exist; nevertheless all actual CI checks must succeed before handoff. Final review belongs to the user; no merge, auto-merge, Issue update or deployment was performed.

Caller time provenance, match lookup, future saving concurrency and rescheduling remain outside this increment. Integration has zero cases. These limits are explicit in the plan, API remarks and independent reviews. Any production/test source change invalidates the frozen-source evidence/reviews until reassessed.

## 7. References

- [Workflow / Issue baseline](../stories/PRODE-004/workflow-status.md).
- [Plan](../stories/PRODE-004/implementation-plan.md) and [QA design](../stories/PRODE-004/test-design.md).
- [Implementation evidence](../stories/PRODE-004/implementation-evidence.md).
- [Architecture review](../stories/PRODE-004/architecture-review.md) and [code review](../stories/PRODE-004/code-review.md).
- [CI report](../stories/PRODE-004/ci-report.md).
