using UnityEngine;
/**
 * Sets a GameObject to active/inactive if a card effect is played.
 * Limited to single enabling / disabling effect.
 * By <author>mgackowski</author>.
 */
public class EffectStatusDisplay : MonoBehaviour
{
    [SerializeField] string enablingEffectName;
    [SerializeField] ComedianType enablingEffectInvoker;
    [SerializeField] string disablingEffectName;
    [SerializeField] ComedianType disablingEffectInvoker;

    [SerializeField] GameObject displayContainer;

    private void OnEffectResolutionStarted(CardEffectArgs args)
    {
        if (args.EffectName.Equals(enablingEffectName) && args.Target.Type == enablingEffectInvoker)
        {
            displayContainer.SetActive(true);
        }
        else if (args.EffectName.Equals(disablingEffectName) && args.Target.Type == disablingEffectInvoker)
        {
            displayContainer.SetActive(false);
        }

    }

    private void Awake()
    {
        GameplayEventBus.Instance().Subscribe<EffectResolutionStartedEvent, CardEffectArgs>(OnEffectResolutionStarted);
    }

    private void OnDestroy()
    {
        GameplayEventBus.Instance().Unsubscribe<EffectResolutionStartedEvent, CardEffectArgs>(OnEffectResolutionStarted);
    }

}
