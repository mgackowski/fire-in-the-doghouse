using UnityEngine;

public abstract class CardScoringRule : ScriptableObject
{
    public abstract int GetBaseScore(CardPlay invoker, GameplayState gameplayState);

}