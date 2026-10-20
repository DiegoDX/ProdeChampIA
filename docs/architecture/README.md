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

The API currently contains only host creation and startup. Domain, Application and Infrastructure contain no source classes. No database provider, persistence model, middleware feature or business endpoint is configured. No dependency-injection abstractions are introduced before they are needed.

Document significant future decisions under [decisions/](decisions/README.md). SQL Server and Angular are planned; this scaffold does not configure them.
