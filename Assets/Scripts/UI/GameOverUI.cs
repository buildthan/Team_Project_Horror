using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : BaseUI
{
    public TextMeshProUGUI scoreText;
    public Button titleButton;
    protected override UIState GetUIState()
    {
        return UIState.GameOver;
    }

    public override void Init(UIManager uiManager)
    {
        base.Init(uiManager);

        titleButton.onClick.AddListener(OnClickGameOverTitleButton);
    }

    public void OnClickGameOverTitleButton()
    {
        uiManager.PlayUIClickAudio();
        uiManager.OnClickGameOverTitle();
    }
}
