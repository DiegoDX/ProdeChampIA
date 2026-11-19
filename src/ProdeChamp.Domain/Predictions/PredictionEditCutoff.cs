using ProdeChamp.Domain.Matches;

namespace ProdeChamp.Domain.Predictions;

public static class PredictionEditCutoff
{
    /// <summary>Determines whether an attempt passes the scheduled-kickoff cutoff.</summary>
    /// <remarks>
    /// The caller supplies the associated match and trusted server time captured at validation.
    /// A true result passes only this cutoff rule. Preserve that decision for the attempt even
    /// if subsequent saving finishes after kickoff. This method neither edits nor saves predictions.
    /// </remarks>
    public static bool IsAllowed(Match match, DateTimeOffset validatedAtUtc)
    {
        ArgumentNullException.ThrowIfNull(match);

        if (validatedAtUtc.Offset != TimeSpan.Zero)
        {
            throw new ArgumentException("Validation time must have a zero UTC offset.", nameof(validatedAtUtc));
        }

        return validatedAtUtc < match.StartTimeUtc;
    }
}
