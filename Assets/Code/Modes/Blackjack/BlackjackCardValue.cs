using System.Collections;
using System.Collections.Generic;

public class BlackjackCardValue
{
    public readonly int HighetsToStillHit = 20;
    public Dictionary<PokerCardRank, int> CardValue { get { return _CardValue; } }
    private Dictionary<PokerCardRank, int> _CardValue = new Dictionary<PokerCardRank, int>();

    public BlackjackCardValue()
    {
        BuildCardValue();
    }

    private void BuildCardValue()
    {
        _CardValue.Add(PokerCardRank.Ace, 1);
        _CardValue.Add(PokerCardRank.Two, 2);
        _CardValue.Add(PokerCardRank.Three, 3);
        _CardValue.Add(PokerCardRank.Four, 4);
        _CardValue.Add(PokerCardRank.Five, 5);
        _CardValue.Add(PokerCardRank.Six, 6);
        _CardValue.Add(PokerCardRank.Seven, 7);
        _CardValue.Add(PokerCardRank.Eight, 8);
        _CardValue.Add(PokerCardRank.Nine, 9);
        _CardValue.Add(PokerCardRank.Ten, 10);
        _CardValue.Add(PokerCardRank.Jack, 10);
        _CardValue.Add(PokerCardRank.Queen, 10);
        _CardValue.Add(PokerCardRank.King, 10);
    }
}