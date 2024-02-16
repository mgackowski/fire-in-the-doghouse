using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ScoringRule/ComebackScoringRule")]
public class ComebackScoringRule : CardScoringRule
{

    public override int GetBaseScore(Card invoker, Act context)
    {
        if (context.DiscardPile.Count == 0)
        {
            return 0;
        }

        foreach(CardEffect effect in context.DiscardPile.Peek().card.effects)
        {
            if (effect.GetType() == typeof(DebuffOpponentCardEffect))
            {
                return 6;
            }
        }
        return 0;
    }
}