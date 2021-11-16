using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Febucci.UI;

public class GameDataEventListenerMeter : MonoBehaviour
{
    [SerializeField]
    private Meter _Meter;
    [SerializeField]
    private TextAnimatorPlayer _Text;

    private delegate void MeterProcess();
    private Dictionary<Meter, MeterProcess> _MeterProcess = new Dictionary<Meter, MeterProcess>();
    //private RMG_GameData _GameData = RMG_GameData.Instance;
    private void Awake()
    {
        BuildMeterProcess();
    }
    private void Start()
    {
        _MeterProcess[_Meter]();
    }

    private void BuildMeterProcess()
    {
        _MeterProcess.Add(Meter.Wallet, RegisterOnWalletChanged);
        _MeterProcess.Add(Meter.Bet, RegisterOnBetChanged);
        _MeterProcess.Add(Meter.Win, RegisterOnWinsChanged);
    }

    private void RegisterOnBetChanged()
    {
        RMG_GameData.OnBetChanged += OnValueChanged;
    }

    private void RegisterOnWalletChanged()
    {
        RMG_GameData.OnWalletChanged += OnValueChanged;
    }

    private void RegisterOnWinsChanged()
    {
        RMG_GameData.OnWinningsChanged += OnValueChanged;
    }

    private void OnValueChanged(int val)
    {
        _Text.ShowText("{diagexp}$" + val + "{/diagexp}");
    }


}

public enum Meter
{
    Wallet,
    Win,
    Bet
}
