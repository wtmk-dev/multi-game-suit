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

    public void SetGameMode(string mode)
    {
        _CurrentGameMode = mode;
    }

    public bool PlaceBet(int bet)
    {
        if(bet > _Money)
        {
            return false;
        }
        else
        {
            _Money -= bet;
            return true;
        }
    }

    private string _CurrentGameMode;
    private int _Money;

    public RMG_GameData() { _Money = 9999999; }
}
