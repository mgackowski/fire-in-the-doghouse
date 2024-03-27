using UnityEngine;

/**
 * Having this component in a Scene will cause events in the Act to resolve in
 * order. This class raises events to the GameplayEventBus whenever a stage is
 * about to start (e.g. TurnStarted, DialogueStarted), and waits for a
 * corresponding "Finished" event to be raised to continue.
 */
public class ActCoordinator : MonoBehaviour
{

    enum ActState
    {
        ActIntroStarted, ActIntroFinished,
        TurnStartAnimationsStarted, TurnStartAnimationsFinished,
        CardPlayAnimationsStarted, CardPlayAnimationsFinished,
        DialogueStarted, DialogueFinished,
        ScoreResolutionStarted, ScoreResolutionFinished,
        EffectResolutionStarted, EffectResolutionFinished,
        TurnEndingStarted, TurnEndingFinished,
        ActEndingStarted, ActEndingFinished
    }


    // TODO: Public Inspector fields here


    DialogueGenerator dialogueGenerator;
    ActState actState = ActState.ActIntroStarted;

    // Cached references to objects used in events
    GameplayState state;    // TODO: Find a nice way to inject this
    CardPlay currentPlay;
    GameplayStateArgs stateArgs;
    CardPlayArgs cardPlayArgs;


    /* The start of the act - cards are already selected, now the player observes
     * how the battle resolves across multiple turns.
     */
    private void StartActIntro()
    {
        state.CurrentNoun = dialogueGenerator.GetRandomNoun();
        state.CurrentAdjective = dialogueGenerator.GetRandomAdjective();

        state.HumanComedian.ResetComedian();
        state.CpuComedian.ResetComedian();

        GameplayEventBus.Instance().Publish<ActIntroStartedEvent, GameplayStateArgs>(stateArgs);
    }

    private void OnActIntroFinished(GameplayStateArgs args)
    {
        if (actState != ActState.ActIntroStarted) return;
        GameplayState state = args.State;

        actState = ActState.ActIntroFinished;
        if (state.CardQueue.Count > 0)
        {
            StartTurnStartAnimations();
        }
        else
        {
            StartActEnding();
        }
        
    }

    /* Launch animations associated with turn start.
     */
    public void StartTurnStartAnimations()
    {
        actState = ActState.TurnStartAnimationsStarted;
        GameplayEventBus.Instance().Publish<TurnStartAnimationsStartedEvent, GameplayStateArgs>(stateArgs);
    }

    public void OnTurnStartAnimationsFinished(DefaultEventArgs args)
    {
        if (actState != ActState.TurnStartAnimationsStarted) return;
        actState = ActState.TurnStartAnimationsFinished;
        StartCardPlayAnimations();
    }

    /* Launch animations associated with a card being played.
     */
    public void StartCardPlayAnimations()
    {
        actState = ActState.CardPlayAnimationsStarted;

        if (!state.CardQueue.TryDequeue(out currentPlay))
        {
            Debug.LogError("Attempted to resolve card but Q empty.");
            return;
        }

        MessageSystem.Push($"{currentPlay.player.ComedianName} plays {currentPlay.card.cardName}.", MessageType.SYSTEM);

        GameplayEventBus.Instance().Publish<CardPlayAnimationsStartedEvent, CardPlayArgs>(cardPlayArgs);
    }

    public void OnCardPlayAnimationsFinished(DefaultEventArgs args)
    {
        if (actState != ActState.CardPlayAnimationsStarted) return;
        actState = ActState.CardPlayAnimationsFinished;
        StartDialogue();
    }

    /* Invoke events that make the characters deliver their lines.
     */
    public void StartDialogue()
    {
        actState = ActState.DialogueStarted;

        int lineNumber = Random.Range(0, currentPlay.card.dialogueLines.Count);
        string dialogueLine = dialogueGenerator.GetSentence(
            currentPlay.card.dialogueLines[lineNumber],
            state.CurrentNoun, state.CurrentAdjective,
            currentPlay.player.Type == ComedianType.PLAYER ? state.CpuComedian.ComedianName : state.HumanComedian.ComedianName);

        MessageSystem.Push(dialogueLine, currentPlay.player.Type == ComedianType.PLAYER ? MessageType.DIALOGUE_PLAYER : MessageType.DIALOGUE_CPU);

        GameplayEventBus.Instance().Publish<DialogueStartedEvent, DialogueArgs>(new DialogueArgs()
        {
            Speaker = currentPlay.player,
            DialogueLine = dialogueLine
        });
    }

    public void OnDialogueFinished(DefaultEventArgs args)
    {
        if (actState != ActState.DialogueStarted) return;
        actState = ActState.DialogueFinished;
        StartScoreResolution();
    }

    /* Calculate the score of this turn's play, display it, play audience reactions etc.
     */
    public void StartScoreResolution()
    {
        actState = ActState.ScoreResolutionStarted;

            // TODO

        GameplayEventBus.Instance().Publish<ScoreResolutionStartedEvent, DefaultEventArgs>(new DefaultEventArgs());
    }

    public void OnScoreResolutionFinished(DefaultEventArgs args)
    {
        if (actState != ActState.ScoreResolutionStarted) return;
        actState = ActState.ScoreResolutionFinished;
        StartEffectResolution();
    }

    /* Apply additional effects caused by the play, e.g. place penalty on opponent.
     */
    public void StartEffectResolution()
    {
        actState = ActState.EffectResolutionStarted;

            // TODO

        GameplayEventBus.Instance().Publish<EffectResolutionStartedEvent, DefaultEventArgs>(new DefaultEventArgs());
    }

    public void OnEffectResolutionFinished(DefaultEventArgs args)
    {
        if (actState != ActState.EffectResolutionStarted) return;
        actState = ActState.EffectResolutionFinished;
        StartTurnEnding();
    }

    /* The turn is finishing - the opponent will take over afterwards.
     */
    public void StartTurnEnding()
    {
        actState = ActState.TurnEndingStarted;
        GameplayEventBus.Instance().Publish<TurnEndingStartedEvent, DefaultEventArgs>(new DefaultEventArgs());
    }

    
    public void OnTurnFinished(DefaultEventArgs args)
    {
        if (actState != ActState.TurnEndingStarted) return;
        actState = ActState.TurnEndingFinished;

        if (state.CardQueue.Count > 0)
        {
            StartActIntro();
        }
        else
        {
            StartActEnding();
        }
        StartTurnEnding();
    }

    /* No more cards in queue - finish up Act */
    public void StartActEnding()
    {
        actState = ActState.ActEndingStarted;
        GameplayEventBus.Instance().Publish<ActEndingStartedEvent, DefaultEventArgs>(new DefaultEventArgs());
    }

    public void OnActEndingFinished(DefaultEventArgs args)
    {
        if (actState != ActState.ActEndingStarted) return;
        actState = ActState.ActEndingFinished;
        // TODO: Change game mode accordingly once implemented
    }

    private void Awake()
    {
        stateArgs = new GameplayStateArgs() { State = state };
        cardPlayArgs = new CardPlayArgs() { CardPlay = currentPlay };
        dialogueGenerator = new DialogueGenerator();
    }

    private void Start()
    {
        GameplayEventBus.Instance().Subscribe<ActIntroFinishedEvent, GameplayStateArgs>(OnActIntroFinished);
        GameplayEventBus.Instance().Subscribe<TurnStartAnimationsFinishedEvent, DefaultEventArgs>(OnTurnStartAnimationsFinished);
        GameplayEventBus.Instance().Subscribe<CardPlayAnimationsFinishedEvent, DefaultEventArgs>(OnCardPlayAnimationsFinished);
        GameplayEventBus.Instance().Subscribe<DialogueFinishedEvent, DefaultEventArgs>(OnDialogueFinished);
        GameplayEventBus.Instance().Subscribe<ScoreResolutionFinishedEvent, DefaultEventArgs>(OnScoreResolutionFinished);
        GameplayEventBus.Instance().Subscribe<EffectResolutionFinishedEvent, DefaultEventArgs>(OnEffectResolutionFinished);
        GameplayEventBus.Instance().Subscribe<TurnEndingFinishedEvent, DefaultEventArgs>(OnTurnFinished);
        GameplayEventBus.Instance().Subscribe<ActEndingFinishedEvent, DefaultEventArgs>(OnActEndingFinished);
    }

    private void OnDestroy()
    {
        GameplayEventBus.Instance().Unsubscribe<TurnStartAnimationsFinishedEvent, DefaultEventArgs>(OnTurnStartAnimationsFinished);
        GameplayEventBus.Instance().Unsubscribe<CardPlayAnimationsFinishedEvent, DefaultEventArgs>(OnCardPlayAnimationsFinished);
        GameplayEventBus.Instance().Unsubscribe<DialogueFinishedEvent, DefaultEventArgs>(OnDialogueFinished);
        GameplayEventBus.Instance().Unsubscribe<ScoreResolutionFinishedEvent, DefaultEventArgs>(OnScoreResolutionFinished);
        GameplayEventBus.Instance().Unsubscribe<EffectResolutionFinishedEvent, DefaultEventArgs>(OnEffectResolutionFinished);
        GameplayEventBus.Instance().Unsubscribe<TurnEndingFinishedEvent, DefaultEventArgs>(OnTurnFinished);
        GameplayEventBus.Instance().Unsubscribe<ActEndingFinishedEvent, DefaultEventArgs>(OnActEndingFinished);
    }


}
