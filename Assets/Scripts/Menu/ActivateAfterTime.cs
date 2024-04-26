using System.Collections;
using UnityEngine;

/**
 * By <author>electazxz</author>.
 */
public class ActivateAfterTime : MonoBehaviour
{
    public float delay;
    public GameObject fadeOut;
    void Start()
    {
        StartCoroutine(LoadLevelAfterDelay(delay));
    }

    IEnumerator LoadLevelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        fadeOut.SetActive(true);
    }
}
