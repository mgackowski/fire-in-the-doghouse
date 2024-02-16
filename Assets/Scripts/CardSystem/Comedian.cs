using System;

[Serializable]
public class Comedian
{
    //public event Action BonusChanged;
    //public event Action ScoreChanged;

    public string ComedianName { get; private set; }
    public ComedianType Type { get; }
    public int Bonus { get; private set; } = 0;
    public int Score { get; private set; } = 0;

    public Comedian(string comedianName, ComedianType type)
    {
        ComedianName = comedianName;
        Type = type;
    }

    public void SetBonus(int bonus)
    {
        this.Bonus = bonus;
        //BonusChanged.Invoke();
    }

    public void ResetBonus()
    {
        Bonus = 0;
        //BonusChanged.Invoke();
    }

    public void AddToScore(int pointsToAdd)
    {
        Score += pointsToAdd;
        //ScoreChanged.Invoke();
    }

    public void ResetScore()
    {
        Score = 0;
        //ScoreChanged.Invoke();
    }

    public void ResetComedian()
    {
        ResetScore();
        ResetBonus();
    }

}
