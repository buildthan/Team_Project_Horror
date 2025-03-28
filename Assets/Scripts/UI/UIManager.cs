using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum UIState
{
    Title,
    Game,
    Option,

    Loading
}

public class UIManager : MonoBehaviour
{

    UIState currentState = UIState.Title;

    //ChangeState로 관리
    TitleUI titleUI = null;
    GameUI gameUI = null;
    OptionUI optionUI = null;

    //독립적으로 관리
    LoadingUI loadingUI = null;

    static UIManager instance;

    public static UIManager Instance //싱글톤 선언
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }

    private void Awake() //싱글톤 선언
    {


        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            if (instance != this)
                Destroy(this.gameObject);
        }


        titleUI = GetComponentInChildren<TitleUI>(true);
        titleUI?.Init(this);
        gameUI = GetComponentInChildren<GameUI>(true);
        gameUI?.Init(this);
        optionUI = GetComponentInChildren<OptionUI>(true);
        optionUI?.Init(this);
        loadingUI = GetComponentInChildren<LoadingUI>(true);
        loadingUI?.Init(this);


        ChangeState(UIState.Title);
    }

    public void ChangeState(UIState state) //UI오브젝트를 on off 해주는 기능
    {
        currentState = state; //아래에서 해당하는 UI오브젝트를 찾아 on off 해줌
        titleUI?.SetActive(currentState);
        gameUI?.SetActive(currentState);
        optionUI?.SetActive(currentState);
    }


    //Title 내부

    public void OnClickStart()
    {
        SceneManager.LoadScene("KYH_UI_LoadingScene");
        loadingUI.SetActive(UIState.Loading); //로딩UI틀어주고
        ChangeState(UIState.Game);
    }

    public void OnClickOption()
    {
        ChangeState(UIState.Option);
    }

    public void OnClickExit()
    {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }

}
