using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Febucci.UI;

public class PokerView : StateView
{
    public GameObject PlayersHandGroup, DealersHandGroup;
    public List<BetController> Bets;
    public GameObject OverlayText;
    public Button Exchange, Stay;
    public SoundManagerPlayArgs WinTune;

    public virtual void SetOverlayText(string text)
    {
        _OverlayText.ShowText(text);
    }

    public virtual void SetTitleText(string text)
    {
        _TitleText.ShowText(text);
    }

    public PokerCardView GetCardView()
    {
        return (PokerCardView)_CardViewPool.GetPoolable();
    }

    [SerializeField]
    private GameObject _CardViewPrefab;
    [SerializeField]
    private TextAnimatorPlayer _OverlayText, _TitleText;

    private IPoolable[] _CardViews = new IPoolable[199];
    private Pool _CardViewPool;

    private void Awake()
    {
        BuildPool();
    }

    private void BuildPool()
    {
        for (int i = 0; i < _CardViews.Length; i++)
        {
            GameObject clone = Instantiate(_CardViewPrefab);
            PokerCardView view = clone.GetComponent<PokerCardView>();
            clone.transform.SetParent(transform);
            clone.gameObject.SetActive(false);
            _CardViews[i] = view;
        }

        _CardViewPool = new Pool(_CardViews);
    }

    /*
    public virtual void SetDealerScore(string text)
    {
        _DealerScore.ShowText(text);
    }

    public virtual void SetPlayerScore(string text)
    {
        _PlayerScore.ShowText(text);
    }

    [SerializeField]
    private TextAnimatorPlayer _OverlayText, _PlayerScore, _DealerScore;
    */
}
