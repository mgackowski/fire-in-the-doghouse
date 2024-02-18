using System;
/**
* Encapsulates an event that can be raised or subscribed to.
* The event containst arguments of type T.
*/
public interface IGameEvent<T> where T : IEventArgs
{
    public void Subscribe(Action<T> action);
    public void Unsubscribe(Action <T> action);
    public void Publish(T args);
}