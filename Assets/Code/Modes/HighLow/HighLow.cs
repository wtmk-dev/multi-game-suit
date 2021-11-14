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
        _View.SetOverlayText("Welcome to High / Low \n Please place your bets!");
        _StateData.IdelStateTimer.OnTimerComplete += Idle_HideOverlayText;
        _StateData.IdelStateTimer.Start(_StateData.IdelShowOverlayTime);
    }

    private void Idle_HideOverlayText()
    {
        _StateData.IdelStateTimer.OnTimerComplete -= Idle_HideOverlayText;
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
            _View.SetOverlayText("Welcome to High / Low \n Please place your bets!");
        }

        _StateData.IdelStateTimer.Start(_StateData.IdelShowOverlayTime);
    }

    private void Deal_Enter()
    {
        UnregisterOnSelectedBets();        
        DealCards();

        _View.OverlayText.SetActive(true);
        _View.SetOverlayText("Hey! Beautiful person,\n Do you think the face up card is HIGHER or LOWER than the face down card?");

        _StateData.DealStateTimer.OnTimerComplete += Deal_MessageHide;
        _StateData.DealStateTimer.Start(_StateData.DealShowOverlayTime);
        RegisterHigh();
        RegisterLow();
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
        if(_GameData.PlaceBet(bet.Bet))
        {
            StateChange(HighLowState.Deal);
        }
        else
        {
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
        _View.SetOverlayText("Hmmmm Low you think!?");

        StateChange(HighLowState.Pick);
    }

    private void OnHighSelected()
    {
        _StateData.Selection = HighLowSelection.High;
        UnregisterHigh();
        UnregisterLow();

        _View.OverlayText.SetActive(true);
        _View.SetOverlayText("Hmmmm High you think!?");

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
        if(_StateData.Left.State == PokerCardState.FaceDown)
        {
            SetCardState(_StateData.Left, PokerCardState.FaceUp);
        }
        else if (_StateData.Right.State == PokerCardState.FaceDown)
        {
            SetCardState(_StateData.Right, PokerCardState.FaceUp);
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
        ResolveWinner();
        
        _Debug.Log($"{_StateData.Selection}");
        _Debug.Log($"{_StateData.IsWinner}");
        _Debug.Log($"Base Card : {_StateData.Base.Model.Suit.ToString()} - {_StateData.Base.Model.Rank.ToString()}");
        _Debug.Log($"High Card : {GetHighCard().Model.Suit.ToString()} - {GetHighCard().Model.Rank.ToString()}");
        _Debug.Log($"Left Card : {_StateData.Left.Model.Suit.ToString()} - {_StateData.Left.Model.Rank.ToString()}");
        _Debug.Log($"Right Card : {_StateData.Right.Model.Suit.ToString()} - {_StateData.Right.Model.Rank.ToString()}");

        TransitionOutcome();
    }

    private void Celebrate_Enter()
    {
        if(_StateData.IsWinner)
        {
            _StateData.WinStreak++;
            int totalWin = _GameData.CurrentBet * _StateData.WinStreak;
            SetOverlayText($"Congratz!\n\n WIN STREAK - {_StateData.WinStreak} : TOTAL WIN - {totalWin}");
            _GameData.AddWinnings(totalWin);

            SetCardState(_StateData.Base, PokerCardState.FaceDown);
        }
        else
        {
            _StateData.WinStreak = 0;
            SetOverlayText($"You can't win them all.\n\nRun it back?");
            SetCardState(_StateData.Left, PokerCardState.FaceDown);
            SetCardState(_StateData.Right, PokerCardState.FaceDown);

            _StateData.Left = null;
            _StateData.Right = null;
            _StateData.Base = null;

            _Deck.InitDeckFromCards();
        }    

        _StateData.CelebrateStateTimer.OnTimerComplete += Celebrate_Complete;
        _StateData.CelebrateStateTimer.Start(_StateData.CelebrateShowOverlayTime);
    }

    private void Celebrate_Complete()
    {
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
        if (_StateData.Right == null)
        {
            _StateData.Right = (PokerCard)_StateData.Deck.Draw();
            SetCardView(_StateData.Right, _View.Right);
            SetCardState(_StateData.Right,PokerCardState.FaceDown);
        }

        if (_StateData.Left == null)
        {
            _StateData.Left = (PokerCard)_StateData.Deck.Draw();
            SetCardView(_StateData.Left, _View.Left);
            SetCardState(_StateData.Left, PokerCardState.FaceDown);
        }

        SetBaseCard();
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

    private void SetBaseCard()
    {
        if(_StateData.Base == null)
        {
            _StateData.Base = _StateData.Left;
            SetCardState(_StateData.Base, PokerCardState.FaceUp);
        }
        else
        {
            if(_StateData.Base == _StateData.Left)
            {
                _StateData.Base = _StateData.Right;
            }
            else if(_StateData.Base == _StateData.Right)
            {
                _StateData.Base = _StateData.Left;
            }
        }
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
        PokerCard card = GetHighCard();
        if (_StateData.Selection == HighLowSelection.High)
        {
            if (card.Model.Rank == _StateData.Base.Model.Rank && card.Model.Suit == _StateData.Base.Model.Suit)
            {
                _StateData.IsWinner = true;
            }
            else
            {
                _StateData.IsWinner = false;
            }
        }
        else if (_StateData.Selection == HighLowSelection.Low)
        {
            if (card.Model.Rank != _StateData.Base.Model.Rank && card.Model.Suit != _StateData.Base.Model.Suit)
            {
                _StateData.IsWinner = true;
            }
            else
            {
                _StateData.IsWinner = false;
            }
        }
    }
    /*
    private void SwapBaseCard()
    {
        if(_StateData.Left == _StateData.Base)
        {
            _StateData.Base = _StateData.Right;
        }else if(_StateData.Right == _StateData.Base)
        {
            _StateData.Base = _StateData.Left;
        }

        //SetCardState(_StateData.Base, PokerCardState.FaceDown);
    }
    */

    private void TransitionOutcome()
    {
        StateChange(HighLowState.Celebrate);
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
    public readonly float IdelShowOverlayTime = 3600f;

    public Timer DealStateTimer;
    public readonly float DealShowOverlayTime = 5600f;

    public Timer PickStateTimer;
    public readonly float PickShowOverlayTime = 3600f;

    public Timer RevealStateTimer;
    public readonly float RevealShowOverlayTime = 2700f;

    public Timer CelebrateStateTimer;
    public readonly float CelebrateShowOverlayTime = 3600f;

    public PokerDeck Deck { get { return _Deck; } }
    public PokerCard Left, Right;
    public PokerCard Base;

    public int WinStreak;

    public void Update()
    {
        IdelStateTimer.Tick();
        DealStateTimer.Tick();
        PickStateTimer.Tick();
        RevealStateTimer.Tick();
        CelebrateStateTimer.Tick();
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
