using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Febucci.UI;

public class GameModeView : StateView
{
    public virtual void SetOverlayText(string text)
    {
        _OverlayText.ShowText(text);
    }
    public virtual void SetDealerScore(string text)
    {
        _DealerScore.ShowText(text);
    }

    public virtual void SetPlayerScore(string text)
    {
        _PlayerScore.ShowText(text);
    }

    
    public TextAnimatorPlayer _OverlayText, _PlayerScore, _DealerScore;
}
