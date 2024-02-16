using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ScoringRule/PunchUpScoringRule")]
public class PunchUpScoringRule : CardScoringRule
{

    public override int GetBaseScore(Card invoker, Act context)
    {
        Comedian currentPlayer = context.CurrentPlayer;
        Comedian opponent;
        if (currentPlayer == context.CpuOpponent)
        {
            opponent = context.HumanPlayer;
        }
        else
        {
            opponent = context.CpuOpponent;
        }

        if (currentPlayer.Score >= opponent.Score)
        {
            return 1;
        }
        else
        {
            return 4;
        }

    }
}