using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RMG_CreditsScreen : State
{
    public override IStateView View {get{return _View;}}

    public override void OnEnter()
    {
        _View.SetActive(true);
        _View.Exit.onClick.AddListener(ExitSelected);
    }

    public override void OnExit()
    {
        _View.SetActive(false);
        _View.Exit.onClick.RemoveListener(ExitSelected);
        _Ready = false;
    }

    private GameScreenTags _ScreenTag = new GameScreenTags();
    private RMG_CreditsView _View;
    public RMG_CreditsScreen(RMG_CreditsView view) 
    {
        Tag = _ScreenTag.Credits;
        _View = view;
    }

    private void ExitSelected()
    {
        NextState = _ScreenTag.Start;
        _Ready = true;
    }
}
