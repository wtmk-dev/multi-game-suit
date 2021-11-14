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

    private void OnLowSelected()
    {
        UnregisterHigh();
        UnregisterLow();

        _View.OverlayText.SetActive(true);
        _View.SetOverlayText("Hmmmm Low you think!?");

        StateChange(HighLowState.Pick);
    }

    private void OnHighSelected()
    {
        UnregisterHigh();
        UnregisterLow();

        _View.OverlayText.SetActive(true);
        _View.SetOverlayText("Hmmmm High you think!?");

        StateChange(HighLowState.Pick);
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
    }

    private void Deal_Enter()
    {
        UnregisterOnSelectedBets();
        SetLeftCard();
        SetRigtCard();

        _StateData.DealStateTimer.OnTimerComplete += Deal_MessageHide;

        _View.OverlayText.SetActive(true);
        _View.SetOverlayText("Click High if you think the face down card is Higer then the face up card!Or Low if you thing its lower! Good Luck");

        _StateData.DealStateTimer.Start(_StateData.DealShowOverlayTime);

        RegisterHigh();
        RegisterLow();
        //wait for player to take action
    }

    private void SetLeftCard()
    {
        if(_StateData.Left == null)
        {

        }
    }

    private void SetRigtCard() 
    {
        if (_StateData.Left == null)
        {

        }
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
        //if(_View.Left)
    }

    private void StateChange(HighLowState state, float waitTime = 0f)
    {
        try
        {
            _StateProcess.StateEnter[state.ToString()]();
        }catch(KeyNotFoundException e)
        {
            _Debug.Error(e.ToString());
            _Debug.Log($"High Low State {state} does not have a state enter process registered");
        }
        _State = state;
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
}

public enum HighLowState
{
    Init,
    Bet,
    Deal,
    Pick,
    Reveal,
    Outcome,
    Idle
}

public class HighLowStateData : Updatable
{
    public Timer DealStateTimer;
    public readonly float DealShowOverlayTime = 5600f;

    public Timer PickStateTimer;
    public readonly float PickShowOverlayTime = 3600f;

    public PokerDeck Deck;
    public PokerCard Left, Right;

    public void Update()
    {
        DealStateTimer.Tick();
        PickStateTimer.Tick();
    }

    private PokerDeck _Deck;
    public HighLowStateData(PokerDeck deck)
    {
        _Deck = deck;
        DealStateTimer = new Timer(0f);
        PickStateTimer = new Timer(0f);
    }
}
