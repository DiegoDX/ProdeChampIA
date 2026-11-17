using ProdeChamp.Domain.Matches;

namespace ProdeChamp.Domain.Predictions;

public sealed class Prediction
{
    private Prediction(Guid participantId, Guid matchId, int predictedHomeGoals, int predictedAwayGoals,
        DateTimeOffset registeredAtUtc)
    {
        ParticipantId = participantId;
        MatchId = matchId;
        PredictedHomeGoals = predictedHomeGoals;
        PredictedAwayGoals = predictedAwayGoals;
        RegisteredAtUtc = registeredAtUtc;
    }

    public Guid ParticipantId { get; }
    public Guid MatchId { get; }
    public int PredictedHomeGoals { get; }
    public int PredictedAwayGoals { get; }
    public DateTimeOffset RegisteredAtUtc { get; }

    /// <summary>Creates a prediction after checking the caller's active-prediction snapshot.</summary>
    /// <remarks>
    /// The caller supplies the valid match, actual UTC registration instant and complete/current active context.
    /// This method neither stores predictions nor guarantees uniqueness across concurrent callers.
    /// </remarks>
    public static Prediction Create(
        Guid? participantId,
        Match? match,
        int predictedHomeGoals,
        int predictedAwayGoals,
        DateTimeOffset registeredAtUtc,
        IReadOnlyCollection<Prediction> activePredictions)
    {
        if (participantId is null)
        {
            throw new ArgumentNullException(nameof(participantId));
        }

        if (participantId == Guid.Empty)
        {
            throw new ArgumentException("Participant ID must not be empty.", nameof(participantId));
        }

        ArgumentNullException.ThrowIfNull(match);
        ArgumentOutOfRangeException.ThrowIfNegative(predictedHomeGoals);
        ArgumentOutOfRangeException.ThrowIfNegative(predictedAwayGoals);

        if (registeredAtUtc.Offset != TimeSpan.Zero)
        {
            throw new ArgumentException("Registration time must have a zero UTC offset.", nameof(registeredAtUtc));
        }

        ArgumentNullException.ThrowIfNull(activePredictions);

        foreach (var existing in activePredictions)
        {
            if (existing is null)
            {
                throw new ArgumentException("Active predictions must not contain null entries.", nameof(activePredictions));
            }

            if (existing.ParticipantId == participantId.Value && existing.MatchId == match.Id)
            {
                throw new InvalidOperationException("An active prediction already exists for this participant and match.");
            }
        }

        return new Prediction(participantId.Value, match.Id, predictedHomeGoals, predictedAwayGoals, registeredAtUtc);
    }
}
