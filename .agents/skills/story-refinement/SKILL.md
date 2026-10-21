---
name: story-refinement
description: Refine and validate ProdeChamp user stories before implementation; expose business-rule ambiguities, acceptance-criteria gaps, dependencies, and test scenarios without silently changing approved scope. Use when a story is being drafted, reviewed, or prepared for implementation.
---

# Story Refinement — ProdeChamp

## Purpose and authority

Refine a user story into a clear, testable proposal that an architect, QA agent, and developer can use. This skill is **analysis and planning only**, not implementation or approval.

1. Read repository `AGENTS.md` first and follow its instructions. If instructions conflict, `AGENTS.md` takes precedence.
2. Read the target story and its current acceptance criteria in `docs/user-stories/`.
3. Read relevant product rules in `docs/product/`, architecture in `docs/architecture/README.md`, and any applicable decisions in `docs/architecture/decisions/`. Read only what is relevant; `docs/` is the source of truth for documented product and architecture decisions.
4. Inspect directly related stories or contracts only where needed to identify prerequisites, conflicts, or overlap.
5. If required source material is missing or inaccessible, say what is missing and mark conclusions as provisional. Do not invent its contents.

## Refinement process

### 1. Establish the baseline

- Identify story ID/title, actor, business objective, approved scope, existing acceptance criteria, and known dependencies.
- Distinguish **approved/documented facts**, **proposed improvements**, **assumptions**, and **open questions**; cite repository paths or story/criterion IDs for facts when available.
- Detect contradictions between the target story and documented product decisions; report them rather than silently choosing a rule.
- Preserve the existing acceptance criteria verbatim when quoting or reproducing the baseline. Never silently rewrite, remove, or add approved acceptance criteria.

### 2. Validate business behavior

- Check who may perform each action, required preconditions, success/failure outcomes, permissions, and observable behavior.
- Identify meaningful boundary cases, state transitions, validation, duplicate actions, concurrency risks, and relevant time/date rules.
- For football predictions, inspect applicable documented rules only: competition/league membership, match status, prediction cutoffs, timezone, scoring, ranking, ties, and result corrections. **Do not assume these rules exist or invent their values.**
- Identify what is explicitly out of scope. Avoid speculative features and gold-plating.
- Raise targeted questions when missing business rules would change behavior, data meaning, or expected results. State what decision is blocked by each question.

### 3. Assess acceptance criteria

- Evaluate each existing criterion for clarity, observability, unambiguous pass/fail behavior, atomicity where practical, and consistency with scope.
- Suggest missing or clearer criteria as **PROPOSED — NOT APPROVED**. Use Given/When/Then only when it improves clarity; avoid reformatting approved criteria as though they changed.
- Include positive, negative, authorization, and boundary scenarios where relevant, without multiplying scenarios that add no coverage.
- Map proposed test scenarios to existing acceptance-criterion IDs or to explicitly labeled proposed criteria. Never imply tests were executed.

### 4. Identify dependencies and delivery risks

- Note dependencies on other stories, existing contracts, data, integrations, architecture decisions, or decisions from the product owner.
- Flag material implications for Clean Architecture boundaries and the C#/.NET 10, SQL Server, and Angular stack, without designing or implementing a solution prematurely.
- Flag any likely new NuGet/npm dependency for approval under `AGENTS.md`; do not recommend EF Core, MediatR, or another dependency as an approved decision.
- Separate **blocking questions** from **non-blocking suggestions**. Do not declare a story ready if a blocking business rule remains unresolved.

## Approval and scope guardrails

- **Never modify approved acceptance criteria without explicit user authorization.** Show a proposed revision separately, with the reason and expected impact.
- Never invent scoring formulas, ranking/tie-break rules, deadlines, match-status semantics, league restrictions, or authorization policy.
- Do not add requirements, features, abstractions, dependencies, or technical tasks beyond approved story scope.
- Do not edit source code, tests, migrations, architecture decision records, or story files unless the user explicitly asks for that separate action; even then, preserve unapproved material and clearly label proposals.
- Do not implement, commit, push, merge a PR, or mark a story approved/Ready on the user's behalf.
- If approval is required, request it explicitly rather than treating the refinement as authorization.

## Required response format

Return a concise, actionable refinement report with these sections:

1. **Story baseline:** ID/title, goal, approved scope, current acceptance criteria (referenced by ID or faithfully quoted), and consulted files.
2. **Assessment:** what is already clear; issues or gaps, each with severity (**Blocker / Important / Suggestion**), evidence, and impact.
3. **Open questions:** precise business questions, why each matters, and whether it blocks implementation.
4. **Proposed refinement — NOT APPROVED:** improved user-story wording and any suggested acceptance criteria, clearly separated from the approved baseline. Do not fabricate answers to open questions.
5. **Verification outline:** test scenarios mapped to approved or proposed criteria; expected outcomes only when documented or explicitly hypothetical.
6. **Dependencies and scope:** prerequisites, risks, assumptions, and explicit out-of-scope items.
7. **Readiness:** **Ready for approval**, **Blocked pending decisions**, or **Needs further refinement**, with specific reasons. "Ready for approval" does not mean approved for implementation.
8. **Engineering handoff:** files changed (**None** unless explicitly changed), technical decisions (**None approved** unless evidenced), tests executed (**Not run — refinement only**), known limitations, and 1–3 relevant interview questions, as required by `AGENTS.md`.

Keep findings tied to concrete story criteria or repository evidence. Prefer the smallest refinement that makes the work testable and prevents rework. Do not claim to have inspected files, run tests, or received approvals that you have not actually verified.
