# PRODE-002 — Match construction correction evidence

Date: 2026-11-17. Branch: `fix/PRODE-002-match-construction`.

## Baseline, authority and local plan

- Base: `e39f37450c21e2cf7c02f0dbd11f524a0fd95272`, verified against remote `main` before branching. The Skills work remains on `chore/development-factory-skills`; this correction does not include it. Pre-existing local `.codex/config.toml` changes are preserved and excluded from the correction.
- Retrieved [Issue #4](https://github.com/DiegoDX/ProdeChampIA/issues/4) on 2026-11-17 using `gh issue view 4 --repo DiegoDX/ProdeChampIA --json number,title,url,body,state,labels,comments,updatedAt` (exit 0). Observed CLOSED, updated `2026-11-17T18:07:55Z`, no comments. Its unchanged criteria are: AC1, identifier/home/away teams; AC2, UTC start date/time; AC3, a team cannot play itself. Closed state is not evidence that every criterion is implemented.
- The user selected option **1** after the explicit offer to correct the empty `Match` constructor and its 31 failing tests. This authorizes the local correction. It does not authorize publishing a different story branch under the earlier Skills publication approval.
- After the completed correction and its 83 passing local tests were presented, the user explicitly replied **“si autorizo”** to the request for commit, push and PR creation against `main`. This is separate authorization for this correction; no merge is authorized. Remote CI verification follows publication.
- Inputs: [implementation plan](implementation-plan.md) and [QA design / historical RED](qa-report.md). The plan's initial approval questions are historical; QA records the user's explicit team/AC3 deferral and the non-team construction contract. [PR #6](https://github.com/DiegoDX/ProdeChampIA/pull/6), titled `docs(PRODE-002): approve implementation plan`, is merged. This task is a scoped correction of that existing tested contract, not a new architecture proposal or full story lifecycle.
- Plan: reproduce existing behavioral RED; assign the five properties; implement guards for empty ID, nonzero offset and undefined status in increments; run focused checks and Release regression; self-review and synchronize documentation. Reuse the 39 existing cases without changing expectations or adding redundant tests. No new dependencies, API, SQL, lifecycle behavior or team rules.

## Source identity and traceability

Only production change: `src/ProdeChamp.Domain/Matches/Match.cs`. Final Git blob: `d3288bba5a598e2c355d4e38e1755c016838cc28`. Baseline blob: `529a59e3171f85c07b04112162eaaa50d04b38d3`. Tests remain unchanged at blob `7286c7c3fa4b24cc73048db81bf7e3bfc0dddf9c`. `MatchStatus`, scoring, project references and packages are unchanged.

| Scope | Existing cases | Code / outcome |
| --- | --- | --- |
| AC1 representation and raw team roles | M01, M04, M13, M14 | Direct assignment to Id/HomeTeam/AwayTeam; no normalization or team identity inference. |
| AC1 / P2 nonempty ID | M05, M12 | `Guid.Empty` throws `ArgumentException`, parameter `id`. |
| AC2 UTC | M01, M08, M09, M12 | Preserve exact zero-offset input; otherwise `ArgumentException`, parameter `startTimeUtc`; no conversion or date window. |
| P1 / P2 status | M10, M11, M12, M16 | Preserve all four enum members; undefined values throw `ArgumentOutOfRangeException`, parameter `status`. |
| P3 independence and immutable public state | M13, M15 | Pure in-memory constructor and get-only properties. No global ID lookup. |
| AC3 and team validation | Deliberately deferred in approved QA scope | Not implemented, not covered and not claimed complete. |
| PRODE-001 regression | R01, both scoring test classes | All 44 existing cases pass as part of 83-case regression. |

## Commands and execution

Working directory for every command: `C:\ProdeChampIA\ProdeChampIA`. SDK observed in the initial error path: 10.0.401; repository `global.json` permits feature roll-forward from 10.0.100. Existing tooling remains xUnit 2.9.3, VSTest, Microsoft.NET.Test.Sdk 17.14.1 and VS adapter 3.1.4.

Common environment, set before each .NET invocation:

```powershell
$env:DOTNET_CLI_HOME=Join-Path (Get-Location) '.local/dotnet'
$env:DOTNET_SKIP_FIRST_TIME_EXPERIENCE='1'
$env:DOTNET_CLI_TELEMETRY_OPTOUT='1'
$env:DOTNET_ADD_GLOBAL_TOOLS_TO_PATH='0'
```

The first RED attempt was blocked by MSB3491 access denied to `src/ProdeChamp.Domain/obj/Debug/net10.0/ProdeChamp.Domain.AssemblyInfoInputs.cache`; exit 1, no tests executed. The identical command was then run with runtime approval outside the sandbox. Subsequent .NET checks used that approved execution context. This environment failure is not behavioral RED or a failed correction round.

Executed commands, in order:

```powershell
dotnet test tests/ProdeChamp.UnitTests/ProdeChamp.UnitTests.csproj --no-restore --filter 'Story=PRODE-002' --logger 'trx;LogFileName=PRODE-002-fix-red.trx' --results-directory tests/TestResults --verbosity minimal

# After direct assignments, before guards:
dotnet test tests/ProdeChamp.UnitTests/ProdeChamp.UnitTests.csproj --no-restore --filter 'Story=PRODE-002&(FullyQualifiedName~Preserves|FullyQualifiedName~Constructor_WhenIdIsReused)' --logger 'trx;LogFileName=PRODE-002-fix-assignments-green.trx' --results-directory tests/TestResults --verbosity minimal

# After the ID guard:
dotnet test tests/ProdeChamp.UnitTests/ProdeChamp.UnitTests.csproj --no-restore --filter 'Story=PRODE-002&FullyQualifiedName~Constructor_WithEmptyId' --logger 'trx;LogFileName=PRODE-002-fix-id-green.trx' --results-directory tests/TestResults --verbosity minimal

# After the UTC guard:
dotnet test tests/ProdeChamp.UnitTests/ProdeChamp.UnitTests.csproj --no-restore --filter 'Story=PRODE-002&(FullyQualifiedName~Constructor_WithNonzeroOffset|FullyQualifiedName~Constructor_WithUtcDateBoundaries)' --logger 'trx;LogFileName=PRODE-002-fix-utc-green.trx' --results-directory tests/TestResults --verbosity minimal

# After the enum guard:
dotnet test tests/ProdeChamp.UnitTests/ProdeChamp.UnitTests.csproj --no-restore --filter 'Story=PRODE-002' --logger 'trx;LogFileName=PRODE-002-fix-green.trx' --results-directory tests/TestResults --verbosity minimal

dotnet build ProdeChamp.slnx --no-restore --configuration Release --verbosity minimal
dotnet test ProdeChamp.slnx --no-build --no-restore --configuration Release --logger 'trx;LogFilePrefix=PRODE-002-fix-regression' --results-directory tests/TestResults --verbosity minimal
```

| Execution / increment | Exit | Executed / passed / failed / skipped |
| --- | --- | --- |
| Verified RED against unchanged skeleton | 1 | 39 / 8 / 31 / 0 |
| Property preservation GREEN | 0 | 17 / 17 / 0 / 0 |
| Empty ID GREEN | 0 | 1 / 1 / 0 / 0 |
| UTC rejection and boundaries GREEN | 0 | 14 / 14 / 0 / 0 |
| All PRODE-002 GREEN | 0 | 39 / 39 / 0 / 0 |
| Release solution build | 0 | All 6 projects; 0 warnings and 0 errors |
| Release unit regression | 0 | 83 / 83 / 0 / 0, comprising 39 PRODE-002 and 44 PRODE-001 |
| Release integration runner | 0 (solution command) | 0 executed; no integration coverage |

RED assertions demonstrated missing assignments and guards: expected nonempty IDs but received `Guid.Empty`, expected supplied strings but received null, and expected argument exceptions but none were thrown. Compilation and discovery succeeded. The eight initial passes established public shape/default values, not full correctness. All missing behaviors were observed before implementation. No further refactor was needed; constructor checks and assignments remain explicit. Final regression validates their composition. Unsuccessful correction rounds: 0.

TRX files remain ignored under `tests/TestResults/`, including `PRODE-002-fix-{red,assignments-green,id-green,utc-green,green}.trx`, `PRODE-002-fix-regression_net10.0_20261117164350.trx` (integration, zero cases) and `PRODE-002-fix-regression_net10.0_20261117164351.trx` (83 unit cases). They are not solution items.

## Review and limitations

Structural checks passed (exit 0): XML parse and `Test-Path` checks retained all 6 projects and 13 unique existing solution items; 21 local Markdown links in the changed documentation resolved; `git diff --check` was clean; the scoped `git diff --name-only` inspection confirmed tests, Skills, workflow, other layers, scoring and `MatchStatus` unchanged. Both new documents are solution items and generated TRX files remain excluded. Git inspection commands used the invocation-scoped `-c safe.directory=C:/ProdeChampIA/ProdeChampIA` option without changing global configuration.

Executed `dotnet msbuild src/ProdeChamp.Domain/ProdeChamp.Domain.csproj -getItem:Compile -getProperty:EnableDefaultCompileItems` and the same command for `tests/ProdeChamp.UnitTests/ProdeChamp.UnitTests.csproj`, with the common environment above (exit 0 each). `EnableDefaultCompileItems` is true; `Matches/Match.cs`, `Matches/MatchStatus.cs` and `Matches/MatchTests.cs` each occur once in their owning project's compile items. No project file edit or duplicate compile entry is needed.

Self-review by the implementing Codex agent: constructor signature, property mutability and enum remain stable; guards throw the required types and parameter names before construction returns; input values are preserved; no legitimate test changed, dependency was added or outward layer reference introduced. No independent review clearance is claimed. Team validity and AC3 remain deferred. Visual Studio UI was not inspected. At local verification completion, no correction commit, push, PR, CI run or Issue write had been performed. Publication was subsequently authorized as recorded above; remote results belong in the linked report and PR handoff. Historical RED reports remain unchanged.

See the [engineering report](../../engineering-reports/PRODE-002-correction-engineering-report.md) for the final handoff.
