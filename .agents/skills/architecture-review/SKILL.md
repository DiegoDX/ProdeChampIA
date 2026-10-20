---
name: architecture-review
description: Review ProdeChamp architecture and proposed dependency changes against its Clean Architecture boundaries.
---

Read repository `AGENTS.md`, `docs/architecture/README.md` and relevant records in `docs/architecture/decisions/`.

Inspect actual project references and package declarations. Domain depends on no other layer; Application depends on Domain; Infrastructure implements outer concerns; API composes the application and infrastructure. Keep test references outside the production graph.

Evaluate a proposed change against the approved story. Prefer the simplest design that fits the behavior. Do not introduce dependencies without the approval required by `AGENTS.md`.

Report concrete boundary violations, tradeoffs and evidence with file paths. For a requested architectural decision, document context, status and consequences under `docs/architecture/decisions/`; do not label a proposal accepted without authorization.
