using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokerDeckFactory : MonoBehaviour
{
    public PokerDeck Build_DeckFromDefinition(PokerDeckDefinition deckDefinition)
    {
        var models = _PokerModelFactory.Build_DeckFromDefinition(deckDefinition);
        var cards = Build_PokerCardsFromModels(models, deckDefinition);
        return new PokerDeck(cards);
    }

    private PokerModelFactory _PokerModelFactory = new PokerModelFactory();
    private List<ICard> Build_PokerCardsFromModels(List<PokerCardModel> models, PokerDeckDefinition deckDefinition)
    {
        var cards = new List<ICard>();
        for (int i = 0; i < models.Count; i++)
        {
            GameObject clone = Instantiate(deckDefinition.PokerCardPrefab);
            PokerCardView view = clone.GetComponent<PokerCardView>();
            PokerCard card = new PokerCard(view, models[i]);
            cards.Add(card);
        }
        return cards;
    }
}
