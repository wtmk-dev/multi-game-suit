using System;
using UnityEngine;

[Serializable]
public class TributeCardModel
{
    public Sprite Front { get { return _Front; } private set { _Front = value; } }
    public Sprite Back { get { return _Back; } private set { _Back = value; } }
    public TributeCardRank Rank { get { return _Rank; } private set { _Rank = value; } }
    public TributeCardSuit Suit { get { return _Suit; } private set { _Suit = value; } }

    private Sprite _Front, _Back;
    private TributeCardRank _Rank;
    private TributeCardSuit _Suit;

    public TributeCardModel(Sprite front, Sprite back, TributeCardRank rank, TributeCardSuit suit)
    {
        Front = front;
        Back = back;
        _Rank = rank;
        _Suit = suit;
    }
}

    