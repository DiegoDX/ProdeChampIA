# Product documentation

ProdeChamp is a football prediction platform. Product behavior, scoring, competitions and permissions require approved user stories before implementation.

Store approved product scope and business rules here. Sprint 0 defines no business rules.

## PRODE-001 — current scope

The [approved Issue #1](https://github.com/DiegoDX/ProdeChampIA/issues/1) awards 5 points for an exact score, 3 for a correct winner or a correct draw with a different score, and 0 for an incorrect outcome. Awards do not accumulate. The [implementation plan](../stories/PRODE-001/implementation-plan.md) preserves the five Issue acceptance criteria.

On 2026-10-26 the user approved the Domain-only calculator and QA/RED closure, prohibited negative goals, and then explicitly confirmed rejection by the calculator with `ArgumentOutOfRangeException` identifying the offending parameter. All four inputs are validated before scoring, including exact-score cases. Zero is valid. The conversation approval is recorded separately from the unchanged five GitHub Issue acceptance criteria; no Issue update was performed. Scoring and validation are verified by 44 passing unit tests locally and on GitHub. Persistence, rankings, authentication and frontend are outside this story. Independent review returned APPROVED; publication was authorized on 2026-10-27 and final human review of PR #3 remains pending. See the [CI/publication report](../stories/PRODE-001/ci-report.md).

## PRODE-002 — scoped Match increment

[Issue #4](https://github.com/DiegoDX/ProdeChampIA/issues/4) requires an identifier, home/away teams, UTC kickoff and no self-play. The existing user-approved increment retains provisional team strings unchanged and defers team logic/validation, including self-play prevention (AC3). The 2026-11-17 correction completes the existing construction contract: preserve supplied properties, reject empty IDs/nonzero UTC offsets/undefined statuses and allow all four defined states. It does not establish full Issue completion, despite the Issue being closed. No persistence, endpoints, external football API, authentication or match lifecycle is implemented. See the [QA scope decisions](../stories/PRODE-002/qa-report.md) and [correction report](../engineering-reports/PRODE-002-correction-engineering-report.md).
