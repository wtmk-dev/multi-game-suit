using System.Collections;
using System.Collections.Generic;

public class Blackjack : State 
{
    public override IStateView View { get { return _View; } }

    private BlackjackView _View;
    public Blackjack(BlackjackView view)
    {
        _View = view;
    }
}
