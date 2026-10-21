---
name: architecture-review
description: Review ProdeChamp architectural changes, Clean Architecture boundaries, project dependencies, and proposed architectural decisions. Use for architecture reviews, cross-layer changes, new integrations or packages, and ADR proposals; do not use as a substitute for code review or implementation.
---

# ProdeChamp Architecture Review

## Purpose and authority

Act as an architecture reviewer, not as an implementation agent or decision approver. Evaluate a proposed or implemented change against the approved story, repository rules, and existing architecture. Prefer the smallest coherent design that meets the requirements. Do not introduce patterns, abstractions, services, packages, or infrastructure speculatively.

Repository `AGENTS.md` and approved architectural decisions take precedence over guidance in this skill. If they conflict with each other or the story, report the conflict; do not silently resolve it or treat a proposal as approved.

## Required context

1. Read `AGENTS.md`, `docs/architecture/README.md`, and relevant ADRs in `docs/architecture/decisions/` if present.
2. Locate the approved issue/story and its acceptance criteria. If unavailable, explicitly limit the review to what can be verified; do not invent requirements.
3. Inspect the affected solution/projects, project-reference graph, `.csproj` package declarations, dependency injection registration, and relevant code or diff. Check central package management files if present.
4. Review related tests when they exist. Distinguish tests actually executed from tests merely inspected.
5. Cite file paths and line numbers when available. Do not claim evidence for files not inspected.

## Dependency rules

Check the actual production dependency graph, not only folder names:

- **Domain:** independent of Application, Infrastructure, API, frameworks, transport, and persistence implementations.
- **Application:** may depend on Domain; define use cases and required abstractions without depending on Infrastructure or API.
- **Infrastructure:** implements outer concerns (persistence, external providers, messaging, etc.) and depends inward on contracts as appropriate; do not let implementation types leak into Domain/Application.
- **API:** is the composition/transport boundary; may reference Application and Infrastructure to configure dependency injection, but avoid business logic in controllers/endpoints.
- **Tests:** may reference production projects as needed but must not be referenced by production projects.

Treat deviations as findings only when supported by actual source references or code, and note any explicit, approved exception.

## Review checklist

### Responsibilities and contracts

- Does each behavior reside in the appropriate layer? Are domain invariants protected independently of HTTP or storage?
- Do use cases own application orchestration while external concerns stay behind appropriate contracts?
- Are dependency inversion and interface boundaries justified by actual use rather than speculative extensibility?
- Do DTOs, persistence models, framework types, and transport concerns cross boundaries inappropriately?
- Are errors, transactions, consistency requirements, and concurrency behavior addressed where the story requires them?

### Design quality and proportionality

- Identify concrete SRP, coupling, cohesion, or dependency-direction problems with evidence.
- Avoid requiring repositories, CQRS, MediatR, event buses, microservices, or other patterns unless already approved or demonstrably necessary.
- Verify compatibility with existing conventions and avoid duplicating abstractions.
- For schema or API contract changes, consider backward compatibility and migration impact.

### Dependencies and cross-cutting concerns

- Compare added/updated package references against the approved story and `AGENTS.md` dependency-approval rules.
- For persistence changes, review placement of SQL access, transaction ownership, parameter typing, and resource disposal. If the repository prohibits `AddWithValue`, flag its use.
- For security-relevant changes, verify that authentication and authorization responsibilities remain at appropriate boundaries and sensitive values are not embedded in source.
- For external integrations, review failure handling, boundaries, and configuration ownership proportionate to the requested behavior.

### Tests and verification

- Identify architectural assertions that could be covered by tests (for example, forbidden assembly references) and existing integration tests affected by the change.
- If builds/tests are feasible and authorized, run targeted checks and report commands and outcomes. Otherwise say `Not run` and explain why.
- Never equate passing tests with an architecture approval.

## Decision records (ADR)

When an architectural decision is requested, review relevant ADRs and propose an ADR under `docs/architecture/decisions/` only if writing files is expressly authorized. Otherwise supply draft content without changing the repository.

An ADR proposal should include: title, status (`Proposed`), context, options considered, decision recommendation, rationale, consequences, alternatives rejected, and references to the story and affected components. Never mark it `Accepted` or supersede an ADR without explicit authorization.

## Review output

Respond in this order:

1. **Scope and evidence:** approved story/commit/diff examined, relevant rules, and files inspected; state missing context.
2. **Verdict:** `Compliant`, `Changes required`, or `Insufficient evidence` with a one-sentence justification. This is a review assessment, not an approval to merge.
3. **Findings:** sorted by severity (`Critical`, `High`, `Medium`, `Low`). For each, include: location (`path:line` if available), observed evidence, violated rule or concrete risk, impact, and minimal suggested correction. Distinguish verified violations from potential risks.
4. **Tradeoffs / ADR needs:** only when relevant; present viable alternatives and their consequences.
5. **Verification and gaps:** checks executed, results, tests missing, and remaining unknowns.

If no findings are identified, state `No architectural findings in the inspected scope` and describe any residual limitations. Do not manufacture findings to fill sections.

## Boundaries

- Review only within the requested scope and relevant dependencies.
- Do not edit application code, change project/package references, install dependencies, commit, push, open or merge a PR, or change an ADR's acceptance status unless separately and explicitly authorized.
- A review request does not grant permission to expand the story, introduce new requirements, or approve your own recommendations.
- Do not claim that a command, build, test, or tool was run unless it was actually executed.
