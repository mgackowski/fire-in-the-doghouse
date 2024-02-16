using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/CardEffect/DebuffOpponentCardEffect")]
public class DebuffOpponentCardEffect : CardEffect
{
    public new string Name { get; } = "Jab Opponent";

    public override void applyEffect(Card invoker, Act context)
    {
        MessageSystem.Push("That's affected their focus a bit.", MessageType.SYSTEM);
        if (context.CurrentPlayer == context.HumanPlayer)
        {
            context.CpuOpponent.SetBonus(-1);  //TODO: might interfere with other bonuses
        }
        else if (context.CurrentPlayer == context.CpuOpponent)
        {
            context.HumanPlayer.SetBonus(-1);
        }
    }
}