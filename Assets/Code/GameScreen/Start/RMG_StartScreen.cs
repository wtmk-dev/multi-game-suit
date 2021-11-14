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
        Unregister();
        _View.SetActive(false);
    }

    private GameScreenTags _ScreenTags = new GameScreenTags();
    private GameData _GameData = GameData.Instance;
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
    }

    private void BlackjackSelected() 
    {
        _Debug.Log("BlackjackSelected");
    }

    private void FiveCardSelected() 
    {
        _Debug.Log("FiveCardSelected");
    }

    private void SevenCardSelected() 
    {
        _Debug.Log("SevenCardSelected");
    }
}