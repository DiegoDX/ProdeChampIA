---
name: interview-preparation
description: Prepare evidence-based senior .NET engineering interview questions and answers from completed ProdeChamp work. Use after an engineering report is available, or when reviewing a specific implemented story, PR or architectural decision.
---

# Interview preparation — ProdeChamp

## Mission
Turn **verified engineering work** into interview-ready discussion. Focus on decisions, trade-offs, alternatives, failure modes and test evidence rather than generic textbook questions. Follow repository `AGENTS.md`; `docs/` is the source of truth for approved product and architecture decisions.

## Read and verify context
1. Read `AGENTS.md` and the requested story/PR/task scope.
2. Read the corresponding engineering report, approved acceptance criteria in `docs/user-stories/`, relevant `docs/architecture/README.md` and applicable ADRs in `docs/architecture/decisions/`.
3. Inspect affected source code, project references, test code and available test/CI results. Use Git diffs when the request concerns a change.
4. If a report, implementation or test output cannot be accessed, label the evidence gap explicitly. Never infer that tests passed merely because tests exist.
5. Separate **Implemented and verified**, **Implemented but not verified**, **Planned/proposed**, and **Unknown**. Do not claim SQL Server, Angular, AI/RAG, persistence, authentication or other capabilities have been implemented solely because the project plans to use them.

## Question selection
Choose questions proportional to the work actually performed and to the requested level (default: senior backend .NET). Prioritize:
- **Problem and requirements:** what the story solves, acceptance criteria, constraints and why the scope was chosen.
- **Design and architecture:** Clean Architecture boundaries, layering, dependency injection, simplicity versus abstraction, SOLID when relevant; concrete alternatives and why they were rejected.
- **C# / .NET:** async I/O, cancellation, error handling, lifetime management, APIs and design choices **only where relevant to the inspected change**.
- **Persistence / SQL Server:** data integrity, transactions, typed parameters and avoiding `AddWithValue`, query behavior and performance **only when the relevant code exists**.
- **Testing / TDD:** RED evidence, GREEN evidence, refactoring, unit versus integration tests, regressions, test gaps and actual executed commands/results.
- **Security / reliability / performance:** trust boundaries, authorization, validation, concurrency and operational trade-offs where the change warrants them.
- **Frontend / AI:** Angular, RAG, agents or MCP only when there is actual associated code or an explicitly requested hypothetical/design question.

Avoid duplicate or trivia-only questions. Favor follow-up questions such as “What would break if…?”, “What alternative did you reject and why?”, and “How would you prove this works?”. Flag hypothetical discussions as **Hypothetical**, not shipped functionality.

## Question and answer quality
For each question include:
1. **Question** and **level** (fundamental / applied / senior follow-up).
2. **Why it matters** in this particular ProdeChamp change.
3. **Evidence**: concrete repository file paths, test names, ADRs or report sections; identify when a claim has not been independently verified.
4. **Answer outline**: situation/problem → decision and reasoning → alternatives/trade-offs → validation and limitations. Only present this as a factual project answer when evidence supports it.
5. **Follow-up/probe** that requires explanation, not memorization.
6. **Confidence/evidence status**: verified / reported only / proposed / unknown.

When users request questions only, omit model answers; when they request mock interviews, ask one question at a time, wait for their answer and then give precise feedback grounded in project evidence. Do not fabricate personal achievements, production metrics, incidents or test outcomes.

## Output and files
- Default to a concise, curated set (5–10 questions) ranked by relevance; respect a user-specified count, topic, seniority or language.
- Group by the specific story/change, not by a fixed technology checklist.
- If the user requests a persisted artifact, create or update a clearly named Markdown file under `docs/interview-questions/`, linking the relevant engineering report and story. Preserve existing material unless explicitly authorized to replace it.
- For an artifact, include **Scope**, **Evidence sources**, **Questions and answer outlines** (if requested), **Verification gaps**, and **Suggested follow-up study**.
- Do not edit production code, tests, acceptance criteria or architecture decisions as part of interview preparation. Do not install dependencies, commit, push or merge without explicit authorization.

## Completion report
State files created/changed (or “none”), evidence reviewed, whether any tests were **actually run by this task** (normally none), known gaps and the interview questions produced. Follow `AGENTS.md` required reporting without falsely claiming development or test execution.
