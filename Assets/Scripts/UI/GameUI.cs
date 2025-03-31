using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Timeline.Actions.MenuPriority;

/// <summary>
/// 인벤토리와 슬롯 관리
/// </summary>
public class GameUI : BaseUI
{
    public GameObject inventory;
    public GameObject[] inventorySlots;
    public GameObject inventorySlotPrefab;
    public GameObject crosshair;

    public Transform dropPosition;  // Player의 dropPosition을 저장

    [Header("Selected Item")]
    public TextMeshProUGUI selectedItemName;    // 선택아이템의 이름
    public TextMeshProUGUI selectedItemDescription; // 설명
    public TextMeshProUGUI selectedItemStatName;    // 스탯
    public TextMeshProUGUI selectedItemStatValue;   // 값
    /// Button이지만, 활성화 비활성화만 해줄 것이므로 GameObject로 들고 있는 것이 편하다
    public GameObject useButton;    // 사용버튼
    public GameObject equipButton;  // 장착버튼
    public GameObject unEquipButton;    // 해제버튼
    public GameObject dropButton;   // 버리기버튼

    //인벤토리 슬롯 접근용
    public Transform inventoryContent;

    private PlayerController controller;    // 정보를 주고받을 플레이어의 정보(특히 delegate를 가져오기 위함이다)
    private PlayerCondition condition;  // 정보를 주고받을 플레이어의 상태

    //플레이어 정보 명시용
    [Header("Player Condition Indicators")]
    public Image hpIndicator;


    // 선택된 아이템의 정보 저장
    BaseItemDataSO selectedItem;
    int selectedItemIndex = 0;
    // 장착, 해제
    private int curEquipIndex;


    public void Start()
    {
        //// 플레이어와 인벤토리 연결
        //CharacterManager.Instance.Player.controller.inventory += Toggle;

        //dropPosition = CharacterManager.Instance.Player.dropPosition;

        //CharacterManager.Instance.Player.addItem += AddItem;  // delegate에 함수 등록


        inventorySlots = new GameObject[20]; //아이템 슬롯 생성
        //추후에 아이템 데이터를 받아와서 적용하는 것으로 바꿀 것.

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            inventorySlots[i] = Instantiate(inventorySlotPrefab, inventoryContent);
        }
    }
    public void Update()
    {
        /// 플레이어의 Input System과 연결하는 방식으로 수정
        //if(Input.GetKeyDown(KeyCode.Tab) && this.gameObject.activeSelf)
        //{
        //    if (inventory.activeSelf == false)
        //    {
        //        Time.timeScale = 0;
        //        inventory.SetActive(true);
        //    }
        //    else
        //    {
        //        Time.timeScale = 1;
        //        inventory.SetActive(false);
        //    }
        //}
    }

    protected override UIState GetUIState()
    {
        return UIState.Game;
    }

    public override void Init(UIManager uiManager)
    {
        base.Init(uiManager);

    }
    /// <summary>
    /// 플레이어와 인벤토리 연결
    /// </summary>
    public void Toggle()
    {
        if (IsOpen())
        {
            uiManager.PlayUIClickAudio();
            CharacterManager.Instance.Player.controller.ToggleCursor(false);
            inventory.SetActive(false);
            crosshair.SetActive(true);
            Time.timeScale = 1f;
        }
        else
        {
            uiManager.PlayUIClickAudio();
            CharacterManager.Instance.Player.controller.ToggleCursor(true);
            inventory.SetActive(true);
            crosshair.SetActive(false);
            Time.timeScale = 0f;
        }
    }
    public bool IsOpen()
    {
        // activeInHierarchy: 하이러키에 활성화 되어있다면 true 리턴
        return inventory.activeInHierarchy;
    }

    // 인벤토리에 아이템 추가
    public void AddItem()
    {
        BaseItemDataSO data = CharacterManager.Instance.Player.BaseItemData;  // 상호작용 중인 아이템 데이터를 받아온다

        // 중복 가능한 아이템인가? (canStack이 true일 때 가능하다)
        if (data.canStack)
        {
            ItemSlot slot = GetItemStack(data); // 슬롯 가져온다
            // 숫자 올린다(최대 12까지)
            if (slot != null)
            {
                slot.quantity++;

                // UIUpdate
                UpdateUI();

                CharacterManager.Instance.Player.BaseItemData = null;   // 데이터 초기화(일 끝냈으면 비워라)
                return;
            }
        }
        // 아니라면 비어있는 슬롯 가져온다
        ItemSlot emptySlot = GetEmptySlot();

        // 비어있는 슬롯이 있다면
        if (emptySlot != null)
        {
            emptySlot.itemData = data;  // 아이템 추가
            emptySlot.quantity = 1;

            // UIUpdate
            UpdateUI();

            CharacterManager.Instance.Player.BaseItemData = null;   // 일 끝냈으면 비워라
            return;
        }
        // 없다면 파밍한 아이템을 버린다
        ThrowItem(data);
        CharacterManager.Instance.Player.BaseItemData = null;   // 데이터 초기화(일 끝냈으면 비워라)
    }
    // 아이템 버리기
    void ThrowItem(BaseItemDataSO data)
    {
        /// 다시 검색하지 않고, 미리 저장한 프리팹 리소스를 이용하여 인스턴스를 생성
        Instantiate(data.prefab, dropPosition.position, Quaternion.Euler(Vector3.one * UnityEngine.Random.value * 360));
    }


    void UpdateUI()
    {
        // 모든 슬롯을 조사하여, 슬롯에 데이터가 있으면 setting
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            ItemSlot slot = inventorySlots[i].GetComponent<ItemSlot>(); // GameObject에서 ItemSlot 가져오기

            if (slot.itemData != null)
            {
                slot.Set();
            }
            else
            {
                slot.Clear();
            }
        }
    }

    // 중복할 수 있는 아이템이라면 
    ItemSlot GetItemStack(BaseItemDataSO data)
    {
        // 모든 슬롯을 조사하여, data가 있는 슬롯이 있다면 리턴
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            ItemSlot slot = inventorySlots[i].GetComponent<ItemSlot>(); // GameObject에서 ItemSlot 가져오기

            if (slot.itemData == data && slot.quantity < data.maxStackAmount)
            {
                return slot;    // 해당 슬롯을 반환
            }
        }
        return null;
    }
    // 비어있는 슬롯 가져오기
    ItemSlot GetEmptySlot()
    {
        // 모든 슬롯을 조사하여, data가 있는 슬롯이 있다면 리턴
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            ItemSlot slot = inventorySlots[i].GetComponent<ItemSlot>(); // GameObject에서 ItemSlot 가져오기
            if (slot.itemData == null)
            {
                return slot;
            }
        }
        // 모든 슬롯에 데이터가 있다면
        return null;
    }


    // ItemSlot 스크립트 먼저 수정
    // 선택한 아이템 정보창에 업데이트 해주는 함수
    public void SelectItem(int index)
    {
        uiManager.PlayUIClickAudio();

        ItemSlot slot = inventorySlots[index].GetComponent<ItemSlot>(); // GameObject에서 ItemSlot 가져오기

        if (slot.itemData == null) return;  // 슬롯에 아이템이 없다면 리턴

        // 배열에 접근해서 해당 인덱스에 있는 아이템을 가져온다
        selectedItem = slot.itemData;   // selectedItem 변수에 아이템 정보 저장
        selectedItemIndex = index;

        selectedItemName.text = selectedItem.name;
        selectedItemDescription.text = selectedItem.description;

        // text에 스탯을 넣어야하는데, 모든 아이템에 스탯이 있는 것이 아니므로 일단 비운다
        selectedItemStatName.text = string.Empty;
        selectedItemStatValue.text = string.Empty;


        /// 자료형을 비교하는 방법을 연습
        /// BaseItemDataSO로 저장이 되어있지만, 이건 업캐스팅으로 실제 저장된 것은
        /// FoodDataSO, WeaponDataSO, BulletDataSO 중 하나
        /// 해당 자료형인 아이템인 경우에 
        useButton.SetActive(selectedItem.GetType() == typeof(FoodDataSO));  // 선택한 아이템의 type이 consumable일때 사용버튼 활성화
        equipButton.SetActive(selectedItem.GetType() == typeof(WeaponDataSO) && !slot.equipped);   /// 선택한 아이템의 type이 Equipable이고, 장착하지 않았다면, 장착버튼 활성화
        unEquipButton.SetActive(selectedItem.GetType() == typeof(WeaponDataSO) && slot.equipped);  /// 선택한 아이템의 type이 Equipable이고, 장착했다면, 해제버튼 활성화
        dropButton.SetActive(true); // 버리기 버튼은 활성화
    }

    // 버튼 이벤트 함수: 사용하기
    //public void OnUseButton()
    //{
    //    // 아이템 type이 consumable일 때만 가능하다
    //    if (selectedItem.GetType() == typeof(FoodDataSO))
    //    {
    //        for (int i = 0; i < selectedItem.consumables.Length; i++)
    //        {
    //            switch (selectedItem.consumables[i].type)
    //            {
    //                case ConsumableType.Health:
    //                    condition.Heal(selectedItem.consumables[i].value);
    //                    break;
    //                case ConsumableType.Hunger:
    //                    condition.Eat(selectedItem.consumables[i].value);
    //                    break;
    //            }
    //        }
    //        RemoveSelctedItem();
    //    }
    //}

    // 버튼 이벤트 함수: 장착
    public void OnEquipButton()
    {
        ItemSlot slot = inventorySlots[curEquipIndex].GetComponent<ItemSlot>(); // GameObject에서 ItemSlot 가져오기

        if (slot.equipped)
        {
            // UnEquip
            UnEquip(curEquipIndex);
        }

        slot.equipped = true;   // 장착
        curEquipIndex = selectedItemIndex;
        //CharacterManager.Instance.Player.equip.EquipNew(selectedItem);
        UpdateUI();

        SelectItem(selectedItemIndex);
    }
    void UnEquip(int index)
    {
        ItemSlot slot = inventorySlots[index].GetComponent<ItemSlot>(); // GameObject에서 ItemSlot 가져오기

        slot.equipped = false;
        //CharacterManager.Instance.Player.equip.UnEquip();
        UpdateUI();

        if (selectedItemIndex == index)
        {
            SelectItem(selectedItemIndex);
        }
    }
    // 버튼 이벤트 함수: 해제
    public void OnUpEquipButton()
    {
        UnEquip(selectedItemIndex);
    }


    //플레이어 HP 정보 업데이트용
    public void UpdatePlayerHpIndicator(float curhp, float maxHp) //플레이어의 현재 체력과 최대 체력을 받아옴
    {
        hpIndicator.color = new Color(1- curhp / maxHp, curhp/maxHp,0);
    }
    
}
