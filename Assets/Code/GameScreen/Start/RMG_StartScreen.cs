using System;
using System.Collections;
using System.Collections.Generic;

public class RMG_StartScreen : State
{
    public override IStateView View { get { return _View; } }

    public override void OnEnter()
    {
        _EventManager.FireEvent(_AudioEvent.Play, _Audio);
        _View.SetActive(true);
        Register();
    }

    public override void OnExit()
    {
        _Ready = false;
        Unregister();
        _View.SetActive(false);
    }

    private GameScreenTags _GameScreenTag = new GameScreenTags();
    private EventManager _EventManager = EventManager.Instance;
    private GameScreenTags _ScreenTags = new GameScreenTags();
    private GameModeTag _GameMode = new GameModeTag(); 
    private RMG_GameData _GameData = RMG_GameData.Instance;
    private RMG_StartScreenView _View;
    private Dood _Debug = Dood.Instance;
    private AudioEvent _AudioEvent = new AudioEvent();

    private SoundManagerPlayArgs _Audio;

    public RMG_StartScreen(RMG_StartScreenView view)
    {
        _View = (RMG_StartScreenView)view;
        Tag = _ScreenTags.Start;

        _Audio = new SoundManagerPlayArgs();
        _Audio.Loop = true;
        _Audio.Aclip = _View.StartScreenAudio;
        _Audio.Volume = .02f;
    }

    private void Register()
    {
        _View.HighLowButton.onClick.AddListener(HighLowSelected);
        _View.BlackjackButton.onClick.AddListener(BlackjackSelected);
        _View.FiveCardButton.onClick.AddListener(FiveCardSelected);
        _View.SevenCardButton.onClick.AddListener(SevenCardSelected);
        _View.CreditsButton.onClick.AddListener(CreditsSelected);
    }

    private void Unregister()
    {
        _View.HighLowButton.onClick.RemoveListener(HighLowSelected);
        _View.BlackjackButton.onClick.RemoveListener(BlackjackSelected);
        _View.FiveCardButton.onClick.RemoveListener(FiveCardSelected);
        _View.SevenCardButton.onClick.RemoveListener(SevenCardSelected);
    }

    private void CreditsSelected()
    {
        StateChange(_GameScreenTag.Credits);
    }

    private void HighLowSelected() 
    {
        SelectGameMode(_GameMode.HighLow);
    }

    private void BlackjackSelected() 
    {
        SelectGameMode(_GameMode.Blackjack);
    }

    private void FiveCardSelected() 
    {
        SelectGameMode(_GameMode.Poker5);
    }

    private void SevenCardSelected() 
    {
        SelectGameMode(_GameMode.Poker7);
    }

    private void SelectGameMode(string mode)
    {
        _GameData.SetGameMode(mode);
        StateChange(_GameScreenTag.Game);
    }

    private void StateChange(string state)
    {
        NextState = state;
        _Ready = true;
    }
}