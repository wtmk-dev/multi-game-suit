using System;
using System.Collections;
using System.Collections.Generic;

public class RMG_StartScreen : State
{
    public override IStateView View { get { return _View; } }

    public override void OnEnter()
    {
        _View.SetActive(true);
        Register();
    }

    public override void OnExit()
    {
        _Ready = false;
        Unregister();
        _View.SetActive(false);
    }

    private GameScreenTags _ScreenTags = new GameScreenTags();
    private GameModeTag _GameMode = new GameModeTag(); 
    private RMG_GameData _GameData = RMG_GameData.Instance;
    private RMG_StartScreenView _View;
    private Dood _Debug = Dood.Instance;

    public RMG_StartScreen(RMG_StartScreenView view)
    {
        _View = (RMG_StartScreenView)view;
        Tag = _ScreenTags.Start;
    }

    private void Register()
    {
        _View.HighLowButton.onClick.AddListener(HighLowSelected);
        _View.BlackjackButton.onClick.AddListener(BlackjackSelected);
        _View.FiveCardButton.onClick.AddListener(FiveCardSelected);
        _View.SevenCardButton.onClick.AddListener(SevenCardSelected);
    }

    private void Unregister()
    {
        _View.HighLowButton.onClick.RemoveListener(HighLowSelected);
        _View.BlackjackButton.onClick.RemoveListener(BlackjackSelected);
        _View.FiveCardButton.onClick.RemoveListener(FiveCardSelected);
        _View.SevenCardButton.onClick.RemoveListener(SevenCardSelected);
    }

    private void HighLowSelected() 
    {
        _Debug.Log("HighLowSelected");
        _GameData.SetGameMode(_GameMode.HighLow);
        StateChange();
    }

    private void BlackjackSelected() 
    {
        _Debug.Log("BlackjackSelected");
        _GameData.SetGameMode(_GameMode.Blackjack);
        StateChange();
    }

    private void FiveCardSelected() 
    {
        _Debug.Log("FiveCardSelected");
        _GameData.SetGameMode(_GameMode.Poker5);
        StateChange();
    }

    private void SevenCardSelected() 
    {
        _Debug.Log("SevenCardSelected");
        _GameData.SetGameMode(_GameMode.Poker7);
        StateChange();
    }

    private void StateChange()
    {
        NextState = _ScreenTags.Game;
        _Ready = true;
    }
}