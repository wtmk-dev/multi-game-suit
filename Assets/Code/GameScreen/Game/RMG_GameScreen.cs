using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RMG_GameScreen : State
{
    public override IStateView View { get { return _View; } }

    public override void OnEnter()
    {
        _View.SetActive(true);
        _View.SetCanvasAsParent(ExtractRectsFromDeck());
    }

    private GameScreenTags _ScreenTags = new GameScreenTags();
    private GameData _GameData = GameData.Instance;
    private RMG_GameScreenView _View;
    private PokerDeck _Deck;

    private HighLow _HighLow;
    private Blackjack _Blackjack;
    private Poker _FiveCard, _SevenCard;

    private StateDirector _ModeDirector;
    private IState[] _Modes = new IState[4]; //High Low , Blackjack, Poker 5, Poker 7

    public RMG_GameScreen(RMG_GameScreenView view, PokerDeck deck)
    {
        Tag = _ScreenTags.Game;
        _View = (RMG_GameScreenView)view;
        _Deck = deck;

        InitModes(BuildModes());
    }

    private IState[] BuildModes()
    {
        _HighLow = new HighLow(_View.HighLow);
        _Blackjack = new Blackjack(_View.Blackjack);
        _FiveCard = new Poker(_View.Poker);
        _SevenCard = new Poker(_View.Poker);

        IState[] gameStates = new IState[] { _HighLow, _Blackjack, _FiveCard, _SevenCard };

        return gameStates;
    }

    private void InitModes(IState[] states)
    {
        _ModeDirector = new StateDirector(states);
    }

    private List<RectTransform> ExtractRectsFromDeck()
    {
        var transforms = new List<RectTransform>();
        for (int i = 0; i < _Deck.Cards.Count; i++)
        {
            _Deck.Cards[i].Init();
            transforms.Add(_Deck.Cards[i].View.RectTransform);
        }
        return transforms;
    }
}