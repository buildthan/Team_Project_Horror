using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class GameManager : MonoBehaviour
{

    static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (null == instance) //싱글톤 선언 과정
            {
                return null;
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null) //싱글톤 선언 과정
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            if (instance != this)
                Destroy(this.gameObject);
        }

    }
}
