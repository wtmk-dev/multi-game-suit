using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealerAnimationEventListener : MonoBehaviour
{
    [SerializeField]
    private Animator _Animator;

    private EventManager _EventManager = EventManager.Instance;
    
    void Awake()
    {
        RMG_GameData.OnBetChanged += OnBetChanged;
        RMG_GameData.OnWinningsChanged += OnWinningChanged;

        _EventManager.RegisterEventCallback(GameModeEvent.Idel.ToString(), OnIdel);

        _EventManager.RegisterEventCallback(GameModeEvent.Lose.ToString(), OnLose);
    }

    private void OnIdel(string name, object data)
    {
        _Animator.Play("Idle");
    }

    private void OnLose(string name, object data)
    {
        _Animator.Play("Lose");
    }

    private void OnBetChanged(int id)
    {
        _Animator.Play("Bet");
    }

    private void OnWinningChanged(int id)
    {
        _Animator.Play("Win");
    }
}
