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

    public string GetHoldemHandKey()
    {
        string key = "";

        switch (_Rank)
        {
            case PokerCardRank.Two:
                key += "2";
                break;
            case PokerCardRank.Three:
                key += "3";
                break;
            case PokerCardRank.Four:
                key += "4";
                break;
            case PokerCardRank.Five:
                key += "5";
                break;
            case PokerCardRank.Six:
                key += "6";
                break;
            case PokerCardRank.Seven:
                key += "7";
                break;
            case PokerCardRank.Eight:
                key += "8";
                break;
            case PokerCardRank.Nine:
                key += "9";
                break;
            case PokerCardRank.Ten:
                key += "t";
                break;
            case PokerCardRank.Jack:
                key += "j";
                break;
            case PokerCardRank.Queen:
                key += "q";
                break;
            case PokerCardRank.King:
                key += "k";
                break;
            case PokerCardRank.Ace:
                key += "a";
                break;
        }

        switch (_Suit)
        {
            case PokerCardSuit.Diamonds:
                key += "d";
                break;
            case PokerCardSuit.Clubs:
                key += "c";
                break;
            case PokerCardSuit.Hearts:
                key += "h";
                break;
            case PokerCardSuit.Spades:
                key += "s";
                break;            
        }

        return key;
    }
}

    