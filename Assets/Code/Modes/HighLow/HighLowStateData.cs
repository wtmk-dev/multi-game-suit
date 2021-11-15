
public class HighLowStateData : Updatable
{
    public bool IsWinner;
    public HighLowSelection Selection;

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
    public PokerCard Left, Right, Base = null;

    public int WinStreak;

    public readonly string IdelStateText = "{vertexp}Hello Welcome\nThis is High Low \n Please place your bet!{/vertexp}";
    public readonly string Deal_EnterText = "Is the FACE UP card \n HIGHER or LOWER \n than the FACE DOWN card?";
    public readonly string OnSlectedText = "{horiexp}1.2.3.{/horiexp}";
    public readonly string Celebrate_EnterText = "<bounce>QQ--\nBetter luck next time.</bounce>";

    public string GetCelebrationText(int winStreak, int totalWin)
    {
        return $"<wiggle>Beautiful!</wiggle>\n<incr>WIN STREAK {winStreak}<incr>\nTOTAL WIN ${totalWin}";
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
    public HighLowStateData(PokerDeck deck)
    {
        _Deck = deck;
        _Deck.InitDeckFromCards();
        IdelStateTimer = new Timer(0f);
        DealStateTimer = new Timer(0f);
        PickStateTimer = new Timer(0f);
        RevealStateTimer = new Timer(0f);
        CelebrateStateTimer = new Timer(0f);
        WinStreak = 0;
    }
}
