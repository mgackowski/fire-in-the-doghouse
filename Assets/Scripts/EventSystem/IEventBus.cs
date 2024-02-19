using System;

public interface IEventBus
{
    public static void Subscribe<T, U>(Action<U> callback) => throw new NotImplementedException();
    public static void Unsubscribe<T, U>(Action<U> callback) => throw new NotImplementedException();
    public static void Publish<T, U>(U args) => throw new NotImplementedException();

}