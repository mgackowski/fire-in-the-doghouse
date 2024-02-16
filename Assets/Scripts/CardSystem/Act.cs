using System;
using System.Collections.Generic;

public class Act
{
    public event Action ActPlaybackStartedEvent;
    public event Action TurnStartedEvent;
    public event Action<CardPlay> CardPlayEvent;
    public event Action EffectResolutionEvent;
    public event Action<int> ScoreResolutionEvent;
    public event Action TurnEndedEvent;
    public event Action ActPlaybackFinishedEvent;

    public Queue<CardPlay> CardQueue { get; }
    public Stack<CardPlay> DiscardPile { get; private set; }

    public bool SetupActive { get; private set; } = false;
    public ComedyStyle SetupType { get; private set; }
    public Comedian SetupPlayer { get; private set; }

    public Comedian HumanPlayer { get; private set; }
    public Comedian CpuOpponent { get; private set; }
    public Comedian CurrentPlayer { get; private set; }

    DialogueGenerator dialogueGen = new DialogueGenerator();
    string currentNoun;
    string currentAdjective;
    CardPlay currentPlay;

    public Act(Queue<CardPlay> cardQueue, Comedian humanPlayer, Comedian cpuOpponent, Stack<CardPlay> discardPile = null)
    {
        CardQueue = cardQueue;
        HumanPlayer = humanPlayer;
        CpuOpponent = cpuOpponent;

        if (discardPile == null)
        {
            DiscardPile = new Stack<CardPlay>();
        }
        else
        {
            DiscardPile = discardPile;
        }

        currentNoun = dialogueGen.GetRandomNoun();
        currentAdjective = dialogueGen.GetRandomAdjective();
        
    }

    public void StartPlayback()
    {
        ActPlaybackStartedEvent?.Invoke();
        
    }

    public void FinishPlayback()
    {
        ActPlaybackFinishedEvent?.Invoke();
    }

    public void StartTurn() {

        bool cardsLeft;
        cardsLeft = CardQueue.TryDequeue(out currentPlay);
        if (!cardsLeft)
        {
            //Debug.LogError("Card queue empty for some reason!");
            return;
        }

        TurnStartedEvent?.Invoke();
        //PlayCard(currentPlay);

    }

    public void PlayCard()
    {

        Comedian player = currentPlay.player;
        Card currentCard = currentPlay.card;
        CurrentPlayer = player;

        // Deliver line
        MessageSystem.Push($"{player.ComedianName} plays {currentPlay.card.cardName}.", MessageType.SYSTEM);
        Random rng = new Random();
        int lineNumber = rng.Next(currentCard.dialogueLines.Count);
        string dialogueLine = dialogueGen.GetSentence(currentCard.dialogueLines[lineNumber], currentNoun, currentAdjective,
            CurrentPlayer == HumanPlayer ? CpuOpponent.ComedianName : HumanPlayer.ComedianName);
        MessageSystem.Push(dialogueLine, CurrentPlayer == HumanPlayer ? MessageType.DIALOGUE_PLAYER : MessageType.DIALOGUE_CPU);

        CardPlayEvent?.Invoke(currentPlay);

    }

    public void ResolveEffects()
    {
        Comedian player = currentPlay.player;
        Card currentCard = currentPlay.card;

        // Resolve card effects
        foreach (CardEffect effect in currentCard.effects)
        {
            effect.applyEffect(currentCard, this);
        }
        EffectResolutionEvent?.Invoke();
    }

    public void ResolveScoreEffect()
    {
        Comedian player = currentPlay.player;
        Card currentCard = currentPlay.card;

        // Resolve score
        int effectiveScore = CalculateCardScore(currentPlay);
        currentPlay.effectiveScore = effectiveScore;
        player.AddToScore(effectiveScore);
        MessageSystem.Push($"{player.ComedianName} gained {effectiveScore} points.", MessageType.SYSTEM);


        ScoreResolutionEvent?.Invoke(effectiveScore);
    }



    public void FinishTurn()
    {
        // Discard
        DiscardPile.Push(currentPlay);
        TurnEndedEvent?.Invoke();
    }

    public void ActivateSetup(ComedyStyle comedyStyle, Comedian player)
    {
        SetupActive = true;
        SetupType = comedyStyle;
        SetupPlayer = player;
        //SetupActiveStateChanged.Invoke(true);
    }

    public void ResetSetup()
    {
        SetupActive = false;
        //SetupActiveStateChanged.Invoke(false);
    }

    int CalculateCardScore(CardPlay play)
    {
        int cardScore = Math.Max(play.card.scoringRule.GetBaseScore(play.card, this) + play.player.Bonus, 0);

        // if playing without style bonuses
        if (SetupActive && play.player == SetupPlayer)
        {
            cardScore += 1;
            MessageSystem.Push("It's following the Setup.", MessageType.SYSTEM);
        }

        // if playing with card style bonuses
        /*
        if (play.card.Style.Equals(DiscardPile.Peek().card.Style)) {
            MessageSystem.Push("Bonus for consistent style.", MessageType.SYSTEM);
            cardScore++;
        }
        */
        return cardScore;
    }

}
