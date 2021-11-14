using System.Collections;
using System.Collections.Generic;

public class PokerCard : ICard
{
    public PokerCardView View { get { return _View; } }
    private PokerCardView _View;
    private PokerCardModel _Model;

    public void Init()
    {
        _View.Skin(_Model.Sprite);
    }

    public PokerCard(PokerCardView view, PokerCardModel model)
    {
        _View = view;
        _Model = model;
    }
}
