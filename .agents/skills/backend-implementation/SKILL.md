---
name: backend-implementation
description: Implement approved ProdeChamp backend user stories in C#/.NET 10 and SQL Server, following Clean Architecture and the repository's test-first engineering workflow. Use for changes to Domain, Application, Infrastructure, API, and their backend tests; not for frontend features or unapproved architecture changes.
---

# Backend Implementation — ProdeChamp

Read [AGENTS.md](../../../AGENTS.md) and the [shared factory contract](../../../docs/development-factory/README.md) for authoritative story lookup, stage prerequisites, artifact paths, verification evidence and the common handoff. Reuse approvals already granted for the same scope.

## Authority and prerequisites

1. Retrieve the approved GitHub Issue and separately approved decisions, then read relevant product/architecture docs and ADRs.
2. Inspect the actual solution, project references, existing code, test projects, conventions, and the human-approved implementation plan and QA test design required for story implementation. Explicitly scoped maintenance follows the shared contract's task exception. Do not assume a class, pattern, endpoint, database table, or dependency exists.
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

## Backend specialization of TDD

Read and follow [tdd-implementation](../tdd-implementation/SKILL.md); it owns RED, GREEN, refactoring, correction evidence and structural-change exceptions. Run one implementation pass using this specialization, not a second TDD workflow.

- Inspect the actual xUnit package and runner before using fixtures, async lifecycle APIs or filters. The current projects use xUnit 2.9.3 with VSTest; the VS adapter's 3.x version does not imply xUnit v3. Recheck project configuration instead of upgrading it implicitly.
- Use Domain/Application unit tests for isolated behavior, SQL Server integration tests for persistence and API integration tests for HTTP/auth contracts.
- For approved SQL work, identify the isolated database, fixture lifecycle, schema prerequisites, cleanup and parallelism policy. Missing infrastructure is a blocker for that boundary, not a reason to substitute mocks or claim integration success.
- Preserve legitimate tests, explicit SQL parameter typing and the approved contract. Reconcile saved source with the build/test evidence before handoff.

## Security and API checks

- Enforce authentication and authorization at the correct boundary for the approved endpoint, not solely in Angular.
- Validate untrusted inputs and use safe error handling: do not expose stack traces, secrets, sensitive data, or raw SQL errors in responses/logs.
- Keep API route, method, status-code, and payload decisions aligned with approved contracts and current repository conventions. Flag contract-breaking changes before implementation.
- Do not add authentication methods, roles, permissions, rate limits, or business rules that were not approved; instead raise an explicit review question when necessary.

## Final verification and handoff

- Inspect the final diff for accidental scope expansion, architecture violations, unauthorized dependencies, secret leakage, and changed API contracts.
- Update relevant `docs/` documentation only for authorized, actually implemented changes; do not retrospectively edit approved requirements to match code.
- Use the shared handoff and link the single implementation evidence artifact. Identify affected layers, criterion coverage, actual RED/GREEN/regression results, migrations/API impacts and remaining blockers; `engineering-report` owns the final report.
- Hand off for independent `code-review`, `engineering-report`, PR preparation and CI verification. Do not self-approve or claim external checks, deployment, commits, or merges without evidence.
