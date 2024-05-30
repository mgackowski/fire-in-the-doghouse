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
            cardsToDeal.Add(state.HumanDeck[selectedIndex]);
            state.HumanDeck.RemoveAt(selectedIndex);
        }
        cardSelector.SpawnCards(cardsToDeal);

    }

    private void OnCardSelectFinished(DefaultEventArgs args)
    {
        // Finally, create an object to transfer this information to the next scene:
        Instantiate(nextSceneMessengerPrefab);

        // Other objects in the scene are scripted to perform a scene transition when animations
        // are finished.

    }

    private void Awake()
    {
        GameplayEventBus.Instance().Subscribe<CardSelectStartedEvent, GameplayStateArgs>(OnCardSelectStarted);
        if (state == null)
        {
            ResetState();
        }
    }

    private void OnDestroy()
    {
        GameplayEventBus.Instance().Unsubscribe<CardSelectStartedEvent, GameplayStateArgs>(OnCardSelectStarted);

    }

}
