using System.Collections.Generic;

public class BlackjackStateData : Updatable
{
    public bool IsWinner;
    public BlackjackSelection Selection;

    public Timer IdelStateTimer;
    public readonly float IdelShowOverlayTime = 2600f;

    public Timer DealStateTimer;
    public readonly float DealShowOverlayTime = 300f;

    public Timer PickStateTimer;
    public readonly float PickShowOverlayTime = 1800f;

    public Timer RevealStateTimer;
    public readonly float RevealShowOverlayTime = 2000f;

    public Timer CelebrateStateTimer;
    public readonly float CelebrateShowOverlayTime = 2600f;

    public PokerDeck Deck { get { return _Deck; } }
    public List<PokerCard> DealersHand, PlayersHand;

    public readonly int StaringHandSize = 2;

    public readonly string IdelStateText = "{vertexp}Blackjack\nPlease place your bet!{/vertexp}";
    public readonly string OnSlectedText = "{horiexp}1.2.3.{/horiexp}";
    public readonly string Celebrate_EnterText = "<bounce>QQ--\nBetter luck next time.</bounce>";

    public string GetCelebrationText(int winStreak, int totalWin)
    {
        return $"<wiggle>Beautiful!</wiggle>\n<incr>WIN STREAK {winStreak}<incr>\nTOTAL WIN ${totalWin}";
    }

    public string GetDeal_EnterText((int,int)score)
    {
        return "{vertexp}You have " + score +"\nWill you HIT or STAY{vertexp}";
    }

    public void Update()
    {
        IdelStateTimer.Tick();
        DealStateTimer.Tick();
        PickStateTimer.Tick();
        RevealStateTimer.Tick();
        CelebrateStateTimer.Tick();
    }

    public void ShuffleDeck()
    {
        _Deck.InitDeckFromCards();
    }

    private PokerDeck _Deck;
    public BlackjackStateData(PokerDeck deck)
    {
        _Deck = deck;
        _Deck.InitDeckFromCards();
        IdelStateTimer = new Timer(0f);
        DealStateTimer = new Timer(0f);
        PickStateTimer = new Timer(0f);
        RevealStateTimer = new Timer(0f);
        CelebrateStateTimer = new Timer(0f);
        DealersHand = new List<PokerCard>();
        PlayersHand = new List<PokerCard>();
    }
}
