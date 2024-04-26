using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * By <author>electazxz</author>.
 */
public class SceneChange : MonoBehaviour
{
    public float delay;
    public string NewLevel;
    void Start()
    {
        StartCoroutine(LoadLevelAfterDelay(delay));
    }

    IEnumerator LoadLevelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(NewLevel);
    }
}
