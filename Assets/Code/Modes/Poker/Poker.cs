using System.Collections;
using System.Collections.Generic;
using HoldemHand;

public class Poker : GameMode
{
    public override string Tag { get; protected set; }
    public override IStateView View { get { return _View; } }

    public override void OnEnter()
    {
        RegisterGameScreenEvents();
        StateChange(PokerState.Init);
    }

    public override void OnExit()
    {
        UnregisterStay();
        UnregisterExchange();
        UnregisterOnSelectedBets();
        UnregisterGameScreenEvents();

        ClearHand(_StateData.DealersHand);
        ClearHand(_StateData.PlayersHand);

        _View.SetActive(false);
        _StateData.Clear();

        _View.Exchange.gameObject.SetActive(false);
        _View.Stay.gameObject.SetActive(false);

        _Ready = false;
    }

    public override bool OnUpdate()
    {
        _StateData.Update();
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
    private int _CurrentRules;
    private AudioEvent _AudioEvent = new AudioEvent();

    public Poker(PokerView view, PokerDeck deck, string tag, int rules)
    {
        _View = view;
        Tag = tag;
        _Deck = deck;
        RegisterStateEnter();

        _CurrentRules = rules;
    }

    private void Init_Enter()
    {
        if (_StateData == null)
        {
            _StateData = new PokerStateData(_Deck);
        }

        _View.gameObject.SetActive(true);
        _View.SetTitleText(_StateData.GetTitleText(_CurrentRules));

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
        SetOverlayText(_StateData.PickStateText);
        RevealHand(_StateData.PlayersHand);


        if(_GameData.IsAutoPlay)
        {
            OnStaySelected();
        }
        else
        {
            RegisterHand(_StateData.PlayersHand);
            RegisterExchange();
            RegisterStay();
        }
    }

    private void Reveal_Enter()
    {
        RevealHand(_StateData.DealersHand);
        StateChange(PokerState.Outcome);
    }
    
    private void Outcome_Enter()
    {
        SetOverlayText(_StateData.OutcomeText);

        //_Debug.Log("Player's Hand : " + GetHoldemHand(_StateData.PlayersHand));
        //_Debug.Log("Dealer's Hand : " + GetHoldemHand(_StateData.DealersHand));

        ulong playersHand = Hand.ParseHand(GetHoldemHand(_StateData.PlayersHand));
        ulong dealersHand = Hand.ParseHand(GetHoldemHand(_StateData.DealersHand));

        uint pval = Hand.Evaluate(playersHand, _CurrentRules);
        uint dval = Hand.Evaluate(dealersHand, _CurrentRules);


        _StateData.PlayersHandKey = Hand.DescriptionFromMask(playersHand);
        _StateData.DealersHanKey = Hand.DescriptionFromMask(dealersHand);
        
        if (pval == dval)
        {
            _StateData.Outcome = PokerOutcome.Push;
        }
        else if(pval < dval)
        {
            _StateData.Outcome = PokerOutcome.Lose;
        }
        else{
            _StateData.Outcome = PokerOutcome.Win;
        }

        _StateData.OutcomeStateTimer.OnTimerComplete += Outcome_Complete;
        _StateData.OutcomeStateTimer.Start(_StateData.OutcomeTime);
    }

    private void Outcome_Complete()
    {
        _StateData.OutcomeStateTimer.OnTimerComplete -= Outcome_Complete;
        StateChange(PokerState.Celebrate);
    }

    private void Celebrate_Enter()
    {
        switch(_StateData.Outcome)
        {
            case PokerOutcome.Lose:
                _EventManager.FireEvent(GameModeEvent.Lose.ToString());
                
                SetOverlayText(_StateData.GetLoseText(_StateData.DealersHanKey));
                break;
            case PokerOutcome.Win:
                ResolveWin();
                break;
            case PokerOutcome.Push:
                _GameData.Push();
                SetOverlayText(_StateData.TieText);
                break;
        }

        _StateData.CelebrateStateTimer.OnTimerComplete += Celebrate_Complete;
        _StateData.CelebrateStateTimer.Start(_StateData.CelebrateShowOverlayTime);
    }

    private void Celebrate_Complete()
    {
        _StateData.CelebrateStateTimer.OnTimerComplete -= Celebrate_Complete;
        StateChange(PokerState.Idle);
    }

    private void ResolveWin()
    {
        int wins = _GameData.CurrentBet * _StateData.BetMulti;
        _GameData.AddWinnings(wins);
        SetOverlayText(_StateData.GetCelebrationText(wins, _StateData.PlayersHandKey));
        _EventManager.FireEvent(_AudioEvent.Play, _View.WinTune);
    }

    private void OnStaySelected()
    {
        for (int i = 0; i < _StateData.PlayersHand.Count; i++)
        {
            _StateData.PlayersHand[i].View.SetHightlight(false);
        }

        _StateData.ExchangeCards.Clear();

        OnExchange();
    }

    private void DoAutoPlay()
    {
        
        StateChange(PokerState.Reveal);
    }

    private void OnExchange()
    {
        UnregisterHand(_StateData.PlayersHand);
        UnregisterStay();
        UnregisterExchange();
        UnregisterOnSelectedBets();

        for (int i = 0; i < _StateData.ExchangeCards.Count; i++)
        {
            PokerCard card = GetViewBoundCard();
            _StateData.PlayersHand.Add(card);
            AttachCardToGroupView(card, _View.PlayersHandGroup.transform);
            card.SetState(PokerCardState.FaceUp);
        }

        for (int i = 0; i < _StateData.ExchangeCards.Count; i++)
        {
            if(_StateData.PlayersHand.Contains(_StateData.ExchangeCards[i]))
            {
                _StateData.PlayersHand.Remove(_StateData.ExchangeCards[i]);
            }

            _StateData.ExchangeCards[i].View.Kill();
            _StateData.ExchangeCards[i].View.SetActive(false);
        }

        _StateData.ExchangeCards.Clear();
        StateChange(PokerState.Reveal);
    }

    private string GetHoldemHand(List<PokerCard> hand)
    {
        string key = "";

        for (int i = 0; i < hand.Count; i++)
        {
            key += hand[i].Model.GetHoldemHandKey() + " ";
        }

        key.Trim();
        return key;
    }

    private void RegisterHand(List<PokerCard> hand) 
    {
        for (int i = 0; i < hand.Count; i++)
        {
            int index = i;
            hand[i].View.Select.onClick.AddListener(()=>{OnCardInHandClicked(hand[index]);});
        }
    }
    
    private void UnregisterHand(List<PokerCard> hand)
    {
        for (int i = 0; i < hand.Count; i++)
        {
            hand[i].View.Select.onClick.RemoveAllListeners();
        }
    }

    private void OnCardInHandClicked(PokerCard card)
    {
        if (_StateData.ExchangeCards.Contains(card))
        {
            card.View.SetHightlight(false);
            _StateData.ExchangeCards.Remove(card);
        }
        else
        {
            card.View.SetHightlight(true);
            _StateData.ExchangeCards.Add(card);
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

    private void UnregisterStay()
    {
        _View.Stay.onClick.RemoveListener(OnStaySelected);
    }

    private void UnregisterExchange()
    {
        _View.Exchange.onClick.RemoveListener(OnExchange);
    }

    private void RevealHand(List<PokerCard> hand)
    {
        for (int i = 0; i < hand.Count; i++)
        {
            hand[i].SetState(PokerCardState.FaceUp);
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
        _StateProcess.RegisterEnter(PokerState.Reveal.ToString(), Reveal_Enter);
        _StateProcess.RegisterEnter(PokerState.Outcome.ToString(), Outcome_Enter);
        _StateProcess.RegisterEnter(PokerState.Celebrate.ToString(), Celebrate_Enter);
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
        //_Debug.Log("DealCards");

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
        while (hand.Count < _CurrentRules)
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

    private void SetOverlayText(string text)
    {
        _View.OverlayText.SetActive(true);
        _View.SetOverlayText(text);
    }

    protected override void OnGameSelect(string name, object data)
    {
        if (_State == PokerState.Idle)
        {
            _EventManager.FireEvent(MenuEvent.GameSelectMenuShow.ToString());
        }
    }

}

public enum PokerState
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
