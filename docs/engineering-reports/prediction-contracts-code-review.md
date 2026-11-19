# Prediction contracts correction - independent code review

## Summary

- Reviewer: `/root/review_prediction_code`; **Independent**. This reviewer did not implement the production code or tests and edited only this report.
- Date: 2026-11-19; Issue retrieved with `gh issue view 10 --repo DiegoDX/ProdeChampIA --json number,title,body,state,labels,comments,url,updatedAt` during review. [Issue #10](https://github.com/DiegoDX/ProdeChampIA/issues/10) remains OPEN, updated `2026-11-18T23:50:54Z`, with no comments changing the baseline.
- Task: separately user-authorized correction of PRODE-003/004 contracts: delete PredictionEditCutoff, expose `Prediction.CanEdit(DateTimeOffset matchStartTimeUtc)` using internal server time, change Create to `Guid matchId`, and update tests. This correction uses the scoped-maintenance exception; it does not edit GitHub acceptance criteria.
- Branch/source: frozen working tree on `feature/PRODE-004`, local HEAD `33caab5a34717d2b058bc64f9bbe4bbe5af3e77b`, target main `950ec8963012f2367ae2a95a1891f8b10ba84458`. Remote feature parent reported by coordinator: `48fd50992998e3f4a0f2663a1e958dbdbb1b0c54` (prior metadata checkpoint). Review binds the correction's code/test hashes below rather than claiming a not-yet-created publication commit.
- Verdict: **APPROVED**. No blocking correctness, regression, security or test-coverage findings in the inspected correction scope. Risk is low within the explicitly documented Domain-only boundary.

## Scope and source identity

Read AGENTS.md, code-review skill, shared factory contract, current architecture/product/testing documentation, decisions index, correction plan/evidence/report, Domain Prediction and Match, Domain/unit project files, complete factory and replacement cutoff tests, and the relevant tracked diff including deleted old cutoff code/tests. Unrelated skill edits and prior CI/report metadata were excluded from this review's product scope.

| File | Independently verified SHA-256 |
| --- | --- |
| src/ProdeChamp.Domain/Predictions/Prediction.cs | `e522384b84493396fe483c9b2ae60a59f821f73836733db949273fed30186c29` |
| src/ProdeChamp.Domain/ProdeChamp.Domain.csproj | `abcfb100d90997c536fb2382f44d2d0de5fe40715fc5ad249b8b9800d6313e3f` |
| tests/ProdeChamp.UnitTests/Predictions/PredictionTests.cs | `15eba9465f1e74b6651d373db05e160a3ae8872238ce17708b8bc1dcbed7dc7b` |
| tests/ProdeChamp.UnitTests/Predictions/PredictionCanEditTests.cs | `49f3539adb8a31b116169ba5f412ed14f467ff73c21be3a47a50fb02e69d1dbc` |

The untracked replacement cutoff test file was temporarily missing during preflight. The coordinator restored it; this reviewer then read the complete restored file, independently verified its hash matches the previously tested snapshot, and inspected fresh build/test evidence. This initial evidence gap is resolved for the final reviewed scope; absence of that file in any publication would invalidate this verdict.

## Findings

No actionable code findings in the inspected scope.

- Prediction.cs:28-40 exposes only kickoff publicly, uses TimeProvider.System, validates the UTC kickoff convention previously enforced by Match, reads time once, and compares strictly before kickoff. The internal overload gives deterministic boundary tests without a new dependency or public time override. There is no edit/save operation, hidden state or mutation.
- Prediction.cs:49-95 uses the supplied Guid directly, rejects Guid.Empty, preserves participant/goal/UTC/context guards, compares duplicate participant/match IDs, and retains immutable constructed values. It does not add a creation deadline.
- Factory null-Match/object-identity/status fixtures are obsolete under the explicitly requested Guid input. Replacing them with empty/nonempty Guid checks preserves the relevant identity invariant. Other factory expected results remain intact.
- Replacement cutoff tests cover adjacent ticks, equality, past/future, extreme dates, UTC kickoff guards, public clock behavior, advancing independent attempts, one captured instant and nonmutation. No expected cutoff behavior was weakened.

## Acceptance criteria

| Criterion / authorized correction | Assessment | Evidence |
| --- | --- | --- |
| AC-01: allow pre-kickoff validation | Covered | Strict comparison and before/tick-before rows |
| AC-02: block equality | Covered | Equality row and repeated-attempt test |
| AC-03: block later validation | Covered | Tick-after/later rows; no form-opening input |
| AC-04: retain pre-kickoff decision through subsequent processing | Covered within evaluation scope | Single clock read, returned value retained while later calls use new instants; no save-time recheck in implementation. Saving integration is excluded. |
| AC-05: evaluation leaves Prediction unchanged | Covered | Source review of method plus allowed/blocked nonmutation test |
| Instance CanEdit with kickoff only and server time | Covered | Public signature, System wrapper and public-method test |
| Create accepts Guid rather than Match | Covered | Guid signature, empty guard, ID preservation and duplicate tests |

## Test evidence

No tests rerun by this reviewer: the coordinator had just rebuilt and executed the frozen source, and the actual results were available locally. This reviewer independently parsed raw TRX counters and inspected the new build log rather than relying solely on a narrative claim.

- Original recorded factory RED/GREEN: `create-red.trx` has 41 executed/failed; `create-green.trx` has 41 executed/passed, 0 failed/skipped.
- Original cutoff RED: `can-edit-red.trx` has 22 executed/failed. Combined `prediction-green.trx` has 63 executed/passed.
- Original full `unit-regression.trx`: 146 executed/passed, 0 failed/skipped. Integration TRX: 0 executed.
- Fresh publication verification: `.local/prediction-contracts/publication-build.log` ends with successful Release solution build, 0 warnings/errors. `.local/prediction-contracts/publication-unit.trx` has **146 executed/passed, 0 failed/skipped**. Coordinator commands: `dotnet build ProdeChamp.slnx -c Release --no-restore`; `dotnet test tests/ProdeChamp.UnitTests/ProdeChamp.UnitTests.csproj -c Release --no-build --no-restore --logger 'trx;LogFileName=publication-unit.trx' --results-directory .local/prediction-contracts`.
- `git -c safe.directory=C:/ProdeChampIA/ProdeChampIA diff --check` executed by this reviewer: exit 0, line-ending notices only.
- Exact implementation commands, environment/setup distinction, test counts and prior evidence are in [implementation evidence](prediction-contracts-implementation-evidence.md). JSON was not requested; no schema-validation claim.

## Residual risks and handoff

- A Guid/kickoff cannot prove external match existence or correct association; the caller owns those preconditions. Complete/current duplicate context still cannot establish global concurrent uniqueness. These are documented boundaries, not newly introduced persistence guarantees.
- System clock belongs to the executing process. Public evaluation must run on the server; no endpoint, deployment, authorization or saving integration was implemented or verified.
- Integration has no cases. Recorded successful unit/build results do not claim integration coverage or current remote CI success.
- This verdict covers the identified final source and its correction, not unrelated dirty files or future changes. The coordinator must include the replacement test source, add this report to Solution Explorer, isolate the publication scope, verify exact-head CI and obtain human review before any merge. Agents do not merge.

## Final verdict

**APPROVED** - ready for publication/human review subject to exact-source CI and the coordinator's publication checks. Report file added: `docs/engineering-reports/prediction-contracts-code-review.md`; no application code, tests, requirements or Git/GitHub state changed by this reviewer.
