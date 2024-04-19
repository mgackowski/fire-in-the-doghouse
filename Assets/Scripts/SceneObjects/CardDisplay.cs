using UnityEngine;
using UnityEngine.UI;

public class CardGameObject : MonoBehaviour
{
    public Image cardImage;

    Card card;

    private void Start()
    {
        UpdateAppearance();
    }

    public void SetCard(Card newCard)
    {
        card = newCard;
        UpdateAppearance();
    }

    void UpdateAppearance()
    {
        if (card.graphic != null)
        {
            cardImage.sprite = card.graphic;
        }
    }

}