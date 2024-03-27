using System;
using System.Collections.Generic;

/**
 * TODO: Deprecate.
 * Pass messages in appropriate events in the GameplayEventBus.
 */
public static class MessageSystem
{
    public static event Action<Message> messageArrived;

    static readonly Queue<Message> messageQueue = new Queue<Message>();

    public static void Push(string message, MessageType type)
    {
        messageQueue.Enqueue(new Message() {text = message, type = type});
        // Implement delay here if necessary.
        messageArrived?.Invoke(messageQueue.Dequeue());
    }

}
