/**
 * List of gameplay events that can be raised.
 */

public class ActIntroStartedEvent : GameplayEvent<GameplayStateArgs> { }
public class ActIntroFinishedEvent : GameplayEvent<GameplayStateArgs> { }
public class TurnStartAnimationsStartedEvent : GameplayEvent<GameplayStateArgs> { }
public class TurnStartAnimationsFinishedEvent : GameplayEvent<DefaultEventArgs> { }
public class CardPlayAnimationsStartedEvent : GameplayEvent<CardPlayArgs> { }
public class CardPlayAnimationsFinishedEvent : GameplayEvent<DefaultEventArgs> { }
public class DialogueStartedEvent : GameplayEvent<DialogueArgs> { }
public class DialogueFinishedEvent : GameplayEvent<DefaultEventArgs> { }
public class EffectResolutionStartedEvent : GameplayEvent<CardPlayArgs> { }
public class EffectResolutionFinishedEvent : GameplayEvent<DefaultEventArgs> { }
public class ScoreResolutionStartedEvent : GameplayEvent<ScoreArgs> { }
public class ScoreResolutionFinishedEvent : GameplayEvent<DefaultEventArgs> { }
public class TurnEndingStartedEvent : GameplayEvent<DefaultEventArgs> { }
public class TurnEndingFinishedEvent : GameplayEvent<DefaultEventArgs> { }
public class ActEndingStartedEvent : GameplayEvent<DefaultEventArgs> { }
public class ActEndingFinishedEvent : GameplayEvent<DefaultEventArgs> { }

/**
 * List of event args objects that can be transmitted alongside events.
 */

public class DefaultEventArgs : IEventArgs { }

public class GameplayStateArgs : IEventArgs
{
    public GameplayState State { get; set; }
}

public class CardPlayArgs : IEventArgs {
    public CardPlay CardPlay { get; set; }
}

public class DialogueArgs : IEventArgs
{
    public Comedian Speaker { get; set; }
    public string DialogueLine { get; set; }
}

public class ScoreArgs : IEventArgs
{
    public Comedian TurnPlayer { get; set; }
    public int TurnScore { get; set; }
    public int TotalScore {  get; set; }

}