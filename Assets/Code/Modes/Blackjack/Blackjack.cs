using System.Collections;
using System.Collections.Generic;

public class Blackjack : State 
{
    public override string Tag { get; protected set; }
    public override IStateView View { get { return _View; } }

    private BlackjackView _View;
    private GameModeTag _GameModeTag = new GameModeTag();
    public Blackjack(BlackjackView view)
    {
        _View = view;
        Tag = _GameModeTag.Blackjack;
    }
}
