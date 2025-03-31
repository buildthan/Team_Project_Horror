using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseSettingUI : BaseUI
{
    public Button backButton;
    protected override UIState GetUIState()
    {
        return UIState.PauseSetting;
    }

    public override void Init(UIManager uiManager)
    {
        base.Init(uiManager);

        backButton.onClick.AddListener(OnClickPauseSettingBackButton);
    }

    public void OnClickPauseSettingBackButton()
    {
        uiManager.OnClickPauseSettingBack();
    }
}
