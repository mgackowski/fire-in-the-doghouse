using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Card")]
public class Card : ScriptableObject
{
    public string cardName;
    public string description;
    public Sprite graphic;
    public int baseScore;
    public ComedyStyle style;
    [SerializeReference] public List<CardEffect> effects;
    public CardScoringRule scoringRule;

    public List<string> dialogueLines;

}