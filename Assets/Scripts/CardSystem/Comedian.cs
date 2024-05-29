using System;

[Serializable]
public class Comedian
{
    public string ComedianName { get; private set; }
    public ComedianType Type { get; }
    public Character Model { get; private set; }
    public int Bonus { get; private set; } = 0;
    public int Score { get; private set; } = 0;

    public Comedian(string comedianName, ComedianType type, Character model = Character.PEANUT)
    {
        ComedianName = comedianName;
        Type = type;
        Model = model;
    }

    public void SetBonus(int bonus)
    {
        this.Bonus = bonus;
    }

    public void ResetBonus()
    {
        Bonus = 0;
    }

    public void AddToScore(int pointsToAdd)
    {
        Score += pointsToAdd;
    }

    public void ResetScore()
    {
        Score = 0;
    }

    public void ResetComedian()
    {
        ResetScore();
        ResetBonus();
    }

}
