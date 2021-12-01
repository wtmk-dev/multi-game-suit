using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TributeModelFactory 
{ 
    public List<TributeCardModel> Build_DeckFromDefinition(TributeDeckDefinition deckDefinition)
    {
        List<TributeCardModel> deck = new List<TributeCardModel>();
        //deck.AddRange(Build_SuitFromSprite(TributeCardSuit.Spades, deckDefinition.Spades, deckDefinition.Back));
        //deck.AddRange(Build_SuitFromSprite(TributeCardSuit.Hearts, deckDefinition.Hearts, deckDefinition.Back));
        //deck.AddRange(Build_SuitFromSprite(TributeCardSuit.Clubs, deckDefinition.Clubs, deckDefinition.Back));
        //deck.AddRange(Build_SuitFromSprite(TributeCardSuit.Diamonds, deckDefinition.Diamonds, deckDefinition.Back));

        return deck;
    }

    private List<TributeCardModel> Build_SuitFromSprite(TributeCardSuit suit, List<Sprite> sprite, Sprite back)
    {
        var cards = new List<TributeCardModel>();
        for (int i = 0; i < sprite.Count; i++)
        {
            cards.Add(new TributeCardModel(sprite[i], back, (TributeCardRank)i, suit));
        }

        return cards;
    }
}
