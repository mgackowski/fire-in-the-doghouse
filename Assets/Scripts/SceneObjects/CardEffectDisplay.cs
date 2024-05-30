using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardEffectDisplay : MonoBehaviour
{
    [SerializeField] bool invokeAnimationFinishedEvent = true;

    [Header("Hard appear/disappear (no animation)")]
    [SerializeField] GameObject objectToShow;
    [SerializeField] float durationSeconds = 3f; // First event to finish will trigger EffectResolutionFinishedEvent

    [Header("Text fields")]
    [SerializeField] TextMeshProUGUI effectNameComponent;
    [SerializeField] TextMeshProUGUI effectTargetComponent;

    void OnEffectResolutionStarted(CardEffectArgs args)
    {
        effectNameComponent.text = args.EffectName;
        effectTargetComponent.text = args.Target.ComedianName;
        StartCoroutine(ShowDisplay());
    }

    private IEnumerator ShowDisplay()
    {
        objectToShow.SetActive(true);
        yield return new WaitForSeconds(durationSeconds);
        objectToShow.SetActive(false);

        if(invokeAnimationFinishedEvent)
        {
            GameplayEventBus.Instance().Publish<EffectResolutionFinishedEvent, DefaultEventArgs>(new DefaultEventArgs());
        }

    }

    private void Awake()
    {
        GameplayEventBus.Instance().Subscribe<EffectResolutionStartedEvent, CardEffectArgs>(OnEffectResolutionStarted);
        objectToShow.SetActive(false);
    }

    private void OnDestroy()
    {
        GameplayEventBus.Instance().Unsubscribe<EffectResolutionStartedEvent, CardEffectArgs>(OnEffectResolutionStarted);
    }

}
