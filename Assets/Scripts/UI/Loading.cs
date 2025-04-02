using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    [SerializeField] float time;
    [SerializeField] string scenenames;

    void Start()
    {
        scenenames = UIManager.Instance.nextSceneName;
        StartCoroutine(LoadScene(scenenames));
    }

    IEnumerator LoadScene(string name)
    {
        yield return null;
        AsyncOperation op = SceneManager.LoadSceneAsync(name); // 비동기 Scene 로딩 ( 로딩할 Scene 이름 )
        op.allowSceneActivation = false;  // Scene 이 로딩 되었을때 바로 실행할지 .
        yield return new WaitForSecondsRealtime(1.5f); // 1초 대기
        op.allowSceneActivation = true; // 로딩된 Scene 실행.

        if (name == "ISG_Item")
        {
            UIManager.Instance.ChangeState(UIState.Game);
        }
        else if (name == "KYH_UI")
        {
            UIManager.Instance.ChangeState(UIState.Title);
        }
        else if (name == "GameScene")
        {
            UIManager.Instance.ChangeState(UIState.Game);
        }
    }
}
