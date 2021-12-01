using System.Collections;
using System.Collections.Generic;

public class TributeDeckFactory
{
    public TributeDeck Build_DeckFromDefinition(TributeDeckDefinition deckDefinition)
    {
        var models = _TributeModelFactory.Build_DeckFromDefinition(deckDefinition);
        var cards = Build_TributeCardsFromModels(models, deckDefinition);
        return new TributeDeck(cards);
    }

    private TributeModelFactory _TributeModelFactory = new TributeModelFactory();
    private List<ICard> Build_TributeCardsFromModels(List<TributeCardModel> models, TributeDeckDefinition deckDefinition)
    {
        var cards = new List<ICard>();
        for (int i = 0; i < models.Count; i++)
        {
            TributeCard card = new TributeCard(models[i]);
            cards.Add(card);
        }
        return cards;
    }
}
