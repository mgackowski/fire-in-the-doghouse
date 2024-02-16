using UnityEngine;

[System.Serializable]
public abstract class CardEffect : ScriptableObject
{
    public string Name { get; }
    public abstract void applyEffect(Card invoker, Act context);
    
}