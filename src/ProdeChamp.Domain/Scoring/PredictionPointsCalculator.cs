namespace ProdeChamp.Domain.Scoring;

public sealed class PredictionPointsCalculator
{
    public int Calculate(
        int predictedHomeGoals,
        int predictedAwayGoals,
        int actualHomeGoals,
        int actualAwayGoals)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(predictedHomeGoals);
        ArgumentOutOfRangeException.ThrowIfNegative(predictedAwayGoals);
        ArgumentOutOfRangeException.ThrowIfNegative(actualHomeGoals);
        ArgumentOutOfRangeException.ThrowIfNegative(actualAwayGoals);

        if (predictedHomeGoals == actualHomeGoals && predictedAwayGoals == actualAwayGoals)
        {
            return 5;
        }

        return predictedHomeGoals.CompareTo(predictedAwayGoals) == actualHomeGoals.CompareTo(actualAwayGoals)
            ? 3
            : 0;
    }
}
