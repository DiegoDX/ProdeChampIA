---
name: backend-implementation
description: Implement approved ProdeChamp backend user stories in C#/.NET 10 and SQL Server, following Clean Architecture and the repository's test-first engineering workflow. Use for changes to Domain, Application, Infrastructure, API, and their backend tests; not for frontend features or unapproved architecture changes.
---

# Backend Implementation — ProdeChamp

## Authority and prerequisites

1. Read repository `AGENTS.md`, the approved story and its acceptance criteria in `docs/user-stories/`, and relevant `docs/product/` and `docs/architecture/` documentation, including applicable ADRs.
2. Inspect the actual solution, project references, existing code, test projects, conventions, and the approved implementation plan and QA test design if available. Do not assume a class, pattern, endpoint, database table, or dependency exists.
3. Treat `docs/` as the source of truth for product and architecture decisions. Follow `AGENTS.md` if instructions conflict.
4. Implement only approved behavior. If a business rule, authorization policy, scoring rule, contract, or schema change is ambiguous, state the specific blocker and request a decision before implementing that affected part; continue with independent approved work when safe.
5. Do not change acceptance criteria, expand scope, add packages (including EF Core or MediatR), or create/accept ADRs without required approval. Do not push to `main`, merge PRs, or commit secrets.

## Plan before editing

- Map each acceptance criterion to affected backend layer(s), anticipated production changes, and test cases. Identify data migration, API compatibility, security, and transaction implications.
- Prefer the smallest maintainable change consistent with existing patterns. Avoid speculative abstractions, premature generalization, and unrelated refactoring.
- If an approved contract is shared with Angular, preserve it or explicitly flag breaking changes for coordination. Do not implement frontend code under this skill.

## Clean Architecture boundaries

- **Domain:** entities, value objects, invariants, and pure business behavior; no dependency on Application, Infrastructure, API, web frameworks, or persistence details.
- **Application:** use cases, orchestration, interfaces/ports, DTOs where existing conventions place them, and application validation; depend inward on Domain, never on Infrastructure or API.
- **Infrastructure:** implement persistence and external integrations behind the contracts defined inward; contain SQL Server details, configuration, and infrastructure failures.
- **API:** HTTP endpoints, authentication/authorization integration, request binding, validation at the boundary, response mapping, and dependency injection composition; keep business decisions out of controllers.
- Inspect actual `.csproj` references when changing project boundaries. Keep test dependencies outside the production dependency graph.
- Use dependency injection and `async`/`await` for asynchronous I/O. Respect `CancellationToken` propagation where applicable; do not introduce needless async wrappers.

## SQL Server rules

- Follow the repository's approved data-access approach; do not introduce EF Core, Dapper, or another persistence package without approval.
- Use parameterized commands with explicit SQL types (and size/precision/scale where applicable). **Never use `AddWithValue`.** Never concatenate untrusted values into SQL.
- Handle SQL connection and command lifetime safely; use async APIs where supported. Keep queries in the Infrastructure layer.
- Use transactions for approved multi-step operations that require atomicity. Consider uniqueness, concurrency, and idempotency only where the story or existing invariants require them; do not invent product policy.
- Treat schema changes and data migrations as explicit deliverables requiring approved scope and an appropriate verification/rollback approach. Never alter production data or run destructive scripts without authorization.

## TDD execution (coordinate with `$tdd-implementation`)

For each approved observable behavior, follow **Red → Green → Refactor**:

1. Select the narrowest meaningful test: Domain/Application unit tests for isolated logic; infrastructure integration tests for SQL behavior; API integration tests for HTTP/auth/boundary behavior.
2. Write a test that demonstrates an approved criterion. Run it and verify a genuine behavior-related failure (**RED**), not compilation, discovery, setup, or unavailable-environment failure.
3. Implement the smallest correct change (**GREEN**), rerun the targeted test, then refactor without changing behavior and rerun relevant tests.
4. Add negative, boundary, and permission scenarios only when grounded in approved requirements or existing invariants. Avoid mocking away the behavior being verified.
5. Run the relevant regression suite if the environment supports it. Record exact commands, actual outcomes, and any tests skipped or blocked. A command returning success with **zero tests discovered** is not proof of passing tests.
6. For structure-only work with no testable behavior, explain why behavioral TDD is inapplicable; use appropriate build or structural checks instead. Never manufacture a RED step.

Never weaken, delete, or rewrite a legitimate test simply to make a build pass. Never report a test as passing unless it actually ran and passed.

## Security and API checks

- Enforce authentication and authorization at the correct boundary for the approved endpoint, not solely in Angular.
- Validate untrusted inputs and use safe error handling: do not expose stack traces, secrets, sensitive data, or raw SQL errors in responses/logs.
- Keep API route, method, status-code, and payload decisions aligned with approved contracts and current repository conventions. Flag contract-breaking changes before implementation.
- Do not add authentication methods, roles, permissions, rate limits, or business rules that were not approved; instead raise an explicit review question when necessary.

## Final verification and handoff

- Inspect the final diff for accidental scope expansion, architecture violations, unauthorized dependencies, secret leakage, and changed API contracts.
- Update relevant `docs/` documentation only for authorized, actually implemented changes; do not retrospectively edit approved requirements to match code.
- Provide an engineering handoff aligned with `AGENTS.md`:
  1. Story/acceptance criteria addressed, with any unresolved criteria.
  2. Files changed and purpose, grouped by Domain/Application/Infrastructure/API/Tests/Docs.
  3. Technical decisions, alternatives considered where material, and any approval needed.
  4. TDD evidence: exact RED and GREEN commands/results; final build/test commands with **Passed / Failed / Blocked / Not Run / Zero Discovered** distinguished.
  5. Known limitations, environmental blockers, migrations/API impacts, and residual risks.
  6. Suggested senior .NET interview questions grounded in the actual implementation.
- Hand off for independent `code-review`, `engineering-report`, PR preparation and CI verification. Do not self-approve or claim external checks, deployment, commits, or merges without evidence.
