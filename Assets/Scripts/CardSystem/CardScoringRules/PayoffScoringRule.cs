using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ScoringRule/PayoffScoringRule")]
public class PayoffScoringRule : CardScoringRule
{

    public override int GetBaseScore(Card invoker, Act context)
    {
        if (!context.SetupActive)
        {
            return 0;
        }
        if (context.SetupType.Equals(invoker.style))
        {
            return 5;
        }
        else
        {
            return 2;
        }
        
    }
}