using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Its presence in a Scene will cause the card queue in GameplayState to resolve.
 * In other words, it plays out the act start to finish. Various events are raised.
 */
public class ActCoordinator : MonoBehaviour
{
    // Replace with proper state machine if things get complex
    enum ActState
    {
        Introduction, AwaitingCardAnimationFinish
    }


    // place Inspector fields here that control playback, i.e., enabled, speed etc


    DialogueGenerator dialogueGenerator;

    public void DoStuff(GameplayState state)
    {
        // when Act starts

        // Prepare GameplayState for beginning of Act
        PrepareAct(state);



        // raise appropriate events based on a timer,
        // some of them should include callbacks to methods here


    }

    public void ResolveCard(GameplayState state)
    {
        CardPlay currentPlay;
        if (!state.CardQueue.TryDequeue(out currentPlay))
        {
            Debug.LogError("Attempted to resolve card but Q empty.");
            return;
        }

        Comedian player = currentPlay.player;
        Card currentCard = currentPlay.card;

        /* Replace with event for UI. Potentially use a callback?
         * here's the problem: we want execution to wait if there's an animation
         * (end method here and listen to event when anim finished).
         * But if there's no anim, we just want this to continue.
         * How? Figure this out!
         */
        

        MessageSystem.Push($"{player.ComedianName} plays {currentPlay.card.cardName}.", MessageType.SYSTEM);

        int lineNumber = Random.Range(0, currentCard.dialogueLines.Count);
        string dialogueLine = dialogueGenerator.GetSentence(
            currentCard.dialogueLines[lineNumber],
            state.CurrentNoun, state.CurrentAdjective,
            player.Type == ComedianType.PLAYER ? state.CpuComedian.ComedianName : state.HumanComedian.ComedianName);
        
        MessageSystem.Push(dialogueLine, player.Type == ComedianType.PLAYER ? MessageType.DIALOGUE_PLAYER : MessageType.DIALOGUE_CPU);

        // Invoke dialogue started event.


    }



    public void PrepareAct(GameplayState state)
    {
        state.CurrentNoun = dialogueGenerator.GetRandomNoun();
        state.CurrentAdjective = dialogueGenerator.GetRandomAdjective();

        state.HumanComedian.ResetComedian();
        state.CpuComedian.ResetComedian();

    }

    
}
