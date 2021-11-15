using System.Collections;
using System.Collections.Generic;

public class PokerCard : ICard
{
    public PokerCardState State { get { return _State; } }
    public PokerCardView View { get { return _View; } }
    public PokerCardModel Model { get { return _Model; } }

    public void SetView(PokerCardView view)
    {
        _View = view;
    }

    public void SetState(PokerCardState state)
    {
        StateChange(state);
    }

    public PokerCard(PokerCardModel model)
    {
        _Model = model;

        _StateProcess.RegisterEnter(PokerCardState.FaceUp.ToString(), FaceUp_Enter);
        _StateProcess.RegisterEnter(PokerCardState.FaceDown.ToString(), FaceDown_Enter);
        _State = PokerCardState.FaceDown;
    }

    private PokerCardView _View;
    private PokerCardModel _Model;
    private PokerCardState _State;
    private PokerCardAnimationDict _AnimationDict = new PokerCardAnimationDict();
    private StateProcess _StateProcess = new StateProcess();    

    private void FaceUp_Enter()
    {
        _View.Skin(_Model.Front);
        CheckStartAnimation();
    }

    private void FaceDown_Enter()
    {
        CheckEndAnimation();
        _View.Skin(_Model.Back);
    }

    private void StateChange(PokerCardState state)
    {
        _State = state;
        _StateProcess.StateEnter[_State.ToString()]();
    }

    private void CheckStartAnimation()
    {
        if(_AnimationDict.AnimationID.ContainsKey((_Model.Suit, _Model.Rank)))
        {
            _View.Animator.enabled = true;
            _View.Animator.Play(_AnimationDict.AnimationID[(_Model.Suit, _Model.Rank)]);
        }
    }

    private void CheckEndAnimation()
    {
        _View.Animator.enabled = false;

        //_View.Animator.Rebind();
        //_View.Animator.Update(0f);
    }

}

public enum PokerCardState
{
    FaceUp,
    FaceDown    
}
