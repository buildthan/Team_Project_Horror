using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseSettingUI : BaseUI
{
    public Button backButton;
    public Slider bgmSlider;
    public Slider sfxSlider;
    protected override UIState GetUIState()
    {
        return UIState.PauseSetting;
    }

    public override void Init(UIManager uiManager)
    {
        base.Init(uiManager);

        backButton.onClick.AddListener(OnClickPauseSettingBackButton);
    }

    public void Start()
    {
        bgmSlider.onValueChanged.AddListener(SoundManager.Instance.SetBGMVolume);
        sfxSlider.onValueChanged.AddListener(SoundManager.Instance.SetSFXVolume);
    }

    public void OnClickPauseSettingBackButton()
    {
        uiManager.OnClickPauseSettingBack();
    }
}
