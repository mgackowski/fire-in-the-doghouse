using UnityEngine;

/**
 * By <author>electazxz</author>.
 */
public class StopMusic : MonoBehaviour
{
    void Start()
    {
        GameObject.FindGameObjectWithTag("Music").GetComponent<MusicPlayer>().StopMusic();
    }

}
