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
        
        return _Ready;
    }

    private Dood _Debug = Dood.Instance;
    private HighLowView _View;
    private GameModeTag _GameModeTag = new GameModeTag();
    private StateProcess _StateProcess = new StateProcess();
    private HighLowState _State;
    private RMG_GameData _GameData = new RMG_GameData();
    public HighLow(HighLowView view)
    {
        _View = view;
        Tag = _GameModeTag.HighLow;

        RegisterStateEnter();
    }

    private void OnLowSelected()
    {
        _Debug.Log($"High Low State {_State}");
    }

    private void OnHighSelected()
    {
        _Debug.Log($"High Low State {_State}");
    }

    private void Init_Enter()
    {
        _View.SetActive(true);

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
        }
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
    }

    private void RegisterHigh()
    {
        _View.High.onClick.AddListener(OnHighSelected);
    }

    private void RegisterLow()
    {
        _View.Low.onClick.AddListener(OnLowSelected);
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
