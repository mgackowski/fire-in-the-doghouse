using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTester : MonoBehaviour
{
    // Script is just to test audio manager functions

    public SoundManager soundManag;
    private int i;
    void Start()
    {
        i = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            soundManag.playHoverSound();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            soundManag.playCardSelection();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            soundManag.playAudienceCheer(i);
            ++i;
        }
    }
}
