using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/CardEffect/RemoveSetupCardEffect")]
public class RemoveSetupCardEffect : CardEffect
{
    public new string Name { get; } = "Remove Setup";

    public override void applyEffect(Card invoker, Act context)
    {
        MessageSystem.Push("We're done with that last setup.", MessageType.SYSTEM);
        context.ResetSetup();
    }
}