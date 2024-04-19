using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ScoringRule/BasicScoringRule")]
public class BasicScoringRule : CardScoringRule
{
    public override int GetBaseScore(CardPlay invoker, GameplayState context)
    {
        return invoker.card.baseScore;
    }
}