---
name: tdd-implementation
description: Implement an approved ProdeChamp user story with evidence-based Red–Green–Refactor TDD in .NET 10, SQL Server or Angular. Use only when coding an approved story or explicitly requested scoped change; do not use for story approval or architecture decisions.
---

# TDD Implementation — ProdeChamp

## Purpose and authority

Implement **only approved behavior** through short, observable Red → Green → Refactor cycles. This is an **implementation skill**, not authorization to refine business rules, approve architectural changes, commit, push, or merge.

1. Read repository `AGENTS.md` first. Its rules prevail if this skill conflicts with them.
2. Read the approved story and acceptance criteria in `docs/user-stories/`, relevant `docs/product/` rules, `docs/architecture/README.md`, and applicable decisions in `docs/architecture/decisions/`.
3. Inspect existing code, tests, solution/project references, build/test commands, and conventions before proposing changes. Do not assume a test framework, directory name, or mocking library exists.
4. If story approval, acceptance criteria, required business rules, or essential source material cannot be verified, **stop only the affected implementation**. Report the exact blocker; do not infer scoring, competition, prediction deadlines, tie-breaking, or authorization rules.

## Workflow

### 1. Define scope and plan

- Identify story ID, approved acceptance-criteria IDs, intended observable outcomes, in-scope layers, and explicitly excluded behavior.
- Produce a short implementation plan mapping **each relevant acceptance criterion → test(s) → likely code location**. Note dependencies, database/environment requirements, and risks.
- Choose the **lowest useful test boundary**: Domain/Application unit tests for isolated rules; integration tests for SQL Server repositories, persistence and external boundaries; API tests for HTTP/authorization/contracts; Angular component/service tests for frontend behavior when in scope. Add end-to-end tests only if justified by approved behavior and existing tooling.
- Prefer public behavior and realistic boundary conditions over implementation-detail assertions. Test important failure and edge paths, not merely the happy path.
- If a new package, abstraction, architecture change, or requirement is necessary, **request approval before adding it**. In particular, do not introduce EF Core, MediatR, or other unapproved dependencies.

### 2. RED — demonstrate a meaningful failure

- Write the smallest focused automated test for **one approved behavior** before changing production code.
- Execute the test using the repository's actual test command. Record command, test name, and relevant output.
- Confirm it fails because the expected behavior is absent or incorrect, with a clear assertion or expected behavioral failure. A missing dependency, broken environment, unrelated failing test, or generic compilation/setup error is **not validated RED**.
- If test execution is blocked, distinguish `BLOCKED` from `RED`; fix only authorized environment/test-harness problems, or report the blocker. Never claim a verified RED when one was not observed.

### 3. GREEN — implement the minimum behavior

- Change only the production code needed to satisfy the failing test and approved acceptance criterion.
- Respect pragmatic Clean Architecture: Domain has no outward layer dependency; Application depends inward; Infrastructure implements external concerns; API is a composition/transport boundary. Keep tests outside the production dependency graph.
- Use dependency injection and async/await for asynchronous I/O where applicable. For SQL Server, parameterize queries with explicit appropriate types/sizes; **never use `AddWithValue`**. Preserve transactional consistency and resource disposal where relevant.
- Run the focused test again. It must pass for the correct reason; if it does not, do not declare GREEN.

### 4. REFACTOR — preserve behavior

- Improve clarity, duplication, naming, or design **only within the approved scope**. Avoid speculative abstractions, patterns, and unrelated cleanup.
- Rerun focused tests after refactoring. Repeat RED → GREEN → REFACTOR for the next behavior until the approved criteria are covered.
- Never delete, weaken, skip, or rewrite a test merely to make production code pass; change a test only to correct an objectively invalid test and document why, without changing approved expected behavior.

### 5. Verify and review

- Run affected unit, integration, API, and frontend tests supported by the repository, then an appropriate broader regression suite when feasible. Record actual commands, counts/outcomes, and reasons for skips.
- If integration tests require SQL Server, configure an isolated test database/fixture where available; **never run destructive tests against production**. Do not substitute mocks for a repository/database integration requirement.
- Review acceptance-criteria coverage, authentication/authorization impacts, input validation, relevant concurrency/date/time boundaries, architecture references, secrets, logging, migrations and backward compatibility as applicable.
- Synchronize relevant `docs/` documentation with **implemented and approved** behavior; do not convert proposals into accepted decisions or alter acceptance criteria without explicit approval.
- Do not claim any test, review, or verification ran unless it actually ran. Report unavailable tooling and unverified risks honestly.

## Special cases and guardrails

- **Structure-only changes:** If no observable behavior exists, state why behavioral TDD is not applicable; use appropriate build, structural, or architecture verification rather than inventing a fake failing test.
- **Legacy code:** First characterize existing behavior with focused tests when required to safely change it; distinguish characterization from new acceptance tests.
- **Ambiguity:** Ask for a targeted business decision and avoid implementing the dependent behavior until resolved. Unrelated, independently approved work may proceed.
- **Authority:** Do not modify acceptance criteria, invent product rules, introduce unapproved packages, commit secrets, push directly to `main`, merge a PR, or claim autonomous approval. Do not commit unless separately authorized.

## Required engineering report

Return a compact, evidence-based report:

1. **Story and scope:** story ID, acceptance criteria addressed, deferred/blocked items.
2. **Plan and traceability:** criterion → test(s) → implementation files.
3. **TDD evidence:** for each meaningful cycle, test name, exact RED command and observed failure, GREEN command/result, refactor verification. Mark `NOT RUN` or `BLOCKED` explicitly if applicable.
4. **Files changed:** each file and why; documentation updated.
5. **Technical decisions:** key choices, tradeoffs, dependencies or approvals needed.
6. **Verification:** exact test/build commands, actual outcomes, failures/skips and regression coverage.
7. **Limitations and risks:** unresolved questions, environment gaps, uncovered scenarios.
8. **Interview questions:** 2–4 relevant senior engineering questions arising from the actual change, with brief expected-answer themes.

Treat passing tests as evidence for tested behavior, **not** proof that all requirements, production conditions, or architecture concerns are satisfied.
