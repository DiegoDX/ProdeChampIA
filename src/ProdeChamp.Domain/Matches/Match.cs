namespace ProdeChamp.Domain.Matches;

public sealed class Match
{
    public Match(Guid id, string homeTeam, string awayTeam, DateTimeOffset startTimeUtc, MatchStatus status)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Match ID must not be empty.", nameof(id));
        }

        if (startTimeUtc.Offset != TimeSpan.Zero)
        {
            throw new ArgumentException("Match start time must have a zero UTC offset.", nameof(startTimeUtc));
        }

        if (!Enum.IsDefined(status))
        {
            throw new ArgumentOutOfRangeException(nameof(status), status, "Match status must be a defined value.");
        }

        Id = id;
        HomeTeam = homeTeam;
        AwayTeam = awayTeam;
        StartTimeUtc = startTimeUtc;
        Status = status;
    }

    public Guid Id { get; }
    public string HomeTeam { get; }
    public string AwayTeam { get; }
    public DateTimeOffset StartTimeUtc { get; }
    public MatchStatus Status { get; }
}
