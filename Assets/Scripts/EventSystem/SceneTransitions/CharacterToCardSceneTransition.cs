using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterToCardSceneTransition : MonoBehaviour
{
    GameplayState state;

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameplayStateArgs args = new GameplayStateArgs()
        {
            State = state
        };
        GameplayEventBus.Instance().Publish<CardSelectStartedEvent, GameplayStateArgs>(args);
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
