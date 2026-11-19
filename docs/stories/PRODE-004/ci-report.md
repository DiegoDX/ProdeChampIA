# PRODE-004 - Publication and CI evidence

## Authority and scope

### Current contract correction - 2026-11-19

User authorized publishing the entity/primitive-input correction on feature/PRODE-004 and its PR to main. Existing [PR #13](https://github.com/DiegoDX/ProdeChampIA/pull/13) is reused. Preflight remote parent `48fd50992998e3f4a0f2663a1e958dbdbb1b0c54`, main unchanged at `950ec8963012f2367ae2a95a1891f8b10ba84458`. Fresh local Release build0 warnings/errors, 146 unit executed/passed0failed/skipped; integration remains0. Independent correction code/architecture reviews APPROVED. See [correction evidence](../../engineering-reports/prediction-contracts-implementation-evidence.md).

Current-head remote verification is pending publication. Historical147-case runs below cover the previous static evaluator, not these changed contracts. Publish only inspected correction/review/document paths via guarded, non-forced Git Data API feature-ref update; preserve local .git read-only restrictions, existing remote history and unrelated skill edits. No workflow changes or Actions mutations are needed. Final exact-head results must be checked and recorded; a successful older run is insufficient.

### Original-contract publication (historical)

The user explicitly authorized local commits, pushing feature/PRODE-004 and creating a PR against main, with passing checks and final human review; no merge, auto-merge, Issue write or deployment. Publish only inspected story files; preserve/exclude pre-existing architect/tdd-implementation skill edits. Existing .NET CI is sufficient; no pipeline edits or Actions mutations are needed.

Baseline main: `950ec8963012f2367ae2a95a1891f8b10ba84458`. Fresh Issue #10 body/updatedAt matched the planning baseline on prepublication refresh; no matching PR existed (`gh pr list --repo DiegoDX/ProdeChampIA --head feature/PRODE-004 --state all --json number,state,url,headRefOid`, exit 0, empty array).

## Local results

[Implementation evidence](implementation-evidence.md) records three behavioral RED/GREEN cycles, 21 focused passes, Release six-project build with 0 warnings/errors, 147 total unit passes/0 failed/skipped and integration0. Source/test SHA256 and scope-bound independent reviews are linked from [workflow status](workflow-status.md). No dependency, workflow, project-reference or old test changes.

## Required-check configuration

`gh api repos/DiegoDX/ProdeChampIA/branches/main --jq '{name: .name, protected: .protected}'` returned protected=false. `gh api repos/DiegoDX/ProdeChampIA/rulesets --jq '.[] | {id,name,enforcement}'` returned sole ruleset Protect main, id25513747, enforcement=disabled. No required-check enforcement is currently configured; do not interpret that as a CI pass. The coordinator will require all actual .NET CI checks for the exact PR head to succeed before final handoff.

## Remote publication / CI

Both independent local review gates passed. Initial agent staging failed exit128 creating .git/index.lock (Permission denied). The user then executed the scoped publication helper in their own terminal: commit and feature push succeeded. Agent verified local HEAD, published branch ref and exact 15-file commit scope; unrelated skill edits were excluded.

Implementation commit: `33caab5a34717d2b058bc64f9bbe4bbe5af3e77b`, message feat(PRODE-004): enforce scheduled kickoff edit cutoff. Agent created [PR #13](https://github.com/DiegoDX/ProdeChampIA/pull/13) using `gh pr create --repo DiegoDX/ProdeChampIA --base main --head feature/PRODE-004 --title 'feat(PRODE-004): block prediction edits at scheduled kickoff' --body-file .local/PRODE-004/pr-body.md`, exit0. PR is OPEN, non-draft, targets main; no merge/auto-merge/Issue write.

| Event / exact implementation head | Run | Observed result |
| --- | --- | --- |
| push / 33caab5a34717d2b058bc64f9bbe4bbe5af3e77b | [44549746982](https://github.com/DiegoDX/ProdeChampIA/actions/runs/44549746982) | SUCCESS; raw downloaded TRX confirms 147 unit passes, failed/skipped0; integration0. |
| pull_request / same source head | [44549866439](https://github.com/DiegoDX/ProdeChampIA/actions/runs/44549866439) | SUCCESS; raw downloaded TRX confirms 147 unit passes including21 new, failed/skipped0; integration0. Log confirms synthetic merge d6d9b5b9d3dc129c914718006a4c75b2a15a187d, build0 warnings/errors. |

Commands: `gh pr view 13 --repo DiegoDX/ProdeChampIA --json number,url,state,isDraft,headRefOid,baseRefName,mergeStateStatus,statusCheckRollup`; `gh run list --repo DiegoDX/ProdeChampIA --branch feature/PRODE-004 --limit 5 --json databaseId,headSha,event,status,conclusion,url`; `gh run view 44549866439 --repo DiegoDX/ProdeChampIA --json databaseId,headSha,event,status,conclusion,url,jobs`; `gh run download 44549746982 --repo DiegoDX/ProdeChampIA --name test-results --dir .local/PRODE-004/remote/implementation-push` (exit0). Raw downloaded unit/integration TRX inspected, not inferred from a green badge. `gh pr checks 13 --repo DiegoDX/ProdeChampIA --required` reports no required checks (exit1); all actual checks remain mandatory for this delivery.

Also downloaded the PR run using `gh run download 44549866439 --repo DiegoDX/ProdeChampIA --name test-results --dir .local/PRODE-004/remote/implementation-pr`, exit0; parsed actual UnitTestResult names to confirm21 story cases. `gh run view 44549866439 --repo DiegoDX/ProdeChampIA --log` confirmed the tested merge and build/test counts. The first log-cache attempt was denied in the user-profile cache; using a process-local LOCALAPPDATA under ignored .local/PRODE-004/gh-cache resolved log retrieval without machine configuration changes.

The documentation-only publication checkpoint updates this report, workflow-status and engineering report without changing reviewed source/tests. It is published through the authenticated GitHub Git Data API on feature/PRODE-004, using the verified implementation commit as parent and a non-forced ref update; local .git remains untouched/read-only. Request bodies are generated by ignored `.local/PRODE-004/publish-ci-metadata.ps1`: POST git/trees (base tree4f27521a339b1c095922821b29c2f839dbfd0357, exactly3 documentation paths), POST git/commits (parent33caab5), then PATCH git/refs/heads/feature/PRODE-004 (force=false). Guard/compare the branch and exact three-file scope before updating the ref. Verify the resulting exact latest PR head and all actual checks before handoff, and persist final SHA/run URLs/outcomes in the PR body/Checks to avoid a self-referential metadata commit. Local checkout metadata remains at the implementation commit until user synchronization; document contents can still match the published final tree. This limitation does not establish a CI pass.

## Limitations

Integration currently executes zero cases. Pure Domain verification cannot establish trusted-time sourcing, match lookup or excluded save/edit behavior. Review verdicts do not authorize merge. Any pending/missing/cancelled/failed exact-head check blocks a ready-for-review delivery claim.
