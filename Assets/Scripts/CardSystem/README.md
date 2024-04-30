# CardSystem

In the concept for the game, each player (`Comedian`) chooses a number of `Card`s from their `Deck`, which get queued. Then, when the "act" resolves, each `CardPlay` is resolved, and the players are scored accordingly.

All `Card`s have a base score value, but some have special `CardScoringRule`s, and many have special `CardEffect`. In order to affect the state of the game, those rules accept a reference to the current [`GameplayState`](../GameplayState.cs) (outside this directory).

Apart from `GameplayState` being a key dependency, `Card`s, `Deck`s, `CardEffect`s and `CardScoringRule`s are all `ScriptableObjects`. This allows them to be serialized, easily created and customised via the Inspector in the Unity engine.

## Example usage

This is what the `ActCoordinator` calls when the card effect resolution phase starts. 'currentPlay' being the latest `CardPlay` being resolved, and 'state' holding a reference to the current `GameplayState` (player scores, discard pile, etc.) 
```
public void StartEffectResolution()
{
	actState = ActState.EffectResolutionStarted;
	foreach (CardEffect effect in currentPlay.card.effects)
	{
		effect.applyEffect(currentPlay, state);
	}
	//publish event etc. (code omitted)
}
```

Here is what a scoring rule implementation for a 'Payoff' card looks like. This play awards extra points if a 'Setup' card was played, and rewards playing cards of the same suit ("style").

```
    public override int GetBaseScore(CardPlay invoker, GameplayState context)
    {
        if (!context.SetupActive)
        {
            return scoreForMiss;
        }
        if (context.SetupType.Equals(invoker.card.style))
        {
            return scoreForMatchingStyle;
        }
        else
        {
            return scoreForNonMatchingStyle;
        }
    }
```

Note that in the above, this scoring rule is a `ScriptableObject` with exposed parameters for `scoreForMiss`, `scoreForMatchingStyle` and `scoreForNonMatchingStyle` values. So just by using the Unity inspector, one can create many variants of this scoring rule, and attach them to new cards.

The card system being broken down into individual elements like this makes the logic in each rather simple and easy to follow.

## Components

### [ActCoordinator]()

[!NOTE]
This is arguably not part of the system, but a client of it. It might be split into two classes in future revisions.

Ensures that the events during a comedy act transpire in order, including the switching of turns, card plays, effect and score resolution. This is the only `MonoBehaviour` as it is a useful front-end for the act's overall behaviour during development.

### [Card](Card.cs)

Has a name, description, graphic, base Score, style, a list of `CardEffect`s and a `CardScoringRule`. Also contains a list of dialogue lines for the comedians for use with the [`DialogueGenerator`](../DialogueGenerator.cs).

### [CardPlay](CardPlay.cs)

Encapsulates information about the card, its player, and how it was scored. This enriches the `Card` with some useful context when stored in `GameplayState`'s card queue or discard pile.

### [Comedian](Comedian.cs)

Represents a player; tracks their name, score and "buffs". Contains methods to modify this state safely.

### [ComedianType](ComedianType.cs)

Enum that differentiates human players from computer players.

### [ComedyStyle](ComedyStyle.cs)

Like a card's suit, otherwise identical `Card`s may have different style. This is an enum for describing it.

### [Deck](Deck.cs)

A list of `Card`s.

### [GameState](GameState.cs)

An enum that represents a game phase, such as the intro, score resolution, or effectresolution. See `ActCoordinator`.

### CardEffects/[CardEffect](CardEffects/CardEffect.cs)

An abstract class that represents an effect of a `Card`. Its subtypes define the behaviour of this effect with respect to the `GameplayState`.

### CardScoringRules/[CardScoringRule](CardScoringRules/CardScoringRule.cs)

An abstract class that represents how a `Card`'s score should be calculated, with respect to the `GameplayState`.