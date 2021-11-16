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
        UnegisterStay();
        UnregisterHit();
        _StateData.Selection = BlackjackSelection.Stay;
        StateChange(BlackjackState.Resolve);
    }

    private void OnHitSelected()
    {
        UnegisterStay();
        UnregisterHit();
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

    private void DealCard(List<PokerCard> hand, UnityEngine.Transform target)
    {
        PokerCard card = GetViewBoundCard();
        AttachCardToGroupView(card, target);
        card.SetState(PokerCardState.FaceUp);
        hand.Add(card);
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

    private void Pick_Enter()
    {
        RegisterStay();
        RegisterHit();

        SetOverlayText(_StateData.GetDeal_EnterText(EvaluateHand(_StateData.PlayersHand)));
    }

    private void Resolve_Enter()
    {
        if (_StateData.Selection == BlackjackSelection.Hit)
        {
            Resolve_Hit();
        }else if(_StateData.Selection == BlackjackSelection.Stay)
        {
            Resolve_Stay();
        }
    }

    private void Resolve_Hit()
    {
        DealCard(_StateData.PlayersHand, _View.PlayersHandGroup.transform);
        
        //check for bust or see if can hit again

        (int, int) score = EvaluateHand(_StateData.PlayersHand);
        _Debug.Log(score.ToString());

        if (HasAceInHand(_StateData.PlayersHand) && score.Item2 == _Rules.Blackjack)
        {
            StateChange(BlackjackState.Outcome);
        } else if (score.Item1 < _Rules.MaxToHit)
        {
            StateChange(BlackjackState.Pick);
        } else if (HasAceInHand(_StateData.PlayersHand) && score.Item2 < _Rules.MaxToHit)
        {
            StateChange(BlackjackState.Pick);
        } else 
        {
            StateChange(BlackjackState.Outcome);
        }
    }

    private void Resolve_Stay()
    {
        StateChange(BlackjackState.ResolveDealer);
    }

    private bool HasAceInHand(List<PokerCard> hand)
    {
        bool hasAce = false;
        for (int i = 0; i < hand.Count; i++)
        {
            if(hand[i].Model.Rank == PokerCardRank.Ace)
            {
                hasAce = true;
                break;
            }
        }

        return hasAce;
    }

    

    private void Outcome_Enter()
    {
        _Debug.Log("outcome");
        (int, int) score = EvaluateHand(_StateData.PlayersHand);
        //check for bust
        if (score.Item1 > _Rules.Blackjack)
        {
            _StateData.HasBusted = true;
            StateChange(BlackjackState.Celebrate);
        } else if (HasAceInHand(_StateData.PlayersHand) && score.Item2 > _Rules.Blackjack)
        {
            _StateData.HasBusted = true;
            StateChange(BlackjackState.Celebrate);
        }
        else
        {
            StateChange(BlackjackState.ResolveDealer);
        }
    }

    private void ResolveDealer_Enter()
    {
        _StateData.DealersHand[1].SetState(PokerCardState.FaceUp);
        (int, int) score = EvaluateHand(_StateData.DealersHand);
        _Debug.Log(score.ToString());

        int scoreToUse = SetDealersAceValue(score);

        if (scoreToUse < _Rules.DealerMinToHit)
        {
            _StateData.ResolveDealerTimer.OnTimerComplete += ResolveDealer_Hit;
            _StateData.ResolveDealerTimer.Start(_StateData.ResolveDealerTime);
        }else if(scoreToUse > _Rules.DealerMinToStay)
        {
            StateChange(BlackjackState.Celebrate);
        }
    }

    private int SetDealersAceValue((int,int) score)
    {
        int scoreToUse = 0;
        if (HasAceInHand(_StateData.DealersHand))
        {
            if (score.Item1 >= _Rules.DealerMinToHit || score.Item2 >= _Rules.DealerMinToHit)
            {
                scoreToUse = score.Item2;
            }
            else if (score.Item1 <= _Rules.DealerMinToHit || score.Item2 <= _Rules.DealerMinToHit)
            {
                scoreToUse = score.Item1;
            }
        }
        return scoreToUse;
    }

    private void ResolveDealer_Hit()
    {
        _StateData.ResolveDealerTimer.OnTimerComplete -= ResolveDealer_Hit;
    }

    private void Celebrate_Enter()
    {
        _Debug.Log("Celebrate");
        if(_StateData.HasBusted)
        {
            _StateData.HasBusted = false;
            SetOverlayText(_StateData.BustText);
        }

        ClearHand(_StateData.PlayersHand);
        ClearHand(_StateData.DealersHand);
    }

    private void ClearHand(List<PokerCard> hand)
    {
        for (int i = 0; i < hand.Count; i++)
        {
            hand[i].View.Kill();
        }

        hand.Clear();
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
        _StateProcess.RegisterEnter(BlackjackState.ResolveDealer.ToString(), ResolveDealer_Enter);
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

    private void UnegisterStay()
    {
        _View.Stay.onClick.RemoveListener(OnStaySelected);
    }

    private void UnregisterHit()
    {
        _View.Hit.onClick.RemoveListener(OnHitSelected);
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
    ResolveDealer,
    Celebrate,
    Idle
}

public enum BlackjackSelection
{
    Hit, Stay
}
