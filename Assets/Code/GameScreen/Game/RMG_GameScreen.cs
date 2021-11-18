using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RMG_GameScreen : State
{
    public override IStateView View { get { return _View; } }

    public override void OnEnter()
    {
        _View.SetActive(true);
        RegisterGameSelect();
        StartGameMode();
    }

    public override bool OnUpdate()
    {
        _ModeDirector.OnUpdate();
        return _Ready;
    }

    private GameScreenTags _ScreenTags = new GameScreenTags();
    private RMG_GameData _GameData = RMG_GameData.Instance;
    private EventManager _EventManager = EventManager.Instance;

    private RMG_GameScreenView _View;
    private PokerDeck _Deck;

    private HighLow _HighLow;
    private Blackjack _Blackjack;
    private Poker _FiveCard, _SevenCard;

    private StateDirector _ModeDirector;
    private IState[] _Modes = new IState[4]; //High Low , Blackjack, Poker 5, Poker 7
    private GameModeTag _GameModes = new GameModeTag();

    public RMG_GameScreen(RMG_GameScreenView view, PokerDeck deck)
    {
        Tag = _ScreenTags.Game;
        _View = (RMG_GameScreenView)view;
        _Deck = deck;

        InitModes(BuildModes());
    }

    private void RegisterGameSelect()
    {
        _View.GameSelect.onClick.AddListener(OnGameSelect);   
    }

    private void UnregisterGameSelect()
    {
        _View.GameSelect.onClick.RemoveListener(OnGameSelect);
    }

    private void OnGameSelect()
    {
        _EventManager.FireEvent(RMG_GameScreenEvent.GameSelect.ToString());
    }

    private void OnGameSelectExit(string name, object data)
    {
        RegisterGameSelect();
        _EventManager.FireEvent(RMG_GameScreenEvent.GameSelectExit.ToString());
    }

    private void StartGameMode()
    {
        _ModeDirector.SetCurrentState(_GameData.CurrentGameMode);
        _ModeDirector.IsActive = true;
    }

    private IState[] BuildModes()
    {
        _HighLow = new HighLow(_View.HighLow, _Deck);
        _HighLow.OnModeChange += GameModeChanged;
        _Blackjack = new Blackjack(_View.Blackjack, _Deck);
        _Blackjack.OnModeChange += GameModeChanged;
        _FiveCard = new Poker(_View.Poker, _Deck, _GameModes.Poker5, 5);
        _FiveCard.OnModeChange += GameModeChanged;
        _SevenCard = new Poker(_View.Poker, _Deck, _GameModes.Poker7, 7);
        _SevenCard.OnModeChange += GameModeChanged;

        IState[] gameStates = new IState[] { _HighLow, _Blackjack, _FiveCard, _SevenCard };

        return gameStates;
    }

    private void GameModeChanged()
    {
        for (int i = 0; i < _View.Bets.Count; i++)
        {
            _View.Bets[i].SetActive(false);
        }
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
            transforms.Add(_Deck.Cards[i].View.RectTransform);
        }
        return transforms;
    }
}

public enum RMG_GameScreenEvent
{
    GameSelect,
    GameSelected,
    GameSelectExit
}