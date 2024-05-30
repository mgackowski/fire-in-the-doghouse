using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/CardEffect/ActivateSetupCardEffect")]
public class ActivateSetupCardEffect : CardEffect
{
    public override string Name { get; } = "Activate Setup";

    public override void applyEffect(CardPlay invoker, GameplayState context)
    {
        context.ActivateSetup(invoker.player, invoker.card.style);
        MessageSystem.Push("A joke is being set up...", MessageType.SYSTEM);
    }
}