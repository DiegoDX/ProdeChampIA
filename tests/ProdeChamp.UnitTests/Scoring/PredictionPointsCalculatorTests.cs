using ProdeChamp.Domain.Scoring;

namespace ProdeChamp.UnitTests.Scoring;

[Trait("Story", "PRODE-001")]
public sealed class PredictionPointsCalculatorTests
{
    // AC1 and AC5: exact scores receive one award, including exact draws.
    [Theory]
    [InlineData(2, 1, 2, 1)]
    [InlineData(0, 2, 0, 2)]
    [InlineData(0, 0, 0, 0)]
    [InlineData(2, 2, 2, 2)]
    public void Calculate_WithExactScore_ReturnsFiveWithoutAddingOutcomePoints(
        int predictedHomeGoals, int predictedAwayGoals,
        int actualHomeGoals, int actualAwayGoals)
    {
        var calculator = new PredictionPointsCalculator();

        var points = calculator.Calculate(
            predictedHomeGoals: predictedHomeGoals,
            predictedAwayGoals: predictedAwayGoals,
            actualHomeGoals: actualHomeGoals,
            actualAwayGoals: actualAwayGoals);

        Assert.Equal(5, points);
    }

    // AC2: both winners, including cases where just one goal value matches.
    [Theory]
    [InlineData(2, 1, 3, 0)]
    [InlineData(0, 1, 1, 3)]
    [InlineData(2, 1, 2, 0)]
    [InlineData(2, 1, 3, 1)]
    [InlineData(1, 2, 0, 2)]
    [InlineData(1, 2, 1, 3)]
    public void Calculate_WithCorrectWinnerAndDifferentScore_ReturnsThree(
        int predictedHomeGoals, int predictedAwayGoals,
        int actualHomeGoals, int actualAwayGoals)
    {
        var calculator = new PredictionPointsCalculator();

        var points = calculator.Calculate(
            predictedHomeGoals: predictedHomeGoals,
            predictedAwayGoals: predictedAwayGoals,
            actualHomeGoals: actualHomeGoals,
            actualAwayGoals: actualAwayGoals);

        Assert.Equal(3, points);
    }

    // AC3: zero-zero can be either the prediction or the final result.
    [Theory]
    [InlineData(1, 1, 2, 2)]
    [InlineData(0, 0, 1, 1)]
    [InlineData(1, 1, 0, 0)]
    public void Calculate_WithCorrectDrawAndDifferentScore_ReturnsThree(
        int predictedHomeGoals, int predictedAwayGoals,
        int actualHomeGoals, int actualAwayGoals)
    {
        var calculator = new PredictionPointsCalculator();

        var points = calculator.Calculate(
            predictedHomeGoals: predictedHomeGoals,
            predictedAwayGoals: predictedAwayGoals,
            actualHomeGoals: actualHomeGoals,
            actualAwayGoals: actualAwayGoals);

        Assert.Equal(3, points);
    }

    // AC4: all six mismatched outcome directions, plus zero-zero results.
    [Theory]
    [InlineData(2, 0, 0, 2)]
    [InlineData(0, 2, 2, 0)]
    [InlineData(1, 1, 2, 1)]
    [InlineData(1, 1, 1, 2)]
    [InlineData(2, 1, 1, 1)]
    [InlineData(1, 2, 1, 1)]
    [InlineData(1, 0, 0, 0)]
    [InlineData(0, 1, 0, 0)]
    public void Calculate_WithIncorrectOutcome_ReturnsZero(
        int predictedHomeGoals, int predictedAwayGoals,
        int actualHomeGoals, int actualAwayGoals)
    {
        var calculator = new PredictionPointsCalculator();

        var points = calculator.Calculate(
            predictedHomeGoals: predictedHomeGoals,
            predictedAwayGoals: predictedAwayGoals,
            actualHomeGoals: actualHomeGoals,
            actualAwayGoals: actualAwayGoals);

        Assert.Equal(0, points);
    }

    // Technical boundaries of the planned int API; no new points or goal cap.
    [Theory]
    [InlineData(1, 0, 1, 0, 5)]
    [InlineData(0, 1, 0, 1, 5)]
    [InlineData(int.MaxValue, 0, int.MaxValue, 0, 5)]
    [InlineData(0, int.MaxValue, 0, int.MaxValue, 5)]
    [InlineData(int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue, 5)]
    [InlineData(int.MaxValue, 0, int.MaxValue - 1, 0, 3)]
    [InlineData(0, int.MaxValue, 0, int.MaxValue - 1, 3)]
    [InlineData(int.MaxValue, int.MaxValue - 1, int.MaxValue - 1, 0, 3)]
    [InlineData(int.MaxValue - 1, int.MaxValue, 0, int.MaxValue - 1, 3)]
    [InlineData(int.MaxValue, int.MaxValue, int.MaxValue - 1, int.MaxValue - 1, 3)]
    [InlineData(int.MaxValue, 0, 0, int.MaxValue, 0)]
    [InlineData(0, int.MaxValue, int.MaxValue, 0, 0)]
    public void Calculate_AtIntegerBoundaries_ReturnsExpectedPoints(
        int predictedHomeGoals, int predictedAwayGoals,
        int actualHomeGoals, int actualAwayGoals, int expectedPoints)
    {
        var calculator = new PredictionPointsCalculator();

        var points = calculator.Calculate(
            predictedHomeGoals: predictedHomeGoals,
            predictedAwayGoals: predictedAwayGoals,
            actualHomeGoals: actualHomeGoals,
            actualAwayGoals: actualAwayGoals);

        Assert.Equal(expectedPoints, points);
    }

    // Plan S12: one instance must not retain totals between calculations.
    [Fact]
    public void Calculate_WhenInstanceIsReused_ReturnsIndependentAwards()
    {
        var calculator = new PredictionPointsCalculator();

        Assert.Equal(5, calculator.Calculate(
            predictedHomeGoals: 2, predictedAwayGoals: 1,
            actualHomeGoals: 2, actualAwayGoals: 1));
        Assert.Equal(0, calculator.Calculate(
            predictedHomeGoals: 2, predictedAwayGoals: 0,
            actualHomeGoals: 0, actualAwayGoals: 2));
        Assert.Equal(5, calculator.Calculate(
            predictedHomeGoals: 2, predictedAwayGoals: 1,
            actualHomeGoals: 2, actualAwayGoals: 1));
    }
}
