using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * Manages progression of a comedy night.
 * TODO: Merge with Act, as there is currently only one Act per Night.
 * TODO: Expose act events in a single object, e.g. here, or use an event bus
 */
public class NightManager : MonoBehaviour
{
    public int maxActs = 3;
    public CameraSwitcher cameras;
    public Audience audience;
    public List<ComedianAppearance> comedians;

    public Comedian humanPlayer;
    public Comedian cpuPlayer;

    public Deck humanStartingDeck;
    public Deck cpuStartingDeck;

    //GameState gameState;
    //int actNumber = 1;
    public Act act;

    List<Card> currentHumanDeck;
    List<Card> currentCpuDeck;
    Queue<CardPlay> cardQueue;

    public List<Card> humanDrawnCards;
    Card[] selectedCards = new Card[4];

    public float secondsBetweenEvents = 1f;

    public void ResetNight()
    {
        //gameState = GameState.ACT_PREP;
        //actNumber = 1;

        humanPlayer.ResetComedian();
        cpuPlayer.ResetComedian();

        currentHumanDeck = new List<Card>(humanStartingDeck.cards);
        currentCpuDeck = new List<Card>(cpuStartingDeck.cards);

        selectedCards = new Card[4];
        cardQueue = new Queue<CardPlay>();

        act = new Act(cardQueue, humanPlayer, cpuPlayer);
        cameras.UpdateActBinding();
        audience.UpdateActBindings();
        foreach (ComedianAppearance ca in comedians)
        {
            ca.UpdateActBindings();
        }

    }

    /* Allow the player to choose their cards */
    public void StartActPrep()
    {
        //gameState = GameState.ACT_PREP;
        // present UI to the player to select cards
        humanDrawnCards = DrawCardsFromDeck(currentHumanDeck);

    }

    public List<Card> DrawCardsFromDeck(List<Card> sourceDeck)
    {
        List<Card> drawnCards = new List<Card>();
        for (int i = 0; i <= 5; i++)
        {
            int randomInt = Random.Range(0, sourceDeck.Count);
            Card randomCard = sourceDeck[randomInt];
            sourceDeck.RemoveAt(randomInt);
            drawnCards.Add(randomCard);
        }
        return drawnCards;
    }

    /* Called by the card selector script each time a card is picked during prep */
    public void SelectCardForAct(Card card, int orderInAct)
    {
        selectedCards[orderInAct - 1] = card;
    }

    /* Called by the card selector script when all cards have been selected */
    public bool SubmitCardsForAct()
    {
        // Check if four cards selected
        foreach (Card card in selectedCards)
        {
            if (card == null)
            {
                Debug.LogError("Four cards not selected!");
                return false;
            }
        }

        // Randomise opponent selections
        List<Card> opponentCards = DrawCardsFromDeck(currentCpuDeck);

        // Put cards into the card queue for the Act
        foreach (Card card in selectedCards)
        {
            cardQueue.Enqueue(new CardPlay() { card = card, player = humanPlayer });
            currentHumanDeck.Remove(card);

            Card opponentCard = opponentCards[0];
            opponentCards.RemoveAt(0);
            cardQueue.Enqueue(new CardPlay() { card = opponentCard, player = cpuPlayer });
        }
        StartActPlayback();
        return true;
    }

    public void StartActPlayback()
    {
        //gameState = GameState.ACT_PLAYBACK;
        StartCoroutine(DirectAct());
        MessageSystem.Push("Time for the act!", MessageType.SYSTEM);
    }

    IEnumerator DirectAct()
    {
        act.StartPlayback();
        while (act.CardQueue.Count != 0)
        {
            act.StartTurn();
            yield return new WaitForSeconds(secondsBetweenEvents);
            act.PlayCard();
            yield return new WaitForSeconds(secondsBetweenEvents);
            act.ResolveEffects();
            act.ResolveScoreEffect();
            yield return new WaitForSeconds(secondsBetweenEvents);
            act.FinishTurn();
        }
        yield return new WaitForSeconds(secondsBetweenEvents);
        MessageSystem.Push("Act finished!", MessageType.SYSTEM);
        act.FinishPlayback();

    }

    private void Start()
    {
        //testing
        humanPlayer = new Comedian("Jam", ComedianType.PLAYER);
        cpuPlayer = new Comedian("Peanut", ComedianType.CPU);

        ResetNight();
        StartActPrep();

        //testing
        SelectCardForAct(humanDrawnCards[0], 1);
        SelectCardForAct(humanDrawnCards[1], 2);
        SelectCardForAct(humanDrawnCards[2], 3);
        SelectCardForAct(humanDrawnCards[3], 4);

        SubmitCardsForAct();
    }

}
