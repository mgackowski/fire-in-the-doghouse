using System;

/**
 * An event that happens during gameplay, e.g. turn change or score change.
 */
public class GameplayEvent<T> : IGameEvent<T> where T : IEventArgs
{
    Action<T> callbacks;

    public void Subscribe(Action<T> action)
    {
        callbacks += action;
    }

    public void Unsubscribe(Action<T> action)
    {
        callbacks -= action;
    }

    public void Publish(T args)
    {
        callbacks?.Invoke(args);
    }

}