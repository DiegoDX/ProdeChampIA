using ProdeChamp.Domain.Predictions;

namespace ProdeChamp.UnitTests.Predictions;

[Trait("Story", "PRODE-004")]
public sealed class PredictionCanEditTests
{
    private static readonly DateTimeOffset KickoffUtc = new(2026, 6, 10, 15, 0, 0, TimeSpan.Zero);

    [Theory]
    [InlineData(-10_000_000L, true)]
    [InlineData(-1L, true)]
    [InlineData(0L, false)]
    [InlineData(1L, false)]
    [InlineData(10_000_000L, false)]
    public void CanEdit_AtCutoffBoundary_UsesStrictComparison(long validationOffsetTicks, bool expected)
    {
        var clock = new TestTimeProvider(KickoffUtc.AddTicks(validationOffsetTicks));

        Assert.Equal(expected, CreatePrediction().CanEdit(KickoffUtc, clock));
    }

    [Theory]
    [InlineData(-840)]
    [InlineData(-240)]
    [InlineData(-1)]
    [InlineData(1)]
    [InlineData(330)]
    [InlineData(840)]
    public void CanEdit_WithNonzeroKickoffOffset_ThrowsForMatchStartTimeUtc(int offsetMinutes)
    {
        var kickoff = KickoffUtc.ToOffset(TimeSpan.FromMinutes(offsetMinutes));

        var exception = Assert.Throws<ArgumentException>(() => CreatePrediction().CanEdit(kickoff));

        Assert.Equal("matchStartTimeUtc", exception.ParamName);
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

    [Theory]
    [MemberData(nameof(DateBoundaries))]
    public void CanEdit_AtDateBoundaries_ComparesFullInstant(
        DateTimeOffset kickoffUtc, DateTimeOffset serverTimeUtc, bool expected)
    {
        Assert.Equal(expected, CreatePrediction().CanEdit(kickoffUtc, new TestTimeProvider(serverTimeUtc)));
    }

    [Fact]
    public void CanEdit_WithPublicMethod_UsesCurrentServerTime()
    {
        var prediction = CreatePrediction();
        var serverTimeUtc = DateTimeOffset.UtcNow;

        Assert.True(prediction.CanEdit(serverTimeUtc.AddDays(1)));
        Assert.False(prediction.CanEdit(serverTimeUtc.AddDays(-1)));
    }

    [Fact]
    public void CanEdit_ForNewAttempts_ReadsCurrentTimeAndPreservesEarlierDecision()
    {
        var prediction = CreatePrediction();
        var clock = new TestTimeProvider(KickoffUtc.AddTicks(-1));

        var originalDecision = prediction.CanEdit(KickoffUtc, clock);
        clock.UtcNow = KickoffUtc;
        var atKickoffDecision = prediction.CanEdit(KickoffUtc, clock);
        clock.UtcNow = KickoffUtc.AddTicks(1);
        var laterDecision = prediction.CanEdit(KickoffUtc, clock);

        Assert.True(originalDecision);
        Assert.False(atKickoffDecision);
        Assert.False(laterDecision);
    }

    [Fact]
    public void CanEdit_WhenClockAdvancesDuringEvaluation_CapturesTimeOnce()
    {
        var clock = new TestTimeProvider(KickoffUtc.AddTicks(-1)) { AdvanceAfterRead = TimeSpan.FromSeconds(1) };

        Assert.True(CreatePrediction().CanEdit(KickoffUtc, clock));
        Assert.Equal(1, clock.ReadCount);
        Assert.True(clock.UtcNow > KickoffUtc);
    }

    [Fact]
    public void CanEdit_ForAllowedAndBlockedAttempts_DoesNotModifyPrediction()
    {
        var prediction = CreatePrediction();
        var before = (prediction.ParticipantId, prediction.MatchId, prediction.PredictedHomeGoals,
            prediction.PredictedAwayGoals, prediction.RegisteredAtUtc);
        var clock = new TestTimeProvider(KickoffUtc.AddTicks(-1));

        Assert.True(prediction.CanEdit(KickoffUtc, clock));
        clock.UtcNow = KickoffUtc;
        Assert.False(prediction.CanEdit(KickoffUtc, clock));

        Assert.Equal(before, (prediction.ParticipantId, prediction.MatchId, prediction.PredictedHomeGoals,
            prediction.PredictedAwayGoals, prediction.RegisteredAtUtc));
    }

    [Fact]
    public void CanEdit_WithNullTimeProvider_ThrowsForTimeProvider()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => CreatePrediction().CanEdit(KickoffUtc, null!));

        Assert.Equal("timeProvider", exception.ParamName);
    }

    private static Prediction CreatePrediction() => Prediction.Create(
        new Guid("11111111-1111-1111-1111-111111111111"),
        new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
        2, 1, KickoffUtc.AddHours(-1), []);

    private sealed class TestTimeProvider(DateTimeOffset utcNow) : TimeProvider
    {
        public DateTimeOffset UtcNow { get; set; } = utcNow;
        public TimeSpan AdvanceAfterRead { get; init; }
        public int ReadCount { get; private set; }

        public override DateTimeOffset GetUtcNow()
        {
            var capturedTime = UtcNow;
            ReadCount++;
            UtcNow += AdvanceAfterRead;
            return capturedTime;
        }
    }
}
