using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] ComedianType owner;
    TextMeshProUGUI textComponent;

    void OnScoreResolutionFinished(ScoreArgs args)
    {
        if (args.TurnPlayer.Type != owner)
        {
            return;
        }
        textComponent.text = args.TotalScore.ToString();
        //Play animation here to draw attention
        StartCoroutine(EmphasizeText());
        
    }

    /*
     * A simple effect for emphasis until we develop the UI more fully
     */
    private IEnumerator EmphasizeText(float scale = 3f, float duration = 0.3f)
    {
        float originalSize = textComponent.fontSize;
        textComponent.fontSize *= scale;
        yield return new WaitForSeconds(duration);
        textComponent.fontSize = originalSize;
    }

    void Awake()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
        GameplayEventBus.Instance().Subscribe<ScoreResolutionFinishedEvent, ScoreArgs>(OnScoreResolutionFinished);
    }

    void OnDestroy()
    {
        GameplayEventBus.Instance().Unsubscribe<ScoreResolutionFinishedEvent, ScoreArgs>(OnScoreResolutionFinished);
    }
}
