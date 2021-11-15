using System;
using UnityEngine;

[Serializable]
public class PokerCardModel
{
    public Sprite Front { get { return _Front; } private set { _Front = value; } }
    public Sprite Back { get { return _Back; } private set { _Back = value; } }
    public PokerCardRank Rank { get { return _Rank; } private set { _Rank = value; } }
    public PokerCardSuit Suit { get { return _Suit; } private set { _Suit = value; } }

    private Sprite _Front, _Back;
    private PokerCardRank _Rank;
    private PokerCardSuit _Suit;

    public PokerCardModel(Sprite front, Sprite back, PokerCardRank rank, PokerCardSuit suit)
    {
        Front = front;
        Back = back;
        _Rank = rank;
        _Suit = suit;
    }
}
