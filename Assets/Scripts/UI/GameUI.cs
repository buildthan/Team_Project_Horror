using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;
using UnityEngine.UIElements;

public class GameUI : BaseUI
{
    public BaseItem testItem;


    public GameObject inventory;
    public GameObject[] inventorySlots;
    public GameObject inventorySlotPrefab;


    private ItemSlot selectedItem;
    private int selectedItemIndex;
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDescription;


    protected override UIState GetUIState()
    {
        return UIState.Game;
    }

    public override void Init(UIManager uiManager)
    {
        base.Init(uiManager);

    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab) && this.gameObject.activeSelf)
        {
            if (inventory.activeSelf == false)
            {
                Time.timeScale = 0;
                inventory.SetActive(true);
            }
            else
            {
                Time.timeScale = 1;
                inventory.SetActive(false);
            }
        }
    }

    public void Start()
    {
        inventorySlots = new GameObject[20]; //아이템 슬롯 생성
        //추후에 아이템 데이터를 받아와서 적용하는 것으로 바꿀 것.

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            inventorySlots[i] = Instantiate(inventorySlotPrefab, this.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0));
            inventorySlots[i].GetComponent<ItemSlot>().index = i;
            inventorySlots[i].GetComponent<ItemSlot>().inventory = this;
            inventorySlots[i].GetComponent<ItemSlot>().Clear();
        }

        ClearSelectedItemWindow();

        //test용 명령어
        AddItem();
    }

    void ClearSelectedItemWindow()
    {
        selectedItem = null;

        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;
    }


    public void UpdateUI()
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i].GetComponent<ItemSlot>().item != null)
            {
                inventorySlots[i].GetComponent<ItemSlot>().Set();
            }
            else
            {
                inventorySlots[i].GetComponent<ItemSlot>().Clear();
            }
        }
    }

    public void AddItem()
    {
        

        if (testItem.baseItemData.canStack)
        {
            ItemSlot slot = GetItemStack(testItem);
            if (slot != null)
            {
                slot.quantity++;
                UpdateUI();
                //CharacterManager.Instance.Player.itemData = null;
                return;
            }
        }

        ItemSlot emptySlot = GetEmptySlot();

        if (emptySlot != null)
        {
            emptySlot.item = testItem;
            emptySlot.quantity = 1;
            UpdateUI();
            //CharacterManager.Instance.Player.itemData = null;
            return;
        }

        //ThrowItem(data);
        //CharacterManager.Instance.Player.itemData = null;
    }

    ItemSlot GetItemStack(BaseItem data)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i].GetComponent<ItemSlot>().item == data && inventorySlots[i].GetComponent<ItemSlot>().quantity < data.baseItemData.maxStackAmount)
            {
                return inventorySlots[i].GetComponent<ItemSlot>();
            }
        }
        return null;
    }

    ItemSlot GetEmptySlot()
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i].GetComponent<ItemSlot>().item == null)
            {
                return inventorySlots[i].GetComponent<ItemSlot>();
            }
        }
        return null;
    }

    // ItemSlot 스크립트 먼저 수정
    public void SelectItem(int index)
    {
        if (inventorySlots[index].GetComponent<ItemSlot>().item == null) return;

        selectedItem = inventorySlots[index].GetComponent<ItemSlot>();
        selectedItemIndex = index;

        selectedItemName.text = selectedItem.item.baseItemData.name;
        selectedItemDescription.text = selectedItem.item.baseItemData.description;
    }
}
