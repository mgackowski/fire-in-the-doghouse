using System.Collections.Generic;
using UnityEngine;

public class Audience : MonoBehaviour
{


    public List<AudioClip> reactionClips;
    public NightManager manager;

    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        //UpdateActBindings();
    }

    public void UpdateActBindings()
    {
        SubscribeToActEvents();
    }

    public void OnScoreResolved(int score)
    {
        Mathf.Clamp(score, 0, 6);
        audioSource.clip = reactionClips[score];
        audioSource.Play();
    }

    void SubscribeToActEvents()
    {
        if (manager.act == null)
        {
            return;
        }
        manager.act.ScoreResolutionEvent += OnScoreResolved;
    }

}
