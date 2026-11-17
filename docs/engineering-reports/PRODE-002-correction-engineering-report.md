# Engineering Report — PRODE-002 constructor correction

- Date: 2026-11-17.
- Reference: [Issue #4](https://github.com/DiegoDX/ProdeChampIA/issues/4), existing plan and QA scope; user-selected local correction.
- Outcome: local correction complete and tests green; full Issue completion remains partial because AC3 is deferred.
- Evidence boundary: branch `fix/PRODE-002-match-construction`, base `e39f37450c21e2cf7c02f0dbd11f524a0fd95272`, working-tree `Match.cs` blob `d3288bba5a598e2c355d4e38e1755c016838cc28`.

## 1. Executive summary

Completed the existing `Match` skeleton with property assignments and the documented non-team guards. The same tests that reproduced 31 behavioral failures now pass without modification. Release compilation succeeds and all 83 unit tests pass. Team identity/validation, including self-play rejection, remains deferred by the recorded user decision; a closed GitHub Issue does not establish that criterion's completion.

## 2. Acceptance criteria traceability

| Criterion | Status | Implementation / verification |
| --- | --- | --- |
| AC1: identifier, home team, away team | Verified in scoped increment | Direct assignment, raw value/role preservation and nonempty ID guard; existing QA cases M01/M04/M05/M13/M14. |
| AC2: UTC start | Verified | Exact zero-offset timestamps retained; nonzero offsets rejected; M01/M08/M09/M12. |
| AC3: no self-play | Not implemented — deferred | Existing explicit user direction in QA scope; unchanged Issue criterion remains unmet. |
| Approved P1/P2/P3 | Verified within non-team scope | Four defined statuses accepted, invalid non-team inputs rejected, independent in-memory instances and get-only state; M10–M16. |
| Existing scoring behavior | Verified regression | All 44 PRODE-001 cases pass unchanged. |

## 3. Files changed

| File | Purpose |
| --- | --- |
| `src/ProdeChamp.Domain/Matches/Match.cs` | Replace empty constructor with explicit guards and assignments; remove skeleton-only null-forgiving initializers. |
| `docs/stories/PRODE-002/implementation-evidence.md` | Single source for approval/scope, exact commands, source identities, RED/GREEN observations and actual results. |
| This report | Final result and limitations. |
| `docs/architecture/README.md` | Describe actual Match construction and domain boundaries. |
| `docs/product/README.md` | Record the delivered increment and deferred AC3 without changing the Issue. |
| `docs/testing/README.md` | Document current 39-case focused / 83-case regression results and zero integration execution. |
| `ProdeChamp.slnx` | Expose new evidence and report by repository folder in Solution Explorer. |

The existing `.codex/config.toml` modification is unrelated and preserved. Tests, `MatchStatus`, scoring, project/package references, historical plan/QA artifacts, Skills and workflow are unchanged relative to the correction base.

## 4. Technical decisions and trade-offs

Keep the approved public constructor and get-only properties in dependency-free Domain. Use `ArgumentException` for empty ID and nonzero UTC offset and `ArgumentOutOfRangeException` for undefined status, preserving parameter names. Validate rather than normalize dates; accept all defined statuses and representable UTC dates. Assign provisional team strings unchanged. No interfaces, services, async wrappers, SQL boundaries or packages are needed for this synchronous in-memory operation.

## 5. Verification results

See [implementation evidence](../stories/PRODE-002/implementation-evidence.md) for exact commands, environment, failed assertions, test mappings and TRX paths. Tests were executed before code changes and existing expectations were preserved.

| Check | Actual result |
| --- | --- |
| Initial sandbox run | Blocked by MSB3491 on an `obj` cache write; no test executed. Approved external rerun used for RED. |
| Focused RED | 39 executed: 8 passed, 31 failed, 0 skipped; exit 1 for expected assertions. |
| Incremental GREEN | Property preservation 17/17, ID 1/1, UTC 14/14, then full PRODE-002 39/39; exit 0 each. |
| Release build | 6 projects, 0 warnings, 0 errors; exit 0. |
| Release regression | 83 unit tests passed, 0 failures/skips; exit 0. Integration executed 0 cases and supplies no integration coverage. |
| Structure / visibility | 6 projects and 13 unique existing solution items; 21 local documentation links resolve. MSBuild default compile items include Match/MatchStatus/MatchTests once each. Diff hygiene and scope checks passed; exit 0. |
| Review | Implementer self-review only; independent human review remains pending. |
| Remote publication / CI | Not run for this correction. Previous failed PR #7 checks do not validate or invalidate this separate local working tree. |

## 6. Risks, limitations and next action

AC3 and team validity remain deliberately unenforced; this increment cannot establish full Issue acceptance. Integration and Visual Studio UI were not verified. Local passing tests do not change the checks on the already published Skills PR. The user subsequently authorized commit, push and PR creation with **“si autorizo”**; remote verification follows publication. Independent human review remains pending. Never merge automatically. No Issue update or reopening was performed.

## 7. References

- [Implementation plan — historical design](../stories/PRODE-002/implementation-plan.md)
- [QA report — existing design, scope decisions and historical RED](../stories/PRODE-002/qa-report.md)
- [Current implementation evidence](../stories/PRODE-002/implementation-evidence.md)
