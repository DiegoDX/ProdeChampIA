# PRODE-004 - Workflow status

- Issue: [#10 - Block prediction edits at scheduled kickoff](https://github.com/DiegoDX/ProdeChampIA/issues/10), OPEN.
- Retrieved on 2026-11-19 before `2026-11-19T14:37:38.3150459Z`, using `gh issue view 10 --repo DiegoDX/ProdeChampIA --json number,title,body,state,labels,comments,url,updatedAt`, exit 0. Issue updatedAt: `2026-11-18T23:50:54Z`; no comments. The story label is not approval.
- User authorization: after concrete plan v1 was presented, the user instructed "Implement PRODE-004" and "execute the complete development workflow", explicitly authorizing files, local commits, feature-branch push and PR creation, build/tests/reviews and in-scope fixes, stopping at passing CI for final review and prohibiting merge. This authorizes progression through plan/QA/TDD for unchanged scope; no separately signed unseen QA design is claimed. Business changes and Issue writes remain unauthorized.
- Branch: `feature/PRODE-004`; verified baseline main: `950ec8963012f2367ae2a95a1891f8b10ba84458`. Implementation commit/local HEAD after user-terminal publication: `33caab5a34717d2b058bc64f9bbe4bbe5af3e77b`, verified against the published branch ref. Any final documentation-only head is identified by PR #13 and its exact-head verification in the PR body/Checks.
- Pre-existing changes: `.agents/skills/architect/SKILL.md` and `.agents/skills/tdd-implementation/SKILL.md`. Preserved; exclude from story commits/publication.
- Environment: initial directory/file writes were denied. User refreshed inheritance on docs, src, tests and .local from their own PowerShell. Current probes passed directory creation, file creation/modification/read-back and cleanup in docs/stories, src, tests and .local; apply_patch write/read-back also passed in docs/stories. No permission changes were executed by the agent.
- Current stage: **User-authorized contract correction locally verified and independently reviewed; publication to existing PR #13 in progress**. Current correction source/evidence below supersede original-contract completion claims.
- Unsuccessful correction rounds: **0/3**. Environment prerequisites were not behavioral RED or implementation correction rounds.

## Stage ledger

### Current contract correction - 2026-11-19

The user explicitly requested instance `Prediction.CanEdit(DateTimeOffset matchStartTimeUtc)` with internal server time, removal of PredictionEditCutoff, Guid matchId in Create and matching test changes; then authorized uploading to the PRODE-004 branch and its PR to main. This is separately approved scoped maintenance, not an edit to Issue criteria. The [correction plan/evidence](../../engineering-reports/prediction-contracts-implementation-evidence.md) and [report](../../engineering-reports/prediction-contracts-engineering-report.md) own the current scope.

- Branch/PR: existing feature/PRODE-004 / [PR #13](https://github.com/DiegoDX/ProdeChampIA/pull/13), OPEN/non-draft, base main. Remote parent `48fd50992998e3f4a0f2663a1e958dbdbb1b0c54`; main `950ec8963012f2367ae2a95a1891f8b10ba84458`. Fresh Issue unchanged; updatedAt `2026-11-18T23:50:54Z`.
- Verification Passed: restored missing new test source to its identical tested hash, fresh Release build0 warnings/errors and 146 unit passes/0failed/skipped. Integration0, no coverage claim. Original source/test snapshot now replaced by four hashes in correction evidence.
- Reviews Passed: independent [code](../../engineering-reports/prediction-contracts-code-review.md) and [architecture](../../engineering-reports/prediction-contracts-architecture-review.md) reviewers APPROVED, no findings, matching final hashes/raw evidence.
- Publication Running: use guarded non-forced Git Data feature-ref publication because local .git remains read-only; retain existing remote parent, exclude unrelated .agents edits and update existing PR rather than duplicating it. Exact-head CI pending publication.
- Refinement/architect/QA lifecycle stages Not applicable to this explicitly scoped correction; user decisions and local plan/matrix recorded before implementation. No new business rules/dependencies or Issue edits. Frontend/SQL/API remain outside scope. Skills used: TDD/backend/report for implementation, story-workflow/code-review/architecture-review/ci for publication.

### Original-contract lifecycle (historical)

| Stage / skill | Status | Evidence / next action |
| --- | --- | --- |
| Issue / branch / base | Passed | Fresh Issue unchanged; feature branch based on verified main; published implementation commit verified. |
| story-refinement | Passed | [Refinement](refinement.md); no unresolved business questions or proposed criteria changes. |
| architect / progression authority | Passed | [Plan v1](implementation-plan.md) presented before user's explicit full-workflow implementation instruction. No contract/business change. |
| test-design | Passed | [QA design v1](test-design.md), 21 rows plus R01/R02 review obligations, prepared before executable tests under explicit progression instruction. |
| tdd-implementation / backend-implementation | Passed | [Evidence](implementation-evidence.md), three observed RED/GREEN cycles, no failed corrections. |
| Frontend / SQL / API implementation | Not applicable | Explicitly outside Issue scope. |
| Build / automated tests | Passed | Release build 0 warnings/errors; 21 focused and 147 total unit passes, failed/skipped 0; integration executed 0 (no coverage claim). |
| Independent architecture / code reviews | Passed | [Architecture](architecture-review.md) and [code](code-review.md): distinct review-only agents, APPROVED/no findings; both verified R01/R02/raw TRX/current hashes and independently reran 21 passes. |
| engineering-report | Passed | [Report](../../engineering-reports/PRODE-004-engineering-report.md) consolidates actual implementation/checks/reviews/publication and explicit final-head verification boundary. |
| Publication / CI | Running | User committed/pushed reviewed15 files as 33caab5; agent created [PR #13](https://github.com/DiegoDX/ProdeChampIA/pull/13). Implementation push CI passed (147 unit/integration0 verified remotely); final exact-head actual checks must pass and be recorded in PR body/Checks. |

## Approval boundary

The original planning handoff stopped before QA as required. After that concrete plan was presented, the user explicitly instructed the complete implementation workflow through PR/CI. Apply that higher-priority progression instruction to unchanged scope without another intermediate pause; record QA design before tests and preserve all business requirements. Authority comes from this user's instruction, not file existence, unrelated skill edits or PRODE-003 approvals. No merge authority exists.

## Verification evidence

Working directory: `C:\ProdeChampIA\ProdeChampIA`; source boundary: base SHA above plus planning documents and solution links, with the two pre-existing skill edits preserved.

- Current permission probe: for each of src, tests and .local, create a GUID-named `.codex-write-probe-*` directory, create `probe.txt`, overwrite its content, verify read-back, remove that file and its empty directory. Command exit 0; all three passed. Earlier identical docs/stories probe and an apply_patch probe passed and were removed. These are filesystem checks, not product tests.
- Cached Issue snapshot: `.local/PRODE-004/issue-baseline.json`, retrieved exit 0; ignored local evidence, excluded from solution items. Persistent faithful baseline follows below.
- Documentation/solution validation: `. '.\.local\PRODE-004\verify-planning.ps1'`, exit 0 at `2026-11-19T14:40:26.0173061Z`. Valid solution XML; all 6 projects retained; all 49 unique solution paths exist; all 4 new documents are linked; all 14 relative Markdown links in the new documents resolve; Issue body matches the captured JSON exactly; tracked diff and new-document whitespace checks pass; production/tests have no diff; local evidence is ignored. Script is ignored local verification tooling, not a solution item. A final rerun after evidence-only annotations must remain green.
- Plan v1 SHA256: `293F493135487DA343BEC717FBC0966CACCCDC29550EB2BB31981119543A6DE6`. Approval applies to this proposed contract; later technical changes require reassessing scope/approval.
- The preceding four-document/no-source-diff validation is historical planning evidence, superseded for implemented scope by [implementation evidence](implementation-evidence.md). Build/tests now ran successfully on the frozen source hashes recorded there; 147 unit passes and integration0. The original plan SHA above identifies the presented technical content before authorization annotations, not the current annotated file hash.
- Final local document/source check: `. '.\.local\PRODE-004\verify-story.ps1'`, exit 0 at `2026-11-19T14:52:30.2934732Z`: 6 projects, 54 existing unique solution paths, 9 new documents, 23 valid relative links, exact Issue body, frozen source/test hashes, unchanged existing source/tests, whitespace and document credential-pattern checks. Rechecked scope hashes after independent reviews; unchanged. Coordinator semantic gate confirms distinct reviewer identities/independence, matching current source, all criteria/R01/R02, raw genuine evidence, APPROVED verdicts with no findings. No JSON requested or schema claim.
- Publication preflight: fresh Issue body/updatedAt unchanged; remote main still equals base; no matching PR; main protected=false and sole ruleset disabled. Exact-head actual CI must still pass even without enforced required checks. No Issue write or merge authorized.
- Publication attempt: `git -c safe.directory=C:/ProdeChampIA/ProdeChampIA add --` followed by the exact 15-file whitelist in `.local/PRODE-004/publish-reviewed-story.ps1` failed exit128: `fatal: Unable to create 'C:/ProdeChampIA/ProdeChampIA/.git/index.lock': Permission denied`. Subsequent cached checks/commit/push were not executed. Session filesystem policy marks .git read-only; no attempt to change permissions or bypass that restriction. Prepared scoped user-terminal handoff, with branch/base/current hash checks and unrelated staged-file protection. This is an environment prerequisite, not a product correction round; count remains0/3.
- User-terminal handoff subsequently succeeded: 33caab5 committed/pushed, exact15-file scope verified; only unrelated skill edits remained dirty. Agent-created PR #13 targets main and is non-draft. Push [44549746982](https://github.com/DiegoDX/ProdeChampIA/actions/runs/44549746982) and PR run [44549866439](https://github.com/DiegoDX/ProdeChampIA/actions/runs/44549866439) both succeeded on exact implementation head. Downloaded raw remote TRX confirms unit147/integration0, including21 new cases; PR log confirms tested synthetic merge d6d9b5b9d3dc129c914718006a4c75b2a15a187d and build0 warnings/errors. Source/test hashes and independent verdicts unchanged. Final metadata-head checks are verified separately in PR body/Checks.
- Final documentation-only checkpoint uses the authorized GitHub Git Data API to publish exactly this ledger, ci-report and engineering report on the feature branch with parent33caab5 and force=false, preserving the local .git restriction. Exact final PR head/check evidence must be verified in the PR body/Checks, not inferred from the older source-head runs. Local .git metadata remains at33caab5 until user synchronization; do not reset/stash unrelated skill edits. No source/test changes or new product correction rounds.

## Issue baseline

The following body is reproduced verbatim from the retrieved Issue. Technical proposals are separate in the plan.

```text
### User Story

As a participant,
I want attempts to change a prediction to be blocked from the match’s scheduled kickoff onward,
so that all participants have the same deadline for changing their predictions.

### Business Rules

1. The cutoff is the match’s scheduled kickoff date and time.
2. Eligibility is determined using the server time when the server validates the edit attempt.
3. If validation occurs strictly before scheduled kickoff, the attempt passes the cutoff rule.
4. If validation occurs exactly at scheduled kickoff or afterward, the attempt is blocked.
5. An attempt validated before kickoff passes the cutoff rule even if saving finishes after kickoff.
6. Opening an editing form before kickoff does not extend the deadline.
7. Evaluating the cutoff rule does not modify the existing prediction.
8. Actual kickoff delays do not extend the scheduled deadline.

### Acceptance Criteria

For the following scenarios, scheduled kickoff is 15:00:00 UTC on the match date.

#### AC-01 — Allow an attempt validated before kickoff

Given an existing prediction associated with the match,
When the server validates an edit attempt at 14:59:59 UTC,
Then the attempt passes the cutoff rule.

#### AC-02 — Block an attempt validated exactly at kickoff

Given an existing prediction associated with the match,
When the server validates an edit attempt at 15:00:00 UTC,
Then the cutoff rule blocks the attempt.

#### AC-03 — Block an attempt validated after kickoff

Given an existing prediction associated with the match,
When the server validates an edit attempt after 15:00:00 UTC,
Then the cutoff rule blocks the attempt, regardless of when the participant began editing.

#### AC-04 — Use validation time when processing crosses kickoff

Given the server validates an edit attempt before scheduled kickoff,
When saving finishes after scheduled kickoff,
Then the attempt remains permitted by the cutoff rule.

#### AC-05 — Preserve the existing prediction during evaluation

Given an existing prediction,
When the cutoff rule is evaluated,
Then the evaluation leaves the prediction unchanged, whether the attempt passes or is blocked.

### Scope

This story covers only the cutoff rule. Passing this rule does not establish that an edit satisfies other validation or permission requirements.

### Out of Scope

- Implementing prediction editing or saving.
- Creating new predictions.
- User interfaces and participant messages.
- API endpoints.
- Authentication and authorization.
- Score validation and scoring calculations.
- Match rescheduling and its effect on the deadline.
- Additional restrictions based on match status.

### Dependencies

- Each prediction is associated with a match.
- Each match has a scheduled kickoff date and time in UTC.
- The server time at validation is available for comparison.
```
