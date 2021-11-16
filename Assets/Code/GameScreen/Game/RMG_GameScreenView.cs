using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RMG_GameScreenView : StateView
{
    public PokerView Poker;
    public BlackjackView Blackjack;
    public HighLowView HighLow;
    public Button GameSelect;
    public List<GameObject> Bets;

    [SerializeField]
    private Canvas _UI;
    
    public void SetCanvasAsParent(List<RectTransform> transforms)
    {
        for (int i = 0; i < transforms.Count; i++)
        {
            transforms[i].SetParent(_UI.transform);
            transforms[i].position = Vector3.zero;
        }
    }
}
