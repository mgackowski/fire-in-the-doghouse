using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ScoringRule/PlusOneScoringRule")]
public class PlusOneScoringRule : CardScoringRule
{
    [SerializeField] int bonusScore = 1;

    public override int GetBaseScore(CardPlay invoker, GameplayState context)
    {
        if (context.DiscardPile.Count == 0)
        {
            return bonusScore;
        }
        else
        {
            return context.DiscardPile.Peek().effectiveScore + bonusScore;
        }

    }
}