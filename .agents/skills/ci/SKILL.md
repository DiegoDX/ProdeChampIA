---
name: ci
description: Configure and verify ProdeChamp continuous integration with GitHub Actions, .NET builds and test evidence. Use for CI setup, pipeline changes or failed checks; excludes deployment and production-code implementation.
---

# CI — ProdeChamp

Deliver a verifiable build-and-test pipeline and an honest account of what ran locally versus on GitHub. This skill covers continuous integration; it does not deploy or choose a hosting platform.

## Establish the evidence

- Read [AGENTS.md](../../../AGENTS.md), the requested story/task, [testing guidance](../../../docs/testing/README.md), applicable architecture decisions, and existing workflow files under `.github/workflows/`.
- Inspect branch, working-tree changes, `global.json`, solution and project files. Include untracked deliverables in the review and preserve unrelated changes.
- Check existing CI before creating another workflow. Use `gh run list`, `gh run view` and `gh pr checks` where applicable. Associate every remote result with its branch, commit and run URL; an older successful `main` run does not verify the working tree.
- If GitHub or a tool is inaccessible, continue safe local checks and name the limitation. An interrupted command without a completed result is not a pass.

## Configure or diagnose within the request

- For a configuration task, describe the intended triggers and checks before editing. For a failed run, inspect the failing step/log and reproduce the relevant failure when feasible; avoid unrelated workflow rewrites.
- Keep SDK selection consistent with `global.json` and use the repository's actual solution/projects. Restore, build and test in compatible configurations; `--no-build` requires a successful build in that configuration.
- Keep runner and action changes proportional to the task. Consult primary GitHub/Microsoft documentation for unfamiliar or version-sensitive behavior. Do not add product dependencies or change project boundaries to make CI pass.
- Make failed restore/build/tests fail the job. When unit tests are expected, detect zero execution instead of accepting a successful runner exit as business verification. Do not hard-code a story's current test count as a permanent pipeline requirement.
- Report an empty integration project separately. Do not create placeholder tests, skip legitimate failures or invent database coverage. Respect approved integration prerequisites when real fixtures exist.
- Preserve test evidence such as TRX with distinct paths/names for each project. Upload only intended build/test artifacts, including failure evidence when available; exclude secrets and unrelated workspace data.
- Use the permissions the workflow needs. Keep PR validation separate from privileged publication; do not use deployment credentials or privileged PR events for ordinary tests.
- Inspect duplicate triggers, concurrency and timeouts when changing them. Publishing a branch can trigger Actions; preparing a local workflow does not run it remotely.

## Verify the resulting workflow

- Validate workflow syntax with available tooling and inspect expressions, paths and action inputs. State if only manual inspection was possible; a syntax check is not a GitHub-hosted execution.
- Run the relevant restore/build/test commands locally where authorized. Record exact commands, configuration, exit codes and executed/passed/failed/skipped counts. Inspect result files, including zero discovery.
- Exercise important new pipeline failure checks where practical, such as a unit report with zero executed tests. Keep synthetic evidence isolated from the real test results.
- If existing sandbox artifacts or NuGet configuration block execution, distinguish environment failure from test failure. Use permitted escalation when needed; do not silently alter machine configuration or dependency policy.
- Confirm the final source matches the verified build if concurrent editor changes occur. Rerun only checks affected by a subsequent change or unresolved failure.
- Add new workflow files and reports to `ProdeChamp.slnx` solution items as required by AGENTS.md. Source/test files belong in their owning projects without duplicate compile entries.

## Handoff and authority

For a story, record evidence in `docs/stories/{STORY_ID}/ci-report.md`; for repository-wide CI maintenance, use an appropriately named report under `docs/engineering-reports/`. Follow an existing task-specific report convention when present.

Include the scope/baseline, files changed, technical decisions, local commands/results, remote run IDs/URLs, known limitations and next action. Distinguish **configured**, **verified locally**, **passed on GitHub**, **failed**, **blocked** and **not run** per check. Include grounded interview questions for completed engineering work as AGENTS.md requires.

Do not modify production behavior, weaken tests, change acceptance criteria or grant code-review/human approval. Fixing a business-code defect found in CI belongs to a separately authorized implementation task. Respect existing authorization and AGENTS.md for commits, pushes, Issue/PR writes and publication; reading a run is not permission to dispatch, rerun or cancel it. Never merge. For CD, first obtain a separate approved deployment scope, environment and release/rollback process.
