using System.Collections;
using System.Collections.Generic;

public class TributeCard : ICard
{
    public TributeCardState State { get { return _State; } }
    public TributeCardView View { get { return _View; } }
    public TributeCardModel Model { get { return _Model; } }

    public void SetState(TributeCardState state)
    {
        StateChange(state);
    }

    public void SetView(TributeCardView view)
    {
        _View = view;
    }

    public TributeCard(TributeCardModel model)
    {
        _Model = model;

        _StateProcess.RegisterEnter(TributeCardState.FaceUp.ToString(), FaceUp_Enter);
        _StateProcess.RegisterEnter(TributeCardState.FaceDown.ToString(), FaceDown_Enter);
        _State = TributeCardState.FaceDown;
    }

    private TributeCardView _View;
    private TributeCardModel _Model;
    private TributeCardState _State;
    //private TributeCardAnimationDict _AnimationDict = new TributeCardAnimationDict();
    private StateProcess _StateProcess = new StateProcess();    

    private void FaceUp_Enter()
    {
        _View.Skin(_Model.Front);
        //CheckStartAnimation();
    }

    private void FaceDown_Enter()
    {
        CheckEndAnimation();
        _View.Skin(_Model.Back);
    }

    private void StateChange(TributeCardState state)
    {
        _State = state;
        _StateProcess.StateEnter[_State.ToString()]();
    }

   /* private void CheckStartAnimation()
    {
        if(_AnimationDict.AnimationID.ContainsKey((_Model.Suit, _Model.Rank)))
        {
            _View.Animator.enabled = true;
            _View.Animator.Play(_AnimationDict.AnimationID[(_Model.Suit, _Model.Rank)]);
        }
    }*/

    private void CheckEndAnimation()
    {
        _View.Animator.enabled = false;

        //_View.Animator.Rebind();
        //_View.Animator.Update(0f);
    }

}

public enum TributeCardState
{
    FaceUp,
    FaceDown    
}
