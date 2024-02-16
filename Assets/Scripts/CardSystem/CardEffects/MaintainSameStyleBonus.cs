using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/CardEffect/MaintainSameStyleBonus")]
public class MaintainSameStyleBonus : CardEffect
{
    public new string Name { get; } = "Maintain style bonus";

    public override void applyEffect(Card invoker, Act context)
    {
        if (context.DiscardPile.Count == 0)
        {
            MessageSystem.Push("That's an odd way to start.", MessageType.SYSTEM);
            return;
        }
        if (context.DiscardPile.Peek().card.style.Equals(invoker.style))
        {
            //
        }
        else
        {
            context.CurrentPlayer.AddToScore(1);
            MessageSystem.Push("That's a smooth transition.", MessageType.SYSTEM);
        }
    }
}