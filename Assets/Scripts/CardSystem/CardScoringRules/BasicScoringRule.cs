using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ScoringRule/BasicScoringRule")]
public class BasicScoringRule : CardScoringRule
{
    public override int GetBaseScore(Card invoker, Act context)
    {
        return invoker.baseScore;
    }
}