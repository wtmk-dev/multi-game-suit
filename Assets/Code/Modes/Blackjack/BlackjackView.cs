using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Febucci.UI;

public class BlackjackView : StateView
{
    public GameObject PlayersHandGroup, DealersHandGroup;
    public Button Hit, Stay;
    public List<BetController> Bets;
    public GameObject OverlayText;
    public GameObject GameSelectGrid;

    public void SetOverlayText(string text)
    {
        _OverlayText.ShowText(text);
    }
    public void SetDealerScore(string text)
    {
        _DealerScore.ShowText(text);
    }

    public void SetPlayerScore(string text)
    {
        _PlayerScore.ShowText(text);
    }

    public PokerCardView GetCardView()
    {
        return (PokerCardView) _CardViewPool.GetPoolable();
    }

    [SerializeField]
    private GameObject _CardViewPrefab;
    [SerializeField]
    private Canvas _UI;
    [SerializeField]
    private TextAnimatorPlayer _OverlayText, _PlayerScore, _DealerScore;

    private IPoolable[] _CardViews = new IPoolable[20];
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

}
