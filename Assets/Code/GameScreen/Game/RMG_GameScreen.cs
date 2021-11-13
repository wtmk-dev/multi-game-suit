using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RMG_GameScreen : State
{
    public override IStateView View { get { return _View; } }

    private GameScreenTags _ScreenTags = new GameScreenTags();
    private GameData _GameData = GameData.Instance;
    private RMG_GameScreenView _View;
    public RMG_GameScreen(RMG_GameScreenView view)
    {
        _View = (RMG_GameScreenView)view;
        Tag = _ScreenTags.Game;
    }
}