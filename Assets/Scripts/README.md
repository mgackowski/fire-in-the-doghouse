# Systems

Here are a few systems I wrote to help us develop this while (hopefully) avoiding headaches. Much of this has been refactored since the first build we did for Global Game Jam. - @mgackowski

## [EventSystem](EventSystem)

Pure C# (no engine dependency). Facilitates communication between objects, and should be used instead of holding references to "manager" classes etc. Introduces a `GameplayEventBus`, which is an event bus with two generic type parameters for event type and argument type.

```
// usage in client code
GameplayEventBus.Instance().Subscribe<ActIntroStartedEvent, GameplayStateArgs>(OnActIntroStarted);
GameplayEventBus.Instance().Publish<DialogueStartedEvents, DialogueArgs>(args);
```

## [CardSystem](CardSystem)

Breaks down cards into objects containing `CardScoringRule`s and `CardEffects`. Uses Unity's ScriptableObjects to easily create new variants from Inspector.


## [MessageSystem](MessageSystem)

Deprecate. This was a message queue that processed system messages and dialogue, but we can now use the event bus system in UI objects, and the DialogueStarted/DialogueFinished events, respectively.