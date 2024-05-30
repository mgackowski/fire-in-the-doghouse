using UnityEngine;

/**
 * Controls the behaviour of a comedian on stage, in response to game events.
 */
[RequireComponent(typeof(Animator))]
public class PerformingComedian : MonoBehaviour
{
    [SerializeField] ComedianType comedianType;
    [SerializeField] string jokeAnimBoolName = "tellingJoke";

    Animator anim;

    private void OnDialogueStarted(DialogueArgs args)
    {
        if (args.Speaker.Type == comedianType)
        {
            anim.SetBool(jokeAnimBoolName, true);
        }
    }

    /*
     * The animation will continue until OnDialogueFInishedEvent is raised, e.g.
     * by the UI after done rendering the text.
     */
    private void OnDialogueFinished(DefaultEventArgs args)
    {
        anim.SetBool(jokeAnimBoolName, false);
    }

    private void Start()
    {
        anim = GetComponent<Animator>();

        GameplayEventBus.Instance().Subscribe<DialogueStartedEvent, DialogueArgs>(OnDialogueStarted);
        GameplayEventBus.Instance().Subscribe<DialogueFinishedEvent, DefaultEventArgs>(OnDialogueFinished);
    }

    private void OnDestroy()
    {
        GameplayEventBus.Instance().Unsubscribe<DialogueStartedEvent, DialogueArgs>(OnDialogueStarted);
        GameplayEventBus.Instance().Unsubscribe<DialogueFinishedEvent, DefaultEventArgs>(OnDialogueFinished);
    }

}
