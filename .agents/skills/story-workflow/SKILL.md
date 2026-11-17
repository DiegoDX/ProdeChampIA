---
name: story-workflow
description: Coordinate the ProdeChamp GitHub user-story lifecycle using existing specialized skills for requirements, architecture, test-first development, independent reviews and reporting; publish a PR and verify Actions only when explicitly authorized.
---

# ProdeChamp story workflow

Read [AGENTS.md](../../../AGENTS.md) and the [shared factory contract](../../../docs/development-factory/README.md) for authoritative story lookup, stage prerequisites, artifact paths, verification evidence and the common handoff. Reuse approvals already granted for the same scope.

Coordinate stage selection, handoffs, authorization and evidence. Specialized skills own their analysis, design, tests, implementation, reviews and reports. Read their actual instructions instead of reproducing their checklists. This is a session workflow, not an unattended background service.

## Authority

- Read repository `AGENTS.md`, applicable nested instructions, and relevant product, architecture, ADR and testing documentation under `docs/`. GitHub Issues supply approved story baselines; `docs/` supplies product and architecture decisions. Report contradictions instead of inventing rules.
- Execute dependent stages sequentially. Proceed through authorized work without approval for routine files, local feature branches, verification or reports. Make reasonable technical decisions within accepted business requirements.
- Never silently change acceptance criteria, business rules, approved contracts or expected test outcomes. Stop affected work for unresolved product ambiguity, significant scope changes, unapproved dependencies or architectural decisions needing approval. Unaffected independent work may continue, but dependent stages remain blocked.
- Honor human stage boundaries in `AGENTS.md` and selected skills. The current `architect` requires human plan approval before QA. Complete the reviewable deliverable before requesting a missing approval, cite its exact instruction/path, and reuse approvals already evidenced for the same scope. Honor explicit user instructions about progression; invoking this coordinator alone does not approve an unseen plan or resolve product questions.
- Commits, pushes, Issue/PR writes and Actions mutations need applicable explicit authorization. Record its scope. Reading GitHub does not authorize writing. Never push directly to `main`, merge, enable auto-merge, expose credentials or deploy.
- Preserve pre-existing tracked/untracked work. Never reset, stash, delete, commit or publish unrelated changes. Stop for a concrete isolation decision if needed.

## Routing to existing skills

Read selected `SKILL.md` files under `.agents/skills/` before use; re-check actual names if the catalog changes. Record why inapplicable stages or specializations are skipped. Use the shared factory contract and its review-result schema when structured review output is requested.

| Skill | Responsibility and selection |
| --- | --- |
| `story-refinement` | Validate requirements/readiness; keep proposals distinct from the approved baseline. |
| `architect` | Prepare the implementation plan; observe required plan approval before QA. |
| `test-design` | Design traceable acceptance scenarios/test levels; it does not implement automated tests. |
| `tdd-implementation` | Implement executable tests and own observable Red-Green-Refactor cycles. |
| `backend-implementation` | Specialize TDD for affected C#/.NET/SQL Server layers; skip without backend scope. |
| `frontend-implementation` | Specialize TDD for actual Angular scope; skip without UI scope. |
| `architecture-review` | Review relevant architectural boundaries, references, dependencies, integrations or ADR proposals. |
| `code-review` | Review implemented story changes for criteria, correctness, regressions, security and test evidence. |
| `engineering-report` | Consolidate actual changes, decisions, verification and limitations. |
| `ci` | Verify Actions and diagnose failures; change pipelines only within approved scope. |

Use `tdd-implementation` as the shared method with applicable backend/frontend specialization, not competing implementations. Sequence full-stack slices by real dependencies and approved contracts. For structure-only or CI-only work, explain why behavioral TDD is inapplicable; use appropriate structural/build/CI checks without manufactured RED evidence.

## Persistent evidence

Create or resume `docs/stories/{STORY_ID}/workflow-status.md`. Link specialized artifacts instead of duplicating them. Add new documentation to `ProdeChamp.slnx` by repository folder; retain projects and SDK default source inclusion. Exclude generated logs, results, caches and secrets from solution items.

Record:

- Repository, Issue URL/ID, retrieval time, `updatedAt`, faithful acceptance-criteria baseline and approval evidence. Keep separately approved user decisions distinct from unchanged Issue criteria. If necessary, assign local traceability IDs without editing the Issue.
- Baseline branch/commit, intended base, pre-existing changes, selected skills, skipped-stage reasons, plan version and scoped authorization references.
- Per-stage status: `Pending`, `Running`, `Passed`, `Failed`, `Blocked`, `Not run` or `Not applicable`, with inputs, artifacts, evidence and unresolved items. File existence alone does not establish completion.
- Criterion-to-case/test-to-code traceability. For each execution: exact command, working directory, source revision/working-tree scope, exit code, observed result, discovered/executed/passed/failed/skipped counts where available, relevant failure output and artifact path. Redact secrets. Label examples as unexecuted.
- RED/GREEN/refactor observations; reviewer identity, reviewed scope, verdict, findings and disposition; correction counter; PR head SHA and CI run URLs/status.

Update after every stage, correction and stop. On resume, inspect the current Issue and working tree, verify prior evidence/approvals against current scope, and rerun invalidated checks. Do not promote historical results to current results or reset retries merely because a session resumed.

## Sequential stages

### 1. Retrieve the GitHub Issue

Resolve the Issue and repository from the request and repository metadata. Use `gh issue view <number-or-url>` to retrieve body, state, labels, comments, URL and update timestamp, for example with `--json number,title,body,state,labels,comments,url,updatedAt`. Inspect referenced dependencies/PRs only as relevant. Record the actual command/outcome without credentials.

Check branch, HEAD and working-tree status. Work on a story feature branch from the verified intended base; reuse an appropriate branch. Do not blindly carry unrelated dirty work across branches. Verify approval from actual Issue/conversation evidence; a story label is not approval. If the Issue, approval or essential source is unavailable, block dependent implementation rather than inventing a replacement Issue or contents.

### 2. Refine and validate requirements

Invoke `story-refinement` with the Issue and relevant docs. Persist its handoff at `docs/stories/{STORY_ID}/refinement.md`. Advance only with clear observable outcomes and accepted scope. `Ready for approval` is not approval. Keep proposals unapproved and resolve blocking business questions before advancing affected work. Do not update the Issue without separate authorization.

### 3. Prepare architecture

Invoke `architect` with validated requirements and actual repository context. Check `docs/stories/{STORY_ID}/implementation-plan.md` against the baseline, affected layers, decisions, test strategy, risks and open questions. The architect stops at its handoff; the coordinator enters QA only with required plan approval evidenced. Reuse an approved plan only after checking its continued applicability.

### 4. Design and implement acceptance tests first

Invoke `test-design` after the plan gate. Persist its artifact using its established convention. Map every approved criterion to observable scenarios, test levels, expected outcomes and prerequisites. Route ambiguous expectations back to refinement; QA must not invent rules.

Then hand the design to `tdd-implementation` with applicable implementation specialization. Implement executable tests before production behavior using actual repository tooling; do not require a new E2E framework. For each behavior, run the focused test and verify behavioral RED before stage 5. Compilation, discovery, setup and unavailable-environment errors are `Blocked`/`Failed`, never validated RED. If compilation needs a minimal public shape, keep it nonfunctional and record it separately; do not implement the behavior before observing its failing test.

Repeat stages 4-5 per small behavior increment in dependency order. If a test immediately passes, determine whether behavior already exists or the test is ineffective. Never break production to manufacture RED; record already verified behavior accurately.

### 5. Implement approved functionality

Continue the same TDD handoff with selected backend/frontend specialization. Implement the approved behavior for the verified failing test, observe GREEN, refactor and rerun. Repeat for remaining behavior. Return significant scope/design changes to the appropriate human decision gate; do not silently extend the plan.

### 6. Execute verification and correct failures

Have the implementation skill execute relevant build, focused and regression checks with real repository commands. Include SQL/API/Angular checks only where applicable. Confirm tests actually executed. Zero discovery, skips, interrupted commands and missing prerequisites do not establish acceptance coverage. Use isolated integration fixtures; mocked tests do not verify required real boundaries.

Route failures to implementation, or `ci` for pipeline defects within scope. Apply the bounded correction policy. Preserve legitimate tests and document any objectively invalid test correction. Rerun affected checks and update traceability. Required failed/blocked checks prevent a successful delivery claim.

### 7. Review architecture and code

Review a stable diff against the verified base, including relevant untracked deliverables and source/test evidence. Select `architecture-review` for architectural impact; use `code-review` for implemented story changes.

When subagents are supported, this skill authorizes separate review-only agents: an architecture reviewer when relevant and a code reviewer, neither having implemented the change. Supply the Issue, approved plan, baseline/current scope, raw test evidence and repository rules; let them form their own findings. These independent passes may run concurrently on the same frozen scope. Do not mutate reviewed code while they run. Delegation does not authorize GitHub writes or external messages.

Persist `architecture-review.md` when selected and `code-review.md` under `docs/stories/{STORY_ID}/`, using the shared verdict/severity contract. Include reviewer identity, independence and reviewed source; validate requested JSON against the shared schema and evaluate its semantic gates. Route corrections to implementation, never reviewers. After fixes, rerun affected tests and review passes. Earlier approval does not cover changed code.

Without independent reviewers, perform available separate passes labeled `Self-review; independent review not performed`. Do not self-approve through `code-review` or claim independent clearance. Mark independence unverified and require external human review before that gate passes. Honor blocking findings and `BLOCKED` verdicts. Technical verdicts never authorize merging.

### 8. Generate the engineering report

Invoke `engineering-report` with baseline, actual diff, traceability, commands/results, correction history and reviews. Persist under `docs/engineering-reports/` using story ID and existing convention. Verify claims against evidence. If delivery is blocked, produce a partial/blocked report; reporting may run even when earlier stages could not finish.

### 9. Publish a PR and verify Actions only when authorized

Prepare a reviewable local PR title/body from the final diff, criteria coverage, tests, reviews and limitations. If commit/push/PR authorization is missing, record `Not run - awaiting publication authorization`, present prepared scope and stop before that action. Reuse existing explicit authorization within its scope. Complete reviewable work before asking for missing final authorization; cite the applicable `AGENTS.md` permission rule.

Before publication, re-fetch the Issue, verify the intended base and current diff, and ensure approvals, checks and reviews still cover that exact source. Prepare the complete title/body before requesting any missing final authorization.

With authorization and prior verification/review gates satisfied, stage only inspected story files, commit only if authorized, push only the feature branch, then use `gh pr create` or an explicitly authorized update to a matching existing PR. Check for matching PRs first to avoid duplicates. Use a body file for multiline text. Record actual SHA, branch, PR URL and base/head. Never close the Issue as complete with unverified/deferred criteria.

Invoke `ci` with `gh pr checks`, `gh run list` and `gh run view` as appropriate. Correlate required checks/runs with the exact PR head SHA and run URLs. Pending, cancelled, missing and stale checks do not pass. Poll at most 10 times over at most 10 minutes, with waits no longer than 60 seconds; if still pending, record `Blocked - CI pending` and hand off URLs. Do not dispatch, rerun or cancel Actions without applicable explicit authorization.

For failures, retrieve logs and route defects to implementation or `ci`. Apply the shared correction budget, rerun affected local checks and renew affected reviews before corrective publication. If authorization does not cover another push, prepare the fix and stop at that boundary. Refresh CI/report/ledger evidence with final remote outcomes. Never merge, enable auto-merge or deploy.

## Bounded correction policy

- Allow at most **three unsuccessful correction rounds total per story run**, shared across local tests, reviews and CI. Persist across handoffs/resumption; never reset by renaming failures or switching skills. Successful TDD increments and the first expected behavioral RED are not failed corrections.
- Each round records failure/finding, hypothesized cause, in-scope fix, rerun of failing check and affected regression/review checks. Increment if any targeted blocker remains or a new blocking regression appears. Record incomplete commands accurately.
- Stop earlier for missing authority, unresolved product decisions, significant scope changes, unsafe prerequisites or an external blocker without an actionable local fix. Do not repeat unchanged commands against unavailable prerequisites.
- At the third unsuccessful round, stop corrections and publication. Preserve work and produce a partial report with attempted fixes, remaining failures, evidence gaps and the specific next decision/action. Only explicit user direction can reopen work with a new bounded budget.

## Final handoff and invocation

Report Issue, branch, stage outcomes, files changed, technical decisions, actual checks/results, review verdicts/independence, limitations and PR/CI URLs or why publication did not run. Distinguish local engineering completion, publication readiness and full story completion. No stage passes without required evidence/authorization; a passing subset cannot hide unresolved criteria.

Local invocation:

```text
Use $story-workflow for GitHub Issue #<number>. Run the local lifecycle within approved scope. Do not commit, push or create a PR.
```

After reviewable work and required approvals exist:

```text
Use $story-workflow to resume Issue #<number> from workflow-status.md. I authorize committing the reviewed story files, pushing its feature branch and creating the PR against main. Verify GitHub Actions. Do not merge.
```

If the client has not listed the new skill, ask it to read `.agents/skills/story-workflow/SKILL.md` explicitly and follow it for the Issue. Creating this coordinator does not authorize running a story.
