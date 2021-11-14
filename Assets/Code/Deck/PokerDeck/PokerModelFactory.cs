using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokerModelFactory 
{ 
    public List<PokerCardModel> Build_DeckFromDefinition(PokerDeckDefinition deckDefinition)
    {
        List<PokerCardModel> deck = new List<PokerCardModel>();
        deck.AddRange(Build_SuitFromSprite(PokerCardSuit.Spades, deckDefinition.Spades, deckDefinition.Back));
        deck.AddRange(Build_SuitFromSprite(PokerCardSuit.Hearts, deckDefinition.Hearts, deckDefinition.Back));
        deck.AddRange(Build_SuitFromSprite(PokerCardSuit.Clubs, deckDefinition.Clubs, deckDefinition.Back));
        deck.AddRange(Build_SuitFromSprite(PokerCardSuit.Diamonds, deckDefinition.Diamonds, deckDefinition.Back));

        return deck;
    }

    private List<PokerCardModel> Build_SuitFromSprite(PokerCardSuit suit, List<Sprite> sprite, Sprite back)
    {
        var cards = new List<PokerCardModel>();
        for (int i = 0; i < sprite.Count; i++)
        {
            cards.Add(new PokerCardModel(sprite[i], back, (PokerCardRank)i, suit));
        }

        return cards;
    }
}
