namespace FSH.Starter.AdjdDashboard.Application.Common.DTOs;

public record AlertCountsDto
{
    public int MissingEntry { get; init; }
    public int MissingExit { get; init; }
    public int DoubleEntry { get; init; }
    public int DoubleExit { get; init; }
    public int Away { get; init; }
}
