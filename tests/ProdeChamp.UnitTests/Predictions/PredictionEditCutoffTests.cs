using ProdeChamp.Domain.Matches;
using ProdeChamp.Domain.Predictions;

namespace ProdeChamp.UnitTests.Predictions;

[Trait("Story", "PRODE-004")]
public sealed class PredictionEditCutoffTests
{
    private static readonly DateTimeOffset KickoffUtc = new(2026, 6, 10, 15, 0, 0, TimeSpan.Zero);

    // C01: AC-01/02/03, including exact tick precision.
    [Theory]
    [InlineData(-10_000_000L, true)]
    [InlineData(-1L, true)]
    [InlineData(0L, false)]
    [InlineData(1L, false)]
    [InlineData(10_000_000L, false)]
    public void IsAllowed_AtCutoffBoundary_UsesStrictComparison(long validationOffsetTicks, bool expected)
    {
        var result = PredictionEditCutoff.IsAllowed(CreateMatch(), KickoffUtc.AddTicks(validationOffsetTicks));

        Assert.Equal(expected, result);
    }

    // C02: proposed technical null-input contract.
    [Fact]
    public void IsAllowed_WithNullMatch_ThrowsForMatch()
    {
        var exception = Assert.Throws<ArgumentNullException>(() =>
            PredictionEditCutoff.IsAllowed(null!, KickoffUtc.AddSeconds(-1)));

        Assert.Equal("match", exception.ParamName);
    }

    // C03: require the same zero-offset UTC convention as Match and Prediction.
    [Theory]
    [InlineData(-840)]
    [InlineData(-240)]
    [InlineData(1)]
    [InlineData(840)]
    public void IsAllowed_WithNonzeroValidationOffset_ThrowsForValidatedAtUtc(int offsetMinutes)
    {
        var validation = KickoffUtc.AddSeconds(-1).ToOffset(TimeSpan.FromMinutes(offsetMinutes));

        var exception = Assert.Throws<ArgumentException>(() =>
            PredictionEditCutoff.IsAllowed(CreateMatch(), validation));

        Assert.Equal("validatedAtUtc", exception.ParamName);
    }

    public static TheoryData<DateTimeOffset, DateTimeOffset, bool> DateBoundaries => new()
    {
        { DateTimeOffset.MinValue, DateTimeOffset.MinValue, false },
        { DateTimeOffset.MaxValue, DateTimeOffset.MaxValue, false },
        { DateTimeOffset.MaxValue, DateTimeOffset.MaxValue.AddTicks(-1), true },
        { DateTimeOffset.MinValue.AddTicks(1), DateTimeOffset.MinValue, true },
        { new DateTimeOffset(2027, 1, 1, 0, 0, 0, TimeSpan.Zero),
            new DateTimeOffset(2027, 1, 1, 0, 0, 0, TimeSpan.Zero).AddTicks(-1), true },
        { new DateTimeOffset(2027, 1, 1, 0, 0, 0, TimeSpan.Zero).AddTicks(-1),
            new DateTimeOffset(2027, 1, 1, 0, 0, 0, TimeSpan.Zero), false }
    };

    // C04: compare complete UTC instants without overflow or date-only truncation.
    [Theory]
    [MemberData(nameof(DateBoundaries))]
    public void IsAllowed_AtDateBoundaries_ComparesFullInstant(
        DateTimeOffset kickoffUtc, DateTimeOffset validatedAtUtc, bool expected)
    {
        Assert.Equal(expected, PredictionEditCutoff.IsAllowed(CreateMatch(kickoffUtc), validatedAtUtc));
    }

    // C05: match status must not introduce another restriction.
    [Theory]
    [InlineData(MatchStatus.Scheduled)]
    [InlineData(MatchStatus.InProgress)]
    [InlineData(MatchStatus.Finished)]
    [InlineData(MatchStatus.Cancelled)]
    public void IsAllowed_ForEveryMatchStatus_UsesScheduledTimeOnly(MatchStatus status)
    {
        var match = CreateMatch(status: status);

        Assert.True(PredictionEditCutoff.IsAllowed(match, KickoffUtc.AddTicks(-1)));
        Assert.False(PredictionEditCutoff.IsAllowed(match, KickoffUtc));
    }

    // C06: AC-04's captured instant controls each decision; later evaluation retains no state.
    [Fact]
    public void IsAllowed_ForIndependentAttempts_UsesEachSuppliedValidationInstant()
    {
        var match = CreateMatch();
        var beforeKickoff = KickoffUtc.AddSeconds(-1);

        var originalDecision = PredictionEditCutoff.IsAllowed(match, beforeKickoff);
        var laterAttempt = PredictionEditCutoff.IsAllowed(match, KickoffUtc.AddSeconds(1));
        var repeatedOriginalInstant = PredictionEditCutoff.IsAllowed(match, beforeKickoff);

        Assert.True(originalDecision);
        Assert.False(laterAttempt);
        Assert.True(repeatedOriginalInstant);
    }

    private static Match CreateMatch(DateTimeOffset? kickoffUtc = null, MatchStatus status = MatchStatus.Scheduled) =>
        new(new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "Home", "Away", kickoffUtc ?? KickoffUtc, status);
}
