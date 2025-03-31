using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseUI : BaseUI
{
    public Button backButton;
    public Button settingButton;
    public Button titleButton;
    protected override UIState GetUIState()
    {
        return UIState.Pause;
    }

    public override void Init(UIManager uiManager)
    {
        base.Init(uiManager);

        backButton.onClick.AddListener(OnClickPauseBackButton);
        settingButton.onClick.AddListener(OnClickPauseSettingButton);
        titleButton.onClick.AddListener(OnClickPauseTitleButton);
    }

    public void OnClickPauseBackButton()
    {
        uiManager.OnClickPauseBack();
    }
    
    public void OnClickPauseSettingButton()
    {
        uiManager.OnClickPauseSetting();
    }

    public void OnClickPauseTitleButton()
    {
        uiManager.OnClickPauseTitle();
    }

}
