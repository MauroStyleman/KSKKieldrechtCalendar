namespace KSKKieldrecht1;

public class Match
{
    public string Id { get; set; }
    public DateTime StartTime { get; set; }
    public string Channel { get; set; }
    public Team HomeTeam { get; set; }
    public Team AwayTeam { get; set; }
    public Outcome Outcome { get; set; }
    public string State { get; set; }
}