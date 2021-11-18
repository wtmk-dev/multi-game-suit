using System.Collections.Generic;

public class PokerStateData : Updatable
{
    public PokerOutcome Outcome;

    public Timer IdelStateTimer;
    public readonly float IdelShowOverlayTime = 2600f;

    public Timer OutcomeStateTimer;
    public readonly float OutcomeTime = 1600f;

    public Timer CelebrateStateTimer;
    public readonly float CelebrateShowOverlayTime = 2800f;

    public PokerDeck Deck { get { return _Deck; } }
    public int BetMulti { get { return _BetMulti; } }
    public List<PokerCard> DealersHand, PlayersHand, ExchangeCards;

    public readonly string IdelStateText = "{vertexp}Poker\nPlease place your bet!{/vertexp}";
    public readonly string OutcomeText = "SHOW";
    public readonly string PickStateText = "{vertexp}Click any cards you wish to exchange.\n Then click exchange.\nOr Click stay.";
    public readonly string TieText = "PUSH";
    public readonly string LoseText = "<bounce>I. Just got lucky\nRun it back?<bounce>";

    public string GetTitleText(int size)
    {
        return $"{size} Card";
    }

    public string GetCelebrationText(int totalWin)
    {
        return $"<rainb>Chicken Dinner!</rainb>\n<incr>\nTOTAL WIN ${totalWin}<incr>";
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
        OutcomeStateTimer.Tick();
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
        CelebrateStateTimer.Stop();

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
        CelebrateStateTimer = new Timer(0f);
        OutcomeStateTimer = new Timer(0f);
        DealersHand = new List<PokerCard>();
        PlayersHand = new List<PokerCard>();
        ExchangeCards = new List<PokerCard>();
        _BetMulti = 1;
    }
}

public enum PokerOutcome
{
    Win,
    Push,
    Lose
}
