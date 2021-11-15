using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Febucci.UI;

public class HighLowView : StateView
{
    public PokerCardView Left, Right, Deck;
    public Button High, Low;
    public List<BetController> Bets;
    public GameObject OverlayText;
  
    public override void SetActive(bool isActive)
    {
        base.SetActive(isActive);
        _UI.gameObject.SetActive(isActive);
    }

    public void SetOverlayText(string text)
    {
        _OverlayText.ShowText(text);
    }

    [SerializeField]
    private Canvas _UI;
    [SerializeField]
    private TextAnimatorPlayer _OverlayText;

    private void Awake()
    {
        _UI.gameObject.SetActive(false);
    }
}
