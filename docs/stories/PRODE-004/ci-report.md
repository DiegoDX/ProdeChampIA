# PRODE-004 - Publication and CI evidence

## Authority and scope

The user explicitly authorized local commits, pushing feature/PRODE-004 and creating a PR against main, with passing checks and final human review; no merge, auto-merge, Issue write or deployment. Publish only inspected story files; preserve/exclude pre-existing architect/tdd-implementation skill edits. Existing .NET CI is sufficient; no pipeline edits or Actions mutations are needed.

Baseline main: `950ec8963012f2367ae2a95a1891f8b10ba84458`. Fresh Issue #10 body/updatedAt matched the planning baseline on prepublication refresh; no matching PR existed (`gh pr list --repo DiegoDX/ProdeChampIA --head feature/PRODE-004 --state all --json number,state,url,headRefOid`, exit 0, empty array).

## Local results

[Implementation evidence](implementation-evidence.md) records three behavioral RED/GREEN cycles, 21 focused passes, Release six-project build with 0 warnings/errors, 147 total unit passes/0 failed/skipped and integration0. Source/test SHA256 and scope-bound independent reviews are linked from [workflow status](workflow-status.md). No dependency, workflow, project-reference or old test changes.

## Required-check configuration

`gh api repos/DiegoDX/ProdeChampIA/branches/main --jq '{name: .name, protected: .protected}'` returned protected=false. `gh api repos/DiegoDX/ProdeChampIA/rulesets --jq '.[] | {id,name,enforcement}'` returned sole ruleset Protect main, id25513747, enforcement=disabled. No required-check enforcement is currently configured; do not interpret that as a CI pass. The coordinator will require all actual .NET CI checks for the exact PR head to succeed before final handoff.

## Remote publication / CI

Both independent local review gates passed. Authorized staging of the explicit 15 story files failed exit128: `fatal: Unable to create 'C:/ProdeChampIA/ProdeChampIA/.git/index.lock': Permission denied`. The current session treats .git as read-only. No commit, push or PR was performed. No remote story SHA, PR URL or successful run is claimed yet.

Prepared `.local/PRODE-004/publish-reviewed-story.ps1` for the user's own terminal: it checks the branch/base/frozen source hashes, excludes unrelated staged files and skill edits, stages exactly15 inspected story files, checks whitespace, commits and pushes feature/PRODE-004. This handoff does not alter permissions or merge. Once the branch is pushed, the coordinator can create the PR and inspect CI using gh. Actual outcomes must replace this blocker after completion. If a metadata-only follow-up is committed, verify its exact latest head separately and persist final head/run evidence in the PR body/Checks to avoid a self-referential commit hash.

## Limitations

Integration currently executes zero cases. Pure Domain verification cannot establish trusted-time sourcing, match lookup or excluded save/edit behavior. Review verdicts do not authorize merge. Any pending/missing/cancelled/failed exact-head check blocks a ready-for-review delivery claim.
