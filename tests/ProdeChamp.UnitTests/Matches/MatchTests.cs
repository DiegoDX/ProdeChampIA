using ProdeChamp.Domain.Matches;

namespace ProdeChamp.UnitTests.Matches;

[Trait("Story", "PRODE-002")]
public sealed class MatchTests
{
    private static readonly Guid MatchId = new("a2b21cb2-aef7-401b-8549-bcb8aa53ec1a");
    private static readonly DateTimeOffset Kickoff = new(2026, 6, 11, 19, 0, 0, TimeSpan.Zero);

    // M01: AC1, AC2 and P1. All state is supplied explicitly by the caller.
    [Fact]
    public void Constructor_WithValidInputs_PreservesAllProperties()
    {
        var match = new Match(MatchId, "Argentina", "Bolivia", Kickoff, MatchStatus.Scheduled);

        Assert.Equal(MatchId, match.Id);
        Assert.Equal("Argentina", match.HomeTeam);
        Assert.Equal("Bolivia", match.AwayTeam);
        Assert.Equal(Kickoff, match.StartTimeUtc);
        Assert.Equal(TimeSpan.Zero, match.StartTimeUtc.Offset);
        Assert.Equal(MatchStatus.Scheduled, match.Status);
    }

    // M04: preserve raw strings and home/away order. AC3 and team validation are deferred.
    [Theory]
    [InlineData("  ArGenTina ", "\tBOLIVIA  ")]
    [InlineData("\tBOLIVIA  ", "  ArGenTina ")]
    public void Constructor_WithRawTeamValues_PreservesValuesAndRoles(string homeTeam, string awayTeam)
    {
        var match = new Match(MatchId, homeTeam, awayTeam, Kickoff, MatchStatus.Scheduled);

        Assert.Equal(homeTeam, match.HomeTeam);
        Assert.Equal(awayTeam, match.AwayTeam);
    }

    // M05: AC1 and P2. Assert parameter names rather than localized messages.
    [Fact]
    public void Constructor_WithEmptyId_ThrowsArgumentExceptionForId()
    {
        var exception = Assert.Throws<ArgumentException>(() =>
            new Match(Guid.Empty, "Argentina", "Bolivia", Kickoff, MatchStatus.Scheduled));

        Assert.Equal("id", exception.ParamName);
    }

    // M14: no GUID version or arbitrary numeric range is required.
    [Theory]
    [InlineData("00000000-0000-0000-0000-000000000001")]
    [InlineData("ffffffff-ffff-ffff-ffff-ffffffffffff")]
    public void Constructor_WithNonemptyGuidBoundaries_PreservesId(string idText)
    {
        var id = Guid.Parse(idText);

        var match = new Match(id, "Argentina", "Bolivia", Kickoff, MatchStatus.Scheduled);

        Assert.Equal(id, match.Id);
    }

    // M08: AC2 and P2. These equivalent instants still have invalid nonzero offsets.
    [Theory]
    [InlineData(-840)]
    [InlineData(-240)]
    [InlineData(-1)]
    [InlineData(1)]
    [InlineData(330)]
    [InlineData(345)]
    [InlineData(840)]
    public void Constructor_WithNonzeroOffset_ThrowsArgumentExceptionForStartTimeUtc(int offsetMinutes)
    {
        var startTime = Kickoff.ToOffset(TimeSpan.FromMinutes(offsetMinutes));

        var exception = Assert.Throws<ArgumentException>(() =>
            new Match(MatchId, "Argentina", "Bolivia", startTime, MatchStatus.Scheduled));

        Assert.Equal("startTimeUtc", exception.ParamName);
    }

    public static TheoryData<DateTimeOffset> UtcStartTimes => new()
    {
        // default is also DateTimeOffset.MinValue; cover this boundary once.
        default(DateTimeOffset),
        DateTimeOffset.MinValue.AddTicks(1),
        new DateTimeOffset(1960, 1, 1, 0, 0, 0, TimeSpan.Zero),
        new DateTimeOffset(2000, 2, 29, 23, 59, 59, TimeSpan.Zero),
        new DateTimeOffset(2099, 12, 31, 23, 59, 59, TimeSpan.Zero),
        DateTimeOffset.MaxValue.AddTicks(-1),
        DateTimeOffset.MaxValue
    };

    // M09: AC2. No clock, future-only constraint or undocumented precision limit.
    [Theory]
    [MemberData(nameof(UtcStartTimes))]
    public void Constructor_WithUtcDateBoundaries_PreservesExactStartTime(DateTimeOffset startTimeUtc)
    {
        var match = new Match(MatchId, "Argentina", "Bolivia", startTimeUtc, MatchStatus.Scheduled);

        Assert.Equal(startTimeUtc, match.StartTimeUtc);
        Assert.Equal(TimeSpan.Zero, match.StartTimeUtc.Offset);
    }

    // M10: P1. There is no Scheduled-only initial-state policy.
    [Theory]
    [InlineData(MatchStatus.Scheduled)]
    [InlineData(MatchStatus.InProgress)]
    [InlineData(MatchStatus.Finished)]
    [InlineData(MatchStatus.Cancelled)]
    public void Constructor_WithEachDefinedStatus_PreservesStatus(MatchStatus status)
    {
        var match = new Match(MatchId, "Argentina", "Bolivia", Kickoff, status);

        Assert.Equal(status, match.Status);
    }

    // M11: P1 and P2. Adjacent undefined values and both int boundaries.
    [Theory]
    [InlineData(-1)]
    [InlineData(4)]
    [InlineData(int.MinValue)]
    [InlineData(int.MaxValue)]
    public void Constructor_WithUndefinedStatus_ThrowsArgumentOutOfRangeExceptionForStatus(int statusValue)
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            new Match(MatchId, "Argentina", "Bolivia", Kickoff, (MatchStatus)statusValue));

        Assert.Equal("status", exception.ParamName);
    }

    // M12: P2. Reject each combination without prescribing guard order.
    [Theory]
    [InlineData(true, 60, 0)]
    [InlineData(true, 0, -1)]
    [InlineData(false, -60, -1)]
    [InlineData(true, 60, -1)]
    public void Constructor_WithMultipleInvalidNonTeamInputs_RejectsWithoutRequiringGuardOrder(
        bool emptyId, int offsetMinutes, int statusValue)
    {
        var id = emptyId ? Guid.Empty : MatchId;
        var startTime = Kickoff.ToOffset(TimeSpan.FromMinutes(offsetMinutes));

        Assert.ThrowsAny<ArgumentException>(() =>
            new Match(id, "Argentina", "Bolivia", startTime, (MatchStatus)statusValue));
    }

    // M13: P3. Duplicate IDs need no lookup; separate instances retain their own state.
    [Fact]
    public void Constructor_WhenIdIsReused_CreatesIndependentMatchesWithoutExternalState()
    {
        var first = new Match(MatchId, "Argentina", "Bolivia", Kickoff, MatchStatus.Scheduled);
        var otherKickoff = Kickoff.AddDays(1);

        var second = new Match(MatchId, "Uruguay", "Chile", otherKickoff, MatchStatus.Finished);

        Assert.NotSame(first, second);
        Assert.Equal(MatchId, first.Id);
        Assert.Equal(MatchId, second.Id);
        Assert.Equal("Argentina", first.HomeTeam);
        Assert.Equal("Bolivia", first.AwayTeam);
        Assert.Equal(Kickoff, first.StartTimeUtc);
        Assert.Equal(MatchStatus.Scheduled, first.Status);
        Assert.Equal("Uruguay", second.HomeTeam);
        Assert.Equal("Chile", second.AwayTeam);
        Assert.Equal(otherKickoff, second.StartTimeUtc);
        Assert.Equal(MatchStatus.Finished, second.Status);
    }

    // M15: public setters/init accessors must not bypass constructor invariants.
    [Theory]
    [InlineData(nameof(Match.Id))]
    [InlineData(nameof(Match.HomeTeam))]
    [InlineData(nameof(Match.AwayTeam))]
    [InlineData(nameof(Match.StartTimeUtc))]
    [InlineData(nameof(Match.Status))]
    public void PublicState_HasNoPublicSetterOrInitAccessor(string propertyName)
    {
        var property = typeof(Match).GetProperty(propertyName);

        Assert.NotNull(property);
        Assert.NotNull(property.GetGetMethod());
        Assert.Null(property.GetSetMethod());
    }

    // M16: numeric values are internal, not a wire or storage contract.
    [Fact]
    public void MatchStatus_DefinesExactlyTheFourApprovedValues()
    {
        Assert.Equal(
            new[] { "Scheduled", "InProgress", "Finished", "Cancelled" },
            Enum.GetNames<MatchStatus>());
        Assert.Equal(new[] { 0, 1, 2, 3 }, Enum.GetValues<MatchStatus>().Select(value => (int)value));
    }
}
