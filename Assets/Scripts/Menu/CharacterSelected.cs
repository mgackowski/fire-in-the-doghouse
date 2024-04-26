using UnityEngine;

/**
 * By <author>electazxz</author>.
 */
public class CharacterSelected : MonoBehaviour
{
    public Animator character;
    void Start()
    {
        character.GetComponent<Animator>();
        character.SetBool("isClicked", false);
    }

    // Update is called once per frame
    void OnMouseDown()
    {
        character.SetBool("isClicked", true);
    }
}
