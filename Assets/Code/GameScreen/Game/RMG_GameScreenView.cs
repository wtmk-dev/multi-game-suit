using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class RMG_GameScreenView : StateView
{
    public PokerView Poker;
    public BlackjackView Blackjack;
    public HighLowView HighLow;

    [SerializeField]
    private Canvas _UI;

    public void SetCanvasAsParent(List<RectTransform> transforms)
    {
        for (int i = 0; i < transforms.Count; i++)
        {
            transforms[i].SetParent(transform);
        }
    }
}
