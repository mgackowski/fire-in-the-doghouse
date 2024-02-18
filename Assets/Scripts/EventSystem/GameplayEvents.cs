/**
 * List of gameplay events that can be raised.
 */

public class ActPlaybackStartedEvent : GameplayEvent<DefaultEventArgs> { }
public class TurnStartedEvent : GameplayEvent<DefaultEventArgs> { }
public class CardPlayEvent : GameplayEvent<CardPlayArgs> { }
public class EffectResolutionEvent : GameplayEvent<DefaultEventArgs> { }
public class ScoreResolutionEvent : GameplayEvent<ScoreArgs> { }
public class TurnEndedEvent : GameplayEvent<DefaultEventArgs> { }
public class ActPlaybackFinishedEvent : GameplayEvent<DefaultEventArgs> { }

/**
 * List of event args objects that can be transmitted alongside events.
 */

public class DefaultEventArgs : IEventArgs { }

public class CardPlayArgs : IEventArgs {
    public CardPlay CardPlay { get; set; }
}

public class ScoreArgs : IEventArgs
{
    public int NewScore { get; set; }
}