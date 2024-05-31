using System.Collections.Generic;
using UnityEngine;

/**
 * Instantiate the characters based on GameplayState - this allows for dynamic
 * placement of characters based on what was chosen on the character select screen.
 * By <author>mgackowski</author>.
 */
public class ComedianSpawner : MonoBehaviour
{
    [SerializeField] ComedianType spawnType;

    [Header("Character prefabs (Awake event only)")]
    [SerializeField] GameObject jamPrefab;
    [SerializeField] GameObject peanutPrefab;

    Dictionary<Character, GameObject> characters = new Dictionary<Character, GameObject>();

    private void OnActIntroStarted(GameplayStateArgs args)
    {
        if (spawnType == ComedianType.PLAYER)
        {
            Instantiate(characters[args.State.HumanComedian.Model], transform);
        }
        else if (spawnType == ComedianType.CPU)
        {
            Instantiate(characters[args.State.CpuComedian.Model], transform);
        }
        
    }

    private void Awake()
    {
        GameplayEventBus.Instance().Subscribe<ActIntroStartedEvent, GameplayStateArgs>(OnActIntroStarted);
        characters.Add(Character.JAM, jamPrefab);
        characters.Add(Character.PEANUT, peanutPrefab);
    }

    private void OnDestroy()
    {
        GameplayEventBus.Instance().Unsubscribe<ActIntroStartedEvent, GameplayStateArgs>(OnActIntroStarted);
    }
}
