using System.Collections;
using UnityEngine;

/**
 * By <author>electazxz</author>.
 */
public class MouthAnimation : MonoBehaviour
{
    public float timeDelay;
    public Material expressionOne;
    public Material expressionTwo;
    public Material expressionThree;
    public Material expressionFour;
    public GameObject Object;
    public AudioSource Talking;

    private void Start()
    {
        StartCoroutine(ActivationRoutine());
    }

    private IEnumerator ActivationRoutine()
    {
        while (Talking.isPlaying)
        {
            yield return new WaitForSeconds(timeDelay);
            Object.GetComponent<SkinnedMeshRenderer>().material = expressionOne;
            yield return new WaitForSeconds(timeDelay);
            Object.GetComponent<SkinnedMeshRenderer>().material = expressionTwo;
            yield return new WaitForSeconds(timeDelay);
            Object.GetComponent<SkinnedMeshRenderer>().material = expressionThree;
            yield return new WaitForSeconds(timeDelay);
            Object.GetComponent<SkinnedMeshRenderer>().material = expressionFour;
        }
    }
}

