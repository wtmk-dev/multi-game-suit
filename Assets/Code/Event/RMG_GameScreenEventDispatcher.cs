using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RMG_GameScreenEventDispatcher : MonoBehaviour
{
    [SerializeField]
    private RMG_GameScreenEvent _Event;
    [SerializeField]
    private Button _Button;

    private EventManager _EventManager = EventManager.Instance;
    private void Awake()
    {
        _Button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        _EventManager.FireEvent(_Event.ToString());
    }
}
