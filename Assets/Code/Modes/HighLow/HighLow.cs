using System.Collections;
using System.Collections.Generic;

public class HighLow : GameMode 
{
    public override string Tag { get; protected set; }
    public override IStateView View { get { return _View; } }

    public override void OnEnter()
    {
        RegisterGameScreenEvents();
        StateChange(HighLowState.Init);
    }

    public override bool OnUpdate()
    {
        _StateData.Update();
        return _Ready;
    }

    public override void OnExit()
    {
        UnregisterHigh();
        UnregisterLow();
        UnregisterOnSelectedBets();
        UnregisterGameScreenEvents();
        _View.SetActive(false);


        _Ready = false;
    }

    private EventManager _EventManager = EventManager.Instance;
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
        if(_StateData == null)
        {
            _StateData = new HighLowStateData(_Deck);
        }
        
        _View.SetActive(true);
        _View.OverlayText.SetActive(false);

        InitBets();
        StateChange(HighLowState.Idle);
    }

    private void InitBets()
    {
        int baseBet = _GameData.BaseBet;

        for (int i = 0; i < _View.Bets.Count; i++)
        {
            int multi = (i + 1);
            baseBet *= multi;

            _View.Bets[i].gameObject.SetActive(true);
            _View.Bets[i].Init(new BetEventArgs(baseBet, multi));
        }
    }

    private void Idle_Enter()
    {
        RegisterOnSelectedBets();

        _View.OverlayText.SetActive(true);
        _View.SetOverlayText(_StateData.IdelStateText);
    }

    private void Deal_Enter()
    {
        DealCards();

        _View.OverlayText.SetActive(true);
        _View.SetOverlayText(_StateData.Deal_EnterText);

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
        _EventManager.FireEvent(MenuEvent.GameSelectMenuHide.ToString());

        if (_GameData.PlaceBet(bet.Bet))
        {
            StateChange(HighLowState.Deal);
        }
        else
        {
            RegisterOnSelectedBets();
            StateChange(HighLowState.Idle);
        }
    }

    private void OnLowSelected()
    {
        _StateData.Selection = HighLowSelection.Low;
        UnregisterHigh();
        UnregisterLow();
        

        _View.OverlayText.SetActive(true);
       
        _StateData.PickStateTimer.OnTimerComplete += Pick_Complete;
        _StateData.PickStateTimer.Start(_StateData.PickShowOverlayTime);
        _View.SetOverlayText(_StateData.OnSlectedText);
    }

    private void OnHighSelected()
    {
        _StateData.Selection = HighLowSelection.High;
        UnregisterHigh();
        UnregisterLow();
        _EventManager.FireEvent(MenuEvent.GameSelectMenuHide.ToString());

        _View.OverlayText.SetActive(true);
       
        _StateData.PickStateTimer.OnTimerComplete += Pick_Complete;
        _StateData.PickStateTimer.Start(_StateData.PickShowOverlayTime);
        _View.SetOverlayText(_StateData.OnSlectedText);
    }

    private void Pick_Complete()
    {
        _StateData.PickStateTimer.OnTimerComplete -= Pick_Complete;

        _View.SetOverlayText("");
        _View.OverlayText.SetActive(false);

        StateChange(HighLowState.Pick);
    }

    private void Pick_Enter()
    {
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
            SetOverlayText(_StateData.GetCelebrationText(_StateData.WinStreak, totalWin));
            _GameData.AddWinnings(totalWin);

            SetCardState(_StateData.Base, PokerCardState.FaceDown);
        }
        else
        {
            _StateData.WinStreak = 0;
            SetOverlayText(_StateData.Celebrate_EnterText);
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
        //HideOverlayText();
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

    protected override void OnGameSelect(string name, object data)
    {
        if (_State == HighLowState.Idle)
        {
            _EventManager.FireEvent(MenuEvent.GameSelectMenuShow.ToString());
        }
    }

    protected override void OnGameSelectExit(string name, object data)
    {
       
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
