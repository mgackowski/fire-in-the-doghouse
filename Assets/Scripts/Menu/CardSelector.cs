using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Android;

/**
 * By <author>mgackowski</author>.
 */
public class CardSelector : MonoBehaviour
{
    [SerializeField] GameObject selectableCardPrefab;
    [SerializeField] TextMeshProUGUI descriptionText;

    List<Card> selectedCards = new List<Card>();
    int cardNumber = 0;
    int selectLimit = 1;

    public void SpawnCards(List<Card> cards, int selectLimit)
    {
        SelectableCard newCardObject;
        foreach (Card card in cards) {
            newCardObject = Instantiate(selectableCardPrefab, transform).GetComponent<SelectableCard>();
            newCardObject.UpdateCard(this, card);
        }
        cardNumber = cards.Count - 1;
        this.selectLimit = selectLimit;
        ResetDescription();
    }

    public void ChangeDescription(string message)
    {
        descriptionText.text = message;
    }

    public void ResetDescription()
    {
        descriptionText.text = $"Pick {cardNumber} cards in order.";
    }

    public int MarkForSelection(Card selectedCard)
    {
        //TODO: Selecting multiples of same card seems to be glitchy
        selectedCards.Add(selectedCard);
        if ( selectedCards.Count >= selectLimit )
        {
            FinishSelection();
        }
        return selectedCards.Count;
    }

    void FinishSelection()
    {
        GameplayEventBus.Instance().Publish<CardSelectFinishedEvent, CardSelectArgs>(new CardSelectArgs()
        {
            Cards = selectedCards
        });
    }

}
