---
name: tdd-implementation
description: Implement approved ProdeChamp behavior using a failing-test-first workflow; apply when a user story is ready for coding.
---

Read repository `AGENTS.md`, the approved story and acceptance criteria in `docs/user-stories/`, and relevant architecture documentation.

Produce a scoped implementation plan. Resolve ambiguous business rules before implementing affected behavior. Select UnitTests for isolated Domain/Application behavior and IntegrationTests for API/Infrastructure boundaries.

Write a meaningful test for the approved behavior. Run it and verify that it fails because the behavior is missing, rather than from setup or compilation errors. Implement the smallest maintainable change, rerun the test, and refactor while preserving behavior. Record actual commands and outcomes.

Keep dependency directions intact. Introduce neither speculative abstractions nor unapproved packages. Do not weaken tests or acceptance criteria to obtain a passing result. For structure-only tasks without behavior, report that behavioral TDD is not applicable instead of inventing tests.

Synchronize relevant docs and provide the engineering output required by `AGENTS.md`.
