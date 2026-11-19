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

    /// <summary>Checks the scheduled-kickoff cutoff against the current UTC server time.</summary>
    /// <remarks>
    /// The caller supplies the scheduled UTC kickoff associated with this prediction.
    /// A true result passes only this cutoff rule. Preserve the result for this attempt
    /// if later processing finishes after kickoff; a new call evaluates a new attempt.
    /// This method neither edits nor saves the prediction.
    /// </remarks>
    public bool CanEdit(DateTimeOffset matchStartTimeUtc) => CanEdit(matchStartTimeUtc, TimeProvider.System);

    internal bool CanEdit(DateTimeOffset matchStartTimeUtc, TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);

        if (matchStartTimeUtc.Offset != TimeSpan.Zero)
        {
            throw new ArgumentException("Match start time must have a zero UTC offset.", nameof(matchStartTimeUtc));
        }

        var validatedAtUtc = timeProvider.GetUtcNow();
        return validatedAtUtc < matchStartTimeUtc;
    }

    /// <summary>Creates a prediction after checking the caller's active-prediction snapshot.</summary>
    /// <remarks>
    /// The caller supplies the associated match ID, actual UTC registration instant and complete/current active context.
    /// A nonempty match ID does not establish that the match exists in a catalog or database.
    /// This method neither stores predictions nor guarantees uniqueness across concurrent callers.
    /// </remarks>
    public static Prediction Create(
        Guid? participantId,
        Guid matchId,
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

        if (matchId == Guid.Empty)
        {
            throw new ArgumentException("Match ID must not be empty.", nameof(matchId));
        }

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

            if (existing.ParticipantId == participantId.Value && existing.MatchId == matchId)
            {
                throw new InvalidOperationException("An active prediction already exists for this participant and match.");
            }
        }

        return new Prediction(participantId.Value, matchId, predictedHomeGoals, predictedAwayGoals, registeredAtUtc);
    }
}
