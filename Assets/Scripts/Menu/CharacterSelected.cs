using UnityEngine;

/**
 * By <author>electazxz</author>, modified by <author>mgackowski</author>.
 */
public class CharacterSelected : MonoBehaviour
{
    [SerializeField] Character characterModel;
    [SerializeField] string characterName;

    [SerializeField] Animator character;

    void Start()
    {
        character.GetComponent<Animator>();
        character.SetBool("isClicked", false);
    }

    void OnMouseDown()
    {
        character.SetBool("isClicked", true);

        CharacterSelectArgs args = new CharacterSelectArgs()
        {
            CharacterModel = characterModel,
            CharacterName = characterName
        };
        GameplayEventBus.Instance().Publish<CharacterSelectedEvent, CharacterSelectArgs>(args);
    }
}
