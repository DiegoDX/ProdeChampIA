---
name: frontend-implementation
description: Implement approved ProdeChamp Angular frontend behavior with test-first development, accessible UI, typed API contracts, and verifiable results. Use when a ready user story requires frontend code changes.
---

# Frontend Implementation — ProdeChamp

## Purpose and authority

Implement only frontend behavior authorized by an approved user story. This skill specializes the shared `tdd-implementation` workflow for Angular and TypeScript; it does not replace `AGENTS.md`, approved acceptance criteria, architectural decisions, or the API contract. When instructions conflict, follow the repository's governing documents and stop on unresolved business rules.

## Read before editing

1. Read repository `AGENTS.md`, the approved story and acceptance criteria under `docs/user-stories/`, relevant `docs/product/` and `docs/architecture/` records, and any available `docs/test-design/` plan.
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

## TDD: Red → Green → Refactor

1. Map each relevant acceptance criterion to observable UI behavior and a test level, using the approved QA design when present.
2. **RED:** write a focused automated test first, run it, and confirm it fails because the approved behavior is missing—not because of a broken setup, compilation error or wrong expectation. Record the actual command and failure.
3. **GREEN:** implement the smallest maintainable Angular change that satisfies the test and criterion. Rerun the focused test and record the result.
4. **REFACTOR:** simplify structure and naming without changing behavior; rerun the focused tests.
5. Repeat per behavior; then run relevant broader checks supported by the repository (unit/component tests, lint, type-check/build, and applicable E2E tests). Report each command and actual outcome.
6. Use HTTP mocking for isolated frontend tests, asserting request method, URL, payload and handling; use real integration/E2E checks only when environments and contracts are available. Do not claim mocked tests verify a live backend.
7. Cover meaningful positive, negative and boundary scenarios, including validation and async error handling when required; avoid tests that only assert components instantiate or duplicate framework behavior.
8. Never weaken tests, alter approved acceptance criteria, or update snapshots blindly just to obtain a green result. If behavioral TDD is inapplicable to a structure-only change, explain why and use suitable static/build verification instead.

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
7. **Interview questions:** 2–5 grounded questions related to the actual decisions made, as required by `AGENTS.md`.

Keep relevant documentation in sync with authorized changes. Never present unverified behavior as completed.
