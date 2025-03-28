using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionUI : BaseUI
{
    public Button backButton;
    protected override UIState GetUIState()
    {
        return UIState.Option;
    }

    public override void Init(UIManager uiManager)
    {
        base.Init(uiManager);

        backButton.onClick.AddListener(OnClickBackButton);
    }

    public void OnClickBackButton()
    {
        uiManager.OnClickTitleOptionBack();
    }
}
