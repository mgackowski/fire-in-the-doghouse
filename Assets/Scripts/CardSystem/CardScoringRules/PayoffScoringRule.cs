using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ScoringRule/PayoffScoringRule")]
public class PayoffScoringRule : CardScoringRule
{
    [SerializeField] int scoreForMatchingStyle = 5;
    [SerializeField] int scoreForNonMatchingStyle = 2;
    [SerializeField] int scoreForMiss = 0;

    public override int GetBaseScore(CardPlay invoker, GameplayState context)
    {
        // Rules if different comedy styles are in play
        /*if (!context.SetupActive)
        {
            return scoreForMiss;
        }
        if (context.SetupType.Equals(invoker.card.style))
        {
            return scoreForMatchingStyle;
        }
        else
        {
            return scoreForNonMatchingStyle;
        }*/

        // Rules if comedy styles are not a mechanic
        if (!context.SetupActive || context.SetupPlayer != invoker.player)
        {
            return scoreForMiss;
        }
        else
        {
            return scoreForMatchingStyle;
        }

    }
}