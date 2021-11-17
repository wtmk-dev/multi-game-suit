using System.Collections.Generic;

public class PokerStateData : Updatable
{
    public bool PlayerBust, DealerBust, PlayerWin, DealerWin;
    public BlackjackSelection Selection;

    public Timer IdelStateTimer;
    public readonly float IdelShowOverlayTime = 2600f;

    public Timer ResolveDealerTimer;
    public readonly float ResolveDealerTime = 800f;

    public Timer PickStateTimer;
    public readonly float PickShowOverlayTime = 1800f;

    public Timer BustTimer;
    public readonly float BustTime = 1600f;

    public Timer CelebrateStateTimer;
    public readonly float CelebrateShowOverlayTime = 2800f;

    public PokerDeck Deck { get { return _Deck; } }
    public int BetMulti { get { return _BetMulti; } }
    public List<PokerCard> DealersHand, PlayersHand, ExchangeCards;

    public readonly string IdelStateText = "{vertexp}Poker\nPlease place your bet!{/vertexp}";
    public readonly string TieText = "PUSH";

    public string GetTitleText(int size)
    {
        return $"{size} Card";
    }

    public string GetCelebrationText(int totalWin)
    {
        return $"<wiggle>WINNER!</wiggle>\n<incr>\nTOTAL WIN ${totalWin}<incr>";
    }

    public string GetDeal_EnterText(bool aceInHand, (int, int) score)
    {
        string txt = "";
        if (aceInHand)
        {
            txt = "{vertexp}You have " + score + "\nWill you HIT or STAY{vertexp}";
        }
        else
        {
            txt = "{vertexp}You have " + score.Item1 + "\nWill you HIT or STAY{vertexp}";
        }
        return txt;
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

    public void Clear()
    {
        IdelStateTimer.Stop();
        ResolveDealerTimer.Stop();
        PickStateTimer.Stop();
        BustTimer.Stop();
        CelebrateStateTimer.Stop();

        PlayerBust = false;
        DealerBust = false;
        PlayerWin = false;
        DealerWin = false;

        DealersHand.Clear();
        PlayersHand.Clear();

        _BetMulti = 1;

        ShuffleDeck();
    }

    private PokerDeck _Deck;
    private int _BetMulti;
    public PokerStateData(PokerDeck deck)
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
        ExchangeCards = new List<PokerCard>();
        _BetMulti = 1;
    }
}
