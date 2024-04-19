using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ScoringRule/PunchUpScoringRule")]
public class PunchUpScoringRule : CardScoringRule
{

    [SerializeField] int scoreIfSuccess = 4;
    [SerializeField] int scoreIfFailure = 1;

    public override int GetBaseScore(CardPlay invoker, GameplayState context)
    {
        Comedian currentPlayer = invoker.player;
        Comedian opponent;
        if (currentPlayer == context.CpuComedian)
        {
            opponent = context.HumanComedian;
        }
        else
        {
            opponent = context.CpuComedian;
        }

        if (currentPlayer.Score >= opponent.Score)
        {
            return scoreIfFailure;
        }
        else
        {
            return scoreIfSuccess;
        }

    }
}