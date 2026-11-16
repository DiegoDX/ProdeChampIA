# Sprint 0 retrospective — PRODE-001

- Date: 2026-11-03.
- Scope: Sprint 0 foundation and the first completed story. This does not redefine the original [scaffold report](../engineering-reports/sprint-0.md).
- Baseline: local and remote `main` at `208e2288183a24d580ee95f8ff89f442ca38f0d5`, merge of [PR #3](https://github.com/DiegoDX/ProdeChampIA/pull/3).
- Retrospective branch: `docs/sprint-0-retrospective`.
- Authority: assessment and recommendations only; no production code, tests, acceptance criteria, Skills or remote settings changed. This task does not commit, push or merge.

## 1. Technical Lead assessment

**PRODE-001 satisfies its five accepted scoring criteria. The delivery workflow has substantial evidence, but approvals and independent review are only partially auditable.** The implementation is a small, pure Domain rule with meaningful unit tests. Retained TRX files demonstrate behavioral RED and GREEN; GitHub confirms successful checks for the final PR and actual merge.

Architect → QA → Developer → Reviewer → PR → CI was followed in the recorded narrative, with qualifications: CI also ran before PR creation; Git does not preserve separate architecture/QA/RED/GREEN checkpoints; the reviewer verdict survives in publication notes rather than a standalone review; and human stage approvals are reported without the original conversation available. It would be inaccurate to certify every stage boundary or reviewer independence from these records alone.

Before the next story, prioritize durable approval/review evidence, appropriate merge controls, earlier contract decisions and reliable source/build handoffs. Additional framework layers or integration fixtures would not improve this Domain-only delivery.

## 2. Evidence boundary

**Direct** means repository contents, Git objects, inspected TRX, or current GitHub responses. **Reported** means a historical document describes an action but its primary record is unavailable. **Unverified** means the sources cannot establish the claim. Missing evidence is not a verified process violation.

Sources inspected:

- [AGENTS.md](../../AGENTS.md), all 11 repository Skills, [architecture](../architecture/README.md), [ADR guidance](../architecture/decisions/README.md), product/story/testing documentation, solution/projects, calculator and both test classes.
- PRODE-001 [plan](../stories/PRODE-001/implementation-plan.md), [QA report](../stories/PRODE-001/qa-report.md), [implementation report](../engineering-reports/PRODE-001-engineering-report.md), [validation report](../engineering-reports/PRODE-001-validation-engineering-report.md) and [CI/publication report](../stories/PRODE-001/ci-report.md).
- Git history/diff from pre-story `main` `d4d85f0` through merge `208e228`, and source/test blobs compared with implementation commit `fe9155f`.
- [Issue #1](https://github.com/DiegoDX/ProdeChampIA/issues/1): CLOSED, original five criteria, no comments, updated `2026-10-28T15:25:17Z`. Negative-input behavior is absent from the Issue; additional approval is recorded locally.
- PR #3 body, commits, checks and metadata: MERGED by `DiegoDX` at `2026-10-28T15:25:15Z`; author also `DiegoDX`; `reviews=[]`, `comments=[]`, empty `reviewDecision`. No formal GitHub approval is recorded; this does not disprove review outside GitHub.
- Seven available CI runs: two pre-story and five story-related. Final PR/merge job steps and logs, artifact metadata, and current branch metadata were inspected. The branch API reports `main.protected=false`; historical protection and complete ruleset configuration were not audited.
- Fourteen local TRX files under `tests/ProdeChamp.UnitTests/TestResults/`, `tests/TestResults/` and `TestResults/`, plus four previously downloaded GitHub TRX files under `.local/PRODE-001-github-40529598727/` and `.local/PRODE-001-github-40530486568/`. Counters, individual results and failure messages were inspected. These ignored files exist here; they are not portable Git history or independently bound local source snapshots.

No original approval transcript, standalone reviewer report, per-behavior development transcript, coverage measurement or production execution evidence was available.

## 3. Workflow reconstruction and stage evidence

| Stage | Evidence and outcome | Assessment / gap |
| --- | --- | --- |
| Foundation | `acd01f0` initializes the solution; `0b2e301`/PR #2 adds Skills; `9ef10be` and `d4d85f0` update setup/instructions. Original Sprint 0 report records build success with zero tests. | Structure is inspectable. Original environment interventions are reported. Zero-test foundation checks do not validate PRODE-001. |
| Architect, 2026-10-23 | Plan identifies Issue #1, Domain-only calculator, four-integer contract, precedence, QA scenarios and validation question. `9d6b476` adds the architect Skill on the feature branch. | Design deliverable verified. Plan first committed with implementation; Git cannot prove its completion date or approval before QA. |
| QA / RED, 2026-10-23 | QA report maps all five criteria and records authorization for tests/minimal skeleton. TRX records 4 failures, then 34 failures, all `NotImplementedException` after discovery. | Behavioral RED corroborated. Initial permission and missing-type compilation failures correctly excluded from RED. Original authorization is reported only. |
| Developer, 2026-10-26 | Developer RED: 34 failed; GREEN: 34 passed. Validation RED: original 34 passed/new 10 failed because no exception was thrown; GREEN and final regression: 44 passed. | Two coarse-grained RED/GREEN cycles corroborated. Short cycles for every scoring behavior are not evidenced. Reports say no further refactoring was needed. |
| Reviewer, before publication | CI/publication report says independent review examined 21 changed files and 14 local TRX, returned APPROVED with no actionable P0–P3 findings, and did not rerun tests. | **Reported only** for execution and independence. No standalone review, reviewer identity or formal GitHub review available. Retrospective code inspection cannot replace historical review evidence. |
| Publication / PR, 2026-10-27 | `fe9155f` contains implementation/tests/support; `da522b4` adds publication evidence. PR created `15:20:59Z`, feature branch → main; final head `da522b4517914a6858404ea94a8a60d672fa2832`. | Branch/commits/PR directly verified. Authorization for commit/push/PR reported in documents/PR body; original message unavailable. |
| CI, 2026-10-27–28 | Feature push/PR and merge runs succeeded; first feature push CI preceded PR creation. Details below. | Direct remote evidence, plus retained PR TRX. The workflow overlaps rather than running as a strictly serial PR-then-CI chain. |
| Human merge / closure, 2026-10-28 | Merge has parents `d4d85f0` and `da522b4`; GitHub records `DiegoDX` merging PR #3; Issue #1 closed. | Human account action verified. No basis to allege an agent merged or pushed directly to main; no separate formal human review approval recorded. |

The plan/QA report date work to 2026-10-23; final design/QA closure approval is reported on 2026-10-26. QA separately records user authorization for its RED task. The precise earlier architect-to-QA approval boundary cannot be reconstructed: do not backdate later approval or assume an unauthorized transition.

Git contains three PR commits, not stage checkpoints. Relative to pre-story main, the PR spans **21 files, 1,225 additions and 11 deletions**; `fe9155f` alone spans 20 files. The calculator is 25 lines. Factory setup and documentation increased review scope, but the reports describe authorization for that supporting work; file count alone is not unauthorized scope expansion.

## 4. CI and test evidence

### Remote verification

| Run | Event / reported head | Result and evidence depth |
| --- | --- | --- |
| [40529598727](https://github.com/DiegoDX/ProdeChampIA/actions/runs/40529598727) | Feature push / `fe9155f` | SUCCESS metadata; retained downloaded TRX confirms 44 passed (34 scoring + 10 validation), integration 0. Historical report records build/upload success. |
| [40530093397](https://github.com/DiegoDX/ProdeChampIA/actions/runs/40530093397) | PR / `fe9155f` | SUCCESS in run list; detailed logs/TRX not inspected for this run. |
| [40530478380](https://github.com/DiegoDX/ProdeChampIA/actions/runs/40530478380) | Feature push / `da522b4` | SUCCESS in run list and PR checks; detailed logs/TRX not inspected for this run. |
| [40530486568](https://github.com/DiegoDX/ProdeChampIA/actions/runs/40530486568) | Final PR / `da522b4` | Jobs/logs: restore/build, unit-execution guard and upload succeeded; build 0 warnings/errors; 44 passed, 0 failed/skipped; integration 0. Retained TRX confirms counts and 34/10 split. |
| [40715775163](https://github.com/DiegoDX/ProdeChampIA/actions/runs/40715775163) | Main push / `208e228` | Jobs/logs: restore/build, guard and upload succeeded; build 0 warnings/errors; 44 passed, 0 failed/skipped; integration reports no tests available. Artifact metadata inspected; merge TRX not downloaded by this task. |

For the PR event, GitHub reports source head `da522b4`, while checkout logs show `refs/remotes/pull/3/merge`. The PR checks exercise a merge ref; the later main run separately verifies actual merge `208e228`.

Earlier successful runs [39673364514](https://github.com/DiegoDX/ProdeChampIA/actions/runs/39673364514) (`9ef10be`) and [39978279692](https://github.com/DiegoDX/ProdeChampIA/actions/runs/39978279692) (`d4d85f0`) do not verify this story.

The [workflow](../../.github/workflows/ci.yml) separates unit/integration results, rejects zero executed unit tests, uses read-only contents permission and uploads TRX with `always()`. The CI report describes a successful local synthetic zero-execution check; that fixture/run was not reverified here. A successful integration step with zero tests is not integration coverage.

Artifact metadata reports `expired=false` for final PR [12745966391](https://github.com/DiegoDX/ProdeChampIA/actions/runs/40530486568/artifacts/12745966391) (8,871 bytes, expires `2026-11-10T15:24:37Z`) and merge [12827644684](https://github.com/DiegoDX/ProdeChampIA/actions/runs/40715775163/artifacts/12827644684) (8,913 bytes, expires `2026-11-11T15:26:38Z`). Fourteen-day retention is operationally useful but insufficient as the only long-term portfolio evidence.

### Retained local results

| TRX evidence | Actual inspected result |
| --- | --- |
| `PRODE-001-exact-red.trx` | 4 executed/failed with `NotImplementedException`, 0 passed/skipped. |
| `PRODE-001-unit-red.trx`, `PRODE-001-developer-red.trx` | Each: 34 executed/failed with `NotImplementedException`, 0 passed/skipped. |
| `PRODE-001-developer-green.trx` | 34 executed/passed, 0 failed/skipped. |
| `PRODE-001-validation-red.trx` | 44 executed, 34 passed, 10 failed: `Assert.Throws() Failure: No exception was thrown`, expecting `ArgumentOutOfRangeException`. |
| `PRODE-001-validation-green.trx` | 44 executed/passed, 0 failed/skipped. |
| Regression pair ending `20261026154040` / `154041` | 34 passed unit tests / 0 integration tests. |
| Regression pair ending `20261026172035` / `172036` | 44 passed unit tests / 0 integration tests. |
| Final regression pair ending `20261026180309` / `180308` | 44 passed unit tests / 0 integration tests. |
| `TestResults/unit/unit-tests.trx`, `TestResults/integration/integration-tests.trx` | Local Release verification: 44 passed / 0 integration tests. |

These are completed historical runs inspected now, not new test execution. Timestamp order supports the narrative; local TRX does not independently establish exact source content at each historical run.

## 5. Does the implementation satisfy the story?

The [Issue](https://github.com/DiegoDX/ProdeChampIA/issues/1) is authoritative for these five criteria. The [calculator](../../src/ProdeChamp.Domain/Scoring/PredictionPointsCalculator.cs) and [scoring tests](../../tests/ProdeChamp.UnitTests/Scoring/PredictionPointsCalculatorTests.cs) implement/test them without persistence, ranking, authentication or frontend.

| Criterion | Implementation / assertion | Assessment |
| --- | --- | --- |
| AC1: prediction 2-1, result 2-1 → 5 | Both goal positions equal; return 5. Exact-score theory includes QA E01. | Satisfied; included in final passing PR suite and merge run. |
| AC2: prediction 2-1, result 3-0 → 3 | Equal outcome comparisons return 3. Correct-winner theory includes W01. | Satisfied; same passing evidence. |
| AC3: prediction 1-1, result 2-2 → 3 | Both `CompareTo` results are zero. Draw theory includes D01. | Satisfied; same passing evidence. |
| AC4: prediction 2-0, result 0-2 → 0 | Outcomes differ; return 0. Incorrect-outcome theory includes I01. | Satisfied; same passing evidence. |
| AC5: categories must not accumulate | Exact-score early return precedes outcome comparison; exact wins/draws assert exactly 5. Instance-reuse fact asserts 5, 0, 5. | Satisfied; implementation cannot return 8 or retain totals. |

Additional contract: four guards reject negative inputs before scoring. The [validation suite](../../tests/ProdeChamp.UnitTests/Scoring/PredictionPointsCalculatorValidationTests.cs) covers -1/`int.MinValue` for every parameter, checks exception type/`ParamName` for single-invalid cases, and rejects two exact negative-score cases. Ten passing cases verify this behavior. Approval is consistently reported in plan/reports/PR; it is not a sixth original Issue criterion or independently verified conversation approval.

The design is proportionate: stateless sealed Domain class, synchronous calculation without I/O, exact-score precedence, outcome `CompareTo`, standard .NET guards, no new product packages or speculative interface/endpoint. Source/test blobs match `fe9155f` and `208e228`; `git diff fe9155f 208e228 -- src tests` is empty. No accepted-story defect was identified in inspected code/assertions.

The tests cover both winners, every mismatched outcome direction, partial score matches, zero, large nonnegative integers, nonaccumulation, reuse and invalid inputs through public behavior. Forty-four cases are not a coverage percentage or exhaustive proof. API, SQL Server and Angular operation are unverified and outside scope; an empty integration project is not itself a missing acceptance criterion.

## 6. Manual interventions and problems

| Intervention / problem | Evidence and impact | Recommendation |
| --- | --- | --- |
| Clarify contract | Plan proposed negative rejection; QA deferred it; reports record later human approval and a separate 10-case RED/GREEN cycle. | Resolve behavior-changing input/error contracts before dependent tests. Record who approved what/when without rewriting original criteria. The follow-up was legitimate clarification. |
| Clarify “QA skill” | QA report says the user selected `test-design` and extended its design-only scope to executable tests/minimal skeleton. | Make invocation distinguish test design from authorized QA/RED execution; avoid repeated role/name clarification. |
| Environment permissions/packages | Foundation report records CLI-profile/NuGet access failures and process-local settings; QA/developer report `MSB3491`/`CS2012` and authorized reruns outside sandbox. | Preflight CLI/build/cache access and required permissions once; reuse valid authorization. Keep environment failure distinct from RED. Historical approval interactions are reported only. |
| Repeated Visual Studio visibility requests | QA/implementation follow-ups record solution items, user feedback, UI checks/reload attempts and `Compile Update` metadata. | Add solution items at creation; verify SDK inclusion and UI refresh separately. Preserve default compile globs and avoid duplicate Compile entries. |
| Guards disappeared after GREEN | Validation report describes source reverting without guards, editor/disk reconciliation, restoration and repeated build/regression. Final TRX shows 44 passed; current guards exist. Cause remains undetermined. | Identify saved source verified by tests; inspect disk diff at handoff; rerun affected checks after source changes. Do not attribute an unknown cause to user/editor. |
| Tooling/checker friction | Implementation report records repairing a root-path bug in a Markdown-link checker. Foundation/CI reports record missing Python; CI had no actionlint/YAML parser. | Use a small repeatable verification procedure; name the validator actually used. Add tooling for demonstrated needs, not speculative factory complexity. |
| CI scope clarification | CI report records an interrupted attempt before clarification and excludes it from completed verification. | Establish task mode from authorized scope before changing Skills/workflows. The interruption's cause is not established as a product/check failure. |
| Human review/publication/merge | Reports describe separately requested review/publication authorization; GitHub confirms publication and human-account merge. | Preserve human control and retain evidence. Do not request the same already-granted permission unnecessarily. Agents never merge. |

### Avoidable work and documentation drift

- **Status duplication:** `da522b4` synchronizes ten documents; the PR repeats status again. Current README/product/architecture/story notes still say PR review/merge is pending after the verified merge. Keep dated history, but link one maintained lifecycle record from current summaries. This retrospective records closure without rewriting those historical reports.
- **Mixed review scope:** Skills/CI setup, solution visibility and scoring shared one PR. Separate future factory maintenance when independently scoped/authorized; do not manufacture extra PRs for tiny business changes.
- **Duplicate CI:** two feature heads generated four push/PR runs, plus the merge run. Concurrency uses `github.ref`; push and PR refs differ, so it does not coalesce that pair. This is observable duplicate execution, not a demonstrated reliability failure.
- **Repeated broad checks:** source restoration justified final regression; final-head CI correctly rechecked the documentation commit. Preserve those. Solution-item-only edits need structural/link checks rather than invented behavioral TDD; reports already handled several such follow-ups appropriately.
- **Nonportable byte hashes:** current test files use CRLF; historical test SHA-256 values correspond to LF. Normalizing to LF reproduces both historical hashes; Git blobs remain identical through implementation/merge/current clean-filtered files. This does not show weakened assertions. Record commit/blob identity or explicit normalization in future evidence.

No reliable effort, token cost or stage-duration breakdown is available. Priorities reflect observed risk/friction, not invented productivity savings.

## 7. Do Codex Skills need improvements?

Yes: improve handoffs and evidence retention. Preserve existing safeguards against invented results, unapproved rules/dependencies and test weakening, and existing distinctions between zero discovery and passing behavior. Avoid adding another overlapping implementation/review Skill.

| Skill(s) | Proposed improvement | Completion signal |
| --- | --- | --- |
| `story-refinement`, `architect` | Retrieve authoritative GitHub Issue, then consult docs for decisions. Record contract blockers and compact approval references. Several Skills assume criteria are in `docs/user-stories/`, which is only an index here. | Next story identifies Issue/baseline, decisions/blockers and approval source before dependent work. |
| `test-design`, `tdd-implementation` | Keep design-only default; define explicit handoff to authorized executable QA/RED, including a minimum throwing skeleton when needed. Use established story-folder convention instead of competing default paths. | Invocation says design vs execution; RED records discovery and expected failure. No new generic QA Skill required. |
| `backend-implementation`, `tdd-implementation` | Add source/build reconciliation already explicit in `ci`: inspect saved disk source, preserve tests, identify commit/blob or uncommitted snapshot, rerun affected checks on change. Link one TDD evidence record. | Handoff identifies verified source and accurately describes focused vs batch cycles. No refactor required merely to fill a stage. |
| `code-review` | Define persisted default, e.g. `docs/stories/{STORY_ID}/review-report.md`; record reviewer role/identity when available, reviewed source, findings, AC coverage, test sources and advisory verdict. Current Skill already asks for Markdown; no standalone artifact survives here. | Review itself can be inspected independently of publication notes. Do not claim reviewer independence without evidence. |
| `engineering-report` | Link one current lifecycle status to dated stage records; distinguish approval claims from direct evidence; prefer portable source identities. | Final handoff reconciles Issue/PR/merge/CI without competing current summaries. |
| `ci` | Preserve zero-unit guard and local/remote distinction; capture event, source head, tested ref and merge run; assess duplicate triggers and retention deliberately. | Final evidence names final-head/merge checks and retains a durable summary before artifacts expire. |
| Deliverable-producing Skills | Reference existing AGENTS.md visibility requirements at handoff; recognize session authorization already granted. | New files appear through their projects/solution folders and no repeated unnecessary approval prompt is required. |

`frontend-implementation` was inspected but not exercised; no evidence supports an Angular-specific change. Separate `architecture-review` or `interview-preparation` sessions cannot be proven. Questions in reports do not establish that a Skill was invoked. These are evidence limits, not reasons to remove Skills.

## 8. Priorities before the next story

Proposed follow-ups and owners only; no Skill, GitHub setting or Issue change is executed here. P0 means factory readiness priority, not the code-review defect severity scale.

| Priority | Action / proposed owner | Reason | Verifiable completion |
| --- | --- | --- | --- |
| P0 | Durable approvals and review — Technical Lead + reviewer | Review is summarized only, original approvals unavailable, added contract absent from Issue. | Next story has Issue baseline, dated approval references and persisted advisory review tied to source. Remote Issue updates require explicit authorization; original criteria retained. |
| P0 | Agree/configure merge controls — human repository owner | Current branch API says `protected=false`; PR #3 has no formal approval record. Historical enforcement remains unknown. | Owner audits effective rules/rulesets and enables appropriate PR/required-CI controls where available; documents human review/merge responsibility and solo-owner limitations. Agents never merge. |
| P0 | Source/build handoff and environment preflight — developer + CI maintainer | Lost guards and repeated permission failures caused concrete rework. | Saved source identity matches verification; known cache/permission needs documented, valid authorization reused, affected checks rerun on source change. |
| P1 | Earlier contract decisions — Technical Lead/product owner + architect | Deferred validation required clarification and another cycle. | Input/error decisions or explicit blockers recorded before dependent tests; unaffected approved work can continue. |
| P1 | Targeted Skill/path/status updates — factory maintainer | QA-role ambiguity and duplicated summaries caused friction/stale status. | Consistent GitHub/docs lookup, explicit QA/RED handoff, persisted review, one lifecycle status. No overlapping new Skill. |
| P1 | Portable evidence retention — CI maintainer + reviewer | RED TRX ignored; remote TRX expires after 14 days. | Durable text summary records commands, counts/failure reasons, source identity and run/artifact links. Retention chosen deliberately; no generated results/logs in solution items. |
| P2 | Reduce CI/documentation churn — CI maintainer + Technical Lead | Four feature runs for two heads and ten-document synchronization. | Deliberate trigger/status policy retaining final-head and merge verification; measure savings before claiming them. |
| P2 | Reusable validators where justified — factory maintainer | Manual validation limitations and repaired link checker. | Small reproducible checks with actual tools/results reported; no speculative infrastructure or unrelated integration prerequisites. |

The next story should retain a compact Issue → approved decisions → QA design/RED → verified source → persisted review → authorized PR → final CI evidence chain. Make boundaries auditable without multiplying permission prompts or copying status into every README.

## 9. Retrospective delivery

| Changed file | Purpose |
| --- | --- |
| `docs/retrospectives/sprint-0.md` | Evidence, findings, manual interventions, story assessment and recommendations. |
| `ProdeChamp.slnx` | Link under docs/retrospectives for Visual Studio while retaining existing projects/items. |

Technical decisions: preserve historical reports, use this requested artifact as the retrospective handoff, distinguish direct/reported/unverified evidence, and propose process/Skill changes without applying them.

Tests/builds executed by this task: **none**, because changes are documentation/solution organization. Historical test evidence above was inspected, not rerun. Delivery verification covers final diff, solution XML, linked-file existence, all six retained projects, local Markdown destinations and unchanged source/tests.

Actual delivery checks: `git diff --check` passed; the solution XML retained all 6 projects and 8 existing solution items, with 1 new retrospective item; all linked targets and 13 local Markdown destinations exist. Whitespace/conflict-marker checks passed for the untracked report. Git inventory confirmed only this report and `ProdeChamp.slnx` changed; production, tests, Skills, workflows and AGENTS.md remain unchanged.

Known limitations: unavailable approval/reviewer transcripts, no independently bound historical local source snapshots, no coverage percentage/integration execution, no historical protection/effective-ruleset audit, no rerun of synthetic CI guards, and finite artifact retention. Visual Studio visibility is checked structurally; its UI is not inspected by this task.

## 10. Interview questions generated

1. **What distinguishes behavioral RED from a broken environment?** Discovery plus expected behavior-related failure: compare QA/developer throwing-skeleton results with permission/compilation failures. Validation RED shows 34 existing passes and 10 missing-exception failures; see section 4.
2. **Does an APPROVED note prove independent review?** It records a verdict. Reviewer identity, source scope and retained review evidence are needed to assess independence and actual coverage; see section 3 and PR metadata.
3. **Why distinguish PR source head, merge ref and actual merge SHA?** The PR runner checks out a merge ref; the main run separately verifies actual merge `208e228`. Tie checks to event/ref/source rather than a green badge; see section 4.
4. **Why are Domain unit tests sufficient here, and why validate first?** No approved I/O/HTTP boundary exists. Early guards prevent invalid exact scores receiving 5; public assertions verify precedence/exceptions. Add integration tests when an approved integration exists; see section 5.

