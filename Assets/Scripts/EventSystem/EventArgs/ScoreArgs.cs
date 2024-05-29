/**
 * By <author>mgackowski</author>.
 */

public class ScoreArgs : IEventArgs
{
    public Comedian TurnPlayer { get; set; }
    public int TurnScore { get; set; }
    public int TotalScore {  get; set; }
}
