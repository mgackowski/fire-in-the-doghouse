using UnityEngine;
using UnityEngine.SceneManagement;

public class CardToMainSceneTransition : MonoBehaviour
{
    GameplayState state;

    public void SetState(GameplayState stateToCarry)
    {
        state = stateToCarry;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameplayStateArgs args = new GameplayStateArgs()
        {
            State = state
        };
        GameplayEventBus.Instance().Publish<ActIntroStartedEvent, GameplayStateArgs>(args);
        Destroy(gameObject);
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
