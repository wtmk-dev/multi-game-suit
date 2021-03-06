using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSelectMenu : MonoBehaviour
{
    public Button HighLow, Blackjack, Poker5, Poker7;
    public GameObject Frame, GridLayout;

    private EventManager _EventManager = EventManager.Instance;
    private GameModeTag _GameModes = new GameModeTag();

    private void Awake()
    {
        //_EventManager.RegisterEventCallback(MenuEvent.GameSelectMenuShow.ToString(), OnShowMenu);
        //_EventManager.RegisterEventCallback(MenuEvent.GameSelectMenuHide.ToString(), OnHideMenu);
    }

    private void Start()
    {
        //SetActive(false);
        Register();
    }

    private void OnShowMenu(string name, object data)
    {
        SetActive(true);
        Register();
    }

    private void OnHideMenu(string name, object data)
    {
        _Exit();
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

    private void OnPoker5()
    {
        _EventManager.FireEvent(RMG_GameScreenEvent.GameSelected.ToString(), _GameModes.Poker5);
    }

    private void OnPoker7()
    {
        _EventManager.FireEvent(RMG_GameScreenEvent.GameSelected.ToString(), _GameModes.Poker7);
    }

    private void _Exit()
    { 
        /*
        if(Frame.activeInHierarchy == true)
        {
            SetActive(false);
            Unregister();

            _EventManager.FireEvent(MenuEvent.GameSelectMenuHideComplete.ToString());
        } 
        */
    }

    private void Register()
    {
        HighLow.onClick.AddListener(OnHighLow);
        Blackjack.onClick.AddListener(OnBlackjack);
        Poker5.onClick.AddListener(OnPoker5);
        Poker7.onClick.AddListener(OnPoker7);
    }

    private void Unregister()
    {
        HighLow.onClick.RemoveListener(OnHighLow);
        Blackjack.onClick.RemoveListener(OnBlackjack);
        Poker5.onClick.RemoveListener(OnPoker5);
        Poker7.onClick.AddListener(OnPoker7);
    }

    private void SetActive(bool isActive)
    {
        Frame.gameObject.SetActive(isActive);
        GridLayout.gameObject.SetActive(isActive);
    }
}

public enum MenuEvent
{
    GameSelectMenuShow,
    GameSelectMenuHide,
    GameSelectMenuHideComplete
}
