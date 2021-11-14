using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighLowView : StateView
{
    public PokerCardView Left, Right, Deck;
    public Button High, Low;
    public List<BetController> Bets;

    public override void SetActive(bool isActive)
    {
        base.SetActive(isActive);
        _UI.gameObject.SetActive(isActive);
    }

    [SerializeField]
    private Canvas _UI;

    private void Awake()
    {
        _UI.gameObject.SetActive(false);
    }
}
