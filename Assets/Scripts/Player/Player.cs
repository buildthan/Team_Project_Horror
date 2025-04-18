using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController controller;
    public PlayerCondition condition { get; private set; }

    // 무기 currentWeapon를 연결하기 위해 Player에 선언했다
    public Interaction interaction;

    // 상호작용 관련
    public Action<BaseItem> addItem;  // 아이템 상호작용을 할 때 호출할 함수를 저장할 delegate
                           
    // 아이템 정보 표시
    public BaseItemDataSO BaseItemData;   // 현재 Interaction 중인 아이템 정보

    public Transform dropPosition;  // 드랍할 위치

    public Transform weaponPosition;    // 무기 장착 위치: 기존에는 오른팔 -> GunHolder로 수정했다
    

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerCondition>();
        interaction = GetComponent<Interaction>();

        addItem += UIManager.Instance.gameUI.AddItem;  // delegate에 함수 등록

        /// 처음 씬에는 플레이어가 없기 때문에 Null이 생기는 문제.
        CharacterManager.Instance.Player = this;
        UIManager.Instance.gameUI.dropPosition = dropPosition;
    }

    //private void Start()
    //{
    //    controller = GetComponent<PlayerController>();
    //    condition = GetComponent<PlayerCondition>();

    //    ///// 처음 씬에는 플레이어가 없기 때문에 Null이 생기는 문제.
    //    CharacterManager.Instance.Player = this;

    //}
}
