---
name: engineering-report
description: Produce an evidence-based ProdeChamp engineering report for an approved story or completed engineering task. Use after implementation and verification, or when the user requests a report; distinguish verified results from incomplete or unexecuted checks.
---

# Engineering Report — ProdeChamp

## Purpose
Create an auditable, concise handoff of **what actually changed**, **why**, **how it was verified**, and **what remains unresolved**. This is a reporting skill, not authorization to implement, approve, merge, push, deploy, or change requirements.

## Sources of truth (read before writing)
1. `AGENTS.md` and the approved story/acceptance criteria in `docs/user-stories/`.
2. Applicable product and architecture decisions in `docs/`, including ADRs.
3. Actual working-tree diff, changed files, and their relevant surrounding code; inspect uncommitted changes where available.
4. Real test/build/lint output, CI results, and existing implementation or QA review notes if accessible.
5. Related questions under `docs/interview-questions/` when they exist.

Never substitute the plan or an agent's claim for repository or command evidence. If a source is unavailable, state the gap. Respect `docs/` as the project's decision source of truth and keep reports aligned with the approved scope.

## Procedure
1. **Identify the unit of work:** story/task ID, title, baseline, acceptance criteria, and expected scope. If there is no approved story for behavior changes, flag this as a blocker rather than treating assumptions as requirements.
2. **Inspect actual changes:** list changed/added/deleted files with one-line purpose; identify meaningful behavior, contract, schema, dependency, or configuration changes. Exclude incidental files unless relevant. Distinguish files actually changed from files merely referenced.
3. **Trace acceptance criteria:** for each criterion, cite implementation evidence and relevant tests, and classify as `Verified`, `Implemented—not verified`, `Not implemented`, or `Blocked`. Do not mark a criterion verified solely because code exists.
4. **Record decisions and trade-offs:** explain rationale and consequences, especially Clean Architecture boundaries, testability, security, SQL Server typed parameters (never `AddWithValue`), dependency approval, and out-of-scope exclusions when relevant. Do not imply a proposed ADR is approved.
5. **Collect verification evidence:** quote exact commands and relevant outcomes from recorded runs. Use `Passed`, `Failed`, `Not run`, `Blocked`, or `Zero tests discovered`. State totals and environment limitations when known. A command invocation without a completed result is not a pass. Distinguish local verification from CI.
6. **Document risk and follow-ups:** concrete limitations, failures, remaining tasks, and suggested owners/actions when supported. Flag unauthorized dependency additions, scope changes, missing approval, and security concerns; do not silently normalize them.
7. **Generate interview preparation:** include 3–5 technically probing questions tied to real decisions, tests or trade-offs. Add brief answer pointers and file references. Never claim planned technologies have been implemented. Link separate artifacts under `docs/interview-questions/` if present; create them only if explicitly requested.
8. **Reconcile the report:** compare claims with the final visible repository state and available results. Avoid claims of commit, PR, merge, deployment, release, or production verification without explicit evidence.

## Output format
Use this structure in the response and, when a report artifact is requested or appropriate, in `docs/engineering-reports/<story-or-task-id>-engineering-report.md` (follow existing repository naming conventions when present):

```markdown
# Engineering Report — <Story / Task ID and title>

- Date: <date of report>
- Scope / reference: <approved story and documents>
- Outcome: Complete | Partial | Blocked | Not verified
- Evidence boundary: <branch/working tree or commit, if known>

## 1. Executive summary
<2–5 factual sentences: what changed and readiness/limitations>

## 2. Acceptance criteria traceability
| Criterion | Status | Implementation evidence | Verification evidence |
|---|---|---|---|

## 3. Files changed
| File | Change | Purpose |
|---|---|---|

## 4. Technical decisions and trade-offs
<Decisions, rationale, rejected options when evidenced, and impact>

## 5. Verification results
| Check | Command / source | Status | Actual result |
|---|---|---|---|
<Include test totals, failing assertions, zero discovery, and unrun checks explicitly>

## 6. Risks, limitations, and follow-up actions
<Impact, unresolved items, blockers, and next step>

## 7. Interview questions
<3–5 questions, brief answer pointers, and concrete code/test references>

## 8. References
<Story, ADRs, relevant tests/reviews and linked interview artifact, if available>
```

## Non-negotiable rules
- Follow `AGENTS.md`. Do not modify acceptance criteria without approval, change tests to force a pass, add unapproved dependencies, or implement beyond the story.
- Do not invent files, command output, test counts, architecture decisions, CI status, or successful deployment.
- Do not run destructive, deployment, push, merge or commit operations as part of reporting. If asked to run safe verification commands, report their exact observed results.
- Do not label an incomplete, failed, or unverified task as completed. Report partial evidence honestly.
- If no repository or execution evidence is available, deliver a clearly marked **provisional report**, listing what must be inspected before final sign-off.
- The report informs human review; it does not grant approval or authorize a PR merge.
