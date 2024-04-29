using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Audience : MonoBehaviour
{
    [SerializeField] List<AudioClip> reactionClips;

    [SerializeField] bool triggerFinishedEvent = true;
    [SerializeField] float triggerAfterSeconds = 2f;

    AudioSource audioSource;

    public void OnScoreResolved(ScoreArgs args)
    {
        int score = args.TurnScore;
        Mathf.Clamp(score, 0, 6);
        audioSource.clip = reactionClips[score];
        audioSource.Play();

        if (triggerFinishedEvent)
        {
            StartCoroutine(WaitForFinish());
        }
    }

    private IEnumerator WaitForFinish()
    {
        yield return new WaitForSeconds(triggerAfterSeconds);
        //TODO: DefaultEventArgs should provide a prototype without instantiation
        GameplayEventBus.Instance().Publish<ScoreResolutionFinishedEvent, DefaultEventArgs>(new DefaultEventArgs());
    }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        GameplayEventBus.Instance().Subscribe<ScoreResolutionStartedEvent, ScoreArgs>(OnScoreResolved);
    }

    private void OnDestroy()
    {
        GameplayEventBus.Instance().Unsubscribe<ScoreResolutionStartedEvent, ScoreArgs>(OnScoreResolved);
    }

}
