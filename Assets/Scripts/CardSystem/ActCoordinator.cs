using System;
using UnityEngine;

/**
 * Having this component in a Scene will cause events in the Act to resolve in
 * order. This class raises events to the GameplayEventBus whenever a stage is
 * about to start (e.g. TurnStarted, DialogueStarted), and waits for a
 * corresponding "Finished" event to be raised to continue.
 * Starts by receiving an ActIntroStarted event with a set of GameplayStateArgs,
 * which are injected into the object and used as a source of truth onwards.
 */
public class ActCoordinator : MonoBehaviour
{

    enum ActState
    {
        ActIntroStarted, ActIntroFinished,
        TurnStartAnimationsStarted, TurnStartAnimationsFinished,
        CardPlayAnimationsStarted, CardPlayAnimationsFinished,
        DialogueStarted, DialogueFinished,
        EffectResolutionStarted, EffectResolutionFinished,
        ScoreResolutionStarted, ScoreResolutionFinished,
        TurnEndingStarted, TurnEndingFinished,
        ActEndingStarted, ActEndingFinished
    }

    [Header("Dialogue generation")]
    [SerializeField] TextAsset nounsList;
    [SerializeField] TextAsset adjectivesList;

    [Header("Scoring")]
    [SerializeField] int setupBonus = 1; // Points awarded for successful Setup.

    DialogueGenerator dialogueGenerator;
    ActState actState = ActState.ActIntroStarted;
    GameplayState state;

    // Cached references to objects used in events
    CardPlay currentPlay;
    GameplayStateArgs stateArgs;
    CardPlayArgs cardPlayArgs;

    /* The start of the act - cards are already selected, now the player observes
     * how the battle resolves across multiple turns.
     */
    private void OnActIntroStarted(GameplayStateArgs args)
    {
        SetNewState(args.State);

        state.CurrentNoun = dialogueGenerator.GetRandomNoun();
        state.CurrentAdjective = dialogueGenerator.GetRandomAdjective();

        state.HumanComedian.ResetComedian();
        state.CpuComedian.ResetComedian();
    }

    private void OnActIntroFinished(GameplayStateArgs args)
    {
        if (actState != ActState.ActIntroStarted) return;

        actState = ActState.ActIntroFinished;
        if (state.CardQueue.Count > 0)
        {
            StartTurnStartAnimations();
        }
        else
        {
            Debug.Log("No more cards in queue, skipping to act ending.");
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

        cardPlayArgs = new CardPlayArgs()
        {
            CardPlay = currentPlay
        };

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

        int lineNumber = UnityEngine.Random.Range(0, currentPlay.card.dialogueLines.Count);
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

    /* Apply additional effects caused by the play, e.g. place penalty on opponent.
    */
    public void StartEffectResolution()
    {
        actState = ActState.EffectResolutionStarted;

        foreach (CardEffect effect in currentPlay.card.effects)
        {
            effect.applyEffect(currentPlay, state);
        }
        // TODO: Raise for each effect, sequentially, and use more specific args e.g. EffectArgs
        GameplayEventBus.Instance().Publish<EffectResolutionStartedEvent, CardPlayArgs>(cardPlayArgs);
    }

    public void OnEffectResolutionFinished(DefaultEventArgs args)
    {
        if (actState != ActState.EffectResolutionStarted) return;
        actState = ActState.EffectResolutionFinished;
        StartScoreResolution();
    }

    /* Calculate the score of this turn's play, display it, play audience reactions etc.
     */
    public void StartScoreResolution()
    {
        actState = ActState.ScoreResolutionStarted;

        int effectiveScore = CalculateCardScore(currentPlay);
        currentPlay.effectiveScore = effectiveScore;
        currentPlay.player.AddToScore(effectiveScore);
        MessageSystem.Push($"{currentPlay.player.ComedianName} gained {effectiveScore} points.", MessageType.SYSTEM);

        GameplayEventBus.Instance().Publish<ScoreResolutionStartedEvent, ScoreArgs>(new ScoreArgs()
        {
            NewScore = effectiveScore
        }) ;
    }

    public void OnScoreResolutionFinished(DefaultEventArgs args)
    {
        if (actState != ActState.ScoreResolutionStarted) return;
        actState = ActState.ScoreResolutionFinished;
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
            StartTurnStartAnimations();
        }
        else
        {
            StartActEnding();
        }
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

    public void SetNewState(GameplayState newState)
    {
        Debug.Log("New GameplayState provided to ActCoordinator.");
        state = newState;
        stateArgs = new GameplayStateArgs() { State = newState };
    }

    private void Awake()
    {
        if (state == null)
        {
            Debug.Log("ActCoordinator.Awake() called and no GameplayState present; creating an empty state.");
            state = new GameplayState();
        }

        stateArgs = new GameplayStateArgs() { State = state };
        cardPlayArgs = new CardPlayArgs() { CardPlay = currentPlay };
        dialogueGenerator = new DialogueGenerator(nounsList, adjectivesList);

        GameplayEventBus.Instance().Subscribe<ActIntroStartedEvent, GameplayStateArgs>(OnActIntroStarted);
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
        GameplayEventBus.Instance().Unsubscribe<ActIntroStartedEvent, GameplayStateArgs>(OnActIntroStarted);
        GameplayEventBus.Instance().Unsubscribe<ActIntroFinishedEvent, GameplayStateArgs>(OnActIntroFinished);
        GameplayEventBus.Instance().Unsubscribe<TurnStartAnimationsFinishedEvent, DefaultEventArgs>(OnTurnStartAnimationsFinished);
        GameplayEventBus.Instance().Unsubscribe<CardPlayAnimationsFinishedEvent, DefaultEventArgs>(OnCardPlayAnimationsFinished);
        GameplayEventBus.Instance().Unsubscribe<DialogueFinishedEvent, DefaultEventArgs>(OnDialogueFinished);
        GameplayEventBus.Instance().Unsubscribe<ScoreResolutionFinishedEvent, DefaultEventArgs>(OnScoreResolutionFinished);
        GameplayEventBus.Instance().Unsubscribe<EffectResolutionFinishedEvent, DefaultEventArgs>(OnEffectResolutionFinished);
        GameplayEventBus.Instance().Unsubscribe<TurnEndingFinishedEvent, DefaultEventArgs>(OnTurnFinished);
        GameplayEventBus.Instance().Unsubscribe<ActEndingFinishedEvent, DefaultEventArgs>(OnActEndingFinished);
    }

    int CalculateCardScore(CardPlay play)
    {
        int cardScore = Math.Max(play.card.scoringRule.GetBaseScore(play, state) + play.player.Bonus, 0);

        // Extra points if Setup effect is active for that player.
        if (state.SetupActive && play.player == state.SetupPlayer)
        {
            cardScore += setupBonus;
            MessageSystem.Push("It's following the Setup.", MessageType.SYSTEM);
        }
        return cardScore;
    }


}
