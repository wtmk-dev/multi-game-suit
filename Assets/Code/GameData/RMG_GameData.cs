using System;
using System.Collections;
using System.Collections.Generic;

public class RMG_GameData
{
    private static readonly RMG_GameData _Instance = new RMG_GameData();

    public static RMG_GameData Instance
    {
        get
        {
            return _Instance;
        }
    }

    public static event Action<int> OnBetChanged;
    public static event Action<int> OnWalletChanged;
    public static event Action<int> OnWinningsChanged;

    public string CurrentGameMode { get { return _CurrentGameMode; } set { _CurrentGameMode = value; } }
    public int CurrentBet { get { return _CurrentBet; } set { _CurrentBet = value; } }
    public int BaseBet { get { return _BaseBet; } private set { _BaseBet = value; } }
    public bool IsAutoPlay { get { return _IsAutoPlay; } }

    public void SetGameMode(string mode)
    {
        _CurrentGameMode = mode;
    }

    public void AddWinnings(int totalWin)
    {
        _Money += _CurrentBet + totalWin;
        _CurrentBet = 0;

        OnWinningsChanged?.Invoke(totalWin);
        OnBetChanged?.Invoke(_CurrentBet);
        OnWalletChanged?.Invoke(_Money);
    }

    public bool PlaceBet(int bet)
    {
        if(bet > _Money)
        {
            return false;
        }
        else
        {
            _CurrentBet = bet;
            _Money -= bet;

            OnWalletChanged?.Invoke(_Money);
            OnBetChanged?.Invoke(_CurrentBet);
            return true;
        }
    }

    public void Push()
    {
        _Money += _CurrentBet;
        _CurrentBet = 0;

        OnWalletChanged?.Invoke(_Money);
        OnBetChanged?.Invoke(_CurrentBet);
    }

    public void AddMoney(int money)
    {
        _Money += money;
    }

    public void SetBaseBet(int bet)
    {
        _BaseBet = bet;
    }

    public void SetAutoPlay(bool isAutoPlay)
    {
        _IsAutoPlay = isAutoPlay;
    }

    private string _CurrentGameMode;
    private int _Money;
    private int _CurrentBet;
    private int _BaseBet;
    public bool _IsAutoPlay;
    public RMG_GameData() 
    { 
        _Money = 0;
        _CurrentBet = 0;
        _BaseBet = 5;
    }
}
