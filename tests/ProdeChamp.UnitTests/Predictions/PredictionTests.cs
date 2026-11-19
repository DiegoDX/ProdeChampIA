using ProdeChamp.Domain.Predictions;

namespace ProdeChamp.UnitTests.Predictions;

[Trait("Story", "PRODE-003")]
public sealed class PredictionTests
{
    private static readonly Guid ParticipantA = new("11111111-1111-1111-1111-111111111111");
    private static readonly Guid ParticipantB = new("22222222-2222-2222-2222-222222222222");
    private static readonly Guid MatchAId = new("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
    private static readonly Guid MatchBId = new("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");
    private static readonly DateTimeOffset RegisteredAtUtc =
        new DateTimeOffset(2026, 6, 10, 12, 34, 56, TimeSpan.Zero).AddTicks(1_234_567);

    // P01: AC-01/05 and BR6. Time and existing active predictions are explicit inputs.
    [Fact]
    public void Create_WithValidInputs_PreservesAssociationsScoreAndUtcTimestamp()
    {
        var matchId = MatchAId;

        var prediction = Prediction.Create(ParticipantA, matchId, 2, 1, RegisteredAtUtc, []);

        Assert.Equal(ParticipantA, prediction.ParticipantId);
        Assert.Equal(MatchAId, prediction.MatchId);
        Assert.Equal(2, prediction.PredictedHomeGoals);
        Assert.Equal(1, prediction.PredictedAwayGoals);
        Assert.Equal(RegisteredAtUtc.Ticks, prediction.RegisteredAtUtc.Ticks);
        Assert.Equal(TimeSpan.Zero, prediction.RegisteredAtUtc.Offset);
    }

    // P03: AC-03, explicitly approved null participant rejection.
    [Fact]
    public void Create_WithNullParticipantId_ThrowsForParticipantId()
    {
        var exception = Assert.Throws<ArgumentNullException>(() =>
            Prediction.Create(null, MatchAId, 2, 1, RegisteredAtUtc, []));

        Assert.Equal("participantId", exception.ParamName);
    }

    // P04: AC-03.
    [Fact]
    public void Create_WithEmptyParticipantId_ThrowsForParticipantId()
    {
        var exception = Assert.Throws<ArgumentException>(() =>
            Prediction.Create(Guid.Empty, MatchAId, 2, 1, RegisteredAtUtc, []));

        Assert.Equal("participantId", exception.ParamName);
    }

    // P06: the Guid contract now owns empty match ID rejection.
    [Fact]
    public void Create_WithEmptyMatchId_ThrowsForMatchId()
    {
        var exception = Assert.Throws<ArgumentException>(() =>
            Prediction.Create(ParticipantA, Guid.Empty, 2, 1, RegisteredAtUtc, []));

        Assert.Equal("matchId", exception.ParamName);
    }

    // P07: AC-02, independent negative home values.
    [Theory]
    [InlineData(-1)]
    [InlineData(int.MinValue)]
    public void Create_WithNegativeHomeGoals_ThrowsForHomeGoals(int homeGoals)
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            Prediction.Create(ParticipantA, MatchAId, homeGoals, 1, RegisteredAtUtc, []));

        Assert.Equal("predictedHomeGoals", exception.ParamName);
    }

    // P08: AC-02, independent negative away values.
    [Theory]
    [InlineData(-1)]
    [InlineData(int.MinValue)]
    public void Create_WithNegativeAwayGoals_ThrowsForAwayGoals(int awayGoals)
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            Prediction.Create(ParticipantA, MatchAId, 2, awayGoals, RegisteredAtUtc, []));

        Assert.Equal("predictedAwayGoals", exception.ParamName);
    }

    // P10: AC-05, reject nonzero offsets even for an equivalent instant.
    [Theory]
    [InlineData(-840)]
    [InlineData(-240)]
    [InlineData(-1)]
    [InlineData(1)]
    [InlineData(330)]
    [InlineData(840)]
    public void Create_WithNonzeroRegistrationOffset_ThrowsForRegisteredAtUtc(int offsetMinutes)
    {
        var registrationTime = RegisteredAtUtc.ToOffset(TimeSpan.FromMinutes(offsetMinutes));

        var exception = Assert.Throws<ArgumentException>(() =>
            Prediction.Create(ParticipantA, MatchAId, 2, 1, registrationTime, []));

        Assert.Equal("registeredAtUtc", exception.ParamName);
    }

    // P11: missing caller context must not silently disable duplicate checking.
    [Fact]
    public void Create_WithNullActivePredictions_ThrowsForActivePredictions()
    {
        var exception = Assert.Throws<ArgumentNullException>(() =>
            Prediction.Create(ParticipantA, MatchAId, 2, 1, RegisteredAtUtc, null!));

        Assert.Equal("activePredictions", exception.ParamName);
    }

    // P12: reject malformed context; the valid entry is unrelated to the candidate.
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void Create_WithNullContextEntry_ThrowsForActivePredictions(bool validEntryFirst)
    {
        IReadOnlyCollection<Prediction> activePredictions = validEntryFirst
            ? [Prediction.Create(ParticipantB, MatchBId, 0, 0, RegisteredAtUtc, []), null!]
            : [null!];

        var exception = Assert.Throws<ArgumentException>(() =>
            Prediction.Create(ParticipantA, MatchAId, 2, 1, RegisteredAtUtc, activePredictions));

        Assert.Equal("activePredictions", exception.ParamName);
    }

    // P13: BR5, scores do not change participant/match ID pair identity.
    [Theory]
    [InlineData(2, 1)]
    [InlineData(0, 0)]
    public void Create_WithDuplicatePair_ThrowsRegardlessOfScore(int existingHome, int existingAway)
    {
        var existing = Prediction.Create(ParticipantA, MatchAId, existingHome, existingAway, RegisteredAtUtc, []);

        Assert.Throws<InvalidOperationException>(() =>
            Prediction.Create(ParticipantA, MatchAId, 2, 1, RegisteredAtUtc, [existing]));
    }

    // P14: BR5, do not stop after the first unrelated entry.
    [Fact]
    public void Create_WithDuplicateAfterUnrelatedEntries_Throws()
    {
        IReadOnlyCollection<Prediction> activePredictions =
        [
            Prediction.Create(ParticipantA, MatchBId, 0, 0, RegisteredAtUtc, []),
            Prediction.Create(ParticipantB, MatchAId, 0, 0, RegisteredAtUtc, []),
            Prediction.Create(ParticipantA, MatchAId, 0, 0, RegisteredAtUtc, [])
        ];

        Assert.Throws<InvalidOperationException>(() =>
            Prediction.Create(ParticipantA, MatchAId, 2, 1, RegisteredAtUtc, activePredictions));
    }

    // P15: BR5, a different match permits the same participant.
    [Fact]
    public void Create_WithSameParticipantAndDifferentMatch_Succeeds()
    {
        var existing = Prediction.Create(ParticipantA, MatchAId, 0, 0, RegisteredAtUtc, []);

        var prediction = Prediction.Create(ParticipantA, MatchBId, 2, 1, RegisteredAtUtc, [existing]);

        Assert.Equal(ParticipantA, prediction.ParticipantId);
        Assert.Equal(MatchBId, prediction.MatchId);
    }

    // P16: BR5, a different participant permits the same match.
    [Fact]
    public void Create_WithDifferentParticipantAndSameMatch_Succeeds()
    {
        var existing = Prediction.Create(ParticipantA, MatchAId, 0, 0, RegisteredAtUtc, []);

        var prediction = Prediction.Create(ParticipantB, MatchAId, 2, 1, RegisteredAtUtc, [existing]);

        Assert.Equal(ParticipantB, prediction.ParticipantId);
        Assert.Equal(MatchAId, prediction.MatchId);
    }

    // P17: the caller retains ownership of its collection on success and failure.
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void Create_WithCallerOwnedList_DoesNotModifyIt(bool duplicate)
    {
        var existing = Prediction.Create(ParticipantA, MatchAId, 0, 0, RegisteredAtUtc, []);
        var activePredictions = new List<Prediction> { existing };
        var before = activePredictions.ToArray();

        if (duplicate)
        {
            Assert.Throws<InvalidOperationException>(() =>
                Prediction.Create(ParticipantA, MatchAId, 2, 1, RegisteredAtUtc, activePredictions));
        }
        else
        {
            var prediction = Prediction.Create(ParticipantB, MatchAId, 2, 1, RegisteredAtUtc, activePredictions);
            Assert.Equal(ParticipantB, prediction.ParticipantId);
        }

        Assert.Equal(before.Length, activePredictions.Count);
        for (var index = 0; index < before.Length; index++)
        {
            Assert.Same(before[index], activePredictions[index]);
        }
    }

    // P02: AC-01/02, zero and the full nonnegative integer range are valid.
    [Theory]
    [InlineData(0, 0)]
    [InlineData(0, 2)]
    [InlineData(2, 0)]
    [InlineData(int.MaxValue, 0)]
    [InlineData(0, int.MaxValue)]
    [InlineData(int.MaxValue, int.MaxValue)]
    public void Create_WithNonnegativeGoalBoundaries_PreservesGoals(int homeGoals, int awayGoals)
    {
        var prediction = Prediction.Create(ParticipantA, MatchAId, homeGoals, awayGoals, RegisteredAtUtc, []);

        Assert.Equal(homeGoals, prediction.PredictedHomeGoals);
        Assert.Equal(awayGoals, prediction.PredictedAwayGoals);
    }

    // P05: AC-03, no additional GUID version/range rule.
    [Theory]
    [InlineData("00000000-0000-0000-0000-000000000001")]
    [InlineData("ffffffff-ffff-ffff-ffff-ffffffffffff")]
    public void Create_WithNonemptyParticipantGuidBoundaries_PreservesId(string idText)
    {
        var participantId = Guid.Parse(idText);

        var prediction = Prediction.Create(participantId, MatchAId, 2, 1, RegisteredAtUtc, []);

        Assert.Equal(participantId, prediction.ParticipantId);
    }

    public static TheoryData<DateTimeOffset> UtcRegistrationTimes => new()
    {
        DateTimeOffset.MinValue,
        DateTimeOffset.MinValue.AddTicks(1),
        new DateTimeOffset(2000, 2, 29, 23, 59, 59, TimeSpan.Zero).AddTicks(1),
        DateTimeOffset.MaxValue.AddTicks(-1),
        DateTimeOffset.MaxValue
    };

    // P09: AC-05, preserve precision without inventing a future/past-time rule.
    [Theory]
    [MemberData(nameof(UtcRegistrationTimes))]
    public void Create_WithUtcDateBoundaries_PreservesExactTimestamp(DateTimeOffset registeredAtUtc)
    {
        var prediction = Prediction.Create(ParticipantA, MatchAId, 2, 1, registeredAtUtc, []);

        Assert.Equal(registeredAtUtc.Ticks, prediction.RegisteredAtUtc.Ticks);
        Assert.Equal(TimeSpan.Zero, prediction.RegisteredAtUtc.Offset);
    }

    // P18: no registry; duplicate checking depends only on caller-provided context.
    [Fact]
    public void Create_RepeatedWithUnchangedEmptyContext_DoesNotRegisterHiddenState()
    {
        var activePredictions = new List<Prediction>();
        var matchId = MatchAId;

        var first = Prediction.Create(ParticipantA, matchId, 2, 1, RegisteredAtUtc, activePredictions);
        var second = Prediction.Create(ParticipantA, matchId, 2, 1, RegisteredAtUtc, activePredictions);

        Assert.NotSame(first, second);
        Assert.Equal(first.ParticipantId, second.ParticipantId);
        Assert.Equal(first.MatchId, second.MatchId);
        Assert.Equal(first.PredictedHomeGoals, second.PredictedHomeGoals);
        Assert.Equal(first.PredictedAwayGoals, second.PredictedAwayGoals);
        Assert.Equal(first.RegisteredAtUtc, second.RegisteredAtUtc);
        Assert.Empty(activePredictions);
    }

    // P19: caller context changes and independent creations preserve earlier values.
    [Fact]
    public void Create_WhenCallerContextChanges_PreservesIndependentPredictionValues()
    {
        var activePredictions = new List<Prediction>();
        var first = Prediction.Create(ParticipantA, MatchAId, 2, 1, RegisteredAtUtc, activePredictions);
        activePredictions.Add(first);
        var later = RegisteredAtUtc.AddMinutes(1);

        var second = Prediction.Create(ParticipantB, MatchBId, 4, 3, later, activePredictions);

        Assert.Equal(ParticipantA, first.ParticipantId);
        Assert.Equal(MatchAId, first.MatchId);
        Assert.Equal(2, first.PredictedHomeGoals);
        Assert.Equal(1, first.PredictedAwayGoals);
        Assert.Equal(RegisteredAtUtc.Ticks, first.RegisteredAtUtc.Ticks);
        Assert.Equal(ParticipantB, second.ParticipantId);
        Assert.Equal(MatchBId, second.MatchId);
        Assert.Equal(4, second.PredictedHomeGoals);
        Assert.Equal(3, second.PredictedAwayGoals);
        Assert.Equal(later.Ticks, second.RegisteredAtUtc.Ticks);
    }

    // P20: accept any nonempty match Guid without requiring a Match object or catalog.
    [Theory]
    [InlineData("00000000-0000-0000-0000-000000000001")]
    [InlineData("ffffffff-ffff-ffff-ffff-ffffffffffff")]
    public void Create_WithNonemptyMatchGuidBoundaries_PreservesId(string idText)
    {
        var matchId = Guid.Parse(idText);

        var prediction = Prediction.Create(ParticipantA, matchId, 2, 1, RegisteredAtUtc, []);

        Assert.Equal(matchId, prediction.MatchId);
    }
}
