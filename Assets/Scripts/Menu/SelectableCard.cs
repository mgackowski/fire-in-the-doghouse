using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/**
 * By <author>mgackowski</author>.
 */
public class SelectableCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] Image cardImage;
    [SerializeField] TextMeshProUGUI selectNumberText;

    Card card;
    CardSelector selector;
    bool selectable = true;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!selectable)
        {
            return;
        }
        int count = selector.MarkForSelection(card);
        selectNumberText.text = count.ToString();
        selectable = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!selectable)
        {
            return;
        }
        selector.ChangeDescription($"{card.cardName} : {card.description}");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!selectable)
        {
            return;
        }
        selector.ResetDescription();
    }

    public void UpdateCard(CardSelector selector, Card newCard)
    {
        card = newCard;
        this.selector = selector;
        cardImage.sprite = card.graphic;
    }

}