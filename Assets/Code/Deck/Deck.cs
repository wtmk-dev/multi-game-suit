using System.Collections;
using System.Collections.Generic;

public class Deck 
{
    private IList<ICard> _Cards;
    public Deck(IList<ICard> cards)
    {
        _Cards = cards;
    }
}
