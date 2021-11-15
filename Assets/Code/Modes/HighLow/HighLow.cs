using System.Collections;
using System.Collections.Generic;

public class HighLow : State 
{
    public override string Tag { get; protected set; }
    public override IStateView View { get { return _View; } }

    public override void OnEnter()
    {
        StateChange(HighLowState.Init);
    }

    public override bool OnUpdate()
    {
        _StateData.Update();
        return _Ready;
    }

    private Dood _Debug = Dood.Instance;
    private HighLowView _View;

    private GameModeTag _GameModeTag = new GameModeTag();
    private StateProcess _StateProcess = new StateProcess();

    private HighLowState _State;
    private RMG_GameData _GameData = new RMG_GameData();

    private HighLowStateData _StateData;
    private PokerDeck _Deck;
   
    public HighLow(HighLowView view, PokerDeck deck)
    {
        _Deck = deck;
        _View = view;
        Tag = _GameModeTag.HighLow;

        RegisterStateEnter();
    }
    
    private void Init_Enter()
    {
        _StateData = new HighLowStateData(_Deck);
        _View.SetActive(true);
        _View.OverlayText.SetActive(false);

        InitBets();
        StateChange(HighLowState.Idle);
    }

    private void InitBets()
    {
        int baseBet = 5;
        for (int i = 0; i < _View.Bets.Count; i++)
        {
            if(i != 0)
            {
                baseBet *= i;
            }

            _View.Bets[i].Init(new BetEventArgs(baseBet));
        }
    }

    private void Idle_Enter()
    {
        RegisterOnSelectedBets();

        _View.OverlayText.SetActive(true);
        _View.SetOverlayText("<rainb>Hello Welcome</rainb> \n this is High Low \n Please place your bet!");
        _StateData.IdelStateTimer.OnTimerComplete += Idle_HideOverlayText;
        _StateData.IdelStateTimer.Start(_StateData.IdelShowOverlayTime);
    }

    private void Idle_HideOverlayText()
    {
        _StateData.IdelStateTimer.OnTimerComplete -= Idle_HideOverlayText;

        if (_State == HighLowState.Deal)
        {
            return;
        }

        _StateData.IdelStateTimer.OnTimerComplete += Idle_ShowOverlayText;
        _View.OverlayText.SetActive(false);
        _View.SetOverlayText("");

        _StateData.IdelStateTimer.Start(_StateData.IdelShowOverlayTime);
    }

    private void Idle_ShowOverlayText()
    {
        if (_State == HighLowState.Idle)
        {
            _StateData.IdelStateTimer.OnTimerComplete += Idle_HideOverlayText;
            _StateData.IdelStateTimer.OnTimerComplete -= Idle_ShowOverlayText;
            _View.OverlayText.SetActive(true);
            _View.SetOverlayText("<rainb>Hello Welcome</rainb> \n this is High Low \n Please place your bet!");
            _StateData.IdelStateTimer.Start(_StateData.IdelShowOverlayTime);
        }
        else
        {
            _StateData.IdelStateTimer.OnTimerComplete -= Idle_HideOverlayText;
        }
    }

    private void Deal_Enter()
    {
        DealCards();

        _View.OverlayText.SetActive(true);
        _View.SetOverlayText("Is the FACE UP card \n HIGHER or LOWER \n than the FACE DOWN card?");

        RegisterHigh();
        RegisterLow();

        //_StateData.DealStateTimer.OnTimerComplete += Deal_MessageHide;
        //_StateData.DealStateTimer.Start(_StateData.DealShowOverlayTime);
        //wait for player to take action
    }

    private void Deal_MessageHide()
    {
        _StateData.DealStateTimer.OnTimerComplete -= Deal_MessageHide;
        _View.SetOverlayText("");
        _View.OverlayText.SetActive(false);
    }

    private void BetSelected(BetEventArgs bet)
    {
        UnregisterOnSelectedBets();

        if(_GameData.PlaceBet(bet.Bet))
        {
            StateChange(HighLowState.Deal);
        }
        else
        {
            RegisterOnSelectedBets();
            _Debug.Log("Need more moneyssss");
            StateChange(HighLowState.Idle);
        }
    }
    private void OnLowSelected()
    {
        _StateData.Selection = HighLowSelection.Low;
        UnregisterHigh();
        UnregisterLow();

        _View.OverlayText.SetActive(true);
        _View.SetOverlayText("<shake>...LOWER!?</shake>");

        StateChange(HighLowState.Pick);
    }

    private void OnHighSelected()
    {
        _StateData.Selection = HighLowSelection.High;
        UnregisterHigh();
        UnregisterLow();

        _View.OverlayText.SetActive(true);
        _View.SetOverlayText("<shake>..HIGHER!?</shake>");

        StateChange(HighLowState.Pick);
    }

    private void Pick_Enter()
    {
        _StateData.PickStateTimer.OnTimerComplete += Pick_Complete;
        _StateData.PickStateTimer.Start(_StateData.PickShowOverlayTime);
    }

    private void Pick_Complete()
    {
        _StateData.PickStateTimer.OnTimerComplete -= Pick_Complete;

        _View.SetOverlayText("");
        _View.OverlayText.SetActive(false);

        StateChange(HighLowState.Reveal);
    }

    private void Reveal_Enter()
    {
        if (_StateData.Base == _StateData.Left)
        {
            _StateData.Right.SetState(PokerCardState.FaceUp);
        }
        else if (_StateData.Base == _StateData.Right)
        {
            _StateData.Left.SetState(PokerCardState.FaceUp);
        }

        _StateData.RevealStateTimer.OnTimerComplete += Reveal_Complete;
        _StateData.RevealStateTimer.Start(_StateData.RevealShowOverlayTime);
    }

    private void Reveal_Complete()
    {
        _StateData.RevealStateTimer.OnTimerComplete -= Reveal_Complete;
        StateChange(HighLowState.Outcome);
    }

    private void Outcome_Enter()
    {
        _Debug.Log($"{_StateData.Selection}");

        ResolveWinner();
        StateChange(HighLowState.Celebrate);

        _Debug.Log($"{_StateData.IsWinner}");
    }

    private void Celebrate_Enter()
    {
        if(_StateData.IsWinner)
        {
            _StateData.WinStreak++;
            int totalWin = _GameData.CurrentBet * _StateData.WinStreak;
            SetOverlayText($"<wiggle>Beautiful!</wiggle>\n<incr>WIN STREAK -{_StateData.WinStreak}<incr>\nTOTAL WIN ${totalWin}");
            _GameData.AddWinnings(totalWin);

            SetCardState(_StateData.Base, PokerCardState.FaceDown);
        }
        else
        {
            _StateData.WinStreak = 0;
            SetOverlayText($"<bounce>QQ--Better Luck next time.</bounce>");
            SetCardState(_StateData.Base, PokerCardState.FaceDown);
            
            _StateData.Base = null;
            _StateData.ShuffleDeck();
        }    

        _StateData.CelebrateStateTimer.OnTimerComplete += Celebrate_Complete;
        _StateData.CelebrateStateTimer.Start(_StateData.CelebrateShowOverlayTime);
    }

    private void Celebrate_Complete()
    {
        _StateData.CelebrateStateTimer.OnTimerComplete -= Celebrate_Complete;
        HideOverlayText();
        StateChange(HighLowState.Idle);
    }

    private void StateChange(HighLowState state, float waitTime = 0f)
    {
        _State = state;

        try
        {
            _StateProcess.StateEnter[state.ToString()]();
        }catch(KeyNotFoundException e)
        {
            _Debug.Error(e.ToString());
            _Debug.Log($"High Low State {state} does not have a state enter process registered");
        }
    }

    private void RegisterOnSelectedBets()
    {
        for (int i = 0; i < _View.Bets.Count; i++)
        {
            _View.Bets[i].OnBetSelected += BetSelected;
        }
    }

    private void UnregisterOnSelectedBets()
    {
        for (int i = 0; i < _View.Bets.Count; i++)
        {
            _View.Bets[i].OnBetSelected -= BetSelected;
        }
    }

    private void RegisterStateEnter()
    {
        _StateProcess.RegisterEnter(HighLowState.Init.ToString(), Init_Enter);
        _StateProcess.RegisterEnter(HighLowState.Idle.ToString(), Idle_Enter);
        _StateProcess.RegisterEnter(HighLowState.Deal.ToString(), Deal_Enter);
        _StateProcess.RegisterEnter(HighLowState.Pick.ToString(), Pick_Enter);
        _StateProcess.RegisterEnter(HighLowState.Reveal.ToString(), Reveal_Enter);
        _StateProcess.RegisterEnter(HighLowState.Outcome.ToString(), Outcome_Enter);
        _StateProcess.RegisterEnter(HighLowState.Celebrate.ToString(), Celebrate_Enter);
    }

    private void RegisterHigh()
    {
        _View.High.onClick.AddListener(OnHighSelected);
    }

    private void RegisterLow()
    {
        _View.Low.onClick.AddListener(OnLowSelected);
    }

    private void UnregisterHigh()
    {
        _View.High.onClick.RemoveListener(OnHighSelected);
    }

    private void UnregisterLow()
    {
        _View.Low.onClick.RemoveListener(OnLowSelected);
    }

    private void DealCards()
    {
        if (_StateData.Base == null)
        {
            _StateData.Right = (PokerCard)_StateData.Deck.Draw();
            SetCardView(_StateData.Right, _View.Right);
            SetCardState(_StateData.Right, PokerCardState.FaceDown);

            _StateData.Left = (PokerCard)_StateData.Deck.Draw();
            SetCardView(_StateData.Left, _View.Left);
            SetCardState(_StateData.Left, PokerCardState.FaceDown);

            _StateData.Base = _StateData.Left;
        }
        else if (_StateData.Base == _StateData.Left)
        {
            _StateData.Left = (PokerCard)_StateData.Deck.Draw();
            SetCardView(_StateData.Left, _View.Left);
            SetCardState(_StateData.Left, PokerCardState.FaceDown);
            _StateData.Base = _StateData.Right;
        }
        else if (_StateData.Base == _StateData.Right)
        {
            _StateData.Right = (PokerCard)_StateData.Deck.Draw();
            SetCardView(_StateData.Right, _View.Right);
            SetCardState(_StateData.Right, PokerCardState.FaceDown);
            _StateData.Base = _StateData.Left;
        }

        SetCardState(_StateData.Left, PokerCardState.FaceDown);
        SetCardState(_StateData.Right, PokerCardState.FaceDown);

        SetCardState(_StateData.Base, PokerCardState.FaceUp);
    }

    private void SetCardView(PokerCard card, PokerCardView view)
    {
        card.SetView(view);
    }

    private void SetCardState(PokerCard card, PokerCardState state)
    {
        card.SetState(state);
    }

   
    private PokerCard GetHighCard()
    {
        if (_StateData.Left.Model.Rank == _StateData.Right.Model.Rank)
        {
            if (_StateData.Left.Model.Suit > _StateData.Right.Model.Suit)
            {
                return _StateData.Left;
            }
            else if (_StateData.Left.Model.Suit < _StateData.Right.Model.Suit)
            {
                return _StateData.Right;
            }

        }
        else if (_StateData.Left.Model.Rank > _StateData.Right.Model.Rank)
        {
            return _StateData.Left;
        }

        return _StateData.Right;
    }

    private void ResolveWinner()
    {
        PokerCard highCard = GetHighCard();
        _StateData.IsWinner = false;

        if (_StateData.Selection == HighLowSelection.High)
        {
            if(highCard == _StateData.Base)
            {
                _StateData.IsWinner = true;
            }
        }
        else if (_StateData.Selection == HighLowSelection.Low)
        {
            if(highCard != _StateData.Base)
            {
                _StateData.IsWinner = true;
            }
        }
    }

    private void SetOverlayText(string text)
    {
        _View.OverlayText.SetActive(true);
        _View.SetOverlayText(text);
    }

    private void HideOverlayText()
    {
        _View.SetOverlayText("");
        _View.OverlayText.SetActive(false);
    }
}

public enum HighLowState
{
    Init,
    Bet,
    Deal,
    Pick,
    Reveal,
    Outcome,
    Celebrate,
    Idle
}

public enum HighLowSelection
{
    High, Low
}

public class HighLowStateData : Updatable
{
    public bool IsWinner;
    public HighLowSelection Selection;

    public Timer IdelStateTimer;
    public readonly float IdelShowOverlayTime = 7600f;

    public Timer DealStateTimer;
    public readonly float DealShowOverlayTime = 9300f;

    public Timer PickStateTimer;
    public readonly float PickShowOverlayTime = 6700f;

    public Timer RevealStateTimer;
    public readonly float RevealShowOverlayTime = 2400f;

    public Timer CelebrateStateTimer;
    public readonly float CelebrateShowOverlayTime = 7300f;

    public PokerDeck Deck { get { return _Deck; } }
    public PokerCard Left, Right, Base = null;

    public int WinStreak;

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
