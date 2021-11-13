using System;
using UnityEngine;

[Serializable]
public class PokerCardModel
{
    public Sprite Sprite { get { return _Sprite; } private set { _Sprite = value; } }
    public PokerCardRank Rank { get { return _Rank; } private set { _Rank = value; } }
    public PokerCardSuit Suit { get { return _Suit; } private set { _Suit = value; } }

    private Sprite _Sprite;
    private PokerCardRank _Rank;
    private PokerCardSuit _Suit;

    public PokerCardModel(Sprite sprite, PokerCardRank rank, PokerCardSuit suit)
    {
        Sprite = sprite;
    }
}
