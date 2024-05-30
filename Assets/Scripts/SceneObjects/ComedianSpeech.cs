using System.Collections;
using TMPro;
using UnityEngine;

/**
 * Listens to the DialogueStartedEvent and animates character text.
 * More importantly, triggers the DialogueFinishedEvent.
 */
[RequireComponent(typeof(TextMeshProUGUI))]
public class ComedianSpeech : MonoBehaviour
{
    [SerializeField] ComedianType comedianType;
    [SerializeField] bool useTypewriterEffect = true;
    [SerializeField][Min(float.Epsilon)] float charactersPerSecond = 30f;
    [SerializeField] float waitBeforeEventFinished = 2f;
    [SerializeField] float waitBeforeClearingText = 0f;

    TextMeshProUGUI textComponent;

    private void Awake()
    {
        GameplayEventBus.Instance().Subscribe<DialogueStartedEvent, DialogueArgs>(OnDialogueStarted);
        textComponent = GetComponent<TextMeshProUGUI>();

        if (charactersPerSecond <= 0f)
        {
            Debug.LogWarning("ComedianSpeech will not animate text as charactersPerSecond is not a positive value.");
        }
    }

    private void OnDestroy()
    {
        GameplayEventBus.Instance().Unsubscribe<DialogueStartedEvent, DialogueArgs>(OnDialogueStarted);
    }
    private void OnDialogueStarted(DialogueArgs args)
    {
        if (args.Speaker.Type == comedianType)
        {
            StartCoroutine(DisplayLine(args.DialogueLine));
        }
    }

    private IEnumerator DisplayLine(string line)
    {
        if (!useTypewriterEffect || charactersPerSecond <= 0f)
        {
            textComponent.text = line;
        }
        else
        {
            textComponent.text = "";
            for (int i = 0; i < line.Length; i++)
            {
                textComponent.text += line[i];
                yield return new WaitForSeconds(1 / charactersPerSecond);
            }
        }
        yield return new WaitForSeconds(waitBeforeEventFinished);

        //TODO: DefaultEventArgs cound use a Prototype / cached instance since it is virtually unchanging
        GameplayEventBus.Instance().Publish<DialogueFinishedEvent, DefaultEventArgs>(new DefaultEventArgs());

        yield return new WaitForSeconds(waitBeforeClearingText);
        textComponent.text = "";

    }

}