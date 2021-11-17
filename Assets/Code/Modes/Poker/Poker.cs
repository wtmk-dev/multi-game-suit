using System.Collections;
using System.Collections.Generic;

public class Poker : State
{
    public override string Tag { get; protected set; }
    public override IStateView View { get { return _View; } }

    public override void OnEnter() 
    {
        StateChange(PokerState.Init);
    }

    public override void OnExit() 
    { 
    
    }

    public override bool OnUpdate() 
    { 
        return _Ready; 
    }

    private GameModeTag _GameModeTag = new GameModeTag();
    private EventManager _EventManager = EventManager.Instance;
    private RMG_GameData _GameData = RMG_GameData.Instance;
    private Dood _Debug = Dood.Instance;
    private PokerView _View;
    protected StateProcess _StateProcess = new StateProcess();
    private PokerStateData _StateData;
    private PokerDeck _Deck;
    private PokerState _State;
    private Dictionary<string, PokerValue> _RuleSets = new Dictionary<string, PokerValue>();
    private PokerValue _CurrentRules;

    public Poker(PokerView view, PokerDeck deck, string tag)
    {
        _View = view;
        Tag = tag;
        _Deck = deck;
        RegisterStateEnter();
        
        _RuleSets.Add(_GameModeTag.Poker5, new PokerFiveCardValue());
        _RuleSets.Add(_GameModeTag.Poker7, new PokerFiveCardValue());

        _CurrentRules = _RuleSets[tag];
    }

    private void Init_Enter()
    {
        if(_StateData == null)
        {
            _StateData = new PokerStateData(_Deck);
        }

        _View.gameObject.SetActive(true);
        _View.SetTitleText(_StateData.GetTitleText(_CurrentRules.HandSize));

        InitBets();
        StateChange(PokerState.Idle);
    }

    private void Idle_Enter()
    {
        _EventManager.FireEvent(GameModeEvent.Idel.ToString());

        ClearHand(_StateData.PlayersHand);
        ClearHand(_StateData.DealersHand);

        _StateData.ShuffleDeck();

        RegisterOnSelectedBets();

        _View.OverlayText.SetActive(true);
        _View.Exchange.gameObject.SetActive(true);
        _View.Stay.gameObject.SetActive(true);

        _View.SetOverlayText(_StateData.IdelStateText);
    }

    private void Deal_Enter()
    {
        DealCards();
    }

    private void Pick_Enter()
    {
        RegisterExchange();
        RegisterStay();
        RevealHand(_StateData.PlayersHand);
    }

    private void OnStaySelected()
    {

    }

    private void OnExchange() 
    {
        for (int i = 0; i < _StateData.ExchangeCards.Count; i++)
        {
            
        }
    }

    private void RegisterStay()
    {
        _View.Stay.onClick.AddListener(OnStaySelected);
    }

    private void RegisterExchange()
    {
        _View.Exchange.onClick.AddListener(OnExchange);
    }

    private void RevealHand(List<PokerCard> hand)
    {
        for (int i = 0; i < hand.Count; i++)
        {
            hand[i].SetState(PokerCardState.FaceUp);
        }
    }

    private void RegisterHandForPick(List<PokerCard> hand)
    {
        for (int i = 0; i < hand.Count; i++)
        {
            //
        }
    }

    private void ClearHand(List<PokerCard> hand)
    {
        for (int i = 0; i < hand.Count; i++)
        {
            hand[i].View.Kill();
        }

        hand.Clear();
    }

    private void RegisterStateEnter()
    {
        _StateProcess.RegisterEnter(PokerState.Init.ToString(), Init_Enter);
        _StateProcess.RegisterEnter(PokerState.Idle.ToString(), Idle_Enter);
        _StateProcess.RegisterEnter(PokerState.Deal.ToString(), Deal_Enter);
        _StateProcess.RegisterEnter(PokerState.Pick.ToString(), Pick_Enter);
    }

    private void StateChange(PokerState state, float waitTime = 0f)
    {
        _State = state;

        try
        {
            _StateProcess.StateEnter[state.ToString()]();
        }
        catch (KeyNotFoundException e)
        {
            _Debug.Error(e.ToString());
            _Debug.Log($"High Low State {state} does not have a state enter process registered");
        }
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

    private void RegisterOnSelectedBets()
    {
        for (int i = 0; i < _View.Bets.Count; i++)
        {
            _View.Bets[i].OnBetSelected += BetSelected;
        }
    }

    private void BetSelected(BetEventArgs bet)
    {
        UnregisterOnSelectedBets();
        _EventManager.FireEvent(MenuEvent.GameSelectMenuHide.ToString());

        _StateData.SetBetMulti(bet.BetMulti);

        if (_GameData.PlaceBet(bet.Bet))
        {
            StateChange(PokerState.Deal);
        }
        else
        {
            RegisterOnSelectedBets();
            StateChange(PokerState.Idle);
        }
    }

    private void UnregisterOnSelectedBets()
    {
        for (int i = 0; i < _View.Bets.Count; i++)
        {
            _View.Bets[i].OnBetSelected -= BetSelected;
        }
    }

    private void DealCards()
    {
        _Debug.Log("DealCards");

        GetStartingHand(_StateData.DealersHand);
        GetStartingHand(_StateData.PlayersHand);

        AttachHandToGroupView(_StateData.DealersHand, _View.DealersHandGroup.transform);
        AttachHandToGroupView(_StateData.PlayersHand, _View.PlayersHandGroup.transform);

        StateChange(PokerState.Pick);
    }

    private void AttachHandToGroupView(List<PokerCard> hand, UnityEngine.Transform transform)
    {
        for (int i = 0; i < hand.Count; i++)
        {
            AttachCardToGroupView(hand[i], transform);
        }
    }

    private void AttachCardToGroupView(PokerCard card, UnityEngine.Transform transform)
    {
        card.View.gameObject.SetActive(true);
        card.View.transform.SetParent(transform);
    }

    private void GetStartingHand(List<PokerCard> hand)
    {
        while (hand.Count < _CurrentRules.HandSize)
        {
            AddCardToHand(hand);
        }
    }

    private void AddCardToHand(List<PokerCard> hand)
    {
        hand.Add(GetViewBoundCard());
    }

    private PokerCard GetViewBoundCard()
    {
        PokerCardView view = _View.GetCardView();
        PokerCard card = (PokerCard)_StateData.Deck.Draw();
        card.SetView(view);
        card.SetState(PokerCardState.FaceDown);
        return card;
    }
}

public enum PokerState
{
    Init,
    Bet,
    Deal,
    Pick,
    Idle
}
