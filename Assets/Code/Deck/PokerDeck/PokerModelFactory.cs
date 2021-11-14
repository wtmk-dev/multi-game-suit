using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokerModelFactory 
{ 
    public List<PokerCardModel> Build_DeckFromDefinition(PokerDeckDefinition deckDefinition)
    {
        List<PokerCardModel> deck = new List<PokerCardModel>();
        deck.AddRange(Build_SuitFromSprite(PokerCardSuit.Spades, deckDefinition.Spades));
        deck.AddRange(Build_SuitFromSprite(PokerCardSuit.Hearts, deckDefinition.Hearts));
        deck.AddRange(Build_SuitFromSprite(PokerCardSuit.Clubs, deckDefinition.Clubs));
        deck.AddRange(Build_SuitFromSprite(PokerCardSuit.Diamonds, deckDefinition.Diamonds));

        return deck;
    }

    private List<PokerCardModel> Build_SuitFromSprite(PokerCardSuit suit, List<Sprite> sprite)
    {
        var cards = new List<PokerCardModel>();
        for (int i = 0; i < sprite.Count; i++)
        {
            cards.Add(new PokerCardModel(sprite[i], (PokerCardRank)i, suit));
        }

        return cards;
    }
}
