using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode : State
{
    private EventManager _EventManager = EventManager.Instance;

    public virtual void RegisterGameScreenEvents()
    {
        _EventManager.RegisterEventCallback(RMG_GameScreenEvent.GameSelect.ToString(), OnGameSelect);
        _EventManager.RegisterEventCallback(RMG_GameScreenEvent.GameSelectExit.ToString(), OnGameSelectExit);
    }

    public virtual void UnregisterGameScreenEvents()
    {
        _EventManager.UnregisterEventCallback(RMG_GameScreenEvent.GameSelect.ToString(), OnGameSelect);
        _EventManager.UnregisterEventCallback(RMG_GameScreenEvent.GameSelectExit.ToString(), OnGameSelectExit);
    }

    protected void OnGameSelect(string name, object data)
    {
        //
    }

    protected void OnGameSelectExit(string name, object data)
    {
        //
    }
}
