using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionUI : BaseUI
{
    public Button backButton;
    public Slider bgmSlider;
    public Slider sfxSlider;
    protected override UIState GetUIState()
    {
        return UIState.Option;
    }

    public override void Init(UIManager uiManager)
    {
        base.Init(uiManager);

        backButton.onClick.AddListener(OnClickBackButton);
    }

    public void Start()
    {
        bgmSlider.onValueChanged.AddListener(SoundManager.Instance.SetBGMVolume);
        sfxSlider.onValueChanged.AddListener(SoundManager.Instance.SetSFXVolume);
    }

    public void OnClickBackButton()
    {
        uiManager.PlayUIClickAudio();
        uiManager.OnClickTitleOptionBack();
    }
}
