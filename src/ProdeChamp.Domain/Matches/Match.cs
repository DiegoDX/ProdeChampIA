namespace ProdeChamp.Domain.Matches;

// Compile-only TDD Red skeleton. Property assignment and validation are intentionally absent.
public sealed class Match
{
    public Match(Guid id, string homeTeam, string awayTeam, DateTimeOffset startTimeUtc, MatchStatus status)
    {
    }

    public Guid Id { get; }
    public string HomeTeam { get; } = null!;
    public string AwayTeam { get; } = null!;
    public DateTimeOffset StartTimeUtc { get; }
    public MatchStatus Status { get; }
}
