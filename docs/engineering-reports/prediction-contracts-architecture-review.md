# Prediction contracts correction - independent architecture review

## Scope and evidence

- Date: 2026-11-19.
- Reviewer: `/root/review_prediction_architecture`.
- Independence: **Independent**; this reviewer did not implement the change and changed only this review artifact.
- Scope: user-authorized maintenance correction to PRODE-003/PRODE-004: remove PredictionEditCutoff, move evaluation to `Prediction.CanEdit(DateTimeOffset matchStartTimeUtc)` with internal/server time, replace Create's Match parameter with Guid, and update tests. The user's separately approved correction is the technical authorization; the original Issue remains unchanged.
- Fresh source: `gh issue view 10 --repo DiegoDX/ProdeChampIA --json number,title,body,state,comments,url,updatedAt`, exit 0. [Issue #10](https://github.com/DiegoDX/ProdeChampIA/issues/10) was OPEN, updated `2026-11-18T23:50:54Z`, with no comments. Its five acceptance criteria remain the strict before/equal/after cutoff, validation-time decision preservation and prediction nonmutation.
- Local branch: `feature/PRODE-004`; verified local HEAD `33caab5a34717d2b058bc64f9bbe4bbe5af3e77b`. Reviewed the correction working-tree diff against that HEAD, including the restored untracked cutoff test file. Intended PR target is main; this review does not claim to verify a future remote commit.
- Inspected: AGENTS.md, shared Development Factory contract, architecture-review skill, product/architecture docs, ADR index, correction plan/evidence/report, Prediction.cs, old class/test deletion diffs, changed factory tests, complete PredictionCanEditTests, all six .csproj files, API Program.cs, source inventory and solution project/document entries. Unrelated skill edits are outside this review.

The final reviewed source SHA-256 values were independently recalculated and match the implementation evidence:

| File | SHA-256 |
| --- | --- |
| src/ProdeChamp.Domain/Predictions/Prediction.cs | `e522384b84493396fe483c9b2ae60a59f821f73836733db949273fed30186c29` |
| src/ProdeChamp.Domain/ProdeChamp.Domain.csproj | `abcfb100d90997c536fb2382f44d2d0de5fe40715fc5ad249b8b9800d6313e3f` |
| tests/ProdeChamp.UnitTests/Predictions/PredictionTests.cs | `15eba9465f1e74b6651d373db05e160a3ae8872238ce17708b8bc1dcbed7dc7b` |
| tests/ProdeChamp.UnitTests/Predictions/PredictionCanEditTests.cs | `49f3539adb8a31b116169ba5f412ed14f467ff73c21be3a47a50fb02e69d1dbc` |

## Verdict

**APPROVED** - The requested entity behavior and primitive input contracts preserve inward dependencies and the documented evaluation-only boundary without new packages or speculative abstractions.

This is an advisory technical review, not permission to merge.

## Findings

**No architectural findings in the inspected scope.**

- `Prediction.cs:28` keeps the public signature to scheduled kickoff only. The internal overload at line 30 accepts BCL TimeProvider; line 39 captures its value once and line 40 uses strict comparison. The clock is not retained and evaluation does not mutate entity properties.
- `Prediction.cs:49` now accepts Guid matchId; lines 67-70 own empty-ID rejection formerly supplied by Match construction. Duplicate comparison and construction use that same identifier at lines 89 and 95. Prediction no longer requires the Match type.
- Domain has no ProjectReference or PackageReference. Its SDK InternalsVisibleTo item grants test-assembly access without a production reference to tests. TimeProvider is part of .NET; no clock package/custom interface is introduced. Remaining dependencies are unchanged: Application -> Domain; Infrastructure -> Application/Domain; API -> Application/Infrastructure; tests depend on production only.
- API remains composition/hosting only. No persistence, external service, transport contract, DI registration or async wrapper was added. The new test file uses SDK default inclusion rather than duplicate Compile items.

## Trade-offs and ADR needs

The public wrapper uses the executing process's system UTC clock, as the user requested. Production callers must execute on the server; deterministic tests use the internal TimeProvider overload. This is a proportionate test seam for this scope and does not justify a new service or ADR.

Primitive IDs/kickoff reduce coupling to Match but cannot establish catalog existence or correct prediction-to-kickoff association. Current XML/product/architecture documentation explicitly places those responsibilities on callers. There is no existing API/database boundary to validate those facts in this increment. The accepted caller-snapshot uniqueness limitation remains unchanged.

| Criterion / boundary | Architecture assessment |
| --- | --- |
| AC-01/02/03: before/equal/after | Covered by single captured clock read and strict instant comparison; test source includes adjacent ticks and date boundaries |
| AC-04: validation decision survives later processing | Covered within evaluation-only scope: no save-time recheck/state is retained; caller preserves the returned boolean; new calls intentionally evaluate new attempts |
| AC-05: prediction unchanged | Covered by get-only state, absence of mutation and an actual before/after test around allowed/blocked evaluations |
| Guid factory correction | Covered by direct ID input/guard/comparison without Domain-to-outer-layer dependency |

## Verification and gaps

- Reviewer executed source/diff/project inspection, Git HEAD/branch queries, SHA-256 calculation and XML TRX-counter reads; all final source checks completed successfully. The obsolete class/test paths are absent.
- The cutoff test file was temporarily missing during review. The coordinator restored its original bytes; the reviewer then inspected its full source and independently confirmed the exact previously tested SHA-256 above. This evidence gap is resolved for the reviewed snapshot.
- Inspected coordinator's fresh `.local/prediction-contracts/publication-build.log`: Release solution build succeeded, six projects, 0 warnings/errors. Inspected `publication-unit.trx`: 146 executed/passed, 0 failed/skipped. Earlier raw RED/GREEN TRX counters matched the correction evidence: factory RED 41 failures / GREEN 41 passes; cutoff RED 22 failures; combined GREEN 63 passes; original final regression 146 passes.
- Inspected integration.trx: zero tests discovered/executed; this is no integration coverage. The reviewer did not rerun build/tests because the fresh coordinator evidence and final source hashes match.
- No remote CI/publication result is claimed for this working tree. No production deployment, server-clock provenance, authorization, editing/saving, database existence or concurrent durable uniqueness is verified.
- No JSON requested; no schema validation claimed. A later source/contract change invalidates this review and requires reassessment.

## Handoff

- Outcome: **Passed** architectural review on the frozen correction snapshot.
- Files changed by this reviewer: only this architecture-review document. Coordinator should add it to the engineering-reports solution folder alongside the code review.
- Technical decision: accept the user-requested entity placement/primitive inputs and BCL clock test seam within the existing Domain boundary.
- Required next action: coordinator reconciles both independent reviews with the final source, completes authorized publication and checks exact-head CI. No merge is authorized by this review.
