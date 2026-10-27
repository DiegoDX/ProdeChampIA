# Documentation

This directory is the source of truth for product and architecture decisions. Sprint 0 creates the structure without inventing business requirements.

| Directory | Purpose |
| --- | --- |
| [product/](product/README.md) | Product scope and approved business rules |
| [user-stories/](user-stories/README.md) | User stories and acceptance criteria |
| [architecture/](architecture/README.md) | Layer boundaries and technical structure |
| [architecture/decisions/](architecture/decisions/README.md) | Architecture decision records |
| [testing/](testing/README.md) | Test strategy and verification commands |
| [engineering-reports/](engineering-reports/sprint-0.md) | Completed work, actual results and limitations |
| [interview-questions/](interview-questions/sprint-0.md) | Questions grounded in engineering decisions |

Current story: [PRODE-001 plan](stories/PRODE-001/implementation-plan.md), [historical QA/RED evidence](stories/PRODE-001/qa-report.md), and [implementation engineering report](engineering-reports/PRODE-001-engineering-report.md). Scoring and the approved negative-input rejection pass all 44 unit tests locally and on GitHub. Independent code review returned APPROVED; [PR #3](https://github.com/DiegoDX/ProdeChampIA/pull/3) awaits final human review.

Continuous integration: the [ci skill](../.agents/skills/ci/SKILL.md) configures and verifies GitHub Actions without deployment. Its [application and publication report](stories/PRODE-001/ci-report.md) records successful local Release checks and the remote run associated with the published implementation commit.
