using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TributeDeck : Deck
{
    public List<TributeCard> Cards { get { return _Cards; } }
    private List<TributeCard> _Cards;

    public TributeDeck(List<ICard> cards) : base(cards)
    {
        _Cards = UpCastCards(cards);
    }

    private List<TributeCard> UpCastCards(List<ICard> cards)
    {
        var tributeCards = new List<TributeCard>();
        for (int i = 0; i < cards.Count; i++)
        {
            tributeCards.Add((TributeCard)cards[i]);
        }
        return tributeCards;
    }
}
