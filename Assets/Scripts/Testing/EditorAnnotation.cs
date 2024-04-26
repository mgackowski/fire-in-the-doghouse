using UnityEngine;

/**
 * Use for adding a quick and user-friendly description to an object in the Editor.
 */
public class EditorAnnotation : MonoBehaviour
{

#if UNITY_EDITOR

    [TextArea(6, 6)]
    [SerializeField] string description;

#else
 
    void OnEnable()
    {
        Destroy(this);
    }
 
#endif

}