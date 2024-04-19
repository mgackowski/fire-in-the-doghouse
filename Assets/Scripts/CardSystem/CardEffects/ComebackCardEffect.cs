using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/CardEffect/ComebackCardEffect")]
public class ComebackCardEffect : CardEffect
{
    public new string Name { get; } = "Comeback after Jab";

    public override void applyEffect(CardPlay invoker, GameplayState context)
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
                invoker.player.ResetBonus();
                MessageSystem.Push("Oof! That's gotta hurt.", MessageType.SYSTEM);

                if (invoker.player == context.HumanComedian)
                {
                    context.CpuComedian.SetBonus(-1);
                }
                else if (invoker.player == context.CpuComedian)
                {
                    context.HumanComedian.SetBonus(-1);
                }
                return;
            }
        }
    }

}