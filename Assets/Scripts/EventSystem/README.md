# EventSystem

Pure C# (no engine dependency). Facilitates communication between objects. Introduces a `GameplayEventBus`, which is an event bus with two generic type parameters for event type and argument type.

Example:
- `ActCoordinator` determines it's time to deliver lines, and publishes a `DialogueStartedEvent`. `DialogueArgs` contain information on who should speak, and what.
- `CameraSwitcher`, `Comedian` and `ComedianSpeech` are all listeners. They set the camera, animation, and text display respectively. When `ComedianSpeech` is finished, it sends a `DialogueFinishedEvent`.
- Any listener to the `DialogueFinishedEvent` can now decide what to do. In previous implementations, they would have been dependent on an instance of `ActCoordinator`, or some other `-Manager`.   

## Usage

```
// usage in client code
GameplayEventBus.Instance().Subscribe<ActIntroStartedEvent, GameplayStateArgs>(OnActIntroStarted);
GameplayEventBus.Instance().Publish<DialogueStartedEvents, DialogueArgs>(args);
```

Pros:
- Like any event driven system, eliminates the need for two scene objects to hold references to each other.
- It also means scene objects don't need to be wired to any specific event publisher class (one-to-many, or multicast). This is great for building new objects and testing them in isolation.
- Instead, anything can publish, and anything can subscribe to any `IGameEvent` (many-to-many).
- Unlike a typical event bus, we are not limited to the same event arguments type, so we only end up passing the information that's necessary.

Cons:
- Since anything can publish to the bus, exercise extra caution here.
- Support for many event/eventArgs types comes with having to provide two generic type parameters when publishing/subscribing. The event type and argument type must match. This *is* enforced by the compiler, but likely won't get auto-suggested. All event types and corresponding eventArgs types are defined in [`GameplayEvents.cs`](GameplayEvents.cs) and should be checked there.
- If there's no need for arguments for an event type, we still need to pass an empty `DefaultEventArgs` as it inherits from `IEventArgs`.

## Components

### [GameplayEvent](GameplayEvent.cs)
`public class GameplayEvent<T> : IGameEvent<T> where T : IEventArgs`

The subtype of all gameplay events; contains a list of functions to call when invoked. Only one of each type exists in the event bus.

### [GameplayEventBus](GameplayEventBus.cs)

`public class GameplayEventBus : IEventBus`

Accessed via a static instance (`Instance()`), allows for the publishing and subscribing to gameplay events (see Usage).

### [GameplayEvents](GameplayEvents.cs)

Contains and defines subtypes of `GameplayEvent` and implementations of `IEventArgs`. This is where new event types should be introduced, and where their compatible argument types are declared.

`IEventArgs` implementations have dependencies on other types used during gameplay.

### [IGameEvent](IGameEvent.cs), [IEventBus](IEventBus.cs), [IEventArgs](IEventArgs.cs)

Interfaces for the above.