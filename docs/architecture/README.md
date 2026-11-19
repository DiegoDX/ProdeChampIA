# Architecture

## PRODE-004 - cutoff rule on Prediction (2026-11-19 correction)

The user explicitly replaced PredictionEditCutoff with the instance method `Prediction.CanEdit(DateTimeOffset matchStartTimeUtc)`. It requires a zero-offset scheduled UTC kickoff and captures `TimeProvider.System.GetUtcNow()` once per call, returning true strictly before kickoff and false at/after it. The public caller supplies only kickoff; an internal TimeProvider overload supports deterministic boundary tests. SDK InternalsVisibleTo exposes internals to the unit-test assembly without a production reference to it. No clock package, custom clock interface, retained state, scoring, status policy or persistence is introduced.

No project/package reference, DI registration or outer-layer implementation changes. Evaluation does not modify Prediction; callers preserve the returned decision for the same attempt even if later processing crosses kickoff. A new call evaluates current server time again. Correct kickoff association and running this public method on the server remain caller responsibilities. See the current [correction report](../engineering-reports/prediction-contracts-engineering-report.md) and [evidence](../engineering-reports/prediction-contracts-implementation-evidence.md). Earlier PRODE-003/004 plans/reviews/CI describe historical contracts and do not verify this correction.

The solution follows pragmatic Clean Architecture. All six projects target `net10.0` with nullable reference types and implicit usings enabled.

| Project | Direct project references | Responsibility |
| --- | --- | --- |
| ProdeChamp.Domain | None | Future business entities, value objects and rules |
| ProdeChamp.Application | Domain | Future use cases and ports |
| ProdeChamp.Infrastructure | Application, Domain | Future external adapters |
| ProdeChamp.API | Application, Infrastructure | HTTP boundary and composition root |
| ProdeChamp.UnitTests | Domain, Application | Future isolated business and use-case tests |
| ProdeChamp.IntegrationTests | API, Infrastructure | Future boundary and adapter tests |

Inner layers must not reference outer layers. API references Infrastructure to compose implementations through dependency injection when services exist. Domain stays independent of frameworks and external services. Test projects are outside production dependency boundaries.

The API currently contains only host creation and startup. Domain contains the synchronous, stateless `Scoring/PredictionPointsCalculator` for PRODE-001. It uses exact-score precedence and outcome comparisons to return one award of 5, 3 or 0. Its four-`int` contract, lack of interface and Domain-only scope were approved on 2026-10-26. Application and Infrastructure contain no source classes. No database provider, persistence model, middleware feature or business endpoint is configured. No dependency-injection abstractions are introduced before they are needed.

The user approved rejecting negative goals with `ArgumentOutOfRangeException` identifying the offending parameter. `Calculate` applies `ArgumentOutOfRangeException.ThrowIfNegative` to each of its four arguments before evaluating any scoring category. Zero and all nonnegative `int` values remain valid; no maximum below `int.MaxValue` is imposed. See the [implementation report](../engineering-reports/PRODE-001-engineering-report.md) for local evidence and the [CI report](../stories/PRODE-001/ci-report.md) for 44 passing remote unit tests. Independent review returned APPROVED; final human review of PR #3 remains pending.

## PRODE-002 — Match construction

Domain also contains `Matches/Match` and the four-value `MatchStatus` enum. `Match` accepts a caller-supplied nonempty GUID, provisional team strings, a zero-offset `DateTimeOffset` and any defined status. Its constructor rejects empty IDs and nonzero offsets with parameter-specific `ArgumentException`, and undefined statuses with `ArgumentOutOfRangeException`. It preserves supplied values in public get-only properties. Team validation and self-play rejection remain deferred by user direction; no team-validity guarantee is made. No project references, dependencies, I/O, lifecycle methods or scoring integration are added. See the [correction report](../engineering-reports/PRODE-002-correction-engineering-report.md) for local verification and limitations.

Document significant future decisions under [decisions/](decisions/README.md). SQL Server and Angular are planned; this scaffold does not configure them.

## PRODE-003 — immutable Prediction factory

Domain contains `Predictions/Prediction` with private construction and get-only ParticipantId, MatchId, PredictedHomeGoals, PredictedAwayGoals and RegisteredAtUtc. After the user-authorized correction, static Create accepts a nullable participant GUID, a `Guid matchId`, integer goals, a DateTimeOffset registration instant and an IReadOnlyCollection of active predictions. It rejects null/empty participant IDs, Guid.Empty match IDs, null context, negative goals, nonzero registration offsets, null collection entries and a duplicate participant/match ID pair. Prediction no longer depends on the Match type. Caller-supplied complete/current context and registration time remain required.

The factory scans caller context without retaining/modifying it; it neither registers/persists predictions nor calls the scoring calculator. It has no outward project references, new packages, I/O or service registration. A nonempty match ID is not database/catalog proof, and caller context cannot guarantee global/concurrent/durable uniqueness. Match/scoring implementations and the six-project dependency graph remain unchanged. Current factory and cutoff tests are described in the [correction evidence](../engineering-reports/prediction-contracts-implementation-evidence.md).
