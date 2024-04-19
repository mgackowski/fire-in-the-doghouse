using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ScoringRule/RandomScoringRule")]
public class RandomScoringRule : CardScoringRule
{

    public override int GetBaseScore(CardPlay invoker, GameplayState context)
    {
        System.Random rng = new System.Random();  // not optimal
        return rng.Next(0, 5);

    }
}