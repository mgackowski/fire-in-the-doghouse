using System.Collections.Generic;
using UnityEngine;

/**
 * By <author>mgackowski</author>.
 */
public class CardSelector : MonoBehaviour
{
    [SerializeField] GameObject selectableCardPrefab;

    public void SpawnCards(List<Card> cards)
    {
        SelectableCard newCardObject;
        foreach (Card card in cards) {
            newCardObject = Instantiate(selectableCardPrefab, transform).GetComponent<SelectableCard>();
            newCardObject.UpdateCard(card);
        }
    }

}
