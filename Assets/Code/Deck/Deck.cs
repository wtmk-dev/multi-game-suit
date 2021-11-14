using System.Collections;
using System.Collections.Generic;

public class Deck 
{
    public virtual ICard Draw()
    {
        ICard card = _Deck.Dequeue();
        _CardsOutOfDeck.Add(card);
        return card;
    }

    public virtual void InitDeckFromCards()
    {
        _Deck.Clear();
        _CardsOutOfDeck.Clear();
        Shuffle();

        for (int i = 0; i < _Cards.Count; i++)
        {
            _Deck.Enqueue(_Cards[i]);
        }
    }

    public virtual void Shuffle()
    {
        _Tools.Shuffle(_Cards);
    }

    public virtual void ReturnToDeck(ICard card)
    {
        if (_CardsOutOfDeck.Contains(card)) 
        {
            _CardsOutOfDeck.Remove(card);
        }

        _Deck.Enqueue(card);
    }

    private WTMK _Tools = WTMK.Instance;
    private IList<ICard> _Cards;
    private List<ICard> _CardsOutOfDeck = new List<ICard>();
    private Queue<ICard> _Deck = new Queue<ICard>();

    public Deck(IList<ICard> cards)
    {
        _Cards = cards;

        InitDeckFromCards();
    }

    
}
