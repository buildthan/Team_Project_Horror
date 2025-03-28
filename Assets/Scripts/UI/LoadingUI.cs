using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingUI : BaseUI //독립적으로 작동한다.
{
    protected override UIState GetUIState() //사용안함
    {
        return UIState.Loading;
    }

    public override void Init(UIManager uiManager)
    {
        base.Init(uiManager);

    }
}
