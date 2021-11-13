using System.Collections;
using System.Collections.Generic;

public class PokerCard 
{
    private PokerCardView _View;
    private PokerCardModel _Model;
    public PokerCard(PokerCardView view, PokerCardModel model)
    {
        _View = view;
        _Model = model;
        _View.Skin(_Model.Sprite);
    }
}
