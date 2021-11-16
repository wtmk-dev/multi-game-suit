using System.Collections.Generic;

public class BlackjackStateData : Updatable
{
    public bool PlayerBust, DealerBust, PlayerWin, DealerWin;
    public BlackjackSelection Selection;

    public Timer IdelStateTimer;
    public readonly float IdelShowOverlayTime = 2600f;

    public Timer ResolveDealerTimer;
    public readonly float ResolveDealerTime = 300f;

    public Timer PickStateTimer;
    public readonly float PickShowOverlayTime = 1800f;

    public Timer BustTimer;
    public readonly float BustTime = 1600f;

    public Timer CelebrateStateTimer;
    public readonly float CelebrateShowOverlayTime = 2600f;

    public PokerDeck Deck { get { return _Deck; } }
    public int BetMulti { get { return _BetMulti; } }
    public List<PokerCard> DealersHand, PlayersHand;

    public readonly int StaringHandSize = 2;
    public readonly string IdelStateText = "{vertexp}Blackjack\nPlease place your bet!{/vertexp}";
    public readonly string OnSlectedText = "{horiexp}1.2.3.{/horiexp}";
    public readonly string BustText = "{horiexp}<bounce>(X.X) BUST (X.X)</bounce>{horiexp}";
    public readonly string LoseText = "<bounce>Can't win em' all\nBut you can catch em' all";

    public string GetCelebrationText(int totalWin)
    {
        return $"<wiggle>WINNER!</wiggle>\n<incr>\nTOTAL WIN ${totalWin}<incr>";
    }

    public string GetDeal_EnterText((int,int)score)
    {
        return "{vertexp}You have " + score +"\nWill you HIT or STAY{vertexp}";
    }

    public void Update()
    {
        IdelStateTimer.Tick();
        ResolveDealerTimer.Tick();
        PickStateTimer.Tick();
        BustTimer.Tick();
        CelebrateStateTimer.Tick();
    }

    public void ShuffleDeck()
    {
        _Deck.InitDeckFromCards();
    }

    public void SetBetMulti(int value)
    {
        _BetMulti = value;
    }

    private PokerDeck _Deck;
    private int _BetMulti;
    public BlackjackStateData(PokerDeck deck)
    {
        _Deck = deck;
        _Deck.InitDeckFromCards();
        IdelStateTimer = new Timer(0f);
        ResolveDealerTimer = new Timer(0f);
        PickStateTimer = new Timer(0f);
        BustTimer = new Timer(0f);
        CelebrateStateTimer = new Timer(0f);
        DealersHand = new List<PokerCard>();
        PlayersHand = new List<PokerCard>();
        _BetMulti = 1;
    }
}
