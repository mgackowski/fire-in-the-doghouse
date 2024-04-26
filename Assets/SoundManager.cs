using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Tooltip("List of audience sounds for when joke lands")]
    public List<AudioClip> audienceSound;
    [Tooltip("Sound that plays when mouse hovers over a card")]
    public AudioClip hoverSound;
    [Tooltip("Sound that plays when selecting a card")]
    public AudioClip selectSound;
    [Tooltip("List of talking sounds for when a character speaks")]
    public List<AudioClip> avatarSpeech;
    private AudioSource audioSource; // Reference to Audio source
    public AudioSource musicSource;
    public bool isInDrawPhase = false; // Bool for tracking when to play music
     void Start()
    {
        audioSource = GetComponent<AudioSource>();
      //  musicSource = GetComponentInChildren<AudioSource>();
        musicSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (isInDrawPhase && !musicSource.isPlaying)
        {
            musicSource.Play();
        }
        else
        {
            if (!isInDrawPhase)
            {
                musicSource.Pause();
            }
        }
    } 
    public void playCardSelection()
    {
        audioSource.PlayOneShot(selectSound);
    }

    public void playHoverSound()
    {
        audioSource.PlayOneShot(hoverSound);
    }

    public void playAudienceCheer(int level)
    {
        audioSource.PlayOneShot(audienceSound[level]);  
    }

}
