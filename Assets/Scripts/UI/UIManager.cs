using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UIState
{
    Title,
    Game
}

public class UIManager : MonoBehaviour
{

    UIState currentState = UIState.Title;

    TitleUI titleUI = null;
    GameUI gameUI = null;

    static UIManager instance;

    public static UIManager Instance //�̱��� ����
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

    private void Awake() //�̱��� ����
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


        ChangeState(UIState.Title);
    }

    public void ChangeState(UIState state) //UI������Ʈ�� on off ���ִ� ���
    {
        currentState = state; //�Ʒ����� �ش��ϴ� UI������Ʈ�� ã�� on off ����
        titleUI?.SetActive(currentState);
        gameUI?.SetActive(currentState);
    }
}
