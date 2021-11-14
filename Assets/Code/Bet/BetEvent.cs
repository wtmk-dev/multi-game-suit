using System.Collections;
using System.Collections.Generic;

public class BetEvent
{
    public readonly string Bet = "BE!Bet";
}

public class BetEventArgs
{
    public int Bet { get { return _Bet; } private set { _Bet = value; } }
    private int _Bet;
    public BetEventArgs(int bet)
    {
        _Bet = bet;
    }
}
