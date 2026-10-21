---
name: test-design
description: Design traceable, risk-based test scenarios for approved ProdeChamp user stories before TDD implementation. Use when QA needs to translate acceptance criteria into verifiable cases and identify gaps, boundaries, and integration risks without changing requirements or implementing production code.
---

# Test Design — ProdeChamp

## Purpose and authority

Act as the QA test designer for ProdeChamp (.NET 10, C#, SQL Server, Angular). Produce an executable-ready **test design**, not speculative product requirements or test results.

1. Read the repository `AGENTS.md` first; its constraints take precedence over this skill.
2. Read the target story and its **approved** acceptance criteria in `docs/user-stories/`, relevant `docs/product/`, `docs/architecture/README.md`, and applicable ADRs in `docs/architecture/decisions/`.
3. Inspect existing code, test-project conventions, and relevant endpoints/contracts only when available. Do not assume that planned integrations exist.
4. Use `docs/` as the source of truth. If documents conflict, report the conflict; do not silently choose a new business rule.

## Scope and guardrails

- Map tests only to the approved story and its acceptance criteria. Preserve their wording, IDs, and intent.
- **Do not** change acceptance criteria, create scoring/competition rules, or expand scope without explicit approval.
- **Do not** implement production code, rewrite tests to pass, add packages, create commits, push to `main`, merge PRs, or claim approval.
- Do not require EF Core, MediatR, or other new dependencies without approval. Respect Clean Architecture and SQL Server's prohibition on `AddWithValue` when considering implementation constraints.
- Distinguish **approved facts**, **QA-derived test proposals**, **assumptions**, and **open questions**.
- If an unknown business rule materially changes expected behavior, mark affected tests **BLOCKED — business decision required** and ask a precise question. Continue designing unaffected tests.
- Do not invent numeric limits, timing SLAs, prediction deadlines, point calculations, tie breakers, roles, or competition behavior.

## Workflow

### 1. Establish test basis

- Identify story ID/title, approval status, acceptance criteria, dependencies, and relevant architectural constraints.
- Identify input/output contracts, domain invariants, authorization boundaries, persistence effects, and important state transitions **only where specified or evidenced**.
- Check whether each acceptance criterion is unambiguous, observable, and testable. Flag gaps for `story-refinement` rather than silently rewriting the story.

### 2. Build a coverage matrix

For **each** acceptance criterion, propose cases that are relevant to the approved behavior:

- Happy path and alternative valid paths.
- Invalid inputs and meaningful boundary values; derive exact values only from approved rules.
- Error responses, forbidden/unauthenticated access, and relevant role/ownership restrictions.
- State transitions, duplicates, idempotency, consistency, or concurrency **when relevant**.
- Persistence and external-boundary behavior (API, SQL Server, Angular) **when relevant**.
- Regression coverage for nearby existing behavior likely affected by the change.

Avoid mechanically multiplying test cases; favor high-value, independent checks. Assign stable IDs (for example `US-001-TC-01`) and link each to its acceptance criterion.

### 3. Choose appropriate test levels

- **Domain unit tests:** pure business rules, invariants, value objects; no database or web hosting.
- **Application unit tests:** orchestration/use cases with controlled fakes or mocks at boundaries.
- **Infrastructure integration tests:** real SQL Server behavior, data access, constraints, transactions; keep environment requirements explicit.
- **API integration tests:** HTTP contract, validation, authn/authz, DI, middleware, and persistence where needed.
- **Angular tests:** component/service behavior, reactive forms, error states, user-facing interactions where the story includes UI.
- **End-to-end tests:** only when a cross-layer user journey warrants their cost and suitable tooling already exists or is approved.

Follow actual repository project names and conventions rather than inventing test projects. Prefer deterministic, isolated tests with clear setup/teardown; avoid order dependence, live third-party services, fixed sleeps, or shared mutable data. If integration infrastructure is unavailable, record the precondition and a proposed validation path instead of reporting success.

### 4. Specify test cases

Each case must include:

- **ID and linked acceptance criterion**.
- **Level / target component** and **priority** (`P0` critical, `P1` important, `P2` supplementary), justified by risk.
- **Given / When / Then** or equivalent setup, action, and observable expected outcome.
- **Test data / fixtures** and boundary rationale; mark unknown values as TBD.
- **Dependencies / environment** (such as SQL Server) and whether the case is **Ready** or **Blocked**.
- **Suggested automation location** only if supported by current repository structure; otherwise label as proposed.

Where security-sensitive functionality is approved, test access denial and information exposure as applicable. For SQL-backed flows, consider parameter correctness, transaction boundaries, rollback, and concurrency only insofar as the approved functionality makes them relevant.

### 5. Prepare TDD handoff

- Order cases so `tdd-implementation` can tackle one externally observable behavior at a time with **Red → Green → Refactor**.
- Recommend the first meaningful failing test and the expected failure reason: *missing behavior*, not compilation, fixture, or infrastructure failure.
- Identify which tests should remain integration or E2E rather than forcing all tests into isolated unit tests.
- Provide a traceability summary showing every acceptance criterion covered, partially covered, or blocked.
- State what is **not tested** and why. Never claim test execution, passing status, coverage percentage, or readiness without evidence.

## Deliverable format

When a test-design artifact is requested, store it under `docs/test-design/` using the story ID (for example `docs/test-design/US-001-test-design.md`), unless the repository has an established convention. Do not overwrite approved source material.

Use this structure:

1. **Story reference and test basis** — paths and approval status.
2. **Scope / out of scope** — traceable to the approved story.
3. **Ambiguities and blockers** — exact questions and affected case IDs.
4. **Risk assessment** — high-impact failure modes and prioritization rationale.
5. **Acceptance-criteria coverage matrix** — `AC ID | case IDs | level | coverage (covered/partial/blocked)`.
6. **Detailed test cases** — required case fields from step 4.
7. **TDD implementation order** — first RED scenario and subsequent increments.
8. **Environment / test data requirements** — isolation, fixtures, credentials/secrets handling.
9. **Uncovered risks and assumptions** — clearly labeled.
10. **Handoff summary** — cases Ready vs Blocked; no implied product sign-off.

If this skill only designs tests, explicitly report **tests executed: none (design only)**. If commands were actually run to inspect/discover tests, record each command and its real outcome, distinguishing failed, skipped, blocked, and zero-test discovery.

## Definition of done for test design

The document is ready for human review when approved criteria are traceable to proposed tests, expected outcomes are observable, blockers are visible, and the TDD handoff is actionable. Only an authorized human or established repository process can approve scope changes or move the story to implementation readiness.
