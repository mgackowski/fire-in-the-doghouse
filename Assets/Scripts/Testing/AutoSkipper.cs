using System;
using System.Collections;
using UnityEngine;

/**
 * The AutoSkipper listens to typical events raised throughout the game, and
 * can be configured to automatically respond to them, advancing the game
 * and acting as a "mock" for other objects. Created to be used in testing.
 * Events are raised with empty (default initialised) arguments.
 */
public class AutoSkipper : MonoBehaviour
{
    [SerializeField] bool autoStart = false; // send ActIntroStartedEvent on awake

    [Header("Invoke next event after receiving the following:")]
    [SerializeField] bool actIntroStartedEvent = false;
    [SerializeField] float actIntroStartedEventWait = 2f;
    [SerializeField] bool actIntroFinishedEvent = false;
    [SerializeField] float actIntroFinishedEventWait = 2f;
    [SerializeField] bool turnStartAnimationsStartedEvent = false;
    [SerializeField] float turnStartAnimationsStartedEventWait = 2f;
    [SerializeField] bool turnStartAnimationsFinishedEvent = false;
    [SerializeField] float turnStartAnimationsFinishedEventWait = 2f;
    [SerializeField] bool cardPlayAnimationsStartedEvent = false;
    [SerializeField] float cardPlayAnimationsStartedEventWait = 2f;
    [SerializeField] bool cardPlayAnimationsFinishedEvent = false;
    [SerializeField] float cardPlayAnimationsFinishedEventWait = 2f;
    [SerializeField] bool dialogueStartedEvent = false;
    [SerializeField] float dialogueStartedEventWait = 2f;
    [SerializeField] bool dialogueFinishedEvent = false;
    [SerializeField] float dialogueFinishedEventWait = 2f;
    [SerializeField] bool effectResolutionStartedEvent = false;
    [SerializeField] float effectResolutionStartedEventWait = 2f;
    [SerializeField] bool effectResolutionFinishedEvent = false;
    [SerializeField] float effectResolutionFinishedEventWait = 2f;
    [SerializeField] bool scoreResolutionStartedEvent = false;
    [SerializeField] float scoreResolutionStartedEventWait = 2f;
    [SerializeField] bool scoreResolutionFinishedEvent = false;
    [SerializeField] float scoreResolutionFinishedEventWait = 2f;
    [SerializeField] bool turnEndingStartedEvent = false;
    [SerializeField] float turnEndingStartedEventWait = 2f;
    [SerializeField] bool turnEndingFinishedEvent = false;
    [SerializeField] float turnEndingFinishedEventWait = 2f;
    [SerializeField] bool actEndingStartedEvent = false;
    [SerializeField] float actEndingStartedEventWait = 2f;
    [SerializeField] bool actEndingFinishedEvent = false;
    [SerializeField] float actEndingFinishedEventWait = 2f;

    DefaultEventArgs emptyArgs = new DefaultEventArgs();
    GameplayStateArgs emptyStateArgs = new GameplayStateArgs() { State = new GameplayState() };
    CardPlayArgs emptyPlay = new CardPlayArgs() { CardPlay = new CardPlay() };
    DialogueArgs emptyDialogue = new DialogueArgs() { DialogueLine = "(Testing only)" };
    ScoreArgs emptyScore = new ScoreArgs() { NewScore = 0 };

    IEnumerator InvokeDelayedEvent<T, U>(float delay, U args)
        where T : GameplayEvent<U>, new()
        where U : IEventArgs
    {
        Debug.Log($"AutoSkipper will publish event {typeof(T)} in {delay}s...");
        yield return new WaitForSeconds(delay);
        GameplayEventBus.Instance().Publish<T, U>(args);
        Debug.Log($"AutoSkipper published event {typeof(T)}.");
    }

    void OnActIntroStarted(IEventArgs args)
    {
        if (actIntroStartedEvent)
            StartCoroutine(InvokeDelayedEvent<ActIntroFinishedEvent, GameplayStateArgs>(actIntroStartedEventWait, emptyStateArgs));
    }

    void OnActIntroFinished(IEventArgs args)
    {
        if (actIntroFinishedEvent)
            StartCoroutine(InvokeDelayedEvent<TurnStartAnimationsStartedEvent, GameplayStateArgs>(actIntroFinishedEventWait, emptyStateArgs));
    }

    void OnTurnStartAnimationsStarted(IEventArgs args)
    {
        if (turnStartAnimationsStartedEvent)
            StartCoroutine(InvokeDelayedEvent<TurnStartAnimationsFinishedEvent, DefaultEventArgs>(turnStartAnimationsStartedEventWait, emptyArgs));
    }

    void OnTurnStartAnimationsFinished(IEventArgs args)
    {
        if (turnStartAnimationsFinishedEvent)
            StartCoroutine(InvokeDelayedEvent<CardPlayAnimationsStartedEvent, CardPlayArgs>(turnStartAnimationsFinishedEventWait, emptyPlay));
    }

    void OnCardPlayAnimationsStarted(IEventArgs args)
    {
        if (cardPlayAnimationsStartedEvent)
            StartCoroutine(InvokeDelayedEvent<CardPlayAnimationsFinishedEvent, DefaultEventArgs>(cardPlayAnimationsStartedEventWait, emptyArgs));
    }

    void OnCardPlayAnimationsFinished(IEventArgs args)
    {
        if (cardPlayAnimationsFinishedEvent)
            StartCoroutine(InvokeDelayedEvent<DialogueStartedEvent, DialogueArgs>(cardPlayAnimationsFinishedEventWait, emptyDialogue));
    }

    void OnDialogueStarted(IEventArgs args)
    {
        if (dialogueStartedEvent)
            StartCoroutine(InvokeDelayedEvent<DialogueFinishedEvent, DefaultEventArgs>(dialogueStartedEventWait, emptyArgs));
    }

    void OnDialogueFinished(IEventArgs args)
    {
        if (dialogueFinishedEvent)
            StartCoroutine(InvokeDelayedEvent<EffectResolutionStartedEvent, CardPlayArgs>(dialogueFinishedEventWait, emptyPlay));
    }

    void OnEffectResolutionStarted(IEventArgs args)
    {
        if (effectResolutionStartedEvent)
            StartCoroutine(InvokeDelayedEvent<EffectResolutionFinishedEvent, DefaultEventArgs>(effectResolutionStartedEventWait, emptyArgs));
    }

    void OnEffectResolutionFinished(IEventArgs args)
    {
        if (effectResolutionFinishedEvent)
            StartCoroutine(InvokeDelayedEvent<ScoreResolutionStartedEvent, ScoreArgs>(effectResolutionFinishedEventWait, emptyScore));
    }

    void OnScoreResolutionStarted(IEventArgs args)
    {
        if (scoreResolutionStartedEvent)
            StartCoroutine(InvokeDelayedEvent<ScoreResolutionFinishedEvent, DefaultEventArgs>(scoreResolutionStartedEventWait, emptyArgs));
    }

    void OnScoreResolutionFinished(IEventArgs args)
    {
        if (scoreResolutionFinishedEvent)
            StartCoroutine(InvokeDelayedEvent<TurnEndingStartedEvent, DefaultEventArgs>(scoreResolutionFinishedEventWait, emptyArgs));
    }

    void OnTurnEndingStarted(IEventArgs args)
    {
        if (turnEndingStartedEvent)
            StartCoroutine(InvokeDelayedEvent<TurnEndingFinishedEvent, DefaultEventArgs>(turnEndingStartedEventWait, emptyArgs));
    }

    void OnTurnEndingFinished(IEventArgs args)
    {
        if (turnEndingFinishedEvent)
            StartCoroutine(InvokeDelayedEvent<ActEndingStartedEvent, DefaultEventArgs>(turnEndingFinishedEventWait, emptyArgs));
    }

    void OnActEndingStarted(IEventArgs args)
    {
        if (actEndingStartedEvent)
            StartCoroutine(InvokeDelayedEvent<ActEndingFinishedEvent, DefaultEventArgs>(actEndingStartedEventWait, emptyArgs));
    }

    void OnActEndingFinished(IEventArgs args)
    {
        if (actEndingFinishedEvent)
            StartCoroutine(InvokeDelayedEvent<ActIntroStartedEvent, GameplayStateArgs>(actEndingFinishedEventWait, emptyStateArgs));
    }

    void Start()
    {
        GameplayEventBus.Instance().Subscribe<ActIntroStartedEvent, GameplayStateArgs>(OnActIntroStarted);
        GameplayEventBus.Instance().Subscribe<ActIntroFinishedEvent, GameplayStateArgs>(OnActIntroFinished);
        GameplayEventBus.Instance().Subscribe<TurnStartAnimationsStartedEvent, GameplayStateArgs>(OnTurnStartAnimationsStarted);
        GameplayEventBus.Instance().Subscribe<TurnStartAnimationsFinishedEvent, DefaultEventArgs>(OnTurnStartAnimationsFinished);
        GameplayEventBus.Instance().Subscribe<CardPlayAnimationsStartedEvent, CardPlayArgs>(OnCardPlayAnimationsStarted);
        GameplayEventBus.Instance().Subscribe<CardPlayAnimationsFinishedEvent, DefaultEventArgs>(OnCardPlayAnimationsFinished);
        GameplayEventBus.Instance().Subscribe<DialogueStartedEvent, DialogueArgs>(OnDialogueStarted);
        GameplayEventBus.Instance().Subscribe<DialogueFinishedEvent, DefaultEventArgs>(OnDialogueFinished);
        GameplayEventBus.Instance().Subscribe<EffectResolutionStartedEvent, CardPlayArgs>(OnEffectResolutionStarted);
        GameplayEventBus.Instance().Subscribe<EffectResolutionFinishedEvent, DefaultEventArgs>(OnEffectResolutionFinished);
        GameplayEventBus.Instance().Subscribe<ScoreResolutionStartedEvent, ScoreArgs>(OnScoreResolutionStarted);
        GameplayEventBus.Instance().Subscribe<ScoreResolutionFinishedEvent, DefaultEventArgs>(OnScoreResolutionFinished);
        GameplayEventBus.Instance().Subscribe<TurnEndingStartedEvent, DefaultEventArgs>(OnTurnEndingStarted);
        GameplayEventBus.Instance().Subscribe<TurnEndingFinishedEvent, DefaultEventArgs>(OnTurnEndingFinished);
        GameplayEventBus.Instance().Subscribe<ActEndingStartedEvent, DefaultEventArgs>(OnActEndingStarted);
        GameplayEventBus.Instance().Subscribe<ActEndingFinishedEvent, DefaultEventArgs>(OnActEndingFinished);

        if (autoStart)
        {
            GameplayEventBus.Instance().Publish<ActIntroStartedEvent, GameplayStateArgs>(emptyStateArgs);
        }

    }

    private void OnDestroy()
    {
        GameplayEventBus.Instance().Unsubscribe<ActIntroStartedEvent, GameplayStateArgs>(OnActIntroStarted);
        GameplayEventBus.Instance().Unsubscribe<ActIntroFinishedEvent, GameplayStateArgs>(OnActIntroFinished);
        GameplayEventBus.Instance().Unsubscribe<TurnStartAnimationsStartedEvent, GameplayStateArgs>(OnTurnStartAnimationsStarted);
        GameplayEventBus.Instance().Unsubscribe<TurnStartAnimationsFinishedEvent, DefaultEventArgs>(OnTurnStartAnimationsFinished);
        GameplayEventBus.Instance().Unsubscribe<CardPlayAnimationsStartedEvent, CardPlayArgs>(OnCardPlayAnimationsStarted);
        GameplayEventBus.Instance().Unsubscribe<CardPlayAnimationsFinishedEvent, DefaultEventArgs>(OnCardPlayAnimationsFinished);
        GameplayEventBus.Instance().Unsubscribe<DialogueStartedEvent, DialogueArgs>(OnDialogueStarted);
        GameplayEventBus.Instance().Unsubscribe<DialogueFinishedEvent, DefaultEventArgs>(OnDialogueFinished);
        GameplayEventBus.Instance().Unsubscribe<EffectResolutionStartedEvent, CardPlayArgs>(OnEffectResolutionStarted);
        GameplayEventBus.Instance().Unsubscribe<EffectResolutionFinishedEvent, DefaultEventArgs>(OnEffectResolutionFinished);
        GameplayEventBus.Instance().Unsubscribe<ScoreResolutionStartedEvent, ScoreArgs>(OnScoreResolutionStarted);
        GameplayEventBus.Instance().Unsubscribe<ScoreResolutionFinishedEvent, DefaultEventArgs>(OnScoreResolutionFinished);
        GameplayEventBus.Instance().Unsubscribe<TurnEndingStartedEvent, DefaultEventArgs>(OnTurnEndingStarted);
        GameplayEventBus.Instance().Unsubscribe<TurnEndingFinishedEvent, DefaultEventArgs>(OnTurnEndingFinished);
        GameplayEventBus.Instance().Unsubscribe<ActEndingStartedEvent, DefaultEventArgs>(OnActEndingStarted);
        GameplayEventBus.Instance().Unsubscribe<ActEndingFinishedEvent, DefaultEventArgs>(OnActEndingFinished);

    }

}
