# ProdeChamp — Agent Instructions

## Project

ProdeChamp is a football prediction platform built using C#, .NET 10, SQL Server and Angular.

The repository also serves as an AI-assisted software engineering portfolio.

## Engineering Principles

- Follow Clean Architecture pragmatically.
- Prefer simple, maintainable solutions.
- Apply SOLID where appropriate.
- Do not introduce unnecessary abstractions.
- Use dependency injection.
- Use async/await for asynchronous I/O.
- Do not use AddWithValue with SQL Server.
- Do not introduce EF Core, MediatR or other dependencies without approval.
- Do not implement functionality outside the approved user story.

## Development Workflow

1. Read the user story and acceptance criteria.
2. Inspect relevant architecture documentation.
3. Produce an implementation plan.
4. Write tests before implementing production code.
5. Verify that the tests fail for the expected reason.
6. Implement the functionality.
7. Run relevant tests.
8. Review the changes.
9. Produce an engineering report.

## Restrictions

- Never modify acceptance criteria without approval.
- Never modify tests merely to make implementation pass.
- Never push directly to main.
- Never merge a Pull Request.
- Never commit secrets.
- Ask for clarification when business rules are ambiguous.

## Required Output

Every completed task must report:

- Files changed.
- Technical decisions.
- Tests executed and their actual results.
- Known limitations.
- Interview questions generated.

## Documentation

Use docs/ as the source of truth for product and architecture decisions.

Keep documentation synchronized with implementation.
