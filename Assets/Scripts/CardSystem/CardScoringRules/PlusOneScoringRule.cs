using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ScoringRule/PlusOneScoringRule")]
public class PlusOneScoringRule : CardScoringRule
{

    public override int GetBaseScore(Card invoker, Act context)
    {
        if (context.DiscardPile.Count == 0)
        {
            return 1;
        }
        else
        {
            return context.DiscardPile.Peek().effectiveScore + 1;
        }

    }
}