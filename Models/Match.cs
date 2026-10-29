namespace GeographyGame.Backend.Models;

public enum MatchStatus
{
    Waiting,
    InProgress,
    Completed,
    Cancelled
}

public class Match
{
    public int Id { get; set; }

    public int Player1Id { get; set; }
    public int Player2Id { get; set; }

    public int Player1Score { get; set; }
    public int Player2Score { get; set; }
    public int? WinnerId { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? FinishedAt { get; set; }

    public MatchStatus Status { get; set; }
}

