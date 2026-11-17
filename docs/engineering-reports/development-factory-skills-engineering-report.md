# Engineering Report — Development Factory Skills

- Date: 2026-11-17
- Scope: user-approved repository maintenance and publication from `chore/development-factory-skills` to a PR targeting `main`; no product Issue is closed.
- Outcome: PR #7 updated with the human-merged PRODE-002 correction; current local structural checks, Release build and all 83 unit tests passed. New remote checks follow publication and are recorded on the PR.
- Evidence boundary: original base `e39f37450c21e2cf7c02f0dbd11f524a0fd95272`; current main integration base `0688b594a27716c64be27c296fa08480a352c62e`. Local `.codex/config.toml` changes are excluded from publication.

## 1. Executive summary

The changes give 11 existing Skills a shared source, handoff and evidence contract while keeping specialist responsibilities. Automatic interview preparation is removed, and historical interview documents are preserved. This is instructions/documentation maintenance: story refinement, product architecture approval, QA design and behavioral TDD are not applicable. Publication was already authorized in the preceding session; the current session resumes it without granting merge authority.

## 2. Scope traceability

| Maintenance outcome | Status | Evidence |
| --- | --- | --- |
| Reuse specialist Skills with explicit ownership | Verified by inspection | Shared contract and 11 updated Skills; TDD owns execution and QA owns design. |
| Align Issue baselines, approvals, artifacts and reviews | Verified by inspection | Shared contract, coordinator, review Skills and advisory JSON schema. Schema validity alone cannot establish semantic correctness. |
| Remove automatic interview deliverables | Verified | Skill deleted; requirements removed from active Skills and PR template; historical documents have no diff. |
| Retain Visual Studio project visibility | Verified structurally | Solution XML retains all 6 projects and references existing unique solution items. UI inspection not performed. |
| Commit, push, create PR and verify Actions | Published; updating after main synchronization | [PR #7](https://github.com/DiegoDX/ProdeChampIA/pull/7); initial failed runs are historical below. Current local regression passes; renewed remote evidence belongs to the updated PR head. Human review remains required. |

## 3. Files changed

| File or group | Purpose |
| --- | --- |
| `.agents/skills/{architect,architecture-review,backend-implementation,ci,code-review,engineering-report,frontend-implementation,story-refinement,story-workflow,tdd-implementation,test-design}/SKILL.md` | Shared contract links, consistent evidence/authority and less duplicated stage responsibility. |
| `.agents/skills/interview-preparation/SKILL.md` | Deleted following the approved removal of interview preparation. |
| `AGENTS.md`, `.github/pull_request_template.md` | Remove required interview output and preserve historical documents. |
| `docs/development-factory/README.md` | Shared handoffs, ownership, evidence, approval and resumption contract. |
| `docs/development-factory/review-result.schema.json` | Advisory review structure and basic verdict/severity/independence constraints. |
| `docs/README.md`, `ProdeChamp.slnx` | Documentation index and visibility of Skills, factory artifacts, PR template and this report. |
| This report | Current verification and publication handoff. |

## 4. Technical decisions and trade-offs

GitHub Issues remain the approved story baseline, with separately approved decisions recorded distinctly. Product and architecture documentation stays under `docs/`. Backend/frontend specialize the same TDD execution method; QA remains design-only. Review verdicts and severity names are shared, while source, independence and evidence still require semantic checks. No external Skills, packages, unattended runner or dependencies were installed. No production code, tests or CI workflow changed.

## 5. Initial publication verification results

Commands ran from `C:\ProdeChampIA\ProdeChampIA`. Git commands used the invocation-scoped option `-c safe.directory=C:/ProdeChampIA/ProdeChampIA` because sandbox ownership differs; no global Git configuration was changed.

| Check / command | Status | Actual result |
| --- | --- | --- |
| PowerShell frontmatter/name checks over `Get-ChildItem .agents/skills -Filter SKILL.md -Recurse` | Passed, exit 0 | 11 active Skills; names match directories. Interview Skill directory absent. |
| PowerShell Markdown-link extraction with `[regex]::Matches` and `Test-Path` relative to each Skill | Passed, exit 0 | All 28 local links in active Skills resolve. This count excludes links in other documentation. |
| `[xml]` parse of `ProdeChamp.slnx`, `Test-Path` for targets, uniqueness check and comparison with `git show HEAD:ProdeChamp.slnx` | Passed, exit 0 | All 6 project paths retained. 25 unique existing solution items including this report; verified before staging. |
| `Get-Content -Raw docs/development-factory/review-result.schema.json \| ConvertFrom-Json` | Passed, exit 0 | JSON syntax parses. Full Draft 2020-12 schema validation was not run; no validator was installed. |
| `git diff --name-only HEAD -- docs/interview-questions src tests .github/workflows` | Passed, exit 0 | Empty output: historical interviews, product code, tests and workflow unchanged. |
| `git diff --check` | Passed, exit 0 | No whitespace errors in the inspected diff; repeated on staged scope before commit. |
| `gh auth status` outside sandbox | Passed, exit 0 | Existing Windows GitHub authentication works. Sandbox authentication check failed; no credentials were changed. |
| `gh pr list --repo DiegoDX/ProdeChampIA --head chore/development-factory-skills --state all --json number,title,url,state,headRefOid,baseRefName` | Passed, exit 0 | No existing PR for this branch at pre-publication lookup. |
| `gh api repos/DiegoDX/ProdeChampIA/branches/main --jq .commit.sha` | Passed, exit 0 | Remote main equals the recorded base. |
| Local .NET build and behavioral tests | Not run | Documentation/instructions maintenance. Existing CI will build and test the published source; historical test counts are not current evidence. |
| Independent review / Visual Studio UI inspection | Not run | Self-review of maintenance only; human review remains pending on the PR. |

## 6. Risks, limitations and next action

Structural checks do not prove every future agent follows these instructions, validate a full story lifecycle or replace human review. Full JSON Schema validation and Visual Studio UI inspection remain unperformed. The integration project currently has no test classes, so successful runner completion must not be reported as integration coverage.

The changed local `.codex/config.toml` remains uncommitted. PRODE-002 was corrected separately and merged by the human in PR #8; current synchronization results are below. Human review of the Skills changes and new remote verification remain the final delivery steps. No product-code correction was made as part of the Skills maintenance and no test was weakened. Unsuccessful correction rounds: 0; the original CI failures were diagnosed rather than repeatedly retried.

## Historical publication and failed CI evidence

Published initial maintenance commit `784abd88a0e6704b94b31c5852ae97560dbe2feb` to `chore/development-factory-skills` and created [PR #7](https://github.com/DiegoDX/ProdeChampIA/pull/7) against `main`. No merge was performed. This report update is a subsequent documentation-only commit; the initial run results below apply specifically to `784abd8`. Final-head CI status is recorded in the PR publication handoff rather than treating an earlier run as validation of a later commit.

| Run | Event / tested source | Observed result |
| --- | --- | --- |
| [44170201210](https://github.com/DiegoDX/ProdeChampIA/actions/runs/44170201210) | Push, head `784abd88a0e6704b94b31c5852ae97560dbe2feb` | Failed: 83 unit tests, 52 passed, 31 failed, 0 skipped; test command exit 1. |
| [44170253312](https://github.com/DiegoDX/ProdeChampIA/actions/runs/44170253312) | PR, same head; checkout logged merge `01ebe8c` into base `e39f37450c21e2cf7c02f0dbd11f524a0fd95272` | Restore/build passed, build had 0 warnings/errors. Same unit totals and exit 1; integration and unit-execution guard skipped after failure. TRX upload succeeded. |

Executed remote test command: `dotnet test tests/ProdeChamp.UnitTests/ProdeChamp.UnitTests.csproj --no-build --no-restore --configuration Release --logger "trx;LogFileName=unit-tests.trx" --results-directory TestResults/unit`. Evidence was retrieved using `gh run watch 44170253312 --repo DiegoDX/ProdeChampIA --exit-status --interval 10` (exit 1), `gh run view 44170253312 --repo DiegoDX/ProdeChampIA --log` and `gh run view 44170201210 --repo DiegoDX/ProdeChampIA --log-failed` (retrieval exit 0). Runner working directory was `D:\a\ProdeChampIA\ProdeChampIA`.

Those failures belonged to `MatchTests`: property preservation and missing argument validation. At initial publication, `Match.cs` contained a compile-only RED skeleton with an empty constructor. Comparing the initial publication head to original base `e39f37450c21e2cf7c02f0dbd11f524a0fd95272` showed no source, test or workflow diff; `git ls-tree` identified the same `Match.cs` blob `529a59e3171f85c07b04112162eaaa50d04b38d3` and `MatchTests.cs` blob `7286c7c3fa4b24cc73048db81bf7e3bfc0dddf9c`. This established that the implicated source and tests were already present in that base, without claiming a separately executed baseline test run. The separate authorized correction subsequently replaced this skeleton.

## Synchronization after human merge of PR #8 — 2026-11-17

The user reported **“Ya hice merge del PR #8”** after the agreed next step to update PR #7 and verify its checks. GitHub retrieval confirmed PR #8 is MERGED at `2026-11-17T21:17:07Z`, merge commit `0688b594a27716c64be27c296fa08480a352c62e`, also current remote main. Existing authorization for publication of the Skills feature branch is reused; no agent merged a PR or changed main.

Fetched `origin/main` and `origin/chore/development-factory-skills`, then integrated `origin/main` on the Skills branch with `git merge --no-commit --no-ff origin/main`. The sole conflict was two independently added engineering-report solution items in `ProdeChamp.slnx`; both entries were retained. All PRODE-002 correction files were retained from main. The only new manual changes for synchronization are this report and the solution conflict resolution. No product behavior, tests, packages or workflow were edited.

Current pre-publication verification, from `C:\ProdeChampIA\ProdeChampIA`:

- Solution XML, target existence and uniqueness: **6 projects, 27 unique existing solution items**, including both reports and the PRODE-002 evidence. Skill metadata and **28 local links across 11 active Skills** passed. No conflict markers remain in the solution.
- `git diff origin/main -- src tests .github/workflows` returned empty (exit 0). `Match.cs` blob is `d3288bba5a598e2c355d4e38e1755c016838cc28`; `MatchTests.cs` remains `7286c7c3fa4b24cc73048db81bf7e3bfc0dddf9c`, matching the corrected source already merged by the human.
- With the existing four `DOTNET_*` environment settings recorded in [PRODE-002 implementation evidence](../stories/PRODE-002/implementation-evidence.md), `dotnet build ProdeChamp.slnx --no-restore --configuration Release --verbosity minimal` passed (exit 0), all six projects, **0 warnings / 0 errors**.
- `dotnet test ProdeChamp.slnx --no-build --no-restore --configuration Release --logger 'trx;LogFilePrefix=factory-main-sync-regression' --results-directory tests/TestResults --verbosity minimal` passed (exit 0): **83 unit tests passed, 0 failed/skipped**; integration executed **0** cases. TRX files: `tests/TestResults/factory-main-sync-regression_net10.0_20261117171858.trx` (integration) and `...20261117171859.trx` (unit). Generated artifacts remain ignored and excluded from solution items.

The merge commit containing this report is followed by fresh push/PR Actions verification; final source SHA, run URLs, actual totals and outcome are recorded in the PR body and final handoff. Earlier failed checks and PR #8's passing runs are not claimed as verification of the new PR #7 head. Independent human review, full JSON Schema validation and Visual Studio UI inspection remain unperformed. AC3/team-validation deferral from PRODE-002 is unchanged.

## 7. References

- [Shared factory contract](../development-factory/README.md)
- [Review result schema](../development-factory/review-result.schema.json)
- [Repository instructions](../../AGENTS.md)
- [Existing CI workflow](../../.github/workflows/ci.yml)
