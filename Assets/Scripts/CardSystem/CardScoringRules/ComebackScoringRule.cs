using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ScoringRule/ComebackScoringRule")]
public class ComebackScoringRule : CardScoringRule
{
    [SerializeField] int scoreIfSuccessful = 6;
    [SerializeField] int scoreIfUnsuccessful = 0;

    public override int GetBaseScore(CardPlay invoker, GameplayState context)
    {
        if (context.DiscardPile.Count == 0)
        {
            return scoreIfUnsuccessful;
        }

        foreach(CardEffect effect in context.DiscardPile.Peek().card.effects)
        {
            if (effect.GetType() == typeof(DebuffOpponentCardEffect))
            {
                return scoreIfSuccessful;
            }
        }
        return 0;
    }
}