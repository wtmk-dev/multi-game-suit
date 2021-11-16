using System.Collections;
using System.Collections.Generic;

public class BetEvent
{
    public readonly string Bet = "BE!Bet";
}

public class BetEventArgs
{
    public int BetMulti { get { return _BetMulti; } private set { _BetMulti = value; } }
    public int Bet { get { return _Bet; } private set { _Bet = value; } }
    private int _Bet;
    private int _BetMulti;
    public BetEventArgs(int bet, int betMulti)
    {
        _Bet = bet;
        _BetMulti = betMulti;
    }
}
