using System.Collections;
using System.Collections.Generic;

public class Blackjack : State 
{
    public override string Tag { get; protected set; }
    public override IStateView View { get { return _View; } }

    public override void OnEnter()
    {
        StateChange(BlackjackState.Init);
    }

    public override bool OnUpdate()
    {
        _StateData.Update();
        return _Ready;
    }

    private Dood _Debug = Dood.Instance;
    private BlackjackView _View;
    private GameModeTag _GameModeTag = new GameModeTag();

    private BlackjackState _State;
    private RMG_GameData _GameData = new RMG_GameData();
    private StateProcess _StateProcess = new StateProcess();

    private BlackjackStateData _StateData;
    private PokerDeck _Deck;
    private BlackjackCardValue _Rules = new BlackjackCardValue(); 

    public Blackjack(BlackjackView view, PokerDeck deck)
    {
        _Deck = deck;
        _View = view;
        Tag = _GameModeTag.Blackjack;

        RegisterStateEnter();
    }

    private void OnStaySelected()
    {
        _StateData.Selection = BlackjackSelection.Stay;
        StateChange(BlackjackState.Resolve);
    }

    private void OnHitSelected()
    {
        _StateData.Selection = BlackjackSelection.Hit;
        StateChange(BlackjackState.Resolve);
    }

    private void BetSelected(BetEventArgs bet)
    {
        UnregisterOnSelectedBets();
        if (_GameData.PlaceBet(bet.Bet))
        {
            StateChange(BlackjackState.Deal);
        }
        else
        {
            RegisterOnSelectedBets();
            StateChange(BlackjackState.Idle);
        }
    }

    private void Init_Enter()
    {
        _StateData = new BlackjackStateData(_Deck);
        _View.SetActive(true);
        _View.OverlayText.SetActive(false);

        InitBets();
        StateChange(BlackjackState.Idle);
    }

    private void InitBets()
    {
        int baseBet = _GameData.BaseBet;

        for (int i = 0; i < _View.Bets.Count; i++)
        {
            int multi = (i + 1);
            baseBet *= multi;

            _View.Bets[i].gameObject.SetActive(true);
            _View.Bets[i].Init(new BetEventArgs(baseBet));
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
        StateChange(BlackjackState.Pick);
    }

    private (int,int) EvaluateHand(List<PokerCard> hand)
    {
        int valAceAs1 = 0;
        int valAceAs11 = 0;
        for (int i = 0; i < hand.Count; i++)
        {
            if(hand[i].Model.Rank == PokerCardRank.Ace)
            {
                valAceAs1 += 1;
                valAceAs11 += 11;
            }
            else
            {
                valAceAs11 += _Rules.CardValue[hand[i].Model.Rank];
                valAceAs11 += 10;
                valAceAs1 += _Rules.CardValue[hand[i].Model.Rank];
            }
        }

        return (valAceAs1, valAceAs11);
    }

    private void DealCards()
    {
        GetStartingHand(_StateData.DealersHand);
        GetStartingHand(_StateData.PlayersHand);

        AttachHandToGroupView(_StateData.DealersHand, _View.DealersHandGroup.transform);
        AttachHandToGroupView(_StateData.PlayersHand, _View.PlayersHandGroup.transform);

        ShowCardsInHand(_StateData.PlayersHand);
        ShowCard(_StateData.DealersHand[0]);
    }

    private void DealCard(UnityEngine.Transform target)
    {
        PokerCard card = (PokerCard) _StateData.Deck.Draw();
        AttachCardToGroupView(card, target);
    }

    private void ShowCardsInHand(List<PokerCard> hand)
    {
        for (int i = 0; i < hand.Count; i++)
        {
            ShowCard(hand[i]);
        }
    }

    private void ShowCard(PokerCard card)
    {
        card.SetState(PokerCardState.FaceUp);
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
        card.View.transform.SetParent(transform);
    }

    private void GetStartingHand(List<PokerCard> hand)
    {
        while (hand.Count < _StateData.StaringHandSize)
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

    private void Deal_MessageHide()
    {
        _StateData.DealStateTimer.OnTimerComplete -= Deal_MessageHide;
        _View.SetOverlayText("");
        _View.OverlayText.SetActive(false);
    }

    private void Pick_Enter()
    {
        RegisterStay();
        RegisterHit();
        SetOverlayText(_StateData.GetDeal_EnterText(EvaluateHand(_StateData.PlayersHand)));
    }

    private void Resolve_Enter()
    {
        if(_StateData.Selection == BlackjackSelection.Hit)
        {
            Resolve_Hit();
        }else if(_StateData.Selection == BlackjackSelection.Stay)
        {
            Resolve_Stay();
        }

        
    }

    private void Resolve_Hit()
    {
        DealCard(_View.PlayersHandGroup.transform);
        //check for bust or see if can hit again

        (int, int) score = EvaluateHand(_StateData.PlayersHand);
        if (score.Item1 < _Rules.HighetsToStillHit)
        {
            StateChange(BlackjackState.Pick);
        }
    }

    private void Resolve_Stay()
    { 
    
    }

    private void Outcome_Enter()
    {
       
    }

    private void Celebrate_Enter()
    {
        
    }

    private void Celebrate_Complete()
    {
        _StateData.CelebrateStateTimer.OnTimerComplete -= Celebrate_Complete;
        //HideOverlayText();
        StateChange(BlackjackState.Idle);
    }

    private void RegisterStateEnter()
    {
        _StateProcess.RegisterEnter(BlackjackState.Init.ToString(), Init_Enter);
        _StateProcess.RegisterEnter(BlackjackState.Idle.ToString(), Idle_Enter);
        _StateProcess.RegisterEnter(BlackjackState.Deal.ToString(), Deal_Enter);
        _StateProcess.RegisterEnter(BlackjackState.Pick.ToString(), Pick_Enter);
        _StateProcess.RegisterEnter(BlackjackState.Resolve.ToString(), Resolve_Enter);
        _StateProcess.RegisterEnter(BlackjackState.Outcome.ToString(), Outcome_Enter);
        _StateProcess.RegisterEnter(BlackjackState.Celebrate.ToString(), Celebrate_Enter);
    }

    private void RegisterOnSelectedBets()
    {
        for (int i = 0; i < _View.Bets.Count; i++)
        {
            _View.Bets[i].OnBetSelected += BetSelected;
        }
    }

    private void RegisterStay()
    {
        _View.Stay.onClick.AddListener(OnStaySelected);
    }

    private void RegisterHit()
    {
        _View.Hit.onClick.AddListener(OnHitSelected);
    }

    private void UnregisterOnSelectedBets()
    {
        for (int i = 0; i < _View.Bets.Count; i++)
        {
            _View.Bets[i].OnBetSelected -= BetSelected;
        }
    }

    private void StateChange(BlackjackState state, float waitTime = 0f)
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

    private void SetOverlayText(string text)
    {
        _View.OverlayText.SetActive(true);
        _View.SetOverlayText(text);
    }
}

public enum BlackjackState
{
    Init,
    Bet,
    Deal,
    Pick,
    Resolve,
    Outcome,
    Celebrate,
    Idle
}

public enum BlackjackSelection
{
    Hit, Stay
}
