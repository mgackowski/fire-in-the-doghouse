using System;
using System.Collections.Generic;

/**
 * Allows for publishing and subscription to events that happen during gameplay,
 * e.g. a turn or score change. Subscription/publishing of an event for the first time
 * adds that event to a dictionary, so publishers and subscribers are no longer
 * dependent on the existence of each other.
 * Events can be raised by any publisher (as implementation is based on Action delegates,
 * not C# events).
 * Events implement IEventArgs and are defined, along with compatible argument wrapping
 * objects, inside GameplayEvents.cs.
 */
public class GameplayEventBus : IEventBus
{
    static GameplayEventBus instance;
    Dictionary<Type, Func<object>> events = new();

    GameplayEventBus() { }

    public static GameplayEventBus Instance()
    {
        if (instance == null)
        {
            instance = new GameplayEventBus();
        }
        return instance;
    }

    /*
     * Subscribe to event T which transmits event arguments U.
     * Must provide callback function; called when the event is invoked.
     */
    public void Subscribe<T, U>(Action<U> callback)
        where T: GameplayEvent<U>, new()
        where U : IEventArgs
    {
        var gameEvent = GetEvent<T, U>();
        gameEvent.Subscribe(callback);
    }

    /*
     * Unsubscribe from event T which transmits event arguments U.
     * Must provide callback function; called when the event is invoked.
     */
    public void Unsubscribe<T, U>(Action<U> callback)
        where T : GameplayEvent<U>, new()
        where U : IEventArgs
    {
        var gameEvent = GetEvent<T, U>();
        gameEvent.Unsubscribe(callback);
    }

    /*
     * Trigger event T, using compatible args U.
     */
    public void Publish<T, U>(U args)
        where T : GameplayEvent<U>, new()
        where U : IEventArgs
    {
        var gameEvent = GetEvent<T, U>();
        gameEvent.Publish(args);
    }

    //TODO: The casts here produce null results; investigate
    static T GetEvent<T, U>()
    where T : GameplayEvent<U>, new()
    where U : IEventArgs
    {
        if (instance.events.ContainsKey(typeof(T)))
        {
            return instance.events[typeof(T)]() as T;
        }
        else
        {
            // no need to populate dictionary with all possible events at start,
            // they are created on demand at request time (max 1 of each)
            var newEvent = new T();
            instance.events.Add(typeof(T), () => newEvent);
            return newEvent;
        }
    }

}