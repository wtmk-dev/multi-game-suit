using System.Collections;
using System.Collections.Generic;

public class Poker : State
{
    public override string Tag { get; protected set; }
    public override IStateView View { get { return _View; } }

    private PokerView _View;
    public Poker(PokerView view, string tag)
    {
        _View = view;
        Tag = tag;
    }
}
