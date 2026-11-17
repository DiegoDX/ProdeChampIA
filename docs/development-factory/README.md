# Development Factory contract

Repository `AGENTS.md` governs permissions and engineering constraints. This document owns shared handoffs; each Skill owns its specialist decisions. Apply explicit user instructions and reuse approval already granted for the same scope.

## Baselines and authorization

- For story work, retrieve the GitHub Issue using `gh issue view` with body, comments, URL and `updatedAt`. Record retrieval time, faithful acceptance criteria and actual approval evidence. Labels alone do not prove approval.
- GitHub Issues own approved story criteria; `docs/product/`, architecture docs and accepted ADRs own documented product and architecture decisions. `docs/user-stories/` is a supporting index. Keep separately approved user decisions distinct from the unchanged Issue. Report contradictions or inaccessible sources; never invent their contents.
- Preserve requirements and legitimate tests. Stop affected work for business ambiguity, scope expansion, significant unapproved architecture changes or new dependencies. Continue independent authorized work when possible.
- For a full story lifecycle, verify the human-approved implementation plan before QA, and the QA design before executable tests/implementation. Plans and designs are inputs, not approval. Re-check approvals when their scope changes.
- Explicitly scoped maintenance or corrections may use the user's task as their authorization baseline without inventing a GitHub story or forcing irrelevant stages. Record scope, local plan, checks and why any lifecycle stages do not apply. This does not bypass business-rule, dependency or architecture approval.
- Work on an appropriate feature branch and preserve pre-existing work. If branch creation or file writes are denied, report the exact restriction; do not change permissions or treat a patch proposal as an applied change.
- Commits, pushes, Issue/PR writes and Actions mutations require applicable explicit authorization. Prepare a concrete reviewable result first. Reading GitHub does not authorize writing. Agents never merge, enable auto-merge or deploy under this workflow.

## Ownership

| Owner | Responsibility |
| --- | --- |
| `story-workflow` | Sequence stages, approvals, correction budget and current lifecycle status. |
| `story-refinement` | Observable business requirements, illustrative scenarios and unresolved product decisions. |
| `architect` | Minimal technical plan, contracts, affected boundaries and test strategy. |
| `test-design` | Detailed acceptance-case matrix, priorities, fixtures and implementation order; design only. |
| `tdd-implementation` | Executable tests, observable RED/GREEN/refactor cycles and one implementation evidence record. |
| Backend/frontend specialization | Stack-specific implementation and validation within that same TDD pass. |
| Architecture/code reviewer | Review a frozen source scope, report actionable findings and advisory technical verdicts. |
| `engineering-report` | Consolidate final results by linking evidence, without re-running every stage or copying logs. |
| `ci` | Pipeline configuration/diagnosis and source-bound Actions results; no product behavior changes. |

Interview preparation and interview questions are excluded from required deliverables and automatic story stages. Preserve historical interview documents; do not regenerate or delete them as part of routine story work.

## Artifact paths

For a story, use `docs/stories/{STORY_ID}/`:

| Artifact | Owner |
| --- | --- |
| `workflow-status.md` | Coordinator; the one current lifecycle record. |
| `refinement.md` | Refinement, when a persisted refinement is needed. |
| `implementation-plan.md` | Architect. |
| `test-design.md` | QA design. |
| `implementation-evidence.md` | TDD implementer; append RED/GREEN/refactor and relevant verification evidence. |
| `code-review.md`, `architecture-review.md` | Respective reviewers. |
| Sibling `code-review.json`, `architecture-review.json` | Respective reviewers, only when structured output is requested. |
| `ci-report.md` | CI. |

Final engineering reports belong in `docs/engineering-reports/<story-or-task-id>-engineering-report.md`. For maintenance without a story, use suitably named reports in that directory; no artificial story folder is required.

Reuse an existing approved QA/evidence artifact, including `qa-report.md`, when it already serves the purpose. Link it from the lifecycle record instead of relocating history, overwriting evidence or creating competing copies. New documentation and relevant non-project deliverables must appear in `ProdeChamp.slnx`; retain all projects and exclude generated results, caches, logs and secrets.

## Handoff and evidence

Every completed task reports files changed, technical decisions, checks actually executed and known limitations. A stage adds only its specialist findings and artifact links.

A stage handoff identifies:

- Story/task and acceptance-criterion IDs, input artifact revisions and approval references.
- Source: verified base commit, reviewed/tested revision and any relevant uncommitted or untracked scope. Use commit/blob identity or a recorded snapshot; file existence and historical hashes alone do not establish the current source.
- Outcome: `Pending`, `Running`, `Passed`, `Failed`, `Blocked`, `Not run` or `Not applicable`, with reasons and unresolved criteria.
- Exact executed commands, working directory, configuration, exit codes, observed outcomes and discovered/executed/passed/failed/skipped counts where available. Record the failure that establishes RED and label examples as unexecuted.
- Required next action and the evidence invalidated by later source/contract changes.

Reconcile saved source with build outputs before using `--no-build`. Rerun affected checks and review after changes; do not promote stale results to current evidence. Compilation, discovery and fixture failures are not behavioral RED. Zero discovery, skipped or interrupted required tests do not establish coverage. Use real isolated SQL/API boundaries when required; mocked UI tests do not verify a live API.

Read the actual SDK, xUnit package, test runner and Angular workspace before selecting commands/APIs. Do not migrate frameworks or add tooling implicitly. A successful subset cannot conceal an unresolved required check. Keep durable summaries and CI URLs before generated artifacts expire.

## Review output and gates

New code and architecture reviews use the same advisory verdicts:

| Verdict | Meaning |
| --- | --- |
| `APPROVED` | Required review completed on the identified source with no blocking findings. |
| `CHANGES_REQUESTED` | One or more evidenced blocking findings require correction. |
| `BLOCKED` | Essential context, evidence or required reviewer independence is missing. |

Both use P0 (critical), P1 (high), P2 (medium), P3 (low). P0/P1 always block; a P2 blocks only with demonstrated impact and explicit rationale; P3 does not block. Record reviewer identity and `Independent`, `Self-review` or `Unknown`; a self-review never clears the independent review gate. A reviewer must not implement their own corrections. Historical reports retain their original vocabulary.

When JSON is requested, use [review-result.schema.json](review-result.schema.json) and record whether schema validation actually ran. Markdown and JSON must describe the same source, findings and verdict. Do not fabricate fields when evidence is missing.

Before accepting a review, the coordinator checks both schema (when JSON is used) and semantics: source matches current scope, reviewer independence is evidenced, criteria and required checks are covered, blocking findings are resolved and the claimed verdict is justified. Schema validity proves structure only; it neither verifies evidence nor grants publication/merge approval. Missing or invalid required review output blocks the gate.

## Publication and resumption

The coordinator preserves its existing budget of three unsuccessful correction rounds across tests, reviews and CI. Expected first RED and successful TDD increments are not failed corrections. Do not reset the budget on resume.

Before publishing, re-fetch the Issue, verify the intended base and current source, and confirm approvals, required checks and reviews still cover it. Prepare the complete local PR title/body before requesting missing publication authorization. After publication, bind required Actions results to the exact PR head and tested ref; pending, missing, cancelled or stale checks do not pass.

The Skills describe a session workflow. They do not install an unattended runner, schedule background execution or enforce GitHub protections. An external controller would need durable state and approval enforcement; adding this contract does not create that controller.
