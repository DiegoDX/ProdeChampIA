# PRODE-004 - Refinement

## 1. Story baseline

Source: [Issue #10](https://github.com/DiegoDX/ProdeChampIA/issues/10), updated `2026-11-18T23:50:54Z`, retrieved 2026-11-19; faithful full body in [workflow status](workflow-status.md#issue-baseline). The user requested continuation through the coordinator. Scope is evaluation of the edit cutoff only; this refinement does not approve a technical plan or implementation.

The participant's objective is a common deadline: validation strictly before scheduled kickoff passes, equality or later fails. Server validation time is decisive even when processing crosses kickoff. Opening the form earlier and actual kickoff delays do not change the scheduled deadline. Evaluation must preserve the existing prediction.

Consulted: AGENTS.md, the shared factory contract, product/architecture/testing READMEs, architecture/decisions/README.md, Match.cs, MatchStatus.cs, Prediction.cs, the unit test project and existing PredictionTests conventions. Baseline SHA is recorded in workflow status.

## 2. Assessment

The Issue resolves the previous likely timing ambiguities explicitly. AC-01/02/03 define the strict comparison and equality boundary. AC-04 fixes which instant is authoritative, and AC-05 requires evaluation without mutation. Scope expressly excludes implementing editing or saving. No blocking contradictions or missing business decisions were found.

**Important implementation boundary:** the existing Prediction stores MatchId, not a Match object or kickoff. Match already exposes a validated zero-offset StartTimeUtc. The evaluator may use that existing Match and an explicit validation instant without inventing persistence or a prediction editing method. The caller must supply the associated match and trustworthy server time; this story cannot establish those facts through an API or database that does not yet exist.

Existing Match supports Scheduled, InProgress, Finished and Cancelled. The Issue expressly excludes additional match-status restrictions, so those statuses must not silently change the cutoff comparison. Prediction.Create remains a separate creation contract; this story must not start blocking creation.

## 3. Open questions

None blocking within the Issue's evaluation-only scope. Technical API shape and argument errors remain proposed in the architecture plan. Match rescheduling and future edit/save orchestration remain excluded rather than unresolved requirements for this increment.

## 4. Proposed refinement - NOT APPROVED

No wording or acceptance-criteria changes proposed. Preserve all five criteria and all eight business rules unchanged. The proposed Domain API is an implementation choice for plan approval, not an additional business rule.

## 5. Verification outline

| Criterion | Observable result / verification boundary |
| --- | --- |
| AC-01 | At 14:59:59 UTC for a 15:00:00 UTC kickoff, the cutoff passes. |
| AC-02 | At exactly 15:00:00 UTC, the cutoff blocks. |
| AC-03 | After 15:00:00 UTC, the cutoff blocks; opening time has no effect. |
| AC-04 | A decision computed from a pre-kickoff validation instant remains permitted; no later save time or wall-clock recheck is part of evaluation. No save integration test can be claimed in this scope. |
| AC-05 | Source review must establish that evaluation neither accesses nor changes any prediction. Future QA may use meaningful behavioral checks where observable, without manufacturing a save/edit implementation. |

Detailed fixtures, priorities and executable-case counts belong to test-design after plan approval. No tests were executed during refinement.

## 6. Dependencies and scope

Existing Match supplies scheduled UTC kickoff, and Prediction supplies the associated MatchId. Trustworthy server validation time and correct match selection are caller prerequisites. No persistence, UI, REST, authorization, scoring, rescheduling, status policy, clock package or new dependency is required or approved.

## 7. Readiness

**Ready for approval**: requirements are clear enough to prepare a plan. This is not plan/QA approval or story completion. The coordinator may prepare the plan but must honor its approval boundary before QA.

## 8. Engineering handoff

- File created: this refinement; coordinator evidence is in workflow status.
- Technical decisions: none approved; proposed contracts are in [plan v1](implementation-plan.md).
- Tests: not run - refinement only.
- Limitations: no runtime proof of server-time provenance or external association lookup; no editing or saving exists in scope.
