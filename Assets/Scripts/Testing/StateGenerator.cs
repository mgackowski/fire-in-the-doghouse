using UnityEngine;

/**
 * Create a GameplayState and raise event to start a game with it.
 * Use for testing.
 */
public class StateGenerator : MonoBehaviour
{
    [SerializeField] Deck playerDeck;
    [SerializeField] Deck cpuDeck;

    [Header("Randomise cards picked")]
    [SerializeField] int addFromDeck = 5; // number of cards to add to card queue

    private void Start()
    {
        GameplayState state = new GameplayState();

        state.HumanComedian = new Comedian("Player", ComedianType.PLAYER);
        state.CpuComedian = new Comedian("CPU", ComedianType.CPU);

        state.HumanDeck.Clear();
        state.HumanDeck.AddRange(playerDeck.cards);
        state.CpuDeck.Clear();
        state.CpuDeck.AddRange(cpuDeck.cards);

        // randomise card choices
        for (int i = 0; i < addFromDeck; i++)
        {
            int humanCardIndex = Random.Range(0, state.HumanDeck.Count);
            int cpuCardIndex = Random.Range(0, state.CpuDeck.Count);

            CardPlay randomPlay = new CardPlay()
            {
                card = state.HumanDeck[humanCardIndex],
                player = state.HumanComedian
            };
            state.HumanDeck.RemoveAt(humanCardIndex);
            state.CardQueue.Enqueue(randomPlay);

            randomPlay = new CardPlay()
            {
                card = state.CpuDeck[cpuCardIndex],
                player = state.CpuComedian
            };
            state.CpuDeck.RemoveAt(cpuCardIndex);
            state.CardQueue.Enqueue(randomPlay);
        }

        // raise event to start the act
        GameplayStateArgs args = new GameplayStateArgs()
        {
            State = state
        };
        GameplayEventBus.Instance().Publish<ActIntroStartedEvent, GameplayStateArgs>(args);

    }
}
