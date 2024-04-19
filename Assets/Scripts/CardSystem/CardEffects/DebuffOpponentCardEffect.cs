using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/CardEffect/DebuffOpponentCardEffect")]
public class DebuffOpponentCardEffect : CardEffect
{
    public new string Name { get; } = "Jab Opponent";

    public override void applyEffect(CardPlay invoker, GameplayState context)
    {
        MessageSystem.Push("That's affected their focus a bit.", MessageType.SYSTEM);
        if (invoker.player == context.HumanComedian)
        {
            context.CpuComedian.SetBonus(-1);  //TODO: might interfere with other bonuses
        }
        else if (invoker.player == context.CpuComedian)
        {
            context.HumanComedian.SetBonus(-1);
        }
    }
}