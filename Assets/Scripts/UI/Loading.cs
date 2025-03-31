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

        
        UIManager.Instance.ChangeState(UIState.Game);

        // 플레이어와 인벤토리 연결
        //CharacterManager.Instance.Player.controller.inventory += UIManager.Instance.gameUI.Toggle;

        //UIManager.Instance.gameUI.dropPosition = CharacterManager.Instance.Player.dropPosition;

        //CharacterManager.Instance.Player.addItem += AddItem;  // delegate에 함수 등록
    }
}
