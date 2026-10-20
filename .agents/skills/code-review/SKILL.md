---
name: code-review
description: Review a ProdeChamp change for correctness, scope, tests and architecture when a review is requested.
---

Read repository `AGENTS.md`, the approved story and relevant architecture documentation. Inspect the diff and surrounding code before drawing conclusions.

Prioritize correctness, regression risks, acceptance-criteria coverage and layer violations. Check that dependency changes are authorized and that secrets are absent. Where SQL exists, check parameter typing and the prohibition on `AddWithValue`.

Report actionable findings with severity, file location, concrete impact and a suggested correction. Distinguish verified failures from concerns. If no findings are identified, say so and describe testing gaps or residual limitations. Do not claim tests ran unless you ran them or cite an actual recorded run.

A review request alone does not authorize changing requirements, committing, pushing or merging.
