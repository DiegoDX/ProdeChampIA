# Architecture

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

The user approved rejecting negative goals with `ArgumentOutOfRangeException` identifying the offending parameter. `Calculate` applies `ArgumentOutOfRangeException.ThrowIfNegative` to each of its four arguments before evaluating any scoring category. Zero and all nonnegative `int` values remain valid; no maximum below `int.MaxValue` is imposed. See the [current implementation report](../engineering-reports/PRODE-001-engineering-report.md) for the 44 passing local unit tests. Independent review and human approval remain pending.

Document significant future decisions under [decisions/](decisions/README.md). SQL Server and Angular are planned; this scaffold does not configure them.
