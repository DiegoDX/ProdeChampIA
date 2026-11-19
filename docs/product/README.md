# Product documentation

## PRODE-004 - scheduled edit cutoff

[Issue #10](https://github.com/DiegoDX/ProdeChampIA/issues/10) is implemented as an evaluation-only Domain rule. PredictionEditCutoff.IsAllowed permits an attempt strictly before Match.StartTimeUtc and blocks equality or later, using the trusted zero-offset UTC server validation instant supplied by the caller. Opening a form earlier or a delayed actual kickoff does not extend the scheduled deadline. Preserve the decision for that attempt if later processing crosses kickoff; no editing or saving is implemented here.

Evaluation neither accesses nor changes Prediction and adds no status restrictions. Correct associated-match selection and trustworthy time sourcing remain caller prerequisites; passing this cutoff does not establish full edit permission. Creation, UI, API, SQL, authorization, scoring and rescheduling remain excluded. [Implementation evidence](../stories/PRODE-004/implementation-evidence.md) records 21 new / 147 total passing local unit cases; reviews and remote CI are recorded separately in [workflow status](../stories/PRODE-004/workflow-status.md).

ProdeChamp is a football prediction platform. Product behavior, scoring, competitions and permissions require approved user stories before implementation.

Store approved product scope and business rules here. Sprint 0 defines no business rules.

## PRODE-001 — current scope

The [approved Issue #1](https://github.com/DiegoDX/ProdeChampIA/issues/1) awards 5 points for an exact score, 3 for a correct winner or a correct draw with a different score, and 0 for an incorrect outcome. Awards do not accumulate. The [implementation plan](../stories/PRODE-001/implementation-plan.md) preserves the five Issue acceptance criteria.

On 2026-10-26 the user approved the Domain-only calculator and QA/RED closure, prohibited negative goals, and then explicitly confirmed rejection by the calculator with `ArgumentOutOfRangeException` identifying the offending parameter. All four inputs are validated before scoring, including exact-score cases. Zero is valid. The conversation approval is recorded separately from the unchanged five GitHub Issue acceptance criteria; no Issue update was performed. Scoring and validation are verified by 44 passing unit tests locally and on GitHub. Persistence, rankings, authentication and frontend are outside this story. Independent review returned APPROVED; publication was authorized on 2026-10-27 and final human review of PR #3 remains pending. See the [CI/publication report](../stories/PRODE-001/ci-report.md).

## PRODE-002 — scoped Match increment

[Issue #4](https://github.com/DiegoDX/ProdeChampIA/issues/4) requires an identifier, home/away teams, UTC kickoff and no self-play. The existing user-approved increment retains provisional team strings unchanged and defers team logic/validation, including self-play prevention (AC3). The 2026-11-17 correction completes the existing construction contract: preserve supplied properties, reject empty IDs/nonzero UTC offsets/undefined statuses and allow all four defined states. It does not establish full Issue completion, despite the Issue being closed. No persistence, endpoints, external football API, authentication or match lifecycle is implemented. See the [QA scope decisions](../stories/PRODE-002/qa-report.md) and [correction report](../engineering-reports/PRODE-002-correction-engineering-report.md).

## PRODE-003 — prediction creation

[Issue #5](https://github.com/DiegoDX/ProdeChampIA/issues/5) and the user-approved [plan](../stories/PRODE-003/implementation-plan.md) / [QA design](../stories/PRODE-003/test-design.md) define the current Domain increment. Prediction.Create rejects null/empty participant GUIDs, requires an existing non-null Match, accepts nonnegative integer home/away goals and preserves a caller-supplied zero-offset UTC registration timestamp. It returns immutable participant/match identifiers, goals and time without calculating points.

The user rejected an in-memory registry. The factory instead rejects a matching participant/match pair in the complete/current active-prediction snapshot supplied by the caller, regardless of score or Match object identity. It does not store predictions or modify the snapshot. This approved boundary does not prove participant/catalog membership, database match existence or global/concurrent/durable uniqueness; the caller owns context completeness, time sourcing and future serialization/persistence. No lifecycle, REST, SQL, Angular, authentication, editing/deletion or kickoff lock is introduced. Match team validation remains deferred under PRODE-002. See [implementation evidence](../stories/PRODE-003/implementation-evidence.md) for 43 passing new cases and 126 passing local unit cases; remote CI status is tracked separately.
