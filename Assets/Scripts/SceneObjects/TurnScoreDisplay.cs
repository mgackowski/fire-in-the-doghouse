using System.Collections;
using TMPro;
using UnityEngine;

public class TurnScoreDisplay : MonoBehaviour
{
    [SerializeField] bool invokeScoreResolutionFinishedEvent = true;

    [Header("Hard appear/disappear (no animation)")]
    [SerializeField] GameObject objectToShow;
    [SerializeField] float durationSeconds = 3f;

    [Header("Text fields")]
    [SerializeField] TextMeshProUGUI scoreTextboxComponent;

    void OnScoreResolutionStarted(ScoreArgs args)
    {
        scoreTextboxComponent.text = args.TurnScore.ToString();
        StartCoroutine(ShowDisplay(args));
    }

    private IEnumerator ShowDisplay(ScoreArgs args)
    {
        objectToShow.SetActive(true);
        yield return new WaitForSeconds(durationSeconds);
        objectToShow.SetActive(false);

        if(invokeScoreResolutionFinishedEvent)
        {
            GameplayEventBus.Instance().Publish<ScoreResolutionFinishedEvent, ScoreArgs>(args);
        }

    }

    private void Awake()
    {
        GameplayEventBus.Instance().Subscribe<ScoreResolutionStartedEvent, ScoreArgs>(OnScoreResolutionStarted);
        objectToShow.SetActive(false);
    }

    private void OnDestroy()
    {
        GameplayEventBus.Instance().Unsubscribe<ScoreResolutionStartedEvent, ScoreArgs>(OnScoreResolutionStarted);
    }

}
