using ProdeChamp.Domain.Scoring;

namespace ProdeChamp.UnitTests.Scoring;

[Trait("Story", "PRODE-001")]
public sealed class PredictionPointsCalculatorValidationTests
{
    [Theory]
    [InlineData(-1, 0, 0, 0, "predictedHomeGoals")]
    [InlineData(0, -1, 0, 0, "predictedAwayGoals")]
    [InlineData(0, 0, -1, 0, "actualHomeGoals")]
    [InlineData(0, 0, 0, -1, "actualAwayGoals")]
    [InlineData(int.MinValue, 0, 0, 0, "predictedHomeGoals")]
    [InlineData(0, int.MinValue, 0, 0, "predictedAwayGoals")]
    [InlineData(0, 0, int.MinValue, 0, "actualHomeGoals")]
    [InlineData(0, 0, 0, int.MinValue, "actualAwayGoals")]
    public void Calculate_WithNegativeGoal_ThrowsArgumentOutOfRangeExceptionForParameter(
        int predictedHomeGoals, int predictedAwayGoals,
        int actualHomeGoals, int actualAwayGoals, string expectedParameterName)
    {
        var calculator = new PredictionPointsCalculator();

        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => calculator.Calculate(
            predictedHomeGoals: predictedHomeGoals,
            predictedAwayGoals: predictedAwayGoals,
            actualHomeGoals: actualHomeGoals,
            actualAwayGoals: actualAwayGoals));

        Assert.Equal(expectedParameterName, exception.ParamName);
    }

    [Theory]
    [InlineData(-1, 0)]
    [InlineData(0, -1)]
    public void Calculate_WithExactNegativeScore_RejectsBeforeAwardingPoints(
        int homeGoals, int awayGoals)
    {
        var calculator = new PredictionPointsCalculator();

        Assert.Throws<ArgumentOutOfRangeException>(() => calculator.Calculate(
            predictedHomeGoals: homeGoals,
            predictedAwayGoals: awayGoals,
            actualHomeGoals: homeGoals,
            actualAwayGoals: awayGoals));
    }
}
