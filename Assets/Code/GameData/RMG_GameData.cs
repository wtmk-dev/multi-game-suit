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

    public string CurrentGameMode { get { return _CurrentGameMode; } set { _CurrentGameMode = value; } }
    public int CurrentBet { get { return _CurrentBet; } set { _CurrentBet = value; } }


    public void SetGameMode(string mode)
    {
        _CurrentGameMode = mode;
    }

    public void AddWinnings(int totalWin)
    {
        _Money += _CurrentBet + totalWin;
        _CurrentBet = 0;
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
            return true;
        }
    }

    private string _CurrentGameMode;
    private int _Money;
    private int _CurrentBet;
    public RMG_GameData() 
    { 
        _Money = 9999999;
        _CurrentBet = 0;
    }
}
