using System.Collections;
using System.Collections.Generic;

public class HighLow : State 
{
    public override IStateView View { get { return _View; } }

    private HighLowView _View;
    public HighLow(HighLowView view)
    {
        _View = view;
    }
}
