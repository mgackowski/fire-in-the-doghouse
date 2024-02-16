using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/CardEffect/ComebackCardEffect")]
public class ComebackCardEffect : CardEffect
{
    public new string Name { get; } = "Comeback after Jab";

    public override void applyEffect(Card invoker, Act context)
    {
        if (context.DiscardPile.Count == 0)
        {
            MessageSystem.Push("It has no effect.", MessageType.SYSTEM);
            return;
        }
        foreach (CardEffect effect in context.DiscardPile.Peek().card.effects)
        {
            if (effect.GetType() == typeof(DebuffOpponentCardEffect))
            {
                context.CurrentPlayer.ResetBonus();
                MessageSystem.Push("Oof! That's gotta hurt.", MessageType.SYSTEM);

                // TODO: Act should provide a convenient method for this
                if (context.CurrentPlayer == context.HumanPlayer)
                {
                    context.CpuOpponent.SetBonus(-1);
                }
                else if (context.CurrentPlayer == context.CpuOpponent)
                {
                    context.HumanPlayer.SetBonus(-1);
                }
                return;
            }
        }
    }

}