using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardPlayDisplay : MonoBehaviour
{
    [SerializeField] bool invokeAnimationFinishedEvent = true;

    [Header("Hard appear/disappear (no animation)")]
    [SerializeField] GameObject objectToShow;
    [SerializeField] float durationSeconds = 3f;

    [Header("Text fields")]
    [SerializeField] TextMeshProUGUI cardDescriptionComponent;
    [SerializeField] Image cardSpriteComponent;

    void OnCardPlayAnimationsStarted(CardPlayArgs args)
    {
        cardDescriptionComponent.text = args.CardPlay.card.description;
        cardSpriteComponent.sprite = args.CardPlay.card.graphic;
        StartCoroutine(ShowDisplay());
    }

    private IEnumerator ShowDisplay()
    {
        objectToShow.SetActive(true);
        yield return new WaitForSeconds(durationSeconds);
        objectToShow.SetActive(false);

        if(invokeAnimationFinishedEvent)
        {
            GameplayEventBus.Instance().Publish<CardPlayAnimationsFinishedEvent, DefaultEventArgs>(new DefaultEventArgs());
        }

    }

    private void Awake()
    {
        GameplayEventBus.Instance().Subscribe<CardPlayAnimationsStartedEvent, CardPlayArgs>(OnCardPlayAnimationsStarted);
        objectToShow.SetActive(false);
    }

    private void OnDestroy()
    {
        GameplayEventBus.Instance().Unsubscribe<CardPlayAnimationsStartedEvent, CardPlayArgs>(OnCardPlayAnimationsStarted);
    }

}
