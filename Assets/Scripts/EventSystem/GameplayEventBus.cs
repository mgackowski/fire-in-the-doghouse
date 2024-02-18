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
public static class GameplayEventBus
{
    static Dictionary<Type, GameplayEvent<IEventArgs>> events = new Dictionary<Type, GameplayEvent<IEventArgs>>();

    static T GetEvent<T, U>()
        where T : GameplayEvent<U>, new()
        where U : IEventArgs
    {
        if (events.ContainsKey(typeof(T)))
        {
            return events[typeof(T)] as T;
        }
        else
        {
            // no need to populate dictionary with all possible events at start,
            // they are created on demand at request time (max 1 of each)
            T newEvent = new T();
            events.Add(typeof(T), newEvent as GameplayEvent<IEventArgs>);
            return newEvent;
        }
    }

    /*
     * Subscribe to event T which transmits event arguments U.
     * Must provide callback function; called when the event is invoked.
     */
    public static void Subscribe<T, U>(Action<U> callback)
        where T: GameplayEvent<U>, new()
        where U : IEventArgs
    {
        GetEvent<T, U>()?.Subscribe(callback);
    }

    /*
     * Unsubscribe from event T which transmits event arguments U.
     * Must provide callback function; called when the event is invoked.
     */
    public static void Unsubscribe<T, U>(Action<U> callback)
        where T : GameplayEvent<U>, new()
        where U : IEventArgs
    {
        GetEvent<T, U>()?.Unsubscribe(callback);
    }

    /*
     * Trigger event T, using compatible args U.
     */
    public static void Publish<T, U>(U args)
        where T : GameplayEvent<U>, new()
        where U : IEventArgs
    {
        GetEvent<T, U>()?.Publish(args);
    }

}