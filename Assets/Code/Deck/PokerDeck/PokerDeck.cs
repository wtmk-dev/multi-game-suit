using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokerDeck : Deck
{
    public List<PokerCard> Cards { get { return _Cards; } }
    private List<PokerCard> _Cards;

    public PokerDeck(List<ICard> cards) : base(cards)
    {
        _Cards = UpCastCards(cards);
    }

    private List<PokerCard> UpCastCards(List<ICard> cards)
    {
        var pokerCards = new List<PokerCard>();
        for (int i = 0; i < cards.Count; i++)
        {
            pokerCards.Add((PokerCard)cards[i]);
        }
        return pokerCards;
    }
}
