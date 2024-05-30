using UnityEngine;
using UnityEngine.UI;

/**
 * By <author>mgackowski</author>.
 */
public class SelectableCard : MonoBehaviour
{
    [SerializeField] Image cardImage;

    Card card;

    public void UpdateCard(Card newCard)
    {
        card = newCard;
        cardImage.sprite = card.graphic;
    }

}