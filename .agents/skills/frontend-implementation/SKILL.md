---
name: frontend-implementation
description: Implement approved ProdeChamp Angular frontend behavior with test-first development, accessible UI, typed API contracts, and verifiable results. Use when a ready user story requires frontend code changes.
---

# Frontend Implementation — ProdeChamp

Read [AGENTS.md](../../../AGENTS.md) and the [shared factory contract](../../../docs/development-factory/README.md) for authoritative story lookup, stage prerequisites, artifact paths, verification evidence and the common handoff. Reuse approvals already granted for the same scope.

## Purpose and authority

Implement only frontend behavior authorized by an approved user story. This skill specializes the shared `tdd-implementation` workflow for Angular and TypeScript; it does not replace `AGENTS.md`, approved acceptance criteria, architectural decisions, or the API contract. When instructions conflict, follow the repository's governing documents and stop on unresolved business rules.

## Read before editing

1. Retrieve the approved GitHub Issue and separately approved decisions; read relevant product/architecture records. Verify the human-approved implementation plan and QA design for story work. Explicitly scoped maintenance follows the shared contract's task exception.
2. Inspect the actual Angular version, workspace configuration, existing feature structure, coding conventions, test runner, formatting/lint commands, and related frontend code. Do not assume dependencies or scripts exist.
3. Inspect the actual backend endpoints, DTOs, authentication conventions, validation errors and API documentation relevant to the story. Do not invent endpoints or response fields.
4. Check whether the story is ready. Identify missing product decisions, API contract gaps and external blockers before implementing affected behavior.

## Scope and plan

- State the acceptance criteria covered, components/routes/services impacted, API dependencies, test strategy, and any risks.
- Prefer a small vertical slice: user interaction → UI state → API integration → resulting feedback. Keep unrelated refactoring out of scope.
- Keep feature-specific code within the existing project structure; reuse shared components/utilities only when reuse is justified.
- Do not introduce new npm packages, Angular libraries, state-management frameworks, or build tooling without required approval.
- If backend contract changes are required, describe the change and request approval/coordination; do not silently modify backend behavior or fabricate a mock contract as if implemented.

## Frontend engineering standards

- Follow the Angular patterns established by the repository, preferring standalone components, dependency injection, strict TypeScript types and reactive forms where applicable. Use signals or RxJS based on existing conventions and the nature of the state; avoid redundant sources of truth.
- Separate presentation, feature orchestration and HTTP access where complexity justifies it. Components must not duplicate API transport or business rules already owned by the backend.
- Define typed request/response interfaces consistent with actual API payloads. Handle loading, success, empty, validation-error, unauthorized/forbidden and network-failure states as applicable to approved behavior.
- Prevent duplicate submissions where relevant, clean up subscriptions with Angular-supported lifecycle patterns, and avoid unnecessary manual subscriptions.
- Use route guards/interceptors only for their proper concerns; server-side authorization remains mandatory. Do not treat frontend visibility checks as security enforcement.
- Preserve accessible labels, keyboard usability, focus feedback, semantic markup and readable error messages. Respect existing responsive design and localization conventions.
- Avoid unsafe HTML injection, logging secrets, embedding credentials, exposing sensitive data, or storing tokens in a new location without an approved security design.
- Do not optimize speculatively; identify measured performance problems separately.

## Frontend specialization of TDD

Read and follow [tdd-implementation](../tdd-implementation/SKILL.md); it owns the RED/GREEN/refactor cycle, evidence and structural-change exceptions. Apply the approved QA cases to UI behavior without repeating the implementation workflow.

- Use the Angular version and runner actually configured. Do not introduce newer forms, signal APIs, a different test runner or E2E packages implicitly.
- Use HTTP mocking for isolated tests of request method, URL, payload and error handling; it does not verify a live backend.
- Test meaningful interactions, validation and async states, accessibility and authorization feedback where relevant; avoid component-instantiation-only assertions and blind snapshot updates.
- Run the relevant configured component/service tests, lint, build and justified integration/E2E checks. Link the single implementation evidence artifact and reconcile saved source with those results.

## Boundaries and stop conditions

- Do not edit backend, database schemas, product requirements or architectural decisions unless separately authorized.
- Stop implementation of affected behavior if scoring, prediction, league, deadline/time-zone or authorization rules are ambiguous; record a precise question rather than inventing a rule.
- Preserve the approved API contract. If the backend is unavailable, distinguish a locally tested UI from end-to-end verified behavior.
- Do not commit secrets, push directly to `main`, merge PRs, or claim approvals, deployments or passing CI without evidence.

## Required handoff

Return a concise engineering handoff suitable for `engineering-report` and `code-review`:

1. **Scope and acceptance criteria:** what was implemented, omitted or blocked; link criterion IDs where available.
2. **Files changed:** path and purpose for each relevant file.
3. **Technical decisions:** Angular patterns selected and any nontrivial trade-offs; note whether API contracts were confirmed.
4. **TDD evidence:** RED/GREEN/REFACTOR observations, exact commands, actual outcomes, and whether any test discovered zero tests.
5. **Other verification:** lint, build, integration/E2E results, distinguishing Passed / Failed / Not run / Blocked.
6. **Limitations and risks:** outstanding API, UX, accessibility, security or environment issues.

Keep relevant documentation in sync with authorized changes. Never present unverified behavior as completed.
