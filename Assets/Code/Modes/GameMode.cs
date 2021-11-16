using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode : State
{
    private EventManager _EventManager = EventManager.Instance;

    protected virtual void RegisterGameScreenEvents()
    {
        _EventManager.RegisterEventCallback(RMG_GameScreenEvent.GameSelect.ToString(), OnGameSelect);
        _EventManager.RegisterEventCallback(RMG_GameScreenEvent.GameSelected.ToString(), OnGameSelected);

        //_EventManager.RegisterEventCallback(RMG_GameScreenEvent.GameSelectExit.ToString(), OnGameSelectExit);
    }

    protected virtual void UnregisterGameScreenEvents()
    {
        _EventManager.UnregisterEventCallback(RMG_GameScreenEvent.GameSelect.ToString(), OnGameSelect);
        _EventManager.UnregisterEventCallback(RMG_GameScreenEvent.GameSelected.ToString(), OnGameSelected);

        // _EventManager.UnregisterEventCallback(RMG_GameScreenEvent.GameSelectExit.ToString(), OnGameSelectExit);
    }

    protected virtual void OnGameSelect(string name, object data)
    {
        //override to unregister inputs
    }

    protected virtual void OnGameSelected(string name, object data)
    {
        string nextGame = (string)data;

        if (Tag != nextGame)
        {
            //TO:DO check for valid transtion
            NextState = nextGame;
            _Ready = true;
        }
    }

    protected virtual void OnGameSelectExit(string name, object data)
    {
        //override to register inputs
    }

}
