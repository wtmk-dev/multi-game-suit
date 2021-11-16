using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image), typeof(RectTransform))]
public class PokerCardView : MonoBehaviour, ICardView, IPoolable
{
    public event Action<IPoolable> OnReturnRequest;
    public RectTransform RectTransform { get { return _RectTransform; } }
    public Animator Animator { get { return _Animator;  } }

    public void Skin(Sprite sprite)
    {
        gameObject.SetActive(true);
        _Image.sprite = sprite;
    }

    private Image _Image;
    private RectTransform _RectTransform;
    private Animator _Animator;

    void Awake()
    {
        _Image = GetComponent<Image>();
        _RectTransform = GetComponent<RectTransform>();
        _Animator = GetComponent<Animator>();
    }
}
