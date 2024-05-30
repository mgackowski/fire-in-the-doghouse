using UnityEngine;

/**
 * By <author>mgackowski</author>.
 */
public class CharacterSelectCoordinator : MonoBehaviour
{
    [SerializeField] GameObject nextSceneMessengerPrefab;
    GameplayState state;

    public void SetState(GameplayState newState)
    {
        state = newState;
    }

    public void ResetState()
    {
        state = new GameplayState();
    }

    private void OnCharacterSelected(CharacterSelectArgs args)
    {
        // Assuming only Jam and Peanut characters in game...
        state.HumanComedian = new Comedian(args.CharacterName, ComedianType.PLAYER, args.CharacterModel);
        // We're hard-wiring the choice of opponent to whichever is the 'other one':
        if (args.CharacterModel == Character.PEANUT)
        {
            state.CpuComedian = new Comedian("Jam", ComedianType.CPU, Character.JAM);
        }
        else
        {
            state.CpuComedian = new Comedian("Peanut", ComedianType.CPU, Character.PEANUT);
        }

        // Finally, create an object to transfer this information to the next scene:
        Instantiate(nextSceneMessengerPrefab).GetComponent<CharacterToCardSceneTransition>().SetState(state);

        // Other objects in the scene are scripted to perform a scene transition when animations
        // are finished.

    }

    private void Awake()
    {
        GameplayEventBus.Instance().Subscribe<CharacterSelectedEvent, CharacterSelectArgs>(OnCharacterSelected);
        if (state == null)
        {
            ResetState();
        }
    }

    private void OnDestroy()
    {
        GameplayEventBus.Instance().Unsubscribe<CharacterSelectedEvent, CharacterSelectArgs>(OnCharacterSelected);

    }

}
