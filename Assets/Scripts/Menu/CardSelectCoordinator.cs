using System.Collections.Generic;
using UnityEngine;

/**
 * By <author>mgackowski</author>.
 */
public class CardSelectCoordinator : MonoBehaviour
{
    [SerializeField] Deck fullPlayerDeck;
    [SerializeField] Deck fullCpuDeck;
    [SerializeField] int cardsDealt = 5;
    [SerializeField] int cardsToSelect = 4;

    [Header("Scene setup")]
    [SerializeField] GameObject nextSceneMessengerPrefab;
    [SerializeField] CardSelector cardSelector;
    [SerializeField] GameObject fadeOutObject;

    GameplayState state;

    public void SetState(GameplayState newState)
    {
        state = newState;
    }

    public void ResetState()
    {
        state = new GameplayState();
        state.HumanDeck.Clear();
        state.CpuDeck.Clear();
        state.HumanDeck.AddRange(fullPlayerDeck.cards);
        state.CpuDeck.AddRange(fullCpuDeck.cards);

    }

    private void OnCardSelectStarted(GameplayStateArgs args)
    {
        state = args.State;
        if (state.ActNumber <= 1)
        {
            state.ActNumber = 1;
            state.HumanDeck.Clear();
            state.CpuDeck.Clear();
            state.HumanDeck.AddRange(fullPlayerDeck.cards);
            state.CpuDeck.AddRange(fullCpuDeck.cards);
        }
        
        // Start dealing cards for Player
        List<Card> cardsToDeal = new List<Card>();
        for (int i = 0; i < cardsDealt; i++)
        {
            int selectedIndex = Random.Range(0, state.HumanDeck.Count);
            cardsToDeal.Add(state.HumanDeck[selectedIndex]);        //TODO: There's a glitch here
            state.HumanDeck.RemoveAt(selectedIndex);
        }
        cardSelector.SpawnCards(cardsToDeal, cardsToSelect);

    }

    private void OnCardSelectFinished(CardSelectArgs args)
    {
        // Select cards for opponent (currently random)

        for (int i = 0; i < cardsToSelect; i++)
        {
            int cpuCardIndex = Random.Range(0, state.CpuDeck.Count);
            CardPlay cpuPlay = new CardPlay()
            {
                card = state.CpuDeck[cpuCardIndex],
                player = state.CpuComedian
            };
            state.CpuDeck.RemoveAt(cpuCardIndex);

            // Also prepare 
            CardPlay humanPlay = new CardPlay()
            {
                card = args.Cards[i],
                player = state.HumanComedian
            };
            state.HumanDeck.Remove(args.Cards[i]);

            // Alternate between player and CPU, starting with player
            // TODO: Enable control over this:
            state.CardQueue.Enqueue(humanPlay);
            state.CardQueue.Enqueue(cpuPlay);
        }

        // Create an object to transfer gameplay state to the next scene:
        Instantiate(nextSceneMessengerPrefab).GetComponent<CardToMainSceneTransition>().SetState(state);

        // Enable the fadeout, which will automatically change scenes are finished.
        fadeOutObject.SetActive(true);

    }

    private void Awake()
    {
        GameplayEventBus.Instance().Subscribe<CardSelectStartedEvent, GameplayStateArgs>(OnCardSelectStarted);
        GameplayEventBus.Instance().Subscribe<CardSelectFinishedEvent, CardSelectArgs>(OnCardSelectFinished);
        if (state == null)
        {
            ResetState();
        }
    }

    private void OnDestroy()
    {
        GameplayEventBus.Instance().Unsubscribe<CardSelectStartedEvent, GameplayStateArgs>(OnCardSelectStarted);
        GameplayEventBus.Instance().Unsubscribe<CardSelectFinishedEvent, CardSelectArgs>(OnCardSelectFinished);
    }

}
