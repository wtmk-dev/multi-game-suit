using System.Collections;
using System.Collections.Generic;

public class Poker : State
{
    public override IStateView View { get { return _View; } }

    private PokerView _View;
    public Poker(PokerView view)
    {
        _View = view;
    }
}
