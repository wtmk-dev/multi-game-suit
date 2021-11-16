using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSelectMenu : MonoBehaviour
{
    public Button HighLow, Blackjack, Exit;

    private EventManager _EventManager = EventManager.Instance;
    private GameModeTag _GameModes = new GameModeTag();

    private void Awake()
    {
        _EventManager.RegisterEventCallback(RMG_GameScreenEvent.GameSelect.ToString(), OnGameSelect);
    }

    private void OnGameSelect(string name, object data)
    {
        Register();
        SetActive(transform);
    }

    private void OnBlackjack()
    {
        _EventManager.FireEvent(RMG_GameScreenEvent.GameSelected.ToString(), _GameModes.Blackjack);
        _Exit();
    }

    private void OnHighLow()
    {
        _EventManager.FireEvent(RMG_GameScreenEvent.GameSelected.ToString(), _GameModes.HighLow);
        _Exit();
    }

    private void OnExit()
    {
        _EventManager.FireEvent(MenuEvent.GameSelectMenuExit.ToString());
        _Exit();
    }

    private void _Exit()
    {
        Unregister();
        SetActive(false);
    }

    private void Register()
    {
        HighLow.onClick.AddListener(OnHighLow);
        Blackjack.onClick.AddListener(OnBlackjack);
        Exit.onClick.AddListener(OnExit);
    }

    private void Unregister()
    {
        HighLow.onClick.RemoveListener(OnHighLow);
        Blackjack.onClick.RemoveListener(OnBlackjack);
        Exit.onClick.RemoveListener(OnExit);
    }

    private void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}

public enum MenuEvent
{
    GameSelectMenuExit,
}
