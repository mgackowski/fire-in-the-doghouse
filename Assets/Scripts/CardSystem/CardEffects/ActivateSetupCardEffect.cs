using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/CardEffect/ActivateSetupCardEffect")]
public class ActivateSetupCardEffect : CardEffect
{
    public new string Name { get; } = "Activate Setup";

    public override void applyEffect(Card invoker, Act context)
    {
        context.ActivateSetup(invoker.style, context.CurrentPlayer);
        MessageSystem.Push("A joke is being set up...", MessageType.SYSTEM);
    }
}