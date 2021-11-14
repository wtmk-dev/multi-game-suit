using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BetController : MonoBehaviour
{
    public event Action<BetEventArgs> OnBetSelected;
    public void Init(BetEventArgs betEventArgs)
    {
        _BetEventArgs = betEventArgs;
        _Lable.SetText($"${betEventArgs.Bet}");
        _Init = true;
    }

    [SerializeField]
    private Button _Selected;
    [SerializeField]
    private TextMeshProUGUI _Lable;
    private BetEventArgs _BetEventArgs;
    private bool _Init;

    private void Awake()
    {
        _Selected.onClick.AddListener(OnSelected);
        _Init = false;
    }

    private void OnSelected()
    {
        if(!_Init)
        {
            return;
        }

        OnBetSelected?.Invoke(_BetEventArgs);
    }
}
