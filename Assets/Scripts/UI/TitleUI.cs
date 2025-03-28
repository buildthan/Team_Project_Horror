using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleUI : BaseUI
{
    public Button startButton; //주석
    public Button optionButton;
    public Button exitButton;
    protected override UIState GetUIState()
    {
        return UIState.Title;
    }

    public override void Init(UIManager uiManager)
    {
        base.Init(uiManager);

        startButton.onClick.AddListener(OnClickStartButton);
        optionButton.onClick.AddListener(OnClickOptionButton);
        exitButton.onClick.AddListener(OnClickExitButton);
    }

    public void OnClickStartButton()
    {
        uiManager.OnClickStart();
    }

    public void OnClickOptionButton()
    {
        uiManager.OnClickOption();
    }

    public void OnClickExitButton()
    {
        uiManager.OnClickExit();
    }
}
