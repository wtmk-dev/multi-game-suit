using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image), typeof(RectTransform))]
public class TributeCardView : MonoBehaviour, ICardView, IPoolable
{
    public event Action<IPoolable> OnReturnRequest;
    public RectTransform RectTransform { get { return _RectTransform; } }
    public Animator Animator { get { return _Animator;  } }

    public Button Select { get { return _Select; } }

    public void Skin(Sprite sprite)
    {
        gameObject.SetActive(true);
        _Image.sprite = sprite;
    }

    public void Kill()
    {
        OnReturnRequest?.Invoke(this);
    }

    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    public void SetHightlight(bool isActive)
    {
        _Highlight.gameObject.SetActive(isActive);
    }

    private Image _Image;
    private RectTransform _RectTransform;
    private Animator _Animator;
    private Button _Select;
    [SerializeField]
    private Image _Highlight;

    void Awake()
    {
        _Image = GetComponent<Image>();
        _RectTransform = GetComponent<RectTransform>();
        _Animator = GetComponent<Animator>();
        _Select = GetComponent<Button>();

        SetHightlight(false);
    }
}
