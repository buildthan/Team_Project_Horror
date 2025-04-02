using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum UIState
{
    Title,
    Game,
    Option,
    GameOver,
    Pause,
    PauseSetting,

    Nothing
}

public class UIManager : MonoBehaviour
{

    UIState currentState = UIState.Title;
    public string nextSceneName;

    //ChangeState로 관리
    TitleUI titleUI = null;
    public GameUI gameUI = null;
    OptionUI optionUI = null;
    GameOverUI gameOverUI = null;
    PauseUI pauseUI = null;
    PauseSettingUI pauseSettingUI = null;

    public DamageIndicator damageIndicator = null; //유니티에서 할당

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
        gameOverUI = GetComponentInChildren<GameOverUI>(true);
        gameOverUI?.Init(this);
        pauseUI = GetComponentInChildren<PauseUI>(true);
        pauseUI?.Init(this);
        pauseSettingUI = GetComponentInChildren<PauseSettingUI>(true);
        pauseSettingUI?.Init(this);


        ChangeState(UIState.Title);
    }

    public void ChangeState(UIState state) //UI오브젝트를 on off 해주는 기능
    {
        currentState = state; //아래에서 해당하는 UI오브젝트를 찾아 on off 해줌
        titleUI?.SetActive(currentState);
        gameUI?.SetActive(currentState);
        optionUI?.SetActive(currentState);
        gameOverUI?.SetActive(currentState);
        pauseUI?.SetActive(currentState);
        pauseSettingUI?.SetActive(currentState);
    }

    public void Update() //테스트용. 나중에 플레이어에서 수정할 것.
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseUI(); //패스키 할당용
        }

    }

    public void PlayUIClickAudio()
    {
        SoundManager.Instance.PlaySFX("SFX_UI_Button_Keyboard_Space_Thick_1", transform.position);
    }


    //Title 내부

    public void OnClickStart()
    {
        nextSceneName = "GameScene"; //로딩이 끝나면 이동할 씬 이름
        ChangeState(UIState.Nothing); //로딩하는 동안 UI를 모두 꺼준다.
        SceneManager.LoadScene("KYH_UI_LoadingScene");
    }

    public void OnClickOption()
    {
        optionUI.bgmSlider.value = SoundManager.Instance.bgmVolume;
        optionUI.sfxSlider.value = SoundManager.Instance.sfxVolume;
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

    //Option 내부

    public void OnClickTitleOptionBack()
    {
        ChangeState(UIState.Title);
    }

    //GameOver 내부

    public void InvokeGameOverUI() //게임오버 UI 발동
    {
        CharacterManager.Instance.Player.controller.ToggleCursor(true);
        ChangeState(UIState.GameOver);
        Time.timeScale = 0f;
    }
    public void OnClickGameOverTitle() //게임오버씬에서 타이틀로 이동 클릭
    {
        Time.timeScale = 1f;
        nextSceneName = "KYH_UI"; //로딩이 끝나면 이동할 씬 이름
        ChangeState(UIState.Nothing);
        SceneManager.LoadScene("KYH_UI_LoadingScene");
    }

    //Pause 내부

    public void TogglePauseUI()
    {
        if (currentState == UIState.Pause) //패스 화면이 켜져있는 경우 꺼주고
        {
            PlayUIClickAudio();
            CharacterManager.Instance.Player.controller.ToggleCursor(false);
            ChangeState(UIState.Game);
            Time.timeScale = 1f;
        }
        else if(currentState == UIState.Game) //패스 화면이 꺼져있는 경우 켜준다.
        {
            PlayUIClickAudio();
            CharacterManager.Instance.Player.controller.ToggleCursor(true);
            ChangeState(UIState.Pause);
            Time.timeScale = 0f;
        }
    }

    public void OnClickPauseBack()
    {
        CharacterManager.Instance.Player.controller.ToggleCursor(false);
        ChangeState(UIState.Game);
        Time.timeScale = 1f;
    }

    public void OnClickPauseSetting()
    {
        pauseSettingUI.bgmSlider.value = SoundManager.Instance.bgmVolume;
        pauseSettingUI.sfxSlider.value = SoundManager.Instance.sfxVolume;
        ChangeState(UIState.PauseSetting);
    }

    public void OnClickPauseTitle()
    {
        Time.timeScale = 1f;
        nextSceneName = "KYH_UI"; //로딩이 끝나면 이동할 씬 이름
        ChangeState(UIState.Nothing);
        SceneManager.LoadScene("KYH_UI_LoadingScene");
    }

    //PauseSetting내부
    public void OnClickPauseSettingBack()
    {
        ChangeState(UIState.Pause);
    }

    //Game내부

    public void GetDamagedUI() //데미지 받았을 때 ui 처리
    {
        damageIndicator.Flash();
    }
}
