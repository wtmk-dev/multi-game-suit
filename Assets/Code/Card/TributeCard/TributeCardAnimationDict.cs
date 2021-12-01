/*
using System.Collections;
using System.Collections.Generic;


public class PokerCardAnimationDict 
{
    public Dictionary<(PokerCardSuit, PokerCardRank), string> AnimationID { get { return _AnimationID; } }

    private Dictionary<(PokerCardSuit, PokerCardRank), string> _AnimationID = new Dictionary<(PokerCardSuit, PokerCardRank), string>();
    public PokerCardAnimationDict()
    {
        BuildAnimationID();
    }

    private void BuildAnimationID()
    {
        _AnimationID.Add((PokerCardSuit.Diamonds, PokerCardRank.Ace), "DA");
        _AnimationID.Add((PokerCardSuit.Clubs, PokerCardRank.Ace), "CA");
        _AnimationID.Add((PokerCardSuit.Hearts, PokerCardRank.Ace), "HA");
        _AnimationID.Add((PokerCardSuit.Spades, PokerCardRank.Ace), "SA");

        _AnimationID.Add((PokerCardSuit.Diamonds, PokerCardRank.Jack), "DJ");
        _AnimationID.Add((PokerCardSuit.Clubs, PokerCardRank.Jack), "CJ");
        _AnimationID.Add((PokerCardSuit.Hearts, PokerCardRank.Jack), "HJ");
        _AnimationID.Add((PokerCardSuit.Spades, PokerCardRank.Jack), "SJ");

        _AnimationID.Add((PokerCardSuit.Diamonds, PokerCardRank.Queen), "DQ");
        _AnimationID.Add((PokerCardSuit.Clubs, PokerCardRank.Queen), "CQ");
        _AnimationID.Add((PokerCardSuit.Hearts, PokerCardRank.Queen), "HQ");
        _AnimationID.Add((PokerCardSuit.Spades, PokerCardRank.Queen), "SQ");

        _AnimationID.Add((PokerCardSuit.Diamonds, PokerCardRank.King), "DK");
        _AnimationID.Add((PokerCardSuit.Clubs, PokerCardRank.King), "CK");
        _AnimationID.Add((PokerCardSuit.Hearts, PokerCardRank.King), "HK");
        _AnimationID.Add((PokerCardSuit.Spades, PokerCardRank.King), "SK");
    }

}
*/