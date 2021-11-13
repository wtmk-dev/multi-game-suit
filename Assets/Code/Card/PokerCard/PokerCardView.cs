using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PokerCardView : MonoBehaviour
{
    [SerializeField]
    private Image Image;

    public void Skin(Sprite sprite)
    {
        Image.sprite = sprite;
    }
}
