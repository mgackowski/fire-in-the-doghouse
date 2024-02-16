using UnityEngine;

public abstract class CardScoringRule : ScriptableObject
{
    public abstract int GetBaseScore(Card invoker, Act context);

}