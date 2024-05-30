using UnityEngine;

/**
 * By <author>electazxz</author>. Edited by <author>mgackowski</author>.
 */
public class ActivateObjectKeyDown : MonoBehaviour
{
    public GameObject enabledObject;
    void Update()
    {
        if (Input.anyKey)
        {
            enabledObject.SetActive(true);
        }
    }
}