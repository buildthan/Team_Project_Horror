using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.InputSystem.HID;
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
    //public TextMeshProUGUI selectedItemStatName;    // 스탯
    //public TextMeshProUGUI selectedItemStatValue;   // 값
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

    /// <summary>
    /// 리플렉션을 사용하기 위해 itemManager를 연결해야한다
    /// </summary>
    public ItemManager itemManager;

    public BulletManager bulletManager;

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
            inventorySlots[i].GetComponent<ItemSlot>().index = i;
        }
        ClearSelectedItemWindow();
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
    public void AddItem(BaseItem item)
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

                /// 런타임에 자료형을 알기 위해서는 리플렉션을 사용한다
                Type itemType = item.GetType();
                MethodInfo method = typeof(ItemManager).GetMethod("ReturnItemToPool");
                MethodInfo genericMethod = method.MakeGenericMethod(itemType);
                genericMethod.Invoke(itemManager, new object[] { item });                //

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

            /// 런타임에 자료형을 알기 위해서는 리플렉션을 사용한다
            Type itemType = item.GetType();
            MethodInfo method = typeof(ItemManager).GetMethod("ReturnItemToPool");
            MethodInfo genericMethod = method.MakeGenericMethod(itemType);
            genericMethod.Invoke(itemManager, new object[] { item });                //


            // UIUpdate
            UpdateUI();

            CharacterManager.Instance.Player.BaseItemData = null;   // 일 끝냈으면 비워라
            return;
        }
        // 없다면 파밍한 아이템을 버린다
        ThrowItem(data);
        CharacterManager.Instance.Player.BaseItemData = null;   // 데이터 초기화(일 끝냈으면 비워라)
    }
    /// 아이템 버리기(월드에 오브젝트를 생성(실제로는 비활성화한 오브젝트를 밖에다가 놓는 것)하는 점에서 Equip과 로직이 유사하다)
    void ThrowItem(BaseItemDataSO data)
    {
        ItemSlot slot = inventorySlots[selectedItemIndex].GetComponent<ItemSlot>(); // GameObject에서 ItemSlot 가져오기
        selectedItem = slot.itemData;   // selectedItem 변수에 아이템 정보 저장

        // itemParentTr의 자식 중에서 해당 아이템과 일치하는 오브젝트 찾기
        Transform itemToThrow = null;
        foreach (Transform child in itemManager.itemParentTr)
        {
            string childName = child.gameObject.name.Replace("(Clone)", "").Trim();
            string prefabName = selectedItem.prefab.name.Replace("(Clone)", "").Trim();

            if (childName == prefabName)
            {
                itemToThrow = child;
                break;
            }
        }
        if (itemToThrow != null)
        {
            // 아이템을 게임 화면으로 이동
            itemToThrow.position = dropPosition.position;
            itemToThrow.rotation = Quaternion.Euler(Vector3.one * UnityEngine.Random.value * 360);
            itemToThrow.gameObject.SetActive(true);

            // itemPool에서 제거
            // (Clone)까지 저장되므로, (Clone)까지 같이 비교해야한다
            itemManager.RemoveItem(itemToThrow.GetComponent<BaseItem>());
            Debug.Log($"아이템 {itemToThrow.gameObject.name}을(를) 버렸습니다.");
        }
        else
        {
            Debug.LogWarning("버릴 아이템을 찾을 수 없습니다.");
        }
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

    #region 구현 완료
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

        if (slot.itemData == null)
        {
            // 비어있는 슬롯을 선택하면 모든 버튼 비활성화
            //useButton.SetActive(false);
            //equipButton.SetActive(false);
            //unEquipButton.SetActive(false);
            //dropButton.SetActive(false);

            //selectedItemName.text = "";
            //selectedItemDescription.text = "";

            ClearSelectedItemWindow();
            return;  // 슬롯에 아이템이 없다면 리턴
        }
        // 배열에 접근해서 해당 인덱스에 있는 아이템을 가져온다
        selectedItem = slot.itemData;   // selectedItem 변수에 아이템 정보 저장
        selectedItemIndex = index;  /// 선택한 인덱스


        selectedItemName.text = selectedItem.name;
        selectedItemDescription.text = selectedItem.description;

        /// 자료형을 비교하는 방법을 연습
        /// BaseItemDataSO로 저장이 되어있지만, 이건 업캐스팅으로 실제 저장된 것은
        /// FoodDataSO, WeaponDataSO, BulletDataSO 중 하나
        /// GetType은 정확한 자료형 비교만 가능하다
        //useButton.SetActive(selectedItem.GetType() == typeof(FoodDataSO));  // 선택한 아이템의 type이 FoodDataSO일 때 사용버튼 활성화
        //equipButton.SetActive(selectedItem.GetType() == typeof(RangedWeaponDataSO) && !slot.equipped);   /// 선택한 아이템의 type이 RangedWeaponDataSO이고, 장착하지 않았다면, 장착버튼 활성화
        //unEquipButton.SetActive(selectedItem.GetType() == typeof(RangedWeaponDataSO) && slot.equipped);  /// 선택한 아이템의 type이 RangedWeaponDataSO이고, 장착했다면, 해제버튼 활성화

        /// 이렇게 하면 부모로 바꿀 수 있는 경우에도 true다
        useButton.SetActive(selectedItem is FoodDataSO);
        equipButton.SetActive(selectedItem is WeaponDataSO && !slot.equipped);
        unEquipButton.SetActive(selectedItem is WeaponDataSO && slot.equipped);
        /// 해제버튼이 활성화되어있다는건 아이템을 장착하고있다는 말하고 같다
        dropButton.SetActive(!(selectedItem is WeaponDataSO && slot.equipped)); // 무기 장착시 버리기 버튼 비활성화
    }

    // 버튼 이벤트 함수: 사용하기
    public void OnUseButton()
    {
        // 아이템 type이 consumable일 때만 가능하다
        if (selectedItem is FoodDataSO)
        {
            // FoodDataSO에 실제로 저장된 것이 HealincgFood이든, CureFoodDataSO이든
            // UseItem을 호출하면, 내부에서 알아서 다른 동작을 한다(다형성)
            // 하지만 FoodDataSO는 멤버변수이고 이걸로 Food 클래스를 가져와서 UseItem을 호출해야한다
            //... 제네릭이 아니니까 리플렉션도 할 수없다
            // FoodDataSO에 있는 실제 자료형 HealincgFood이든, CureFoodDataSO               

            // 그런데.. scriptable object가 프리팹을 변수로 가지고 있다.
            // 이걸 가져와서 어떻게 하면 될거같은데
            GameObject prefab = selectedItem.prefab;
            if (prefab != null)
            {
                // 프리팹에서 BaseItem을 상속받는 컴포넌트 가져오기
                var baseItemComponent = prefab.GetComponent<BaseItem>();

                if (baseItemComponent != null)
                {
                    // HealingFood 또는 CureFood로 형변환하여 UseItem 호출
                    if (baseItemComponent is HealingFood healingFood)
                    {
                        Debug.Log("HealingFood로 형변환 성공");
                        healingFood.UseItem();
                    }
                    else if (baseItemComponent is CureFood cureFood)
                    {
                        Debug.Log("CureFood로 형변환 성공");
                        cureFood.UseItem();
                    }
                    else
                    {
                        Debug.LogWarning("프리팹에 HealingFood 또는 CureFood가 없습니다.");
                    }
                    /// 사용한 아이템 삭제
                    /// 아이템 매니저에서 itemPool에서 현재 아이템을 검색하고 Remove                
                    /// itemParentTr에서 검색하고, 삭제한다

                    // itemParentTr의 자식 오브젝트들 중에서 selectedItem.prefab과 동일한 프리팹을 검색하여 삭제
                    foreach (Transform child in itemManager.itemParentTr)
                    {
                        //if (child.gameObject.name == selectedItem.prefab.name) // 이름으로 비교하여 동일한 프리팹 확인
                        /// 이름에 Sniper(Clone)처럼 있으면 (Clone)은 이름에서 제거해야한다
                        // (Clone) 제거 후 비교
                        string childName = child.gameObject.name.Replace("(Clone)", "").Trim();
                        string prefabName = selectedItem.prefab.name.Replace("(Clone)", "").Trim();

                        if (childName == prefabName) // 이름으로 비교하여 동일한 프리팹 확인
                        {
                            Debug.Log($"아이템 {child.gameObject.name} 삭제");
                            Destroy(child.gameObject); // 해당 오브젝트 삭제
                            break; // 삭제했으므로 루프 종료
                        }
                    }
                    // ItemManager를 통해 아이템을 완전히 제거
                    // ItemManager를 통해 아이템 제거
                    itemManager.RemoveItem(baseItemComponent);
                }
                // 아이템 슬롯에서 데이터 제거
                RemoveSelctedItem();
            }
        }

        void RemoveSelctedItem()
        {
            // UI 업데이트를 위해 정보를 갱신
            ItemSlot slot = inventorySlots[selectedItemIndex].GetComponent<ItemSlot>(); // GameObject에서 ItemSlot 가져오기

            slot.quantity--;
            if (slot.quantity <= 0)
            {
                selectedItem = null;
                slot.itemData = null;   // 슬롯에서도 아이템 제거해라
                selectedItemIndex = -1;
                ClearSelectedItemWindow();
            }
            UpdateUI(); // UI 업데이트
        }
        // 아이템을 클릭하면 표시되는 정보 초기화
        void ClearSelectedItemWindow()
        {
            selectedItemName.text = string.Empty;
            selectedItemDescription.text = string.Empty;
            //selectedItemStatName.text = string.Empty;
            //selectedItemStatValue.text = string.Empty;

            useButton.SetActive(false);
            equipButton.SetActive(false);
            unEquipButton.SetActive(false);
            dropButton.SetActive(false);
        }
    }

    // 버튼 이벤트 함수: 버리기
    public void OnDropButton()
    {
        ThrowItem(selectedItem);   // 선택된 아이템 버린다
        RemoveSelctedItem();
    }
    // 아이템을 버린 다음에도 UI 업데이트는 해야한다
    void RemoveSelctedItem()
    {
        // UI 업데이트를 위해 정보를 갱신
            ItemSlot slot = inventorySlots[selectedItemIndex].GetComponent<ItemSlot>(); // GameObject에서 ItemSlot 가져오기

        // UI 업데이트를 위해 정보를 갱신
        slot.quantity--;
        if (slot.quantity <= 0)
        {
            selectedItem = null;
            slot.itemData = null;   // 슬롯에서도 아이템 제거해라
            selectedItemIndex = -1;
            ClearSelectedItemWindow();
        }
        UpdateUI(); // UI 업데이트
    }

    // 아이템을 클릭하면 표시되는 정보 초기화
    void ClearSelectedItemWindow()
    {
        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;
        //selectedItemStatName.text = string.Empty;
        //selectedItemStatValue.text = string.Empty;

        useButton.SetActive(false);
        equipButton.SetActive(false);
        unEquipButton.SetActive(false);
        dropButton.SetActive(false);
    }
    #endregion

    #region 장착
    // 버튼 이벤트 함수: 장착
    //
    public void OnEquipButton()
    {
        uiManager.PlayUIClickAudio();

        // selectedItemIndex의 인덱스를 갱신해야한다
        // 마지막에 선택한 인덱스가 남아있다
        ItemSlot slot = inventorySlots[selectedItemIndex].GetComponent<ItemSlot>(); // GameObject에서 ItemSlot 가져오기

        // 선택한 슬롯의 무기가 이미 장착된 상태인지 확인
        if (slot.equipped)
        {
            // 이미 장착된 경우 아무 작업도 하지 않음
            Debug.Log("이미 장착된 아이템입니다.");
            return;
        }

        // 현재 장착 중인 무기를 해제
        if (curEquipIndex >= 0) // 현재 장착 중인 아이템이 있는 경우만 실행
        {
            UnEquip(curEquipIndex);
        }

        // 새로운 무기 장착
        slot.equipped = true;   // 장착
        curEquipIndex = selectedItemIndex;
        // slot.itemData가 가지고 있는 프리팹을 매개변수로 보내야지
        GameObject prefab = slot.itemData.prefab;

        if (prefab != null)
        {
            // 프리팹에서 BaseItem을 상속받는 컴포넌트 가져오기
            var baseItemComponent = prefab.GetComponent<BaseItem>();
            {
                if (baseItemComponent != null)
                {
                    string prefabName = selectedItem.prefab.name.Replace("(Clone)", "").Trim();
                    // itemParentTr의 자식 오브젝트들 중에서 selectedItem.prefab과 동일한 프리팹을 검색하여 삭제
                    foreach (Transform child in itemManager.itemParentTr)
                    {
                        /// 이름에 Sniper(Clone)처럼 있으면 (Clone)은 이름에서 제거해야한다
                        // (Clone) 제거 후 비교
                        string childName = child.gameObject.name.Replace("(Clone)", "").Trim();

                        if (childName == prefabName) // 이름으로 비교하여 동일한 프리팹 확인
                        {
                            itemManager.EquipItem(baseItemComponent);
                        }
                    }
                }
            }
        }
        UpdateUI();
        SelectItem(selectedItemIndex);
    }
    #endregion
    #region 해제
    void UnEquip(int index)
    {
        // 시작할때부터 장착하지 않았다는 전제 하에 
        // 이전에 장착할때 index가 이미 정해져 있다
        // OnEquipButton에서 curEquipIndex를 갱신한 뒤 바꾸지 않았으므로 장착한 무기의 인덱스 그대로다
        ItemSlot slot = inventorySlots[index].GetComponent<ItemSlot>(); // GameObject에서 ItemSlot 가져오기

        slot.equipped = false;  // 해제

        // 기존의 무기를 해제했다면 위치는 그냥 놔두고 비활성화
        // 만약 버리는 경우에만 부모를 CharacterManager.Instance.Player.dropPosition으로 바꾸고 버리면 된다
        // 하지만, 지금은 버리는게 아니라 무기를 해제할 뿐이므로 비활성화만 한다
        /// 아이템 매니저의 itemPool에서 무기를 가져와서 비활성화
        /// 이미 장착하고 있는 무기(itemManager.equippedItems에서 검색)를 비활성화

        /// 아래의 방법으로 itemToUnEquip이 null인데 디버그가 안된다...
        //BaseItem itemToUnEquip = itemManager.equippedItems.FirstOrDefault(item => item.name == slot.itemData.name);
        //if (itemToUnEquip != null)
        //{
        //    // 아이템 비활성화
        //    itemManager.UnEquipItem(itemToUnEquip);
        //}
        //else
        //{
        //    Debug.LogWarning("장착된 아이템을 찾을 수 없습니다.");
        //}


        // 장착된 아이템을 직접 검색하여 비활성화
        BaseItem itemToUnEquip = null;
        string prefabName = slot.itemData.prefab.name.Replace("(Clone)", "").Trim();
        foreach (BaseItem equippedItem in itemManager.equippedItems)
        {
            string childName = equippedItem.gameObject.name.Replace("(Clone)", "").Trim();

            if (equippedItem != null && childName == prefabName)
            {
                itemToUnEquip = equippedItem;
                break; // 찾았으면 반복 종료
            }
        }
        if (itemToUnEquip != null)
        {
            // 아이템 비활성화
            itemManager.UnEquipItem(itemToUnEquip);
        }
        else
        {
            Debug.LogWarning("장착된 아이템을 찾을 수 없습니다.");
        }



        /// icon을 swap하지 않는다. 각자 icon은 각자의 슬롯에 있다.        

        UpdateUI();

        if (selectedItemIndex == index)
        {
            SelectItem(selectedItemIndex);
        }
    }

    // 버튼 이벤트 함수: 해제
    public void OnUpEquipButton()
    {
        // selectedItemIndex를 갱신해야한다
        // 단순 해제만 한다는 것은, 이미 무기가 장착되어있는 인덱스를 가져온다는 뜻이다
        // curEquipIndex
        uiManager.PlayUIClickAudio();
        selectedItemIndex = curEquipIndex;  // 현재 선택한 인덱스이므로 갱신(아마 되어있겠지만)

        UnEquip(selectedItemIndex);

        curEquipIndex = -1; // 장착하는 무기가 없으니까
    }
    #endregion

    //플레이어 HP 정보 업데이트용
    public void UpdatePlayerHpIndicator(float curhp, float maxHp) //플레이어의 현재 체력과 최대 체력을 받아옴
    {
        hpIndicator.color = new Color(1 - curhp / maxHp, curhp / maxHp, 0);
    }

    // 게임 재시작할 경우 인벤토리 비우기
    public void ClearInventory()
    {
        // 모든 슬롯을 조사하여, 슬롯에 데이터가 있으면 비운다
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            ItemSlot slot = inventorySlots[i].GetComponent<ItemSlot>(); // GameObject에서 ItemSlot 가져오기

            if (slot.itemData != null)
                slot.Clear();
        }
    }
}
